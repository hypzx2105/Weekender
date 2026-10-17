using Weekender.Helpers;

namespace Weekender.Tests
{
    public class GeoHelperTests
    {
        [Fact]
        public void DistanceKm_ReturnsZero_ForTheSamePoint()
        {
            double distance = GeoHelper.DistanceKm(53.3498, -6.2603, 53.3498, -6.2603);

            Assert.Equal(0, distance, 3);
        }

        [Fact]
        public void DistanceKm_DublinToLondon_IsAbout464Km()
        {
          
            double distance = GeoHelper.DistanceKm(53.3498, -6.2603, 51.5074, -0.1278);

            Assert.InRange(distance, 460, 468);
        }

        [Fact]
        public void DistanceKm_IsTheSameInBothDirections()
        {
            double dublinToLisbon = GeoHelper.DistanceKm(53.3498, -6.2603, 38.7167, -9.1333);
            double lisbonToDublin = GeoHelper.DistanceKm(38.7167, -9.1333, 53.3498, -6.2603);

            Assert.Equal(dublinToLisbon, lisbonToDublin, 6);
        }
    }
}