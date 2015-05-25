<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content3" ContentPlaceHolderID="headContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $(".show_notify").click(function ()
            {
                var key = $(this).attr("key");
                $.ajax({
                    async: false, timeout: _global_ajax_timeout, type: "POST", cache: false, url: "/Notify.html", data: { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(), key: key },
                    success: function (a, b) {
                        _check_auth(a);
                        var _robj = eval(a);
                        $(".nty_right_cnt_x").html(_robj.Message);
                        $(".nty_title").html(_robj.Data.Title);
                        $(".nty_time").html(_robj.Data.Time);
                    }
                });
            });
            $(".show_notify span.title").hover(function ()
            {
                $(this).toggleClass("title_hover");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">平台大厅</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var notifyList = (List<DBModel.wgs040>)ViewData["NtyList"];
    var notifyItem = (DBModel.wgs040)ViewData["NtyFirst"];
%>
<div class="nty_main">
    
    <div class="nty_left">
        <%=Html.AntiForgeryToken() %>
        <%if( null != notifyList)
          { 
              foreach( var item in notifyList)
              { 
              %>
        <ul>
            <li class="show_notify" title="<%:item.nt002.Trim() %>" key="<%:item.nt001 %>"><span class="date"><%:item.nt004.ToString("yyyy/MM/dd") %></span><span class="title"<%=string.IsNullOrEmpty(item.nt007) ? string.Empty : string.Format(" style='color:#{0};'", item.nt007.Trim())  %>><%:item.nt002.Trim() %></span></li>
        </ul>
        <%
        }/*foreach*/
        }/*if*/ %>
    </div>
    <div class="nty_right_cnt">
        <div class="nty_top text-cent"><span class="nty_title"><%=notifyItem.nt002.Trim() %></span></div>
        <div class="blank-line"></div>
        <div class="nty_top"><span class="nty_time"><%=notifyItem.nt004 %></span></div>
        <div class="blank-line"></div>
        <div class="nty_right_cnt_x"><%=notifyItem.nt003 %></div>
    </div>
</div>

<div id="notify_content" class="dom-hide">
    <%=ViewData["GlobalNotify"] %>
</div>

<script type="text/javascript">
    $(document).ready(function ()
    {
        //_global_ui("notify_content", 502, 28, 15, 0, "重要提示");
        $("#notify_content").dialog({ width: 600, height:350, title: "重要提示", modal: true, resizable: false, position: { my: "center", at: "center", of: window } });
    });
</script>
</asp:Content>
