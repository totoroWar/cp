<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var gameMethodGroupList = (List<DBModel.wgs003>)ViewData["GameMethodGroupList"];
%>
<div class="cjlsoft-body-header">
        <h1>游戏玩法</h1>
        <div class="left-nav">
        <%:ViewData["ParentGameMethodName"] %>
        </div>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
</div>
    <div class="blank-line"></div>
    <form action="/AM/GameMethod" method="post">
        <%:Html.AntiForgeryToken()%>
    <input type="hidden" name="gm001" value="0" />
    <input type="hidden" name="method" value="add" />
    <table class="table-pro" width="100%">
        <tbody>
            <tr>
                <td class="title">游戏编号</td><td><input type="text" name="g001" class="input-text w300px" value="<%:ViewData["GameID"] %>" /></td>
            </tr>
            <tr>
                <td class="title">玩法父级</td><td><input type="text" name="gm002" class="input-text w300px" value="<%:ViewData["ParentID"] %>" /></td>
            </tr>
            <tr>
                <td class="title">玩法组</td><td><% var select = 0; foreach (var gmg in gameMethodGroupList)
                                                 { %><label for="item<%:select %>" style="margin-right:5px;"><input type="radio" id="item<%:select %>" name="gmg001" value="<%:gmg.gmg001 %>" <%if(select == 0){ %>checked="checked"<%} %> /><%:gmg.gmg001 %>-<%:gmg.gmg003 %></label><%select++;} %></td>
            </tr>
            <tr>
                <td class="title">名称</td><td><input type="text" name="gm004" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">显示名称</td><td><input type="text" name="gm005" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">调用方法</td><td><input type="text" name="gm006" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">?</td><td><input type="text" name="gm007" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">?</td><td><input type="text" name="gm008" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">?</td><td><input type="text" name="gm009" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">结算方法</td><td><input type="text" name="gm010" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">单数计算方法</td><td><input type="text" name="gm011" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">单据合法性方法</td><td><input type="text" name="gm012" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">排序</td><td><input type="text" name="gm013" class="input-text w300px" value="0" /></td>
            </tr>
            <tr>
                <td class="title"></td><td><input type="submit" value="保存" class="cjlsoft-post-loading btn-normal" /></td>
            </tr>
        </tbody>
    </table>
    </form>
</asp:Content>
