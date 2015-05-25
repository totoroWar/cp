using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace GameUI
{
    public class CheckData
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public int ResultType { get; set; }
    }
    public class AMRule : AuthorizeAttribute
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
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            object loginUser = httpContext.Session["AMLoginUser"];
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
                if ("Login" == ActionName || "VCode" == ActionName)
                {
                }
                else
                {
                    filterContext.HttpContext.Response.StatusCode = 302;
                    filterContext.Result = new RedirectResult("/AM/Login");
                }
            }
        }
        protected void GetServerData(System.Web.Mvc.AuthorizationContext requestContext)
        {
            DicCookie.Clear();
            foreach (string key in requestContext.HttpContext.Request.Cookies.AllKeys)
            {
                DicCookie.Add(key, requestContext.HttpContext.Request.Cookies[key].Value);
            }
            DicForm.Clear();
            foreach (string key in requestContext.HttpContext.Request.Form.AllKeys)
            {
                DicForm.Add(key, requestContext.HttpContext.Request.Form[key]);
            }
            DicSession.Clear();
            foreach (string key in requestContext.HttpContext.Session.Keys)
            {
                object session = requestContext.HttpContext.Session[key];
                DicSession.Add(key, session == null ? "" : session.ToString());
            }
            DicQueryString.Clear();
            foreach (string key in requestContext.HttpContext.Request.QueryString.AllKeys)
            {
                DicQueryString.Add(key, requestContext.HttpContext.Request[key]);
            }
        }
        protected CheckData CheckAccess(System.Web.Mvc.AuthorizationContext requestContext)
        {
            CheckData result = new CheckData();
            result.ActionName = ActionName;
            result.ControllerName = ControllerName;
            result.ResultType = 1;
            GetServerData(requestContext);
            roleList = serMenu.GetMenuListByCache();
            var findRoleList = roleList.Where(exp => exp.sm006 == ControllerName && exp.sm007 == ActionName).ToList();
            var roleCount = findRoleList.Count();
            if (0 == roleCount)
            {
                result.ResultType = 2;
                result.ControllerName = "Error";
                result.ActionName = "P1";
                return result;
            }
            if (0 < roleCount)
            {
                #region WriteLog
                if (1 == findRoleList[0].sm014)
                {
                    DBModel.wgs011 runLog = new DBModel.wgs011();
                    runLog.log001 = System.Guid.NewGuid();
                    runLog.sm005 = findRoleList[0].sm005;
                    runLog.sm002 = findRoleList[0].sm002;
                    runLog.sm001 = findRoleList[0].sm001;
                    runLog.log002 = ControllerName;
                    runLog.log003 = ActionName;
                    runLog.log004 = DateTime.Now;
                    runLog.log005 = requestContext.HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                    runLog.log006 = int.Parse(requestContext.HttpContext.Request.ServerVariables["REMOTE_PORT"]);
                    runLog.log011 = DicMethod[requestContext.HttpContext.Request.ServerVariables["REQUEST_METHOD"]];
                    runLog.log012 = requestContext.HttpContext.Request.ServerVariables["HTTP_REFERER"];
                    runLog.log013 = requestContext.HttpContext.Request.ServerVariables["URL"];
                    runLog.log014 = requestContext.HttpContext.Request.ServerVariables["SERVER_NAME"];
                    if ("POST" == requestContext.HttpContext.Request.ServerVariables["REQUEST_METHOD"])
                    {
                        runLog.log007 = Newtonsoft.Json.JsonConvert.SerializeObject(DicForm);
                    }
                    if (0 < DicQueryString.Count)
                    {
                        runLog.log009 = Newtonsoft.Json.JsonConvert.SerializeObject(DicQueryString);
                    }
                    if (0 < DicCookie.Count())
                    {
                        runLog.log008 = Newtonsoft.Json.JsonConvert.SerializeObject(DicCookie);
                    }
                    if (0 < DicSession.Count())
                    {
                        runLog.log010 = Newtonsoft.Json.JsonConvert.SerializeObject(DicSession);
                    }
                    serSystem.AddMenuLog(runLog);
                }
                #endregion
                if (string.IsNullOrEmpty(MethodType) && 1 == roleCount)
                {
                    if (1 == roleList[0].sm013)
                    {
                        string sessionRole = (string)requestContext.HttpContext.Session["AMLoginRule"];
                        if (string.IsNullOrEmpty(sessionRole))
                        {
                            result.ResultType = 3;
                            result.ControllerName = "Error";
                            result.ActionName = "P2";
                            return result;
                        }
                    }
                    else
                    {
                    }
                }
            }
            return result;
        }
    }
}