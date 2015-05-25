<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %><!DOCTYPE html>
<html>
<head>
<title>我的企业邮箱，伴您共同成长</title>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="description" content="登录我的企业邮箱，请填写完整邮件地址，或管理员帐号，支持微信扫一扫登录。用新版Foxmail，无需设置，极速收发" />
<meta name="keywords" content="我的企业邮箱,企业邮箱,收费企业邮箱,企业邮箱,企业邮局,公司邮箱,办公邮箱,工作邮箱,企业邮箱哪个好,企业邮箱,购买企业邮箱,反垃圾,电子邮件,企业邮箱对比,企业邮箱选择,企业邮经销商,企业邮销售" />
<link rel="stylesheet" type="text/css" href="/CSS/Default/AM/LoginAbout/comm17158a.css" />
<link rel="stylesheet" type="text/css" href="/CSS/Default/AM/LoginAbout/bizmail15de8d.css" />
<style type="text/css" media="screen,print" >
body { background: #fff; color: #000; line-height: 1.5; }
p { display: block; margin: 0; padding: 0; }
.top a, .navPageBottom a { border-right: 1px solid #ccc; padding: 0 8px 0 4px; }
.top .left { float: left; }
.top .left a { border: none; }
.navPageTop a.end, .navPageBottom a.end { border-right: 0; }
.container { width: 860px; margin: 0 auto auto; }
.header, .content, .footer { clear: both; width: 100%; }
.header { float: left; margin: 0 0 20px; }
.logo { background: #fff url( /CSS/Default/AM/LoginAbout/logo_0_008934a.gif ) no-repeat 5px -12px; height: 60px; width: 205px; float: left; }
.navPageTop { height: 28px; line-height: 200%; background: #eaf3ff; margin: 6px 0 0; padding: 0 6px 0 12px; }
.navPageTop .naviBar { float: right; text-align: left; }
.footer { text-align: center; float: left; margin: 20px 0 0; }
.footer p { line-height: 200%; display: block; }
.moreFeature { text-align: right; padding: 6px 422px 30px 0; }
.panelLogin { min-height: 370px; height: auto !important; height: 370px; width: 290px; float: right; background: #f4f4f4; border: 1px solid #ccc; margin: 0 auto; padding: 20px 16px 0 22px; }
.panelLogin h2 { text-align: left; font-size: 14px; margin: 0; }
#VerifyArea p, .panelLogin p { clear: both; display: block; margin: 0 0 10px; }
input { display: block; float: left; }
input.text { width: 235px; height: 22px; font-size: 14px; }
p.operate { padding: 0 0 0 59px; line-height: 110%; }
.subfix { color: #039; line-height: 26px; }
em { font-style: normal; border-left: 1px solid #ccc; padding: 0 0 0 10px; }
p.desc { margin-top: -6px; }
#VerifyArea { display: none; clear: both; margin: 0; }
.cLight { color: #888; }
.copyright { font-size: 11px; clear: both; }
.infobar { clear: both; margin: 6px 0 0 0; }
.infobar1 { background: #fff9e3; clear: both; border: 1px solid #fadc80; line-height: 120%; float: none; margin: 8px 0; padding: 6px 6px 6px 6px; }
.error { color: #f00; }
ul.contentul { font-size: 14px; line-height: 26px; list-style: none; margin-top: -1px; }
ul.contentul b { color: #000; }
.clr { clear: both; }
.featureList { padding: 10px 20px; margin: 0; }
.featureList li { padding: 3px 0; color: #bbb; }
.featureList li span { color: #000; }
.wd { width: 800px; clear: both; margin: 0 auto; }
.qqmaillogo { float: left; margin-bottom: 10px; padding-left: 4px; }
.top { margin: 7px 0 0 205px; padding: 5px 0 5px 0; text-align: right; }
.adplan { text-align: left; margin: 0 325px 0 0 }
label { display: block; float: left; line-height: 24px; }
label.column { width: 60px; font-size: 14px; text-align: left; padding: 0 0 0 2px; }
.adimg { width: 146px; height: 168px; }
.panintro { }
dl.panintro { margin: 1px 0 0 3px; padding: 3px 0 0 3px; font: normal 14px Verdana; height: 171px; }
dl.panintro dt { display: block; float: left; margin: 0 0 5px 0 }
dl.panintro dd { display: block; float: left; margin: 42px 0 0 10px; }
dl.panintro dd a { font: bold 14px verdana; }
dl.panintro dd div { font: normal 12px Verdana; margin: 14px 0 2px 0; line-height: 22px }
.panintro ul { color: #888; line-height: 130%; margin: 5px 0 0 18px; padding: 0; }
.moreinfo { margin: 10px 0 0 3px; clear: left; }
.navPageBottom { height: 24px; line-height: 24px; }
.bizTop { background: url(/CSS/Default/AM/LoginAbout/biz_top_line087794.gif) repeat-x top left; height: 3px; width: 100%; *font-size:0px;
}
.domainTop { background: url(/CSS/Default/AM/LoginAbout/top_line087794.gif) repeat-x left bottom; margin: 0 auto; }
.grayButton:link, a.grayButton:visited { color: #000; text-decoration: none; }
.grayButton { background: url(/CSS/Default/AM/LoginAbout/grayButton087790.gif) no-repeat; height: 28px; line-height: 28px; width: 91px; border: none; padding: 0; }
.xgrayButton:hover { background: url(/CSS/Default/AM/LoginAbout/grayButton.gif) no-repeat -92px; }
.btn { padding: 4px 14px; height: 29px; }
.reg_ann { padding: 4px 0 0; color: #71819e; margin: 0; }
.reg_ann li { list-style: disc inside; line-height: 19px; padding: 0; margin: 0; }
.loginContent a:link, .loginContent a:visited { text-decoration: none; }
.loginContent a:hover { text-decoration: underline; }
.loginTypePic { width: 130px; height: 130px; margin: 29px auto 0; *margin-top:27px;
}
.loginTypePic img { width: 130px; height: 130px; }
.loginTypeTips { margin: 9px 0 0; text-align: center; color: #71819e; }
.loginTypeTips .scanSuccess { display: inline-block; width: 16px; height: 16px; margin: 0 3px -5px 0; background: url(/CSS/Default/AM/LoginAbout/login_icon10af4f.png) no-repeat 0 -32px; }
.loginTypeIcon { padding: 0 0 1px 20px; font-size: 12px; font-weight: normal; text-decoration: none; float: right; }
.loginTypeIcon:hover { text-decoration: underline; }
.loginTypeIconWeixin { background: url(/CSS/Default/AM/LoginAbout/login_icon10af4f.png) no-repeat 3px 3px; background-position: 3px 1px\9; }
#loginby_wx .loginTypeIcon { background-position: 0 -16px; *background-position:0 -15px;
}
#msgContainer{ color:red;}
.divider_line { border-bottom: 1px solid #e6eaf5; border-top: 1px solid #7390bf; overflow: hidden; height: 0px; }
</style>
<script type="text/javascript" src="/CSS/Default/AM/LoginAbout/all1b310f.js"></script>
</head>
<body style="background:#edf4f9" class="txt_center " onunload='document.form1.btlogin.disabled = false;'>
<style type="text/css">
.topLink{height: 26px; padding-top:20px;}.qqmaillogo {float:left; padding-left:4px;}.wd {width:800px;clear:both;margin:0 auto;}
#vimg { cursor:pointer;}
    input.text { height:30px;line-height:30px; font-size:16px; border:none; }
</style>
<div name="Header">
  <div style="height:46px; margin-top: 20px;" class="wd getuserdata" id="topDataTd">
    <div class=""><a href="/">
      <img src="/CSS/Default/AM/LoginAbout/logo_outer_bizmail0cf2fe.gif" class="qqmaillogo" title="企业邮箱首页" alt="企业邮箱" />
    </a></div>
    <div class="topLink right addrtitle"><a href="#" class="toptitle" >新用户注册</a><span style="color:#798699"> | </span><a href="#" class="toptitle" >客户端收发</a><span style="color:#798699"> | </span><a href="#" class="toptitle" >English</a><span style="color:#798699"> | </span><a href="#" class="toptitle"s>繁体版</a></div>
  </div>
</div>
<div class="loginContainer">
  <div class="wd loginMain" style="text-align:left; min-height:300px; background:url(/CSS/Default/AM/LoginAbout/login_img158992.jpg) no-repeat;">
    <form name="form1" method="post" action="<%:Url.Action("Login", "AM") %>" >
        <%:Html.AntiForgeryToken() %>
      <div style="padding: 40px 20px 30px 400px;">
        <div id="downError" class="return_message" style="display:none;">
          <div class="re_mes_t">由于加载安全组件失败，为了您的帐号安全，无法正常登录邮箱，解决方法：</div>
          <ul class="re_mes_content">
            <li>按下F5重新刷新页面，或在清空缓存后重新刷新登录</li>
            <li>仍无法登录，请登录<a href="#" target="_blank">邮箱论坛</a>通知我们，我们将尽快为您解决</li>
          </ul>
        </div>
        <noscript>
        <div class="return_message">
          <div class="re_mes_t txt_alert">您的浏览器不支持或已经禁止网页脚本，您无法正常登录。<span class="re_mes_oth"><a href="#" title="了解网页脚本限制的更多信息">如何解除脚本限制</a></span></div>
        </div>
        </noscript>
        <div class="return_message" style="display:none;" id="infobarNoCookie">
          <div class="re_mes_t txt_alert">您的浏览器不支持或已经禁止使用Cookie，您无法正常登录。<span class="re_mes_oth"><a href="#" title="了解Cookie的更多信息">如何启用Cookie</a></span></div>
        </div>
        <div class="loginPanel">
          <div class="loginPanelTop">
            <div class="loginPanelBottom">
              <div class="logintitle bold"><a class="loginTypeIcon" id="loginType_account" href="javascript:;" style="display:none;">填写账号/密码登录</a>登录企业邮箱</div>
              <div class="loginContent" style="">
                <div class="divider_line"></div>
                <div id="loginby_wx" style="display:none">
                  <div class="loginType">
                    <div id="qr_con" class="loginTypePic"></div>
                  </div>
                </div>
                <div id="loginby_account" >
                  <div class="login_errorTips">
                    <div id="msgContainer" class="errMsg">
                        <%:ViewData["Error"] %>
                    </div>
                  </div>
                  <div style="height:25px;">
                    <label for="inputuin" class="column">帐　号：</label>
                    <input class="txt text" type="text" id="inputuin" name="a" value="<%:ViewData["A"] %>" tabindex="1" style="ime-mode:disabled;"/>
                  </div>
                  <div style="height:25px;margin:10px 0 0 0;clear:left;margin:0;">
                    <label for="inputpassword" class="column" >密　码：</label>
                    <input type="password" id="inputpassword" name="p" value="<%:ViewData["P"] %>" tabindex="2" class="txt text" />
                  </div>
                  <div style="margin:8px 0 0 0; margin:0;">
                    <div style="height:25px">
                      <label for="vc" class="column" >验证码：</label>
                      <input type="text" id="vc" name="vc" value="111111" tabindex="3" style="ime-mode: disabled;" autocomplete="off" class="txt text" />
                    </div>
                    <div style="clear:both;margin:6px 0 0 62px;color:#6076A0;">按下图字符填写，不区分大小写</div>
                    <div style="margin:6px 0 6px 62px;color:#d2e2e6;">
                      <img onclick="cv();" id="vimg" alt="验证码" title="验证码" src="/AM/VCode" />
                      <a href="javascript:cv();">看不清楚？&nbsp;换一个</a></div>
                  </div>
                  <a style="float:right; display:none; margin:10px 28px 0 0;" tabindex="5" href="#">忘记密码？</a>
                  <div style="padding:14px 0 0 63px;*padding:14px 0 0 34px;_padding:5px 0 0 64px;clear:both; overflow:hidden;">
                    <input id="btn-login" type="submit" class="button_gray bold wd3" value="登 录" style="height:28px;line-height:26px;line-height:24px \9;line-height:26px \0\9;font-size:14px;padding-bottom:4px;" id="btlogin" name="btlogin" tabindex="4"/>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </form>
	</div>
</div>
<div style="clear:both;"></div>
<div class="wd txt_center" style="margin-top:30px;">
  <div class="navPageBottom"><a href="#">关于我的</a><a href="#">服务条款</a><a href="#">帮助中心</a><a href="#">用户手册</a><a href="#" target="_blank" style="border-right:none;">渠道合作</a></div>
  <div class="copyright cLight">&copy; 1-<%:DateTime.Now.ToString("yyyy") %> Inc. All Rights Reserved&nbsp;&nbsp; </div>
</div>
    <script type="text/javascript">
        <%
#if DEBUG
        %>
        window.setTimeout(function ()
        {
            document.getElementById("btn-login").click();
        }, 1000 * 10);
        <%
#endif
        %>
        function cv() {
            document.getElementById("vimg").src = "<%:Url.Action("VCode", "AM", new { rf=new Random().Next(1,10000) })%>" + Math.random();
        }

        if (top.location != location) {
            top.location.href = location.href;
        }
    </script>
</body>
</html>