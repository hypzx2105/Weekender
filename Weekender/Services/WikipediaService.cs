using System.Globalization;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using Weekender.ApiModels;
using Weekender.Helpers;
using Weekender.Models;

namespace Weekender.Services
{
    //Web service provider 2: Wikipedia (free, no API key).
    //REST API page summary: description and photo of the city  https://en.wikipedia.org/api/rest_v1/
    //Action API geosearch: articles about places near the city   https://www.mediawiki.org/wiki/API:Geosearch
    public class WikipediaService : IWikipediaService
    {
        public static readonly string WIKIPEDIA_URL = "https://en.wikipedia.org/";
        
        public static readonly string USER_AGENT = "Weekender/1.0 (https://github.com/hypzx2105/Weekender)";
        public static readonly int SEARCH_RADIUS_METRES = 10000;   
        public static readonly int MAX_LANDMARKS = 8;
        public static readonly double MAX_SUMMARY_DISTANCE_KM = 50;
        private static readonly string SERVICE_NAME = "Wikipedia";

        
        private static readonly string[] EXCLUDED_WORDS =
        {
            "station", "metro", "railway", "tram stop", "school", "college", "university", "hospital",
            "street", "avenue", "parish", "municipality", "borough", "constituency",
            "company", "hotel", "embassy", "airport", "office", "headquarters"
        };

       
        private readonly RestClient client;

        
        public WikipediaService()
        {
            RestClientOptions options = new RestClientOptions(WIKIPEDIA_URL);
            options.UserAgent = USER_AGENT;
            client = new RestClient(options);
        }

       
        public async Task<PlaceSummary?> GetCitySummaryAsync(Destination destination)
        {
            PlaceSummary? summary = await RequestSummaryAsync(destination.Name, destination);

            if (summary == null && !string.IsNullOrWhiteSpace(destination.Country))
            {
               
                summary = await RequestSummaryAsync($"{destination.Name}, {destination.Country}", destination);
            }

            return summary;
        }

        private async Task<PlaceSummary?> RequestSummaryAsync(string title, Destination destination)
        {
            
            var request = new RestRequest("api/rest_v1/page/summary/{title}");
            request.AddUrlSegment("title", title.Replace(' ', '_'));

            RestResponse response = await client.ExecuteAsync(request);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
               
                return null;
            }
            if (!response.IsSuccessful)
            {
                throw new ApiException(SERVICE_NAME, $"Could not get information about {destination.Name}. {response.ErrorMessage}",
                    (int)response.StatusCode, response.ErrorException);
            }

            return ParseSummary(response.Content, destination);
        }

        public async Task<List<Landmark>> GetNearbyLandmarksAsync(Destination destination)
        {
            string coordinates = destination.Latitude.ToString(CultureInfo.InvariantCulture) + "|"
                + destination.Longitude.ToString(CultureInfo.InvariantCulture);

            var request = new RestRequest("w/api.php");
            request.AddParameter("action", "query");
            request.AddParameter("format", "json");
            request.AddParameter("formatversion", "2");
            request.AddParameter("generator", "geosearch");
            request.AddParameter("ggscoord", coordinates);
            request.AddParameter("ggsradius", SEARCH_RADIUS_METRES.ToString());
            request.AddParameter("ggslimit", "50");
            request.AddParameter("prop", "pageimages|description|coordinates");
            request.AddParameter("piprop", "thumbnail");
            request.AddParameter("pithumbsize", "480");
            request.AddParameter("pilimit", "50");
            request.AddParameter("colimit", "50");   

            RestResponse response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new ApiException(SERVICE_NAME, $"Could not get places to visit near {destination.Name}. {response.ErrorMessage}",
                    (int)response.StatusCode, response.ErrorException);
            }

            return ParseLandmarks(response.Content, destination);
        }

        
        public static PlaceSummary? ParseSummary(string? json, Destination destination)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            WikipediaSummaryResponse? summary;
            try
            {
                summary = JsonConvert.DeserializeObject<WikipediaSummaryResponse>(json);
            }
            catch (JsonException ex)
            {
                throw new ApiException(SERVICE_NAME, "Wikipedia returned data in an unexpected format.", 0, ex);
            }

            if (summary == null || string.IsNullOrWhiteSpace(summary.Extract) || summary.Type == "disambiguation")
            {
                
                return null;
            }

            if (summary.Coordinates != null)
            {
                
                double distance = GeoHelper.DistanceKm(destination.Latitude, destination.Longitude,
                    summary.Coordinates.Lat, summary.Coordinates.Lon);
                if (distance > MAX_SUMMARY_DISTANCE_KM)
                {
                    return null;
                }
            }

           
            string imageUrl = summary.OriginalImage?.Source ?? summary.Thumbnail?.Source ?? string.Empty;
            string pageUrl = summary.ContentUrls?.Desktop?.Page ?? string.Empty;

            return new PlaceSummary(summary.Title ?? destination.Name, summary.Description ?? string.Empty,
                summary.Extract, imageUrl, pageUrl);
        }

        public static List<Landmark> ParseLandmarks(string? json, Destination destination)
        {
            List<Landmark> landmarks = new List<Landmark>();

            if (string.IsNullOrWhiteSpace(json))
            {
                return landmarks;
            }

            WikipediaQueryResponse? queryResponse;
            try
            {
                queryResponse = JsonConvert.DeserializeObject<WikipediaQueryResponse>(json);
            }
            catch (JsonException ex)
            {
                throw new ApiException(SERVICE_NAME, "Wikipedia returned data in an unexpected format.", 0, ex);
            }

            if (queryResponse?.Query?.Pages == null)
            {
                return landmarks;
            }

            foreach (WikipediaPage page in queryResponse.Query.Pages)
            {
                
                if (page.Thumbnail?.Source == null || string.IsNullOrWhiteSpace(page.Description)
                    || page.Coordinates == null || page.Coordinates.Count == 0)
                {
                    continue;
                }

                
                if (page.Title.Equals(destination.Name, StringComparison.OrdinalIgnoreCase) || IsExcluded(page.Description))
                {
                    continue;
                }

                WikipediaCoordinate position = page.Coordinates[0];
                double distance = GeoHelper.DistanceKm(destination.Latitude, destination.Longitude, position.Lat, position.Lon);
                Landmark landmark = new Landmark(page.Title, page.Description, page.Thumbnail.Source,
                    position.Lat, position.Lon, distance);

               
                if (!landmarks.Contains(landmark))
                {
                    landmarks.Add(landmark);
                }
            }

            
            landmarks.Sort();

            return landmarks.Take(MAX_LANDMARKS).ToList();
        }

        public static bool IsExcluded(string description)
        {
            foreach (string word in EXCLUDED_WORDS)
            {
                if (description.Contains(word, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}