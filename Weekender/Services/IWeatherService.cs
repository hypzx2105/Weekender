using Weekender.Models;

namespace Weekender.Services
{
   
    public interface IWeatherService
    {
        Task<List<DailyForecast>> GetForecastAsync(double latitude, double longitude, DateTime startDate, DateTime endDate);
    }
}