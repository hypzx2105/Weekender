using Newtonsoft.Json;
using RestSharp;
using Weekender.ApiModels;
using Weekender.Models;

namespace Weekender.Services
{
    //Web service provider 1: Open-Meteo Geocoding API that turns a city name into coordinates.
    //https://open-meteo.com/en/docs/geocoding-api 
    //RestSharp: https://restsharp.dev
    
    public class GeocodingService : IGeocodingService
    {
        public static readonly string GEOCODING_URL = "https://geocoding-api.open-meteo.com/v1/";
        public static readonly int MAX_RESULTS = 8;
        private static readonly string SERVICE_NAME = "Open-Meteo Geocoding";

       
        private readonly RestClient client;

        
        public GeocodingService()
        {
            
            client = new RestClient(GEOCODING_URL);
        }

        
        public async Task<List<Destination>> SearchCitiesAsync(string cityName)
        {
            if (string.IsNullOrWhiteSpace(cityName) || cityName.Trim().Length < 2)
            {
               
                return new List<Destination>();
            }

            var request = new RestRequest("search");
            request.AddParameter("name", cityName.Trim());
            request.AddParameter("count", MAX_RESULTS.ToString());
            request.AddParameter("language", "en");
            request.AddParameter("format", "json");

            
            RestResponse response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new ApiException(SERVICE_NAME, $"Could not search for '{cityName}'. {response.ErrorMessage}",
                    (int)response.StatusCode, response.ErrorException);
            }

            return ParseResponse(response.Content);
        }

        
        public static List<Destination> ParseResponse(string? json)
        {
            List<Destination> destinations = new List<Destination>();

            if (string.IsNullOrWhiteSpace(json))
            {
                return destinations;
            }

            GeocodingResponse? geocodingResponse;
            try
            {
                geocodingResponse = JsonConvert.DeserializeObject<GeocodingResponse>(json);
            }
            catch (JsonException ex)
            {
                
                throw new ApiException(SERVICE_NAME, "The city search returned data in an unexpected format.", 0, ex);
            }

            if (geocodingResponse == null || geocodingResponse.Results == null)
            {
              
                return destinations;
            }

            
            foreach (GeocodingResult result in geocodingResponse.Results)
            {
                Destination destination = new Destination(result.Name, result.Country ?? "Unknown country",
                    result.CountryCode ?? string.Empty, result.Latitude, result.Longitude);
                destination.Region = result.Admin1 ?? string.Empty;
                destination.Timezone = result.Timezone ?? "auto";
                destination.Population = result.Population ?? 0;

                
                if (!destinations.Contains(destination))
                {
                    destinations.Add(destination);
                }
            }

            
            return destinations.OrderByDescending(d => d.Population).ToList();
        }
    }
}