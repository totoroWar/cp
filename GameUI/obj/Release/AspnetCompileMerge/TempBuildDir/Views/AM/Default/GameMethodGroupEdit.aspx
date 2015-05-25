<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="cjlsoft-body-header">
        <h1>编辑玩法组</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
</div>
    <div class="blank-line"></div>
    <form action="/AM/GameMethodGroup" method="post">
        <%:Html.AntiForgeryToken()%>
    <input type="hidden" name="method" value="<%:ViewData["MethodType"] %>" />
    <table class="table-pro" width="100%">
        <tbody>
            <tr>
                <td class="title">名称</td><td><input type="text" name="gmg002" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">显示名称</td><td><input type="text" name="gmg003" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">可用</td><td><input type="text" name="gmg004" class="input-text w300px" value="" /><span class="tips">0停用，1启用</span></td>
            </tr>
            <tr>
                <td class="title">代码字母</td><td><input type="text" name="gmg005" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">代码数字</td><td><input type="text" name="gmg006" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">排序</td><td><input type="text" name="gmg007" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title"></td><td><input type="submit" value="保存" class="cjlsoft-post-loading btn-normal" /></td>
            </tr>
        </tbody>
    </table>
    </form>
</asp:Content>
