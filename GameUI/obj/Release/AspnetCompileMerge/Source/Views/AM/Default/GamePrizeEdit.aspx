<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var editModel = (DBModel.wgs007)ViewData["EditModel"];
    var gameClassID = (int)ViewData["GameClassID"];
%>
<div class="cjlsoft-body-header">
        <h1>编辑游戏奖金组</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
</div>
    <div class="blank-line"></div>
    <form action="/AM/GameClassPrize" method="post">
        <%:Html.AntiForgeryToken()%>
    <input type="hidden" name="gp001" value="<%:editModel.gp001 %>" />
    <input type="hidden" name="gp005" value="<%:editModel.gp005 %>" />
    <input type="hidden" name="gp006" value="<%:editModel.gp006 %>" />
    <input type="hidden" name="gp007" value="<%:editModel.gp007 %>" />
    <input type="hidden" name="gp008" value="<%:editModel.gp008 %>" />
    <input type="hidden" name="gc001" value="<%:gameClassID %>" />
    <input type="hidden" name="method" value="<%:ViewData["MethodType"] %>" />
    <table class="table-pro" width="100%">
        <tbody>
            <tr>
                <td class="title">名称</td><td><input type="text" name="gp002" class="input-text w300px" value="<%:editModel.gp002 %>" /></td>
            </tr>
            <tr>
                <td class="title">显示名称</td><td><input type="text" name="gp003" class="input-text w300px" value="<%:editModel.gp003 %>" /></td>
            </tr>
            <tr>
                <td class="title">开同级个数</td><td><input type="text" name="gp004" class="input-text w300px" value="<%:editModel.gp004 %>" /></td>
            </tr>
            <tr>
                <td class="title">不定位返点</td><td><input type="text" name="gp004" class="input-text w300px" value="<%:editModel.gp007 %>" /></td>
            </tr>
            <tr>
                <td class="title">定位返点</td><td><input type="text" name="gp004" class="input-text w300px" value="<%:editModel.gp008 %>" /></td>
            </tr>
            <tr>
                <td class="title">排序</td><td><input type="text" name="gp009" class="input-text w300px" value="<%:editModel.gp009 %>" /></td>
            </tr>
            <tr>
                <td class="title"></td><td><input type="submit" value="保存" class="cjlsoft-post-loading btn-normal" /></td>
            </tr>
        </tbody>
    </table>
    </form>
</asp:Content>
