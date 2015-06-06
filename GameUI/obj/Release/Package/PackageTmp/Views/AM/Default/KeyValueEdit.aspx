<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var editModel = (DBModel.wgs027)ViewData["EditModel"];
%>
<div class="cjlsoft-body-header">
        <h1>编辑配置键值</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
</div>
    <div class="blank-line"></div>
    <form action="/AM/KeyValue" method="post">
        <%:Html.AntiForgeryToken()%>
    <input type="hidden" name="method" value="<%:ViewData["MethodType"] %>" />
    <table class="table-pro" width="100%">
        <tbody>
            <tr>
                <td class="title">键</td><td><input type="text" name="cfg001" class="input-text w300px" value="<%:editModel.cfg001 != null ? editModel.cfg001.Trim() : "" %>" /></td>
            </tr>
            <tr>
                <td class="title">名称</td><td><input type="text" name="cfg002" class="input-text w300px" value="<%:editModel.cfg002 != null ? editModel.cfg002.Trim() : "" %>" /></td>
            </tr>
            <tr>
                <td class="title">值</td><td><textarea rows="6" cols="10" class="input-text w300px" name="cfg003"><%:editModel.cfg003 %></textarea></td>
            </tr>
            <tr>
                <td class="title">显值</td><td><textarea rows="6" cols="10" class="input-text w300px" name="cfg004"><%:editModel.cfg004 %></textarea></td>
            </tr>
            <tr>
                <td class="title">说明</td><td><textarea rows="6" cols="10" class="input-text w300px" name="cfg005"><%:editModel.cfg005 %></textarea></td>
            </tr>
            <tr>
                <td class="title">排序</td><td><input type="text" name="cfg007" class="input-text w300px" value="<%:editModel.cfg007 %>" /></td>
            </tr>
            <tr>
                <td class="title"></td><td><input type="submit" value="保存" class="cjlsoft-post-loading btn-normal" /></td>
            </tr>
        </tbody>
    </table>
    </form>
</asp:Content>
