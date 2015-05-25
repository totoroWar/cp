using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using GameServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Collections.Specialized;
using System.Collections;
using System.Runtime.InteropServices;
using MemcachedProviders.Cache;
using _NWC = NETCommon;
namespace GameUI.Controllers
{
    public class CheckData
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        /// <summary>
        /// 返回类型
        /// 如果通过则用回原来的控制器或动作，不通过则用权限或不存在的控制器和动作返回
        /// 1 通过
        /// 2 请求的东西不存在
        /// 3 请求的东西需要权限
        /// </summary>
        public int ResultType { get; set; }
    }
    public class BaseController : Controller
    {
        public string ControllerTheme { get; set; }
        public string ActionName { get; set; }
        public string ActionTrueName { get; set; }
        public string ControllerName { get; set; }
        public bool IsAJAX { get; set; }
        public string IPDataFilePath { get; set; }
        public string SessionID { get; set; }
        protected IGame serGame = null;
        protected IMenu serMenu = null;
        protected IFinance serFinance = null;
        protected ISystem serSystem = null;
        protected IUser serUser = null;
        protected DBModel.wgs016 AMLoginUser { get; set; }
        protected DBModel.wgs012 UILoginUser { get; set; }
        protected Dictionary<string, DBModel.wgs027> DicKV
        {
            get
            {
                if (null != serSystem)
                {
                    return serSystem.GetKeyValueDicList();
                }
                return null;
            }
        }
        protected DBModel.wgs027 GetKV(string key, bool cache)
        {
            if (cache)
            {
                return DicKV[key];
            }
            var result = serSystem.GetKeyValue(key);
            return result;
        }
        protected string MethodType = string.Empty;
        protected Dictionary<string, string> DicQueryString = new Dictionary<string, string>();
        protected Dictionary<string, string> DicForm = new Dictionary<string, string>();
        protected Dictionary<string, string> DicSession = new Dictionary<string, string>();
        protected Dictionary<string, string> DicCookie = new Dictionary<string, string>();
        protected List<DBModel.wgs004> roleList = null;
        protected Dictionary<string, int> DicMethod = null;
        //[DllImport("C:\\Windows\\AppProtected.dll")]
        //public static extern bool ServiceNotify();
        //[DllImport("C:\\Windows\\AppProtected.dll")]
        //public static extern string GetHardwareAuthCode();
        //[DllImport("C:\\Windows\\AppProtected.dll")]
        //public static extern int SendInfoToMyServer(string info);
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            return base.BeginExecuteCore(callback, state);
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var des = filterContext.ActionDescriptor;
            if (des is ReflectedActionDescriptor)
            {
                ActionName = (des as ReflectedActionDescriptor).ActionName;
            }
            else if (des is ReflectedAsyncActionDescriptor)
            {
                ActionName = (des as ReflectedAsyncActionDescriptor).ActionName;
            }
            if (des is ReflectedActionDescriptor)
            {
                ActionTrueName = (des as ReflectedActionDescriptor).MethodInfo.Name;
            }
            else if (des is ReflectedAsyncActionDescriptor)
            {
                ActionTrueName = (des as ReflectedAsyncActionDescriptor).AsyncMethodInfo.Name;
            }
            base.OnActionExecuted(filterContext);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type">1 GET、2 POST、3 COOKIES、4 SESSION</param>
        /// <returns></returns>
        protected object GetSV(string key, int type)
        {
            object result = null;
            switch (type)
            {
                case 1:
                    result = string.IsNullOrEmpty(HttpContext.Request.QueryString[key]) || string.IsNullOrWhiteSpace(HttpContext.Request.QueryString[key]) ? result : HttpContext.Request.QueryString[key];
                    break;
                case 2:
                    result = string.IsNullOrEmpty(HttpContext.Request[key]) || string.IsNullOrWhiteSpace(HttpContext.Request[key]) ? result : HttpContext.Request[key];
                    break;
                case 3:
                    result = HttpContext.Request.Cookies[key] == null ? result : HttpContext.Request.Cookies[key];
                    break;
                case 4:
                    result = HttpContext.Session[key] == null ? result : HttpContext.Session[key];
                    break;
            }
            return result;
        }
        protected void GetServerData(System.Web.Routing.RequestContext requestContext)
        {
            //DicCookie.Clear();
            foreach (string key in requestContext.HttpContext.Request.Cookies.AllKeys)
            {
                if (0 != DicCookie.Where(exp => exp.Key == key).Count())
                {
                    DicCookie[key] = requestContext.HttpContext.Request.Cookies[key].Value;
                }
                else
                {
                    DicCookie.Add(key, requestContext.HttpContext.Request.Cookies[key].Value);
                }
            }
            //DicForm.Clear();
            foreach (string key in requestContext.HttpContext.Request.Form.AllKeys)
            {
                if (0 != DicForm.Where(exp => exp.Key == key).Count())
                {
                    DicForm[key] = requestContext.HttpContext.Request.Form[key];
                }
                else
                {
                    DicForm.Add(key, requestContext.HttpContext.Request.Form[key]);
                }
            }
            //DicSession.Clear();
            foreach (string key in requestContext.HttpContext.Session.Keys)
            {
                object session = requestContext.HttpContext.Session[key];
                if (0 != DicSession.Where(exp => exp.Key == key).Count())
                {
                    DicSession[key] = session == null ? "" : session.ToString();
                }
                else
                {
                    DicSession.Add(key, session == null ? "" : session.ToString());
                }
            }
            //DicQueryString.Clear();
            foreach (string key in requestContext.HttpContext.Request.QueryString.AllKeys)
            {
                if (0 != DicForm.Where(exp => exp.Key == key).Count())
                {
                    DicQueryString[key] = requestContext.HttpContext.Request[key];
                }
                else
                {
                    DicQueryString.Add(key, requestContext.HttpContext.Request[key]);
                }
            }
        }
        protected CheckData CheckAccess(System.Web.Routing.RequestContext requestContext)
        {
            CheckData result = new CheckData();
            /*默认请求*/
            result.ActionName = ActionName;
            result.ControllerName = ControllerName;
            result.ResultType = 1;
            if (AMLoginUser.mu001 == 0)
            {
                if (UILoginUser.u001 == 0)
                {
                    return result;
                }
            }
            string[] menuIDs = (string[])requestContext.HttpContext.Session["PGIDs"];
            /*得到当前请求的一些数据*/
            GetServerData(requestContext);
            /*从缓存中取出菜单*/
            roleList = serMenu.GetMenuListByCache();
            /*找出当前请求的控制器、动作对应的记录*/
            var findRoleList = roleList.Where(exp => exp.sm006 == ControllerName && exp.sm007 == ActionName).ToList();
            /*查找出菜单*/
            var roleCount = findRoleList.Count();
            /*请求存在子级，带method，即当前请求methodType不为空情况下*/
            List<DBModel.wgs004> roleSubList = new List<DBModel.wgs004>();
            if (false == string.IsNullOrEmpty(MethodType))
            {
                var methodTypeString = MethodType;
                roleSubList = findRoleList.Where(exp => exp.sm008 != null && exp.sm008.Contains(methodTypeString)).ToList();
            }
            /*请求不存在菜单中*/
            if (0 == roleCount)
            {
            }
            #region 如果权限存在
            DicMethod = serSystem.GetReqeustTypeS(true);
            if (0 < roleCount)
            {
                var curRole = findRoleList[0];
                DBModel.wgs011 runLog = new DBModel.wgs011();
                runLog.log001 = System.Guid.NewGuid();
                runLog.sm005 = findRoleList[0].sm005;
                runLog.sm002 = findRoleList[0].sm002;
                runLog.sm001 = findRoleList[0].sm001;
                if (0 < roleSubList.Count)
                {
                    runLog.sm005 = roleSubList[0].sm005;
                    runLog.sm002 = roleSubList[0].sm002;
                    runLog.sm001 = roleSubList[0].sm001;
                    curRole = roleSubList[0];
                }
                /*是否记录*/
                if (0 == curRole.sm014)
                {
                    return result;
                }
                runLog.log002 = ControllerName;
                runLog.log003 = ActionName;
                runLog.log004 = DateTime.Now;
                if (ControllerName == "AM" && AMLoginUser.mu001 != 0)
                {
                    runLog.u001 = AMLoginUser.mu001;
                    runLog.u002 = AMLoginUser.mu002.Trim();
                    runLog.u003 = _NWC.GeneralValidate.IsNullOrEmpty(AMLoginUser.mu003) ? "" : AMLoginUser.mu003.Trim();
                }
                else if (ControllerName == "UI" && UILoginUser.u001 != 0)
                {
                    runLog.u001 = UILoginUser.u001;
                    runLog.u002 = UILoginUser.u002.Trim();
                    runLog.u003 = _NWC.GeneralValidate.IsNullOrEmpty(UILoginUser.u003) ? "" : UILoginUser.u003.Trim();
                }
                runLog.log005 = _NWC.RequestHelper.GetUserIP(requestContext.HttpContext.Request);
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
                /*最高管理官记录后其他不需要任何验证*/
                if (1 == AMLoginUser.mu001)
                {
                    return result;
                }
                else if (1 == curRole.sm013 && "AM" == ControllerName)
                {
                    if (0 == menuIDs.Count(exp => exp == curRole.sm001.ToString()))
                    {
                        throw new Exception(string.Format("无权限访问{0}", curRole.sm004));
                    }
                }
            }
            #endregion
            return result;
        }
        protected override IAsyncResult BeginExecute(System.Web.Routing.RequestContext requestContext, AsyncCallback callback, object state)
        {
            SessionID = "A_"+requestContext.HttpContext.Session.SessionID;
            //ServiceNotify();
            /*
             * 后台根据域名
             * 直接放入CACHE
             */
            ControllerTheme = "Default";
            /*
             * 物理地址文件路径
             */
            IPDataFilePath = requestContext.HttpContext.Server.MapPath("/App_Data/qqwry.dat");
            /*
             * 多语言
             */
            System.Globalization.CultureInfo globalCI = new System.Globalization.CultureInfo("zh-CHS");
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture = globalCI;
            /*取得请求方法*/
            MethodType = requestContext.HttpContext.Request["method"] == null ? string.Empty : requestContext.HttpContext.Request["method"].Trim();
            ViewData["MethodType"] = MethodType;
            if (null != requestContext.HttpContext.Session["AMLoginUser"])
            {
                AMLoginUser = (DBModel.wgs016)requestContext.HttpContext.Session["AMLoginUser"];
            }
            else
            {
                AMLoginUser = new DBModel.wgs016();
            }
            if (null != requestContext.HttpContext.Session["UILoginUser"])
            {
                UILoginUser = (DBModel.wgs012)requestContext.HttpContext.Session["UILoginUser"];
            }
            else
            {
                UILoginUser = new DBModel.wgs012();
            }
            ViewData["AMLoginUserID"] = AMLoginUser.mu001;
            ViewData["AMLoginUserAccount"] = AMLoginUser.mu002;
            /*取得控制器和动作*/
            ActionName = (string)requestContext.RouteData.Values["action"];
            ControllerName = (string)requestContext.RouteData.Values["controller"];
            IsAJAX = requestContext.HttpContext.Request.IsAjaxRequest();
            /*后台验证*/
            CheckAccess(requestContext);
            ViewData["ControllerTheme"] = ControllerTheme;
            ViewData["SysDateFormat"] = GetKV("SYS_DATE_FORMAT", true).cfg003;
            ViewData["SysDateTimeFormat"] = GetKV("SYS_DATETIME_FORMAT", true).cfg003;
            ViewData["SysMoneyFormat"] = GetKV("SYS_MONEY_FORMAT", true).cfg003;
            ViewData["SysUIBottom"] = GetKV("SYS_UI_BOTTOM", true).cfg003;
            ViewData["UITheme"] = GetKV("SYS_UI_THEME",true).cfg003;
            ViewData["IPFP"] = IPDataFilePath;
            return base.BeginExecute(requestContext, callback, state);
        }
        public ActionResult ViewExPath(string view, string master, object model)
        {
            if (view == string.Empty)
            {
                view = ActionName;
            }
            if (master == string.Empty && model == null)
            {
                return base.View(ControllerTheme + "/" + view);
            }
            else if (master == string.Empty && model != null)
            {
                return base.View(ControllerTheme + "/" + view, model);
            }
            return base.View(ControllerTheme + "/" + view, master, model);
        }
        public ActionResult ViewEx()
        {
            return base.View(ControllerTheme + "/" + ActionName);
        }
        public ActionResult ViewEx(object model)
        {
            return base.View(ControllerTheme + "/" + ActionName, model);
        }
        public ActionResult ViewEx(string master)
        {
            return base.View(ControllerTheme + "/" + ActionName, "~/Views/Shared/" + ControllerName + "/" + ControllerTheme + "/" + master);
        }
        public ActionResult ViewEx(string master, object model)
        {
            return base.View(ControllerTheme + "/" + ActionName, "~/Views/Shared/" + ControllerName + "/" + ControllerTheme + "/" + master, model);
        }
        public ActionResult ViewEx(string view, string master)
        {
            return base.View(ControllerTheme + "/" + view, "~/Views/Shared/" + ControllerName + "/" + ControllerTheme + "/" + master);
        }
        public ActionResult ViewEx(string view, string master, object model)
        {
            return base.View(ControllerTheme + "/" + view, "~/Views/Shared/" + ControllerName + "/" + master, model);
        }
        public virtual PartialViewResult PartialViewEx(string name, object model)
        {
            if (model == null)
            {
                return base.PartialView(ControllerTheme + "/" + name);
            }
            return base.PartialView(ControllerTheme + "/" + name, model);
        }
          
    }
}
