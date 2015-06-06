<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %><asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            jQuery('#i_dts').datetimepicker({
                format: 'Y/m/d',
                lang: "ch",

                timepicker: true
            });
            jQuery('#i_dte').datetimepicker({
                format: 'Y/m/d',
                lang: "ch",

                timepicker: true
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var methodType = (string)ViewData["MethodType"];
%>
<div class="cjlsoft-body-header">
        <h1>盈亏</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
</div>
<div class="blank-line"></div>
<div class="xtool">
        <form method="get" action="/AM/Report">
        <input type="hidden" name="method" value="<%:methodType %>" />
        账号<input type="text" name="account" class="input-text w100px" value="<%:ViewData["Account"] %>" />
        时间范围<input id="i_dts" name="dts" type="text" class="input-text w80px" value="<%:((DateTime)ViewData["DTS"]).ToString(ViewContext.ViewData["SysDateFormat"].ToString()) %>" />
            <input id="i_dte" name="dte" type="text" class="input-text w80px" value="<%:((DateTime)ViewData["DTE"]).ToString(ViewContext.ViewData["SysDateFormat"].ToString()) %>" />
        汇总<input name="total" type="checkbox" value="1" />
        <input type="submit" class="btn-normal" value="统计" />
        </form>
</div>
<div class="blank-line"></div>
<%
    var dayReportList = (List<DBModel.wgs042>)ViewData["DayReportList"];
    decimal linedr005Sum = 0;
    decimal linedr004Sum = 0;
    decimal linedr006Sum = 0;
    decimal linedr007Sum = 0;
    decimal linedr008Sum = 0;
    decimal linedr010Sum = 0;
    decimal linedr011Sum = 0;
    decimal linedr013Sum = 0;
    decimal linedr014Sum = 0;
    decimal linedr015Sum = 0;
    decimal linedr016Sum = 0;
    decimal lineSumTotal = 0;    
%>
<table class="table-pro tp5 w100ps">
        <thead>
            <tr class="table-pro-head">
                <th>账号</th>
                <th>更新时间</th>
                <th>充值金额</th>
                <th>下注金额</th>
                <th>奖金金额</th>
                <th>返点金额</th>
                <th>赠送金额</th>
                <th>分红金额</th>
                <th>提现金额</th>
                <th>充值手续费</th>
                <th>提现手续费</th>
                <!--<th>获取积分</th>
                <th>消费积分</th>-->
                <th>盈利</th>
            </tr>
        </thead>
        <tbody>
            <%if( null != dayReportList)
              {
                  foreach( var item in dayReportList)
                  {
                 %>
            <tr>
              <%
                      //(奖金+返点)-下单金额
                      decimal lineSum = (item.dr006 + item.dr007) - (item.dr004);
                  lineSumTotal += lineSum;
                  linedr005Sum += item.dr005;
                  linedr004Sum += item.dr004;
                  linedr006Sum += item.dr006;
                  linedr007Sum += item.dr007;
                  linedr008Sum += item.dr008;
                  linedr010Sum += item.dr010;
                  linedr011Sum += item.dr011;
                  linedr013Sum += item.dr013;
                  linedr014Sum += item.dr014;
                  linedr015Sum += item.dr015;
                  linedr016Sum += item.dr016;
               %>
                <td><%:item.u002.Trim() %></td>
                <td><%:item.dr003.ToString(ViewContext.ViewData["SysDateTimeFormat"].ToString()) %></td>
                <td><%:item.dr005 %></td>
                <td><%:item.dr004 %></td>
                <td><%:item.dr006 %></td>
                <td><%:item.dr007 %></td>
                <td><%:item.dr011 %></td>
                <td><%:item.dr016 %></td>
                <td><%:item.dr010 %></td>
                <td><%:item.dr014 %></td>
                <td><%:item.dr015 %></td>
                <!--<td><%:item.dr008 %></td>
                <td><%:item.dr013 %></td>-->
                <td><%:lineSum %></td>
            </tr>
            <%
            }/*foreach*/
            }/*if*/ %>
        </tbody>
        <tfoot>
            <tr>
                <td>合计</td>
                <td></td>
                <td><%:linedr005Sum %></td>
                <td><%:linedr004Sum %></td>
                <td><%:linedr006Sum %></td>
                <td><%:linedr007Sum %></td>
                <td><%:linedr011Sum %></td>
                <td><%:linedr016Sum %></td>
                <td><%:linedr010Sum %></td>
                <td><%:linedr014Sum %></td>
                <td><%:linedr015Sum %></td>
                 <!--<td><%:linedr008Sum %></td>
                <td><%:linedr013Sum %></td>-->
                <td><%:lineSumTotal %></td>
            </tr>
        </tfoot>
    </table>
</asp:Content>
