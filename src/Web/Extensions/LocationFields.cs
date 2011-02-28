namespace Web.Extensions
{
    public class LocationFields
    {
        readonly string latitudeFieldId;
        readonly string longitudeFieldId;

        public LocationFields(string latitudeFieldId, string longitudeFieldId)
        {
            this.latitudeFieldId = latitudeFieldId;
            this.longitudeFieldId = longitudeFieldId;
        }

        public string LatitudeFieldId
        {
            get { return latitudeFieldId; }
        }

        public string LongitudeFieldId
        {
            get { return longitudeFieldId; }
        }
    }
}