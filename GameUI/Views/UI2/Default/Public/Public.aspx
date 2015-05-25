<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI2/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server"><%
    var error = ViewData["Error"] == null ? 1 : (int)ViewData["Error"];
%>
<%if (1 == error)
  { %>
<%
    var data = new ViewDataDictionary();
    data.Add("Message", ViewData["Message"]);
%>
<%=Html.Partial(ViewData["ControllerTheme"]+"/Common/Message",data) %>
<%
}/*if 0 == error*/%></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>