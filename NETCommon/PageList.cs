using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NETCommon
{
    public class PageList
    {
        public static string GetPageList(int pageSize, int recordCount, int pageIndex, int pageBlockSize, string pageBlockClass, string pageParamName, string fileName)
        {
            StringBuilder sbResult = new StringBuilder();
            string pageConnect = string.Empty;
            if (Regex.IsMatch(fileName, "\\?"))
            {
                pageConnect = "&";
            }
            else if (Regex.IsMatch(fileName, "@"))
            {
                pageConnect = "&";
            }
            else
            {
                pageConnect = "?";
            }
            fileName = fileName + pageConnect + pageParamName + "=";
            sbResult.AppendFormat("<div class=\"{0}\">", pageBlockClass);

            int currentPageIndex = 0;
            int currentPageStart = 0;
            int pageCount = recordCount / pageSize;
            int pageLess = recordCount % pageSize;

            if (pageLess > 0)
            {
                pageCount += 1;
            }
            if (pageIndex < 0 || pageIndex > pageCount - 1)
            {
                currentPageIndex = 0;
            }
            else
            {
                currentPageIndex = pageIndex;
            }

            /*记录数*/
            //sbResult.AppendFormat("<a href=\"javascript:void(0);\">{0}{1}</a>", "记录数", recordCount);
            sbResult.AppendFormat("<span>{0}{1}</span>", "记录数", recordCount);

            if (0 == recordCount)
            {
                return sbResult.ToString();
            }

            currentPageStart = (currentPageIndex / pageBlockSize);
            currentPageStart = currentPageStart * pageBlockSize;
            /*最前页*/
            if (currentPageIndex == 0)
            {
                sbResult.Append("<a class=\"page_no_link\">最前页</a>");
            }
            else
            {
                sbResult.AppendFormat("<a href=\"{0}{1}\">{2}</a>", fileName, 0, "最前页");
            }
            /*上一页*/
            if (currentPageIndex - 1 >= 0)
            {
                sbResult.AppendFormat("<a href=\"{0}{1}\">{2}</a>", fileName, currentPageIndex - 1, "上一页");
            }
            else
            {
                sbResult.AppendFormat("<a>{0}</a>", "上一页");
            }
            /*前一块*/
            int nbStart = currentPageStart;
            if (currentPageStart != 0 || currentPageStart > pageBlockSize)
            {
                sbResult.AppendFormat("<a href=\"{0}{1}\">{2}</a>", fileName, currentPageIndex - pageBlockSize, "<<");
            }
            else
            {
                sbResult.AppendFormat("<a class=\"page_no_link\">{0}</a>", "<<");
            }
            /*输出块页数*/
            int outIndex = 0;
            for (; currentPageStart < pageCount; currentPageStart++)
            {
                if (currentPageStart == currentPageIndex)
                {
                    sbResult.AppendFormat("<a class=\"page_index_select\">{0}</a>", currentPageStart + 1);
                }
                else
                {
                    sbResult.AppendFormat("<a class='page_index_number' href=\"{0}{1}\">{2}</a>", fileName, currentPageStart, currentPageStart + 1);
                }
                if ((outIndex + 1) % pageBlockSize == 0)
                {
                    break;
                }
                outIndex++;
            }

            /*下一块*/
            if (currentPageStart < pageCount && currentPageStart + 1 < pageCount)
            {
                sbResult.AppendFormat("<a href=\"{0}{1}\">{2}</a>", fileName, currentPageStart + 1, ">>");
            }
            else
            {
                sbResult.AppendFormat("<a class=\"page_no_link\">{0}</a>", ">>");
            }
            /*下一页*/
            if (pageIndex + 1 < pageCount)
            {
                sbResult.AppendFormat("<a href=\"{0}{1}\">{2}</a>", fileName, pageIndex + 1, "下一页");
            }
            else
            {
                sbResult.AppendFormat("<a class=\"page_no_link\">{0}</a>", "下一页");
            }
            /*最尾页*/
            if (pageCount - 1 == currentPageIndex)
            {
                sbResult.AppendFormat("<a class=\"page_no_link\">{0}</a>", "最尾页");
            }
            else
            {
                sbResult.AppendFormat("<a href=\"{0}{1}\">{2}</a>", fileName, pageCount - 1, "最尾页");
            }
            sbResult.Append("</div>");
            return sbResult.ToString();
        }
        public static string GetPageListUI2(int pageSize, int recordCount, int pageIndex, int pageBlockSize, string pageBlockClass, string pageParamName, string fileName)
        {
            StringBuilder sbResult = new StringBuilder();
            string pageConnect = string.Empty;
            if (Regex.IsMatch(fileName, "\\?"))
            {
                pageConnect = "&";
            }
            else if (Regex.IsMatch(fileName, "@"))
            {
                pageConnect = "&";
            }
            else
            {
                pageConnect = "?";
            }
            fileName = fileName + pageConnect + pageParamName + "=";
           sbResult.AppendFormat("<ul>", pageBlockClass);

            int currentPageIndex = 0;
            int currentPageStart = 0;
            int pageCount = recordCount / pageSize;
            int pageLess = recordCount % pageSize;

            if (pageLess > 0)
            {
                pageCount += 1;
            }
            if (pageIndex < 0 || pageIndex > pageCount - 1)
            {
                currentPageIndex = 0;
            }
            else
            {
                currentPageIndex = pageIndex;
            }

            /*记录数*/
           // sbResult.AppendFormat("<li><a href=\"javascript:void(0);\">{0}{1}</a></li>", "记录数", recordCount);
            //sbResult.AppendFormat("<span>{0}{1}</span>", "记录数", recordCount);

            if (0 == recordCount)
            {
                return sbResult.ToString();
            }

            currentPageStart = (currentPageIndex / pageBlockSize);
            currentPageStart = currentPageStart * pageBlockSize;
            /*最前页*/
            if (currentPageIndex == 0)
            {
                sbResult.Append("<li><a class=\"page_no_link\">最前页</a></li>");
            }
            else
            {
                sbResult.AppendFormat("<li><a href=\"{0}{1}\">{2}</a></li>", fileName, 0, "最前页");
            }
            /*上一页*/
            if (currentPageIndex - 1 >= 0)
            {
                sbResult.AppendFormat("<li><a href=\"{0}{1}\">{2}</a></li>", fileName, currentPageIndex - 1, "上一页");
            }
            else
            {
                sbResult.AppendFormat("<li><a>{0}</a></li>", "上一页");
            }
            /*前一块*/
            int nbStart = currentPageStart;
            if (currentPageStart != 0 || currentPageStart > pageBlockSize)
            {
                sbResult.AppendFormat("<li><a href=\"{0}{1}\">{2}</a></li>", fileName, currentPageIndex - pageBlockSize, "<<");
            }
            else
            {
                sbResult.AppendFormat("<li><a class=\"page_no_link\">{0}</a></li>", "<<");
            }
            /*输出块页数*/
            int outIndex = 0;
            for (; currentPageStart < pageCount; currentPageStart++)
            {
                if (currentPageStart == currentPageIndex)
                {
                    sbResult.AppendFormat("<li><a class=\"page_index_select\">{0}</a></li>", currentPageStart + 1);
                }
                else
                {
                    sbResult.AppendFormat("<li><a class='page_index_number' href=\"{0}{1}\">{2}</a></li>", fileName, currentPageStart, currentPageStart + 1);
                }
                if ((outIndex + 1) % pageBlockSize == 0)
                {
                    break;
                }
                outIndex++;
            }

            /*下一块*/
            if (currentPageStart < pageCount && currentPageStart + 1 < pageCount)
            {
                sbResult.AppendFormat("<li><a href=\"{0}{1}\">{2}</a></li>", fileName, currentPageStart + 1, ">>");
            }
            else
            {
                sbResult.AppendFormat("<li><a class=\"page_no_link\">{0}</a></li>", ">>");
            }
            /*下一页*/
            if (pageIndex + 1 < pageCount)
            {
                sbResult.AppendFormat("<li><a href=\"{0}{1}\">{2}</a></li>", fileName, pageIndex + 1, "下一页");
            }
            else
            {
                sbResult.AppendFormat("<li><a class=\"page_no_link\">{0}</a></li>", "下一页");
            }
            /*最尾页*/
            if (pageCount - 1 == currentPageIndex)
            {
                sbResult.AppendFormat("<li><a class=\"page_no_link\">{0}</a></li>", "最尾页");
            }
            else
            {
                sbResult.AppendFormat("<li><a href=\"{0}{1}\">{2}</a></li>", fileName, pageCount - 1, "最尾页");
            }
            sbResult.Append("</ul>");
            return sbResult.ToString();
        }
    }
}















