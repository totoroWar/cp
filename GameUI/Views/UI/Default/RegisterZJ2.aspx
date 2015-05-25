<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="X-UA-Compatible"content="IE=9; IE=8; IE=7; IE=EDGE">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>注册-<%:ViewData["GlobalTitle"] %></title>
    <script type="text/javascript" src="/Scripts/jquery/jquery.js"></script>
    <script type="text/javascript" src="/Scripts/jquery/jquery-ui.min.js"></script>
    <link rel="stylesheet" type="text/css" href="/CSS/<%=ViewData["UITheme"] %>/UI/jqueryui_base/jquery.ui.all.css" />
<link rel="stylesheet" type="text/css" href="/css/zj2/ui/loginregister.css">
</head>
<body>
    <div id="client_download" style="display:none;">
    <%=ViewData["SysClientDownload"] %>
</div>
<div id="membermain_headbg">
<div id="membermain_bg">
<div class="loginhead">
<%--  <a href="#">帮助中心</a>-
  <span> | </span>
  <a href="#">用户登录</a>--%>
  <div class="clear"><!--清除浮动//end--></div>
</div>

<div class="logincomtentmain">
  <div class="login_logo"></div>
  <div class="login_h1">会员注册</div>
  <form action="/UI/Login" method="post" id="form_register">
       <%:Html.AntiForgeryToken() %>
      <input type="hidden" name="code" value="<%:ViewData["RegCode"] %>" /> 
  <div class="logincomtent_form">
    <dl class="dlinput340">
      <dt>账号</dt><dd><input name="username" id="username" type="text" class="input250"/></dd>
    </dl>
    <dl class="dlinput340">
      <dt>昵称</dt><dd><input  name="nickname"  type="text" class="input250"/></dd>
    </dl>
    <dl class="dlinput340">
      <dt>设置密码</dt><dd><input id="default_password" disabled="disabled"type="text" class="input250"  value="<%=ViewData["PDEF"] %>"/></dd>
    </dl>
    <dl class="dlinput225">
      <dt>验证码</dt><dd><input name="v" type="text" class="input62"/><span class="logincomtent_formyzm"><img alt="验证码" title="验证码" src="/UI/VCode" id="validcode_source" align="absmiddle" onclick="refreshimg()"  width="75" height="34" /></span><span class="logincomtent_btnok"><a id="btn_reg" href="#"><img src="images/zj2/ui/logincomtent_btno.jpg" width="109" height="39"></a></span></dd>
    </dl>
  </div>

  <div class="logincomtent_download"><a href="#">下载客户端 ></a></div>
  </form>
</div>


</div><!--membermain_bg//end-->
</div><!--membermain_headbg//end-->
      <div id="ui_message_block" style="display:none;">
    <div class="r_icon"></div>
    <table width="100%">
        <tr>
            <td style="text-align:right; width:150px;">账号：</td>
            <td id="acct"></td>
        </tr>
        <tr>
            <td style="text-align:right;">密码：</td>
            <td id="pwd"></td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:center;"><a id="login_link" href="#" title="马上登录">马上登录</a></td>
        </tr>
    </table>
</div>
</body>
</html>
<script type="text/javascript">
    function download()
    {
        $("#client_download").dialog({ width: 400, height: 350, title: "客户端下载", modal: true, resizable: false, position: { my: "center", at: "center", of: window }, show: { effect: "clip", duration: 500 } });
    }
    $(document).ready(function () {
        $(".loginform").slideDown("slow");
        if (top.location != location) {
            top.location.href = location.href;
        }
        $(document).keydown(function (event) {
            if (event.keyCode == 13) {
                $("#btn_reg").click();
            }
        });

    });
    function refreshimg() {
        $("#validcode_source").attr("src", "/UI/VCode?code=" + Math.random());
    }
    $(document).ready(function ()
    {
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
                        //$.blockUI({ draggable: true, message: $("#ui_message_block"), css: { border: "1px solid #745453", width: "500px", left: "33%", top: "30%", padding: 0, cursor: "default" }, overlayCSS: { cursor: "default" } });
                        //$(".ui_message_content").html(a.Message + "<table width='100%'><tr><td width='50%' align='right'>" + "账号：</td><td align='left'>" + username + "</td><tr><td width='50%' align='right'>" + "密码：</td><td align='left'>" + $("#default_password").html() + "</td></tr></table>" + "<a href='/Login.html?username=" + username + "' title='马上登录' target='_blank'>" + "马上登录</a></p>");
                        //$('.blockOverlay').click($.unblockUI);
                        $("#ui_message_block").dialog({ width: 400, height: 300, title: "注册成功", modal: true, resizable: false, position: { my: "center", at: "center", of: window }, show: { effect: "clip", duration: 500 } });
                        $("#acct").html(username);
                        $("#pwd").html($("#default_password").val());
                        $("#login_link").attr("href", "/Login.html?username=" + username);
                    }
                },
                complete: function (a, b) {
                    refreshimg();
                }
            });
        });
    });
    </script>