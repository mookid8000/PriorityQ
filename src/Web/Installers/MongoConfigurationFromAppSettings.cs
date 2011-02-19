using System;
using System.Collections.Specialized;
using System.Configuration;
using Web.Infrastructure;

namespace Web.Installers
{
    public class MongoConfigurationFromAppSettings
    {
        public MongoConfigurationFromAppSettings(AppEnvironment appEnvironment)
        {
            var sectionPath = string.Format("mongo/{0}", appEnvironment).ToLower();
            var section = (NameValueCollection) ConfigurationManager.GetSection(sectionPath);

            if (section == null)
            {
                var message = string.Format("Cannot instantiate when there's no config section matching {0}", sectionPath);
                
                throw new InvalidOperationException(message);
            }

            Database = section["database"];
            Host = section["host"];
            Port = int.Parse(section["port"]);
            CollectionPrefix = section["collection_prefix"];
        }

        public string Database { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string CollectionPrefix { get; set; }
    }
}