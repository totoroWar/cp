<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI2/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="/Scripts/jquery/jquery-ui.min.js"></script>
<%
    var methodType = (string)ViewData["MethodType"];
%>
<div class="m_body_bg">
<div class="main_box main_tbg">
<div class="main_table_bg">
    <div class="main_table_box">
        <!---个人资料 start-->
        <div class="user_info_box">
            <div class="user_info_tab">
                <ul>
                    <span><a class="info_close" href="#"></a></span>
                    <li class="on"><a href="/UI2/UCenter?method=accountInfo" <%=methodType == "accountInfo" ? "class='item-select'" : "" %>>账户资料</a></li>
                    <li><a href="/UI2/UCenter?method=changePassword" <%=methodType == "changePassword" ? "class='item-select'" : "" %>>修改密码</a></li>
                    <li><a href="/UI2/UCenter?method=withdrawBank" <%=methodType == "withdrawBank" ? "class='item-select'" : "" %>>提现银行</a></li>
                    <li><a href="/UI2/UCenter?method=loginHistory" <%=methodType == "loginHistory" ? "class='item-select'" : "" %>>登陆日志</a></li>
                    <li><a href="/UI2/UCenter?method=childOnline" <%=methodType == "childOnline" ? "class='item-select'" : "" %>>在线下级</a></li>
                </ul>
            </div>
<%if ("accountInfo" == methodType)
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
<div class="user_info_dt_c">
    <div class="user_info_txt">
        
        <dl >
            <table style="text-align:left">
            <tr>
                <td><dd><font color="#DB8B01">账号：</font></dd></td>
                <td><dd><span><%:ViewData["UILoginAccount"] %></span></dd></td>
                <td><dd><font color="#DB8B01">昵称：</font></dd></td>
                <td><dd><span><%:ViewData["UILoginNickname"] %></span></dd></td>
                <td><dd><font color="#DB8B01">分红：</font></dd></td>
                <td><dd><span><%=ViewData["AGStock"] %>%</span></dd></td>
                <td><dd><font color="#DB8B01">级别：</font></dd></td>
                <td><dd><span><%=acctLevelDicList[(int)ViewData["AGAcctLevel"]].Name %></span></dd></td>
                <td><dd><font color="#DB8B01">可用余额：</font></dd></td>
                <td><dd><span><%:string.Format("{0:N2}",ViewData["AGSMoney"]) %></span></dd></td>
            </tr>
                <tr>
                <td><dd><font color="#DB8B01">冻结金额：</font></dd></td>
                <td><dd><span><%:string.Format("{0:N2}",ViewData["AGSHoldMoney"]) %></span></dd></td>
                <td><dd><font color="#DB8B01">可用积分：</font></dd></td>
                <td><dd><span><%:string.Format("{0:N2}",ViewData["AGSPoint"]) %></span></dd></td>
                <td><dd><font color="#DB8B01">VIP：</font></dd></td>
                <td><dd><span><%:levelDicList[(int)ViewData["AGLevel"]] %></span></dd></td>
                <td><dd><font color="#DB8B01">头衔：</font></dd></td>
                <td><dd><span><%=ViewData["AGPosName"] %></span></dd></td>
                <td><dd></dd></td>
                <td><dd></dd></td>
            </tr>
        </table>
           
        </dl>
       
    </div>
  
    <div class="user_fandian" style="margin-top:12px">
        <ul>
            <li class="fd_01"><font color="#DB8B01">返点：</font></li>
            <li class="fd_02">游戏分类</li>
            <li class="fd_03">包含游戏</li>
            <li class="fd_04">拥有返点</li>
            <li class="fd_05">状态</li>
        </ul>
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
        <dl>
            <dd class="fd_01"><font></font></dd>
            <dd class="fd_02"><a href="#"><%:gcName %></a></dd>
            <dd class="fd_03"><%foreach (var g in gameIDs)
                              {%>
                                <a href="#"><%:gDicList[int.Parse(g)].g003 %></a>
                              <%} %></dd>
            <dd class="fd_04"><a href="#"><%:string.Format("{0:N1}",agp.up003) %></a></dd>
            <dd class="fd_05"><a href="javascript:void(0);" gpkey="<%:gpkey %>" gckey="<%:gcKey %>" class="show_agp_detail" title="<%:gcName %>">[查看详细]</a></dd>
        </dl>
         <%} /*foreach gplist*/%>
        <div class="clear"></div>
    </div>
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
        var game_name = $(this).attr("title");
        $("#agp_gpkey").val($(this).attr("gpkey"));
        $("#agp_gckey").val($(this).attr("gckey"));
        var form_data = $("#show_agp_detail").serialize();
        $.ajax({
            async: false, timeout: _global_ajax_timeout, type: "POST", url: "/UI2/UserPrizeInfo", data: form_data, dataType: "html",
            success: function (a, b) {
                //$(".agup_info").html(a);
                $("#ui_up").html(a);
                $("#ui_up").dialog({ title: game_name, width: 500, height: 400, modal: true, resizable: false, position: { my: "center", at: "center", of: window } });
            }
        });
    });
