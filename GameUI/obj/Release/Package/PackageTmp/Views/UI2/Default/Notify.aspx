<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI2/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript">
    $(document).ready(function () {
        $(".show_notify").click(function () {
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
        $(".show_notify span.title").hover(function () {
            $(this).toggleClass("title_hover");
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var notifyList = (List<DBModel.wgs040>)ViewData["NtyList"];
    var notifyItem = (DBModel.wgs040)ViewData["NtyFirst"];
%>
<div class="m_body_bg">
<div class="main_box main_tbg">
    <div class="ajax_tbox gg_list_box">
        <div class="ajaxg_h3"><span><a class="gg_close" href="#"></a></span>平台公告</div>
        <div class="ajax_content">
        	<div class="ajax_gg_list">
            <div class="gg_list">
                <%=Html.AntiForgeryToken() %>
                <%if (null != notifyList)
                  {
                      foreach (var item in notifyList)
                      { 
                      %>
                <h3 class="gg_title"><em class=""></em><%:item.nt002.Trim() %></h3>
                <div style="display: none;" class="gg_content">
                         <%=item.nt003%>;
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
<div id="notify_content" class="dom-hide">
    <%=ViewData["GlobalNotify"] %>
</div>
<script type="text/javascript">
    jQuery(".gg_list").slide({ titCell: "h3.gg_title em", targetCell: ".gg_content", trigger: "click", defaultIndex: 1, effect: "slideDown", delayTime: 300, returnDefault: true, easing: "easeOutCirc" });
    $(document).ready(function () {
        //_global_ui("notify_content", 502, 28, 15, 0, "重要提示");
        $("#notify_content").dialog({ width: 600, height: 350, title: "重要提示", modal: true, resizable: false, position: { my: "center", at: "center", of: window } });
    });
</script>


</div>

<script type="text/javascript">
    $(document).ready(function () {
        //_global_ui("notify_content", 502, 28, 15, 0, "重要提示");
        $("#notify_content").dialog({ width: 600, height: 350, title: "重要提示", modal: true, resizable: false, position: { my: "center", at: "center", of: window } });
    });
</script>
</asp:Content>


