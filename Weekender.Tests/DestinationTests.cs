using System.Globalization;

using Weekender.Models;

namespace Weekender.Tests
{
    public class DestinationTests
    {
        [Fact]
        public void Constructor_SetsAllProperties()
        {
            Destination lisbon = new Destination("Lisbon", "Portugal", "pt", 38.71667, -9.13333);

            Assert.Equal("Lisbon", lisbon.Name);
            Assert.Equal("Portugal", lisbon.Country);
            Assert.Equal("PT", lisbon.CountryCode);   
            Assert.Equal(38.71667, lisbon.Latitude);
            Assert.Equal(-9.13333, lisbon.Longitude);
        }

        [Fact]
        public void DisplayName_IncludesRegion_WhenRegionIsKnown()
        {
            Destination porto = new Destination("Porto", "Portugal", "PT", 41.14961, -8.61099);
            porto.Region = "Porto District";

            Assert.Equal("Porto, Porto District, Portugal", porto.DisplayName);
        }

        [Fact]
        public void DisplayName_SkipsRegion_WhenRegionIsEmpty()
        {
            Destination lisbon = new Destination("Lisbon", "Portugal", "PT", 38.71667, -9.13333);

            Assert.Equal("Lisbon, Portugal", lisbon.DisplayName);
        }

        [Fact]
        public void FlagUrl_UsesLowerCaseCountryCode()
        {
            Destination lisbon = new Destination("Lisbon", "Portugal", "PT", 38.71667, -9.13333);

            Assert.Equal("https://flagcdn.com/w80/pt.png", lisbon.FlagUrl);
        }

        [Fact]
        public void FlagUrl_IsEmpty_WhenCountryCodeIsMissing()
        {
            Destination unknown = new Destination();

            Assert.Equal(string.Empty, unknown.FlagUrl);
        }

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
        public void Equals_ReturnsFalse_ForNullOrOtherTypes()
        {
            Destination paris = new Destination("Paris", "France", "FR", 48.85341, 2.3488);

            Assert.False(paris.Equals("Paris"));
            Assert.False(paris.Equals(null));
            Assert.False(paris == null);
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

        [Fact]
        public void ToPlanUrl_EncodesSpacesAndAccents()
        {
            Destination panama = new Destination("Panamá City", "Panama", "PA", 8.9936, -79.51973);

            string url = panama.ToPlanUrl();

            Assert.Contains("name=Panam%C3%A1%20City", url);
        }

        [Fact]
        public void ToPlanUrl_UsesDotsForDecimals_EvenOnASpanishComputer()
        {
            CultureInfo originalCulture = CultureInfo.CurrentCulture;
            try
            {
                
                CultureInfo.CurrentCulture = new CultureInfo("es-ES");
                Destination lisbon = new Destination("Lisbon", "Portugal", "PT", 38.71667, -9.13333);

                string url = lisbon.ToPlanUrl();

                Assert.Contains("lat=38.71667", url);
                Assert.Contains("lon=-9.13333", url);
            }
            finally
            {
               
                CultureInfo.CurrentCulture = originalCulture;
            }
        }
        
        
        [Fact]
        public void MapEmbedUrl_ShowsTheAreaAroundTheCity_WithAMarkerInTheCentre()
        {
            Destination lisbon = new Destination("Lisbon", "Portugal", "PT", 38.72509, -9.1498);

            string url = lisbon.MapEmbedUrl;

            Assert.StartsWith("https://www.openstreetmap.org/export/embed.html?", url);
            Assert.Contains("bbox=-9.2098,38.69509,-9.0898,38.75509", url);
            Assert.Contains("marker=38.72509,-9.1498", url);
        }

        [Fact]
        public void MapLinkUrl_PointsToTheCityOnOpenStreetMap()
        {
            Destination lisbon = new Destination("Lisbon", "Portugal", "PT", 38.72509, -9.1498);

            Assert.Equal("https://www.openstreetmap.org/?mlat=38.72509&mlon=-9.1498#map=14/38.72509/-9.1498", lisbon.MapLinkUrl);
        }
    }
}