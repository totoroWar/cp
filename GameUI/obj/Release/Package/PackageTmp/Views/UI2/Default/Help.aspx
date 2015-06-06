<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI2/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var methodType = (string)ViewData["MethodType"];
    var helpClassID = (int)ViewData["HelpClassID"];
    var helpList = (List<DBModel.wgs041>)ViewData["HelpList"];
    var helpClassList = (List<DBModel.SysContentClass>)ViewData["HelpClassList"];
    var helpFirst = (DBModel.wgs041)ViewData["SysCntFirst"];
%>
<div class="m_body_bg">
<div class="main_box main_tbg">
    <div class="ajax_tbox gg_list_box">
       
        <div class="ajaxg_h3"><span><a class="gg_close" href="#"></a></span>
           <%
               if (helpClassID == 1)
               {
                   Response.Write("帮助中心");
               }else
               {
                   Response.Write("平台活动");
               }
                %>
            </div>
        <div class="ajax_content">
        	<div class="ajax_gg_list">
            <div class="gg_list">
                <%=Html.AntiForgeryToken() %>
                <%if (null != helpList)
                  {
                      foreach (var item in helpList)
                      { 
                      %>
                <h3 class="gg_title"><em></em><%:item.nc002.Trim() %></h3>
                <div style="display: none;" class="gg_content">
                        <p><%=item.nc003%></p>      
                </div>
                <%
                }/*foreach*/
                }/*if*/ %>

                <div class="clear"></div>
            </div>
            </div>
            
        </div>
    </div>
</div>
</div>
    <script>
        jQuery(".gg_list").slide({titCell:"h3.gg_title em",targetCell:".gg_content",trigger:"click",defaultIndex:1,effect:"slideDown",delayTime:300,returnDefault:true,easing:"easeOutCirc"});
        </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
