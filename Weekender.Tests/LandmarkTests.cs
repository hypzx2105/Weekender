using Weekender.Models;

namespace Weekender.Tests
{
    public class LandmarkTests
    {
        private static Landmark CreateLandmark(string title, double distanceKm)
        {
            return new Landmark(title, "Sample description", "https://upload.wikimedia.org/sample.jpg", 38.7, -9.1, distanceKm);
        }

        [Fact]
        public void CompareTo_ReturnsNegative_WhenThisLandmarkIsCloser()
        {
            Landmark castle = CreateLandmark("São Jorge Castle", 0.3);
            Landmark tower = CreateLandmark("Belém Tower", 7.7);

            Assert.True(castle.CompareTo(tower) < 0);
            Assert.True(tower.CompareTo(castle) > 0);
        }

        [Fact]
        public void CompareTo_UsesTheTitle_WhenTheDistanceIsTheSame()
        {
            Landmark museum = CreateLandmark("Museum", 1.0);
            Landmark church = CreateLandmark("Church", 1.0);

            
            Assert.True(church.CompareTo(museum) < 0);
        }

        [Fact]
        public void CompareTo_PutsNullLast()
        {
            Landmark castle = CreateLandmark("São Jorge Castle", 0.3);

            Assert.True(castle.CompareTo(null) < 0);
        }

        [Fact]
        public void Sort_OrdersLandmarks_ClosestFirst()
        {
            List<Landmark> landmarks = new List<Landmark>
            {
                CreateLandmark("Belém Tower", 7.7),
                CreateLandmark("São Jorge Castle", 0.3),
                CreateLandmark("Lisbon Cathedral", 0.75)
            };

          
            landmarks.Sort();

            Assert.Equal("São Jorge Castle", landmarks[0].Title);
            Assert.Equal("Lisbon Cathedral", landmarks[1].Title);
            Assert.Equal("Belém Tower", landmarks[2].Title);
        }

        [Fact]
        public void Equals_ReturnsTrue_ForTheSameTitle_IgnoringCase()
        {
            Landmark first = CreateLandmark("Belém Tower", 7.7);
            Landmark second = CreateLandmark("BELÉM TOWER", 2.0);

            Assert.True(first.Equals(second));
            Assert.True(first == second);
            Assert.Equal(first.GetHashCode(), second.GetHashCode());
        }

        [Fact]
        public void Equals_ReturnsFalse_ForDifferentTitles()
        {
            Landmark tower = CreateLandmark("Belém Tower", 7.7);
            Landmark castle = CreateLandmark("São Jorge Castle", 0.3);

            Assert.False(tower.Equals(castle));
            Assert.True(tower != castle);
        }

        [Theory]
        [InlineData(0.312, "310 m from the centre")]
        [InlineData(0.746, "750 m from the centre")]
        [InlineData(7.71, "7.7 km from the centre")]
        [InlineData(12.0, "12.0 km from the centre")]
        public void DistanceText_ShowsMetresUnder1Km_AndKmOtherwise(double distanceKm, string expected)
        {
            Landmark landmark = CreateLandmark("Somewhere", distanceKm);

            Assert.Equal(expected, landmark.DistanceText);
        }

        [Fact]
        public void PageUrl_UsesUnderscoresAndEncodesSpecialCharacters()
        {
            Landmark tower = CreateLandmark("Belém Tower", 7.7);

            Assert.Equal("https://en.wikipedia.org/wiki/Bel%C3%A9m_Tower", tower.PageUrl);
        }
    }
}