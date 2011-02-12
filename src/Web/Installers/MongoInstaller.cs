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
            var database = "PriorityQ";
            var host = "localhost";
            var port = 27017;

            container.Register(Component.For<IMongoSession>()
                                   .UsingFactoryMethod(k => new MongoSession(database, host, port))
                                   .LifeStyle.Singleton);

            container.Register(AllTypes.FromThisAssembly().BasedOn<IIndexCreationTask>()
                                   .WithService.Base());
        }
    }
}