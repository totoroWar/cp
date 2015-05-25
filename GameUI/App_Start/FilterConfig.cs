using System.Web;
using System.Web.Mvc;
namespace GameUI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new GetErrorExceptionFilterAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}