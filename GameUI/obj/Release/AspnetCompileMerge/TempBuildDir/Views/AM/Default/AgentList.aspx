<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs012>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th class="w50px">编号</th>
                    <th class="w120px">账号</th>
                    <th class="w100px">注册日期</th>
                    <th>可用金额</th>
                    <th>使用度额</th>
                    <th>冻结金额</th>
                    <th>积分</th>
                    <th>VIP</th>
                    <th>状态</th>
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
                    <td><input type="hidden" name="[<%:listIndex %>].sm001" value="<%:item.u001 %>" /><%:item.u001 %></td>
                    <td><%:item.u002.Trim() %><%:item.u003!=null ? "（"+item.u003.Trim()+"）" : "" %></td>
                    <td><%:item.u005.ToString() %></td>
                    <td><%:item.wgs014.uf001 %></td>
                    <td><%:item.wgs014.uf002 %></td>
                    <td><%:item.wgs014.uf003 %></td>
                    <td><%:item.wgs014.uf004 %></td>
                    <td><%:item.u015 %></td>
                    <td><%:item.u008 %></td>
                    <td class="link-tools"><a href="javascript:window.parent.parent.cjlsoft_show_tab('<%:item.u002.Trim() %>充值','/AM/AgentCharge?key=<%:item.u001 %>',true);" title="充值">充值</a><a href="#" title="改密">改密</a><a href="#" title="限制">限制</a></td>
                </tr>
                <%
                      listIndex++;
                  }/*foreach*/
              }/*if*/ %>
            </tbody>
        </table>
    <div class="blank-line"></div>
    <%=ViewData["PageList"] %>
</asp:Content>
