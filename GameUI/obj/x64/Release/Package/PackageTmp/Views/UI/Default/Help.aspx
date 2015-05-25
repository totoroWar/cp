<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var methodType = (string)ViewData["MethodType"];
        var helpClassID = (int)ViewData["HelpClassID"];
        var helpList = (List<DBModel.wgs041>)ViewData["HelpList"];
        var helpClassList = (List<DBModel.SysContentClass>)ViewData["HelpClassList"];
        var helpFirst = (DBModel.wgs041)ViewData["SysCntFirst"];
    %>
    <%if(5 != helpClassID){ %>
    <div class="ui-page-content-body-header tools">
        <div class="left-nav">
            <%if( null != helpClassList)
              {
                  foreach(var item in helpClassList)
                  { 
                   %>
            <a href="/UI/Help?method=help&ckey=<%:item.ID %>" title="<%:item.Name %>" <%=helpClassID == item.ID ? "class='item-select'" : "" %>><%:item.Name %></a>
            <%}
              }/*if*/ %>
        </div>
    </div>
    <div class="blank-line"></div>
    <%} %>
<div class="nhp_main">
    
    <div class="nhp_left">
        <%=Html.AntiForgeryToken() %>
        <%if (null != helpList)
          {
              foreach (var item in helpList)
              { 
              %>
        <ul>
            <li class="show_help" title="<%:item.nc002.Trim() %>" key="<%:item.nc001 %>"><span class="date"><%:item.nc004.ToString("yyyy/MM/dd") %></span><span class="title"><%:item.nc002.Trim() %></span></li>
        </ul>
        <%
        }/*foreach*/
        }/*if*/ %>
    </div>

    <div class="nhp_right">
        <div class="nhp_right_cnt">
        <div class="nty_top text-cent"><span class="nhp_title"><%=helpFirst.nc002.Trim() %></span></div>
        <div class="blank-line"></div>
            <div class="nhp_right_cnt_x"><%=helpFirst.nc003 %></div>
        </div>
    </div>
</div>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".show_help").click(function () {
                var key = $(this).attr("key");
                $.ajax({
                    async: false, timeout: _global_ajax_timeout, type: "POST", cache: false, url: "/UI/Help", data: { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(), key: key },
                    success: function (a, b) {
                        _check_auth(a);
                        var _robj = eval(a);
                        $(".nhp_title").html(_robj.Data.Title);
                        $(".nhp_right_cnt_x").html(_robj.Message);
                    }
                });
            });

            $(".show_help span.title").hover(function () {
                $(this).toggleClass("title_hover");
            });
        });
    </script>
</asp:Content>