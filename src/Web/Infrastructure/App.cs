namespace Web.Infrastructure
{
    public class App
    {
        static readonly AppEnvironmentHelperFromAppSettings Helper = new AppEnvironmentHelperFromAppSettings();
        static readonly AppEnvironment AppEnvironment;

        static App()
        {
            AppEnvironment = Helper.Current;
        }

        public static AppEnvironment Environment
        {
            get { return AppEnvironment; }
        }
    }
}