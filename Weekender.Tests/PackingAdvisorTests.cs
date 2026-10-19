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
    }
}