using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MongoDB.Bson;
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
            var sessionId = ObjectId.GenerateNewId();

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

            var form = result.Model.As<CreateSessionForm>();
            form.Headline.ShouldBe("");
        }

        [Test]
        public void CanSaveNewSession()
        {
            // arrange
            var now = DateTime.Now;
            var form = new CreateSessionForm
                           {
                               Headline = "Hello there!",
                               Lat = "2.2",
                               Lng = "3.2",
                               ExpirationDate = now.ToShortDateString(),
                               ExpirationTime = TimeSpan.FromHours(1).ToString(),
                               LocalTime = now.ToString(),
                           };
            var newObjectId = ObjectId.GenerateNewId();
            FakeDoc.StubId(newObjectId);
            controller.CurrentUserSession = new UserSession("someone");
            var sessionChecked = false;

            sessionRepository.Stub(r => r.Save(Arg<Session>.Is.Anything))
                .Callback<Session>(s =>
                                       {
                                           s.Headline.ShouldBe("Hello there!");
                                           s.CreatedBy.ShouldBe("someone");
                                           s.Location.Lat.ShouldBe(2.2);
                                           s.Location.Lng.ShouldBe(3.2);
                                           sessionChecked = true;
                                           return true;
                                       });

            // act
            var actionResult = controller.New(form);

            // assert
            var result = actionResult.As<RedirectToRouteResult>();

            result.RouteValues["controller"].ShouldBe("session");
            result.RouteValues["action"].ShouldBe("show");
            result.RouteValues["id"].ShouldBe(newObjectId);
            sessionChecked.ShouldBe(true);

            // OMGWTF?!?
            //System.Reflection.AmbiguousMatchException : Ambiguous match found.
            //at System.RuntimeType.GetMethodImpl(String name, BindingFlags bindingAttr, Binder binder, CallingConventions callConv, Type[] types, ParameterModifier[] modifiers)
            //at System.Type.GetMethod(String name)
            //at Rhino.Mocks.Constraints.LambdaConstraint.Eval(Object obj)
            //at Rhino.Mocks.Expectations.ConstraintsExpectation.DoIsExpected(Object[] args)
            //at Rhino.Mocks.Expectations.AbstractExpectation.IsExpected(Object[] args)
            //at Rhino.Mocks.RhinoMocksExtensions.AssertWasCalled(T mock, Action`1 action, Action`1 setupConstraints)
            //at Rhino.Mocks.RhinoMocksExtensions.AssertWasCalled(T mock, Action`1 action)
            //at Test.Controllers.TestSessionController.CanSaveNewSession() in TestSessionController.cs: line 90 
            //            sessionRepository.AssertWasCalled(r => r.Save(Arg<Session>.Matches(s => s.Headline == "Hello there!"
            //                                                                                    && s.CreatedBy == "someone"
            //                                                                                    && s.Location.Lat == 2.2
            //                                                                                    && s.Location.Lng == 3.2)));
        }

        [Test]
        public void OrdersQuestionsByVotes()
        {
            // arrange
            var id = ObjectId.GenerateNewId();
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

            controller.CurrentUserSession = new UserSession("joe");

            // act
            var result = controller.Show(id.ToString());
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
            question.Stub(q => q.Voters).Return(new List<string>());
            return question;
        }
    }
}