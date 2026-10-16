using Weekender.Data;
using Weekender.Models;

namespace Weekender.Tests
{
    public class CuratedCitiesTests
    {
        [Fact]
        public void GetPopularCities_ReturnsEightCities()
        {
            List<Destination> cities = CuratedCities.GetPopularCities();

            Assert.Equal(8, cities.Count);
        }

        [Fact]
        public void GetPopularCities_HasNoDuplicates()
        {
            List<Destination> cities = CuratedCities.GetPopularCities();

            
            Assert.Equal(cities.Count, cities.Distinct().Count());
        }

        [Fact]
        public void GetPopularCities_AllHaveValidCoordinatesAndCountryCodes()
        {
            List<Destination> cities = CuratedCities.GetPopularCities();

            Assert.All(cities, city =>
            {
                Assert.InRange(city.Latitude, -90, 90);
                Assert.InRange(city.Longitude, -180, 180);
                Assert.Equal(2, city.CountryCode.Length);
            });
        }

        [Fact]
        public void GetPopularCities_ReturnsANewListEachTime()
        {
            List<Destination> first = CuratedCities.GetPopularCities();
            first.Clear();

            List<Destination> second = CuratedCities.GetPopularCities();

            Assert.Equal(8, second.Count);
        }
    }
}