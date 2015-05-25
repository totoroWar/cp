<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs021>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cjlsoft-body-header">
        <h1>账变</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
    </div>
    <div class="blank-line"></div>
    <div class="cjlsoft-body-header tools">
        <span>
            账号<input type="text" class="input-text w100px" value="" />
        </span>
        <span>
            开始日期<input type="text" class="input-text w100px" value="" />
        </span>
        <span>
            结束日期<input type="text" class="input-text w100px" value="" />
        </span>
        <input id="btn_query" type="button" value="筛选" class="btn-normal" />
    </div>
    <div class="blank-line"></div>
    <form action="/AM/Menu" method="post">
        <%:Html.AntiForgeryToken()%>
        <input type="hidden" name="method" value="updateList" />
        <table width="100%" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th>编号</th>
                    <th>账号</th>
                    <th>金额</th>
                    <th>改变前金额</th>
                    <th>积分</th>
                    <th>改变前积分</th>
                    <th>游戏</th>
                    <th>玩法</th>
                    <th>充值方式</th>
                    <th>账变时间</th>
                </tr>
            </thead>
            <tbody>
                <%if (null != Model)
                  {
                      int listIndex = 0;
                      foreach (var item in Model)
                      { %>
                <tr>
                    <td><%:item.uxf001 %></td>
                    <td><%:item.u002 %></td>
                    <td class="fc-red"><%:item.uxf003 %></td>
                    <td><%:item.uxf002 %></td>
                    <td><%:item.uxf005 %></td>
                    <td><%:item.uxf004 %></td>
                    <td><%:item.g001 %></td>
                    <td><%:item.gm001 %></td>
                    <td><%:item.uc001 %></td>
                    <td><%:item.uxf014 %></td>
                </tr>
                <%
                      listIndex++;
                  }/*foreach*/
              }/*if*/ %>
            </tbody>
        </table>
        <%=ViewData["PageList"] %>
    </form>
</asp:Content>
