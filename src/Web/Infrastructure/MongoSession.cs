using System;
using Norm;
using Norm.BSON;
using Norm.Collections;
using Norm.Responses;
using Web.Installers;

namespace Web.Infrastructure
{
    public class MongoSession : IMongoSession, IDisposable
    {
        public string Database { get; set; }
        public string Host { get; set; }

        public event Action<string> CollectionAccessed = delegate { };

        readonly Mongo mongo;

        public MongoSession(string database, string host, int port)
        {
            Database = database;
            Host = host;
            mongo = new Mongo(database, host, port.ToString(), "");
        }

        public MongoSession(MongoConfigurationFromAppSettings mongoConfigurationFromAppSettings)
            : this(mongoConfigurationFromAppSettings.Database,
            mongoConfigurationFromAppSettings.Host,
            mongoConfigurationFromAppSettings.Port)
        {
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