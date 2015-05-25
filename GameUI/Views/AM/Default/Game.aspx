<%@ Import Namespace="DBModel" %><%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs001>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="cjlsoft-body-header">
        <h1>系统游戏</h1>
        <div class="left-nav">
            <a id="a-menu" href="#" title="添加游戏">添加游戏</a>
        </div>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
</div>
    <div class="blank-line"></div>
    <form action="/AM/Game" method="post">
        <%:Html.AntiForgeryToken()%>
        <input type="hidden" name="method" value="updateList" />
    <table width="100%" class="table-pro table-color-row">
        <thead>
            <tr class="table-pro-head">
                <th>编号</th>
                <th>名称</th>
                <th>显示名称</th>
                <th class="dom-hide">图片</th>
                <th>官网</th>
                <th>模块DLL</th>
                <th>参数</th>
                <th class="dom-hide">规则</th>
                <th>是否可用</th>
                <th>排序</th>
            </tr>
        </thead>
        <tbody>
            <%if( null != Model)
              {
                  int listIndex = 0;
                  foreach(var item in Model)
                  { %>
            <tr>
                <td><input type="hidden" name="[<%:listIndex %>].g001" value="<%:item.g001 %>" /><%:item.g001 %></td>
                <td><input type="text" name="[<%:listIndex %>].g002" value="<%:item.g002 %>" class="input-text w100px" /></td>
                <td><input type="text" name="[<%:listIndex %>].g003" value="<%:item.g003 %>" class="input-text w100px" /></td>
                <td class="dom-hide"><input type="text" name="[<%:listIndex %>].g004" value="<%:item.g004 %>" class="input-text w50px" /></td>
                <td><input type="text" name="[<%:listIndex %>].g005" value="<%:item.g005 %>" class="input-text w100px" /></td>
                <td><input type="text" name="[<%:listIndex %>].g006" value="<%:item.g006 %>" class="input-text w100px" /></td>
                <td><input type="text" name="[<%:listIndex %>].g007" value="<%:item.g007 %>" class="input-text w100px" /></td>
                <td class="dom-hide"><input type="text" name="[<%:listIndex %>].g008" value="<%:item.g008 %>" class="input-text w100px" /></td>
                <td><input type="text" name="[<%:listIndex %>].g010" value="<%:item.g010 %>" class="input-text w50px" /></td>
                <td><input type="text" name="[<%:listIndex %>].g009" value="<%:item.g009 %>" class="input-text w50px" /></td>
            </tr>
            <%
                      listIndex++;
                  }/*foreach*/
            }/*if*/ %>
        </tbody>
    </table>

    <div class="blank-line"></div>

    <div id="cjlsoft-bottom-function">
        <input type="submit" value="保存" class="cjlsoft-post-loading btn-normal" />
    </div>
    </form>
</asp:Content>
