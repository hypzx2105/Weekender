using System.Globalization;
using Weekender.Helpers;

namespace Weekender.Tests
{
    public class DateHelperTests
    {
        
        private static DateTime Day(string isoDate)
        {
            return DateTime.ParseExact(isoDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        [Theory]
        [InlineData("2026-10-12", "2026-10-16")]   
        [InlineData("2026-10-14", "2026-10-16")]   
        [InlineData("2026-10-16", "2026-10-16")]   
        [InlineData("2026-10-17", "2026-10-23")]   
        [InlineData("2026-10-18", "2026-10-23")]   
        public void GetNextFriday_ReturnsTheComingFriday(string today, string expectedFriday)
        {
            DateTime result = DateHelper.GetNextFriday(Day(today));

            Assert.Equal(Day(expectedFriday), result);
            Assert.Equal(DayOfWeek.Friday, result.DayOfWeek);
        }

        [Fact]
        public void GetNights_ReturnsTwo_ForFridayToSunday()
        {
            int nights = DateHelper.GetNights(Day("2026-10-16"), Day("2026-10-18"));

            Assert.Equal(2, nights);
        }

        [Fact]
        public void ValidateTripDates_ReturnsNull_ForAValidWeekend()
        {
            string? error = DateHelper.ValidateTripDates(Day("2026-10-16"), Day("2026-10-18"), Day("2026-10-15"));

            Assert.Null(error);
        }

        [Fact]
        public void ValidateTripDates_ReturnsError_WhenTheTripStartsInThePast()
        {
            string? error = DateHelper.ValidateTripDates(Day("2026-10-10"), Day("2026-10-12"), Day("2026-10-15"));

            Assert.Equal("The trip can't start in the past.", error);
        }

        [Theory]
        [InlineData("2026-10-18")]   
        [InlineData("2026-10-17")]   
        public void ValidateTripDates_ReturnsError_WhenReturnIsNotAfterDeparture(string returnDate)
        {
            string? error = DateHelper.ValidateTripDates(Day("2026-10-18"), Day(returnDate), Day("2026-10-15"));

            Assert.Equal("The return date must be after the departure date.", error);
        }

        [Fact]
        public void ValidateTripDates_ReturnsError_WhenTheTripIsTooLong()
        {
            string? error = DateHelper.ValidateTripDates(Day("2026-10-16"), Day("2026-11-16"), Day("2026-10-15"));

            Assert.NotNull(error);
            Assert.Contains("14 nights", error);
        }

        [Theory]
        [InlineData("2026-10-18", true)]    
        [InlineData("2026-10-30", true)]    
        [InlineData("2026-10-31", false)]   
        [InlineData("2026-12-20", false)]
        public void IsWithinForecastRange_ChecksTheSixteenDayLimit(string tripEnd, bool expected)
        {
            bool result = DateHelper.IsWithinForecastRange(Day(tripEnd), Day("2026-10-15"));

            Assert.Equal(expected, result);
        }
    }
}