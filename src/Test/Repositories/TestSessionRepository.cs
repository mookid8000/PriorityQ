using System;
using System.Linq;
using Castle.Core;
using NUnit.Framework;
using Shouldly;
using Web.Models;
using Web.Repositories;

namespace Test.Repositories
{
    [TestFixture]
    public class TestSessionRepository : MongoFixture<SessionRepository>
    {
        protected override SessionRepository Create()
        {
            return new SessionRepository(MongoSession);
        }

        [Test]
        public void CanCountSessions()
        {
            // arrange
            Enumerable.Range(0, 457).ForEach(n => sut.Save(NewSession("something")));

            // act
            var count = sut.CountAllSessions();

            // assert
            count.ShouldBe(457);
        }

        [Test]
        public void ReturnsNewVoteCountWhenIncrementing()
        {
            // arrange
            var newSession = NewSession("something");
            newSession.Questions.Add(new Question("something"));
            sut.Save(newSession);
            var sessionId = newSession.Id;

            // act
            var count1 = sut.IncrementVotesForQuestion(sessionId, 0, Guid.NewGuid().ToString());
            var count2 = sut.IncrementVotesForQuestion(sessionId, 0, Guid.NewGuid().ToString());
            var count3 = sut.IncrementVotesForQuestion(sessionId, 0, Guid.NewGuid().ToString());

            // assert
            count1.ShouldBe(1);
            count2.ShouldBe(2);
            count3.ShouldBe(3);
        }

        [Test, Ignore]
        public void CanGetAllSessions()
        {
            // arrange
            var session1 = NewSession("session1");
            sut.Save(session1);
            var session2 = NewSession("session2");
            sut.Save(session2);
            var session3 = NewSession("session3");
            sut.Save(session3);

            sut.AddQuestion(session1.Id, new Question("q1"));
            sut.AddQuestion(session1.Id, new Question("q2"));

            sut.AddQuestion(session2.Id, new Question("q1"));

            // act
            var headlines = sut.GetAllSessions(0, 10);

            // assert
            headlines.Count.ShouldBe(3);
            headlines.Single(h => h.Headline == "session1").QuestionCount.ShouldBe(2);
            headlines.Single(h => h.Headline == "session2").QuestionCount.ShouldBe(1);
            headlines.Single(h => h.Headline == "session3").QuestionCount.ShouldBe(0);
        }

        [Test]
        public void SessionKeepsTrackOfQuestionCountWhenAddingQuestions()
        {
            // arrange
            var session = NewSession("MongoDB from a .NET dude's angle");
            var id = session.Id;
            sut.Save(session);

            // act
            sut.AddQuestion(id, new Question("question one!"));
            sut.AddQuestion(id, new Question("question one!"));
            sut.AddQuestion(id, new Question("question two!"));
            sut.AddQuestion(id, new Question("question three!"));

            // assert
            var loadedSession = sut.Load(id);
            loadedSession.QuestionCount.ShouldBe(3);
        }

        Session NewSession(string headline)
        {
            return new Session(headline, Guid.NewGuid().ToString());
        }

        [Test]
        public void CannotCastMoreThanOneVotePerId()
        {
            // arrange
            var session = NewSession("MongoDB from a .NET dude's angle");
            session.Questions.Add(new Question("What about this?"));
            var id = session.Id;
            sut.Save(session);
            var voterId = Guid.NewGuid().ToString();

            // act
            sut.IncrementVotesForQuestion(id, 0, voterId);
            sut.IncrementVotesForQuestion(id, 0, voterId);
            sut.IncrementVotesForQuestion(id, 0, voterId);

            // assert
            var loadedSession = sut.Load(id);
            loadedSession.Questions[0].Votes.ShouldBe(1);
        }

        [Test]
        public void AddQuestionIdempotencyAlsoWorksWhenExistingQuestionHasVotes()
        {
            // arrange
            var session = NewSession("MongoDB from a .NET dude's angle");
            var id = session.Id;
            sut.Save(session);

            // act
            sut.AddQuestion(id, new Question("What about this?"));
            sut.IncrementVotesForQuestion(id, 0, Guid.NewGuid().ToString());
            sut.IncrementVotesForQuestion(id, 0, Guid.NewGuid().ToString());
            sut.IncrementVotesForQuestion(id, 0, Guid.NewGuid().ToString());
            sut.AddQuestion(id, new Question("What about this?"));

            // assert
            var loadedSession = sut.Load(id);
            loadedSession.Questions.Count.ShouldBe(1);
            loadedSession.Questions[0].Text.ShouldBe("What about this?");
        }

