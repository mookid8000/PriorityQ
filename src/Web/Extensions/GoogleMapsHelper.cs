using System.Web;
using System.Web.Mvc;
using Web.Infrastructure;

namespace Web.Extensions
{
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

        public IHtmlString Scripts()
        {
            return new MvcHtmlString(App.Environment == AppEnvironment.Debug
                                         ? @"<script src=""http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAApTSOhgbgKsPZn74bYo5QGhT2yXp_ZAY8_ufC3CFXhHIE1NvwkxQPW1flQpleSTbf_ueqoanmhYiLxw"" type=""text/javascript""></script>"
                                         : @"<script src=""http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=ABQIAAAApTSOhgbgKsPZn74bYo5QGhTRtxfLY_M3vrFsARlAcv3vG21kIxThAhh7X7YmGBxYzXRqad9b8TkiSQ"" type=""text/javascript""></script>");
        }
    }
}