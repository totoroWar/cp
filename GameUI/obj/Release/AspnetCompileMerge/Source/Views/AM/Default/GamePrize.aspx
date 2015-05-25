<%@ Import Namespace="DBModel" %><%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs007>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var gameClassList = (List<DBModel.wgs006>)ViewData["GameClassList"];
    var gameClassID = (int)ViewData["GameClassID"];
%>
<div class="cjlsoft-body-header">
        <h1>游戏奖金组</h1>
        <div class="left-nav">
            <a id="a-menu" href="/AM/GameClassPrize?method=add&gameClassID=<%:gameClassID %>" title="添加奖金组">添加奖金组</a>
        </div>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
</div>
        <div class="blank-line"></div>
    <div class="cjlsoft-body-header tools">
        <select id="game" class="drop-select-to-url">
            <%foreach( var g in gameClassList){ %>
            <option value="<%:g.gc001 %>" title="<%:g.gc003 %>" tourl="<%:Url.Action("GameClassPrize", new { gameClassID=g.gc001 })%>" <%=g.gc001 == gameClassID ? "selected='selected'":"" %>><%:g.gc003 %></option>
            <%} %>
        </select>
    </div>
    <div class="blank-line"></div>
    <form action="/AM/GameClassPrize" method="post">
        <%:Html.AntiForgeryToken()%>
        <input type="hidden" name="method" value="updateList" />
    <table width="100%" class="table-pro table-color-row">
        <thead>
            <tr class="table-pro-head">
                <th>编号</th>
                <th>名称</th>
                <th>显示名称</th>
                <th>开同级个数</th>
                <th>不定位返点</th>
                <th>定位返点</th>
                <th>排序</th>
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
                <td><input type="hidden" name="[<%:listIndex %>].gp001" value="<%:item.gp001 %>" /><input type="hidden" name="[<%:listIndex %>].gc001" value="<%:item.gc001 %>" /><%:item.gp001 %></td>
                <td><input type="text" name="[<%:listIndex %>].gp002" value="<%:item.gp002 %>" class="input-text w100px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gp003" value="<%:item.gp003 %>" class="input-text w100px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gp004" value="<%:item.gp004 %>" class="input-text w30px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gp007" value="<%:item.gp007 %>" class="input-text w50px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gp008" value="<%:item.gp008 %>" class="input-text w50px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gp009" value="<%:item.gp009 %>" class="input-text w50px" /></td>
                <td><a href="javascript:window.parent.cjlsoft_show_tab('<%:gameClassList.Where(exp=>exp.gc001==item.gc001).FirstOrDefault().gc003 %>-<%:item.gp003 %>奖金配置','/AM/GameClassPrize?method=config&gameClassID=<%:item.gc001 %>&gameClassPrizeID=<%:item.gp001 %>',true);" title="配置">配置</a></td>
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
