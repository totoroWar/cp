<%@ Import Namespace="DBModel" %><%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs003>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    
%>
<div class="cjlsoft-body-header">
        <h1>游戏玩法组</h1>
        <div class="left-nav">
            <a id="a-menu" href="/AM/GameMethodGroup?method=add" title="添加玩法组">添加玩法组</a>
        </div>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
</div>
    <div class="blank-line"></div>
    <form action="/AM/GameMethodGroup" method="post">
        <%:Html.AntiForgeryToken()%>
        <input type="hidden" name="method" value="updateList" />
    <table width="100%" class="table-pro table-color-row">
        <thead>
            <tr class="table-pro-head">
                <th>编号</th>
                <th>名称</th>
                <th>显示名称</th>
                <th>是否开启</th>
                <th>代码字母</th>
                <th>代码数字</th>
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
                <td><input type="hidden" name="[<%:listIndex %>].gmg001" value="<%:item.gmg001 %>" /><%:item.gmg001 %></td>
                <td><input type="text" name="[<%:listIndex %>].gmg002" value="<%:item.gmg002 %>" class="input-text w100px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gmg003" value="<%:item.gmg003 %>" class="input-text w100px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gmg004" value="<%:item.gmg004 %>" class="input-text w50px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gmg005" value="<%:item.gmg005 %>" class="input-text w200px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gmg006" value="<%:item.gmg006 %>" class="input-text w100px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gmg007" value="<%:item.gmg007 %>" class="input-text w50px" /></td>
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
