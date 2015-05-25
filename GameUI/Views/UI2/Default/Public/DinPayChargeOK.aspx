<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI2/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %><asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"><%:ViewData["GlobalTitle"] %>支付结果</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var error = (int)ViewData["Error"];
    var message = (string)ViewData["Message"];
%>
<%
    var data = new ViewDataDictionary();
    data.Add("Message", message);
    if( 1 == error)
    { 
%>
    <%=Html.Partial(ViewData["ControllerTheme"]+"/Common/Message",data) %>
    <%} %>
    <%if( 0 == error){ %>
    <table class="table-pro tp10 w100ps">
        <tr>
            <td class="title">账号</td>
            <td class="fs16px"><%:ViewData["Account"] %></td>
        </tr>
        <tr>
            <td class="title">充值码</td>
            <td class="fc-red fs16px"><%:ViewData["ChargeCode"] %><span class="tipsline">请牢记此编号，如对充值有疑问请用此编号联系客服</span></td>
        </tr>
        <tr>
            <td class="title">金额</td>
            <td class="fc-red fs16px"><%:ViewData["Money"] %></td>
        </tr>
        <tr>
            <td class="title">时间</td>
            <td><%:ViewData["ChargeOKDateTime"] %></td>
        </tr>
        <tr>
            <td class="title">状态</td>
            <td class="fc-green fs16px">充值成功</td>
        </tr>
    </table>
    <%} %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>