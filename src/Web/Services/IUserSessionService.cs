namespace Web.Services
{
    public interface IUserSessionService
    {
        UserSession GetOrCreateCurrentUserSession();
    }
}