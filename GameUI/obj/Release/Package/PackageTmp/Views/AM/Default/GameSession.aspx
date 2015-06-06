<%@ Import Namespace="DBModel" %><%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs005>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var gameList = (List<DBModel.wgs001>)ViewData["GameList"];
    var gameID = (int)ViewData["GameID"]; 
%>
<div class="cjlsoft-body-header">
        <h1>游戏期数</h1>
        <div class="left-nav">
            <a id="a-menu" href="/AM/GameSession?method=add&gameID=<%:gameID %>" title="添加游戏期数">添加游戏期数</a>
        </div>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
</div>
    <div class="blank-line"></div>
    <div class="cjlsoft-body-header tools">
        <select id="game" class="drop-select-to-url">
            <%foreach( var g in gameList){ %>
            <option value="<%:g.g001 %>" title="<%:g.g003 %>" tourl="<%:Url.Action("GameSession", new { gameID=g.g001 })%>" <%=g.g001 == gameID ? "selected='selected'":"" %>><%:g.g003 %></option>
            <%} %>
        </select>
        <input type="button" value="显示切换" class="show-table-all-col btn-normal" />
    </div>
    <div class="blank-line"></div>
    <form action="/AM/Game" method="post">
        <%:Html.AntiForgeryToken()%>
        <input type="hidden" name="method" value="updateList" />
    <table width="100%" class="table-pro table-color-row">
        <thead>
            <tr class="table-pro-head">
                <th class="w80px">编号</th>
                <th class="w100px">期号</th>
                <th class="w100px">结果</th>
                <th>开盘时间</th>
                <th>封盘时间</th>
                <th>开奖时间</th>
                <th>创建时间</th>
                <th>总单数</th>
                <th>状态</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            <%if( null != Model)
              {
                  int listIndex = 0;
                  foreach(var item in Model)
                  { %>
            <tr>
                <td><input type="hidden" name="[<%:listIndex %>].gs001" value="<%:item.gs001 %>" /><%:item.gs001 %></td>
                <td><input type="text" name="[<%:listIndex %>].gs002" value="<%:item.gs002.Trim() %>" class="input-text w80px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gs007" value="<%:item.gs007 != null ? item.gs007.Trim() : "" %>" class="input-text w100px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gs003" value="<%:item.gs003 %>" class="input-text w120px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gs004" value="<%:item.gs004 %>" class="input-text w120px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gs005" value="<%:item.gs005 %>" class="input-text w120px" /></td>
                <td><input type="hidden" name="[<%:listIndex %>].gs006" value="<%:item.gs006 %>" /><%:item.gs006 %></td>
                <td><input type="text" name="[<%:listIndex %>].gs008" value="<%:item.gs008 %>" class="input-text w50px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gs010" value="<%:item.gs010 %>" class="input-text w50px" /></td>
                <td></td>
            </tr>
            <%
                      listIndex++;
                  }/*foreach*/
            }/*if*/ %>
        </tbody>
    </table>

    <div class="blank-line"></div>

        <%=ViewData["PageList"] %>
    </form>
</asp:Content>
