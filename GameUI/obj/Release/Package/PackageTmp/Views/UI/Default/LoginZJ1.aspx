<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="X-UA-Compatible"content="IE=9; IE=8; IE=7; IE=EDGE">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>登录-<%:ViewData["GlobalTitle"] %></title>
<style type="text/css">
<!--
*{margin: 0px;padding:0px; margin:0 auto;}
body{font-size: 9pt;background:#010101; }
body,form,p,ol,ul,p,h2,h3,h4{margin:0;padding:0;}
ul,li,dl,dd{list-style:none;}
img,fieldset{border:0;}
a:link,a:visited{text-decoration:none;}
a:hover{text-decoration:none;}
em{font-style:normal;}
.left{float: left;}
.right{float: right;}
.clear{clear:both;}
img{border:0px;}
table{margin:0 auto;}

.login_warp{background:#010101; width:100%; height:100%;}
.login_top{background:url(/Images/ZJ1/UI/LoginZJ1/topbg.jpg) repeat top center; width:100%; height:97px; text-align:center; }
.login_bg{background:#010101 url(/Images/ZJ1/UI/LoginZJ1/boxbg.jpg) no-repeat top center; width:100%; height:504px; }
.loginform{width:301px; height:429px;padding-top:162px; padding-left:60px;background:url(/Images/ZJ1/UI/LoginZJ1/loginbg.png) no-repeat top center; background-position-y:90px; background-position:0 90px;}
.loginform span{width:100%; padding-top:10px; display:block;padding-left:45px;}
.logininput { background: url(/Images/ZJ1/UI/LoginZJ1/inputbg.jpg) no-repeat left center; width:170px; height:25px; border:0; color:#FF9900; font-size:14px; font-family:microsoft yahei; font-weight:bold;}
.logininput2 { background: url(/Images/ZJ1/UI/LoginZJ1/verifycodebg.jpg) no-repeat left center; width:75px; border:0; height:25px; color:#FF9900; font-size:14px; font-family:microsoft yahei; font-weight:bold;}
.login_button{padding-top:5px; text-align:left;padding-left:45px; width:100%; line-height:30px; word-spacing:20px}
.khddownload{padding-top:10px; width:100%; text-align:left; padding-left:40px;}
-->
</style>
<script type="text/javascript" src="/Scripts/jquery/jquery.js"></script>
<script type="text/javascript" src="/Scripts/jquery/jquery-ui.min.js"></script>
<link rel="stylesheet" type="text/css" href="/CSS/<%=ViewData["UITheme"] %>/UI/jqueryui_base/jquery.ui.all.css" />
</head>

<body>
<div class="login_warp">
<div class="login_top"></div>
<div class="login_bg"> 
   <div class="loginform"> 
   <form action="/Login.html?sso=yes" method="post" id="form_login">
       <%:Html.AntiForgeryToken() %>
       <input type="hidden" name="method" value="APCheck" />
       <input type="hidden" name="login_check" value="<%:System.Guid.NewGuid().ToString() %>" />
    <span><input name="a" type="text" id="username" class="logininput" value="<%:ViewData["Account"] == null ? ViewData["DefaultName"] : ViewData["Account"] %>" /></span>
	   <span><input name="p" type="password" class="logininput"  /></span>
	   <span><input name="v" type="text" class="logininput2" /> <em><img id="validate" alt="点击刷新" src="/UI/VCode" style="cursor:pointer;" align="absmiddle" title="点击刷新" onclick="refreshimg()" /></em></span>
    <div class="login_button">
   <input type="image" src="/Images/ZJ1/UI/LoginZJ1/login_ok.jpg"/>
   <input type="image" src="/Images/ZJ1/UI/LoginZJ1/login_clear.jpg" style="visibility:hidden;" />
   </div></form>
   <!--<div class="khddownload"><a href="javascript:download();"><img alt="客户端下载" title="客户端下载" src="/Images/ZJ1/UI/LoginZJ1/khd.png" /></a></div>-->
   </div> 
   
</div>
</div>
<div id="client_download" style="display:none;">
    <%=ViewData["SysClientDownload"] %>
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
        $("#validate").attr("src", "/UI/VCode?code=" + Math.random());
    }
    </script>
</body>
</html>