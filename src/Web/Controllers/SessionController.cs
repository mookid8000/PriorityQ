using System.Web.Mvc;
using Web.Extensions;
using Web.Forms;
using Web.Models;
using Web.Repositories;
using Web.ViewModel;

namespace Web.Controllers
{
    public class SessionController : BaseController
    {
        readonly ISessionRepository sessionRepository;

        public SessionController(ISessionRepository sessionRepository)
        {
            this.sessionRepository = sessionRepository;
        }

        [HttpGet]
        public ViewResult New()
        {
            var expirationTime = Time.Now() + 2.Hours();
            var timeOfDay = expirationTime.TimeOfDay;
            var date = expirationTime.Date;
            var form = new CreateSessionForm
                           {
                               ExpirationDate = date.ToShortDateString(),
                               ExpirationTime = string.Format("{0:00}:{1:00}", timeOfDay.Hours,
                                                              timeOfDay.Minutes),
                           };

            return View("new", form);
        }

        [HttpPost]
        public ActionResult New(CreateSessionForm form)
        {
            if (ModelState.IsValid)
            {
                var session = new Session(form.Headline, CurrentUserSession.UserId);

                if (form.HasLocation)
                {
                    session.Location = new Location(form.Lat, form.Lng);
                }

                if (form.HasExpirationTime)
                {
                    session.ExpirationTime = Time.Now() + form.TimeToExpire;
                }

                sessionRepository.Save(session);

                return RedirectToAction("show", "session", new {id = session.Id});
            }

            return View("new", form);
        }

        [HttpGet]
        public ViewResult Show(string id)
        {
            var session = sessionRepository.Load(id.AsObjectId());

            return View(new SessionViewModel(session, CurrentUserSession));
        }

        [HttpPost]
        public JsonResult Upvote(string id, int index)
        {
            var votes = sessionRepository.IncrementVotesForQuestion(id.AsObjectId(), index, CurrentUserSession.UserId);

            return Json(new {votes});
        }

        [HttpPost]
        public ActionResult Add(AddQuestionForm form)
        {
            var sessionId = form.SessionId.AsObjectId();

            if (ModelState.IsValid)
            {
                sessionRepository.AddQuestion(sessionId, new Question(form.Text));
            }

            return RedirectToAction("show", "session", new { id = sessionId });
        }
    }
}