<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs011>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%

%>
    <div class="cjlsoft-body-header">
        <h1>管理日志</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
    </div>
    <div class="blank-line"></div>
    <form action="/AM/Menu" method="post">
        <%:Html.AntiForgeryToken()%>
        <input type="hidden" name="method" value="updateList" />
        <table width="100%" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th class="dom-hide">控制器</th>
                    <th class="dom-hide">动作</th>
                    <th>时间</th>
                    <th>网络地址</th>
                    <th>网络端口</th>
                    <th>FORM数据</th>
                    <th>QUERY数据</th>
                    <th>SESSION数据</th>
                    <th>COOKIE数据</th>
                    <th>来自页面</th>
                    <th>URL</th>
                    <th>域名</th>
                    <th>请求方式</th>
                </tr>
            </thead>
            <tbody>
                <%if (null != Model)
                  {
                      int listIndex = 0;
                      foreach (var item in Model)
                      { %>
                <tr key="<%:item.log001 %>">
                    <td class="dom-hide"><%:item.log002 %></td>
                    <td class="dom-hide"><%:item.log003 %></td>
                    <td><%:item.log004 %></td>
                    <td><%:item.log005 %></td>
                    <td><%:item.log006 %></td>
                    <td><a href="javascript:void(0);" class="show_data" data="<%:item.log007 %>">查看FORM</a></td>
                    <td><a href="javascript:void(0);" class="show_data" data="<%:item.log009 %>">查看QUERY</a></td>
                    <td><a href="javascript:void(0);" class="show_data" data="<%:item.log010 %>">查看SESSION</a></td>
                    <td><a href="javascript:void(0);" class="show_data" data="<%:item.log008 %>">查看COOKIE</a></td>
                    <td><%:item.log012 %></td>
                    <td><%:item.log013 %></td>
                    <td><%:item.log014 %></td>
                    <td><%:item.log011 %></td>
                </tr>
                <%
                      listIndex++;
                  }/*foreach*/
              }/*if*/ %>
            </tbody>
        </table>

        <%=ViewData["PageList"] %>

        <div id="cjlsoft-bottom-function">
            <input type="submit" value="保存" class="cjlsoft-post-loading btn-normal" />
        </div>
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
