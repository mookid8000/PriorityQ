using System.Globalization;

namespace Web.Models
{
    public class Location
    {
        public Location(string lat, string lng)
        {
            Lat = double.Parse(lat, CultureInfo.InvariantCulture);
            Lng = double.Parse(lng, CultureInfo.InvariantCulture);
        }

        protected Location()
        {
        }

        public virtual double Lat { get; set; }
        public virtual double Lng { get; set; }
    }
}