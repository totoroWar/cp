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
          var dicSysPayIPSConfig = (Dictionary<int, string>)ViewData["dicSysPayIPSConfig"];
    %>
<div id="post_pay">
    <form method="post" action="<%=dicSysPayIPSConfig[17] %>"">
    <input type="hidden" name="method" value="ipspayToPost" />
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