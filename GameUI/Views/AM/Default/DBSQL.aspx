<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    DBSQL
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<form action="/AM/DBSQL" method="post">
<table class="table-pro w100ps">
    <tr>
        <td class="title">SQL</td>
        <td>
            <textarea name="sql" class="input-text" rows="15" cols="128"></textarea>
        </td>
    </tr>
    <tr>
        <td class="title">Auth</td>
        <td>
            <input type="text" name="auth" class="input-text w200px" />
        </td>
    </tr>
    <tr>
        <td class="title">Message</td>
        <td class="fc-blue">
            <%=ViewData["Message"] %>
        </td>
    </tr>
    <tr>
        <td class="title"></td>
        <td>
            <input type="submit" value="Run" />
        </td>
    </tr>
</table>
</form>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
