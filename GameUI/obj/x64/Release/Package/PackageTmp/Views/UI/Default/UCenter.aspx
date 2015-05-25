<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var methodType = (string)ViewData["MethodType"];
    %>
    <div class="ui-page-content-body-header tools">
        <div class="left-nav">
            <a href="/UI/UCenter?method=changePassword" title="修改密码" <%=methodType == "changePassword" ? "class='item-select'" : "" %>>修改密码</a>
            <a href="/UI/UCenter?method=withdrawBank" title="提现银行" <%=methodType == "withdrawBank" ? "class='item-select'" : "" %>>提现银行</a>
            <a href="/UI/UCenter?method=accountInfo" title="账户资料" <%=methodType == "accountInfo" ? "class='item-select'" : "" %>>账户资料</a>
            <a href="/UI/UCenter?method=loginHistory" title="登录日志" <%=methodType == "loginHistory" ? "class='item-select'" : "" %>>登录日志</a>
            <a href="/UI/UCenter?method=childOnline" title="在线下级" <%=methodType == "childOnline" ? "class='item-select'" : "" %>>在线下级</a>
        </div>
    </div>
    <div class="blank-line"></div>
    <%if ("changePassword" == methodType)
      {%>
    <form method="post" action="./" id="form_data_1">
        <input type="hidden" name="method" value="changePassword" />
        <input type="hidden" name="target" value="loginPassword" />
        <%:Html.AntiForgeryToken() %>
        <table class="table-pro tp5" width="100%">
            <tr>
                <td class="title"></td>
                <td>登录密码</td>
            </tr>
            <tr>
                <td class="title">输入旧密码：</td>
                <td>
                    <input type="password" name="old_pwd" class="input-text w200px" /></td>
            </tr>
            <tr>
                <td class="title">输入新密码：</td>
                <td>
                    <input type="password" name="new_pwd" class="input-text w200px" /></td>
            </tr>
            <tr>
                <td class="title">确认新密码：</td>
                <td>
                    <input type="password" name="new_pwd_ok" class="input-text w200px" /></td>
            </tr>
            <tr>
                <td class="title"></td>
                <td>
                    <input id="btn_change_pwd_1" type="button" class="btn-normal ui-post-loading" value="确认" /></td>
            </tr>
        </table>
    </form>
    <div class="blank-line"></div>
    <form method="post" action="./" id="form_data_2">
        <input type="hidden" name="method" value="changePassword" />
        <input type="hidden" name="target" value="cashPassword" />
        <%:Html.AntiForgeryToken() %>
        <table class="table-pro tp5" width="100%">
            <tr>
                <td class="title"></td>
                <td>资金密码</td>
            </tr>
            <tr>
                <td class="title">输入旧密码：</td>
                <td>
                    <input type="password" name="old_pwd" class="input-text w200px" /><span class="tipsline">如果还未设置过密码，旧密码可随意输入</span></td>
            </tr>
            <tr>
                <td class="title">输入新密码：</td>
                <td>
                    <input type="password" name="new_pwd" class="input-text w200px" /></td>
            </tr>
            <tr>
                <td class="title">确认新密码：</td>
                <td>
                    <input type="password" name="new_pwd_ok" class="input-text w200px" /></td>
            </tr>
            <tr>
                <td class="title"></td>
                <td>
                    <input id="btn_change_pwd_2" type="button" class="btn-normal ui-post-loading" value="确认" /></td>
            </tr>
        </table>
    </form>
    <div class="blank-line"></div>
    <form method="post" action="./" id="form1">
        <input type="hidden" name="method" value="changePassword" />
        <input type="hidden" name="target" value="welcomeMessage" />
        <%:Html.AntiForgeryToken() %>
        <table class="table-pro tp5" width="100%">
            <tr>
                <td class="title"></td>
                <td>问候语</td>
            </tr>
            <tr>
                <td class="title">问候语：</td>
                <td>
                    <input type="text" name="login_message" class="input-text w200px" maxlength="10" /></td>
            </tr>
            <tr>
                <td class="title"></td>
                <td>
                    <input id="btn_save_lm" type="button" class="btn-normal ui-post-loading" value="确认" /></td>
            </tr>
        </table>
    </form>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#btn_change_pwd_1,#btn_change_pwd_2,#btn_save_lm").click(function () {
                var form = $(this).parents("form");
                var form_data = $(this).parents("form").serialize();
                var this_id = $(this).attr("id");
                $.ajax({
                    async: false, timeout: _global_ajax_timeout, type: "POST", url: "/UI/UCenter", data: form_data, dataType: "json",
                    success: function (a, b) {
                        _check_auth(a);
                        if (0 == a.Code) {
                            alert(a.Message);
                        }
                        else if (1 == a.Code) {
                            alert(a.Message);
                            if ("btn_change_pwd_1" == this_id) {
                                top.location.href = "/UI/Logout";
                            }
                            ui_mask_panel_close();
                            form[0].reset();
                        }
                    },
                    complete: function (a, b) {
                        ui_mask_panel_close();
                    }
                });
            });
        });
    </script>
    <%}/*changePassword*/ %>
    <%else if ("withdrawBank" == methodType)
      { %>
    <div class="block_tools">
    <input type="button" id="btn_withdraw_edit" class="btn-normal" value="添加银行卡" />
    </div>
    <div class="blank-line"></div>
    <div id="dlg_withdraw_edit" class="dom-hide">
        <form action="./" method="post" id="form_data_withdraw_info">
            <input type="hidden" name="uwi001" value="0" />
            <input type="hidden" name="method" value="withdrawBank" />
            <%:Html.AntiForgeryToken() %>
            <table class="table-pro table-noborder" width="100%">
                <tr>
                    <td class="title">资金密码</td>
                    <td>
                        <input type="password" name="cash_password" class="input-text w300px" value="" /></td>
                </tr>
                <tr>
                    <td class="title">银行类型</td>
                    <td>
                        <select name="uwt001">
                            <%
          var wtList = (List<DBModel.wgs024>)ViewData["WTList"];
          foreach (var item in wtList)
          {
                            %>
                            <option value="<%:item.uwt001 %>"><%:item.uwt003 %></option>
                            <%} %>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td class="title">开户人姓名</td>
                    <td>
                        <input type="text" name="uwi004" class="input-text w300px" value="" /></td>
                </tr>
                <tr>
                    <td class="title">卡号/账号</td>
                    <td>
                        <input type="text" name="uwi005" class="input-text w300px" value="" /></td>
                </tr>
                <tr>
                    <td class="title">确认卡号/账号</td>
                    <td>
                        <input type="text" name="uwi005_confirm" class="input-text w300px" value="" /></td>
                </tr>
                <tr>
                    <td class="title">地区选择</td>
                    <td>省区<select id="select_p"></select>
                        城市<select id="select_c"></select>
                    </td>
                </tr>
                <tr>
                    <td class="title">开户所在地区</td>
                    <td>
                        <input type="text" id="wd_region" name="uwi006" class="input-text w300px" value="" readonly="readonly" /></td>
                </tr>
                <tr>
                    <td class="title">开户行</td>
                    <td>
                        <input type="text" name="uwi003" class="input-text w300px" value="" /></td>
                </tr>
                <tr>
                    <td class="title"></td>
                    <td>
                        <input type="button" id="btn_save_wb" class="btn-normal" value="保存" /></td>
                </tr>
            </table>
        </form>
    </div>
    <%=ViewData["WDTips"] %>
    <div class="blank-line"></div>
    <table class="table-pro table-color-row tp5 g_nco" width="100%">
        <thead>
            <tr class="table-pro-head">
                <th class="w50px">编号</th>
                <th>类型</th>
                <th>卡号/账号</th>
                <th>姓名</th>
                <th>开户行</th>
                <th>地区</th>
                <th>绑定时间</th>
            </tr>
        </thead>
        <tbody>
            <%
          var wtDicList = wtList.ToDictionary(exp => exp.uwt001);
          var mwtList = (List<DBModel.wgs023>)ViewData["MWTList"];
          if (null != mwtList)
          {
              foreach (var item in mwtList)
              {
            %>
            <tr>
                <td><%:item.uwi001 %></td>
                <td><%:wtDicList[item.uwt001].uwt003 %></td>
                <td><%:item.uwi005 %></td>
                <td><%:item.uwi004 %></td>
                <td><%:item.uwi003 %></td>
                <td><%:item.uwi006 %></td>
                <td><%:item.uwi010 %></td>
            </tr>

            <%
              }/*foreach*/
          }/*if*/%>
        </tbody>
        <tfoot class="dom-hide">
            <tr>
                <td colspan="10"></td>
            </tr>
        </tfoot>
    </table>
    <script type="text/javascript" src="/Scripts/UI/Default/Location.js"></script>
    <script type="text/javascript">
        $(this).set_lpc_select("select_p", "select_c", "wd_region");
        $("#btn_withdraw_edit").click(function () {
            $("#dlg_withdraw_edit").dialog({ width: 450, title: "提现银行编辑", modal: true, resizable: false, position: { my: "center", at: "center", of: window } });
        });
        $("#btn_save_wb").click(function () {
            var form = $(this).parents("form");
            var form_data = $(this).parents("form").serialize();
            $.ajax({
                async: false, timeout: _global_ajax_timeout, type: "POST", url: "/UI/UCenter", data: form_data, dataType: "json",
                success: function (a, b) {
                    _check_auth(a);
                    alert(a.Message)
                    if (0 == a.Code) {
                    }
                    else if (1 == a.Code) {
                        location.reload();
                        ui_mask_panel_close();
                    }
                },
                complete: function (a, b) {
                    ui_mask_panel_close();
                }
            });
        });
    </script>
    <%}/*withdrawBank*/ %>
    <%else if ("accountInfo" == methodType)
      {
          var agPointList = (List<DBModel.wgs017>)ViewData["AGPoint"];
          var gList = (List<DBModel.wgs001>)ViewData["GList"];
          var gcList = (List<DBModel.wgs006>)ViewData["GCList"];
          var gpList = (List<DBModel.wgs007>)ViewData["GPList"];
          var gDicList = gList.ToDictionary(exp => exp.g001);
          var gpDicList = gpList.ToDictionary(exp => exp.gp001);
          var gcDicList = gcList.ToDictionary(exp => exp.gc001);
          var levelDicList = (Dictionary<int, string>)ViewData["AGLevelName"];
          var acctLevelList = (List<DBModel.SysAccountLevel>)ViewData["AcctLevelList"];
          var acctLevelDicList = acctLevelList.ToDictionary(exp => exp.Level);
    %>
    <table class="table-pro tp5 g_nco" width="100%">
        <tr>
            <td class="title">账号</td>
            <td><%:ViewData["UILoginAccount"] %></td>
        </tr>
        <tr>
            <td class="title">昵称</td>
            <td><%:ViewData["UILoginNickname"] %></td>
        </tr>
        <tr>
            <td class="title">分红</td>
            <td><%=((decimal)ViewData["AGStock"]).ToString("N2") %>%</td>
        </tr>
        <tr>
            <td class="title">级别</td>
            <td><%=acctLevelDicList[(int)ViewData["AGAcctLevel"]].Name %></td>
        </tr>
        <tr>
            <td class="title">可用余额</td>
            <td id="ag_money" class="fc-money"><%:string.Format("{0:N2}",ViewData["AGSMoney"]) %></td>
        </tr>
        <tr>
            <td class="title">冻结金额</td>
            <td><%:string.Format("{0:N2}",ViewData["AGSHoldMoney"]) %></td>
        </tr>
        <tr>
            <td class="title">可用积分</td>
            <td><%:string.Format("{0:N2}",ViewData["AGSPoint"]) %></td>
        </tr>
        <tr>
            <td class="title">VIP</td>
            <td><%:levelDicList[(int)ViewData["AGLevel"]] %></td>
        </tr>
        <tr>
            <td class="title">军衔</td>
            <td><%=ViewData["AGPosName"] %></td>
        </tr>
        <tr>
            <td class="title">返点</td>
            <td>
                <table class="table-pro g_nco">
                    <thead>
                        <tr class="table-pro-head">
                            <th>游戏分类</th>
                            <th>包含游戏</th>
                            <th>拥有返点</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        <%foreach (var agp in agPointList)
                          {
                              var gpkey = agp.gp001;
                              var gcKey = gpDicList[(int)agp.gp001].gc001;
                              var gcName = gcDicList[gpDicList[(int)agp.gp001].gc001].gc003;
                              var gcState = gcDicList[gpDicList[(int)agp.gp001].gc001];
                              var gameIDs = gcDicList[gpDicList[(int)agp.gp001].gc001].gc004.Split(',');
                              if (0 == gcState.gc006)
                                  continue;
                        %>
                        <tr>
                            <td><%:gcName %></td>
                            <td>
                                <%foreach (var g in gameIDs)
                                  {%>
                                <span class="tips"><%:gDicList[int.Parse(g)].g003 %></span>
                                <%} %>
                            </td>
                            <td><span class="fc-red"><%:string.Format("{0:N1}",agp.up003) %></span></td>
                            <td><a href="javascript:void(0);" gpkey="<%:gpkey %>" gckey="<%:gcKey %>" class="show_agp_detail" title="<%:gcName %>">[查看详细]</a></td>
                        </tr>
                        <%} /*foreach gplist*/%>
                    </tbody>
                </table>
            </td>
        </tr>
    </table>
    <div class="dom-hide" id="ui_up">

    </div>
    <form action="./" method="post" id="show_agp_detail">
        <%:Html.AntiForgeryToken() %>
        <%--<input type="hidden" name="target" value="show_agp_detail" />--%>
        <input type="hidden" id="agp_gpkey" name="gpkey" value="0" />
        <input type="hidden" id="agp_gckey" name="gckey" value="0" />
        <%--<input type="hidden" name="method" value="accountInfo" />--%>
    </form>
    <script type="text/javascript">
        $(".show_agp_detail").click(function () {
            var game_name =$(this).attr("title");
            $("#agp_gpkey").val($(this).attr("gpkey"));
            $("#agp_gckey").val($(this).attr("gckey"));
            var form_data = $("#show_agp_detail").serialize();
            $.ajax({
                async: false, timeout: _global_ajax_timeout, type: "POST", url: "/UI/UserPrizeInfo", data: form_data, dataType: "html",
                success: function (a, b) {
                    //$(".agup_info").html(a);
                    $("#ui_up").html(a);
                    $("#ui_up").dialog({ title: game_name, width: 500, height: 400, modal: true, resizable: false, position: { my: "center", at: "center", of: window } });
                }
            });
        });
    </script>
    <div class="blank-line"></div>
    <div class="agup_info">
    </div>
    <%}/*accountInfo*/ %>
    <%else if ("loginHistory" == methodType)
      {
          var ipLOC = new NETCommon.IPPhyLoc(ViewData["IPFP"].ToString());
          %>
    <table class="table-pro table-color-row tp5 g_nco" width="100%">
        <thead>
            <tr class="table-pro-head">
                <th class="w50px">编号</th>
                <th>登录时间</th>
                <th>网络</th>
                <th>物理地址</th>
            </tr>
        </thead>
        <tbody>
            <%
          var loginHistoryList = (List<DBModel.wgs026>)ViewData["LoginHistoryList"];
          if (null != loginHistoryList)
          {
              foreach (var item in loginHistoryList)
              {
            %>
            <tr>
                <td><%:item.ulg001 %></td>
                <td><%:item.ulg002 %></td>
                <td><%:item.ulg004 %></td>
                <td><%:ipLOC.GetIPAll( item.ulg004.Trim() ) %></td>
            </tr>
            <%
              }/*foreach*/
          }/*if*/ %>
        </tbody>
    </table>
    <%}/*loginHistory*/ %>
    <%else if ("childOnline" == methodType)
      {
          var ipLOC = new NETCommon.IPPhyLoc(ViewData["IPFP"].ToString());
          var conList = (List<DBModel.wgs025>)ViewData["ChildsOnline"];
          var childList = (List<DBModel.wgs048>)ViewData["Childs"];
          var acctLevel = (List<DBModel.SysAccountLevel>)ViewData["AcctLevelList"];
          var acctPos = (List<DBModel.SysPositionLevel>)ViewData["AcctPosLevel"];
          %>
    <div class="block_tools">
        只显示在线<input type="checkbox" value="1" id="onlyshow" />
    </div>
    <table class="table-pro table-color-row tp5 g_nco" width="100%">
        <thead>
            <tr class="table-pro-head">
                <th>账号</th>
                <th>级别</th>
                <th>状态</th>
                <th>登录时间</th>
                <th>更新时间</th>
                <th>网络地址</th>
                <th>物理地址</th>
            </tr>
        </thead>
        <tbody>
            <%if( null != conList)
              {
                  foreach(var item in conList)
                  {
                      var curUser = childList.Where(exp=>exp.u002 == item.u001).Take(1).FirstOrDefault();
                      DBModel.SysAccountLevel level = null;
                      if (null != curUser)
                      {
                          level = acctLevel.Where(exp => exp.Level == curUser.u001l).Take(1).FirstOrDefault();  
                      }
                      
                    %>
            <tr class="online<%=item.onl006 == 1? "yes" : "no" %>">
                <td><%:item.u002.Trim() %></td>
                <td><%:level != null ? level.Name : "未知" %></td>
                <td><%=item.onl006 == 1 ? "<span class='fc-green'>在线</span>" : "<span>离线</span>" %></td>
                <td><%:item.onl003 %></td>
                <td><%:item.onl004 %></td>
                <td><%:item.onl005 %></td>
                <td><%:ipLOC.GetIPAll( item.onl005.Trim() ) %></td>
            </tr>
            <%}/*foreach*/
              }/*if*/ %>
        </tbody>
    </table>
    <script type="text/javascript">
        $("#onlyshow").change(function ()
        {
            var isOnline = $(this).prop("checked");
            if (isOnline) {
                $(".onlineno").hide();
            }
            else {
                $(".onlineno").show();
            }
        });
    </script>


    <%}/*loginHistory*/ %>
</asp:Content>
