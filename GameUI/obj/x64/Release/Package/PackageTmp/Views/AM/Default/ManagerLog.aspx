<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs011>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var dicRT = (Dictionary<int, string>)ViewData["ReqType"];
    var type = (int)ViewData["Type"];
    var mType = (int)ViewData["MType"];
%>
    <div class="cjlsoft-body-header">
        <h1>管理日志</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
    </div>
    <div class="blank-line"></div>
    <form action="/AM/ManagerLog" method="get">
        <div class="xtool">
            系统类型
            <select name="mtype">
                <option value="-1">所有</option>
                <option value="0" <%=mType == 0 ? "selected='selected'" : "" %>>系统</option>
                <option value="1" <%=mType == 1 ? "selected='selected'" : "" %>>用户</option>
            </select>
            账号<input type="text" value="<%:ViewData["Account"] %>" class="input-text w100px" name="account" />
            控制器<input type="text" value="<%:ViewData["CTRL"] %>" class="input-text w100px" name="ctrl" />
            动作<input type="text" value="<%:ViewData["ACT"] %>" class="input-text w100px" name="act" />
            关键字<input type="text" value="<%:ViewData["Keyword"] %>" class="input-text w100px" name="keyword" />
            <div class="blank-line"></div>
            时间<input type="text" name="dts" id="DTS" class="input-text w120px" value="<%:ViewData["DTS"] %>" />-<input type="text" name="dte" id="DTE" class="input-text w120px" value="<%:ViewData["DTE"] %>" />
            请求类型
            <select name="type">
                <option value="-1">所有</option>
                <%foreach(var item in dicRT){ %>
                <option value="<%:item.Key %>" <%=type == item.Key ? "selected='selected'" : "" %>><%:item.Value %></option>
                <%} %>
            </select>
            <input type="submit" class="btn-normal" value="查找" />
        </div>
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
                    <td><a href="javascript:void(0);" class="show_data" data="<%:item.log007 %>" title="<%:item.log007 %>">查看FORM</a></td>
                    <td><a href="javascript:void(0);" class="show_data" data="<%:item.log009 %>" title="<%:item.log009 %>">查看QUERY</a></td>
                    <td><a href="javascript:void(0);" class="show_data" data="<%:item.log010 %>" title="<%:item.log010 %>">查看SESSION</a></td>
                    <td><a href="javascript:void(0);" class="show_data" data="<%:item.log008 %>" title="<%:item.log008 %>">查看COOKIE</a></td>
                    <td title="<%:item.log012 %>"><%:NETCommon.StringHelper.GetShortString(item.log012,15,10,"...") %></td>
                    <td title="<%:item.log013 %>"><%:NETCommon.StringHelper.GetShortString(item.log013,15,15, "...") %></td>
                    <td title="<%:item.log014 %>"><%:NETCommon.StringHelper.GetShortString(item.log014,15,15, "...") %></td>
                    <td><%:dicRT[item.log011] %></td>
                </tr>
                <%
                      listIndex++;
                  }/*foreach*/
              }/*if*/ %>
            </tbody>
        </table>

        <%=ViewData["PageList"] %>
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
            jQuery('#DTS').datetimepicker({
                format: 'Y/m/d H:i:s',
                lang: "ch",
                onShow: function (ct) {
                    this.setOptions({
                        maxDate: jQuery('#DTE').val() ? jQuery('#DTE').val() : false
                    })
                },
                timepicker: true
            });
            jQuery('#DTE').datetimepicker({
                format: 'Y/m/d H:i:s',
                lang: "ch",
                onShow: function (ct) {
                    this.setOptions({
                        minDate: jQuery('#DTS').val() ? jQuery('#DTS').val() : false
                    })
                },
                timepicker: true
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
