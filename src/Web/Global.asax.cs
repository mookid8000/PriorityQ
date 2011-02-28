using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Web.Repositories.Indexes;

namespace Web
{
    public class MvcApplication : HttpApplication
    {
        IWindsorContainer container;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Default",
                            "{controller}/{action}/{id}",
                            new {controller = "Home", action = "Index", id = UrlParameter.Optional});

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            container = new WindsorContainer().Install(FromAssembly.This());

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));

            RunIndexCreationTasks();
        }

        void RunIndexCreationTasks()
        {
            var indexCreationTasks = container.ResolveAll<IIndexCreationTask>();

            foreach(var task in indexCreationTasks)
            {
                task.Execute();

                container.Release(task);
            }
        }

        protected void Application_End()
        {
            container.Dispose();
        }
    }
}