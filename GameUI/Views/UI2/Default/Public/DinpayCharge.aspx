<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI2/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    选择充值银行
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
        var dicDinpayConfig = (Dictionary<int, string>)ViewData["DicSysPayDinpayConfig"];
    %>
<div id="post_pay">
    <form method="post" action="<%=dicDinpayConfig[3] %>">
    <input type="hidden" name="method" value="dinpayBank" />
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
            <td class="title">银行</td>
            <td>
                <select name="bank_code">
                    <option value="ABC">农业银行</option>
                    <option value="ICBC">工商银行</option>
                    <option value="CCB">建设银行</option>
                    <option value="BCOM">交通银行</option>
                    <option value="BOC">中国银行</option>
                    <option value="CMB">招商银行</option>
                    <option value="CMBC">民生银行</option>
                    <option value="CEBB">光大银行</option>
                    <option value="CIB">兴业银行</option>
                    <option value="PSBC">中国邮政</option>
                    <option value="SPABANK">平安银行</option>
                    <option value="ECITIC">中信银行</option>
                    <option value="GDB">广东发展银行</option>
                    <option value="SDB">深圳发展银行</option>
                    <option value="HXB">华夏银行</option>
                    <option value="SPDB">浦发银行</option>
                    <option value="BEA">东亚银行</option>
                </select>
            </td>
        </tr>
        <tr>
            <td class="title"></td>
            <td><input type="submit" value="去支付" id="gopay" class="btn-normal" /></td>
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