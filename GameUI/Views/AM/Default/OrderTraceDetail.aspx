<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %><asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content><asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
        var methodType = (string)ViewData["MethodType"];
        var traceObject = (DBModel.TraceOrderDetail)ViewData["TraceObject"];
        var gameList = (List<DBModel.wgs001>)ViewData["GameList"];
        var gameDicList = gameList.ToDictionary(exp => exp.g001);
    %>
    <table class="table-pro tp5 w100ps g_nco">
        <tr>
            <td class="title">账号</td>
            <td><%:traceObject.TraceOrder.u002.Trim() %></td>
        </tr>
        <tr>
            <td class="title">游戏</td>
            <td><%:gameDicList[traceObject.TraceOrder.g001].g003.Trim() %></td>
        </tr>
        <tr>
            <td class="title">时间</td>
            <td><%:traceObject.TraceOrder.sto004.ToString("yyyy-MM-dd HH:mm:ss") %></td>
        </tr>
        <tr>
            <td class="title">开始期号</td>
            <td><%:traceObject.TraceOrder.sto010.Trim() %></td>
        </tr>
        <tr>
            <td class="title">结束期号</td>
            <td><%:traceObject.TraceOrder.sto012.Trim() %></td>
        </tr>
        <tr>
            <td class="title">总单数</td>
            <td><%:traceObject.OrderShowList.Count() %></td>
        </tr>
        <tr>
            <td class="title">总金额</td>
            <td><%:traceObject.TraceOrder.sto002.ToString("N2")%></td>
        </tr>
        <tr>
            <td class="title">实际金额</td>
            <td><%:traceObject.TraceOrder.sto007.ToString("N2")%></td>
        </tr>
        <tr>
            <td class="title">盈利</td>
            <td><%:traceObject.TraceOrder.sto008.ToString("N2")%></td>
        </tr>
        <tr>
            <td class="title">状态</td>
            <td>
                    <%
                      var toStatus = string.Empty;
                      switch (traceObject.TraceOrder.sto005)
                      {
                          case 0:
                              toStatus = "<span class='fc-red'>未完成</span>";
                              break;
                          case 1:
                              toStatus = "<span class='fc-green'>已完成</span>";
                              break;
                          case 2:
                              toStatus = "<span class='fc-gray'>已撤单</span>";
                              break; 
                      }
                        %><%=toStatus %>
            </td>
        </tr>
        <tr>
            <td class="title">类型</td>
            <td><%=traceObject.TraceOrder.sto009 == 0 ? "连续追号" : "追中即停" %></td>
        </tr>
        <tr>
            <td class="title">详情</td>
            <td>
                <table class="table-pro tp3 w100ps">
                    <tr class="table-pro-head">
                        <th class="w100px">订单号</th>
                        <th class="w100px">玩法</th>
                        <th class="w80px">期号</th>
                        <th class="w100px">结果</th>
                        <th class="w30px">倍数</th>
                        <th class="w80px">总金额</th>
                        <th class="w80px">奖金</th>
                        <th>内容</th>
                        <th>状态</th>
                        <th></th>
                    </tr>
                    <%foreach(var item in traceObject.OrderShowList){ %>
                    <tr>
                        <td><%:item.projectid %></td>
                        <td><%:item.methodname %></td>
                        <td><%:item.issue %></td>
                        <td><%:item.nocode %></td>
                        <td><%:item.multiple %></td>
                        <td><%:item.totalprice.ToString("N2") %></td>
                        <td><%:item.bonus.ToString("N2")%></td>
                        <td>
                            <textarea rows="2" cols="20" class="input-text"><%:item.code %></textarea>
                        </td>
                        <td><%:item.statusdesc %></td>
                        <td><a href="javascript:;" name="cancel_order" data="<%:item.projectid %>">撤单</a></td>
                    </tr>
                    <%} %>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>