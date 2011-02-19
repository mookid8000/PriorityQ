using System;
using NUnit.Framework;
using Shouldly;
using Web.Infrastructure;
using Web.Installers;

namespace Test.Installers
{
    [TestFixture]
    public class TestMongoConfigurationFromAppSettings
    {
        [Test]
        public void DoStuff()
        {
            var settings = new MongoConfigurationFromAppSettings(AppEnvironment.Debug);

            settings.Database.ShouldBe("PriorityQ");
            settings.ConnectionString.ShouldBe(new Uri("mongodb://localhost:27017/PriorityQ"));
        }
    }
}