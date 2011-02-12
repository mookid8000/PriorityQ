using System.Collections.Generic;
using System.Web.Mvc;
using Norm;
using NUnit.Framework;
using Rhino.Mocks;
using Shouldly;
using Web.Controllers;
using Web.Forms;
using Web.Models;
using Web.Repositories;
using Web.Services;
using Web.ViewModel;

namespace Test.Controllers
{
    [TestFixture]
    public class TestSessionController : ControllerFixture<SessionController>
    {
        ISessionRepository sessionRepository;

        protected override SessionController Create()
        {
            sessionRepository = Mock<ISessionRepository>();
            return new SessionController(sessionRepository);
        }

        [Test]
        public void CastsUpvoteWithCurrentUserId()
        {
            // arrange
            controller.CurrentUserSession = new UserSession("knownId");
            var sessionId = ObjectId.NewObjectId();

            // act
            controller.Upvote(sessionId.ToString(), 1);

            // assert
            sessionRepository.AssertWasCalled(r => r.IncrementVotesForQuestion(sessionId, 1, "knownId"));
        }

        [Test]
        public void RendersNewView()
        {
            var result = controller.New();
            
            result.ViewName.ShouldBe("new");

            var form = (CreateSessionForm)result.Model;
            form.Headline.ShouldBe("");
        }

        [Test]
        public void CanSaveNewSession()
        {
            // arrange
            var form = new CreateSessionForm
                           {
                               Headline = "Hello there!",
                               Lat = "2.2",
                               Lng = "3.2",
                           };
            var newObjectId = ObjectId.NewObjectId();
            Session.NewId = () => newObjectId;
            controller.CurrentUserSession = new UserSession("someone");

            // act
            var actionResult = controller.New(form);

            // assert
            var result = (RedirectToRouteResult)actionResult;

            result.RouteValues["controller"].ShouldBe("session");
            result.RouteValues["action"].ShouldBe("show");
            result.RouteValues["id"].ShouldBe(newObjectId);

            //sessionRepository.AssertWasCalled(r => r.Save(Arg<Session>.Matches(s => s.Headline == "Hello there!")));
            //sessionRepository.AssertWasCalled(r => r.Save(Arg<Session>.Matches(s => s.CreatedBy == "someone")));
            //sessionRepository.AssertWasCalled(r => r.Save(Arg<Session>.Matches(s => s.Location.Lat == 2.2)));
            //sessionRepository.AssertWasCalled(r => r.Save(Arg<Session>.Matches(s => s.Location.Lng == 3.2)));
        }

        [Test]
        public void OrdersQuestionsByVotes()
        {
            // arrange
            var id = ObjectId.NewObjectId();
            var session = Mock<Session>();
            session.Stub(s => s.Id).Return(id);
            session.Stub(s => s.Questions).Return(new List<Question>
                                                      {
                                                          QuestionWithVotes(20),
                                                          QuestionWithVotes(24),
                                                          QuestionWithVotes(10),
                                                          QuestionWithVotes(13),
                                                      });
            sessionRepository.Stub(r => r.Load(id)).Return(session);

            // act
            var result = controller.Show(id);
            var model = (SessionViewModel) result.Model;

            // assert
            model.Questions.Count.ShouldBe(4);
            
            model.Questions[0].Votes.ShouldBe(24);
            model.Questions[0].Index.ShouldBe(1);
            
            model.Questions[1].Votes.ShouldBe(20);
            model.Questions[1].Index.ShouldBe(0);
            
            model.Questions[2].Votes.ShouldBe(13);
            model.Questions[2].Index.ShouldBe(3);
            
            model.Questions[3].Votes.ShouldBe(10);
            model.Questions[3].Index.ShouldBe(2);
        }

        Question QuestionWithVotes(int votes)
        {
            var question = Mock<Question>();
            question.Stub(q => q.Votes).Return(votes);
            return question;
        }
    }
}