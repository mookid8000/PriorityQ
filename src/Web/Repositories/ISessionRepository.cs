using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Web.Models;

namespace Web.Repositories
{
    public interface ISessionRepository : IRepository
    {
        Session Load(ObjectId id);
        void Save(Session session);
        int IncrementVotesForQuestion(ObjectId id, int questionIndex, string voterId);
        void AddQuestion(ObjectId asObjectId, Question question);
        IList<SessionHeadline> GetAllSessions(int first, int count, DateTime expirationTime);
        long CountAllSessions(DateTime expirationTime);
        IList<SessionHeadline> SearchRadius(double latitude, double longitude, double radius, DateTime expirationTime);
    }
}