using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using NUnit.Framework;
using Web.Infrastructure;
using Web.Installers;
using Web.Models;

namespace Test
{
    public abstract class MongoFixture<TMongoService>
    {
        protected TMongoService sut;

        HashSet<string> accessedCollections;
        bool dropCollections;
        MongoServer mongoServer;
        MongoDatabase mongoDatabase;
        MongoConfigurationFromAppSettings settings;

        protected MongoCollection<T> CollectionFor<T>()
        {
            var collectionName = typeof (T).Name;

            accessedCollections.Add(collectionName);

            return mongoDatabase.GetCollection<T>(collectionName);
        }

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            settings = new MongoConfigurationFromAppSettings(AppEnvironment.Debug);
            mongoServer = MongoServer.Create(settings.ConnectionString);
            mongoServer.Connect();
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

            mongoDatabase = mongoServer.GetDatabase(settings.DatabaseName);

            dropCollections = true;

            sut = Create();

            DoSetUp();
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
    }
}