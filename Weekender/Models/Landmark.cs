using System.Globalization;

namespace Weekender.Models
{
    
    public class Landmark : IComparable<Landmark>
    {
        
        private string title;
        private string description;
        private string imageUrl;
        private double latitude;
        private double longitude;
        private double distanceKm;   

       
        public string Title { get { return title; } set { title = value; } }
        public string Description { get { return description; } set { description = value; } }
        public string ImageUrl { get { return imageUrl; } set { imageUrl = value; } }
        public double Latitude { get { return latitude; } set { latitude = value; } }
        public double Longitude { get { return longitude; } set { longitude = value; } }
        public double DistanceKm { get { return distanceKm; } set { distanceKm = value; } }

        
        public string PageUrl
        {
           
            get { return $"https://en.wikipedia.org/wiki/{Uri.EscapeDataString(title.Replace(' ', '_'))}"; }
        }

        public string DistanceText
        {
            get
            {
                if (distanceKm < 1)
                {
                  
                    int metres = (int)(Math.Round(distanceKm * 100) * 10);
                    return $"{metres} m from the centre";
                }
                return $"{distanceKm.ToString("0.0", CultureInfo.InvariantCulture)} km from the centre";
            }
        }

       
        public Landmark()
        {
            title = string.Empty;
            description = string.Empty;
            imageUrl = string.Empty;
        }

        public Landmark(string title, string description, string imageUrl, double latitude, double longitude, double distanceKm)
        {
            this.title = title;
            this.description = description;
            this.imageUrl = imageUrl;
            this.latitude = latitude;
            this.longitude = longitude;
            this.distanceKm = distanceKm;
        }

      
        public int CompareTo(Landmark? other)
        {
            if (other is null)
            {
                return -1;   
            }

            int result = distanceKm.CompareTo(other.distanceKm);
            if (result == 0)
            {
                result = string.Compare(title, other.title, StringComparison.OrdinalIgnoreCase);
            }
            return result;
        }

        
        public override string ToString()
        {
            return $"Landmark - {title} ({DistanceText})";
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is Landmark))
            {
                return false;
            }

            Landmark other = (Landmark)obj;
            
            return string.Equals(title, other.title, StringComparison.OrdinalIgnoreCase);
        }

        public static bool operator ==(Landmark? left, Landmark? right)
        {
            if (left is null)
            {
                return right is null;
            }
            return left.Equals(right);
        }

        public static bool operator !=(Landmark? left, Landmark? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return title.ToLowerInvariant().GetHashCode();
        }
    }
}