</script>
<%}/*accountInfo*/ %>                    
<%else if ("withdrawBank" == methodType)
{ %>
    <div class="user_info_data_box">
        <div class="user_add_bank">
			<div class="bank_add_btn">
                <span>&gt;&gt;添加银行卡</span><a class="add_bank_a btn-normal add_bank_a" href="#" id="btn_withdraw_edit">添加银行卡</a>
            </div>
            <div class="clear"></div>
            <div class="bank_warn">
                遵循一人一卡原则，杜绝一人多卡现象
            </div>
            <div class="clear"></div>
        
        <%--<div id="dlg_withdraw_edit" class="ajax_tbox">
            <form action="./" method="post" id="form_data_withdraw_info1">
                <input type="hidden" name="uwi001" value="0" />
                <input type="hidden" name="method" value="withdrawBank" />
                <%:Html.AntiForgeryToken() %>
                <div class="ajax_tbox" style="width:100%; margin-top:50px;">

                    <div class="ajax_h3"><span><a class="gda_close_a ui-icon-closethick" href="#"></a></span><cite class="s_scart_t">银行卡</cite></div>
                    <div class="ajax_content">
                        <div class="gd_details scart_box">
                            <ul>
                                <li><span>资金密码：</span> <input type="password" name="cash_password" class="input-text w300px" value="" /></li>
                                <li><span>银行类型：</span> 
                                        <select name="uwt001">
                                        <%
                      var wtList = (List<DBModel.wgs024>)ViewData["WTList"];
                      foreach (var item in wtList)
                      {
                                        %>
                                        <option value="<%:item.uwt001 %>"><%:item.uwt003 %></option>
                                        <%} %>
                                    </select>
                                </li>
                                <li><span>开户人姓名：</span><input type="text" name="uwi004" class="input-text w300px" value="" /></li>
                                <li><span>卡号/账号：</span><input type="text" name="uwi005" class="input-text w300px" value="" /></li>
                                <li><span>确认卡号/账号：</span> <input type="text" name="uwi005_confirm" class="input-text w300px" value="" /></li>
                                <li><span>地区选择：</span> 省区<select id="select_p"></select>
                                    城市<select id="select_c"></select></li>
                                <li><span>开户所在地区：</span><input type="text" id="wd_region" name="uwi006" class="input-text w300px" value="" readonly="readonly" /></li>
                                <li><span>开户行：</span><input type="text" name="uwi003" class="input-text w300px" value="" /></li>
                                <li><span>&nbsp;</span><label>*</label><a class="buy_btn fl_r" id="btn_save_wb" class="btn-normal">保存</a></li>
                            </ul>
                            <div class="clear"></div>
                        </div>
                    </div>
                </div>
            </form>
        </div>--%>
            <style>
                .dom-hide{display:none}
                #form_data_withdraw_info{display:block}
            </style>
<div id="dlg_withdraw_edit" class="dom-hide" >
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
                        <select name="uwt001" class="select_2015">
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
                    <td>省区<select id="select_p" class="select_2015"></select>
                        城市<select id="select_c" class="select_2015"></select>
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
            <div class="bank_list_box">
                <div class="bank_list">
                    <table class="ctable_box bank_table_list" width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tbody><tr>
                            <th>编号</th>
                            <th>类型</th>
                            <th>账号卡号</th>
                            <th>姓名</th>
                            <th>开户行</th>
                            <th>地区</th>
                            <th>绑定时间</th>
                        </tr>
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
                    </tbody></table>

                </div>
                
            </div>
        </div>
    </div>
<script type="text/javascript" src="/Scripts/UI/Default/Location.js"></script>
<script type="text/javascript">
    $(this).set_lpc_select("select_p", "select_c", "wd_region");
    $("#btn_withdraw_edit").click(function () {
        //$(".dom-hide").attr("display", "block");
        $("#dlg_withdraw_edit").dialog({ width: 450, title: "提现银行编辑", modal: true, resizable: false, position: { my: "center", at: "center", of: window } });
    });
    $("#btn_save_wb").click(function () {
        var form = $(this).parents("form");
        var form_data = $(this).parents("form").serialize();
        $.ajax({
            async: false, timeout: _global_ajax_timeout, type: "POST", url: "/UI2/UCenter", data: form_data, dataType: "json",
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
<%if ("changePassword" == methodType)
{%>
<div class="user_mod_psw">
<form method="post" action="./" id="form_data_1">
    <input type="hidden" name="method" value="changePassword" />
    <input type="hidden" name="target" value="loginPassword" />
    <%:Html.AntiForgeryToken() %>   
    <div class="mode_psw fl_l">
        <h3>修改登录密码</h3>
        <dl>
            <dt>输入旧密码：</dt>
            <dd>
                <input class="psw_txt" value="dingshengyule" name="old_pwd" type="password">
                <br><label>旧密码输入错误，请重新输入</label>
            </dd>
        </dl>
        <dl>
            <dt>输入新密码：</dt>
            <dd>
                <input class="psw_txt" name="new_pwd" type="password">
                <br><label class="display_n">新密码输入错误，请重新输入</label>
            </dd>
        </dl>
        <dl>
            <dt>确认新密码：</dt>
            <dd>
                <input class="psw_txt" name="new_pwd_ok" type="password">
                <br><label class="display_n">输入错误，请重新输入</label>
            </dd>
        </dl>
        <div class="psw_submit">
            <input id="btn_change_pwd_1" class="psw_btn btn-normal ui-post-loading" value="确认修改" type="button">
        </div>
    </div>
</form>
<form method="post" action="./" id="form_data_2">
    <input type="hidden" name="method" value="changePassword" />
    <input type="hidden" name="target" value="cashPassword" />
    <%:Html.AntiForgeryToken() %>                    
    <div class="mode_psw margin_left45 fl_l">
        <h3>修改资金密码</h3>
        <dl>
            <dt>输入旧密码：</dt>
            <dd>
                <input class="psw_txt" name="old_pwd" type="password">
                <br><label>如果还未设置过密码，旧密码可随意输入</label>
            </dd>
        </dl>
        <dl>
            <dt>输入新密码：</dt>
            <dd>
                <input class="psw_txt" name="new_pwd" type="password">
                <br><label class="display_n">旧密码输入错误，请重新输入</label>
            </dd>
        </dl>
        <dl>
            <dt>确认新密码：</dt>
            <dd>
                <input class="psw_txt" name="new_pwd_ok" type="password">
                <br><label class="display_n">旧密码输入错误，请重新输入</label>
            </dd>
        </dl>
        <div class="psw_submit">
            <input id="btn_change_pwd_2" class="psw_btn btn-normal ui-post-loading" value="确认修改" type="button">
        </div>
    </div>
</form>
<form method="post" action="./" id="form1">
    <input type="hidden" name="method" value="changePassword" />
    <input type="hidden" name="target" value="welcomeMessage" />
    <%:Html.AntiForgeryToken() %>                     
    <div class="mode_psw margin_left45 fl_l">
        <h3>修改问候语</h3>
        <dl class="why_dl">
            <dt class="why_t">问候语：</dt>
            <dd class="t_wenhouyu">
                <textarea class="ta_why" name="login_message"></textarea>
            </dd>
        </dl>
        <div class="clear"></div>
        <div class="psw_submit">
            <input id="btn_save_lm" class="psw_btn btn-normal ui-post-loading" value="确认修改" type="button">
        </div>
    </div>
 </form>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $("#btn_change_pwd_1,#btn_change_pwd_2,#btn_save_lm").click(function () {
            var form = $(this).parents("form");
            var form_data = $(this).parents("form").serialize();
            var this_id = $(this).attr("id");
            $.ajax({
                async: false, timeout: _global_ajax_timeout, type: "POST", url: "/UI2/UCenter", data: form_data, dataType: "json",
                success: function (a, b) {
                    _check_auth(a);
                    if (0 == a.Code) {
                        alert(a.Message);
                    }
                    else if (1 == a.Code) {
                        alert(a.Message);
                        if ("btn_change_pwd_1" == this_id) {
                            top.location.href = "/UI2/Logout";
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
<%else if ("loginHistory" == methodType)
{
    var ipLOC = new NETCommon.IPPhyLoc(ViewData["IPFP"].ToString());
    %>
        <div class="user_add_bank">
            <div class="bank_list_box">
                <div class="bank_list">
                    <table width="100%" class="ctable_box bank_table_list" border="0" cellspacing="0" cellpadding="0">
                        <tbody><tr>
                            <th>编号</th>
                            <th>登录时间</th>
                            <th>网络</th>
                            <th>物理地址</th>
                        </tr>
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
                    </tbody></table>

            </div>
                <%=ViewData["PageList"] %>
        </div>
    </div>
        
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
<div class="user_info_data_box">
    <div class="user_add_bank">
        <div class="bank_list_box">
            <div class="bank_list">
                <table width="100%" class="ctable_box bank_table_list" border="0" cellspacing="0" cellpadding="0">
                    <tbody><tr>
                        <th>账号</th>
                        <th>级别</th>
                        <th>状态</th>
                        <th>登陆时间</th>
                        <th>更新时间</th>
                        <th>网络地址</th>
                        <th>物理地址</th>
                    </tr>
                   <%if( null != conList)
                  {
                      foreach(var item in conList)
                      {
                          var curUser = childList.Where(exp=>exp.u002 == item.u001).Take(1).FirstOrDefault();
                          DBModel.SysAccountLevel level = null;
                          if (null != curUser)
                          {
                              level = acctLevel.Where(exp => exp.Level == curUser.u002l).Take(1).FirstOrDefault();  
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
                </tbody></table>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    $("#onlyshow").change(function () {
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
        </div>
        <!---个人资料 end-->
        <div class="clear"></div>  
    </div>
</div>
</div>


</div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
