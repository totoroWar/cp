<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta charset="utf-8" >
<title>登录-<%:ViewData["GlobalTitle"] %></title>
    <script type="text/javascript" src="/Scripts/jquery/jquery.js"></script>
    <script  type="text/javascript" src="/Scripts/jquery/jquery-ui.min.js"></script>
<link rel="stylesheet" type="text/css" href="/CSS/ZJ2/UI/loginregister.css" />
    <link rel="stylesheet" type="text/css" href="/CSS/<%=ViewData["UITheme"] %>/UI/jqueryui_base/jquery.ui.all.css" />
</head>

<body>
<div id="membermain_headbg">
<div id="membermain_bg">
<div class="loginhead">
 <%-- <a href="#">帮助中心</a>
  <span> | </span>
  <a href="#">用户注册</a>--%>
  <div class="clear"><!--清除浮动//end--></div>
</div>

<div class="logincomtentmain">
  <div class="login_logo"></div>
  <div class="login_h1">会员登录</div>
   <form action="/Login.html?sso=yes" method="post" id="form_login">
       <%:Html.AntiForgeryToken() %>
       <input type="hidden" name="method" value="APCheck" />
       <input type="hidden" name="login_check" value="<%:System.Guid.NewGuid().ToString() %>" />
  <div class="logincomtent_form">
    <dl class="dlinput340">
      <dt>用户名</dt><dd><input name="a" id="username" type="text" class="input250" /></dd>
    </dl>
    <dl class="dlinput340">
      <dt>密&nbsp;码</dt><dd><input name="p" type="password" class="input250" /></dd>
    </dl>
    <dl class="dlinput225">
      <dt>验证码</dt><dd><input name="v" type="text" class="input62"><span class="logincomtent_formyzm">
          <img id="validate" alt="单击刷新" src="/UI/VCode"  width="75" height="34" title="单击刷新" onclick="refreshimg()" /></span><span class="logincomtent_btnok"><a href="javascript: $('#form_login').submit()"><img src="images/zj2/ui/logincomtent_btnok.jpg" width="109" height="39"/></a></span></dd>
    </dl>
  </div>
  
  <div class="logincomtent_download"><a href="javascript:download();">下载客户端 ></a></div>
  </form>
</div>


</div><!--membermain_bg//end-->
</div><!--membermain_headbg//end-->
    <div id="client_download" style="display:none;">
    <%=ViewData["SysClientDownload"] %>
</div>
<script type="text/javascript">
    function download() {
        $("#client_download").dialog({ width: 400, height: 350, title: "客户端下载", modal: true, resizable: false, position: { my: "center", at: "center", of: window }, show: { effect: "clip", duration: 500 } });
    }
    $(document).ready(function () {

        //$(".loginform").slideDown("normal");
        if (top.location != location) {
            top.location.href = location.href;
        }

        $("#username").focus();

        $(document).keydown(function (event) {
            if (event.keyCode == 13) {
                $("#form_login").submit();
            }
        });
        var error_message = '<%:ViewData["Message"]%>';
            if (error_message != '') {
                alert(error_message);
            }
    });
        function refreshimg() {
            $("#validate").attr("src", "/UI/VCode?code=" + Math.random());
        }
    </script>
</body>
</html>