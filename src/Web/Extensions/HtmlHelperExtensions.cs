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
}