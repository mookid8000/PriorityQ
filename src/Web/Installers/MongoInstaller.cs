using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Web.Infrastructure;
using Web.Repositories.Indexes;

namespace Web.Installers
{
    public class MongoInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IMongoSession>()
                                   .UsingFactoryMethod(k => new MongoSession(k.Resolve<MongoConfigurationFromAppSettings>()))
                                   .LifeStyle.Singleton);

            container.Register(AllTypes.FromThisAssembly().BasedOn<IIndexCreationTask>()
                                   .WithService.Base());
        }
    }
}