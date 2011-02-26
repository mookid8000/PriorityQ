using System.Web;
using System.Web.Mvc;

namespace Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static GoogleMapsHelper GMaps(this HtmlHelper helper)
        {
            return new GoogleMapsHelper(helper);
        }
    }

    public class GoogleMapsHelper
    {
        readonly HtmlHelper helper;

        public GoogleMapsHelper(HtmlHelper helper)
        {
            this.helper = helper;
        }

        public IHtmlString Map(LocationFields locationFields)
        {
            return new MvcHtmlString("<strong>do stuff in here</strong>");
        }
    }

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