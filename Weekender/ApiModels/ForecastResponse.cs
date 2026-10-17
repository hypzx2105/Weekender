using Newtonsoft.Json;

namespace Weekender.ApiModels
{
    //Open-Meteo Forecast API response: https://open-meteo.com/en/docs
    //JsonProperty: https://www.newtonsoft.com/json/help/html/JsonPropertyName.htm
    public class ForecastResponse
    {
        [JsonProperty("timezone")]
        public string? Timezone { get; set; }

        [JsonProperty("daily")]
        public DailyData? Daily { get; set; }
    }

    public class DailyData
    {
        [JsonProperty("time")]
        public List<DateTime> Time { get; set; } = new List<DateTime>();

      
        [JsonProperty("weather_code")]
        public List<int?> WeatherCode { get; set; } = new List<int?>();

        [JsonProperty("temperature_2m_max")]
        public List<double?> MaxTemperature { get; set; } = new List<double?>();

        [JsonProperty("temperature_2m_min")]
        public List<double?> MinTemperature { get; set; } = new List<double?>();

        [JsonProperty("precipitation_probability_max")]
        public List<int?> RainChance { get; set; } = new List<int?>();
    }
}