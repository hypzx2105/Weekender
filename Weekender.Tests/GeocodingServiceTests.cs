using Weekender.Models;
using Weekender.Services;

namespace Weekender.Tests
{
    //Tests for the Open-Meteo geocoding service. They use sample JSON based on a real API response,
    //so they test parsing code without depending on the internet.
    //Sample based on a real response from https://geocoding-api.open-meteo.com/v1/search?name=Paris

    public class GeocodingServiceTests
    {
        private const string SAMPLE_JSON = """
            {
              "results": [
                { "id": 4717560, "name": "Paris", "latitude": 33.66094, "longitude": -95.55551,
                  "country_code": "US", "timezone": "America/Chicago", "population": 24782,
                  "country": "United States", "admin1": "Texas" },
                { "id": 2988507, "name": "Paris", "latitude": 48.85341, "longitude": 2.3488,
                  "country_code": "FR", "timezone": "Europe/Paris", "population": 2138551,
                  "country": "France", "admin1": "Île-de-France" }
              ],
              "generationtime_ms": 0.7
            }
            """;

        [Fact]
        public void ParseResponse_MapsJsonFieldsToDestination()
        {
            List<Destination> destinations = GeocodingService.ParseResponse(SAMPLE_JSON);
            Destination parisFrance = destinations[0];

            Assert.Equal("Paris", parisFrance.Name);
            Assert.Equal("France", parisFrance.Country);
            Assert.Equal("FR", parisFrance.CountryCode);
            Assert.Equal("Île-de-France", parisFrance.Region);
            Assert.Equal("Europe/Paris", parisFrance.Timezone);
            Assert.Equal(2138551, parisFrance.Population);
        }

        [Fact]
        public void ParseResponse_OrdersByPopulation_BiggestFirst()
        {
            List<Destination> destinations = GeocodingService.ParseResponse(SAMPLE_JSON);

            Assert.Equal("FR", destinations[0].CountryCode);
            Assert.Equal("US", destinations[1].CountryCode);
        }

        [Fact]
        public void ParseResponse_ThrowsApiException_WhenResponseIsNotValidJson()
        {
            string htmlErrorPage = "<html><body>502 Bad Gateway</body></html>";

            Assert.Throws<ApiException>(() => GeocodingService.ParseResponse(htmlErrorPage));
        }
    }
}