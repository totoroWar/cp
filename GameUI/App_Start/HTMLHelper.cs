using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace System.Web.Mvc.Html
{
    public static class HTMLExExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="max">系统最大额</param>
        /// <param name="min">系统最小额</param>
        /// <param name="maxp">系统最大点</param>
        /// <param name="p">自己最大点</param>
        /// <returns></returns>
        static public MvcHtmlString GMCalcMaxPrize(this HtmlHelper htmlHelper, decimal max, decimal min, decimal maxp, decimal p)
        {
            var sum = (max - min) * p / maxp;
            var maxPrize = min + sum;
            if (0 >= sum)
            {
                maxPrize = min;
            }
            return MvcHtmlString.Create(string.Format("{0:N4}", maxPrize));
        }
    }
}