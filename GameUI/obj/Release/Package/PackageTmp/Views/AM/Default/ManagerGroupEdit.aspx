<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var managerList = (List<DBModel.wgs004>)ViewData["ManagerList"];
        var editModel = (DBModel.wgs015)ViewData["EditModel"];
    %>
    <div class="cjlsoft-body-header">
        <h1>编辑管理组</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
    </div>
    <div class="blank-line"></div>
    <form action="/AM/ManagerGroup" method="post" id="form_mg">
        <%:Html.AntiForgeryToken()%>
        <input type="hidden" name="pg001" value="<%:editModel.pg001%>" />
        <input type="hidden" name="method" value="<%:ViewData["MethodType"] %>" />
        <input type="hidden" id="set_data" name="pg005" class="input-text w300px" value="<%:editModel.pg005%>" />
        <table class="table-pro" width="100%">
            <tbody>
                <tr>
                    <td class="title">名称</td>
                    <td><input type="text" name="pg003" class="input-text w300px" value="<%:editModel.pg003.Trim()%>" /></td>
                </tr>
                <tr>
                    <td class="title">显示名称</td>
                    <td><input type="text" name="pg004" class="input-text w300px" value="<%:editModel.pg004.Trim()%>" /></td>
                </tr>
                <tr>
                    <td class="title">权限</td>
                    <td>
                        <ul id="pg" class="easyui-tree" data-options="lines:true,checkbox:true">
                            <%foreach (var root in managerList)
                              { %>
                            <%
                                  if (root.sm002 != 0)
                                      continue;
                            %>
                            <li title="<%:root.sm004 %>" id="<%:root.sm001 %>">
                                <span><%:root.sm004 %></span>
                                <ul>
                                    <%foreach (var sub in managerList)
                                      {
                                          if (sub.sm002 != root.sm001)
                                              continue;
                                    %>
                                    <li title="<%:sub.sm004 %>" id="<%:sub.sm001 %>"><span><%:sub.sm004 %></span>
                                        <ul>
                                            <%foreach (var subItem in managerList)
                                              {
                                                  if (subItem.sm002 != sub.sm001)
                                                      continue;
                                            %>

                                            <li title="<%:subItem.sm004 %>" id="<%:subItem.sm001 %>"><span><%:subItem.sm004 %></span></li>

                                            <%}/*subItem*/ %>
                                        </ul>
                                    </li>
                                    <%}/*sub*/ %>
                                </ul>
                            </li>
                            <%}/*root*/ %>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <td class="title"></td>
                    <td>
                        <input id="save_pg" type="button" value="保存" class="cjlsoft-post-loading btn-normal" /></td>
                </tr>
            </tbody>
        </table>
    </form>
    <script type="text/javascript">
        $("#save_pg").click(function () {
            var nodes = $('#pg').tree('getChecked');
            var s = '';
            for (var i = 0; i < nodes.length; i++) {
                if (s != '')
                    s += ',';
                s += nodes[i].id;
            }
            $("#set_data").val(s);
            $("#form_mg").submit();
        });
        $(document).ready(function ()
        {
            if( "<%:editModel.pg001%>" != "0")
            {
                var ids = "<%:editModel.pg005%>";
                var sids = ids.split(',');
                for (var i = 0; i < sids.length; i++) {
                    var node = $('#pg').tree('find', sids[i]);
                    $('#pg').tree('check', node.target);
                }
            }
        });
    </script>
</asp:Content>