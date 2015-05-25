<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs015>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cjlsoft-body-header">
        <h1>管理组</h1>
        <div class="left-nav">
            <a id="a-menu" href="/AM/ManagerGroup?method=add" title="添加管理组">添加管理组</a>
        </div>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
    </div>
    <div class="blank-line"></div>
    <form action="#" method="post">
        <%:Html.AntiForgeryToken()%>
        <input type="hidden" name="method" value="updateList" />
        <table width="100%" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th>编号</th>
                    <th>名称</th>
                    <th>显示名称</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <%if (null != Model)
                  {
                      int listIndex = 0;
                      foreach (var item in Model)
                      { %>
                <tr>
                    <td>
                        <input type="hidden" name="[<%:listIndex %>].sm001" value="<%:item.pg001 %>" /><%:item.pg001 %></td>
                    <td>
                        <input type="hidden" name="[<%:listIndex %>].sm001" value="<%:item.pg003 %>" /><%:item.pg003 %></td>
                    <td>
                        <input type="hidden" name="[<%:listIndex %>].sm001" value="<%:item.pg004 %>" /><%:item.pg004 %></td>
                    <td><a href="/AM/ManagerGroup?method=edit&key=<%:item.pg001 %>">编辑</a></td>
                </tr>
                <%
                      listIndex++;
                  }/*foreach*/
              }/*if*/ %>
            </tbody>
        </table>
    </form>
</asp:Content>
