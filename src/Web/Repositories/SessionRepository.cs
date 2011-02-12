using System;
using System.Collections.Generic;
using System.Linq;
using Norm;
using Norm.BSON;
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

        public int IncrementVotesForQuestion(ObjectId id, int questionIndex, string voterId)
        {
            var updateCrit = new Expando();
            updateCrit["_id"] = id;
            updateCrit[string.Format("Questions.{0}.Voters", questionIndex)] = Q.NotEqual(voterId);

            var updateOp = new Expando();
            updateOp[string.Format("Questions.{0}.Votes", questionIndex)] = M.Increment(1);
            updateOp[string.Format("Questions.{0}.Voters", questionIndex)] = M.Push(voterId);

            var sessionCollection = mongoSession.GetCollection<Session>();
            sessionCollection.Update(updateCrit, updateOp, false, false);

            var selectCrit = new Expando();
            selectCrit["_id"] = id;

            var selectSelector = new Expando();
            selectSelector["Questions.Votes"] = 1;

            return sessionCollection
                .Find(selectCrit, new {}, selectSelector, 1, 0)
                .Single().Questions[questionIndex].Votes;
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

        public IList<SessionHeadline> GetAllSessions(int first, int count)
        {
            var fields = new Expando();
            fields["_id"] = 1;
            fields["Headline"] = 1;
            fields["QuestionCount"] = 1;

            return mongoSession
                .GetCollection<Session>()
                .Find(new Expando(), new Expando(), fields, count, first)
                .Select(s => new SessionHeadline
                                 {
                                     Id = s.Id,
                                     Headline = s.Headline,
                                     QuestionCount = s.QuestionCount,
                                 })
                .ToList();
        }

        public long CountAllSessions()
        {
            return mongoSession
                .GetCollection<Session>()
                .Count();
        }
    }
}