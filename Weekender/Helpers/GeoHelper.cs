namespace Weekender.Helpers
{
    
    public static class GeoHelper
    {
        public static readonly double EARTH_RADIUS_KM = 6371;

         
        //Source: https://en.wikipedia.org/wiki/Haversine_formula
        public static double DistanceKm(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            double latitudeDifference = ToRadians(latitude2 - latitude1);
            double longitudeDifference = ToRadians(longitude2 - longitude1);

            double a = Math.Sin(latitudeDifference / 2) * Math.Sin(latitudeDifference / 2)
                       + Math.Cos(ToRadians(latitude1)) * Math.Cos(ToRadians(latitude2))
                                                        * Math.Sin(longitudeDifference / 2) * Math.Sin(longitudeDifference / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return EARTH_RADIUS_KM * c;
        }

        private static double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}