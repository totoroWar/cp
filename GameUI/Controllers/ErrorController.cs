using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameServices;
namespace GameUI.Controllers
{
    public class ErrorController : Controller
    {
        ISystem serSys = null;
        public ErrorController(ISystem sys)
        {
            serSys = sys;
        }

        public ErrorController()
        { 
        }
        //public ActionResult Index()
        //{
        //    var result = serSys.AddErrorLog((ViewData.Model as HandleErrorInfo).Exception);
        //    ViewData["Message"] = result.Message;
        //    ViewData["ID"] = result.LongData;
        //    ViewData["DateTime"] = DateTime.Now;
        //    return View();
        //}
        public ActionResult P1()
        {
            return View();
        }
        public ActionResult P2()
        {
            return View();
        }
        public ActionResult Index()
        { 
            return View();
        }
    }
}
