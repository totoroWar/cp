<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI2/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    在线充值
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%if( (int)ViewData["Error"] == 1)
      { %>
<%
    var data = new ViewDataDictionary();
    data.Add("Message", ViewData["Message"]);
%>
<%=Html.Partial(ViewData["ControllerTheme"]+"/Common/Message",data) %>
    <%}else { %>
    <%
          var dicYeepayConfig = (Dictionary<int, string>)ViewData["DicSysPayYeepayConfig"];
    %>
<div id="post_pay">
    <form method="post" action="<%=dicYeepayConfig[8] %>">
    <input type="hidden" name="method" value="yeepayToPost" />
    <input type="hidden" name="encodeinfo" value="<%:ViewData["EncodeInfo"] %>" />
    <table class="table-pro tp5 w100ps">
        <tr>
            <td class="title">账号</td>
            <td><%:ViewData["Account"] %></td>
        </tr>
        <tr>
            <td class="title">充值码</td>
            <td><%:ViewData["Key"] %><span class="tipsline">请牢记此编号，如对充值有疑问请用此编号联系客服</span></td>
        </tr>
        <tr>
            <td class="title">金额</td>
            <td><%:ViewData["Money"] %></td>
        </tr>
        <tr>
            <td class="title">时间</td>
            <td><%:ViewData["DateTime"] %></td>
        </tr>
        <tr>
            <td class="title">储蓄卡/信用卡</td>
            <td>
                <select name="bank_card">
                    <%
          var c1 = "ICBC-NET-B2C:工商银行,CMBCHINA-NET-B2C:招商银行,ABC-NET-B2C:中国农业银行,CCB-NET-B2C:建设银行,BCCB-NET-B2C:北京银行,BOCO-NET-B2C:交通银行,CIB-NET-B2C:兴业银行,CMBC-NET-B2C:中国民生银行,CEB-NET-B2C:光大银行,BOC-NET-B2C:中国银行,PINGANBANK-NET-B2C:平安银行,ECITIC-NET-B2C:中信银行,SDB-NET-B2C:深圳发展银行,GDB-NET-B2C:广发银行,SHB-NET-B2C:上海银行,SPDB-NET-B2C:上海浦东发展银行,POST-NET-B2C:中国邮政,BJRCB-NET-B2C:北京农村商业银行,HXB-NET-B2C:华夏银行,PINGANBANK-NET-B2C:平安银行";
          var c1Split = c1.Split(',');
          foreach(var item in c1Split)
          {
              var itemSplit = item.Split(':');
                    %>
                    <option value="<%:itemSplit[0] %>" title="<%:itemSplit[1] %>"><%:itemSplit[1] %></option>
                    <%} %>

                    <%
          var c2 = "ICBC-KUAICREDIT:（信用卡）工商银行,ABC-KUAICREDIT:（信用卡）农业银行,CCB-KUAICREDIT:（信用卡）建设银行,BOC-KUAICREDIT:（信用卡）中国银行,CEB-KUAICREDIT:（信用卡）光大银行,SPDB-KUAICREDIT:（信用卡）浦发银行,CMBC-KUAICREDIT:（信用卡）民生银行,POST-KUAICREDIT:（信用卡）邮储银行,BOSH-KUAICREDIT:（信用卡）上海银行,HXB-KUAICREDIT:（信用卡）华夏银行,CIB-KUAICREDIT:（信用卡）兴业银行,GDB-KUAICREDIT:（信用卡）广发银行,CPB-KUAICREDIT:（信用卡）平安银行";
          var c2Split = c2.Split(',');
          foreach(var item in c2Split)
          {
              var itemSplit = item.Split(':');
                    %>
                    <option value="<%:itemSplit[0] %>" title="<%:itemSplit[1] %>"><%:itemSplit[1] %></option>
                    <%} %>
                </select>
            </td>
        </tr>
        <tr>
            <td class="title"></td>
            <td>
                <input type="submit" value="去支付" id="gopay" class="btn-normal" /></td>
        </tr>
    </table>
    </form>
</div>
<script type="text/javascript">
    $('#gopay').click(function () {
        $('#post_pay').block({
            message: '<h1>处理中……</h1>',
            css: { border: '3px solid #000' }
        });
    });
    </script>
<%} %>
</asp:Content>