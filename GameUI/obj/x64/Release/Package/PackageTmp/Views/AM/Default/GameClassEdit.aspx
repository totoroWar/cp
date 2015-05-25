<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var editModel = (DBModel.wgs006)ViewData["EditModel"];
    List<string> selectGame = new List<string>();
    if (0 != editModel.gc001)
    {
        selectGame = editModel.gc004.Split(',').ToList();
    }
%>
<div class="cjlsoft-body-header">
        <h1>编辑游戏分类</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
</div>
    <div class="blank-line"></div>
    <form action="/AM/GameClass" method="post">
        <%:Html.AntiForgeryToken()%>
    <input type="hidden" name="gc001" value="<%:editModel.gc001 %>" />
    <input type="hidden" name="method" value="<%:ViewData["MethodType"] %>" />
    <table class="table-pro" width="100%">
        <tbody>
            <tr>
                <td class="title">名称</td><td><input type="text" name="gc002" class="input-text w300px" value="<%:editModel.gc002 %>" /></td>
            </tr>
            <tr>
                <td class="title">显示名称</td><td><input type="text" name="gc003" class="input-text w300px" value="<%:editModel.gc003 %>" /></td>
            </tr>
            <tr>
                <td class="title">游戏列表</td>
                <td>
                    <select name="gc004" multiple="multiple" style="width:200px; height:300px;">
                        <%foreach(var item in (List<DBModel.wgs001>)ViewData["GameList"]){ %>
                        <option value="<%:item.g001 %>" <%if( selectGame.Where(exp=>exp == item.g001.ToString()).Count() > 0){  %> selected="selected"<%} %>><%:item.g003 %></option>
                        <%} %>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="title">排序</td><td><input type="text" name="gc005" class="input-text w300px" value="<%:editModel.gc005 %>" /></td>
            </tr>
            <tr>
                <td class="title">开启</td><td><input type="text" name="gc006" class="input-text w300px" value="<%:editModel.gc006 %>" /></td>
            </tr>
            <tr>
                <td class="title"></td><td><input type="submit" value="保存" class="cjlsoft-post-loading btn-normal" /></td>
            </tr>
        </tbody>
    </table>
    </form>
</asp:Content>
