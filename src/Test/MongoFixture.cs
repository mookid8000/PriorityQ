using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MongoDB.Driver;
using NUnit.Framework;
using Web.Infrastructure;
using Web.Installers;
using Web.Models;
using Web.Repositories.Indexes;

namespace Test
{
    public abstract class MongoFixture<TMongoService> : IAppEnvironmentHelper
    {
        protected TMongoService sut;

        HashSet<string> accessedCollections;
        bool dropCollections;
        MongoServer mongoServer;
        MongoDatabase mongoDatabase;
        //MongoConfigurationFromAppSettings settings;

        protected MongoCollection<T> CollectionFor<T>()
        {
            var collectionName = typeof (T).Name;

            accessedCollections.Add(collectionName);

            return mongoDatabase.GetCollection<T>(collectionName);
        }

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            mongoServer = MongoServer.Create(MongoUri());
            mongoServer.Connect();
        }

        Uri MongoUri()
        {
            return new Uri(ConfigurationManager.AppSettings["MONGOHQ_URL"]);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            mongoServer.Disconnect();
        }

        [SetUp]
        public void SetUp()
        {
            FakeDoc.Reset();

            accessedCollections = new HashSet<string>();

            mongoDatabase = mongoServer.GetDatabase(MongoUri().LocalPath.TrimStart('/'));

            dropCollections = true;

            ExecuteIndexCreationTasks();

            sut = Create();

            DoSetUp();
        }

        void ExecuteIndexCreationTasks()
        {
            var indexTasks =
                new[]
                    {
                        new SessionLocationIndex(CollectionFor<Session>()),
                    };

            foreach(var task in indexTasks)
            {
                task.Execute();
            }
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                DoTearDown();
            }
            catch
            {
            }

            if (dropCollections)
            {
                accessedCollections.ToList()
                    .ForEach(collectionName => mongoDatabase.DropCollection(collectionName));

                accessedCollections.Clear();
            }
        }

        protected virtual void DoTestFixtureSetUp() { }
        protected virtual void DoSetUp() { }
        protected virtual void DoTearDown() { }

        protected abstract TMongoService Create();

        protected void DontDropCollections()
        {
            dropCollections = false;
        }

        public AppEnvironment Current
        {
            get { return GetAppEnvironment(); }
        }

        AppEnvironment GetAppEnvironment()
        {
            var environmentAsString = ConfigurationManager.AppSettings["Environment"];

            return (AppEnvironment) Enum.Parse(typeof (AppEnvironment), environmentAsString);
        }
    }
}