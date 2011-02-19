using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Web.Infrastructure;

namespace Web.Installers
{
    public class ConfigurationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<AppEnvironment>()
                                   .UsingFactoryMethod(k => new AppEnvironmentHelper().Current)
                                   .LifeStyle.Singleton);
        }
    }
}