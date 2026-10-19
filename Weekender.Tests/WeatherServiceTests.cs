using Weekender.Models;
using Weekender.Services;

namespace Weekender.Tests
{
    public class WeatherServiceTests
    {
        //Sample based on a real response from https://api.open-meteo.com/v1/forecast
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
        public async Task GetForecastAsync_ThrowsArgumentException_WhenEndIsBeforeStart()
        {
            WeatherService service = new WeatherService();

            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.GetForecastAsync(38.72, -9.13, new DateTime(2026, 10, 18), new DateTime(2026, 10, 16)));
        }
    }
}