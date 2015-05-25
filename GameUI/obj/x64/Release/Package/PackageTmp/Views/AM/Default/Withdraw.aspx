<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs019>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var wcList = (List<DBModel.wgs020>)ViewData["WCList"];
    var wTypeList = (List<DBModel.wgs024>)ViewData["WTypeList"];
    var dicStatus = new Dictionary<int, string>() { { 0, "<span style='color:red;'>未处理</span>" }, { 1, "<span style='color:green;'>已处理</span>" }, { 2, "<span style='color:gray;'>已取消</span>" } };
    var moneyType = new Dictionary<int, string>() { { 0, "可用余额提现" }, { 1, "佣金提现" },{2,"分红提现"} };
%>
    <div class="cjlsoft-body-header">
        <h1>提现</h1>
        <div class="left-nav">声音提醒<input id="chk_sound_alert" type="checkbox" value="1" checked="checked" /></div>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
    </div>
    <div class="blank-line"></div>
    <div class="xtool">
        <form action="/AM/Withdraw">
        <span>
            类型
            <select name="wtype">
                <option value="0">所有</option>
                <%if (null != wTypeList)
                  {
                      foreach (var item in wTypeList)
                      { 
                      %>
                <option value="<%:item.uwt001 %>" <%=(int)ViewData["Wtype"] == item.uwt001 ? "selected='selected'" : "" %>><%:item.uwt002 %></option>
                <%}
                  } %>
            </select>
        </span>
        <span>
            编号<input type="text" name="key" class="input-text w80px" value="<%:ViewData["Wkey"] %>" />
        </span>
        <span>
            账号<input type="text" name="waccount" class="input-text w80px" value="<%:ViewData["CHKAccount"] %>" />
        </span>
        <div class="blank-line"></div>
        <span>
            金额
            <select name="amtt">
                <option value="0">所有</option>
                <option value="1" <%=(int)ViewData["AMTT"] == 1 ? "selected='selected'" : "" %>>小于</option>
                <option value="2" <%=(int)ViewData["AMTT"] == 2 ? "selected='selected'" : "" %>>等于</option>
                <option value="3" <%=(int)ViewData["AMTT"] == 3 ? "selected='selected'" : "" %>>大于</option>
            </select>
            <input type="text" name="amttv" class="input-text w50px" value="<%:ViewData["AMTTV"] %>" />
        </span>
        <span>
            手续
            <select name="amttht">
                <option value="0">所有</option>
                <option value="1" <%=(int)ViewData["AMTTHT"] == 1 ? "selected='selected'" : "" %>>小于</option>
                <option value="2" <%=(int)ViewData["AMTTHT"] == 2 ? "selected='selected'" : "" %>>等于</option>
                <option value="3" <%=(int)ViewData["AMTTHT"] == 3 ? "selected='selected'" : "" %>>大于</option>
            </select>
            <input type="text" name="amtthtv" class="input-text w50px" value="<%:ViewData["AMTTHTV"] %>" />
        </span>
        <span>
            状态
            <select name="status">
                <option value="-1" <%:(int)ViewData["Status"] == -1 ? "selected='selected'" : "" %>>所有</option>
                <option value="0" <%:(int)ViewData["Status"] == 0 ? "selected='selected'" : "" %>>未处理</option>
                <option value="1" <%:(int)ViewData["Status"] == 1 ? "selected='selected'" : "" %>>已处理</option>
                <option value="2" <%:(int)ViewData["Status"] == 2 ? "selected='selected'" : "" %>>已取消</option>
            </select>
        </span>
        <span>
            时间<input type="text" name="dts" class="input-text w120px" value="<%:ViewData["DTS"] %>" id="i_dts" />-<input type="text" name="dte" class="input-text w120px" value="<%:ViewData["DTE"] %>" id="i_dte" />
        </span>
        <input type="submit" class=" btn-normal" value="查找" />
        </form>
    </div>
    <div class="blank-line"></div>
    <%
        var wAmSum = 0.0000m;
        var wAmTSum = 0.0000m;
        var wAMFee = 0.0000m;
    %>
    <form action="/AM/ChargeType" method="post">
        <%:Html.AntiForgeryToken()%>
        <input type="hidden" name="method" value="updateList" />
        <table width="100%" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th class="w50px">编号</th>
                    <th>用户</th>
                    <th>提现方式</th>
                    <th>提现类型</th>
                    <th>金额类型</th>
                    <th>手续费</th>
                    <th>申请时间</th>
                    <th>处理时间</th>
                    <th>状态</th>
                    <th>处理人</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <%if (null != wcList)
                  {
                      int listIndex = 0;
                      foreach (var item in wcList)
                      {
                          wAmSum += item.uw002;
                          wAMFee += item.uw016;
                          %>
                <tr>
                    <td><%:item.uw001 %></td>
                    <td title="<%:item.u003 == null ? string.Empty : item.u003.Trim() %>"><%:item.u002.Trim() %></td>
                    <td><%:item.uw009 %></td>
                    <td>
                        <%=moneyType[item.uw018] %>
                    </td>
                    <td class="fc-red"><%:item.uw002.ToString(ViewContext.ViewData["SysMoneyFormat"].ToString()) %></td>
                    
                    <td><%:item.uw016.ToString(ViewContext.ViewData["SysMoneyFormat"].ToString()) %></td>
                    <td><%:item.uw004 %></td>
                    <td><%:item.uw005 %></td>
                    <td><%=dicStatus[item.uw006] %></td>
                    <td><%:item.mu002 != null ? item.mu002.Trim() : "" %></td>
                    <td class="link-tools"><a href="javascript:void(0);" class="link-handle" data="<%:item.uw001 %>">处理</a></td>
                </tr>
                <%
                      listIndex++;
                  }/*foreach*/
              }/*if*/ %>
            </tbody>
            <tfoot>
                <tr>
                    <td>合计</td>
                    <td></td>
                    <td></td>
                    <td class="fc-red"><%=wAmSum %></td>
                    <td></td>
                    <td><%=wAMFee %></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
            </tfoot>
        </table>
        <%=ViewData["PageList"] %>
    </form>
    <div id="dlg_withdraw_handle" class="ui_block_dlg dom-hide">
        <form id="form_confirm" action="/AM/Withdraw?method=confirmWithdraw" method="post">
            <%:Html.AntiForgeryToken()%>
        <input type="hidden" id="auth_key" name="auth_key" value="" />
        <table class="table-pro" width="100%">
            <tbody>
                <tr>
                    <td class="title">提现方式</td>
                    <td class="s_wt"></td>
                </tr>
                <tr>
                    <td class="title">提现姓名</td>
                    <td class="s_wn"></td>
                </tr>
                <tr>
                    <td class="title">提现账号</td>
                    <td class="s_wacct"></td>
                </tr>
                <tr>
                    <td class="title">提现开户行</td>
                    <td class="s_wbl"></td>
                </tr>
                <tr>
                    <td class="title">提现金额</td>
                    <td class="s_amount "></td>
                </tr>
                <tr class="dom-hide">
                    <td class="title">实际提现金额</td>
                    <td class="s_true_amount "><input type="text" id="wc_true_amount" name="wc_true_amount" class="input-text w200px fc-red" value="0" /></td>
                </tr>
                <tr>
                    <td class="title">手续费</td>
                    <td class="s_fee"><input type="text" id="fee" name="fee" class="input-text w200px fc-blue" value="0" /></td>
                </tr>
                <tr>
                    <td class="title">状态</td>
                    <td class="s_status"></td>
                </tr>
                <tr>
                    <td class="title fc-red">取消</td>
                    <td><input type="checkbox" id="item_cancel" name="item_cancel" value="1" /></td>
                </tr>
                <tr>
                    <td class="title">备注</td>
                    <td class="s_commect">
                        <textarea cols="5" rows="3" id="txt_comment" name="comment" class="input-text w200px"></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="title">操作人</td>
                    <td class="s_op"></td>
                </tr>
                <tr>
                    <td class="title"></td>
                    <td class=""><input type="button" id="btn_confirm_handle" value="确认" /></td>
                </tr>
                </tbody>
            </table>
            </form>
    </div>
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $("#btn_confirm_handle").click(function ()
            {
                var form_data = $("#form_confirm").serialize();
                $.ajax({
                    async: false, type: "POST", timeout: _global_ajax_timeout, url: "/AM/Withdraw?method=confirmWithdraw", data: form_data, dataType: "json",
                    success: function (a, b)
                    {
                        _check_auth(a.Code);
                        if (0 == a.Code)
                        {
                            alert(a.Message);
                        }
                        else if (1 == a.Code)
                        {
                            alert('处理成功');
                            location.href = location.href;
                        }
                    },
                    complete: function (a, b)
                    {
                    }
                });
            });

           // $('#dlg_withdraw_handle').dialog(
           //{
           //    title: '提现处理',
           //    width: 500,
           //    height: 450,
           //    closed: true,
           //    cache: false,
           //    modal: true
           //});

            var status = new Array();
            status[0] = '未处理';
            status[1] = '已处理';
            status[2] = '已取消';

            $(".link-handle").click(function ()
            {
                var data = $(this).attr("data");
                $.ajax({
                    async: false, type: "POST", timeout: _global_ajax_timeout, url: "/AM/Withdraw?method=handle", data: { key: data }, dataType: "json",
                    success: function (a, b)
                    {
                        _check_auth(a.Code);
                        if (0 == a.Code)
                        {
                            alert(a.Message);
                            return;
                        }
                        $("#auth_key").val(a.Data.AuthKey);
                        //$('#dlg_withdraw_handle').dialog("open");
                        _global_ui("dlg_withdraw_handle", 0, 25, 10, 2, "提现处理");
                        $(".s_wt").html(a.Data.BN + "-" + a.Data.WDT);
                        $(".s_wn").html(a.Data.BUN);
                        $(".s_wacct").html(a.Data.BA);
                        $(".s_wbl").html(a.Data.BOL);
                        $(".s_amount").html(a.Data.WAM);
                        $(".s_status").html(status[a.Data.Status]);
                        $(".s_op").html(a.Data.MGName);
                        $("#txt_comment").html(a.Data.Comment);
                    },
                    complete: function (a, b)
                    {
                    }
                });
            });

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

            if ("notset" == $.cookie("chk_sound_alert")) {
                $("#chk_sound_alert").prop("checked", false);
            }
            $("#chk_sound_alert").change(function () {
                $.cookie("chk_sound_alert", $(this).prop("checked") == true ? "set" : "notset", { expires: 128 });
            });

            window.setInterval(function () {
                if (false == $("#chk_sound_alert").prop("checked")) {
                    return;
                }
                $.ajax({
                    async: false, cache: true, timeout: 1000 * 5, type: "POST", url: "/AM/CheckWCash", data: { status: 0 }, dataType: "json",
                    success: function (a, b) {
                        _check_auth(a.Code);
                        if (0 < a.Data)
                        {
                            $.ionSound.play("door_bell");
                        }
                    },
                    complete: function (a, b) {
                    }
                });
            }, 1000 * 5);
        });
    </script>
</asp:Content>
