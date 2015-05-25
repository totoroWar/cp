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
.login_top{background:url(/Images/ZJ1/UI/LoginZJ1/topbg.jpg) no-repeat top center; width:100%; height:97px; text-align:center;}
.login_bg{background:#010101 url(/Images/ZJ1/UI/LoginZJ1/boxbg.jpg) no-repeat left center; width:100%; height:504px; }
.loginform{width:301px; height:429px;padding-top:140px; padding-left:20px;background:url(/Images/ZJ1/UI/LoginZJ1/regbg.png) no-repeat top;background-position-y:70px; background-position:0 70px;}
.loginform span{width:100%; padding-top:10px; display:block;padding-left:80px;}
.logininput { background: url(/Images/ZJ1/UI/LoginZJ1/inputbg.jpg) no-repeat left center; width:170px; height:25px; border:0; color:#FF9900; font-size:14px; font-family:microsoft yahei; font-weight:bold;}
.logininput2 { background: url(/Images/ZJ1/UI/LoginZJ1/verifycodebg.jpg) no-repeat left center; width:75px; border:0; height:25px; color:#FF9900; font-size:14px; font-family:microsoft yahei; font-weight:bold;}
.login_button{ text-align:left;padding-left:80px; width:100%; line-height:30px; word-spacing:20px}
.khddownload{padding-top:15px; width:100%; text-align:left; padding-left:58px;}

#ui_message_block{width:500px; text-align:left;}
.r_icon{ width:100%; height:128px; margin-bottom:5px;background:url(/Images/Common/reg_ok.png) no-repeat; background-position:center 0;}
#ui_message_block table{ font-size:14px; height:30px; line-height:30px; }
#ui_message_block a{ color:#ff6a00; font-weight:bold;}
-->
</style>

</head>

<body>
<div class="login_warp">
<div class="login_top"></div>
<div class="login_bg"> 
   <div class="loginform"> 
   <form action="/UI/Login" method="post" id="form_register">
                <%:Html.AntiForgeryToken() %>
                <input type="hidden" name="code" value="<%:ViewData["RegCode"] %>" /> 
       <span><input name="username" id="username" type="text" class="logininput" /></span>
	   <span><input name="nickname" type="text" class="logininput"  /></span>
	   <span><input id="default_password" disabled="disabled" type="text" class="logininput" value="<%=ViewData["PDEF"] %>"  /></span>
	   <span><input name="v" type="text" class="logininput2" /> <em><img alt="验证码" title="验证码" src="/UI/VCode" id="validcode_source" align="absmiddle" onclick="refreshimg()" style="cursor: pointer;" /></em></span></form>
    <div class="login_button">
   <input type="image" src="/Images/ZJ1/UI/LoginZJ1/reg.jpg" id="btn_reg" title="注册" />
   <input type="image" src="/Images/ZJ1/UI/LoginZJ1/login_clear.jpg" style="visibility:hidden;" />
   </div>
   <div class="khddownload"><a href="javascript:download();"><img alt="客户端下载" title="客户端下载" src="/Images/ZJ1/UI/LoginZJ1/khd.png" /></a></div>
   </div> 
   
</div>
</div>
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


<div id="client_download" style="display:none;">
    <%=ViewData["SysClientDownload"] %>
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