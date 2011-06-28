using System;
using NUnit.Framework;

namespace Test.Installers
{
    [TestFixture]
    public class TestMongoUri : FixtureBase
    {
        [Test]
        public void CanDeduceDatabaseNameFromUri()
        {
            var uri = new Uri("mongodb://mhg:omfgThisIsLong@flame.mongohq.com:27071/PriorityQ_test");
            
            Assert.AreEqual("PriorityQ_test", uri.LocalPath.TrimStart('/'));
        }
    }
}