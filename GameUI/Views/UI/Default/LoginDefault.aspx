<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI/Default/Common/LRAbout.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">登录-<%:ViewData["GlobalTitle"] %></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <div style="width: 420px; margin: 0 auto;">
            <form action="/Login.html?sso=yes" method="post" id="form_login">
                <%:Html.AntiForgeryToken() %>
                <%if ((int)ViewData["AccountCheck"] == 0)
                          { %>
            <table class="table-pro tp5 fs14px" width="100%">
                <tbody>
                    <tr>
                        <td class="title">账号</td>
                        <td>
                            <input class="login_input w200px" type="text" id="username" name="a" value="<%:ViewData["Account"] == null ? ViewData["DefaultName"] : ViewData["Account"] %>" /></td>
                    </tr>
                    <tr>
                        <td class="title">验证码</td>
                        <td>
                            <input class="login_input w100px" type="text" value="" name="v" /><img alt="验证码" src="/UI/VCode" name="validate" id="validate" style="cursor: pointer;" title="点击刷新" onclick="refreshimg()"  /></td>
                    </tr>
                    <tr>
                        <td class="title"></td>
                        <td>
                            <input type="submit" value="登录" class="login_button btn_style_h" onblur="this.hideFocus=true" /></td>
                    </tr>
                    <tr>
                        <td class="title">线路</td>
                        <td>
                            <%=ViewData["GlobalLine"] %>
                        </td>
                    </tr>
                </tbody>
            </table>
            <%} %>
                <%if ((int)ViewData["AccountCheck"] == 1)
                          { %>
                <table class="table-pro tp5 fs14px" width="100%">
                <tbody>
                    <tr>
                        <td class="title">账号</td>
                        <td>
                            <div style="font-size: 14px;" class="form_word"><%:ViewData["Account"] %></div>
                            <input type="hidden" name="login_check" value="<%:ViewData["AuthCheck"] %>" />
                            <input type="hidden" name="method" value="APCheck" />
                            <input type="hidden" name="a" value="<%:ViewData["Account"] %>" />
                        </td>
                    </tr>
                    <tr>
                        <td class="title">问候语</td>
                        <td>
                                        <%
                              var youString = (string)ViewData["LoginMessage"];
                              if (true == string.IsNullOrEmpty(youString))
                              {
                                         %>
                                        <p class="qustion" title=""><font style="color: #333;">您还没有设置问候语，为了您的安全，请尽快设置！</font></p>
                                        <%}else{ %>
                                        <p style="color:#f00;"><%:youString %></p>
                                        <%} %>
                            <p><span style="font-size: 12px; color: #888;">如果问候语与您预设不一致，则为仿冒！不要输入密码！</span></p>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">登录密码</td>
                        <td><input class="login_input w200px" type="password" id="password" name="p" value="" /></td>
                    </tr>
                    <tr>
                        <td class="title"></td>
                        <td>
                            <input type="submit" value="登录" class="login_button btn_style_h" /></td>
                    </tr>
                </tbody>
            </table>
                <%} %>
            </form>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            if (top.location != location) {
                top.location.href = location.href;
            }

            $("#username").focus();
            $("#password").focus();

            $(".btn_style_h").hover(function () {
                $(this).toggleClass("button_h");
            });
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
</asp:Content>
