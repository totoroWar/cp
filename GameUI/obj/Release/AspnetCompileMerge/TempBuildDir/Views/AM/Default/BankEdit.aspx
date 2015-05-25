<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="cjlsoft-body-header">
        <h1>编辑银行</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
</div>
    <div class="blank-line"></div>
    <form action="/AM/Bank" method="post">
        <%:Html.AntiForgeryToken()%>
    <input type="hidden" name="sm001" value="0" />
    <input type="hidden" name="method" value="<%:ViewData["MethodType"] %>" />
    <table class="table-pro" width="100%">
        <tbody>
            <tr>
                <td class="title">名称</td><td><input type="text" name="sb003" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">显示名称</td><td><input type="text" name="sb004" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">官网</td><td><input type="text" name="sb005" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">图标</td><td><input type="text" name="sb006" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">标识</td><td><input type="text" name="sb002" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">可用</td><td><input type="text" name="sb008" class="input-text w300px" value="1" /></td>
            </tr>
            <tr>
                <td class="title">排序</td><td><input type="text" name="sb009" class="input-text w300px" value="0" /></td>
            </tr>
            <tr>
                <td class="title"></td><td><input type="submit" value="保存" class="cjlsoft-post-loading btn-normal" /></td>
            </tr>
        </tbody>
    </table>
    </form>
</asp:Content>