        [Test]
        public void AddingQuestionIsIdempotent()
        {
            // arrange
            var session = NewSession("MongoDB from a .NET dude's angle");
            session.Questions.Add(new Question("How do you provide durability?"));
            session.Questions.Add(new Question("How to administer db settings?"));
            session.Questions.Add(new Question("Uhm, what?"));
            var id = session.Id;
            sut.Save(session);

            // act
            sut.AddQuestion(id, new Question("What about this?"));
            sut.AddQuestion(id, new Question("What about this?"));
            sut.AddQuestion(id, new Question("What about this?"));

            // assert
            var loadedSession = sut.Load(id);
            loadedSession.Questions.Count.ShouldBe(4);
            loadedSession.Questions[3].Text.ShouldBe("What about this?");
        }

        [Test]
        public void CanAddQuestionToExistingSession()
        {
            // arrange
            var session = NewSession("MongoDB from a .NET dude's angle");
            session.Questions.Add(new Question("How do you provide durability?"));
            session.Questions.Add(new Question("How to administer db settings?"));
            session.Questions.Add(new Question("Uhm, what?"));
            var id = session.Id;
            sut.Save(session);

            // act
            sut.AddQuestion(id, new Question("What about this?"));
            sut.AddQuestion(id, new Question("What about that?"));

            // assert
            var loadedSession = sut.Load(id);
            loadedSession.Questions.Count.ShouldBe(5);
            loadedSession.Questions[3].Text.ShouldBe("What about this?");
            loadedSession.Questions[4].Text.ShouldBe("What about that?");
        }

        [Test]
        public void CanSaveAndLoadSession()
        {
            // arrange
            var session = NewSession("MongoDB from a .NET dude's angle");
            session.Questions.Add(new Question("How do you provide durability?"));
            session.Questions.Add(new Question("How to administer db settings?"));
            session.Questions.Add(new Question("Uhm, what?"));

            var id = session.Id;

            // act
            sut.Save(session);

            // assert
            var loadedSession = sut.Load(id);

            loadedSession.Headline.ShouldBe("MongoDB from a .NET dude's angle");
            
            loadedSession.Questions[0].Text.ShouldBe("How do you provide durability?");
            loadedSession.Questions[0].Votes.ShouldBe(0);
            
            loadedSession.Questions[1].Text.ShouldBe("How to administer db settings?");
            loadedSession.Questions[1].Votes.ShouldBe(0);
            
            loadedSession.Questions[2].Text.ShouldBe("Uhm, what?");
            loadedSession.Questions[2].Votes.ShouldBe(0);
        }

        [Test]
        public void CanIncrementVotesOnQuestion()
        {
            // arrange
            var session = NewSession("MongoDB from a .NET dude's angle");
            session.Questions.Add(new Question("How do you provide durability?"));
            session.Questions.Add(new Question("How to administer db settings?"));
            session.Questions.Add(new Question("Uhm, what?"));

            var id = session.Id;
            sut.Save(session);

            // act
            sut.IncrementVotesForQuestion(id, 1, Guid.NewGuid().ToString());
            sut.IncrementVotesForQuestion(id, 2, Guid.NewGuid().ToString());
            sut.IncrementVotesForQuestion(id, 2, Guid.NewGuid().ToString());

            // assert
            var loadedSession = sut.Load(id);

            loadedSession.Questions[0].Votes.ShouldBe(0);
            loadedSession.Questions[1].Votes.ShouldBe(1);
            loadedSession.Questions[2].Votes.ShouldBe(2);
        }

        [Test]
        public void NewCountIsReturnedWhenIncrementing()
        {
            // arrange
            var session = NewSession("MongoDB from a .NET dude's angle");
            session.Questions.Add(new Question("How do you provide durability?"));

            var id = session.Id;
            sut.Save(session);

            // act
            sut.IncrementVotesForQuestion(id, 0, Guid.NewGuid().ToString());
            sut.IncrementVotesForQuestion(id, 0, Guid.NewGuid().ToString());
            sut.IncrementVotesForQuestion(id, 0, Guid.NewGuid().ToString());
            var votes = sut.IncrementVotesForQuestion(id, 0, Guid.NewGuid().ToString());

            // assert
            var loadedSession = sut.Load(id);

            loadedSession.Questions[0].Votes.ShouldBe(4);
        }
    }
}