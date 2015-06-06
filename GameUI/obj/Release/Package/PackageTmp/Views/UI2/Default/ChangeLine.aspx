<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI2/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var p = (string)ViewData["OLP"];
        Dictionary<string, int> dicAll = new Dictionary<string, int>();
        var pk = p.Split(',');
        decimal sum = 0.0000m;
        Random rand = new Random();
        foreach (var item in pk)
        {
            sum += decimal.Parse(item.Split('|')[1]);
            dicAll.Add(item.Split('|')[0], int.Parse(item.Split('|')[1]));
        }
        var list = dicAll.OrderByDescending(exp => exp.Value).ToList();
    %>
    <div class="ui_error_block" style="width:100%;text-align:center;padding-top:120px">
        <h1>切换线路不需要重新登录</h1>
        <div class="ui_error_content">
            <ul class="fs14px">
                <%foreach (var item in list)
                  {
                      decimal percent = 0.0000m;
                      if (item.Value > 0)
                      {
                          percent = (item.Value / sum) * 100;
                      }
                      %>
                <li><a target="_parent" class="ui-post-loading" style="color:green; padding:5px;" href="http://<%:item.Key %>/UI2/ChangeLine?SSOKey=<%:ViewData["Key"] %>&Account=<%:ViewData["Account"] %>"><%:item.Key %></a>（在线率<%=percent.ToString("N4")  %>）</li>
                <%} %>
            </ul>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
