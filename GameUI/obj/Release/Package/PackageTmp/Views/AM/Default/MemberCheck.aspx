<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    MemberCheck
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cjlsoft-body-header">
        <h1>异常分析</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
    </div>
    <div class="blank-line"></div>
    <form method="get" action="/AM/MemberCheck">
账号<input type="text" name="acct" id="acct" class="input-text w100px" value="<%=ViewData["Acct"] %>" /> 时间<input type="text" name="dts" id="dts" class="input-text w120px" value="<%:ViewData["DTS"] %>" />-<input type="text" name="dte" id="dte" class="input-text w120px" value="<%:ViewData["DTE"] %>" /><input type="submit" value="确认" class="btn-normal ui-post-loading" />
    </form>
    <div class="blank-line"></div>
    <%if( (int)ViewData["Show"] == 1)
      {
          var User = (DBModel.wgs012)ViewData["User"];
          var CLRC = (int)ViewData["CLRC"];
          var WCRC = (int)ViewData["WCRC"];
          var ORC = (int)ViewData["ORC"];
          var CList = (List<DBModel.wgs019>)ViewData["CList"];
          var WCList = (List<DBModel.wgs020>)ViewData["WCList"];
          var OList = (List<DBModel.LotteryHistoryOrder>)ViewData["OList"];
          decimal? oSum = OList.Where(exp => exp.iscancel == 0).Sum(exp => (decimal?)exp.totalprice);
          decimal? oCSum = OList.Where(exp => exp.iscancel == 1).Sum(exp=>(decimal?)exp.totalprice);
          int oBetRC = OList.Count(exp => exp.totalprice > 0 && exp.iscancel == 0);
          int oCRC = OList.Count(exp => exp.iscancel == 1);
          int oWRC = OList.Count(exp=>exp.isgetprize ==1 );
          int oPRC = OList.Count(exp => exp.resultpoint > 0 );
          decimal? oWSum = OList.Sum(exp=>(decimal?)exp.bonus);
          decimal? oPSum = OList.Sum(exp => (decimal?)exp.resultpoint);
          decimal? cSum = CList.Sum(exp => (decimal?)exp.uc003);
          decimal? wSum = WCList.Sum(exp => (decimal?)exp.uw002);
           %>
    <table class="table-pro w100ps">
        <tbody>
            <tr>
                <td class="title">账号</td><td><%=User.u002.Trim() %></td>
            </tr>
            <tr>
                <td class="title">注册时间</td><td><%=User.u005.ToString(ViewContext.ViewData["SysDateTimeFormat"].ToString()) %></td>
            </tr>
            <tr>
                <td class="title">登录时间</td><td><%=User.u007.Value.ToString(ViewContext.ViewData["SysDateTimeFormat"].ToString()) %></td>
            </tr>
            <tr>
                <td class="title">余额</td><td><%=User.wgs014.uf001.ToString("N4") %></td>
            </tr>
            <tr>
                <td class="title">冻结金额</td><td><%=User.wgs014.uf003.ToString("N4") %></td>
            </tr>
            <tr>
                <td class="title">积分</td><td><%=User.wgs014.uf004.ToString("N4") %></td>
            </tr>
            <tr>
                <td class="title">充值额/笔</td><td class="fc-green"><%=cSum.Value.ToString("N4") %>/<%=CLRC %></td>
            </tr>
            <tr>
                <td class="title">提现额/笔</td><td class="fc-red"><%=wSum.Value.ToString("N4") %>/<%=WCRC %></td>
            </tr>
            <tr>
                <td class="title">充提差</td><td class="<%= (wSum - cSum).Value > 0 ? "fc-red" : "fc-green" %>"><%=(wSum - cSum).Value.ToString("N4") %>（提现金额减充值金额）</td>
            </tr>
            <tr>
                <td class="title">所有订单</td><td><%=ORC %></td>
            </tr>
            <tr>
                <td class="title">有效订单</td><td><%=oBetRC %></td>
            </tr>
            <tr>
                <td class="title">撤单金额/笔</td><td><%=oCSum.Value.ToString("N4")%>/<%=oCRC %></td>
            </tr>
            <tr>
                <td class="title">下注金额/笔</td><td><%=oSum.Value.ToString("N4")%>/<%=oBetRC %></td>
            </tr>
            <tr>
                <td class="title">中奖金额/笔</td><td><%=oWSum.Value.ToString("N4")%>/<%=oWRC %></td>
            </tr>
            <tr>
                <td class="title">输赢</td><td class="<%=(oWSum-oSum).Value > 0 ? "fc-red" : "fc-green" %>"><%=(oWSum-oSum).Value.ToString("N4")%>（中奖金额减下注金额）</td>
            </tr>
            <tr>
                <td class="title">返点金额/笔</td><td><%=oPSum.Value.ToString("N4")%>/<%=oPRC %></td>
            </tr>
        </tbody>
    </table>
    <%} %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript">
    $(document).ready(function () {
        jQuery('#dts').datetimepicker({
            format: 'Y/m/d H:i:s',
            lang: "ch",
            onShow: function (ct) {
                this.setOptions({
                    maxDate: jQuery('#dte').val() ? jQuery('#dte').val() : false
                })
            },
            timepicker: true
        });
        jQuery('#dte').datetimepicker({
            format: 'Y/m/d H:i:s',
            lang: "ch",
            onShow: function (ct) {
                this.setOptions({
                    minDate: jQuery('#dts').val() ? jQuery('#dts').val() : false
                })
            },
            timepicker: true
        });
    });
</script>
</asp:Content>
