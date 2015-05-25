using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace GameUI
{
    public class UIRule : AuthorizeAttribute
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        protected GameServices.System serSystem = new GameServices.System();
        protected GameServices.Menu serMenu = new GameServices.Menu();
        protected string MethodType = string.Empty;
        protected Dictionary<string, string> DicQueryString = new Dictionary<string, string>();
        protected Dictionary<string, string> DicForm = new Dictionary<string, string>();
        protected Dictionary<string, string> DicSession = new Dictionary<string, string>();
        protected Dictionary<string, string> DicCookie = new Dictionary<string, string>();
        protected List<DBModel.wgs004> roleList = null;
        protected Dictionary<string, int> DicMethod = new Dictionary<string, int>() { { "GET", 0 }, { "HEAD", 1 }, { "POST", 2 }, { "DEBUG", 3 }, { "PUT", 4 }, { "DELETE", 5 }, { "PATCH", 6 }, { "OPTIONS", 7 } };
        protected string[] AllowAction = new string[] { "Login", "VCode", "CWVCode", "Register", "URLCheck", "Public", "DinpayDone", "YeepayDone", "IpspayDone", "Charge", "TestA", "TestB", "ChangeLine" };
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            object loginUser = httpContext.Session["UILoginUser"];
            if (null != loginUser)
            {
                return true;
            }
            return false;
        }
        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            ActionName = (string)filterContext.RouteData.Values["action"];
            ControllerName = (string)filterContext.RouteData.Values["controller"];
            if (AuthorizeCore(filterContext.HttpContext) != true)
            {
                //if ("Login" == ActionName || "VCode" == ActionName || "CWVCode" == ActionName || "Register" == ActionName || "URLCheck" == ActionName || "Public" == ActionName || "DinpayDone" == ActionName || "YeepayDone" == ActionName || "IpspayDone" == ActionName || "Charge" == ActionName || "TestA" == ActionName || "TestB" == ActionName)
                if (0 < AllowAction.Count(exp=>exp == ActionName))
                {
                }
                else
                {
                    filterContext.HttpContext.Response.StatusCode = 302;
                    filterContext.Result = new RedirectResult("/Login.html");
                }
            }
        }
    }
}