<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs019>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var dicCTList = (Dictionary<int,DBModel.wgs009>)ViewData["CTList"];
    var dicBList = (Dictionary<int, DBModel.wgs010>)ViewData["BList"];
%>
    <div class="cjlsoft-body-header">
        <h1>充值</h1>
        <div class="left-nav">声音提醒<input id="chk_sound_alert" type="checkbox" value="1" checked="checked" /></div>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
    </div>
    <div class="blank-line"></div>
    <div class="xtool">
        <form action="/AM/Charge" method="get">
        <span>
            类型
            <select name="chrtype">
                <option value="0">所有</option>
                <%foreach (var item in dicBList)
                  { %>
                <option value="<%:item.Value.sb001 %>" <%=item.Value.sb001 == (int)ViewData["CHRType"] ? "selected='selected'" : "" %>><%:item.Value.sb003 %></option>
                <%} %>
            </select>
        </span>
        <span>
            编号<input type="text" name="chrkey" class="input-text w80px" value="<%:ViewData["CHRkey"] %>" />
        </span>
        <span>
            充值码<input type="text" name="chrckey" class="input-text w100px" value="<%:ViewData["CHRCkey"] %>" />
        </span>
        <span>
            账号<input type="text" name="chraccount" class="input-text w80px" value="<%:ViewData["CHKAccount"] %>" />
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
            实际金额
            <select name="amttt">
                <option value="0">所有</option>
                <option value="1" <%=(int)ViewData["AMTTT"] == 1 ? "selected='selected'" : "" %>>小于</option>
                <option value="2" <%=(int)ViewData["AMTTT"] == 2 ? "selected='selected'" : "" %>>等于</option>
                <option value="3" <%=(int)ViewData["AMTTT"] == 3 ? "selected='selected'" : "" %>>大于</option>
            </select>
            <input type="text" name="amtttv" class="input-text w50px" value="<%:ViewData["AMTTTV"] %>" />
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
                <option value="0" <%:(int)ViewData["Status"] == 0 ? "selected='selected'" : "" %>>未完成</option>
                <option value="1" <%:(int)ViewData["Status"] == 1 ? "selected='selected'" : "" %>>已完成</option>
                <option value="2" <%:(int)ViewData["Status"] == 2 ? "selected='selected'" : "" %>>已取消</option>
            </select>
        </span>
        <div class="blank-line"></div>
        <span>
            时间<input type="text" name="dts" class="input-text w120px" value="<%:ViewData["DTS"] %>" id="i_dts" />-<input type="text" name="dte" class="input-text w120px" value="<%:ViewData["DTE"] %>" id="i_dte" />
        </span>
        <input type="submit" class=" btn-normal" value="查找" />
            <div class="blank-line"></div>
             <span>选择<input type="button" id="selectnotprocesses" class=" btn-normal" value="选择未处理" /><input type="button" class=" btn-normal" id="cancelselect" value="取消选择" /></span>  <span>操作<input type="button" id="cancel" class=" btn-normal" value="取消" /></span>
            
           
        </form>
    </div>
    <div class="blank-line"></div>
    <%
        var sumAmount = 0.0000m;
        var sumFee = 0.0000m;
        var sumAmountT = 0.0000m;
    %>
    <form action="/AM/ChargeType" method="post">
        <%:Html.AntiForgeryToken()%>
        <input type="hidden" name="method" value="updateList" />
        <table width="100%" id="maindata" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th class="w50px">编号</th>
                    <th>用户</th>
                    <th>充值方式</th>
                    <th>金额</th>
                    <th>实际到账</th>
                    <th>手续费</th>
                    <th>充值码</th>
                    <th>充值时间</th>
                    <th>处理时间</th>
                    <th>状态</th>
                    <th>操作人</th>
                    <th>处理人</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <%if (null != Model)
                  {
                      int listIndex = 0;
                      foreach (var item in Model)
                      {
                          sumAmountT += item.uc003;
                          sumAmount += item.uc002;
                          sumFee += item.uc013;
                          %>
                <tr>
                    <td><%:item.uc001 %></td>
                    <td title="<%:item.u003 == null ? string.Empty : item.u003.Trim() %>"><%:item.u002 %></td>
                    <td><%:dicBList[item.sb001].sb003 %></td>
                    <td class="fc-red"><%:item.uc002.ToString(ViewContext.ViewData["SysMoneyFormat"].ToString()) %></td>
                    <td class="fc-green"><%:item.uc003.ToString(ViewContext.ViewData["SysMoneyFormat"].ToString()) %></td>
                    <td class="fc-blue"><%:item.uc013.ToString(ViewContext.ViewData["SysMoneyFormat"].ToString()) %></td>
                    <td><%:item.uc005.Trim() %></td>
                    <td><%:item.uc006 %></td>
                    <td><%:item.uc007 %></td>
                    <td>
                        <%switch (item.uc008) 
                          {
                              case 0:
                                  Response.Write("<span style='color:red;'>未处理</span>");
                                  break;
                              case 1:
                                  Response.Write("<span style='color:green;'>已完成</span>");
                                  break;
                              case 2:
                                  Response.Write("<span style='color:black;'>取消</span>");
                                  break;        
                          } %>

                    </td>
                    <td><%:item.mu002 %></td>
                    <td><%:item.mu002x %></td>
                    <td class="link-tools"><a href="javascript:void(0);" class="link-handle" data="<%:item.uc001 %>">处理</a></td>
                </tr>
                <%
                      listIndex++;
                  }/*foreach*/
              }/*if*/ %>
            </tbody>
            <tfoot>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td class="fc-red"><%=sumAmount %></td>
                    <td class="fc-green"><%=sumAmountT %></td>
                    <td class="fc-blue"><%=sumFee %></td>
                    <td></td>
                    <td></td>
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
    <div id="dlg_charge_handle" class="cjlsoft-dialog-panel ui_block_dlg">
        <form id="form_confirm" action="/AM/Charge?method=confirmCharge" method="post">
        <input type="hidden" id="auth_key" name="auth_key" value="" />
        <table class="table-pro" width="100%">
            <tbody>
                <tr>
                    <td class="title">充值方式</td>
                    <td class="s_ct"></td>
                </tr>
                <tr>
                    <td class="title">充值编号</td>
                    <td class="s_cn"></td>
                </tr>
                <tr>
                    <td class="title">账号</td>
                    <td class="s_account"></td>
                </tr>
                <tr>
                    <td class="title">金额</td>
                    <td class="s_amount fc-red"></td>
                </tr>
                <tr>
                    <td class="title">实际到账</td>
                    <td class="s_trueamount "><input type="text" id="uc003" name="uc003" class="input-text w200px fc-green" value="0" /></td>
                </tr>
                <tr>
                    <td class="title">手续费</td>
                    <td class="s_fee"><input type="text" id="uc013" name="uc013" class="input-text w200px fc-blue" value="0" /></td>
                </tr>
                <tr>
                    <td class="title fc-red">取消</td>
                    <td><input type="checkbox" id="item_cancel" name="item_cancel" value="1" /></td>
                </tr>
                <tr>
                    <td class="title">备注</td>
                    <td class="s_commect">
                        <textarea cols="5" rows="3" name="uc012" class="input-text w200px"></textarea>
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
            $("#btn_confirm_handle").click(function ()
            {
                var form_data = $("#form_confirm").serialize();
                $.ajax({
                    async: false, type: "POST", timeout: 5, url: "/AM/Charge?method=confirmCharge", data: form_data, dataType: "json",
                    success: function (a, b)
                    {
                        _check_auth(a.Code);
                        if (0 == a.Code)
                        {
                            alert(a.Message);
                        }
                        else if (1 == a.Code)
                        {
                            $.ionSound.play("bell_ring");
                            alert('处理成功');
                            location.href = location.href;
                        }
                    },
                    complete: function (a, b)
                    {
                    }
                });
            });
            if ("notset" == $.cookie("chk_sound_alert"))
            {
                $("#chk_sound_alert").prop("checked", false);
            }
            $("#chk_sound_alert").change(function ()
            {
                $.cookie("chk_sound_alert", $(this).prop("checked") == true ? "set" : "notset", { expires: 128 });
            });
           //$('#dlg_charge_handle').dialog(
           //{
           //    title: '充值处理',
           //    width: 500,
           //    height: 370,
           //    closed: true,
           //    cache: false,
           //    modal: true
           //});

            $(".link-handle").click(function ()
            {
                var data = $(this).attr("data");
                $.ajax({
                    async: false, type: "POST", timeout: 5, url: "/AM/Charge?method=edit", data:{uc001:data}, dataType: "json",
                    success: function (a, b)
                    {
                        _check_auth(a.Code);
                        if (0 == a.Code)
                        {
                            alert(a.Message);
                            return;
                        }
                        $(".s_account").html(a.Data.u002);
                        $(".s_amount").html(a.Data.uc002);
                        $(".s_op").html(a.Data.mu002);
                        $(".s_cn").html(a.Data.uc005);
                        $(".s_ct").html(a.Data.sb003 +"-"+ a.Data.ct003);
                        $("#auth_key").val(a.Data.AuthKey);
                        $("#uc003").val(a.Data.uc003);
                        if (2 == a.Data.uc008)
                        {
                            $("#item_cancel").prop("checked", true);
                        }
                        //$('#dlg_charge_handle').dialog("open");
                        _global_ui("dlg_charge_handle", 0, 30, 10, 2, "充值处理");
                        
                    },
                    complete: function (a, b)
                    {
                    }
                });
            });

            //$.ionSound.play("bell_ring");
            window.setInterval(function ()
            {
                if (false == $("#chk_sound_alert").prop("checked"))
                {
                    return ;
                }
                $.ajax({
                    async: false, cache: true,timeout:1000 * 5, type: "POST", url: "/AM/CheckCCash",data:{status:0}, dataType: "json",
                    success: function (a, b)
                    {
                        _check_auth(a.Code);
                        if (0 < a.Data)
                        {
                            $.ionSound.play("button_tiny");
                        }
                    },
                    complete: function (a, b)
                    {
                    }
                });
            }, 1000 * 5);
            $("#selectnotprocesses").click(function () {
                $("#maindata tbody tr:not(:first)").attr("class", "");
                $("#maindata tbody tr td span:contains('未处理')").parent().parent().attr("class", "table-row-select")
            });
            $("#cancelselect").click(function () {
                $("#maindata tr:not(:first)").attr("class", "");
            });
            $("#cancel").click(function () {
                var ids ="";
                $("#maindata tbody tr[class='table-row-select'],#maindata tbody tr[class='table-color-row-even table-row-select']").find("td:eq(0)").each(function () {
                    ids += $(this).html() + ",";
                });
                $.ajax({
                    type: "POST",
                    url: "/AM/Charge?method=batchcancel",
                    data: { ids: ids },
                    success: function (msg) {
                        alert(msg.Message);
                        if (msg.Code == 1) {
                            location.reload();
                        }
                    },
                    error: function () {
                        alert("服务器出错");
                    }
                });
            });
        });
    </script>
</asp:Content>
