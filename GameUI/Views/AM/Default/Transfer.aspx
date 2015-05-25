<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs019>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var tfList = (List<DBModel.wgs043>)ViewData["TransferList"];
    %>
    <div class="cjlsoft-body-header">
        <h1>转账</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
    </div>
    <div class="blank-line"></div>
    <div class="xtool">
        <form action="/AM/Transfer" method="get">
            <span>编号<input type="text" name="chrkey" class="input-text w100px" value="<%:ViewData["Key"] %>" />
            </span>
            <span>账号<input type="text" name="chrckey" class="input-text w100px" value="<%:ViewData["Accoount"] %>" />
            </span>
            <input type="submit" class=" btn-normal" value="查找" />
        </form>
    </div>
    <div class="blank-line"></div>
    <form action="/AM/Transfer" method="get">
        <%:Html.AntiForgeryToken()%>
        <table width="100%" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th class="w100px">编号</th>
                    <th>转账账号</th>
                    <th>收账账号</th>
                    <th>金额</th>
                    <th>手续费</th>
                    <th>转账时间</th>
                    <th>处理时间</th>
                    <th>状态</th>
                    <th>处理人</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <%if (null != tfList)
                  {
                      int listIndex = 0;
                      Dictionary<int, string> status = new Dictionary<int, string>() { { 0, "未审核" }, { 1, "已审核" }, { 2, "已取消" } };
                      foreach (var item in tfList)
                      { %>
                <tr>
                    <td><%:item.tf001 %><textarea rows="5" cols="5" class="dom-hide" id="key<%=item.tf001 %>"><%=Newtonsoft.Json.JsonConvert.SerializeObject(item) %></textarea></td>
                    <td><%:item.tf003.Trim() %></td>
                    <td><%:item.tf006.Trim() %></td>
                    <td><%:item.tf008.ToString("N2") %></td>
                    <td><%:item.tf015.ToString("N2") %></td>
                    <td><%:item.tf009 %></td>
                    <td><%:item.tf014 %></td>
                    <td><%:status[item.tf012] %></td>
                    <td><%:item.mu002 %></td>
                    <td class="link-tools"><a href="javascript:;" title="处理" data="<%:item.tf001 %>" class="ic_setok">处理</a></td>
                </tr>
                <%
                          listIndex++;
                      }/*foreach*/
                  }/*if*/ %>
            </tbody>
        </table>
        <%=ViewData["PageList"] %>
    </form>
    <div class="dom-hide ui_block_dlg" id="ic_setok_dlg">
        <form action="#" method="post" id="form_save_transfer">
            <input type="hidden" id="key" name="key" value="" />
            <table class="table-pro table-noborder w100ps">
                <tr>
                    <td class="title">转账账号</td>
                    <td id="su_name"></td>
                </tr>
                <tr>
                    <td class="title">收账账号</td>
                    <td id="tu_name"></td>
                </tr>
                <tr>
                    <td class="title">金额</td>
                    <td id="tf_amt"></td>
                </tr>
                <tr>
                    <td class="title">时间</td>
                    <td id="tf_dt"></td>
                </tr>
                <tr>
                    <td class="title">审核选项</td>
                    <td>
                        <select name="type" id="status_type">
                            <option value="0">未审核</option>
                            <option value="1">已审核</option>
                            <option value="2">已取消</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td class="title">说明</td>
                    <td>
                        <textarea cols="50" rows="5" name="comment"></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="title"></td>
                    <td>
                        <input type="button" class="btn-normal" id="btn_save" value="保存" /></td>
                </tr>
            </table>
        </form>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            jQuery('#i_dts').datetimepicker({
                format: 'Y/m/d H:i:s',
                lang: "ch",
                onShow: function (ct) {
                    this.setOptions({
                        maxDate: jQuery('#i_dte').val() ? jQuery('#i_dte').val() : false
                    })
                },
                timepicker: true
            });
            jQuery('#i_dte').datetimepicker({
                format: 'Y/m/d H:i:s',
                lang: "ch",
                onShow: function (ct) {
                    this.setOptions({
                        minDate: jQuery('#i_dts').val() ? jQuery('#i_dts').val() : false
                    })
                },
                timepicker: true
            });

            $(".ic_setok").click(function () {
                var key = $(this).attr("data");
                var json = $("#key" + key).val();
                eval("json=" + json + ";");
                $("#key").val(key);
                $("#status_type option[value='" + json.tf012 + "']").prop("selected", true);
                $("#su_name").html(json.tf003);
                $("#tu_name").html(json.tf006);
                $("#tf_amt").html(json.tf008);
                $("#tf_dt").html(json.tf009);
                _global_ui("ic_setok_dlg", 480, 30, 10, 0, "处理");

            });
            $("#btn_save").click(function () {
                $.ajax({
                    timeout: _global_ajax_timeout, dataType: "text", cache: false, type: "POST", data: $("#form_save_transfer").serialize(), url: "/AM/Transfer", success: function (a) {
                        _check_auth(a);
                        eval("var _robj =" + a + ";");
                        if (0 == _robj.Code) {
                            alert(_robj.Message);
                        }
                        else if (1 == _robj.Code)
                        {
                            $.ionSound.play("bell_ring");
                            refresh_current_page();
                        }
                    }
                });
            });
        });
    </script>
</asp:Content>
