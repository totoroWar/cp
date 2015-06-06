<%@ Import Namespace="DBModel" %><%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs006>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    
%>
<div class="cjlsoft-body-header">
        <h1>游戏分类</h1>
        <div class="left-nav">
            <a id="a-menu" href="/AM/GameClass?method=add" title="添加分类">添加分类</a>
        </div>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
</div>
    <div class="blank-line"></div>
    <form action="/AM/GameClass" method="post">
        <%:Html.AntiForgeryToken()%>
        <input type="hidden" name="method" value="updateList" />
    <table width="100%" class="table-pro table-color-row">
        <thead>
            <tr class="table-pro-head">
                <th>编号</th>
                <th>名称</th>
                <th>显示名称</th>
                <th>包含游戏</th>
                <th>开启</th>
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
                <td><input type="hidden" name="[<%:listIndex %>].gc001" value="<%:item.gc001 %>" /><%:item.gc001 %></td>
                <td><input type="text" name="[<%:listIndex %>].gc002" value="<%:item.gc002 %>" class="input-text w100px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gc003" value="<%:item.gc003 %>" class="input-text w100px" /></td>
                <td>
                    <%
                      List<DBModel.wgs001> gameTitleList = (List<DBModel.wgs001>)ViewData["GameOriList"];
                      var classIDs = item.gc004.Split(',').ToList();
                      foreach (var gt in gameTitleList)
                      {
                          if (false == classIDs.Exists(exp => exp == gt.g001.ToString()))
                          {
                              continue; 
                          }
                    %>
                    <span style="padding:0 5px;"><%:gt.g003 %></span>
                    <%} %>
                </td>
                <td><input type="text" name="[<%:listIndex %>].gc006" value="<%:item.gc006 %>" class="input-text w50px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gc005" value="<%:item.gc005 %>" class="input-text w50px" /></td>
                <td><a href="/AM/GameClass?method=edit&gameClassID=<%:item.gc001 %>">修改</a></td>
            </tr>
            <%
                      listIndex++;
                  }/*foreach*/
            }/*if*/ %>
        </tbody>
    </table>
    </form>
</asp:Content>
