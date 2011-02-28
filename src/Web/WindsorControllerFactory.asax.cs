using System;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;

namespace Web
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        readonly IWindsorContainer container;

        public WindsorControllerFactory(IWindsorContainer container)
        {
            this.container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null
                       ? base.GetControllerInstance(requestContext, controllerType)
                       : (IController) container.Resolve(controllerType);
        }

        public override void ReleaseController(IController controller)
        {
            container.Release(controller);
        }
    }
}