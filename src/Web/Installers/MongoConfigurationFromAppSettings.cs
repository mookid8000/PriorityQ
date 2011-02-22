using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using Web.Infrastructure;

namespace Web.Installers
{
    public class MongoConfigurationFromAppSettings : IMongoConfiguration
    {
        readonly IAppEnvironmentHelper appEnvironmentHelper;

        public MongoConfigurationFromAppSettings(IAppEnvironmentHelper appEnvironmentHelper)
        {
            var appEnvironment = appEnvironmentHelper.Current;
            this.appEnvironmentHelper = appEnvironmentHelper;
            var sectionPath = string.Format("mongo/{0}", appEnvironment).ToLower();
            var section = (NameValueCollection)ConfigurationManager.GetSection(sectionPath);

            if (section == null)
            {
                var message = string.Format("Cannot instantiate when there's no config section matching {0}", sectionPath);

                throw new InvalidOperationException(message);
            }

            ConnectionString = new Uri(section["connectionString"]);
            DatabaseName = ConnectionString.ToString().Split('/').Last();
        }

        public string DatabaseName { get; set; }

        public Uri ConnectionString { get; set; }
    }
}