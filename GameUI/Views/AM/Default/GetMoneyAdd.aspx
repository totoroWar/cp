<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <%
    var editModel = (DBModel.wgs056)ViewData["EditModel"];
%>
    <div class="cjlsoft-body-header">
        <h1>添加分红</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
</div>
     <div class="blank-line"></div>
<form action="/AM/GetMoneyAdd" method="post">   
    <input type="hidden" name="s001" value="<%:editModel.s001 %>" /> 
    <input type="hidden" name="s003" value="<%:editModel.s003 %>" />  
    <input type="hidden" name="s005" value="<%:editModel.s005 %>" />
    <input type="hidden" name="u001" value="<%:editModel.u001 %>" />
    <input type="hidden" name="s008" value="<%:editModel.s008 %>" />
    <input type="hidden" name="s009" value="<%:editModel.s009 %>" />
    <input type="hidden" name="s010" value="<%:editModel.s010 %>" />
    <input type="hidden" name="s011" value="<%:editModel.s011 %>" />
    <input type="hidden" name="s012" value="<%:editModel.s012 %>" />
    <input type="hidden" name="s013" value="<%:editModel.s013 %>" />
    <input type="hidden" name="s014" value="<%:editModel.s014 %>" />
    <input type="hidden" name="s015" value="<%:editModel.s015 %>" />
    <input type="hidden" name="s016" value="<%:editModel.s016 %>" />
     <table class="table-pro" width="100%">
        <tbody>
            <tr>
                <td class="title">分红账号</td><td><input type="text" name="u002" class="input-text w300px" value="<%:editModel.u002 %>"  /> 代理账户名</td>
            </tr>
            <tr>
                <td class="title">分红比例</td><td><input type="text" name="f002" class="input-text w300px" value="<%:editModel.f002 %>" /> %</td>
            </tr>
            <tr>
                <td class="title">分红周期</td><td><input type="text" name="s004" class="input-text w300px"  value="<%:editModel.s004 %>"  /> 天</td>
            </tr>
            <tr>
                <td class="title">分红盈亏</td><td><input type="text" name="s002" class="input-text w300px"  value="<%:editModel.s002 %>"  /> 参与分红的盈亏</td>
            </tr>
            <tr>
                <td class="title">周期开始时间</td><td><input type="text" id="s006" name="s006" class="input-text w300px" readonly="readonly" /></td>
            </tr>
            <tr>
                <td class="title">周期开始时间</td><td><input type="text" id="s007" name="s007" class="input-text w300px" readonly="readonly"/></td>
            </tr>
            <tr>
                <td class="title"></td><td><input type="submit" value="保存" class="cjlsoft-post-loading btn-normal" /></td>
            </tr>
        </tbody>
    </table>
    </form>

     <script type="text/javascript">
         $(document).ready(function () {
             jQuery('#s006').datetimepicker({
                 format: 'Y/m/d H:i:00',
                 lang: 'ch',
                 timepicker: true
             });
             jQuery('#s007').datetimepicker({
                 format: 'Y/m/d H:i:00',
                 lang: 'ch',
                 timepicker: true
             });
         })
            </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
