<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    DBTInfo
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var tsList = (List<DBModel.SysTableSize>)ViewData["TSList"];
    tsList = tsList.OrderByDescending(exp => int.Parse(exp.rows)).ToList();
%>
<table width="100%" id="maindata" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th>name</th>
                    <th>rows</th>
                    <th>reserved</th>
                    <th>data</th>
                    <th>index_size</th>
                    <th>unused</th>
                </tr>
            </thead>
            <tbody>
                <% if( null !=  tsList)
                   {
                       foreach(var item in tsList)
                       {
                       %>
                <tr>
                    <td><%:item.name.Replace("wgs", "---") %></td>
                    <td><%:item.rows %></td>
                    <td><%:item.reserved %></td>
                    <td><%:item.data %></td>
                    <td><%:item.index_size %></td>
                    <td><%:item.unused %></td>
                </tr>
                <%}
                  } %>
            </tbody>
        </table>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
