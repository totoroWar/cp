<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI/Default/Common/LRAbout.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %><asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">域名验证-<%:ViewData["GlobalTitle"] %></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="main">
        <div style="width: 420px; margin: 0 auto;">
            <form action="/UI/Login" method="post" id="form_register">
            <table class="table-pro tp5 fs14px" width="100%">
                <tbody>
                    <tr>
                        <td class="title">域名</td>
                        <td><input class="login_input w300px" type="text" name="url" id="url" /></td>
                    </tr>
                    <tr>
                        <td class="title"></td>
                        <td>
                            <input type="button" id="btn_check" value="验证" class="login_button btn_style_h" /></td>
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
<script type="text/javascript">
    $("#btn_check").click(function ()
    {
        $.ajax({
            async: false, cache: false, type: "POST", timeout: 1000 * 5, url: "/Public.html?method=checkURL", data: {url:$("#url").val()}, dataType: "json",
            success: function (a, b) {
                alert(a.Message);
            }
        });
    });
</script>
</asp:Content>