//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace NETCommon
//{
//    public class PageList
//    {
//        /// <summary>
//        /// 分页列表
//        /// </summary>
//        /// <param name="pageSize">页大小</param>
//        /// <param name="recordCount">记录数</param>
//        /// <param name="pageIndex">第N页</param>
//        /// <param name="pageBlockSize">块大小</param>
//        /// <param name="pageBlockClass">块样式</param>
//        /// <param name="pageParamName">参数</param>
//        /// <param name="fileName">文件名</param>
//        /// <returns></returns>
//        public static string GetPageList(int pageSize, int recordCount, int pageIndex, int pageBlockSize, string pageBlockClass, string pageParamName, string fileName)
//        {
//            StringBuilder sbResult = new StringBuilder();
//            string pageConnect = string.Empty;
//            if (Regex.IsMatch(fileName, "\\?"))
//            {
//                pageConnect = "&";
//            }
//            else if (Regex.IsMatch(fileName, "@"))
//            {
//                pageConnect = "&";
//            }
//            else
//            {
//                pageConnect = "?";
//            }
//            fileName = fileName + pageConnect + pageParamName + "=";

//            /*
//             * 
//             */

//            sbResult.AppendFormat("<ul class=\"{0}\">", pageBlockClass);

//            int currentPageIndex = 0;
//            int currentPageStart = 0;
//            int pageCount = recordCount / pageSize;
//            int pageLess = recordCount % pageSize;

