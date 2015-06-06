<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %><asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="/Scripts/ueditor/ueditor.config.js"></script>
    <script type="text/javascript" src="/Scripts/ueditor/ueditor.all.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var methodType = (string)ViewData["MethodType"];
    var sysCntClsList = (List<DBModel.SysContentClass>)ViewData["SysCntClsList"];
    var sysCntList = (List<DBModel.wgs041>)ViewData["SysCntList"];
    var classID = (int)ViewData["ClassID"];
%>
<div class="cjlsoft-body-header">
        <h1>系统内容</h1>
        <div class="left-nav">
            <a href="/AM/SysContent?method=edit" title="添加">添加</a>
            <a href="/AM/SysContent" title="列表">列表</a>
        </div>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
</div>
<div class="blank-line"></div>
<%if( string.Empty == methodType)
  {
      %>
    <div class="xtool">
        <select name="nc007" class="drop-select-to-url">
            <%if (null != sysCntClsList)
              {
                  foreach( var item in sysCntClsList)
                  { 
                  %>
            <option value="<%:item.ID %>" tourl="<%=Url.Action("SysContent","AM", new {classID=item.ID})%>" <%=classID == item.ID ? " selected='selected'" : "" %>><%:item.Name %></option>
            <%}
              } %>
        </select>
    </div>
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
        <%
            if(null != sysCntList)
            { 
                foreach(var item in sysCntList)
                { 
        %>
        <tr>
            <td><%:item.nc001 %></td>
            <td><%:item.nc002 %></td>
            <td><%:item.nc004 %></td>
            <td><%:item.nc006 == 0 ? "隐藏" : "显示" %></td>
            <td><%:item.nc005 %></td>
            <td class="link-tools"><a href="/AM/SysContent?method=edit&key=<%:item.nc001 %>">编辑</a><a href="/AM/SysContent?method=delete&key=<%:item.nc001 %>">删除</a></td>
        </tr>
        <%
        }/*foreach*/
        }/*if*/ %>
    </tbody>
</table>
<%} %>
<%else if( "edit"== methodType)
  {
      var editModel = (DBModel.wgs041)ViewData["EditModel"];
       %>
    <form action="/AM/SysContent" method="post">
    <input name="nc001" type="hidden" value="<%:editModel == null ? 0 : editModel.nc001 %>" />
<table class="table-pro w100ps">
    <tr>
        <td class="title">分类</td>
        <td>
        <select name="nc007">
            <%if (null != sysCntClsList)
              {
                  foreach( var item in sysCntClsList)
                  { 
                  %>
            <option value="<%:item.ID %>" <%=editModel != null ? editModel.nc007 == item.ID ? " selected='selected'" : "" : "" %>><%:item.Name %></option>
            <%}
              } %>
        </select>
        </td>
    </tr>
    <tr>
        <td class="title">标题</td>
        <td><input name="nc002" type="text" class="w200px input-text" value="<%:editModel == null ? string.Empty : editModel.nc002.Trim() %>" /></td>
    </tr>
    <tr>
        <td class="title">发布时间</td>
        <td><input name="nc004" type="text" class="w200px input-text" value="<%:editModel == null ? DateTime.Now.ToString() : editModel.nc004.ToString() %>" /></td>
    </tr>
    <tr>
        <td class="title">排序</td>
        <td><input name="nc005" type="text" class="w200px input-text" value="<%:editModel == null ? 1 : editModel.nc005 %>" /></td>
    </tr>
    <tr>
        <td class="title">状态</td>
        <td>
            <%
      var status = editModel == null ? -1 : editModel.nc006;
            %>
            <select name="nc006">
                <option value="1" <%=status == 1 ? "selected='selected'" : "" %>>显示</option>
                <option value="0" <%=status == 0 ? "selected='selected'" : "" %>>隐藏</option>
            </select>
        </td>
    </tr>
    <tr>
        <td class="title">内容</td>
        <td>
            <script id="nc003" name="nc003" type="text/plain">
                <%=editModel != null ? editModel.nc003 : string.Empty %>
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
    var ue = UE.getEditor('nc003');
</script>
<%} %>
</asp:Content>
