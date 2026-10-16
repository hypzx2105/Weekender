using Weekender.Models;

namespace Weekender.Helpers
{
   
    public static class PackingAdvisor
    {
        public static readonly int RAIN_THRESHOLD = 50;      
        public static readonly double COLD_THRESHOLD = 8;    
        public static readonly double HOT_THRESHOLD = 25;    
       
        public static List<string> GetPackingTips(List<DailyForecast> forecasts)
        {
            List<string> tips = new List<string>();

            if (forecasts.Count == 0)
            {
                return tips;
            }

         
            if (forecasts.Any(day => day.RainChance >= RAIN_THRESHOLD))
            {
                tips.Add("Umbrella or rain jacket - rain is likely on at least one day");
            }
            if (forecasts.Any(day => day.Condition == WeatherCondition.Snow))
            {
                tips.Add("Waterproof boots - snow is expected");
            }
            if (forecasts.Min(day => day.MinTemperature) < COLD_THRESHOLD)
            {
                tips.Add("Warm coat - it gets cold at night");
            }
            if (forecasts.Max(day => day.MaxTemperature) >= HOT_THRESHOLD)
            {
                tips.Add("Sunscreen and sunglasses - it will be hot");
            }

            
            tips.Add("Comfortable walking shoes");

            return tips;
        }
    }
}