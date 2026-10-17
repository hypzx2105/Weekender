using Weekender.Helpers;
using Weekender.Models;

namespace Weekender.Tests
{
    public class PackingAdvisorTests
    {
       
        private static DailyForecast Day(int weatherCode, double max, double min, int rainChance)
        {
            return new DailyForecast(new DateTime(2026, 10, 17), weatherCode, max, min, rainChance);
        }

        [Fact]
        public void GetPackingTips_ReturnsNoTips_WhenThereIsNoForecast()
        {
            List<string> tips = PackingAdvisor.GetPackingTips(new List<DailyForecast>());

            Assert.Empty(tips);
        }

        [Fact]
        public void GetPackingTips_OnlySuggestsShoes_ForMildDryWeather()
        {
            List<DailyForecast> forecasts = new List<DailyForecast> { Day(1, 20, 12, 10), Day(3, 19, 11, 20) };

            List<string> tips = PackingAdvisor.GetPackingTips(forecasts);

            Assert.Single(tips);
            Assert.Equal("Comfortable walking shoes", tips[0]);
        }

        [Theory]
        [InlineData(50, true)]    
        [InlineData(49, false)]
        public void GetPackingTips_SuggestsUmbrella_OnlyWhenRainIsLikely(int rainChance, bool expectUmbrella)
        {
            List<DailyForecast> forecasts = new List<DailyForecast> { Day(3, 20, 12, 10), Day(61, 18, 12, rainChance) };

            List<string> tips = PackingAdvisor.GetPackingTips(forecasts);

            Assert.Equal(expectUmbrella, tips.Any(tip => tip.StartsWith("Umbrella")));
        }

        [Fact]
        public void GetPackingTips_SuggestsWarmCoat_WhenNightsAreCold()
        {
            List<DailyForecast> forecasts = new List<DailyForecast> { Day(0, 12, 3, 0) };

            List<string> tips = PackingAdvisor.GetPackingTips(forecasts);

            Assert.Contains("Warm coat - it gets cold at night", tips);
        }

        [Fact]
        public void GetPackingTips_SuggestsSunscreen_WhenItIsHot()
        {
            List<DailyForecast> forecasts = new List<DailyForecast> { Day(0, 31, 22, 0) };

            List<string> tips = PackingAdvisor.GetPackingTips(forecasts);

            Assert.Contains("Sunscreen and sunglasses - it will be hot", tips);
        }

        [Fact]
        public void GetPackingTips_SuggestsBoots_WhenSnowIsExpected()
        {
            List<DailyForecast> forecasts = new List<DailyForecast> { Day(73, 1, -4, 70) };

            List<string> tips = PackingAdvisor.GetPackingTips(forecasts);

            Assert.Contains("Waterproof boots - snow is expected", tips);
        }
    }
}