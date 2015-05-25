<%@ Import Namespace="DBModel" %><%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs005>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var gameList = (List<DBModel.wgs001>)ViewData["GameList"];
    var gameID = (int)ViewData["GameID"];
    var gameResultList = (List<DBModel.wgs038>)ViewData["GameResultList"];
%>
<div class="cjlsoft-body-header">
        <h1>游戏结果</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
</div>
    <div class="blank-line"></div>
    <div class="cjlsoft-body-header tools">
        <select id="game" class="drop-select-to-url">
            <%foreach( var g in gameList){ %>
            <option value="<%:g.g001 %>" title="<%:g.g003 %>" tourl="<%:Url.Action("GameResult", new { gameID=g.g001 })%>" <%=g.g001 == gameID ? "selected='selected'":"" %>><%:g.g003 %></option>
            <%} %>
        </select>
    </div>
    <div class="blank-line"></div>
<div class="xtool">
    <input type="button" value="全选" class="btn-normal" onclick="table_row_selecct();" /><input type="button" value="返选" class="btn-normal" onclick="table_row_unselect();" /><input type="button" value="取消选中" class="btn-normal" onclick="    table_row_clear_select();" /><input type="button" value="删除" class="btn-normal btn_cancel f_delete" />
</div>
    <form action="/AM/Game" method="post">
        <%:Html.AntiForgeryToken()%>
        <%
                //0初始状态，1已封盘，2未有结果，3已有结果
            Dictionary<int, string> gsStatus = new Dictionary<int, string>();
            gsStatus.Add(0, "<span class='fc-green'>初始状态</span>");
            gsStatus.Add(1, "<span class='fc-gray'>已封盘</span>");
            gsStatus.Add(2, "<span class='fc-red'>未有结果</span>");
            gsStatus.Add(3, "<span class='fc-blue'>已有结果</span>");

            Dictionary<int, string> calcStatus = new Dictionary<int, string>();
            calcStatus.Add(0,"未结算");
            calcStatus.Add(1, "<span class='fc-green'>已结算</span>");
        %>
        <input type="hidden" name="method" value="updateList" />
    <table width="100%" class="table-pro table-color-row">
        <thead>
            <tr class="table-pro-head">
                <th class="w80px">编号</th>
                <th class="w100px">期号</th>
                <th class="w100px">结果</th>
                <th>期数状态</th>
                <th>结算状态</th>
                <th>取得时间</th>
                <th>实开奖时间</th>
                <th>开盘时间</th>
                <th>封盘时间</th>
                <th>开奖时间</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            <%if( null != gameResultList)
              {
                  int listIndex = 0;
                  foreach (var item in gameResultList)
                  { %>
            <tr key="<%:item.gsr001 %>">
                <td><input type="hidden" name="[<%:listIndex %>].gs001" value="<%:item.gs001 %>" /><%:item.gs001 %></td>
                <td><input type="text" name="[<%:listIndex %>].gs002" value="<%:item.gs002.Trim() %>" class="input-text w80px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gs007" value="<%:item.gs007 != null ? item.gs007.Trim() : "" %>" class="input-text w100px" /></td>
                <td><%=gsStatus[item.gsr004] %></td>
                <td><%=calcStatus[item.gsr005] %></td>
                <td><input type="text" name="[<%:listIndex %>].gsr002" value="<%:item.gsr002 %>" class="input-text"  /></td>
                <td><input type="text" name="[<%:listIndex %>].gsr003" value="<%:item.gsr003 %>" class="input-text"  /></td>
                <td><input type="text" name="[<%:listIndex %>].gs003" value="<%:item.gs003 %>" class="input-text w120px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gs004" value="<%:item.gs004 %>" class="input-text w120px" /></td>
                <td><input type="text" name="[<%:listIndex %>].gs005" value="<%:item.gs005 %>" class="input-text w120px" /></td>
                <td class="link-tools"><a href="javascript:;" name="gs_edit" data="<%:item.gsr001 %>">修改</a><a href="javascript:;" name="gs_delete" data="<%:item.gsr001 %>">删除</a></td>
            </tr>
            <%
                      listIndex++;
                  }/*foreach*/
            }/*if*/ %>
        </tbody>
    </table>

    <div class="blank-line"></div>

        <%=ViewData["PageList"] %>

    </form>

    <div id="edit_gs" class="ui_block_dlg">
        <form action="#" method="post" id="form_gs">
            <input type="hidden" name="gsr001" value="" />
            <input type="hidden" name="gs001" value="" />
            <table class="table-pro g_nco w100ps">
                <tr>
                    <td class="title">期号</td>
                    <td><input name="gs002" type="text" value="" /></td>
                </tr>
                <tr>
                    <td class="title">结果</td>
                    <td><input name="gs007" type="text" value="" /></td>
                </tr>
                <tr>
                    <td class="title">开盘时间</td>
                    <td><input name="gs003" type="text" value="" /></td>
                </tr>
                <tr>
                    <td class="title">封盘时间</td>
                    <td><input name="gs004" type="text" value="" /></td>
                </tr>
                <tr>
                    <td class="title">开奖时间</td>
                    <td><input name="gs005" type="text" value="" /></td>
                </tr>
                <tr>
                    <td class="title">创建时间</td>
                    <td><input name="gs006" type="text" value="" /></td>
                </tr>
                <tr>
                    <td class="title">获取时间</td>
                    <td><input name="gsr002" type="text" value="" /></td>
                </tr>
                <tr>
                    <td class="title">实开奖时间</td>
                    <td><input name="gsr003" type="text" value="" /></td>
                </tr>
                <tr>
                    <td class="title">期数状态</td>
                    <td>
                        <select name="gsr004" id="gsr004">
                            <option value="0">初始状态</option>
                            <option value="1">已封盘</option>
                            <option value="2">未有结果</option>
                            <option value="3">已有结果</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td class="title">结算状态</td>
                    <td>
                        <select name="gsr005" id="gsr005">
                            <option value="0">未结算</option>
                            <option value="1">已结算</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td class="title"></td>
                    <td><input type="button" value="保存" class="btn-normal btn_save_gs" /><input type="button" value="关闭" class="btn-normal close_bui" /></td>
                </tr>
            </table>
        </form>
    </div>

    <script type="text/javascript">
        $(".btn_cancel").click(function ()
        {
            if (true != confirm("是否删除选中的记录？"))
            {
                return;
            }
            var ids = get_table_row_keys();
            delete_session(ids);
        });

        $(".btn_save_gs").click(function ()
        {
            var form_data = $("#form_gs").serialize();
            $.ajax({
                timeout: _global_ajax_timeout, cache: false, type: "POST", data: form_data, url: "/AM/GameResult?method=edit", success: function (a) {
                    _check_auth(a);
                    var _robj = eval(a);
                    if (0 == _robj.Code) {
                        alert(_robj.Message);
                    }
                    else if (1 == _robj.Code) {
                        refresh_current_page();
                    }
                }
            });
        });

        $("a[name='gs_edit']").click(function ()
        {
            var key = $(this).attr("data");
            _global_ui("edit_gs", 480, 30, 10, 0);
            $.ajax({
                timeout: _global_ajax_timeout, cache: false, type: "POST", data: { id: key,type:"get" }, url: "/AM/GameResult?method=edit", success: function (a) {
                    _check_auth(a);
                    var _robj = eval(a);
                    if (0 == _robj.Code)
                    {
                        alert(_robj.Message);
                    }
                    else if (1 == _robj.Code)
                    {
                        $("input[name='gsr003'],input[name='gsr002']").val("");
                        $("input[name='gs002']").val(_robj.Data.gs002);
                        $("input[name='gs003']").val(_json_date(_robj.Data.gs003));
                        $("input[name='gs004']").val(_json_date(_robj.Data.gs004));
                        $("input[name='gs005']").val(_json_date(_robj.Data.gs005));
                        $("input[name='gs006']").val(_json_date(_robj.Data.gs006));
                        $("input[name='gs007']").val(_robj.Data.gs007);
                        $("input[name='gs001']").val(_robj.Data.gs001);
                        $("input[name='gsr001']").val(_robj.Data.gsr001);
                        $("#gsr004 option[value='" + _robj.Data.gsr004 + "']").prop("selected", true);
                        $("#gsr005 option[value='" + _robj.Data.gsr005 + "']").prop("selected", true);
                    }
                }
            });
        });

        $("a[name='gs_delete']").click(function ()
        {
            if (true != confirm("是否删除选中的记录？")) {
                return;
            }
            delete_session($(this).attr("data"));
        });

        function delete_session(id)
        {
            $.ajax({
                timeout: _global_ajax_timeout, cache: false, type: "POST", data: { id: id }, url: "/AM/GameResult?method=deleteList", success: function (a) {
                    _check_auth(a);
                    var _robj = eval(a);
                    if (0 == _robj.Code) {
                        alert(_robj.Message);
                    }
                    else if (1 == _robj.Code) {
                        refresh_current_page();
                    }
                }
            });
        }

        jQuery('input[name="gsr002"]').datetimepicker({
            format: 'Y-m-d H:i:s',
            lang: 'ch',
            timepicker: true
        });
        jQuery('input[name="gsr003"]').datetimepicker({
            format: 'Y-m-d H:i:s',
            lang: 'ch',
            timepicker: true
        });
    </script>
</asp:Content>
