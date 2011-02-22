namespace Web.Infrastructure
{
    public interface IAppEnvironmentHelper
    {
        AppEnvironment Current { get; }
    }
}