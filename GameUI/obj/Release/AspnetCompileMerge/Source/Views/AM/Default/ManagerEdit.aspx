<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var editModel = (DBModel.wgs016)ViewData["EditModel"];
%>
<div class="cjlsoft-body-header">
        <h1>编辑管理</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
</div>
    <div class="blank-line"></div>
    <form action="/AM/Manager" method="post">
        <%:Html.AntiForgeryToken()%>
    <input type="hidden" name="mu001" value="<%:editModel.mu001 %>" />
    <input type="hidden" name="method" value="<%:ViewData["MethodType"] %>" />
    <table class="table-pro" width="100%">
        <tbody>
            <tr>
                <td class="title">账号</td><td><input type="text" name="mu002" class="input-text w300px" value="<%:editModel.mu002 %>" /></td>
            </tr>
            <tr>
                <td class="title">昵称</td><td><input type="text" name="mu003" class="input-text w300px" value="<%:editModel.mu003 %>" /></td>
            </tr>
            <tr>
                <td class="title">密码</td><td><input type="text" name="mu004" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">权限</td>
                <td>
                    <select name="pg001">
                        <option value="0">无权限</option>
                        <%foreach(var item in (List<DBModel.wgs015>)ViewData["PGList"]){ %>
                        <option value="<%:item.pg001 %>" title="<%:item.pg003 %>" <%=editModel.pg001 == item.pg001 ? "selected='selected'" : "" %> ><%:item.pg004 %></option>
                        <%} %>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="title">状态</td>
                <td>
                    <select name="mu006">
                        <option value="0" <%=editModel.mu006 == 0 ? "selected='selected'" : "" %>>停用</option>
                        <option value="1" <%=editModel.mu006 == 1 ? "selected='selected'" : "" %>>启用</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="title"></td><td><input type="submit" value="保存" class="cjlsoft-post-loading btn-normal" /></td>
            </tr>
        </tbody>
    </table>
    </form>
</asp:Content>
