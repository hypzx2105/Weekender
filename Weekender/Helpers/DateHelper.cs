namespace Weekender.Helpers
{
   
    public static class DateHelper
    {
        public static readonly int MAX_TRIP_NIGHTS = 14;
        public static readonly int FORECAST_DAYS = 16;   

       
       
        public static DateTime GetNextFriday(DateTime today)
        {
            
            int daysUntilFriday = ((int)DayOfWeek.Friday - (int)today.DayOfWeek + 7) % 7;
            return today.Date.AddDays(daysUntilFriday);
        }

        
        public static int GetNights(DateTime start, DateTime end)
        {
            return (end.Date - start.Date).Days;
        }

         
        public static string? ValidateTripDates(DateTime start, DateTime end, DateTime today)
        {
            if (start.Date < today.Date)
            {
                return "The trip can't start in the past.";
            }
            if (end.Date <= start.Date)
            {
                return "The return date must be after the departure date.";
            }
            if (GetNights(start, end) > MAX_TRIP_NIGHTS)
            {
                return $"Weekender plans trips of up to {MAX_TRIP_NIGHTS} nights.";
            }
            return null;
        }

       
        public static bool IsWithinForecastRange(DateTime end, DateTime today)
        {
            return (end.Date - today.Date).Days < FORECAST_DAYS;
        }
    }
}