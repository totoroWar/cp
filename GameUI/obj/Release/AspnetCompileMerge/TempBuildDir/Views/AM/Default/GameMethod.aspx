<%@ Import Namespace="DBModel" %><%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs002>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var gameList = (List<DBModel.wgs006>)ViewData["GameList"];
    var gameID = (int)ViewData["GameID"]; 
    var parentID = (int)ViewData["ParentID"];
%>
<div class="cjlsoft-body-header">
        <h1>游戏玩法</h1>
        <div class="left-nav">
            <%if( parentID != 0 ) {%>
            <a href="/AM/GameMethod?method=gameID=<%:gameID %>&parentID=0" title="反回主玩法">反回主玩法</a>
            <%} %>
            <a id="a-menu" href="/AM/GameMethod?method=add&gameID=<%:gameID %>&parentID=<%:parentID %>" title="添加玩法">添加玩法</a>
        </div>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
</div>
    <div class="blank-line"></div>
    <div class="cjlsoft-body-header tools">
        <select id="game" class="drop-select-to-url">
            <%foreach( var g in gameList){ %>
            <option value="<%:g.gc001 %>" title="<%:g.gc003 %>" tourl="<%:Url.Action("GameMethod", new { gameID=g.gc001,parentID=parentID })%>" <%=g.gc001 == gameID ? "selected='selected'":"" %>><%:g.gc003 %></option>
            <%} %>
        </select>
        <%:ViewData["ParentGameMethodName"] %>
    </div>
    <div class="blank-line"></div>
    <form action="/AM/GameMethod" method="post">
        <%:Html.AntiForgeryToken()%>
        <input type="hidden" name="method" value="updateList" />
    <table width="100%" class="table-pro table-color-row">
        <thead>
            <tr class="table-pro-head">
                <th>编号</th>
                <th>游戏</th>
                <th>父级</th>
                <th>组</th>
                <th>名称</th>
                <th>显示名称</th>
                <th>?</th>
                <th>?</th>
                <th>?</th>
                <th>调用方法</th>
                <th>结算方法</th>
                <th>单数计算方法</th>
                <th>数合法性方法</th>
                <th>排序</th>
            </tr>
        </thead>
        <tbody>
            <%if( null != Model)
              {
                  int listIndex = 0;
                  foreach(var item in Model)
                  { %>
            <tr>
                <td><input type="hidden" name="[<%:listIndex %>].gm001" value="<%:item.gm001 %>" /><%:item.gm001 %></td>
                <td><input type="text" name="[<%:listIndex %>].g001" value="<%:item.g001 %>" class="input-text w30px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gm002" value="<%:item.gm002 %>" class="input-text w30px" /><a href="/AM/GameMethod?gameID=<%:gameID %>&parentID=<%:item.gm001 %>">下级</a></td>
                <td><input type="text" name="[<%:listIndex %>].gmg001" value="<%:item.gmg001 %>" class="input-text w30px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gm004" value="<%:item.gm004 %>" class="input-text w100px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gm005" value="<%:item.gm005 %>" class="input-text w100px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gm007" value="<%:item.gm007 %>" class="input-text w100px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gm008" value="<%:item.gm008 %>" class="input-text w100px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gm009" value="<%:item.gm009 %>" class="input-text w100px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gm006" value="<%:item.gm006 %>" class="input-text w50px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gm010" value="<%:item.gm010 %>" class="input-text w50px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gm011" value="<%:item.gm011 %>" class="input-text w50px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gm012" value="<%:item.gm012 %>" class="input-text w50px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gm013" value="<%:item.gm013 %>" class="input-text w50px" /></td>
            </tr>
            <%
                      listIndex++;
                  }/*foreach*/
            }/*if*/ %>
        </tbody>
    </table>

    <div class="blank-line"></div>

    <div id="cjlsoft-bottom-function">
        <input type="submit" value="保存" class="cjlsoft-post-loading btn-normal" />
    </div>
    </form>
</asp:Content>
