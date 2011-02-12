namespace Web.Services
{
    public class UserSession
    {
        public UserSession(string userSessionId)
        {
            UserId = userSessionId;
        }

        public string UserId { get; set; }
    }
}