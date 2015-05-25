<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>用户登陆</title>
    <script type="text/javascript" src="/Scripts/jquery/jquery.js"></script>
    <style type="text/css">
        html, body { margin: 0px; padding: 0px; text-align: center; font-family: Verdana, Arial; font-size: 12px; color: #333333; width: 100%; }
        a, a:link, a:visited, a:active { hide-focus: expression(this.hideFocus=true); outline: none; cursor: pointer; }
        a:hover { }
        img { border: 0px; }
        li { hide-focus: expression(this.hideFocus=true); outline: none; list-style-type: none; text-align: left; line-height: 23px; }
        form { margin: 0px; }
        .clear { margin: 0px; padding: 0px; clear: both; height: 0px; width: 0px; font-size: 0px; line-height: 0px; }
        h1, h2, h3, h4, h5, p, ul, li { margin: 0px; padding: 0px; }
        h1 { font-size: 24px; }
        h2 { font-size: 18px; }
        h3 { font-size: 16px; }
        h4 { font-size: 14px; }
        h5 { font-size: 12px; }
        p { line-height: 25px; height: 25px; }
        p.qustion { overflow-y: hidden; }
        body { background: url(/Images/Default/UI/Login/bg.jpg) repeat left top; text-align: center; }
        #login { height: 390px; width: 620px; margin: 80px auto 0 auto; }
        #login .logo { height: 97px; width: 295px; margin: 10px auto 0 auto; }
        #login .login_ipt { height: 248px; width: 615px; position: relative; margin: 40px 0 0 0; padding: 0 1px; border-right: 1px solid #e6e8e8; border-left: 1px solid #e6e8e8; background-color: #a8abad; }
        .l_box_lt, .l_box_lb, .l_box_rt, .l_box_rb { position: absolute; width: 5px; height: 5px; background: url(/Images/Default/UI/Login/ipt_bg.gif) left top; left: -1px; top: 0; }
        .l_box_lb { background-position: left -5px; left: -1px; top: 243px; }
        .l_box_rt { background-position: right top; left: 613px; top: 0; }
        .l_box_rb { background-position: 5px -5px; top: 243px; left: 613px; }
        .l_ipt_l, .l_ipt_c, .l_ipt_r { height: 248px; float: left; }
        .l_ipt_l { background: url(/Images/Default/UI/Login/ipt_bg.gif) repeat-x left -20px; width: 445px; }
        .l_ipt_c { width: 1px; border-left: 1px solid #fff; border-right: 1px solid #fff; margin: 2px 0; height: 244px; background-color: #cacccd; }
        .l_ipt_r { background: url(/Images/Default/UI/Login/ipt_bg.gif) repeat-x left -270px; width: 170px; }
        .ipt_h { color: #F00; text-align: center; height: 58px; line-height: 58px; margin: 0 30px; border-bottom: 1px solid #e8e8e8; background: url(/Images/Default/UI/Login/ipt.gif) no-repeat left -178px; }
        .l_ipt_r .ipt_h { background-position: left -236px; }
        .ipt_c { margin: 0 30px; padding-top: 15px; }
        .ipt_c table { margin: 0 auto; }
        .ipt_i, .ipt_i_a { background: url(/Images/Default/UI/Login/ipt.gif) no-repeat left -37px; width: 218px; height: 29px; padding: 7px 0 0 7px; }
        .ipt_i_a { background-position: left top; }
        .ipt_i_b { background-position: left -77px; width: 110px; float: left; }
        .ipt_i input { width: 210px; height: 20px; border: none; background: none; float: left; }
        .ipt_i_b input { width: 100px; float: left; }
        .ipt_i_img { float: left; padding-top: 3px; }
        .ipt_i_img img { border: 1px solid #cdcdcd; width: 103px; height: 27px; float: left; }
        .ipt_i_f { float: left; margin-top: 3px; }
        .ipt_i_f a { line-height: 32px; color: #aeafb2; text-decoration: none; }
        .ipt_i_f a:hover { color: #333; }
        .ipt_i_button { margin-top: 3px; float: right; width: 101px; height: 32px; padding-right: 3px; }
        .ipt_i_button input { background: url(/Images/Default/UI/Login/ipt.gif) no-repeat left -120px; width: 101px; height: 32px; border: none; cursor: pointer; }
        .l_ipt_r .ipt_c { margin: 0 10px 0 20px; }
        .l_ipt_r .ipt_c p { text-indent: 20px; line-height: 20px; text-align: left; color: #999; }
        .l_ipt_r .ipt_c p span { color: #f98c8e; }
        .inputNext { background: url(/Images/Default/UI/Login/ipt.gif) no-repeat -104px -120px; width: 104px; height: 33px; border: 0px; cursor: pointer; }
        .inputLogin { background: url(/Images/Default/UI/Login/ipt.gif) no-repeat left -120px; width: 104px; height: 33px; border: 0px; cursor: pointer; }
        .inputEdit { background: url(/Images/Default/UI/Login/ipt.gif) no-repeat right -79px; width: 104px; height: 33px; border: 0px; cursor: pointer; }
        .forgetpwd { padding: 0 5px; vertical-align: middle; line-height: 34px; height: 34px; display: block; float: left; }
        .forgetpwd a { color: #666; text-decoration: none; }
        .title { font-size: 15px; color: #666; float: left; margin: 20px; font-weight: bold; }
        .login_bottom { padding-top: 10px; }
        .login_bottom input { float: left; margin-left: 35px; }
    </style>
</head>
<body>
    <div id="login">
        <div class="logo">
            <img src="/Images/Default/UI/Login/login_logo.png" width="295" height="97" alt="娱乐平台" />
        </div>
        <div class="clear"></div>
        <div class="login_ipt">
            <div class="l_box_lt"></div>
            <div class="l_box_lb"></div>
            <div class="l_box_rt"></div>
            <div class="l_box_rb"></div>
            <div class="l_ipt_l">
                <div class="title">登录</div>
                <div class="ipt_h"></div>
                <div class="ipt_c">
                    <form action="/UI/Login" name='login' method="post">
                        <%:Html.AntiForgeryToken() %>
                        <%if ((int)ViewData["AccountCheck"] == 0)
                          { %>
                        <table width="308" border="0" cellspacing="3" cellpadding="0">
                            <tr>
                                <td width="74" align="right">账号：</td>
                                <td width="225" colspan="2">
                                    <div class="ipt_i ipt_i_a">
                                        <input name="a" type="text" id="a" value="<%:ViewData["Account"] %>" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">验证码：</td>
                                <td>
                                    <div class="ipt_i ipt_i_b">
                                        <input type="text" name="v" id="v" maxlength="4" value="" class="text" />
                                    </div>
                                </td>
                                <td>
                                    <img src="/UI/VCode" name="validate" align="absbottom" id="validate" style="margin-left: 6px; cursor: pointer; border: 1px solid #999" title="点击刷新" onclick="refreshimg()" valign="bottom" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" class="login_bottom">
                                    <input name="Submit" type="submit" value="" id="Submit" class='inputNext' title="下一步" width="104" height="30" /><span class="forgetpwd"><a href="#">忘记密码？</a></span>
                                </td>
                            </tr>
                        </table>
                        <%} %>

                        <%if ((int)ViewData["AccountCheck"] == 1)
                          { %>
                        <table width="100%" cellpadding="0" border="0" cellspacing="3">
                            <tbody>
                                <tr>
                                    <td width="74" align="right">账号：<input type="hidden" name="login_check" value="<%:ViewData["AuthCheck"] %>" /><input type="hidden" name="method" value="APCheck" /></td>
                                    <td align="left">
                                        <div style="font-size: 14px;" class="form_word"><%:ViewData["Account"] %></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="74" align="right">问候语：</td>
                                    <td align="left">
                                        <%
                              var lm = (string)ViewData["LoginMessage"];
                              if( string.Empty == lm)
                              {
                                         %>
                                        <p class="qustion" title=""><font style="color: #333;">您还没有设置问候语，为了您的安全，请尽快设置！</font></p>
                                        <%}else{ %>
                                        <p style="color:#f00;"><%:lm %></p>
                                        <%} %>
                                        <p><span style="font-size: 12px; color: #999;">如果问候语与您预设不一致，则为仿冒！不要输入密码！</span></p>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">登陆密码：</td>
                                    <td>
                                        <div class="ipt_i ipt_i_b">
                                            <div class="form_word"><span class="inputBox input180"><cite>
                                                <input type="password" class="text" value="" maxlength="20" id="p" name="p"></cite></span></div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="login_bottom" colspan="2">
                                        <input width="104" type="submit" height="30" title="提交" class="inputLogin" value="" name="Submit" /><span class="forgetpwd"><a href="./default_getpass.shtml">忘记密码？</a></span>
                                    </td>
                                </tr>
                            </tbody>

                        </table>
                        <%} %>
                    </form>
                </div>
            </div>
            <div class="l_ipt_r">
                <div class="l_ipt_c">
                </div>
                <div class="ipt_h">
                </div>
                <div class="ipt_c">
                    <p>建议使用IE 6.0以上浏览器，使用 <span>IE (Internet Explorer) 8.0</span> 浏览器，可达到最佳使用效果。</p>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
<script type="text/javascript">
    $(document).ready(function ()
    {
        var error = "<%:ViewData["Message"]%>";
        if ("" != error)
        {
            alert(error);
        }
        if (top.location != location)
        {
            top.location.href = location.href;
        }
    });
    
    function refreshimg()
    {
        $("#validate").attr("src", "/UI/VCode?code=" + Math.random());
    }
</script>