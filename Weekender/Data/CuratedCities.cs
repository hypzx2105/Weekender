using Weekender.Models;

namespace Weekender.Data
{
   
    public static class CuratedCities
    {
        
        public static List<Destination> GetPopularCities()
        {
            
            List<Destination> cities = new List<Destination>
            {
                CreateCity("Lisbon", "Portugal", "PT", "Lisbon District", 38.72509, -9.1498, "Europe/Lisbon"),
                CreateCity("Barcelona", "Spain", "ES", "Catalonia", 41.38879, 2.15899, "Europe/Madrid"),
                CreateCity("Rome", "Italy", "IT", "Lazio", 41.89193, 12.51133, "Europe/Rome"),
                CreateCity("Amsterdam", "Netherlands", "NL", "North Holland", 52.37403, 4.88969, "Europe/Amsterdam"),
                CreateCity("Kraków", "Poland", "PL", "Lesser Poland", 50.06143, 19.93658, "Europe/Warsaw"),
                CreateCity("Paris", "France", "FR", "Île-de-France", 48.85341, 2.3488, "Europe/Paris"),
                CreateCity("Budapest", "Hungary", "HU", "Budapest", 47.49835, 19.04045, "Europe/Budapest"),
                CreateCity("Edinburgh", "United Kingdom", "GB", "Scotland", 55.95206, -3.19648, "Europe/London")
            };

            return cities;
        }

        private static Destination CreateCity(string name, string country, string countryCode, string region,
            double latitude, double longitude, string timezone)
        {
            Destination city = new Destination(name, country, countryCode, latitude, longitude);
            city.Region = region;
            city.Timezone = timezone;
            return city;
        }
    }
}