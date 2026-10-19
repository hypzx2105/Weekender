using Weekender.Models;

namespace Weekender.Tests
{
    public class DailyForecastTests
    {
        [Theory]
        [InlineData(0, WeatherCondition.Clear)]
        [InlineData(63, WeatherCondition.Rain)]
        [InlineData(42, WeatherCondition.Unknown)]
        public void GetCondition_GroupsWmoCodesCorrectly(int weatherCode, WeatherCondition expected)
        {
            WeatherCondition result = DailyForecast.GetCondition(weatherCode);

            Assert.Equal(expected, result);
        }
    }
}