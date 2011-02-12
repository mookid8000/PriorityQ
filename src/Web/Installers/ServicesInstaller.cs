using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Web.Services;

namespace Web.Installers
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IUserSessionService>().ImplementedBy<CookieBasedUserSessionService>(),
                               Component.For<ICookieJar>().ImplementedBy<AspNetMvcCookieJar>());
        }
    }
}