<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs004>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var menuType = (int)ViewData["MenuType"];
%>
    <div class="cjlsoft-body-header">
        <h1>菜单</h1>
        <div class="left-nav">
            <a href="/AM/Menu?menuType=<%:menuType %>" title="根菜单">根菜单</a>
            <a id="a-menu" href="/AM/Menu?method=add&parentID=<%:ViewData["ParentID"] %>&menuType=<%:ViewData["MenuType"] %>" title="添加菜单">添加菜单</a>
        </div>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
    </div>
    <div class="blank-line"></div>
        <div class="cjlsoft-body-header tools">
        <select id="game" class="drop-select-to-url">
            <option value="0" tourl="/AM/Menu?parentID=<%:ViewData["ParentID"] %>&menuType=0" <%if (0 == menuType ){ %> selected="selected"<%} %>>系统</option>
            <option value="1" tourl="/AM/Menu?parentID=<%:ViewData["ParentID"] %>&menuType=1" <%if (1 == menuType ){ %> selected="selected"<%} %>>用户</option>
        </select>
    </div>
    <div class="blank-line"></div>
    <form action="/AM/Menu" method="post">
        <%:Html.AntiForgeryToken()%>
        <input type="hidden" name="method" value="updateList" />
        <table width="100%" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th>编号</th>
                    <th>名称</th>
                    <th>显示名称</th>
                    <th>父级</th>
                    <th>类型</th>
                    <th>控制器</th>
                    <th>动作</th>
                    <th>参数</th>
                    <th>启用</th>
                    <th>权限标识</th>
                    <th>打开类型</th>
                    <th>验证</th>
                    <th>日志</th>
                    <th>排序</th>
                </tr>
            </thead>
            <tbody>
                <%if (null != Model)
                  {
                      int listIndex = 0;
                      foreach (var item in Model)
                      { %>
                <tr>
                    <td>
                        <input type="hidden" name="[<%:listIndex %>].sm001" value="<%:item.sm001 %>" /><%:item.sm001 %></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].sm003" value="<%:item.sm003 %>" class="input-text w100px" /><a href="/AM/Menu?parentID=<%:item.sm001 %>&menuType=<%:item.sm005 %>">下级</a></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].sm004" value="<%:item.sm004 %>" class="input-text w100px" /></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].sm002" value="<%:item.sm002 %>" class="input-text w30px" /></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].sm005" value="<%:item.sm005 %>" class="input-text w30px" /></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].sm006" value="<%:item.sm006 %>" class="input-text w100px" /></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].sm007" value="<%:item.sm007 %>" class="input-text w150px" /></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].sm008" value="<%:item.sm008 %>" class="input-text w150px" /></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].sm009" value="<%:item.sm009 %>" class="input-text w30px" /></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].sm010" value="<%:item.sm010 %>" class="input-text w30px" /></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].sm011" value="<%:item.sm011 %>" class="input-text w30px" /></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].sm013" value="<%:item.sm013 %>" class="input-text w30px" /></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].sm014" value="<%:item.sm014 %>" class="input-text w30px" /></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].sm012" value="<%:item.sm012 %>" class="input-text w50px" /></td>
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
