using System;
using System.Collections.Generic;
using System.Linq;
using Norm;
using Norm.BSON;
using Norm.Collections;
using Web.Infrastructure;
using Web.Models;

namespace Web.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        readonly IMongoSession mongoSession;

        public SessionRepository(IMongoSession mongoSession)
        {
            this.mongoSession = mongoSession;
        }

        public void Save(Session session)
        {
            mongoSession
                .GetCollection<Session>()
                .Save(session);
        }

        public Session Load(ObjectId id)
        {
            return mongoSession
                .GetCollection<Session>()
                .FindOne(new {Id = id});
        }

        public void AddQuestion(ObjectId id, Question question)
        {
            var crit = new Expando();
            crit["_id"] = id;
            crit["Questions.Text"] = Q.NotEqual(question.Text);

            var op = new Expando();
            op["Questions"] = M.Push(question);
            op["QuestionCount"] = M.Increment(1);

            mongoSession.GetCollection<Session>().Update(crit, op, false, false);
        }

        public int IncrementVotesForQuestion(ObjectId id, int questionIndex, string voterId)
        {
            var sessionCollection = mongoSession.GetCollection<Session>();

            IncrementVotes(id, questionIndex, voterId, sessionCollection);

            return GetVoteCount(id, questionIndex, sessionCollection);
        }

        int GetVoteCount(ObjectId id, int questionIndex, IMongoCollection<Session> sessionCollection)
        {
            var crit = new Expando();
            crit["_id"] = id;

            var selector = new Expando();
            selector["Questions.Votes"] = 1;

            return sessionCollection
                .Find(crit, new {}, selector, 1, 0)
                .Single().Questions[questionIndex].Votes;
        }

        void IncrementVotes(ObjectId id, int questionIndex, string voterId, IMongoCollection<Session> sessionCollection)
        {
            var crit = new Expando();
            crit["_id"] = id;
            crit[string.Format("Questions.{0}.Voters", questionIndex)] = Q.NotEqual(voterId);

            var op = new Expando();
            op[string.Format("Questions.{0}.Votes", questionIndex)] = M.Increment(1);
            op[string.Format("Questions.{0}.Voters", questionIndex)] = M.Push(voterId);

            sessionCollection.Update(crit, op, false, false);
        }

        public IList<SessionHeadline> GetAllSessions(int first, int count, DateTime expirationTime)
        {
            var crit = new Expando();
            crit["ExpirationTime"] = Q.GreaterOrEqual(expirationTime);

            var fields = new Expando();
            fields["_id"] = 1;
            fields["Headline"] = 1;
            fields["QuestionCount"] = 1;

            return mongoSession
                .GetCollection<Session>()
                .Find(crit, new Expando(), fields, count, first)
                .Select(s => new SessionHeadline
                                 {
                                     Id = s.Id,
                                     Headline = s.Headline,
                                     QuestionCount = s.QuestionCount,
                                 })
                .ToList();
        }

        public long CountAllSessions(DateTime expirationTime)
        {
            var crit = new Expando();
            crit["ExpirationTime"] = Q.GreaterOrEqual(expirationTime);

            return mongoSession
                .GetCollection<Session>()
                .Count(crit);
        }
    }
}