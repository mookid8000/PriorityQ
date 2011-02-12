using System;

namespace Web.Services
{
    public interface ICookieJar
    {
        string Get(string name);
        void Set(string name, string value, TimeSpan expiration);
    }
}