using Newtonsoft.Json;

namespace Weekender.ApiModels
{
    
    public class GeocodingResponse
    {
        [JsonProperty("results")]
        public List<GeocodingResult>? Results { get; set; }   
    }

    public class GeocodingResult
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("country_code")]
        public string? CountryCode { get; set; }

        [JsonProperty("admin1")]
        public string? Admin1 { get; set; }   

        [JsonProperty("timezone")]
        public string? Timezone { get; set; }

        [JsonProperty("population")]
        public int? Population { get; set; }   
    }
}