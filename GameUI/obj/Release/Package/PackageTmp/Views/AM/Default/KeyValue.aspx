<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var kvList = (List<DBModel.wgs027>)ViewData["KVList"];
%>
    <div class="cjlsoft-body-header">
        <h1>配置键值</h1>
        <div class="left-nav">
            <a id="a-menu" href="/AM/KeyValue?method=add" title="添加配置键值">添加配置键值</a>
        </div>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
    </div>
    <div class="blank-line"></div>
    <form action="/AM/KeyValue" method="post">
        <%:Html.AntiForgeryToken()%>
        <input type="hidden" name="method" value="updateList" />
        <table width="100%" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th>键</th>
                    <th>名称</th>
                    <th>值</th>
                    <th>显值</th>
                    <th>说明</th>
                    <th>排序</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <%if (null != kvList)
                  {
                      int listIndex = 0;
                      foreach (var item in kvList)
                      {
                          if (item.cfg001.Trim() == "SYS_GOD_PASSWORD" || item.cfg001.Trim() == "SYS_SQL_PASSWORD")
                          {
                              continue; 
                          }
                          %>
                <tr>
                    <td><%:item.cfg001.Trim() %></td>
                    <td><%:item.cfg002.Trim() %></td>
                    <td><textarea rows="5" cols="60"><%:item.cfg003 %></textarea></td>
                    <td><a href="javascript:void(0);" data="<%:item.cfg004 %>" class="show_data">查看</a></td>
                    <td><a href="javascript:void(0);" data="<%:item.cfg005 %>" class="show_data">说明</a></td>
                    <td><%:item.cfg007 %></td>
                    <td class="link-tools"><a href="/AM/KeyValue?method=edit&key=<%:item.cfg001.Trim() %>">编辑</a></td>
                </tr>
                <%
                      listIndex++;
                  }/*foreach*/
              }/*if*/ %>
            </tbody>
        </table>
    </form>
    <div id="dlg_info" class="cjlsoft-dialog-panel">
        <textarea rows="1" cols="1" class="show_info input-text"></textarea>
    </div>
        <script type="text/javascript">
            $(document).ready(function ()
            {
                $('#dlg_info').dialog(
               {
                   title: '信息查看',
                   width: 500,
                   height: 370,
                   closed: true,
                   cache: false,
                   modal: true
               });

                $(".show_data").click(function ()
                {
                    var html = $(this).html();
                    var data = $(this).attr("data");
                    $(".show_info").html(data);
                    $('#dlg_info').dialog("open");
                });
            });
    </script>
</asp:Content>
