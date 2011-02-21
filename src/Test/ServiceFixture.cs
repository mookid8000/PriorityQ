using NUnit.Framework;
using Web.Models;

namespace Test
{
    public abstract class ServiceFixture<TService> : FixtureBase
    {
        protected TService sut;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            DoTestFixtureSetUp();
        }

        [SetUp]
        public void SetUp()
        {
            FakeDoc.Reset();

            sut = Create();

            DoSetUp();
        }

        [TearDown]
        public void TearDown()
        {
            DoTearDown();
        }

        protected virtual void DoTestFixtureSetUp() { }
        protected virtual void DoSetUp() { }
        protected virtual void DoTearDown() { }

        protected abstract TService Create();
    }
}