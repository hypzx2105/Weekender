using Weekender.Models;

namespace Weekender.Services
{
    
    public interface IWikipediaService
    {
        Task<PlaceSummary?> GetCitySummaryAsync(Destination destination);
        Task<List<Landmark>> GetNearbyLandmarksAsync(Destination destination);
        
    }
}