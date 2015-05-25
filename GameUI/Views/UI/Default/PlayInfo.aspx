<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %><asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var data = new ViewDataDictionary();
    data.Add("Message", ViewData["GameMessage"]);
%>
<%=Html.Partial(ViewData["ControllerTheme"]+"/Common/Message",data) %>
</asp:Content>