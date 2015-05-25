<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>登陆-<%:ViewData["GlobalTitle"] %></title>
<link href="/css/<%=ViewData["UITheme"] %>/base.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/ui/<%=ViewData["UITheme"] %>/jquery_min.js" type="text/javascript"></script>
</head>
<style>
    .login_txt{ padding-top:18px;}
</style>
<body class="login_bg">
<div class="login_body">
	<div class="login_mbox">
        <div class="login_logo">
            <a href="#"><img src="/images/<%=ViewData["UITheme"] %>/logoin_logo.png" /></a>
        </div>
        <div class="login_fbox">
             <form action="/Login.html?sso=yes" method="post" id="form_login">
                 <%:Html.AntiForgeryToken() %>
                 <input type="hidden" name="method" value="APCheck" />
       <input type="hidden" name="login_check" value="<%:System.Guid.NewGuid().ToString() %>" />
            	<div class="login_f">
                	<dl>
                    	<dd><input name="a" type="text" id="username" class="login_txt" value="<%:ViewData["Account"] == null ? ViewData["DefaultName"] : ViewData["Account"] %>" /></dd>
                        
                        <dd><input name="p" type="password" class="login_txt"  /></dd>
                        
                        <dd class="code_img"><input name="v" type="text" class="login_txt_img" />
                        	<a href="javascript:void(0)"><img class="code_img" id="validate" src="/UI2/VCode" width="84" height="31" onclick="refreshimg()"/></a>
                            <a class="code_a" href="#" onclick="refreshimg()">换一个</a>
                        </dd>
                        <div class="clear"></div>
                        <dd class="login_sumit" style="padding-top: 10px"><input class="login_btn"  value="登 陆" /></dd>
                    </dl>
                </div>
            </form>
            <div style="clear:both"></div>
            <div class="login_dl">
            	<a href="#" ><span style="position:absolute;top:30px">客户端下载</span> </a>
            </div>
        </div>
        <div class="login_copyright">
        	<p>Copyright © 2015 jz518.com All Rights Reserved</p>
        </div>
    </div>
</div>
<script type="text/javascript">
    function download()
    {
        $("#client_download").dialog({ width: 400, height: 350, title: "客户端下载", modal: true, resizable: false, position: { my: "center", at: "center", of: window }, show: {effect:"clip", duration:500} });
    }
    $(document).ready(function ()
    {

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
        $("#validate").attr("src", "/UI2/VCode?code=" + Math.random());
    }
    </script>
</body>
</html>
