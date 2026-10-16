namespace Weekender.Models
{
    
    public class DailyForecast
    {
        
        private DateTime date;
        private int weatherCode;
        private double maxTemperature;
        private double minTemperature;
        private int rainChance;   

        
        public DateTime Date { get { return date; } set { date = value; } }
        public int WeatherCode { get { return weatherCode; } set { weatherCode = value; } }
        public double MaxTemperature { get { return maxTemperature; } set { maxTemperature = value; } }
        public double MinTemperature { get { return minTemperature; } set { minTemperature = value; } }
        public int RainChance { get { return rainChance; } set { rainChance = Math.Clamp(value, 0, 100); } }

        
        public WeatherCondition Condition { get { return GetCondition(weatherCode); } }
        public string Description { get { return GetDescription(Condition); } }
        public string Icon { get { return GetIconClass(Condition); } }

        //Constructors
        public DailyForecast()
        {
            date = DateTime.Today;
        }

        public DailyForecast(DateTime date, int weatherCode, double maxTemperature, double minTemperature, int rainChance)
        {
            this.date = date;
            this.weatherCode = weatherCode;
            this.maxTemperature = maxTemperature;
            this.minTemperature = minTemperature;
            this.rainChance = Math.Clamp(rainChance, 0, 100);   
        }

        public static WeatherCondition GetCondition(int weatherCode)
        {
            switch (weatherCode)
            {
                case 0:
                    return WeatherCondition.Clear;
                case 1:
                case 2:
                    return WeatherCondition.PartlyCloudy;
                case 3:
                    return WeatherCondition.Cloudy;
                case 45:
                case 48:
                    return WeatherCondition.Fog;
                case 51:
                case 53:
                case 55:
                case 56:
                case 57:
                    return WeatherCondition.Drizzle;
                case 61:
                case 63:
                case 65:
                case 66:
                case 67:
                case 80:
                case 81:
                case 82:
                    return WeatherCondition.Rain;
                case 71:
                case 73:
                case 75:
                case 77:
                case 85:
                case 86:
                    return WeatherCondition.Snow;
                case 95:
                case 96:
                case 99:
                    return WeatherCondition.Thunderstorm;
                default:
                    return WeatherCondition.Unknown;
            }
        }

        public static string GetDescription(WeatherCondition condition)
        {
            switch (condition)
            {
                case WeatherCondition.Clear:
                    return "Clear sky";
                case WeatherCondition.PartlyCloudy:
                    return "Partly cloudy";
                case WeatherCondition.Cloudy:
                    return "Cloudy";
                case WeatherCondition.Fog:
                    return "Fog";
                case WeatherCondition.Drizzle:
                    return "Drizzle";
                case WeatherCondition.Rain:
                    return "Rain";
                case WeatherCondition.Snow:
                    return "Snow";
                case WeatherCondition.Thunderstorm:
                    return "Thunderstorm";
                default:
                    return "Unknown";
            }
        }

        public string IconClass { get { return GetIconClass(Condition); } }
       
        //Icon list: https://icons.getbootstrap.com
        public static string GetIconClass(WeatherCondition condition)
        {
            switch (condition)
            {
                case WeatherCondition.Clear:
                    return "bi-sun";
                case WeatherCondition.PartlyCloudy:
                    return "bi-cloud-sun";
                case WeatherCondition.Cloudy:
                    return "bi-cloud";
                case WeatherCondition.Fog:
                    return "bi-cloud-fog";
                case WeatherCondition.Drizzle:
                    return "bi-cloud-drizzle";
                case WeatherCondition.Rain:
                    return "bi-cloud-rain";
                case WeatherCondition.Snow:
                    return "bi-snow";
                case WeatherCondition.Thunderstorm:
                    return "bi-cloud-lightning-rain";
                default:
                    return "bi-question-circle";
            }
        }

       
        public override string ToString()
        {
            return $"DailyForecast - {date:ddd dd MMM}: {Description}, {minTemperature}-{maxTemperature}°C, {rainChance}% rain";
        }
    }
}