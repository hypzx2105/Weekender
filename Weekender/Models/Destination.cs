using System.Globalization;

namespace Weekender.Models
{
    
    public class Destination
    {
        
        private string name;
        private string country;
        private string countryCode;
        private string region;
        private double latitude;
        private double longitude;
        private string timezone;
        private int population;

        
        public string Name { get { return name; } set { name = value; } }
        public string Country { get { return country; } set { country = value; } }
        public string CountryCode { get { return countryCode; } set { countryCode = value.ToUpper(); } }
        public string Region { get { return region; } set { region = value; } }
        public double Latitude { get { return latitude; } set { latitude = value; } }
        public double Longitude { get { return longitude; } set { longitude = value; } }
        public string Timezone { get { return timezone; } set { timezone = value; } }
        public int Population { get { return population; } set { population = value; } }

        
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(region) || region == name)
                {
                    return $"{name}, {country}";
                }
                return $"{name}, {region}, {country}";
            }
        }

        public string FlagUrl
        {
            get
            {
                
                if (string.IsNullOrWhiteSpace(countryCode))
                {
                    return string.Empty;
                }
                return $"https://flagcdn.com/w80/{countryCode.ToLower()}.png";
            }
        }

        
        public Destination()
        {
            name = "unknown";
            country = "unknown";
            countryCode = string.Empty;
            region = string.Empty;
            timezone = "auto";
        }

        public Destination(string name, string country, string countryCode, double latitude, double longitude)
        {
            this.name = name;
            this.country = country;
            this.countryCode = countryCode.ToUpper();
            this.latitude = latitude;
            this.longitude = longitude;
            region = string.Empty;
            timezone = "auto";
        }
        
        
        public string ToPlanUrl()
        {
            string lat = latitude.ToString(CultureInfo.InvariantCulture);
            string lon = longitude.ToString(CultureInfo.InvariantCulture);

            return $"plan?name={Uri.EscapeDataString(name)}&country={Uri.EscapeDataString(country)}&code={countryCode}"
                   + $"&region={Uri.EscapeDataString(region)}&lat={lat}&lon={lon}&tz={Uri.EscapeDataString(timezone)}";
        }

        
        public override string ToString()
        {
            return $"Destination - {DisplayName} ({latitude}, {longitude})";
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is Destination))
            {
                
                return false;
            }

            Destination other = (Destination)obj;
            
            return name == other.name
                && countryCode == other.countryCode
                && latitude == other.latitude
                && longitude == other.longitude;
        }

        public static bool operator ==(Destination? left, Destination? right)
        {
          
            if (left is null)
            {
                return right is null;
            }
            return left.Equals(right);
        }

        public static bool operator !=(Destination? left, Destination? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            
            return (name, countryCode, latitude, longitude).GetHashCode();
        }
    }
}