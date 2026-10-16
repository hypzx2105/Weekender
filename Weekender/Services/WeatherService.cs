using System.Globalization;
using Newtonsoft.Json;
using RestSharp;
using Weekender.ApiModels;
using Weekender.Models;

namespace Weekender.Services
{
     
    public class WeatherService : IWeatherService
    {
        public static readonly string FORECAST_URL = "https://api.open-meteo.com/v1/";
        private static readonly string SERVICE_NAME = "Open-Meteo Forecast";
        private static readonly string DAILY_VARIABLES = "weather_code,temperature_2m_max,temperature_2m_min,precipitation_probability_max";

        
        private readonly RestClient client;

       
        public WeatherService()
        {
            client = new RestClient(FORECAST_URL);
        }

     
        public async Task<List<DailyForecast>> GetForecastAsync(double latitude, double longitude, DateTime startDate, DateTime endDate)
        {
            if (endDate.Date < startDate.Date)
            {
               
                throw new ArgumentException("The end date can't be before the start date.");
            }

            var request = new RestRequest("forecast");
       
            request.AddParameter("latitude", latitude.ToString(CultureInfo.InvariantCulture));
            request.AddParameter("longitude", longitude.ToString(CultureInfo.InvariantCulture));
            request.AddParameter("daily", DAILY_VARIABLES);
            request.AddParameter("timezone", "auto");
            request.AddParameter("start_date", startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            request.AddParameter("end_date", endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

            RestResponse response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new ApiException(SERVICE_NAME, $"Could not get the forecast. {response.ErrorMessage}",
                    (int)response.StatusCode, response.ErrorException);
            }

            return ParseResponse(response.Content);
        }

        
        public static List<DailyForecast> ParseResponse(string? json)
        {
            List<DailyForecast> forecasts = new List<DailyForecast>();

            if (string.IsNullOrWhiteSpace(json))
            {
                return forecasts;
            }

            ForecastResponse? forecastResponse;
            try
            {
                forecastResponse = JsonConvert.DeserializeObject<ForecastResponse>(json);
            }
            catch (JsonException ex)
            {
                throw new ApiException(SERVICE_NAME, "The forecast returned data in an unexpected format.", 0, ex);
            }

            if (forecastResponse == null || forecastResponse.Daily == null)
            {
                return forecasts;
            }

            DailyData daily = forecastResponse.Daily;

          
            for (int i = 0; i < daily.Time.Count; i++)
            {
               
                int? weatherCode = daily.WeatherCode.ElementAtOrDefault(i);
                double? maxTemperature = daily.MaxTemperature.ElementAtOrDefault(i);
                double? minTemperature = daily.MinTemperature.ElementAtOrDefault(i);
                int? rainChance = daily.RainChance.ElementAtOrDefault(i);

                if (weatherCode == null || maxTemperature == null || minTemperature == null)
                {
                   
                    continue;
                }

                forecasts.Add(new DailyForecast(daily.Time[i], weatherCode.Value, maxTemperature.Value,
                    minTemperature.Value, rainChance ?? 0));
            }

            return forecasts;
        }
    }
}