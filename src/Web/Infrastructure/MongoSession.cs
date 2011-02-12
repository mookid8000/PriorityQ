using System;
using Norm.BSON;
using Norm.Collections;
using Norm.Responses;

namespace Web.Infrastructure
{
    public class MongoSession : IMongoSession, IDisposable
    {
        public string Database { get; set; }
        public string Host { get; set; }

        public event Action<string> CollectionAccessed = delegate { };

        readonly Norm.Mongo mongo;

        public MongoSession(string database, string host, int port)
        {
            Database = database;
            Host = host;
            mongo = new Norm.Mongo(database, host, port.ToString(), "");
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            CollectionAccessed(typeof(T).Name);

            return mongo.GetCollection<T>();
        }

        public void CreateIndex<T>(Expando key, string name, bool unique)
        {
            mongo.GetCollection<T>().CreateIndex(key, name, unique);
        }

        public void DropCollection(string collectionName)
        {
            mongo.Database.DropCollection(collectionName);
        }

        public void Dispose()
        {
            mongo.Dispose();
        }

        public LastErrorResponse LastError()
        {
            return mongo.LastError();
        }
    }
}