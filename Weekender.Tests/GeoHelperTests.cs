using Weekender.Helpers;

namespace Weekender.Tests
{
    public class GeoHelperTests
    {
        [Fact]
        public void DistanceKm_DublinToLondon_IsAbout464Km()
        {
            double distance = GeoHelper.DistanceKm(53.3498, -6.2603, 51.5074, -0.1278);

            Assert.InRange(distance, 460, 468);
        }
    }
}