using System.Web.Mvc;
using Web.Repositories;
using Web.ViewModel;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        readonly ISessionRepository sessionRepository;

        public HomeController(ISessionRepository sessionRepository)
        {
            this.sessionRepository = sessionRepository;
        }

        public ViewResult Index(int? first)
        {
            var firstSessionToShow = first ?? 0;
            var sessionsPerPage = 10;

            var sessionHeadlines = sessionRepository.GetAllSessions(firstSessionToShow, sessionsPerPage);

            var sessionCount = sessionRepository.CountAllSessions();

            return View(new SessionIndexViewModel(sessionHeadlines, sessionCount, firstSessionToShow, sessionsPerPage));
        }
    }
}