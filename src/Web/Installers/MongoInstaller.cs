using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MongoDB.Driver;
using Web.Models;
using Web.Repositories.Indexes;

namespace Web.Installers
{
    public class MongoInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container
                .Register(AllTypes.FromThisAssembly()
                              .BasedOn<IIndexCreationTask>()
                              .WithService.Base(),

                          Component.For<MongoServer>()
                              .UsingFactoryMethod(k => MongoServer.Create(k.Resolve<IMongoConfiguration>().ConnectionString))
                              .OnCreate((k, s) => s.Connect())
                              .LifeStyle.Singleton,

                          Component.For<MongoDatabase>()
                              .UsingFactoryMethod(k => k.Resolve<MongoServer>()
                                                           .GetDatabase("PriorityQ")),

                          RegisterCollection<Session>());
        }

        IRegistration RegisterCollection<T>()
        {
            return Component.For(typeof (MongoCollection<T>))
                .UsingFactoryMethod(k => k.Resolve<MongoDatabase>()
                                             .GetCollection<T>(typeof (T).Name));
        }
    }
}