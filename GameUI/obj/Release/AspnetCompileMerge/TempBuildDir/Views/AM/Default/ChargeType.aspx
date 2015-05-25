<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs009>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cjlsoft-body-header">
        <h1>充值方式</h1>
        <div class="left-nav">
            <a id="a-menu" href="/AM/ChargeType?method=add" title="添加充值方式">添加充值方式</a>
        </div>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
    </div>
    <div class="blank-line"></div>
    <form action="/AM/ChargeType" method="post">
        <%:Html.AntiForgeryToken()%>
        <input type="hidden" name="method" value="updateList" />
        <table width="100%" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th class="w50px">编号</th>
                    <th>银行</th>
                    <th>姓名</th>
                    <th>账号</th>
                    <th>开户行</th>
                    <th>状态</th>
                    <th>标识</th>
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
                    <td><%:item.ct001 %></td>
                    <td><%:item.sb001 %></td>
                    <td><%:item.ct002 %></td>
                    <td><%:item.ct003 %></td>
                    <td><%:item.ct004 %></td>
                    <td><%:item.ct012 == 1 ? "正常" : "停用" %></td>
                    <td><%:item.ct011 %></td>
                    <td class="link-tools"><a href="/AM/ChargeType?method=edit&key=<%:item.ct001 %>">编辑</a></td>
                </tr>
                <%
                      listIndex++;
                  }/*foreach*/
              }/*if*/ %>
            </tbody>
        </table>
    </form>
</asp:Content>
