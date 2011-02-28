using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Web.Models;

namespace Web.Repositories.Indexes
{
    public class SessionLocationIndex : IIndexCreationTask
    {
        readonly MongoCollection<Session> collection;

        public SessionLocationIndex(MongoCollection<Session> collection)
        {
            this.collection = collection;
        }

        public void Execute()
        {
            collection.CreateIndex(IndexKeys.GeoSpatial("Location"));
        }
    }
}