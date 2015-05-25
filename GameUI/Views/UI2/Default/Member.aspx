<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI2/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
    var methodType = (string)ViewData["MethodType"];
%>
<div class="m_body_bg">
<div class="main_box main_tbg">
	<div class="main_table_bg">
    	<div class="main_table_box">
        	<div class="user_info_box">
            	<div class="user_info_tab">
                	<ul>
                    	<span><a class="info_close" href="#"></a></span>
                    	<li><a href="/UI2/Member?method=accountList" <%=methodType == "accountList" ? "class='on'" : "" %>>账号管理</a></li>
                        <li><a href="/UI2/Member?method=createAccount" <%=methodType == "createAccount" ? "class='on'" : "" %>>增加账号</a></li>
                        <li><a href="/UI2/Member?method=reportTotalMoney" <%=methodType == "reportTotalMoney" ? "class='on'" : "" %>>团队余额</a></li>
                        <li><a href="/UI2/Member?method=accountAuto" <%=methodType == "accountAuto" ? "class='on'" : "" %>>推广链接</a></li>
                    </ul>
                </div>
<%if ("accountList" == methodType)
    {
        %>
<%
        List<DBModel.UserPack> userPackList = (List<DBModel.UserPack>)ViewData["UserPackList"];
        var userStatus = (int)ViewData["UserStatus"];
        var amtT = (int)ViewData["AmtT"];
        var amtV = (int)ViewData["AmtV"];
        var pntT = (int)ViewData["PntT"];
        var pntV = (int)ViewData["PntV"];
        var posList = (List<DBModel.SysPositionLevel>)ViewData["PosList"];
        var posDicList = posList.ToDictionary(exp => exp.Level);
        var vipDicList = (Dictionary<int,string>)ViewData["AGLevelNameList"]; 
%>

<div class="user_info_data_box">
    <div class="user_add_bank">
        <div class="search_game_recode">
            <div class="gd_search_input">
                <ul>
                    <li>账号：<input type="text" class="gd_txt" name="userName" value="<%:ViewData["UserName"] %>"</li>
                    <li>注册时间：<input type="text" class="gd_txt gd_time" id="regDTS" name="regDTS" value="<%:ViewData["RDTS"] %>" />-<input type="text" class="gd_txt gd_time" id="regDTE" name="regDTE" value="<%:ViewData["RDTE"] %>" /></li>
                    <li>登陆时间：<input type="text" class="gd_txt gd_time" id="loginDTS" name="loginDTS" value="<%:ViewData["LDTS"] %>" />-<input type="text" class="gd_txt gd_time" id="loginDTE" name="loginDTE" value="<%:ViewData["LDTE"] %>" /></li>
                </ul>
                <div class="clear"></div>
                <form action="/UI2/Member" id="form_account_list" method="get">
                <ul class="margin_top12">
                    <li><span class="select_span">状态：</span>
                       <select name="userStatus" class="select_2015">
                                <option value="-1">所有</option>
                                <option value="0" <%=userStatus==0? "selected='selected'" : "" %>>停用</option>
                                <option value="1" <%=userStatus==1? "selected='selected'" : "" %>>正常</option>
                                <option value="2" <%=userStatus==2? "selected='selected'" : "" %>>暂停</option>
                                <option value="3" <%=userStatus==3? "selected='selected'" : "" %>>冻结</option>
                            </select>
                    </li>
                    <li><span class="select_span">金额：</span>
                        <select name="amountType" class="select_2015">
                                <option value="0" <%=amtT==0? "selected='selected'" : "" %>>所有</option>
                                <option value="1" <%=amtT==1? "selected='selected'" : "" %>>等于</option>
                                <option value="2" <%=amtT==2? "selected='selected'" : "" %>>小于</option>
                                <option value="3" <%=amtT==3? "selected='selected'" : "" %>>大于</option>
                            </select>
                    </li>
                    <li><input type="text" class="gd_txt" name="amountTypeV" value="<%:amtV %>" /></li>
                    <li><span class="select_span">积分：</span>
                        <select name="pointType" class="select_2015">
                                <option value="0" <%=pntT==0? "selected='selected'" : "" %>>所有</option>
                                <option value="1" <%=pntT==1? "selected='selected'" : "" %>>等于</option>
                                <option value="2" <%=pntT==2? "selected='selected'" : "" %>>小于</option>
                                <option value="3" <%=pntT==3? "selected='selected'" : "" %>>大于</option>
                            </select>
                    </li>
                    <li><input type="text" class="gd_txt" name="pointTypeV" value="<%:pntV %>" /></li>
                    <li><input type="submit" class="gd_sbtn ui-post-loading" value="查找" /></li>
                    <li><input type="button" id="reset_form" class="gd_cbtn" value="重置" /></li>
                    <li><input type="button" class="gd_fbtn ui-post-loading ui-page-back" value="返回" /></li>
                </ul>
                    </form>
            </div>
            <div class="clear"></div>
        </div>
        <div class="clear"></div>
        <div class="bank_list_box">
            <div class="bank_list">
                <table width="100%" class="ctable_box youxi_table_list" border="0" cellspacing="0" cellpadding="0">
                    <tbody><tr>
                        <th>账号</th>
                        <th>下级数</th>
                        <th>可用余额</th>
                        <th>冻结余额</th>
                        <th>积分</th>
                        <th>注册时间</th>
                        <th>登陆时间/地址</th>
                        <th>状态</th>
                        <th>VIP</th>
                        <th>头衔</th>
                        <th>分红比</th>
                        <th>操作</th>
                    </tr>
<%if( null != userPackList)
{ 
    foreach(var item in userPackList)
    { 
    %>
                    <tr>
<td><%:item.User.u002.Trim() %></td>
<td><%:item.ChildCount %></td>
<td class="fc-red"><%:item.User.wgs014.uf001.ToString("N2") %></td>
<td><%:item.User.wgs014.uf003.ToString("N2") %></td>
<td><%:item.User.wgs014.uf004.ToString("N2") %></td>
<td><%:item.User.u005 %></td>
<td><%:item.User.u007 %>/<%:item.User.u022 %></td>
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
<td><%=vipDicList[item.User.u015] %></td>
<td><%=posDicList[item.User.u013].Name %></td>
<td><%=(int)item.User.u024 * 100 %>%</td>
<td class="link-tools"><a href="javascript:void(0);" name="edit_user" data="<%:item.User.u001 %>" title="<%:item.User.u002.Trim() %>">修改</a><%if( item.ChildCount > 0){ %><a href="/UI2/Member?method=accountList&parentID=<%:item.User.u001 %>" title="查看下级">下级</a><%} %></td>
</tr>
<%
}/*foreach*/
}/*if*/ %>
                </tbody></table>
            </div>
            <div class="wp_page fl_r">
                <%=ViewData["PageList"] %>
             </div>
            </div>
<div id="edit_user_dlg" class="dom-hide" style="display:none">
<form action="#" method="post" id="form_edit_user">
<%:Html.AntiForgeryToken() %>
<input type="hidden" id="edit_key" name="edit_key" value="" />
<table class="table-pro table-noborder g_nco tp5" width="100%">
<tr>
<td class="title">账号</td>
<td id="ui_acct"></td>
</tr>
<tr>
<td class="title">昵称</td>
<td><input name="nickname" id="ui_nkn" type="text" class="input-text w150px" value="" /></td>
</tr>
<tr class="dom-hide">
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
<td class="title">注册时间</td>
<td id="ui_rd"></td>
</tr>
<tr class="dom-hide">
<td class="title">登录时间</td>
<td id="ui_ld"></td>
</tr>
<tr class="dom-hide">
<td class="title">登录地址</td>
<td id="ui_ip"></td>
</tr>
<tr>
<td class="title">分红</td>
<td><input id="ui_red" name="red_percent" type="text" class="input-text w50px" value="" readonly="readonly" />%<font color="'red'">(分红比例需由管理员修改)</font></td>
</tr>
<tr class="dom-hide">
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
<td>
<table class="table-pro table-noborder w100ps">
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
<td><input type="button" value="保存" class="btn-normal btn_update_user" /></td>
</tr>
</table>
</form>
</div>
<script type="text/javascript">
$(document).ready(function ()
{
$("a[name='edit_user']").click(function ()
{
var key = $(this).attr("data");
var edit_name = $(this).attr("title");
$.ajax({
async: false, timeout: _global_ajax_timeout, type: "POST", cache: false, url: "/UI2/Member?method=editUser", data: { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(), key: key },
success: function (a, b) {
    _check_auth(a);
    $("#edit_user_dlg").dialog({ width: 560, height: 500, modal: true, title: "修改账号-" + edit_name, resizable: false, position: { my: "center", at: "center", of: window } });
    var _robj = eval(a);
    $("#edit_key").val(_robj.Data.UpdateKey);
    $("#ui_acct").html(_robj.Data.UserName);
    $("#ui_nkn").val(_robj.Data.UserNickname);
    $("#ui_rd").html(_robj.Data.RegDate);
    $("#ui_ld").html(_robj.Data.LoginDate);
    $("#ui_ip").html(_robj.Data.LoginIP);
    $("#status option[value='" + _robj.Data.UserState.toString() + "']").prop("selected", true);
    $("#can_child option[value='" + _robj.Data.CanCreate.toString() + "']").prop("selected", true);
    $("#ui_red").val(_robj.Data.RedPercent);
    $("#point_list").empty();
    for (var i = 0; i < _robj.Data.PDL.length; i++)
    {
        $("#point_list").append('<tr><td class="title">' + '<input name="pid" type="hidden" value="' + _robj.Data.PDL[i].PID + '" />' + _robj.Data.PDL[i].GameClassName + "</td><td>" + "现返点：" + _robj.Data.PDL[i].Point + '<span>，返点：<input name="point' + _robj.Data.PDL[i].PID + '" type="text" class="input-text w30px" value="' + _robj.Data.PDL[i].Point + '" />' + "</span>>=" + _robj.Data.PDL[i].MaxPoint + "</td></tr>");
    }
},
complete: function (a, b) {
}
});
});
$(".btn_update_user").click(function ()
{
    var form_data = $("#form_edit_user").serialize();
    alert(form_data)
$.ajax({
async: false, timeout: _global_ajax_timeout, type: "POST", cache: false, url: "/UI2/Member?method=updateUser", data: form_data,
success: function (a, b)
{
    _check_auth(a);
    var _robj = eval(a);
    if (0 == _robj.Code)
    {
        alert(_robj.Message);
    }
    else
    {
        refresh_current_page();
    }
}
});
});
});
$("#reset_form").click(
function ()
{
$("#regDTS,#regDTE,#loginDTS,#loginDTE,input[name='amountType'],input[name='amountTypeV'],input[name='pointType'],input[name='pointTypeV'],input[name='userName']").val("");
});
</script>



    </div>
</div>
<%}/*accountList*/ %>
<%if ("reportTotalMoney" == methodType)
{
    %>
<%
    var tAmt = (decimal)ViewData["TAmt"];
    var tPnt = (decimal)ViewData["TPnt"];
    var tHon = (decimal)ViewData["THon"];
    var self = (int)ViewData["Self"];     
%>
<div class="user_info_data_box">
<form action="/UI2/Member" method="get">
<input type="hidden" name="method" value="reportTotalMoney" />
    <div class="user_add_teams">
        <div class="user_add_team team_yue">
            <dl>
                <dt>团队余额：</dt>
                <dd><font class="f_red"><%:tAmt.ToString("N2") %></font></dd>
            </dl>
            <dl>
                <dt>团队积分：</dt>
                <dd><%:tPnt.ToString("N2") %></dd>
            </dl>
            <dl>
                <dt>团队冻结：</dt>
                <dd><%:tHon.ToString("N2") %></dd>
            </dl>
            <dl>
                <dt>自己：</dt>
                <dd>
                    <select name="self" class="select_2015">
                            <option value="1" <%=self == 1 ? "selected='selected'" : "" %>>包含自己</option>
                            <option value="2" <%=self == 2 ? "selected='selected'" : "" %>>不包含自己</option>
                        </select>
                </dd>
            </dl>
            <dl style="padding-top: 35px;"><dt>&nbsp;</dt><dd><input type="submit" value="确认修改" class="psw_btn ui-post-loading" /></dd></dl>
        </div>
        <div class="clear"></div>
    </div>
</form>
</div>
<%}/*reportTotalMoney*/ %>
<%else if ("accountAuto" == methodType)
{ %>
<%
    var autoRegList = (List<DBModel.wgs035>)ViewData["AutoRegList"];
    var myPDList = (List<DBModel.wgs017>)ViewData["MyPDList"];
    //var gList = (List<DBModel.wgs001>)ViewData["GList"];
    //var gmList = (List<DBModel.wgs002>)ViewData["GMList"];
    var gcList = (List<DBModel.wgs006>)ViewData["GCList"];
    var gcDicList = gcList.ToDictionary(exp => exp.gc001);
%>
<div class="user_info_data_box">
    <div class="user_add_teams">
<form action="/UI2/Member?method=accountAuto" method="post" id="form_url">
<%:Html.AntiForgeryToken() %>
<input type="hidden" name="ar001" value="" />
<input type="hidden" name="ar002" value="" />
<input type="hidden" name="ar003" value="" />
<input type="hidden" name="ar004" value="" />
    <div class="user_add_team">
        <dl>
            <dt>允许开放下级：</dt>
            <dd style="padding-top:10px">
                <select name="allowcreateaccount" class="select_2015">
                        <option value="0" title="不允许">不允许</option>
                        <option value="1" title="允许" selected="selected">允许</option>
                    </select>
            </dd>
        </dl>
    </div><span id="error_message" style="color:red"></span>
    <div class="clear"></div>
    <div class="user_fandian margin_top12">
        <ul>
            <li class="fd_01"><font>返点：</font></li>
            <li class="fd_02">游戏分类</li>
            <li class="fd_03">我的返点</li>
            <li class="fd_04">注册返点</li>
        </ul>
        <%foreach(var mygp in myPDList){ %>
        <dl>
            <dd class="fd_01"></dd>
            <dd class="fd_02"><a href="#"><%:gcDicList[mygp.gc001].gc003 %></a></dd>
            <dd class="fd_03"><input type="hidden" name="pointid" value="<%:mygp.up001 %>" />我的返点<span class="fc-red"><%:mygp.up003.ToString("N1") %></span></dd>
            <dd class="fd_04">保留返点<input name="point" type="text" class="gd_txt gd_time" value="0" /></dd>
        </dl>
        <%} %>
            
        <div class="clear"></div>
        <div class="zc_btn fl_r margin_top12"><input type="button" style="margin-left:28px" id="createurl" class="psw_btn" value="生成推广链接" /><%=ViewData["CreateMax"] %></div>
    </div>
</form>
<script type="text/javascript">
    $("#createurl").click(function () {
        var form_data = $("#form_url").serialize();
        $.ajax({
            async: false, cache: false, type: "POST", timeout: _global_ajax_timeout, url: "/UI2/Member?method=accountAuto", data: form_data,
            success: function (a, b) {
                _check_auth(a);
                var _robj = eval(a);
                if (0 == _robj.Code) {
                    $("#error_message").html(_robj.Message); ui_mask_panel_close();
                }
                else if (1 == _robj.Code) {
                    refresh_current_page();
                }
            },
            complete: function () {
            }
        });
    });
</script>
        <div class="clear"></div>
        <div class="bank_list_box">
            <div class="bank_list">
                <table width="100%" class="ctable_box youxi_table_list" border="0" cellspacing="0" cellpadding="0">
                    <tbody><tr>
                        <th>推广地址</th>
                        <th>返点信息</th>
                        <th>已注册人数</th>
                        <th>允许开放下级</th>
                        <th>创建日期</th>
                        <th>操作</th>
                    </tr>
                     <%if( null != autoRegList)
                      {
                          foreach(var item in autoRegList)
                          {
                              var url = Request.Url.Host + (Request.Url.Port == 80 ? ":8081" : ":" + Request.Url.Port.ToString());
                              url = (string)ViewData["AutoRegURL"];
                     %>
                    <tr>
                    <td>
                        <a href="<%:"http://"+ url +"/Register.html?code="+item.ar002 %>"><%:"http://"+ url +"/Register.html?code="+item.ar002 %></a></td>
                    <%--<td><a class="ajax" href="#inline_content">查看</a></td>--%>
                    <td>
                        <table class="table-pro">
                            <%
                          var pointInfo = item.ar003.Split(',');
                          foreach(var pi in pointInfo)
                          { 
                              var p = pi.Split('|');
                            %>
                            <tr>
                                <td style="border:none"><%:gcDicList[int.Parse(p[0])].gc003 %></td>
                                <td style="border:none">抽取返点<%:p[1] %></td>
                            </tr>
                            <%} %>
                        </table>
                    </td>
                    <td><%:item.ar008 %></td>
                    <td><%=item.ar009 == 0 ? "<span class='fc-red'>不允许</span>" : "<span class='fc-green'>允许</span>" %></td>
                    <td><%:item.ar004 %></td>
                    <td><a href="/UI2/Member?method=accountAuto&key=<%:item.ar001 %>">删除</a></td>
                    </tr>
                  <%}/*for*/
              }/*foreach*/ %>                 
                </tbody></table>
            </div>
            <!--分页-->
        </div>
    </div>
</div>
<%} /*accountAuto*/%>
<%else if ("createAccount" == methodType)
    { %>
<%
        var myPDList = (List<DBModel.wgs017>)ViewData["MyPDList"];
        var gcList = (List<DBModel.wgs006>)ViewData["GCList"];
        var gcDicList = gcList.ToDictionary(exp => exp.gc001);
%>
<form action="#" method="post" id="form_register">
<%:Html.AntiForgeryToken() %>
<div class="user_info_data_box">
<div class="user_add_teams">
    <div class="user_add_team">
        <dl>
            <dt></dt>
            <dd id="error_message"></dd>
        </dl>
        <dl>
            <dt>账号：</dt>
            <dd><input class="psw_txt" type="text" name="username"> <label>不少于四个字符</label></dd>
        </dl>
        <dl>
            <dt>昵称：</dt>
            <dd><input class="psw_txt" type="text" name="nickname"> <label>不少于四个字符</label></dd>
        </dl>
        <dl>
            <dt>密码：</dt>
            <dd><input class="psw_txt" type="text" name="password"> <label>密码不少于6个字符</label></dd>
        </dl>
        <dl>
            <dt>允许开放下级：</dt>
            <dd style="padding-top:10px">
                <select name="allowcreateaccount" class="select_2015">
                        <option value="0" title="不允许">不允许</option>
                        <option value="1" title="允许" selected="selected">允许</option>
                    </select>
            </dd>
        </dl>
    </div>
    <div class="clear"></div>
    <div class="user_fandian margin_top12">
        <ul>
            <li class="fd_01"><font>返点：</font></li>
            <li class="fd_02">游戏分类</li>
            <li class="fd_03">我的返点</li>
            <li class="fd_04">注册返点</li>
        </ul>
        <%foreach(var mygp in myPDList){ %>
        <dl>
            <dd class="fd_01"><font></font></dd>
            <dd class="fd_02"><a href="#"><%:gcDicList[mygp.gc001].gc003 %></a></dd>
            <dd class="fd_03"><a href="#"><input type="hidden" name="pointid" value="<%:mygp.up001 %>" />我的返点<span class="fc-red"><%:mygp.up003.ToString("N1") %></span></a></dd>
            <dd class="fd_04">注册返点<input name="point" type="text" class="gd_txt gd_time" value="<%:(mygp.up003-0.1m).ToString("N1") %>" /></dd>
        </dl>
        <%} %>
        <div class="clear"></div>
        <div class="zc_btn fl_r margin_top12"><input id="btn_register" type="button" class="psw_btn ui-post-loading" value="注册" /></div>
        
    </div>
</div>
</div>
</form>
<script type="text/javascript">
    $(document).ready(function () {
        $("#btn_register").click(function () {
            var form_data = $("#form_register").serialize();
            $.ajax({
                async: false, cache: false, type: "POST", timeout: _global_ajax_timeout, url: "/UI2/Member?method=createAccount", data: form_data,
                success: function (a, b) {
                    _check_auth(a);
                    var _robj = eval(a);
                    if (0 == _robj.Code) {
                        $("#error_message").html(_robj.Message);
                    }
                    else if (1 == _robj.Code) {
                        $("#error_message").removeClass("fc-red");
                        $("#error_message").html("<p>账号：" + $("input[name='username']").val() + "</p><p>密码：" + $("input[name='password']").val() + "</p>");
                        $("input[name='username'],input[name='password'],input[name='nickname']").val("");
                    }
                },
                complete: function () {
                    ui_mask_panel_close();
                }
            });
        });
    });
</script>
<%
}/*accountAuto*/
    %>
            </div>
        </div>
    </div>
</div>


</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
