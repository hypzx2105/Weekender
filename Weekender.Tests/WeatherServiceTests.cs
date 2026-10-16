using Weekender.Models;
using Weekender.Services;

namespace Weekender.Tests
{
    
    public class WeatherServiceTests
    {
        private const string SAMPLE_JSON = """
            {
              "latitude": 38.72, "longitude": -9.13, "timezone": "Europe/Lisbon",
              "daily": {
                "time": ["2026-10-16", "2026-10-17", "2026-10-18"],
                "weather_code": [3, 61, 1],
                "temperature_2m_max": [22.1, 19.4, 21.0],
                "temperature_2m_min": [15.2, 14.8, 13.9],
                "precipitation_probability_max": [10, 80, 5]
              }
            }
            """;

        [Fact]
        public void ParseResponse_ReturnsOneForecastPerDay()
        {
            List<DailyForecast> forecasts = WeatherService.ParseResponse(SAMPLE_JSON);

            Assert.Equal(3, forecasts.Count);
        }

        [Fact]
        public void ParseResponse_JoinsTheParallelListsByPosition()
        {
            List<DailyForecast> forecasts = WeatherService.ParseResponse(SAMPLE_JSON);
            DailyForecast saturday = forecasts[1];

            Assert.Equal(new DateTime(2026, 10, 17), saturday.Date);
            Assert.Equal(61, saturday.WeatherCode);
            Assert.Equal(19.4, saturday.MaxTemperature);
            Assert.Equal(14.8, saturday.MinTemperature);
            Assert.Equal(80, saturday.RainChance);
            Assert.Equal(WeatherCondition.Rain, saturday.Condition);
        }

        [Fact]
        public void ParseResponse_SkipsDaysWithMissingTemperatures()
        {
            string json = """
                { "daily": {
                    "time": ["2026-10-16", "2026-10-17"],
                    "weather_code": [3, 61],
                    "temperature_2m_max": [22.1, null],
                    "temperature_2m_min": [15.2, 14.8],
                    "precipitation_probability_max": [10, 80]
                } }
                """;

            List<DailyForecast> forecasts = WeatherService.ParseResponse(json);

            Assert.Single(forecasts);
            Assert.Equal(new DateTime(2026, 10, 16), forecasts[0].Date);
        }

        [Fact]
        public void ParseResponse_UsesZeroRainChance_WhenItIsMissing()
        {
            string json = """
                { "daily": {
                    "time": ["2026-10-16"],
                    "weather_code": [0],
                    "temperature_2m_max": [24.0],
                    "temperature_2m_min": [16.0],
                    "precipitation_probability_max": [null]
                } }
                """;

            List<DailyForecast> forecasts = WeatherService.ParseResponse(json);

            Assert.Equal(0, forecasts[0].RainChance);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("{ }")]   
        public void ParseResponse_ReturnsEmptyList_WhenThereIsNoDailyData(string? json)
        {
            List<DailyForecast> forecasts = WeatherService.ParseResponse(json);

            Assert.Empty(forecasts);
        }

        [Fact]
        public void ParseResponse_ThrowsApiException_WhenResponseIsNotValidJson()
        {
            Assert.Throws<ApiException>(() => WeatherService.ParseResponse("<html>Service Unavailable</html>"));
        }

        [Fact]
        public async Task GetForecastAsync_ThrowsArgumentException_WhenEndIsBeforeStart()
        {
            WeatherService service = new WeatherService();

            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.GetForecastAsync(38.72, -9.13, new DateTime(2026, 10, 18), new DateTime(2026, 10, 16)));
        }
    }
}