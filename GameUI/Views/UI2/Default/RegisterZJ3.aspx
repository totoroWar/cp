<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>注册-<%:ViewData["GlobalTitle"] %></title>
<link href="/css/<%=ViewData["UITheme"] %>/base.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/ui/<%=ViewData["UITheme"] %>/jquery_min.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Scripts/jquery/jquery.js"></script>
    <script type="text/javascript" src="/Scripts/jquery/jquery-ui.min.js"></script>
    <link rel="stylesheet" type="text/css" href="/CSS/zj3/UI/jqueryui_base/jquery.ui.all.css">
<style>
.register_fbox{ background:url(/images/zj3/register_fbox_bg.png) no-repeat center; height:460px;}
.register_f{ width:360px; padding-top:90px;}
.register_f dl { margin:0; padding:0;}
.register_f dl dd{ padding-top:15px; width:100%; height:32px; line-height:32px;}
.register_f dl dd span{ color:#fff; font:normal 14px/30px "微软雅黑"; width:70px; float:left;}
.register_f input{/* background:none;*/ border:none; float:left;}
input.rig_txt{ background:#fff; width:245px; height:30px; line-height:30x; font-size:14px;}
input.register_txt_img{ background:#fff; float:left; width:110px; height:30px; line-height:30px; font-size:14px;}
.register_f dl dd.rig_code_img a{ color:#fdb02b; font-size:14px; line-height:36px; margin-left:10px; text-decoration:underline;}
.register_f dl dd.rig_code_img a:hover{ color:#f00;}
img.rig_img{ float:left; margin-left:10px;}
.register_f dl dd.rig_sumit{ height:53px; text-align:center;}
input.rig_btn{ background:url(/images/zj3/psw_btn_bg01.png) no-repeat;text-align:center; color:#fff; cursor:pointer; font:normal 18px/12px "微软雅黑"; width:139px; height:43px; margin-left:70px;line-height:43px;}
input.rig_btn:hover{ background:url(/images/zj3/psw_btn_bg02.png) no-repeat center; color:#f00;}
#ui_message_block{display:none}

/*登陆界面*/


.login_dl{ margin-top:25px; text-align:center;}
.login_dl a{ color:#fff; font:normal 16px/12px "微软雅黑"; padding-left:30px;}
.login_dl a:hover{ color:#fdb02b;}
.login_copyright{ font:normal 14px/25px "微软雅黑"; text-align:center; margin-top:80px;}
</style>
</head>

<body class="login_bg">
 <div id="client_download" style="display:none;">
    <%=ViewData["SysClientDownload"] %>
</div>
<div class="login_body">
	<div class="login_mbox">
        <div class="login_logo">
            <a href="#"><img src="/images/zj3/logoin_logo.png" /></a>
        </div>
        <div class="register_fbox">
            <form action="/UI2/Login" method="post" id="form_register">
                <%:Html.AntiForgeryToken() %>
                <input type="hidden" name="code" value="<%:ViewData["RegCode"] %>" /> 
            	<div class="register_f">
                	<dl>
                    	<dd><span>账号：</span><input name="username" id="username" type="text" class="rig_txt"/></dd>
                        <dd><span>昵称：</span><input  name="nickname"  type="text" class="rig_txt"/></dd>
                        <dd><span>设置密码：</span><input id="default_password" disabled="disabled"type="text" class="rig_txt"  value="<%=ViewData["PDEF"] %>"/></dd>
                        <dd class="rig_code_img"><span>验证码：</span><input name="v" type="text" class="register_txt_img"/>
                        	<a href="javascript:void(0)"><img class="rig_img" src="/UI2/VCode" id="validcode_source" align="absmiddle" onclick="refreshimg()"  width="75" height="34"  /></a>
                            <a class="code_a" href="#" onclick="refreshimg()">换一个</a>
                        </dd>
                        
                        <dd class="rig_sumit"><input class="rig_btn" id="btn_reg" value="注 册" /></dd>
                    
                </div>
            </form>
            <div class="login_dl">
            	<a href="#">客户端下载</a>
            </div>
        </div>
        <div class="login_copyright">
        	<p>Copyright  2015 jz518.com All Rights Reserved</p>
        </div>
    </div>
</div>

<div id="ui_message_block">
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
        $("#validcode_source").attr("src", "/UI2/VCode?code=" + Math.random());
    }
    $(document).ready(function ()
    {
        $("#username").value = '';
        $("#validcode_source")[0].value = '';
        $("#validate").attr('src', "/UI2/VCode?rand=" + Math.random());
        $("#username").focus();
        $('#btn_reg').click(function () {
            var form_data = $("#form_register").serialize();
            $.ajax({
                async: false, cache: false, type: "POST", timeout: 1000 * 5, url: "/UI2/Register", data: form_data, dataType: "json",
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

