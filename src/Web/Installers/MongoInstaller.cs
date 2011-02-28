using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MongoDB.Driver;
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
                              .UsingFactoryMethod(CreateMongoServer)
                              .OnCreate((k, s) => s.Connect())
                              .LifeStyle.Singleton,

                          Component.For<MongoDatabase>().UsingFactoryMethod(GetMongoDatabase),

                          Component.For<ILazyComponentLoader>()
                              .ImplementedBy<MongoCollectionComponentLoader>()
                              .LifeStyle.Singleton);
        }

        MongoServer CreateMongoServer(IKernel kernel)
        {
            return MongoServer.Create(kernel.Resolve<IMongoConfiguration>().ConnectionString);
        }

        MongoDatabase GetMongoDatabase(IKernel kernel)
        {
            return kernel.Resolve<MongoServer>()
                .GetDatabase(kernel.Resolve<IMongoConfiguration>().DatabaseName);
        }
    }

    public class MongoCollectionComponentLoader : ILazyComponentLoader
    {
        ConcurrentDictionary<Type, MethodInfo> factoryMethodCache = new ConcurrentDictionary<Type, MethodInfo>();

        public IRegistration Load(string key, Type service, IDictionary arguments)
        {
            var requestedServiceIsMongoCollection = service.IsGenericType
                                                    && service.GetGenericTypeDefinition() == typeof (MongoCollection<>);

            if (!requestedServiceIsMongoCollection)
                return null;

            return Component.For(service).UsingFactoryMethod(k => ResolveCollection(k, service));
        }

        object ResolveCollection(IKernel kernel, Type service)
        {
            var documentType = service.GetGenericArguments().Single();
            var mongoDatabase = kernel.Resolve<MongoDatabase>();

            var collectionMethodInfo = GetCollectionGetter(mongoDatabase, documentType);
            
            return collectionMethodInfo.Invoke(mongoDatabase, new object[] {documentType.Name});
        }

        MethodInfo GetCollectionGetter(MongoDatabase mongoDatabase, Type documentType)
        {
            MethodInfo methodInfoToReturn;

            if (!factoryMethodCache.TryGetValue(documentType, out methodInfoToReturn))
            {
                methodInfoToReturn = mongoDatabase.GetType()
                    .GetMethods()
                    .Single(m => m.Name == "GetCollection"
                                 && m.IsGenericMethod
                                 && m.GetParameters().Length == 1
                                 && m.GetParameters()
                                        .Single().ParameterType ==
                                 typeof (string))
                    .MakeGenericMethod(documentType);

                factoryMethodCache.TryAdd(documentType, methodInfoToReturn);
            }

            return methodInfoToReturn;
        }
    }
}