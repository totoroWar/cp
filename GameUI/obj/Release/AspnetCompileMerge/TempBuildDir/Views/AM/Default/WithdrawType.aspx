<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var wtList = (List<DBModel.wgs024>)ViewData["WTList"];
%>
    <div class="cjlsoft-body-header">
        <h1>提现类型</h1>
        <div class="left-nav">
            <a id="a-menu" href="/AM/WithdrawType?method=add" title="添加提现类型">添加提现类型</a>
        </div>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
    </div>
    <div class="blank-line"></div>
    <form action="/AM/Menu" method="post">
        <%:Html.AntiForgeryToken()%>
        <table width="100%" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th class="w50px">编号</th>
                    <th>名称</th>
                    <th>显示名称</th>
                    <th>状态</th>
                    <th>排序</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <%if (null != wtList)
                  {
                      int listIndex = 0;
                      foreach (var item in wtList)
                      { %>
                <tr>
                    <td><%:item.uwt001 %></td>
                    <td><%:item.uwt002 %></td>
                    <td><%:item.uwt003 %></td>
                    <td><%=item.uwt004 == 1 ? "<span class='fc-green'>启用</span>" : "<span class='fc-red'>停用</span>" %></td>
                    <td><%:item.uwt006 %></td>
                    <td class="link-tools"><a href="/AM/WithdrawType?method=edit&key=<%:item.uwt001 %>">编辑</a></td>
                </tr>
                <%
                      listIndex++;
                  }/*foreach*/
              }/*if*/ %>
            </tbody>
        </table>
    </form>
</asp:Content>
