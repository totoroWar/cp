<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="cjlsoft-body-header">
        <h1>编辑菜单</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
</div>
    <div class="blank-line"></div>
    <form action="/AM/Menu" method="post">
        <%:Html.AntiForgeryToken()%>
    <input type="hidden" name="sm001" value="0" />
    <input type="hidden" name="method" value="<%:ViewData["MethodType"] %>" />
    <table class="table-pro" width="100%">
        <tbody>
            <tr>
                <td class="title">父级</td><td><input type="text" name="sm002" class="input-text w300px" value="<%:ViewData["ParentID"] %>" /></td>
            </tr>
            <tr>
                <td class="title">名称</td><td><input type="text" name="sm003" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">显示名称</td><td><input type="text" name="sm004" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">类型</td><td><input type="text" name="sm005" class="input-text w300px" value="<%:ViewData["MenuType"] %>" /><span class="tips">0管理，1用户</span></td>
            </tr>
            <tr>
                <td class="title">控制器</td><td><input type="text" name="sm006" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">动作</td><td><input type="text" name="sm007" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">参数</td><td><input type="text" name="sm008" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">启用</td><td><input type="text" name="sm009" class="input-text w300px" value="1" /><span class="tips">0禁用、1启用</span></td>
            </tr>
            <tr>
                <td class="title">权限</td><td><input type="text" name="sm010" class="input-text w300px" value="0" /><span class="tips">0作为菜单，1作为隐藏</span></td>
            </tr>
            <tr>
                <td class="title">打开类型</td><td><input type="text" name="sm011" class="input-text w300px" value="0" /><span class="tips">0链接型，1新窗口，3对话框，10无类型</span></td>
            </tr>
            <tr>
                <td class="title">打开类型</td><td><input type="text" name="sm013" class="input-text w300px" value="1" /><span class="tips">0不验证，1验证</span></td>
            </tr>
            <tr>
                <td class="title">打开类型</td><td><input type="text" name="sm014" class="input-text w300px" value="1" /><span class="tips">0不记录，1记录</span></td>
            </tr>
            <tr>
                <td class="title">排序</td><td><input type="text" name="sm012" class="input-text w300px" value="0" /></td>
            </tr>
            <tr>
                <td class="title"></td><td><input type="submit" value="保存" class="cjlsoft-post-loading btn-normal" /></td>
            </tr>
        </tbody>
    </table>
    </form>
</asp:Content>
