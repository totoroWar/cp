using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace GameUI
{
    public class BaseRule : System.Web.Mvc.AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            object loginUser = httpContext.Session["LoginUser"];
            if (null != loginUser)
            {
                return true;
            }
            return false;
        }
        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            if (AuthorizeCore(filterContext.HttpContext) != true)
            {
                filterContext.HttpContext.Response.StatusCode = 302;
                filterContext.Result = new RedirectResult("~/Auth/Login");
                //filterContext.HttpContext.Response.End();
            }
        }
    }
}