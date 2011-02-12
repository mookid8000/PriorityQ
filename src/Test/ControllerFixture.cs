using System.Web.Mvc;
using NUnit.Framework;

namespace Test
{
    public abstract class ControllerFixture<TController> : FixtureBase where TController : IController
    {
        protected TController controller;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            DoTestFixtureSetUp();
        }

        [SetUp]
        public void SetUp()
        {
            controller = Create();

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

        protected abstract TController Create();
    }
}