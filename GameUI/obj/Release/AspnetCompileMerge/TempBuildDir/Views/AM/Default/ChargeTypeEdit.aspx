<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var sbList = (List<DBModel.wgs010>)ViewData["SBList"];
    var editModel = (DBModel.wgs009)ViewData["EditModel"];
     %>
<div class="cjlsoft-body-header">
        <h1>编辑充值方式</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
</div>
    <div class="blank-line"></div>
    <form action="/AM/ChargeType" method="post">
        <%:Html.AntiForgeryToken()%>
    <input type="hidden" name="ct001" value="<%:editModel.ct001 %>" />
    <input type="hidden" name="method" value="<%:ViewData["MethodType"] %>" />
    <table class="table-pro" width="100%">
        <tbody>
            <tr>
                <td class="title">银行</td>
                <td>
                    <select name="sb001">
                        <%foreach(var sb in sbList)
                          { %>
                        <option value="<%:sb.sb001 %>" <%:editModel.sb001 == sb.sb001 ? "selected='selected'" : "" %>><%:sb.sb003 %></option>
                        <%} %>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="title">开户人</td><td><input type="text" name="ct002" class="input-text w300px fs16px" value="<%:editModel.ct002 %>" /></td>
            </tr>
            <tr>
                <td class="title">卡号/收款账号</td><td><input type="text" name="ct003" class="input-text w300px fs16px" value="<%:editModel.ct003 %>" /></td>
            </tr>
            <tr>
                <td class="title">开户行</td><td><input type="text" name="ct004" class="input-text w300px fs16px" value="<%:editModel.ct004 %>" /></td>
            </tr>
            <tr>
                <td class="title">标识</td><td><input type="text" name="ct011" class="input-text w300px fs16px" value="<%:editModel.ct011 %>" /></td>
            </tr>
            <tr>
                <td class="title">状态</td><td><input name="ct012" type="checkbox" value="1" <%:editModel.ct012 == 1 ? "checked='checked'" : "" %> /></td>
            </tr>
            <tr>
                <td class="title"></td><td><input type="submit" value="保存" class="cjlsoft-post-loading btn-normal" /></td>
            </tr>
        </tbody>
    </table>
    </form>
</asp:Content>
