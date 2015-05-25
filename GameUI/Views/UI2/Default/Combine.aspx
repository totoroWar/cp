<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI2/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="/Scripts/jquery/jquery.countdown.min.js"></script>
<%
    var methodType = (string)ViewData["MethodType"];
%>
<%
    var gcList = (List<DBModel.wgs006>)ViewData["GameClassList"];
    var gList = (List<DBModel.wgs001>)ViewData["GameList"];
    var gmList = (List<DBModel.wgs002>)ViewData["GameMethodList"];
    var gDicList = gList.ToDictionary(key => key.g001);
    var orderGame = (int)ViewData["OrderGame"];
    var combuyList = (List<DBModel.wgs031>)ViewData["CombuyList"];
    var status = (int)ViewData["Status"];  
%>
<div class="m_body_bg">
<div class="main_box main_tbg">
	<div class="main_table_bg">
    	<div class="main_table_box">
        	<div class="user_info_box">
                <div class="user_add_bank">
                    <form action="/Combine.html?method=orderCombine" id="form_order_combine" method="get">
                        <input type="hidden" name="orderGameClass" value="<%:ViewData["orderGameClass"] %>" />
                        <input type="hidden" name="method" value="<%:methodType %>" />
                    	<div class="search_game_recode">
                        	<div class="gd_search_input">
                            	<ul>
                                	<li><span class="select_span">游戏：</span>
                                        <div class="divselect" id="divselect01">
                                          <select name="orderGame" class="drop-select-to-url select_2015">
                                                <option value="0" title="所有" tourl="/Combine.html?method=orderCombine&orderGameClass=0&orderGame=0">所有</option>
                                                <%foreach (var gc in gcList)
                                                  {
                                                      if (0 == gc.gc006)
                                                      {
                                                          continue;
                                                      }
                                                      var haveGame = gc.gc004.Split(',');
                                                      foreach (var gameItem in haveGame)
                                                      { 
                                                %>
                                                <option value="<%:gameItem %>" <%:orderGame==int.Parse(gameItem) ? "selected='selected'" : "" %> tourl="/Combine.html?method=orderCombine&orderGameClass=<%:gc.gc001 %>&orderGame=<%:gameItem %>"><%:gDicList[int.Parse(gameItem)].g003.Trim() %></option>
                                                <%
                                                      }/*game*/
                                                  }
                                                %>
                                            </select>
                                        </div>
                                    </li>
                                    
                                    <li>发起账号：<input type="text" class="gd_txt" name="account" value="<%:ViewData["Account"] %>" /></li>
                                    <li>期号<input type="text" class="gd_txt" name="issue" value="<%:ViewData["Issue"] %>" /></li>
                                   <li>状态
                                    <select name="status" class="select_2015">
                                        <option value="-1">所有</option>
                                        <option value="0" <%=status == 0 ? "selected='selected'" : "" %>>进行中</option>
                                        <option value="1" <%=status == 1 ? "selected='selected'" : "" %>>已满人</option>
                                        <option value="2" <%=status == 2 ? "selected='selected'" : "" %>>已撤单</option>
                                        <option value="3" <%=status == 3 ? "selected='selected'" : "" %>>已结算</option>
                                    </select></li>
                                    <li><input type="submit" value="查找" class="gd_sbtn ui-post-loading" /></li>                  
                                </ul>
                            </div>
                            <div class="clear"></div>
                        </div>
                    </form>
                        <div class="clear"></div>
                        <div class="bank_list_box">
                        	<div class="bank_list">
                                <table class="ctable_box youxi_table_list" border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <thead>
                                        <tr class="table-pro-head">
                                            <th>单号</th>
                                            <th>发起者</th>
                                            <th>游戏</th>
                                            <th>期号</th>
                                            <th class="dom-hide">结果</th>
                                            <th>内容</th>
                                            <th>总金额</th>
                                            <th>每份</th>
                                            <th>发起人份数</th>
                                            <th>剩余份数</th>
                                            <th>参与人数</th>
                                            <th>发起时间</th>
                                            <th>剩余时间</th>
                                            <th>状态</th>
                                        </tr>
                                    </thead>
                                    <tbody> 
              <%if( null !=combuyList)
              {
                  Dictionary<int, string> dicStatus = new Dictionary<int, string>() { { 0, "进行中" }, { 1, "已满人" }, { 2, "已撤单" }, {3,"已结算"} };
                  foreach(var item in combuyList)
                  {
                      var haveTime = DateTime.Now - item.gs004.Value;
                      var timeAllow = (haveTime.Seconds < 0) ? true : false;
                      %>
            
            <tr key="<%:item.sco001 %>">
                <td><a href="javascript:ShowDetail(<%:item.sco001%>);" title="查看详情"><%:item.sco001 %></a></td>
                <td><%:item.u002.Trim() %></td>
                <td><%:gDicList[item.g001].g003 %></td>
                <td><%:item.gs002.Trim() %></td>
                <td style="display:none"><%:item.gs007 == null ? string.Empty : item.gs007.Trim() %></td>
                <td title="<%:item.sco013.Trim() %>"><a href="javascript:ShowDetail(<%:item.sco001 %>);"><%:NETCommon.StringHelper.GetShortString(item.sco013.Trim(), 20,20, "...") %></a></td>
                <td class="fc-red"><%:item.sco007.ToString("N2")%></td>
                <td class=""><%:(item.sco007 / 100).ToString("N2")%></td>
                <td class="allow_times"><%:(int)item.sco004 %>%</td>
                <td class="fc-green"><%:item.sco011 %>%</td>
                <td><%:item.sco012 %></td>
                <td><%:item.sco017 %></td>
                <td><div class="fc-green fs14px" data-countdown="<%:item.gs004.Value.ToString("yyyy/MM/dd HH:mm:ss") %>"></div></td>
                <td><%=dicStatus[item.sco009] %><%if (item.sco009 == 0 && timeAllow == true)
                                         { %><span name="join" class="btn_w60h25" data="<%:item.sco001 %>">参与<%:item.sco010 == null ? string.Empty : "[密]" %><%}/*if*/ %></span></td>
                <td class="link-tools dom-hide">
                    <code style="display:none;">
                        <%System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                          item.sco010 = string.Empty;%>
                        <%=jss.Serialize(item) %>
                    </code>
                </td>
            </tr>
            <%}
              } %>
                                </tbody></table>

                            </div>
                            <div class="wp_page fl_r">
                <%=ViewData["PageList"] %>
            </div>
                        </div>
                    </div>
            </div>
        </div>
    </div>
