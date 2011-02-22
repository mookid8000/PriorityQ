using System;
using System.Configuration;

namespace Web.Infrastructure
{
    public class AppEnvironmentHelperFromAppSettings : IAppEnvironmentHelper
    {
        readonly AppEnvironment appEnvironment;

        public AppEnvironmentHelperFromAppSettings()
        {
            appEnvironment = GetAppEnvironment();
        }

        public AppEnvironment Current
        {
            get { return appEnvironment; }
        }

        AppEnvironment GetAppEnvironment()
        {
            var appSettingAsString = ConfigurationManager.AppSettings["Environment"];

            try
            {
                return (AppEnvironment) Enum.Parse(typeof (AppEnvironment), appSettingAsString);
            }
            catch(Exception e)
            {
                var message = string.Format("Could not determine app environment from string: '{0}'", appSettingAsString);
                
                throw new FormatException(message, e);
            }
        }
    }
}