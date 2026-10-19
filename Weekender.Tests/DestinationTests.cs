using Weekender.Models;

namespace Weekender.Tests
{
    public class DestinationTests
    {
        [Fact]
        public void Equals_ReturnsTrue_ForTheSamePlace()
        {
            Destination first = new Destination("Paris", "France", "FR", 48.85341, 2.3488);
            Destination second = new Destination("Paris", "France", "FR", 48.85341, 2.3488);

            Assert.True(first.Equals(second));
            Assert.True(first == second);
            Assert.Equal(first.GetHashCode(), second.GetHashCode());
        }

        [Fact]
        public void Equals_ReturnsFalse_ForCitiesWithTheSameNameInDifferentCountries()
        {
            Destination parisFrance = new Destination("Paris", "France", "FR", 48.85341, 2.3488);
            Destination parisTexas = new Destination("Paris", "United States", "US", 33.66094, -95.55551);

            Assert.False(parisFrance.Equals(parisTexas));
            Assert.True(parisFrance != parisTexas);
        }

        [Fact]
        public void ToPlanUrl_ContainsAllTheDestinationDetails()
        {
            Destination lisbon = new Destination("Lisbon", "Portugal", "PT", 38.71667, -9.13333);
            lisbon.Timezone = "Europe/Lisbon";

            string url = lisbon.ToPlanUrl();

            Assert.StartsWith("plan?", url);
            Assert.Contains("name=Lisbon", url);
            Assert.Contains("code=PT", url);
            Assert.Contains("lat=38.71667", url);
            Assert.Contains("lon=-9.13333", url);
            Assert.Contains("tz=Europe%2FLisbon", url);
        }
    }
}