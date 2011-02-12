using System.Web.Mvc;
using Web.Services;

namespace Web.Controllers
{
    public abstract class BaseController : Controller
    {
        public IUserSessionService UserSessionService { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            CurrentUserSession = UserSessionService.GetOrCreateCurrentUserSession();
        }

        public UserSession CurrentUserSession { get; set; }
    }
}