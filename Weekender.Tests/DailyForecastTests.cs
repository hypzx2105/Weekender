using Weekender.Models;

namespace Weekender.Tests
{
    public class DailyForecastTests
    {
        [Theory]
        [InlineData(0, WeatherCondition.Clear)]
        [InlineData(2, WeatherCondition.PartlyCloudy)]
        [InlineData(3, WeatherCondition.Cloudy)]
        [InlineData(45, WeatherCondition.Fog)]
        [InlineData(53, WeatherCondition.Drizzle)]
        [InlineData(63, WeatherCondition.Rain)]
        [InlineData(81, WeatherCondition.Rain)]      
        [InlineData(75, WeatherCondition.Snow)]
        [InlineData(95, WeatherCondition.Thunderstorm)]
        [InlineData(42, WeatherCondition.Unknown)]   
        public void GetCondition_GroupsWmoCodesCorrectly(int weatherCode, WeatherCondition expected)
        {
            WeatherCondition result = DailyForecast.GetCondition(weatherCode);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void DescriptionAndIcon_MatchTheWeatherCode()
        {
            DailyForecast rainyDay = new DailyForecast(new DateTime(2026, 10, 17), 61, 19.4, 14.8, 80);

            Assert.Equal(WeatherCondition.Rain, rainyDay.Condition);
            Assert.Equal("Rain", rainyDay.Description);
            Assert.Equal("bi-cloud-rain", rainyDay.IconClass);
        }

        [Fact]
        public void GetDescription_HasATextForEveryCondition()
        {
            
            foreach (WeatherCondition condition in Enum.GetValues<WeatherCondition>())
            {
                string description = DailyForecast.GetDescription(condition);

                Assert.False(string.IsNullOrEmpty(description));
                if (condition != WeatherCondition.Unknown)
                {
                    Assert.NotEqual("Unknown", description);
                }
            }
        }

        [Theory]
        [InlineData(150, 100)]
        [InlineData(-5, 0)]
        [InlineData(40, 40)]
        public void RainChance_IsAlwaysBetween0And100(int rainChance, int expected)
        {
            DailyForecast forecast = new DailyForecast(new DateTime(2026, 10, 17), 3, 20, 12, rainChance);

            Assert.Equal(expected, forecast.RainChance);
        }
    }
}