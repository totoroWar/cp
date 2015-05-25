<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI2/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var data = new ViewDataDictionary();
    data.Add("Message", ViewData["GameMessage"]);
%>
<%=Html.Partial(ViewData["ControllerTheme"]+"/Common/Message",data) %>					
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <%--<div style="text-align:center;width:70%;height:100%"></div>--%>
</asp:Content>
