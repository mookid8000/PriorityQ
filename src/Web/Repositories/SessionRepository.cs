using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Web.Models;

namespace Web.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        MongoCollection<Session> coll;

        public SessionRepository(MongoCollection<Session> coll)
        {
            this.coll = coll;
        }

        public void Save(Session session)
        {
            coll.Save(session);
        }

        public Session Load(ObjectId id)
        {
            return coll.FindOne(Query.EQ("_id", id));
        }

        public void AddQuestion(ObjectId id, Question question)
        {
            coll.Update(Query.And(Query.EQ("_id", id),
                                  Query.NE("Questions.Text", question.Text)),
                        Update.Inc("QuestionCount", 1)
                            .PushWrapped("Questions", question),
                        UpdateFlags.None);
        }

        public int IncrementVotesForQuestion(ObjectId id, int questionIndex, string voterId)
        {
            var crit = Query.And(Query.EQ("_id", id),
                                 Query.NE(string.Format("Questions.{0}.Voters", questionIndex), voterId));

            var mod = Update
                .Inc(string.Format("Questions.{0}.Votes", questionIndex), 1)
                .Push(string.Format("Questions.{0}.Voters", questionIndex), voterId);

            coll.Update(crit, mod, UpdateFlags.None);

            var result = coll.FindAs<BsonDocument>(Query.EQ("_id", id))
                .SetFields(Fields.Slice("Questions", questionIndex, 1).Include("Questions.Votes"))
                .Single();

            return result["Questions"].AsBsonArray[0].AsBsonDocument["Votes"].AsInt32;
        }

        public long CountAllSessions(DateTime expirationTime)
        {
            return coll.Count(Query.GTE("ExpirationTime", expirationTime));
        }

        public IList<SessionHeadline> GetAllSessions(int first, int count, DateTime expirationTime)
        {
            return coll.FindAs<BsonDocument>(Query.GTE("ExpirationTime", expirationTime))
                .SetFields(SessionHeadlineDoc.Fields)
                .Skip(first).Take(count)
                .Select(SessionHeadlineDoc.Create)
                .ToList();
        }

        public IList<SessionHeadline> SearchRadius(double latitude, double longitude, double radius, DateTime expirationTime)
        {
            var localtionCriteria = Query.WithinCircle("Location", latitude, longitude, radius);
            var expirationCriteria = Query.GTE("ExpirationTime", expirationTime);

            return coll.FindAs<BsonDocument>(Query.And(localtionCriteria, expirationCriteria))
                .SetFields(SessionHeadlineDoc.Fields)
                .Select(SessionHeadlineDoc.Create)
                .ToList();
        }

        class SessionHeadlineDoc
        {
            public static SessionHeadline Create(BsonDocument d)
            {
                return new SessionHeadline
                           {
                               Id = d["_id"].AsObjectId,
                               Headline = d["Headline"].AsString,
                               QuestionCount = d["QuestionCount", 0].AsInt32,
                           };
            }

            public static string[] Fields
            {
                get { return new[] { "Headline", "QuestionCount" }; }
            }
        }
    }
}