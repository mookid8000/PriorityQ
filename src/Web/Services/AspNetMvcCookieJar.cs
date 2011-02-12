using System;
using System.Web;

namespace Web.Services
{
    public class AspNetMvcCookieJar : ICookieJar
    {
        public string Get(string name)
        {
            var httpContext = GetHttpContextOrThrow();
            var cookies = httpContext.Request.Cookies;
            var cookie = cookies[name];

            return cookie != null
                       ? cookie.Value
                       : null;
        }

        public void Set(string name, string value, TimeSpan expiration)
        {
            var httpContext = GetHttpContextOrThrow();
            var cookies = httpContext.Response.Cookies;
            
            cookies.Add(new HttpCookie(name, value)
                            {
                                Expires = DateTime.UtcNow + expiration
                            });
        }

        HttpContext GetHttpContextOrThrow()
        {
            var httpContext = HttpContext.Current;
            if (httpContext == null)
            {
                throw new InvalidOperationException("There is no current HTTP context");
            }
            return httpContext;
        }
    }
}