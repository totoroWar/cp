<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="cjlsoft-body-header">
<%
    var uf = (DBModel.wgs014)ViewData["UF"];
 %>
        <h1>代理充值</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
</div>
    <div class="blank-line"></div>
    <form action="/AM/AgentCharge" method="post">
        <%:Html.AntiForgeryToken()%>
    <input type="hidden" name="uc005" value="<%:ViewData["ChargetKey"] %>" />
    <input type="hidden" name="method" value="<%:ViewData["MethodType"] %>" />
    <table class="table-pro" width="100%">
        <tbody>
            <tr>
                <td class="title">金额</td><td><input type="text" name="uc002" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">可用余额</td><td><%:uf.uf001 %></td>
            </tr>
            <tr>
                <td class="title">冻结余额</td><td><%:uf.uf003 %></td>
            </tr>
            <tr>
                <td class="title">累计花销</td><td><%:uf.uf002 %></td>
            </tr>
            <tr>
                <td class="title">充值方式</td>
                <td>
                    <%
                        var ctList = (List<DBModel.wgs009>)ViewData["CTList"];
                        var listIndex = 0;
                        foreach (var b in ctList)
                        {
                     %>
                    <input type="radio" name="ct001" value="<%:b.ct001 %>" <%:listIndex==0 ? "checked='checked'": "" %> /><%:b.ct004 %>
                    <%
                            listIndex++;
                    } %>
                </td>
            </tr>
            <tr>
                <td class="title"></td><td><input type="submit" value="保存" class="cjlsoft-post-loading btn-normal" /></td>
            </tr>
        </tbody>
    </table>
    </form>
</asp:Content>
