using GameUI.Controllers;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GameServices;
namespace GameUI
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        public override void Init()
        {
            base.Init();
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            MvcHandler.DisableMvcResponseHeader = true;
        }
        protected void Session_End(object sender, EventArgs e)
        { 
        }
        private void InitUnity()
        {
            //Initializing  the Unity container
            IUnityContainer container = new UnityContainer();
            IControllerFactory factory = new UnityFactory(container);
            //setting the Controller factory for the Unity Container
            ControllerBuilder.Current.SetControllerFactory(factory);
            //container.RegisterType<ICJLSoft, CJLSoft>();
            //container.RegisterType<ICJLSoft2, CJLSoft2>();
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Containers.Default.Configure(container);
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            InitUnity();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            string ControllerName = NETCommon.ConfigHelper.GetConfigString("ControllerName");
            
            RouteConfig.RegisterRoutes(RouteTable.Routes, ControllerName);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BundleTable.EnableOptimizations = true;
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            if (true == NETCommon.ConfigHelper.GetConfigBool("ErrorToDB"))
            {
                //Exception error = Server.GetLastError();
                //var httpContext = ((MvcApplication)sender).Context;
                //var httpErrorCode = (error is HttpException) ? (error as HttpException).GetHttpCode() : 500;
                //httpContext.ClearError();
                //httpContext.Response.Clear();
                //httpContext.Response.StatusCode = 200;
                //HandleErrorInfo errorModel = null;
                //RouteData routeData = new RouteData();
                //routeData.Values["controller"] = "Error";
                //routeData.Values["action"] = "Index";
                //errorModel = new HandleErrorInfo(error, "Error", "Index");
                //var errorController = new ErrorController(new GameServices.System());
                //errorController.ViewData.Model = errorModel;
                //(errorController as IController).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
                var ex = Server.GetLastError(); 
                var httpStatusCode = (ex is HttpException) ? (ex as HttpException).GetHttpCode() : 500; //这里仅仅区分两种错误  
                var httpContext = ((MvcApplication)sender).Context;
                httpContext.ClearError();
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = httpStatusCode;
                var shouldHandleException = true;
                HandleErrorInfo errorModel;
                
                var routeData = new RouteData();
                routeData.Values["controller"] = "Error";

                switch (httpStatusCode)
                {
                    case 404: 
                        routeData.Values["action"] = "P1";
                        errorModel = new HandleErrorInfo(new Exception(string.Format("页面不存在！", httpContext.Request.UrlReferrer), ex), "Error", "P1");
                        break;

                    default:
                         routeData.Values["action"] = "Index";  
                        Exception exceptionToReplace = null; //这里使用了EntLib的异常处理模块的一些功能  
                        errorModel = new HandleErrorInfo(new Exception("系统故障", ex), "Error", "Index"); 
                        break;
                }

                
                var controller = new ErrorController(); ;
                controller.ViewData.Model = errorModel; //通过代码路由到指定的路径  
               ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
                
            }
        }
    }
}