</div>


</div>
    <style>
        #Detail,#e_join{display:none}
    </style>
    <div id="e_join">
        <form id="form_join" action="#">
        <input type="hidden" id="h_mode" value="0" />
        <input type="hidden" id="h_time" value="0" />
        <input type="hidden" id="h_ori_money" />
        <input type="hidden" name="hash_key" id="hash_key" value="" />
        <input type="hidden" name="method" value="joinCombine" />
        <table class="table-pro w100ps table-noborder">
            <tr>
                <td class="title">游戏/玩法</td>
                <td id="e_game_name"></td>
            </tr>
            <tr>
                <td class="title">期号/结果</td>
                <td id="e_issue"></td>
            </tr>
            <tr>
                <td class="title">内容</td>
                <td id="e_game_content"></td>
            </tr>
            <tr>
                <td class="title">发起者</td>
                <td id="e_user"></td>
            </tr>
            <tr class="dom-hide">
                <td class="title">发起时间</td>
                <td id="e_posttime"></td>
            </tr>
            <tr>
                <td class="title">总金额/模式</td>
                <td><span id="e_total"></span>/<span id="e_mode"></span></td>
            </tr>
            <tr>
                <td class="title">参与金额/人数</td>
                <td><span id="e_join_total"></span>/<span id="e_join_nums"></span></td>
            </tr>
            <tr>
                <td class="title">剩余份数/时间</td>
                <td class="fc-green"><span id="e_can_order"></span>/<span id="e_countdown"></span></td>
            </tr>
            <tr>
                <td class="title">注单倍数</td>
                <td id="e_order_time"></td>
            </tr>
            <tr>
                <td class="title">每份</td>
                <td id="e_per_sum" class="fc-red">0</td>
            </tr>
            <tr>
                <td class="title">购买金额</td>
                <td id="e_order_sum" class="fc-red">0</td>
            </tr>
            <tr>
                <td class="title">购买份数</td>
                <td><input type="button" value="-" class="btn-normal btn_num_subtract btn_fun" /><input type="text" name="bet_times" id="bet_times" class="input-text w50px text-cent" value="0" /><input type="button" value="+" class="btn-normal btn_num_add btn_fun" />%<span class="is_password">&nbsp;密码<input type="text" name="bet_password" id="bet_password" class="input-text w100px" value="" /></span></td>
            </tr>
            <tr>
                <td class="title"></td>
                <td><input type="button" class="btn_w60h25" id="btn_join" value="确认" /></td>
            </tr>
        </table>
        </form>
        </div>
        <div id="Detail">
         <table class="table-pro w100ps table-noborder">
             <tr><td>单号</td><td></td></tr>
              <tr><td>发起者</td><td></td></tr>
              <tr><td>游戏</td><td></td></tr>
              <tr><td>期号</td><td></td></tr>
              <tr><td>内容</td><td></td></tr>
              <tr><td>总金额</td><td></td></tr>
              <tr><td>每份</td><td></td></tr>
              <tr><td>发起人份数</td><td></td></tr>
              <tr><td>剩余份数</td><td></td></tr>
              <tr><td>参与人数</td><td></td></tr>
              <tr><td>发起时间</td><td></td></tr>
              <tr><td>剩余时间</td><td></td></tr>
              <tr class="dom-hide"><td>奖金反点</td><td></td></tr>
             <tr><td>模式</td><td></td></tr>
             <tr><td>倍数</td><td></td></tr>
         </table>
    </div>
    <script type="text/javascript">
        function combine_join(key)
        {
            $(".is_password").hide();

            $("#hash_key").val("");
            $("#bet_password").val("");

            $.ajax({
                timeout: _global_ajax_timeout, cache: false, type: "POST", data: { method: "getCombine", key: key }, url: "/UI2/Combine", success: function (a) {
                    _check_auth(a);
                    var _robj = eval(a);
                    if (0 == _robj.Code)
                    {
                        alert(_robj.Message);
                    }
                    else if (1 == _robj.Code)
                    {
                        //_global_ui("e_join", 0, 33, 10, 0, "参与合买");
                        $("#e_join").dialog({ width: 600, title: "参与合买", modal: true, resizable: false, position: { my: "center", at: "center", of: window } });
                        $("#e_game_name").html(_robj.Data.GameName + "/" + _robj.Data.GameMethod);
                        $("#e_issue").html(_robj.Data.Issue + "/" + (_robj.Data.GameResult == "" ? "未开奖" : _robj.Data.GameResult));
                        $('#e_countdown').countdown(_robj.Data.CloseTime, function (event)
                        {
                            $(this).html(event.strftime('%H:%M:%S'));
                        });
                        $("#e_posttime").html(_robj.Data.PostTime);
                        $("#e_total").html(_robj.Data.TotalMoney);
                        $("#h_ori_money").val(_robj.Data.OriTotalMoney);
                        $("#e_per_sum").html((_robj.Data.OriTotalMoney/100.0000).toFixed(2));
                        $("#e_can_order").html(_robj.Data.CanBetNums+"%");
                        $("#e_order_time").html(_robj.Data.OrderTimes);
                        $("#e_game_content").html(_robj.Data.Code);
                        $("#e_join_total").html(_robj.Data.JoinMoney);
                        $("#e_join_nums").html(_robj.Data.JoinNums);
                        $("#e_mode").html(_robj.Data.ModeName);
                        $("#e_user").html(_robj.Data.User);
                        $("#bet_times").val("0");
                        $("#h_mode").val(_robj.Data.Mode);
                        $("#h_time").val(_robj.Data.OrderTimes);
                        $("#hash_key").val(_robj.Data.HashKey);
                        if (1 == _robj.Data.IsPwd)
                        {
                            $(".is_password").show();
                        }
                    }
                }
            })
        }
        //$(document).ready(function () {
        //    $('[data-countdown]').each(function () {
        //        var $this = $(this), finalDate = $(this).data('countdown');
        //        $this.countdown(finalDate, function (event) {
        //            $this.html(event.strftime('%H:%M:%S'));
        //        });
        //    });

            $("#Detail tr td:even").addClass("title");
            $("#btn_join").click(function ()
            {
                $("#e_join").block({message:null});
                var form_data = $("#form_join").serialize();
                $.ajax({
                    timeout: _global_ajax_timeout, cache: false, type: "POST", data:form_data, url: "/UI/Combine", success: function (a) {
                        _check_auth(a);
                        $('#e_join').unblock();
                        var _robj = eval(a);
                        if (0 == _robj.Code) {
                            alert(_robj.Message);
                        }
                        else
                        {
                            refresh_current_page();
                        }
                    }
                });
            });
            $(".btn_fun").click(function ()
            {
                var num = $("#bet_times").val();
                var pattern = /^[0-9]+$/;
                if (false == pattern.test(num.toString()))
                {
                    num = 0;
                }

                if ($(this).hasClass("btn_num_subtract"))
                {
                    $("#bet_times").val(parseInt(num) - 1);
                }
                else
                {
                    $("#bet_times").val(parseInt(num) + 1);
                }
                init_sum();
            });

            $("#bet_times").keyup(function ()
            {
                try
                {
                    init_sum();
                }
                catch(e){}
            });

            function init_sum()
            {
                var nums = $("#bet_times").val();
                var mode = $("#h_mode").val();
                var times = $("#h_time").val();
                var totalMoney = window.parseFloat($("#h_ori_money").val());

                var orderSumAmount = 0.0000;
                orderSumAmount = parseFloat((totalMoney / 100) * nums);
                $("#e_order_sum").html(parseFloat(orderSumAmount).toFixed(2));
            }

            $("span[name='join']").click(function ()
            {
                var key = $(this).attr("data");
                combine_join(key);
            });
            
            $("#btn_cancel_order").click(function () {
                cancel_order();
            });
            $("#Detail").dialog({
                title:"详细信息",
                width:550,
                height:450,
                autoOpen:false,
                modal:true
        });
        });
        function ShowDetail(combuyListId)
        {
                    eval("var dataDetail="+$("tr[key="+combuyListId+"] code").html());
                    $("#Detail table tr:eq(0) td:eq(1)").html($("tr[key="+combuyListId+"]>td:eq(0)>a").html());
                    $("#Detail table tr:eq(1) td:eq(1)").html($("tr[key="+combuyListId+"]>td:eq(1)").html());
                    $("#Detail table tr:eq(2) td:eq(1)").html($("tr[key="+combuyListId+"]>td:eq(2)").html());
                    $("#Detail table tr:eq(3) td:eq(1)").html($("tr[key="+combuyListId+"]>td:eq(3)").html());
                    $("#Detail table tr:eq(4) td:eq(1)").html($("tr[key="+combuyListId+"]>td:eq(5)").attr("title"));
                    $("#Detail table tr:eq(5) td:eq(1)").html($("tr[key="+combuyListId+"]>td:eq(6)").html());
                    $("#Detail table tr:eq(6) td:eq(1)").html($("tr[key="+combuyListId+"]>td:eq(7)").html());
                    $("#Detail table tr:eq(7) td:eq(1)").html($("tr[key="+combuyListId+"]>td:eq(8)").html());
                    $("#Detail table tr:eq(8) td:eq(1)").html($("tr[key="+combuyListId+"]>td:eq(9)").html());
                    $("#Detail table tr:eq(9) td:eq(1)").html($("tr[key="+combuyListId+"]>td:eq(10)").html());
                    $("#Detail table tr:eq(10) td:eq(1)").html($("tr[key="+combuyListId+"]>td:eq(11)").html());
                    $("#Detail table tr:eq(11) td:eq(1)").html($("tr[key="+combuyListId+"]>td:eq(12)").html());
            //$("#Detail table tr:eq(12) td:eq(1)").html(dataDetail.sco015 == null ? "动态奖金，开后后决定" : ("奖金：" + dataDetail.sco015.split('|')[0] + "，返点：" + dataDetail.sco015.split('|')[1]));
                    $("#Detail table tr:eq(13) td:eq(1)").html(dataDetail.sco016 == 1 ? "元" : dataDetail.sco016 == 2 ? "角" : "分");
                    $("#Detail table tr:eq(14) td:eq(1)").html(dataDetail.sco019);
                    $("#Detail").dialog("open");
                
        }
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
