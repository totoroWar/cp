<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<HandleErrorInfo>" %>
<%
    var error = GameServices.MyException.GetInnerException(Model.Exception);
%>
<!DOCTYPE html>
<html>
    <head>
        <title><%:error.Message %></title>
        <meta name="viewport" content="width=device-width" />
        <style>
         body {font-family:"Verdana";font-weight:normal;font-size: .7em;color:black; font-size:12px;} 
         p {font-family:"Verdana";font-weight:normal;color:black;margin-top: -5px}
         b {font-family:"Verdana";font-weight:bold;color:black;margin-top: -5px}
         H1 { font-family:"Verdana";font-weight:normal;font-size:18pt;color:red }
         H2 { font-family:"Verdana";font-weight:normal;font-size:14pt;color:maroon }
         pre {font-family:"Consolas","Lucida Console",Monospace;font-size:11pt;margin:0;padding:0.5em;line-height:14pt}
         .marker {font-weight: bold; color: black;text-decoration: none;}
         .version {color: gray;}
         .error {margin-bottom: 10px;}
         .expandable { text-decoration:underline; font-weight:bold; color:navy; cursor:hand; }
         @media screen and (max-width: 639px) {
          pre { width: 440px; overflow: auto; white-space: pre-wrap; word-wrap: break-word; }
         }
         @media screen and (max-width: 479px) {
          pre { width: 280px; }
         }
        </style>
    </head>

    <body bgcolor="white">

            <span><H1><%:error.Message %><hr width=100% size=1 color=silver></H1>

            <h2> <i><%:error.Message %></i> </h2></span>

            <font face="Arial, Helvetica, Geneva, SunSans-Regular, sans-serif ">

            <b> 说明: </b>执行当前 Web 请求期间，出现未经处理的异常。请检查堆栈跟踪信息，以了解有关该错误以及代码中导致错误的出处的详细信息。

            <br><br>

            <b> 异常详细信息: </b><%:error.Message %><br><br>

            <b>源错误:</b> <br><br>

            <table width=100% bgcolor="#ffffcc">
               <tr>
                  <td>
                      <code><pre>
                          <%:error.Source %>
                      </pre></code>

                  </td>
               </tr>
            </table>

            <br>

            <b> 源文件: </b><%:error.StackTrace %><b> &nbsp;&nbsp; 行: </b> 474
            <br><br>

            <b>堆栈跟踪:</b> <br><br>

            <table width=100% bgcolor="#ffffcc">
               <tr>
                  <td>
                      <code><pre>
                          <%:error.StackTrace %>
                      </pre></code>

                  </td>
               </tr>
            </table>

            <br>

            <hr width=100% size=1 color=silver>

            <b>版本信息:</b>&nbsp;<%:error.TargetSite.Name %>

            </font>

    </body>
</html>
