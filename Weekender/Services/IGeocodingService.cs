using Weekender.Models;

namespace Weekender.Services

{
    public interface IGeocodingService
    {
        Task<List<Destination>> SearchCitiesAsync(string cityName);
    }
}