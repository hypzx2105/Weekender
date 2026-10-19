using Weekender.Models;
using Weekender.Services;

namespace Weekender.Tests
{
    //Tests for the Wikipedia service, using sample JSON shaped like the real API responses
    //so they don't need the internet.
    public class WikipediaServiceTests
    {
        private static readonly Destination LISBON = new Destination("Lisbon", "Portugal", "PT", 38.71667, -9.13333);

        //Sample based on a real response from https://en.wikipedia.org/api/rest_v1/page/summary/Lisbon
        private const string SUMMARY_JSON = """
            {
              "type": "standard",
              "title": "Lisbon",
              "description": "Capital and largest city of Portugal",
              "extract": "Lisbon is the capital and largest city of Portugal.",
              "thumbnail": { "source": "https://upload.wikimedia.org/thumb/320px-Lisbon.jpg", "width": 320, "height": 213 },
              "originalimage": { "source": "https://upload.wikimedia.org/Lisbon.jpg", "width": 4000, "height": 2667 },
              "coordinates": { "lat": 38.71388889, "lon": -9.13944444 },
              "content_urls": { "desktop": { "page": "https://en.wikipedia.org/wiki/Lisbon" } }
            }
            """;

        //Sample JSON based on the documented format of the MediaWiki API (generator=geosearch, formatversion=2)
        //and checked against a real response. Trimmed to the fields I use, plus extra cases to test the filters
        //(a station, a place without a photo and the city's own article).
        //References: https://www.mediawiki.org/wiki/API:Geosearch , https://www.mediawiki.org/wiki/API:Nearby_places_viewer ,  https://www.mediawiki.org/wiki/API:JSON_version_2

        private const string GEOSEARCH_JSON = """
            {
              "batchcomplete": true,
              "query": {
                "pages": [
                  { "pageid": 1, "title": "Belém Tower", "description": "Fortified tower in Lisbon, Portugal",
                    "thumbnail": { "source": "https://upload.wikimedia.org/belem.jpg", "width": 480, "height": 320 },
                    "coordinates": [ { "lat": 38.69157, "lon": -9.21598, "primary": true, "globe": "earth" } ] },
                  { "pageid": 2, "title": "São Jorge Castle", "description": "Castle in Lisbon, Portugal",
                    "thumbnail": { "source": "https://upload.wikimedia.org/castle.jpg", "width": 480, "height": 320 },
                    "coordinates": [ { "lat": 38.71389, "lon": -9.13361, "primary": true, "globe": "earth" } ] },
                  { "pageid": 3, "title": "Lisbon Cathedral", "description": "Roman Catholic cathedral in Lisbon",
                    "thumbnail": { "source": "https://upload.wikimedia.org/cathedral.jpg", "width": 480, "height": 320 },
                    "coordinates": [ { "lat": 38.70997, "lon": -9.13361, "primary": true, "globe": "earth" } ] },
                  { "pageid": 4, "title": "Rossio railway station", "description": "Railway station in Lisbon, Portugal",
                    "thumbnail": { "source": "https://upload.wikimedia.org/rossio.jpg", "width": 480, "height": 320 },
                    "coordinates": [ { "lat": 38.71417, "lon": -9.14083, "primary": true, "globe": "earth" } ] },
                  { "pageid": 5, "title": "Rua Augusta", "description": "Street in Lisbon, Portugal",
                    "coordinates": [ { "lat": 38.71028, "lon": -9.13694, "primary": true, "globe": "earth" } ] },
                  { "pageid": 6, "title": "Lisbon", "description": "Capital and largest city of Portugal",
                    "thumbnail": { "source": "https://upload.wikimedia.org/lisbon.jpg", "width": 480, "height": 320 },
                    "coordinates": [ { "lat": 38.71388889, "lon": -9.13944444, "primary": true, "globe": "earth" } ] }
                ]
              }
            }
            """;

        [Fact]
        public void ParseSummary_MapsJsonFieldsToPlaceSummary()
        {
            PlaceSummary? summary = WikipediaService.ParseSummary(SUMMARY_JSON, LISBON);

            Assert.NotNull(summary);
            Assert.Equal("Lisbon", summary!.Title);
            Assert.Equal("Capital and largest city of Portugal", summary.Description);
            Assert.StartsWith("Lisbon is the capital", summary.Extract);
            Assert.Equal("https://en.wikipedia.org/wiki/Lisbon", summary.PageUrl);
        }

        [Fact]
        public void ParseSummary_ReturnsNull_WhenTheArticleIsAboutAPlaceFarAway()
        {
            string json = """
                { "type": "standard", "title": "Paris", "extract": "Paris is the capital of France.",
                  "coordinates": { "lat": 48.85666667, "lon": 2.35222222 } }
                """;
            Destination parisTexas = new Destination("Paris", "United States", "US", 33.66094, -95.55551);

            Assert.Null(WikipediaService.ParseSummary(json, parisTexas));
        }

        [Fact]
        public void ParseLandmarks_SkipsPlacesWithoutPhoto_StationsAndTheCityItself()
        {
            List<Landmark> landmarks = WikipediaService.ParseLandmarks(GEOSEARCH_JSON, LISBON);

            Assert.Equal(3, landmarks.Count);
            Assert.DoesNotContain(landmarks, landmark => landmark.Title == "Rossio railway station");
            Assert.DoesNotContain(landmarks, landmark => landmark.Title == "Rua Augusta");
            Assert.DoesNotContain(landmarks, landmark => landmark.Title == "Lisbon");
        }

        //Sample response of the MediaWiki Action API with prop=pageimages (one request for several titles),
        //https://en.wikipedia.org/w/api.php?action=query&format=json&formatversion=2&prop=pageimages&piprop=thumbnail&pithumbsize=640&titles=Lisbon|Paris|Budapest
        //Docs: https://www.mediawiki.org/wiki/Extension:PageImages
        //Trimmed to the fields I use. Budapest's photo was removed on purpose to test a city without a photo.

        private const string PHOTOS_JSON = """
                                           {
                                             "batchcomplete": true,
                                             "query": {
                                               "pages": [
                                                 { "pageid": 18091, "title": "Lisbon",
                                                   "thumbnail": { "source": "https://upload.wikimedia.org/thumb/640px-Lisbon.jpg", "width": 640, "height": 427 } },
                                                 { "pageid": 22989, "title": "Paris",
                                                   "thumbnail": { "source": "https://upload.wikimedia.org/thumb/640px-Paris.jpg", "width": 640, "height": 426 } },
                                                 { "pageid": 4061, "title": "Budapest" }
                                               ]
                                             }
                                           }
                                           """;

        [Fact]
        public void ParseCityPhotos_ReturnsOnePhotoPerCity_AndSkipsCitiesWithoutPhoto()
        {
            Dictionary<string, string> photos = WikipediaService.ParseCityPhotos(PHOTOS_JSON);

            Assert.Equal(2, photos.Count);
            Assert.Equal("https://upload.wikimedia.org/thumb/640px-Lisbon.jpg", photos["Lisbon"]);
            Assert.False(photos.ContainsKey("Budapest"));
        }
    }
}