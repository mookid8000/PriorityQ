using System;
using System.Web.Mvc;
using Web.Models;
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
            var now = Time.Now();

            var sessionHeadlines = sessionRepository.GetAllSessions(firstSessionToShow, sessionsPerPage, now);

            var sessionCount = sessionRepository.CountAllSessions(now);

            return View(new SessionIndexViewModel(sessionHeadlines, sessionCount, firstSessionToShow, sessionsPerPage));
        }
    }
}