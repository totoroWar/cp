using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using System.Web.Routing;
namespace GameUI
{
    public class UnityFactory : DefaultControllerFactory
    {
        IUnityContainer _container;
        public UnityFactory(IUnityContainer container)
        {
            _container = container;
        }
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType != null)
            {
                
                return _container.Resolve(controllerType) as IController;
            }
            else
            {
                throw new ArgumentNullException("Control Type is not available");
            }
        }
    }
}