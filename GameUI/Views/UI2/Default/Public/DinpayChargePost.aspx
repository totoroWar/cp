<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI2/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    正在充值
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<form action="<%=ViewData["PostGate"] %>" id="dinpayForm" name="dinpayForm" method="post">
            <input type="hidden" name="sign" id="sign"  value="<%=ViewData["sign"] %>" />
            <input type="hidden" name="merchant_code" id="merchant_code" value="<%=ViewData["merchant_code"] %>" />
            <input type="hidden" name="bank_code" id="bank_code" value="<%=ViewData["bank_code"] %>" />
            <input type="hidden" name="order_no" id="order_no" value="<%=ViewData["order_no"] %>" />
            <input type="hidden" name="order_amount" id="order_amount" value="<%=ViewData["order_amount"] %>" />
            <input type="hidden" name="service_type" id="service_type" value="<%=ViewData["service_type"] %>" />
            <input type="hidden" name="input_charset" id="input_charset" value="<%=ViewData["input_charset"] %>" />
            <input type="hidden" name="notify_url" id="notify_url" value="<%=ViewData["notify_url"] %>" />
            <input type="hidden" name="interface_version" id="interface_version" value="<%=ViewData["interface_version"] %>" />
            <input type="hidden" name="sign_type" id="sign_type" value="<%=ViewData["sign_type"] %>" />
            <input type="hidden" name="order_time" id="order_time" value="<%=ViewData["order_time"] %>" />
            <input type="hidden" name="product_name" id="product_name" value="<%=ViewData["product_name"] %>" />
            <input type="hidden" name="client_ip" id="client_ip" value="<%=ViewData["client_ip"] %>" />
            <input type="hidden" name="extend_param" id="extend_param" value="<%=ViewData["extend_param"] %>" />
            <input type="hidden" name="extra_return_param" id="extra_return_param" value="<%=ViewData["extra_return_param"] %>" />
            <input type="hidden" name="product_code" id="product_code" value="<%=ViewData["product_code"] %>" />
            <input type="hidden" name="product_desc" id="product_desc" value="<%=ViewData["product_desc"] %>" />
            <input type="hidden" name="product_num" id="product_num" value="<%=ViewData["product_num"] %>" />
            <input type="hidden" name="return_url" id="return_url" value="<%=ViewData["return_url"] %>" />
            <input type="hidden" name="show_url" id="show_url" value="<%=ViewData["show_url"] %>" />
        </form>
            <%if("0" == (string)ViewData["Debug"]){ %>
            <script type="text/javascript">
                document.getElementById("dinpayForm").submit();
            </script>
            <%}else{ %>
            <%=ViewData["SignInfo"] %>
            <%} %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
