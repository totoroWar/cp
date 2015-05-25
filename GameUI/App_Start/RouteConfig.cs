using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace GameUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes, string ControllerName)
        {
            string UI = string.IsNullOrEmpty(ControllerName) ? "UI" : ControllerName;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "fileBetUpload",
                url: "filebetupload.html",
                defaults: new { controller = UI, action = "fileBetUpload" }
                );
            routes.MapRoute(
                name: "HistoryResult",
                url: "HistoryResult.html",
                defaults: new { controller = UI, action = "HistoryResult" }
            );
            routes.MapRoute(
                name: "JustGame",
                url: "Game_{gameID}_{gameClassID}.html",
                defaults: new { controller = UI, action = "Play" }
            );
            routes.MapRoute(
                name: "StaticRegister",
                url: "Register.html",
                defaults: new { controller = UI, action = "Register" }
            );
            routes.MapRoute(
                name: "StaticLogin",
                url: "Login.html",
                defaults: new { controller = UI, action = "Login" }
            );
            routes.MapRoute(
                name: "GameResult",
                url: "GameResult.html",
                defaults: new { controller = UI, action = "GameResult" }
            );
            routes.MapRoute(
                name: "Promotion",
                url: "Promotion.html",
                defaults: new { controller = UI, action = "Help", ckey = 5}
                );
            routes.MapRoute(
                name: "Public",
                url: "Public.html",
                defaults: new { controller = UI, action = "Public", ckey = 5 }
                );
            routes.MapRoute(
                name: "PublicDinPayOKASPX",
                url: "PublicDinPayOK.aspx",
                defaults: new { controller = UI, action = "Public", method = "dinpayBankOK" }
                );
            routes.MapRoute(
                name: "PublicDinPayOK",
                url: "PublicDinPayOK.html",
                defaults: new { controller = UI, action = "Public", method = "dinpayBankOK" }
                );
            routes.MapRoute(
                name: "Combine",
                url: "Combine.html",
                defaults: new { controller = UI, action = "Combine" }
                );
            routes.MapRoute(
                name: "Notify",
                url: "Notify.html",
                defaults: new { controller = UI, action = "Notify" }
                );
            routes.MapRoute(
                name: "PlayInfo",
                url: "PlayInfo.html",
                defaults: new { controller = UI, action = "PlayInfo" }
                );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = UI, action = "Index" }
            );

        }
    }
}