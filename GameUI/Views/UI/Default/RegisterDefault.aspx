<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI/Default/Common/LRAbout.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %><asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">注册-<%:ViewData["GlobalTitle"] %></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="main">
        <div style="width: 420px; margin: 0 auto;">
            <form action="/UI/Login" method="post" id="form_register">
                <%:Html.AntiForgeryToken() %>
                <input type="hidden" name="code" value="<%:ViewData["RegCode"] %>" />
            <table class="table-pro tp5 fs14px" width="100%">
                <tbody>
                    <tr>
                        <td class="title">账号</td>
                        <td>
                            <input class="login_input w200px" type="text" name="username" value="" id="username" /></td>
                    </tr>
                    <tr>
                        <td class="title">昵称</td>
                        <td>
                            <input class="login_input w200px" type="text" name="nickname" value="" /></td>
                    </tr>
                    <tr>
                        <td class="title">默认密码</td>
                        <td id="default_password">a585858</td>
                    </tr>
                    <tr>
                        <td class="title">验证码</td>
                        <td>
                            <input class="login_input w100px" type="text" value="" maxlength="4" name="v" /><img alt="验证码" src="/UI/VCode" name="validcode_source" id="validcode_source" style="cursor: pointer;" title="点击刷新" onclick="refreshimg()"  /></td>
                    </tr>
                    <tr>
                        <td class="title"></td>
                        <td>
                            <input type="button" id="btn_reg" value="登录" class="login_button btn_style_h" /></td>
                    </tr>
                    <tr>
                        <td class="title">线路</td>
                        <td>
                            <%=ViewData["GlobalLine"] %>
                        </td>
                    </tr>
                </tbody>
            </table>
            </form>
        </div>
    </div>
<div id="ui_message_block" style="display:none; font-size:14px;">
        <div class="ui_message_content"></div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        if (top.location != location) {
            top.location.href = location.href;
        }

        $(".btn_style_h").hover(function () {
            $(this).toggleClass("button_h");
        });
        $(document).keydown(function (event) {
            if (event.keyCode == 13) {
                $("#btn_reg").click();
            }
        });
    });
    function refreshimg() {
        $("#validcode_source").attr("src", "/UI/VCode?code=" + Math.random());
    }
    $(document).ready(function () {
        $("#username").value = '';
        $("#validcode_source")[0].value = '';
        $("#validate").attr('src', "/UI/VCode?rand=" + Math.random());
        $("#username").focus();
        $('#btn_reg').click(function () {
            var form_data = $("#form_register").serialize();
            $.ajax({
                async: false, cache: false, type: "POST", timeout: 1000 * 5, url: "/UI/Register", data: form_data, dataType: "json",
                beforeSend: function (a) {
                },
                success: function (a, b) {
                    if (0 == a.Code) {
                        alert(a.Message);
                    }
                    else if (1 == a.Code) {
                        var username = $("#username").val();
                        $.blockUI({ draggable: true, message: $("#ui_message_block"), css: { border: "1px solid #eee", width: "500px", left: "33%", top: "30%", padding: 0, cursor: "default" }, overlayCSS: { cursor: "default" } });
                        $(".ui_message_content").html(a.Message + "<table width='100%'><tr><td width='50%' align='right'>" + "账号：</td><td align='left'>" + username + "</td><tr><td width='50%' align='right'>" + "密码：</td><td align='left'>" + $("#default_password").html() + "</td></tr></table>" + "<a href='/Login.html?username=" + username + "' title='马上登录' target='_blank'>" + "马上登录</a></p>");
                        $('.blockOverlay').click($.unblockUI);
                    }
                },
                complete: function (a, b) {
                    refreshimg();
                }
            });
        });
    });
    </script>
</asp:Content>
