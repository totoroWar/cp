<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %><asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="/Scripts/ueditor/ueditor.config.js"></script>
    <script type="text/javascript" src="/Scripts/ueditor/ueditor.all.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var methodType = (string)ViewData["MethodType"];
%>
<div class="cjlsoft-body-header">
        <h1>系统公告</h1>
        <div class="left-nav">
            <a href="/AM/GlobalNotify?method=edit" title="添加">添加</a>
            <a href="/AM/GlobalNotify" title="列表">列表</a>
        </div>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
</div>
<div class="blank-line"></div>
<%if( string.Empty == methodType)
  {
      var notifyList = (List<DBModel.wgs040>)ViewData["NotifyList"];
      %>
<table cellpadding="0" cellspacing="0" border="0" class="table-pro w100ps">
    <thead>
        <tr class="table-pro-head">
            <th>编号</th>
            <th>标题</th>
            <th>发布时间</th>
            <th>状态</th>
            <th>排序</th>
            <th></th>
        </tr>
    </thead>
    <tbody>
        <%if( null != notifyList)
          {
              foreach( var item in notifyList)
              { %>
        <tr>
            <td><%:item.nt001 %></td>
            <td<%=string.IsNullOrEmpty(item.nt007) ? string.Empty : string.Format(" style='color:#{0};'", item.nt007.Trim())  %>><%:item.nt002 %></td>
            <td><%:item.nt004 %></td>
            <td><%:item.nt006 == 0 ? "隐藏" : "显示" %></td>
            <td><%:item.nt005 %></td>
            <td class="link-tools"><a href="/AM/GlobalNotify?method=edit&key=<%:item.nt001 %>">编辑</a><a href="/AM/GlobalNotify?method=delete&key=<%:item.nt001 %>">删除</a></td>
        </tr>
        <%}
          }/*if*/ %>
    </tbody>
</table>
<%} %>
<%else if( "edit"== methodType)
  {
      var editModel = (DBModel.wgs040)ViewData["EditModel"];
       %>
    <form action="/AM/GlobalNotify" method="post">
    <input name="nt001" type="hidden" value="<%:editModel == null ? 0 : editModel.nt001 %>" />
<table class="table-pro w100ps">
    <tr>
        <td class="title">标题</td>
        <td><input name="nt002" type="text" class="w200px input-text" value="<%:editModel == null ? string.Empty : editModel.nt002.Trim() %>" /></td>
    </tr>
    <tr>
        <td class="title">标题颜色</td>
        <td><input name="nt007" type="text" class="w200px input-text" value="<%:editModel == null ? string.Empty : editModel.nt007%>" /></td>
    </tr>
    <tr>
        <td class="title">发布时间</td>
        <td><input name="nt004" type="text" class="w200px input-text" value="<%:editModel == null ? DateTime.Now.ToString() : editModel.nt004.ToString() %>" /></td>
    </tr>
    <tr>
        <td class="title">排序</td>
        <td><input name="nt005" type="text" class="w200px input-text" value="<%:editModel == null ? 1 : editModel.nt005 %>" /></td>
    </tr>
    <tr>
        <td class="title">状态</td>
        <td>
            <%
      var status = editModel == null ? -1 : editModel.nt006;
            %>
            <select name="nt006">
                <option value="1" <%=status == 1 ? "selected='selected'" : "" %>>显示</option>
                <option value="0" <%=status == 0 ? "selected='selected'" : "" %>>隐藏</option>
            </select>
        </td>
    </tr>
    <tr>
        <td class="title">内容</td>
        <td>
            <script id="nt003" name="nt003" type="text/plain">
                <%=editModel != null ? editModel.nt003 : string.Empty %>
            </script>
        </td>
    </tr>
    <tr>
        <td class="title"></td>
        <td><input type="submit" class="btn-normal" value="保存" /></td>
    </tr>
</table>
</form>
<script type="text/javascript">
    var ue = UE.getEditor('nt003');
</script>
<%} %>
</asp:Content>