//            if (pageLess > 0)
//            {
//                pageCount += 1;
//            }
//            if (pageIndex < 0 || pageIndex > pageCount - 1)
//            {
//                currentPageIndex = 0;
//            }
//            else
//            {
//                currentPageIndex = pageIndex;
//            }

//            /*记录数*/
//            //sbResult.AppendFormat("<a href=\"javascript:void(0);\">{0}{1}</a>", "记录数", recordCount);

//            currentPageStart = (currentPageIndex / pageBlockSize);
//            currentPageStart = currentPageStart * pageBlockSize;


//            /*最前页*/
//            sbResult.AppendFormat("<li>");
//            if (currentPageIndex == 0)
//            {
//                sbResult.Append("首页");
//            }
//            else
//            {
//                sbResult.AppendFormat("<a href=\"{0}{1}\">{2}</a>", fileName, 0, "首页");
//            }
//            sbResult.AppendFormat("</li>");


//            /*上一页*/
//            sbResult.AppendFormat("<li>");
//            if (currentPageIndex - 1 >= 0)
//            {
//                sbResult.AppendFormat("<a href=\"{0}{1}\">{2}</a>", fileName, currentPageIndex - 1, "上一页");
//            }
//            else
//            {
//                sbResult.AppendFormat("上一页");
//            }
//            sbResult.AppendFormat("</li>");

//            /*前一块*/
//            //sbResult.AppendFormat("<li>");
//            //int nbStart = currentPageStart;
//            //if (currentPageStart != 0 || currentPageStart > pageBlockSize)
//            //{
//            //    sbResult.AppendFormat("<a href=\"{0}{1}\">{2}</a>", fileName, currentPageIndex - pageBlockSize, "<<");
//            //}
//            //else
//            //{
//            //    sbResult.AppendFormat("<a class=\"page_no_link\">{0}</a>", "<<");
//            //}
//            //sbResult.AppendFormat("</li>");


//            /*输出块页数*/
//            sbResult.AppendFormat("<li>");
//            int outIndex = 0;
//            for (; currentPageStart < pageCount; currentPageStart++)
//            {
//                if (currentPageStart == currentPageIndex)
//                {
//                    sbResult.AppendFormat("<a class=\"thisclass\">{0}</a>", currentPageStart + 1);
//                }
//                else
//                {
//                    sbResult.AppendFormat("<a href=\"{0}{1}\">{2}</a>", fileName, currentPageStart, currentPageStart + 1);
//                }
//                if ((outIndex + 1) % pageBlockSize == 0)
//                {
//                    break;
//                }
//                outIndex++;
//            }
//            sbResult.AppendFormat("</li>");


//            /*下一块*/
//            //sbResult.AppendFormat("<li>");
//            //if (currentPageStart < pageCount && currentPageStart + 1 < pageCount)
//            //{
//            //    sbResult.AppendFormat("<a href=\"{0}{1}\">{2}</a>", fileName, currentPageStart + 1, ">>");
//            //}
//            //else
//            //{
//            //    sbResult.AppendFormat("<a class=\"page_no_link\">{0}</a>", ">>");
//            //}
//            //sbResult.AppendFormat("</li>");


//            /*下一页*/
//            sbResult.AppendFormat("<li>");
//            if (pageIndex + 1 < pageCount)
//            {
//                sbResult.AppendFormat("<a href=\"{0}{1}\">{2}</a>", fileName, pageIndex + 1, "下一页");
//            }
//            else
//            {
//                sbResult.AppendFormat("下一页");
//            }
//            sbResult.AppendFormat("</li>");


//            /*最尾页*/
//            sbResult.AppendFormat("<li>");
//            if (pageCount - 1 == currentPageIndex || pageIndex == pageCount)
//            {
//                sbResult.AppendFormat("末页");
//            }
//            else
//            {
//                sbResult.AppendFormat("<a href=\"{0}{1}\">{2}</a>", fileName, pageCount - 1, "末页");
//            }
//            sbResult.AppendFormat("</li>");


//            /*记录数*/
//            sbResult.AppendFormat("<span class='pageinfo'>共<strong>{0}</strong>页<strong>{1}</strong>条</span>", pageCount, recordCount);

//            sbResult.Append("</ul>");
//            return sbResult.ToString();
//        }
//    }
//}
