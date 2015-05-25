<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs016>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cjlsoft-body-header">
        <h1>管理</h1>
        <div class="left-nav">
            <a id="a-menu" href="/AM/Manager?method=add" title="增加管理">增加管理</a>
        </div>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
    </div>
    <div class="blank-line"></div>
    <form action="/AM/Manager" method="post">
        <%:Html.AntiForgeryToken()%>
        <input type="hidden" name="method" value="updateList" />
        <table width="100%" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th>编号</th>
                    <th>账号</th>
                    <th>权限</th>
                    <th>状态</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <%if (null != Model)
                  {
                      var pgList = (List<DBModel.wgs015>)ViewData["PGList"];
                      Dictionary<int, DBModel.wgs015> dicPG = pgList.ToDictionary(key=>key.pg001);
                      int listIndex = 0;
                      foreach (var item in Model)
                      { %>
                <tr>
                    <td><%:item.mu001 %></td>
                    <td><%:item.mu002 %><span class="tips"><%:item.mu003 %></span></td>
                    <td><%:item.pg001 == 0 ? "无任何权限" : dicPG[item.pg001].pg003 %></td>
                    <td><%:item.mu006 == 1 ? "正常" : "停用" %></td>
                    <td><a href="/AM/Manager?method=edit&key=<%:item.mu001 %>">编辑</a></td>
                </tr>
                <%
                      listIndex++;
                  }/*foreach*/
              }/*if*/ %>
            </tbody>
        </table>
    </form>
</asp:Content>
