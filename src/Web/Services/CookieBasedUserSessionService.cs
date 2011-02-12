using System;

namespace Web.Services
{
    public class CookieBasedUserSessionService : IUserSessionService
    {
        readonly ICookieJar cookieJar;
        const string UserSessionIdKey = "__user_session";

        public CookieBasedUserSessionService(ICookieJar cookieJar)
        {
            this.cookieJar = cookieJar;
        }

        public UserSession GetOrCreateCurrentUserSession()
        {
            var userSessionId = cookieJar.Get(UserSessionIdKey)
                                ?? CreateNewId();

            return new UserSession(userSessionId);
        }

        string CreateNewId()
        {
            var newId = Guid.NewGuid().ToString();

            cookieJar.Set(UserSessionIdKey, newId, TimeSpan.FromDays(7));

            return newId;
        }
    }
}