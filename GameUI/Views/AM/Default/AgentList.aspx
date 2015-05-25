<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
    var userStatus = (int)ViewData["UserStatus"];
    var amtT = (int)ViewData["AmtT"];
    var amtV = (decimal)ViewData["AmtV"];
    var pntT = (int)ViewData["PntT"];
    var pntV = (decimal)ViewData["PntV"];
    var IP = (string)ViewData["IP"];
    var userName = (string)ViewData["UserName"];
    var RDTS = (DateTime?)ViewData["RDTS"];
    var RDTE = (DateTime?)ViewData["RDTE"];
    var LDTS = (DateTime?)ViewData["LDTS"];
    var LDTE = (DateTime?)ViewData["LDTE"];
    var parentID = (int)ViewData["parentID"];
    var userPackList = (List<DBModel.UserPack>)ViewData["UserPackList"];
    var posList = (List<DBModel.SysPositionLevel>)ViewData["PosList"];
    var posDicList = posList.ToDictionary(exp => exp.Level);
    var acctLevelList = (List<DBModel.SysAccountLevel>)ViewData["AcctLevelList"];
    var acctLevelDicList = acctLevelList.ToDictionary(exp => exp.Level);

    decimal lineus001Sum = 0;
    decimal lineus002Sum = 0;
    decimal lineus003Sum = 0;
    decimal lineus004Sum = 0;
    decimal lineus005Sum = 0;
%>
<div class="cjlsoft-body-header">
    <h1>代理列表</h1>
</div>
<div class="blank-line"></div>
<div class="xtool">
    <form action="/AM/AgentList" method="get">
        注册时间
        <input type="text" class="input-text w80px" id="regDTS" name="regDTS" value="<%:ViewData["RDTS"] %>" />-<input type="text" class="input-text w80px" id="regDTE" name="regDTE" value="<%:ViewData["RDTE"] %>" />
        登录时间
        <input type="text" class="input-text w80px" id="loginDTS" name="loginDTS" value="<%:ViewData["LDTS"] %>" />-<input type="text" class="input-text w80px" id="loginDTE" name="loginDTE" value="<%:ViewData["LDTE"] %>" />
        <div class="blank-line"></div>
        账号<input name="userName" type="text" class="input-text w100px" value="<%:userName %>" />
        状态
        <select name="userStatus">
            <option value="-1">所有</option>
            <option value="0" <%=userStatus==0? "selected='selected'" : "" %>>停用</option>
            <option value="1" <%=userStatus==1? "selected='selected'" : "" %>>正常</option>
            <option value="2" <%=userStatus==2? "selected='selected'" : "" %>>暂停</option>
            <option value="3" <%=userStatus==3? "selected='selected'" : "" %>>冻结</option>
        </select>
        金额
        <select name="amountType">
            <option value="0" <%=amtT==0? "selected='selected'" : "" %>>所有</option>
            <option value="1" <%=amtT==1? "selected='selected'" : "" %>>等于</option>
            <option value="2" <%=amtT==2? "selected='selected'" : "" %>>小于</option>
            <option value="3" <%=amtT==3? "selected='selected'" : "" %>>大于</option>
        </select>
        <input type="text" class="input-text w50px" name="amountTypeV" value="<%:amtV %>" />
        积分
        <select name="pointType">
            <option value="0" <%=pntT==0? "selected='selected'" : "" %>>所有</option>
            <option value="1" <%=pntT==1? "selected='selected'" : "" %>>等于</option>
            <option value="2" <%=pntT==2? "selected='selected'" : "" %>>小于</option>
            <option value="3" <%=pntT==3? "selected='selected'" : "" %>>大于</option>
        </select>
        <input type="text" class="input-text w50px" name="pointTypeV" value="<%:pntV %>" />
        <input type="hidden" name="parentID" value="<%:parentID %>" />
        <input type="button" class="btn-normal show_tree" value="代理树" />
        <input type="submit" class="btn-normal ui-post-loading" value="查找" />
        <input type="button" id="reset_form" class="btn-normal" value="重置" />
        <input type="button" class="btn-normal cjlsoft-a-back" value="返回" />
