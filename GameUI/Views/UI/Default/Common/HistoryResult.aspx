<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var gsrList = (List<DBModel.wgs038>)ViewData["GSRList"];
%>
<table class="table-pro" style="width:100%;">
    <tbody>
        <%foreach(var item in gsrList){ %>
        <tr>
            <td><%:item.gs002.Trim() %></td>
            <td style="color:yellow;"><%:item.gs007.Trim() %></td>
        </tr>
        <%} %>
    </tbody>
</table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
