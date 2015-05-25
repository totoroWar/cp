<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">系统授权</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p class="ls_line">已授权给<span><%:ViewData["GlobalTitle"] %></span></p>
    <p class="ls_line">使用期限<span><%:ViewData["SysRegLimit"] %>无限制</span></p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
<style type="text/css">
.ls_line{ font-size:16px; padding:10px 0; color:green;}
.ls_line span{ color:red;}
</style>
</asp:Content>