using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Web.Infrastructure;

namespace Test
{
    public abstract class MongoFixture<TMongoService> : FixtureBase
    {
        HashSet<string> accessedCollections;
        MongoSession mongoSession;
        bool dropCollections;

        protected TMongoService sut;

        [SetUp]
        public void SetUp()
        {
            dropCollections = true;
            accessedCollections = new HashSet<string>();
            mongoSession = new MongoSession("test_db", "localhost", 27017);
            mongoSession.CollectionAccessed += collection => accessedCollections.Add(collection);

            sut = Create();

            DoSetUp();
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                DoTearDown();
            } catch {}

            if (dropCollections)
            {
                accessedCollections.ToList()
                    .ForEach(c => mongoSession.DropCollection(c));

                mongoSession.LastError();
            }
        }

        protected virtual void DoTestFixtureSetUp() { }
        protected virtual void DoSetUp() { }
        protected virtual void DoTearDown() { }

        protected abstract TMongoService Create();

        protected MongoSession MongoSession
        {
            get { return mongoSession; }
        }

        protected void DontDropCollections()
        {
            dropCollections = false;
        }
    }
}