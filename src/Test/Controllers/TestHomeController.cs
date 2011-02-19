using System;
using System.Collections.Generic;
using MongoDB.Bson;
using NUnit.Framework;
using Rhino.Mocks;
using Shouldly;
using Web.Controllers;
using Web.Models;
using Web.Repositories;
using Web.ViewModel;

namespace Test.Controllers
{
    [TestFixture]
    public class TestHomeController : ControllerFixture<HomeController>
    {
        ISessionRepository sessionRepository;

        protected override HomeController Create()
        {
            sessionRepository = Mock<ISessionRepository>();
            return new HomeController(sessionRepository);
        }

        [Test]
        public void RendersHomeView()
        {
            var now = DateTime.Now;
            FakeTime.Fake(now);

            sessionRepository.Stub(r => r.GetAllSessions(0, 10, now))
                .Return(new List<SessionHeadline>
                            {
                                new SessionHeadline
                                    {
                                        Id = ObjectId.GenerateNewId(),
                                        Headline = "Hello there!",
                                        QuestionCount = 23
                                    }
                            });

            var viewResult = controller.Index(null);

            viewResult.ViewName.ShouldBe("");
            var model = (SessionIndexViewModel)viewResult.Model;
            model.SessionHeadlines[0].Headline.ShouldBe("Hello there!");
        }
    }
}
