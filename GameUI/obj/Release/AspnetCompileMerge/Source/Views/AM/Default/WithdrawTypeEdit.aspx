<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var editModel = (DBModel.wgs024)ViewData["EditModel"];
%>
<div class="cjlsoft-body-header">
        <h1>编辑提现类型</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
</div>
    <div class="blank-line"></div>
    <form action="/AM/WithdrawType" method="post">
        <%:Html.AntiForgeryToken()%>
    <input type="hidden" name="uwt001" value="<%:editModel.uwt001 %>" />
    <input type="hidden" name="method" value="<%:ViewData["MethodType"] %>" />
    <table class="table-pro" width="100%">
        <tbody>
            <tr>
                <td class="title">名称</td><td><input type="text" name="uwt002" class="input-text w300px" value="<%:editModel.uwt002 %>" /></td>
            </tr>
            <tr>
                <td class="title">显示名称</td><td><input type="text" name="uwt003" class="input-text w300px" value="<%:editModel.uwt003 %>" /></td>
            </tr>
            <tr>
                <td class="title">启用</td><td><input type="checkbox" value="1" name="uwt004" <%:editModel.uwt004 == 1 ? "checked='checked'" : "" %> /><input type="hidden" name="uwt004" value="0" /></td>
            </tr>
            <tr>
                <td class="title">排序</td><td><input type="text" name="uwt006" class="input-text w300px" value="<%:editModel.uwt006 %>" /></td>
            </tr>
            <tr>
                <td class="title"></td><td><input type="submit" value="保存" class="cjlsoft-post-loading btn-normal" /></td>
            </tr>
        </tbody>
    </table>
    </form>
</asp:Content>
