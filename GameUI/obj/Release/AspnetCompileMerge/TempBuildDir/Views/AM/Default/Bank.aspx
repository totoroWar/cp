<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs010>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cjlsoft-body-header">
        <h1>银行</h1>
        <div class="left-nav">
            <a id="a-menu" href="/AM/Bank?method=add" title="添加银行">添加银行</a>
        </div>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
    </div>
    <div class="blank-line"></div>
    <form action="/AM/Bank" method="post">
        <%:Html.AntiForgeryToken()%>
        <input type="hidden" name="method" value="updateList" />
        <table width="100%" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th>编号</th>
                    <th>名称</th>
                    <th>显示名称</th>
                    <th>标识</th>
                    <th>官网</th>
                    <th>图标</th>
                    <th>可用</th>
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
                        <input type="hidden" name="[<%:listIndex %>].gp001" value="<%:item.sb001 %>" /><input type="hidden" name="[<%:listIndex %>].sb001" value="<%:item.sb001 %>" /><%:item.sb001 %></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].sb003" value="<%:item.sb003 %>" class="input-text w100px" /></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].sb004" value="<%:item.sb004 %>" class="input-text w100px" /></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].sb002" value="<%:item.sb002 %>" class="input-text w30px" /></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].sb005" value="<%:item.sb005 %>" class="input-text w250px" /></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].sb006" value="<%:item.sb006 %>" class="input-text w250px" /></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].sb008" value="<%:item.sb008 %>" class="input-text w50px" /></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].sb009" value="<%:item.sb009 %>" class="input-text w50px" /></td>
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