</form>
</div>
    <div id="parent_tree" class="dom-hide" data-options="lines:true,animate:false"></div>
    <div class="blank-line"></div>
    <table width="100%" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th>编号</th>
                    <th>父级</th>
                    <th>账号</th>
                    <th>下级数</th>
                    <th>层级</th>
                    <th>注册日期</th>
                    <th>可用金额</th>
                    <th>冻结金额</th>
                    <th>佣金金额</th>
                    <th>分红金额</th>
                    <th>登录时间/地址</th>
                    <th>开下级</th>
                    <th>积分</th>
                    <th>VIP</th>
                    <th>军衔</th>
                    <th>分红</th>
                    <th>状态</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <%if (null != userPackList)
                  {
                      int listIndex = 0;
                      foreach (var item in userPackList)
                      { %>
                <%
                  lineus001Sum += item.User.wgs014.uf001;
                  lineus002Sum += item.User.wgs014.uf012;
                  lineus003Sum += item.User.wgs014.uf003;
                  lineus004Sum += item.User.wgs014.uf004;
                  lineus005Sum += item.User.wgs014.uf013;
               %>
                <tr>
                    <td><input type="hidden" name="[<%:listIndex %>].sm001" value="<%:item.User.u001 %>" /><%:item.User.u001 %></td>
                    <td><%:item.User.u014 %></td>
                    <td title="<%:item.User.u003!=null ? ""+item.User.u003.Trim()+"" : "" %>"><a href="/AM/AgentList?parentID=<%:item.User.u001 %>"><%:item.User.u002.Trim() %></a></td>
                    <td><%:item.ChildCount %></td>
                    <td>
                        <%=acctLevelDicList[item.User.u018].Name %>
                    </td>
                    <td><%:item.User.u005.ToString("yyyy:MM:dd") %></td>
                    <td><%:item.User.wgs014.uf001.ToString(ViewContext.ViewData["SysMoneyFormat"].ToString()) %></td>
                    <td><%:item.User.wgs014.uf003.ToString(ViewContext.ViewData["SysMoneyFormat"].ToString()) %></td>
                    <td><%:item.User.wgs014.uf012.ToString(ViewContext.ViewData["SysMoneyFormat"].ToString()) %></td>
                    <td><%:item.User.wgs014.uf013.ToString(ViewContext.ViewData["SysMoneyFormat"].ToString()) %></td>
                    <td><%:item.User.u007 %>/<%:item.User.u022 %></td>
                    <td><%:item.User.u020 == 1 ? "允许" : "不允许" %></td>
                    <td><%:item.User.wgs014.uf004.ToString(ViewContext.ViewData["SysMoneyFormat"].ToString()) %></td>
                    <td><%:item.User.u015 %></td>
                    <td><%:posDicList[item.User.u013].Name %></td>
                    <td><%:item.User.u024 * 100 %>%</td>
                    <td>
                        <%
                      var status = string.Empty;
                      switch (item.User.u008)
                      {
                          case 0:
                              status = "<span class='fc-red'>停用</span>";
                              break;
                          case 1:
                              status = "<span class='fc-green'>正常</span>";
                              break;
                          case 2:
                              status = "<span class='fc-blue'>暂停</span>";
                              break;
                          case 3:
                              status = "<span class='fc-gray'>冻洁</span>";
                              break; 
                      }
                    %>
                    <%=status %>

                    </td>
                    <td class="link-tools"><a href="javascript:window.parent.parent.ui_show_tab('<%:item.User.u002.Trim() %>充值','/AM/AgentCharge?key=<%:item.User.u001 %>',true);" title="充值">充值</a><a href="javascript:void(0);" title="修改" name="edit_user" data="<%:item.User.u001 %>">修改</a><a href="javascript:void(0);" name="edit_user_bank" data="<%:item.User.u001 %>" title="银行卡">银行卡</a><a href="javascript:void(0);" name="unbind_bank" data="<%:item.User.u001 %>" title="解绑银行卡">解绑银行卡</a><a href="javascript:void(0);" name="delete_user" data="<%:item.User.u001 %>" title="删除账号">删除账号</a><a href="javascript:void(0);" name="send_point" data="<%:item.User.u001 %>" data2="<%:item.User.u002.Trim() %>" title="送积分">送积分</a><a href="javascript:void(0);" name="send_frozenSum" data="<%:item.User.u001 %>" data2="<%:item.User.u002.Trim() %>" data3="<%:item.User.wgs014.uf001 %>" title="冻结金额">冻结金额</a></td>
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
                <td></td>
                <td></td>
                <td></td>
                <td class="<%:lineus001Sum>0 ? "fc-yellow" : "" %>"><%:lineus001Sum%></td>
                <td class="<%:lineus003Sum>0 ? "fc-yellow" : "" %>"><%:lineus003Sum%></td>
                <td class="<%:lineus002Sum>0 ? "fc-yellow" : "" %>"><%:lineus002Sum%></td>
                <td class="<%:lineus002Sum>0 ? "fc-yellow" : "" %>"><%:lineus005Sum%></td>
                <td></td>
                <td></td>
                <td class="<%:lineus004Sum>0 ? "fc-yellow" : "" %>"><%:lineus004Sum%></td>
                <td></td>
                <td></td>
                <td></td><td></td>
                <td></td>
            </tr>
        </tfoot>
        </table>
    <div class="blank-line"></div>
    <div id="edit_user_dlg">
        <form action="#" method="post" id="form_edit_user">
            <%:Html.AntiForgeryToken() %>
            <input type="hidden" id="edit_key" name="edit_key" value="" />
        <table class="table-pro g_nco tp5" width="100%">
            <tr>
                <td class="title">账号</td>
                <td id="ui_acct"></td>
            </tr>
            <tr>
                <td class="title">昵称</td>
                <td><input name="nickname" id="ui_nkn" type="text" class="input-text w150px" value="" /></td>
            </tr>
            <tr>
                <td class="title">登录密码</td>
                <td><input name="login_pwd" id="login_pwd" type="text" class="input-text w150px" value="" /></td>
            </tr>
            <tr>
                <td class="title">资金密码</td>
                <td><input name="cash_pwd" id="cash_pwd" type="text" class="input-text w150px" value="" /></td>
            </tr>
            <tr>
                <td class="title">状态</td>
                <td>
                    <select id="status" name="status">
                        <option value="0">停用</option>
                        <option value="1">正常</option>
                        <option value="2">暂停</option>
                        <option value="3">冻结</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="title">分红</td>
                <td  >
                    <input type="text" class="input-text w40px" id="red_percent" name="red_percent" readonly="readonly"  />%<font color="red"> （若要修改分红比例，请到"账户->分红设置"中更改）</font>
                </td>
            </tr>
            <tr>
                <td class="title">注册时间</td>
                <td id="ui_rd"></td>
            </tr>
            <tr>
                <td class="title">登录时间</td>
                <td id="ui_ld"></td>
            </tr>
            <tr>
                <td class="title">登录地址</td>
                <td id="ui_ip"></td>
            </tr>
            <tr>
                <td class="title">开下级</td>
                <td>
                    <select id="can_child" name="can_child">
                        <option value="0">不允许</option>
                        <option value="1">允许</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="title">返点</td>
                <td style="padding:5px;">
                    <table class="table-pro" width="100%">
                        <thead>
                            <tr class="table-pro-head">
                                <th>游戏</th>
                                <th>返点</th>
                            </tr>
                        </thead>
                        <tbody id="point_list">
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="title"></td>
                <td style="padding:3px;"><input type="button" value="保存" class="btn-normal btn_update_user" /><input type="button" value="关闭" class="btn-normal close_bui" /></td>
            </tr>
        </table>
        </form>
    </div>
    <div id="edit_bank_dlg" class="ui_block_dlg">
        <form action="#" method="post" id="form_edit_bank">
            <input type="hidden" name="updatekey" value="" />
        <table class="table-pro w400px">
            <tr>
                <td class="title">银行</td>
                <td>
                    <select name="bank_code" id="bank_code">
                        <%if (null != ViewData["WTypeList"])
                          {
                              var wtList = (List<DBModel.wgs024>)ViewData["WTypeList"];
                              foreach (var item in wtList)
                              {
                              %>
                        <option value="<%:item.uwt001 %>"><%:item.uwt002 %></option>
                        <%}
                          } %>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="title">开户人</td>
                <td><input type="text" name="bank_user" class="input-text w200px" /></td>
            </tr>
            <tr>
                <td class="title">卡号</td>
                <td><input type="text" name="bank_no" class="input-text w200px" /></td>
            </tr>
            <tr>
                <td class="title">所在地</td>
                <td><input type="text" name="bank_location" class="input-text w200px" /></td>
            </tr>
            <tr>
                <td class="title">开户行</td>
                <td><input type="text" name="bank_open" class="input-text w200px" /></td>
            </tr>
            <tr>
                <td class="title">绑定时间</td>
                <td id="bank_t"></td>
            </tr>
            <tr>
                <td class="title"></td>
                <td><input type="button" id="btn_update_bank" value="保存" /></td>
            </tr>
        </table>
            </form>
    </div>
    <div id="user_send_point">
        <form action="/AM/AgentList" id="form_send_point">
            <input type="hidden" name="method" value="sendPoint" />
            <input type="hidden" name="key" value="0" />
            <table class="table-pro g_nco tp5" width="100%">
                <tr>
                    <td class="title">账号</td>
                    <td id="send_to_user"></td>
                </tr>
                <tr>
                    <td class="title">积分</td>
                    <td><input type="text" name="point" class="input-text w150px" value="0" /></td>
                </tr>
                <tr>
                    <td class="title">积分</td>
                    <td><input type="button" value="确认" class="btn-normal btn_send_point" /></td>
                </tr>
            </table>
        </form>
    </div>
    <div id="user_send_frozenSum">
        <form action="/AM/AgentList" id="form_send_frozenSum">
            <input type="hidden" name="method" value="sendFrozenSum" />
            <input type="hidden" name="key" value="0" />
            <table class="table-pro g_nco tp5" width="100%">
                <tr>
                    <td class="title">账号</td>
                    <td id="send_to1_user"></td>
                </tr>
                <tr>
                    <td class="title">可用金额</td>
                    <td id="momey"></td>
                </tr>
                <tr>
                    <td class="title">冻结金额</td>
                    <td><input type="text" name="frozenSum" class="input-text w150px" value="0" /></td>
                </tr>
                <tr>
                    <td class="title">备注</td>
                    <td><textarea cols="6" rows="4" name="remarks" class="input-text w200px input-text-hover" cjlsoft-input-text-old-value=""></textarea></td>
                    
                </tr>
                <tr>
                    <td class="title">冻结金额</td>
                    <td><input type="button" value="确认" class="btn-normal btn_send_frozenSum" /></td>
                </tr>
            </table>
        </form>
    </div>
    <%=ViewData["PageList"] %>
<script type="text/javascript">
    $("#edit_user_dlg").dialog({ width: 500, height: 570, title: "修改", closed: true, modal: true, top: "0%", position: { my: "center", at: "top", of: window } });
    $("#user_send_point").dialog({ width: 500, height: 120, title: "赠送积分", closed: true, modal: true, position: { my: "center", at: "center", of: window } });
    $("#user_send_frozenSum").dialog({ width: 500, height: 220, title: "冻结金额", closed: true, modal: true, position: { my: "center", at: "center", of: window } });
    $(document).ready(function () {
        $("#parent_tree").tree({ url: "/AM/MemberAgent?method=getAGUIDs" });
        $('#parent_tree').tree({
            onClick: function (node) {
                //$("#if_agent_list").attr("src", "/AM/AgentList?parentID=" + node.id);
                location.href = "/AM/AgentList?parentID=" + node.id;
            }
        });

        $("#reset_form").click(
            function () {
                $("#regDTS,#regDTE,#loginDTS,#loginDTE,input[name='userName'],input[name='amountType'],input[name='amountTypeV'],input[name='pointType'],input[name='pointTypeV'],input[name='userName']").val("");
            });

        //$("#if_agent_list").attr("src", "/AM/AgentList?parentID=0");

        $(".show_tree").click(function ()
        {
            $("#parent_tree").toggleClass("dom-hide");
        });

        jQuery('#regDTS').datetimepicker({
            format: 'Y/m/d',
            lang: 'ch',
            onShow: function (ct) {
                this.setOptions({
                    maxDate: jQuery('#regDTE').val() ? jQuery('#regDTE').val() : false
                })
            },
            timepicker: false
        });
        jQuery('#regDTE').datetimepicker({
            format: 'Y/m/d',
            lang: 'ch',
            onShow: function (ct) {
                this.setOptions({
                    minDate: jQuery('#regDTS').val() ? jQuery('#regDTS').val() : false
                })
            },
            timepicker: false
        });
        jQuery('#loginDTS').datetimepicker({
            format: 'Y/m/d',
            lang: 'ch',
            onShow: function (ct) {
                this.setOptions({
                    maxDate: jQuery('#loginDTE').val() ? jQuery('#loginDTE').val() : false
                })
            },
            timepicker: false
        });
        jQuery('#loginDTE').datetimepicker({
            format: 'Y/m/d',
            lang: 'ch',
            onShow: function (ct) {
                this.setOptions({
                    minDate: jQuery('#loginDTS').val() ? jQuery('#loginDTS').val() : false
                })
            },
            timepicker: false
        });
    });

    $("a[name='send_frozenSum']").click(function ()
    {
        $("#user_send_frozenSum").dialog("open");
        $("input[name='key']").val($(this).attr("data"));
        $("#send_to1_user").html($(this).attr("data2"));
        $("#momey").html($(this).attr("data3"));
        $("input['name=frozenSum']").val("0");
    });
    $(".btn_send_frozenSum").click(function () {
        $.ajax({
            async: false, timeout: _global_ajax_timeout, type: "POST", cache: false, url: "/AM/AgentList?method=sendFrozenSum", data: $("#form_send_frozenSum").serialize(),
            success: function (a, b) {
                _check_auth(a);
                var _robj = eval(a);
                if (1 == _robj.Code) {
                    alert("冻结金额成功");
                    refresh_current_page();
                }
                else {
                    alert(_robj.Message);
                }
            }
        });
    });
    $(".btn_send_point").click(function ()
    {
        $.ajax({
            async: false, timeout: _global_ajax_timeout, type: "POST", cache: false, url: "/AM/AgentList?method=sendPoint", data: $("#form_send_point").serialize(),
            success: function (a, b) {
                _check_auth(a);
                var _robj = eval(a);
                if (1 == _robj.Code) {
                    alert("赠送积分成功");
                    refresh_current_page();
                }
                else {
                    alert(_robj.Message);
                }
            }
        });
    });

    $("a[name='send_point']").click(function () {
        $("#user_send_point").dialog("open");
        $("input[name='key']").val($(this).attr("data"));
        $("#send_to_user").html($(this).attr("data2"));
        $("input['name=point']").val("0");
    });

    $("a[name='unbind_bank']").click(function ()
    {
        if (confirm("解绑操作会删除用户的提现银行卡信息！是否继续？")) {
            var key = $(this).attr("data");
            $.ajax({
                async: false, timeout: _global_ajax_timeout, type: "POST", cache: false, url: "/AM/AgentList?method=userBankUnbind", data: { key: key },
                success: function (a, b) {
                    _check_auth(a);
                    var _robj = eval(a);
                    if (1 == _robj.Code) {
                        refresh_current_page();
                    }
                    else {
                        alert(_robj.Message);
                    }
                }
            });
        }
    });

    $("a[name='delete_user']").click(function () {
        if (confirm("删除用户将删除所有相关信息！可能会导致账目不对")) {
            var key = $(this).attr("data");
            $.ajax({
                async: false, timeout: 1000 * 60, type: "POST", cache: false, url: "/AM/AgentList?method=deleteUser", data: { key: key },
                success: function (a, b) {
                    _check_auth(a);
                    var _robj = eval(a);
                    if (1 == _robj.Code) {
                        refresh_current_page();
                    }
                    else {
                        alert(_robj.Message);
                    }
                }
            });
        }
    });

    $("#btn_update_bank").click(function ()
    {
        if (false == confirm("确认更改？"))
        {
            return;
        }
        var form_data = $("#form_edit_bank").serialize();
        $.ajax({
            async: false, timeout: _global_ajax_timeout, type: "POST", cache: false, url: "/AM/AgentList?method=editBank", data: form_data,
            success: function (a, b) {
                _check_auth(a);
                var _robj = eval(a);
                if (1 == _robj.Code)
                {
                    _global_close_ui();
                }
                else {
                    alert(_robj.Message);
                }
            }
        });
    });
    $("a[name='edit_user_bank']").click(function () {
        _global_ui("edit_bank_dlg", 0, 30, 20, 0, "银行卡", 0);
        var key = $(this).attr("data");
        $.ajax({
            async: false, timeout: _global_ajax_timeout, type: "POST", cache: false, url: "/AM/AgentList?method=editBank", data: { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(), key: key },
            success: function (a, b) {
                _check_auth(a);
                var _robj = eval(a);
                if (1 == _robj.Code) {
                    $('input[name="bank_user"]').val(a.Data.uwi004);
                    $('input[name="bank_no"]').val(a.Data.uwi005);
                    $('input[name="bank_location"]').val(a.Data.uwi006);
                    $('input[name="bank_open"]').val(a.Data.uwi003);
                    $('input[name="updatekey"]').val(a.Data.uwi001);
                    $("#bank_code option[value='" + a.Data.uwt001 + "']").attr("selected", true);
                    $("#bank_t").html(a.Message);
                }
                else
                {
                    alert(_robj.Message);
                    _global_close_ui();
                }
            }
        });
    });
    $(".btn_update_user").click(function () {
        var form_data = $("#form_edit_user").serialize();
        $.ajax({
            async: false, timeout: _global_ajax_timeout, type: "POST", cache: false, url: "/AM/AgentList?method=updateUser", data: form_data,
            success: function (a, b) {
                _check_auth(a);
                var _robj = eval(a);
                if (0 == _robj.Code) {
                    alert(_robj.Message);
                }
                else {
                    refresh_current_page();
                }
            }
        });
    });
    $("a[name='edit_user']").click(function () {
        var key = $(this).attr("data");
        $.ajax({
            async: false, timeout: _global_ajax_timeout, type: "POST", cache: false, url: "/AM/AgentList?method=editUser", data: { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(), key: key },
            success: function (a, b)
            {
                _check_auth(a);
                //_global_ui("edit_user_dlg", 480, 30, 5, 0, "账户修改");
                $("#edit_user_dlg").dialog("open");
                var _robj = eval(a);
                if (1 == _robj.Code)
               {
                    $("#edit_key").val(_robj.Data.UpdateKey);
                    $("#ui_acct").html(_robj.Data.UserName);
                    $("#ui_nkn").val(_robj.Data.UserNickname);
                    $("#ui_rd").html(_robj.Data.RegDate);
                    $("#ui_ld").html(_robj.Data.LoginDate);
                    $("#ui_ip").html(_robj.Data.LoginIP);
                    $("#status option[value='" + _robj.Data.UserState.toString() + "']").prop("selected", true);
                    $("#can_child option[value='" + _robj.Data.CanCreate.toString() + "']").prop("selected", true);
                    $("#red_percent").val(_robj.Data.RedPercent);
                    $("#point_list").empty();
                    for (var i = 0; i < _robj.Data.PDL.length; i++) {
                        $("#point_list").append('<tr><td class="title">' + '<input name="pid" type="hidden" value="' + _robj.Data.PDL[i].PID + '" />' + _robj.Data.PDL[i].GameClassName + "</td><td>" + "返点：" + _robj.Data.PDL[i].Point + '<span>，新返点：<input name="point' + _robj.Data.PDL[i].PID + '" type="text" class="input-text w30px" value="' + _robj.Data.PDL[i].Point + '" />' + "</span></td></tr>");
                    }
                }
            },
            complete: function (a, b) {
            }
        });
    });
</script>
</asp:Content>
