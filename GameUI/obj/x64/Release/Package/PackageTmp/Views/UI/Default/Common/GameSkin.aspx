<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%
    var curGSItem = (DBModel.wgs005)ViewData["CurGSItem"];
    var lastGSItem = (DBModel.wgs005)ViewData["LastGSItem"];
    var historyOrderList = (List<DBModel.LotteryHistoryOrder>)ViewData["HistoryOrderList"];
    var currentGSDayCount = (int)ViewData["GSDayCount"];
    var UITheme = (string)ViewData["UITheme"];
    var setInfo = (string)ViewData["GSetInfo"];
    var setInfoSplit = setInfo.Split(':');
    var resultBit = setInfoSplit[3].Split('-');
%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title><%:ViewData["GlobalTitle"] %>-<%:ViewData["Title"] %></title>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
<meta http-equiv="Pragma" content="no-cache" />
<link href="/StaticGame/Theme/SSC_<%=UITheme%>/CSS/play.css" rel="stylesheet" type="text/css" />
<link href="/CSS/<%=UITheme%>/UI/UI.css" rel="stylesheet" type="text/css" />
<link rel="stylesheet" type="text/css" href="/CSS/<%=UITheme%>/UI/jqueryeasyui/default/easyui.css" />
<script type="text/javascript">var pri_imgserver = '/StaticGame/Theme/SSC_<%=UITheme%>';</script>
<script type="text/javascript" src="/StaticGame/Theme/SSC_<%=UITheme%>/Scripts/jquery.js"></script>
<script type="text/javascript" src="/StaticGame/Theme/SSC_<%=UITheme%>/Scripts/gamecommon.js"></script>
<script type="text/javascript" src="/StaticGame/Theme/SSC_<%=UITheme%>/Scripts/jquery.dialogUI.js"></script>
<script type="text/javascript" src="/StaticGame/Theme/SSC_<%=UITheme%>/Scripts/lang_zh.js"></script>
<script type="text/javascript" src="/StaticGame/Theme/SSC_<%=UITheme%>/Scripts/face.<%=setInfoSplit[1] %>.js"></script>
<script type="text/javascript" src="/StaticGame/Theme/SSC_<%=UITheme%>/Scripts/methods.<%=setInfoSplit[1] %>.js"></script>
<script type="text/javascript" src="/StaticGame/Theme/SSC_<%=UITheme%>/Scripts/jquery.game.<%=setInfoSplit[2] %>.js"></script>
<script type="text/javascript" src="/StaticGame/Theme/SSC_<%=UITheme%>/Scripts/jquery.messager-min.js"></script>
<script type="text/javascript" src="/StaticGame/Theme/SSC_<%=UITheme%>/Scripts/message.js"></script>
<script type="text/javascript" src="/Scripts/jquery-easyui/jquery.easyui.min.js"></script>
<script type="text/javascript" src="/Scripts/UI/<%=UITheme%>/ALLHandle.js"></script>
<script type="text/javascript">
    function ResumeError() { return true; } window.onerror = ResumeError;
    document.onselectstart = function (event)
    {
        if (window.event)
        {
            event = window.event;
        }
        try
        {
            var the = event.srcElement;
            if (!((the.tagName == "INPUT" && the.type.toLowerCase() == "text") || the.tagName == "TEXTAREA"))
            {
                return false;
            }
            return true;
        } 
        catch (e)
        {
            return false;
        }
    }
</script>
</head>
<body>
	<div id="rightcon">
		<div id="msgbox" class="win_bot" style="display: none;">
			<h5 id="msgtitle"></h5>
			<div class="wb_close" onclick="javascript:msgclose();"></div>
			<div class="clear"></div>
			<div class="wb_con">
				<p id="msgcontent"></p>
			</div>
			<div class="clear"></div>
			<a class="wb_p" href="#" onclick="javascript:prenotice();" id="msgpre">上一条</a><a class="wb_n" href="#" onclick="javascript:nextnotice();">下一条</a>
		</div>
		<div class=" game_rc">
			<form action="/UI/PlayInfo.cgi">
				<!--奖期基本信息开始-->
				<div class="gm_con">
					<div class="gm_con_lt"></div>
					<div class="gm_con_rt"></div>
					<div class="gm_con_lb"></div>
					<div class="gm_con_rb"></div>
					<div class="gm_con_to">
						<div class="gct_l">
							<h3><%:ViewData["GameName"] %></h3>
							<p class="gct_now">
								正在销售 <strong>第<span id="current_issue"><%:curGSItem.gs002.Trim() %></span>期
                                    <%if( 0 < currentGSDayCount) {%>
								</strong> 总共: <strong><span id="current_sale"><%:currentGSDayCount %></span></strong>期
                                <%} %>
							</p>
							<div class="clear"></div>
							<div class="gct_time">
								<p class="gct_now">
									本期销售截止时间&nbsp;<span class="nbox" id="current_endtime"><%:curGSItem.gs004.ToString("yyyy-MM-dd HH:mm:ss") %></span>
								</p>
								<div class="clear"></div>
								<p class="gct_now gct_now1">剩余</p>
								<div class="gct_time_now">
									<div class="gct_time_now_l">
										<span id="count_down">00:00:00</span>
									</div>
								</div>
							</div>
							<div class="gct_menu">
								<a class="gct_menu_yl" href='javascript:window.parent.parent.ui_show_tab("<%:ViewData["GameName"] %>遗漏", "/GameResult.html?gameID=<%=ViewData["GameID"] %>&gameClassID=<%:ViewData["GameClassID"] %>", true);'></a>
							</div>
						</div>
						<div class="gct_r">
							<h3>
								<%:ViewData["Title"] %>&nbsp;第<b><span class="nn" id="lt_gethistorycode"><%:lastGSItem.gs002.Trim() %></span></b>期 
                                <span id="lt_opentimebox" style="display: none;">&nbsp;&nbsp;
                                <span id="waitopendesc">等待开奖</span>&nbsp;<span style="color: #F9CE46;" id="lt_opentimeleft"></span></span>
                                <span id="lt_opentimebox2" style="display: none; color: #F9CE46;"><strong>&nbsp;&nbsp;正在开奖</strong></span>
                                <span class="history_result" id="history_result">[近期开奖]</span>
							</h3>
							<div style="display: none;" class="tad" id="showadvbox">
								<a href="javascript:window.parent.parent.ui_show_tab('活动','/Promotion.html',true);"><img alt="活动" src='/Images/Common/<%:ViewData["GSWaitAD"] %>' border="0" /></a>
							</div>
                            <%if(string.IsNullOrEmpty(lastGSItem.gs007) )
                              {
                                  %>
                            <div class="gct_r_nub" id="showcodebox">
                                <%foreach(var rb in resultBit){ %>
                                <div class="gr_s <%=rb == "n" ? "gr_sx" : "" %>" flag="<%=rb == "n" ? "normal" : "move" %>"></div>
                                <%} %>
                            </div>
                            <%}else
                              { %>
                            <%
                                var result = lastGSItem.gs007.Trim().Split(',');
                                var resultIndex = 0;
                            %>
							<div class="gct_r_nub" id="showcodebox">
                                <%foreach (var rb in resultBit)
                                  { %>
                                <%if( rb == "n"){ %>
								<div class="gr_s <%=rb == "n" ? "gr_sx" : "" %>" flag="normal"></div>
                                <%}/*if n*/else { %>
                                <div class="gr_s gr_s<%:result[resultIndex] %>" flag="move"></div>
                                <%resultIndex++;
                                  } /*else*/%>
                                <%} %>
							</div>
                            <%}/*result */ %>
						</div>
						<div class="clear"></div>
					</div>
				</div>
				<!--奖期基本信息结束-->
				<div class="gm_con">
					<div class="gm_con_lt"></div>
					<div class="gm_con_rt"></div>
					<div class="gm_con_lb"></div>
					<div class="gm_con_rb"></div>
					<div class="gm_con_to">
						<!--投注选号标签开始-->
						<div class="tz_body">
							<div class="unit">
								<div class="unit_title">
									<div class="ut_l"></div>
									<div class="u_tab_div" id="tabbar-div-s2"></div>
									<div class="ut_r"></div>
								</div>
								<div id="tabCon">
									<div class="tabcon_n">
										<div class="nl_lt"></div>
										<div class="nl_rt"></div>
										<div class="nl_lb"></div>
										<div class="nl_rb"></div>
										<ul id="tabbar-div-s3">
										</ul>
									</div>
								</div>
							</div>
							<div class="clear"></div>
						</div>
						<!--投注选标签开始-->
						<div class="clear"></div>
						<!--投注选号区开始-->
						<div class="tzn_body">
							<div class="tzn_b_n">
								<div class="tbn_lt"></div>
								<div class="tbn_lb"></div>
								<div class="tbn_rt"></div>
								<div class="tbn_rb"></div>
								<div class="tbn_top">
									<h5 id="lt_desc"></h5>
									<span class="methodexample" id="lt_example"></span> <span
										class="methodhelp" id="lt_help"></span>
									<div class="clear"></div>
								</div>
								<div class="clear"></div>
								<div class="tbn_cen">
									<div class="tbn_c_ft"></div>
									<div class="tbn_c_s">
										<div id="lt_selector"></div>
										<div class="random_sel_button" id="random_sel_button"></div>
										<div class="clear"></div>
									</div>
									<div class="tbn_c_fb"></div>
								</div>
								<div class="tbn_bot">
									<div class="tbn_b_top">
										<div class="tbn_bt_sel">
											您选择了 <strong><span class=n id="lt_sel_nums">0</span></strong>
											注, 共 <strong><span class=n id="lt_sel_money">0</span></strong>
											元, 倍数: <span class="changetime" id="reducetime" title="减少1倍">－</span>
											<input name='lt_sel_times' type='text' size="4" class='bei' id="lt_sel_times" /> <span class="changetime" id="plustime" title="增加1倍">＋</span> 倍 <select name="lt_sel_modes" id="lt_sel_modes">
												<option>元模式</option>
												<option>角模式</option>
											</select> <span id="lt_sel_prize"></span>
										</div>
										<div class="g_submit" id="lt_sel_insert">
											<span></span>
										</div>
										<div class="clear"></div>
									</div>
                                    <%--<div id="__show_choose_prize">
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td class="__td_first_blank"></td>
                                                <td class="__td_silder_ctrl"><span id="__prize_silder_bar"></span></td>
                                                <td class="__td_silder_value"><span id="__prize_change_info">0/0%</span></td>
                                            </tr>
                                        </table>
                                    </div>--%>
                                    <div id="function_bar">
                                        <div id="function_bar_combuy">
                                            <label for="lt_combuy_check">合买<input id="lt_combuy_check" name="lt_combuy_check" type="checkbox" value="1" class="ut_zh input" /></label>注金比例：<select name="lt_self_percent" id="lt_self_percent"></select>&nbsp;参与密码：<input title="参与者需要输入密码方可进行合买，如果公开请留空" type="text" name="lt_combuy_password" id="lt_combuy_password" />
                                        </div>
                                        <div id="function_bar_right">
                                            <div id="prize_silder_bar_c"><span id="prize_silder_bar"></span></div>
                                            <div id="prize_change_info">0/0%</div>
                                        </div>
                                    </div>
									<div class="tbn_b_bot">
										<div class="tbn_bb_l">
											<div class="tbn_bb_ln">
												<h4>
													<input class="tbn_clear" id="lt_cf_clear" name=""
														type="button" value="" /> <span class="icons_q1"
														id="lt_cf_help">&nbsp;&nbsp;&nbsp;</span> 投注项: <span
														id="lt_cf_count">0</span>
												</h4>
												<div class="tz_tab_list_box">
													<table cellspacing=0 cellpadding=0 border=0
														id="lt_cf_content" class="tz_tab_list">
														<tr class='nr'>
															<td class="tl_li_l" width="4"></td>
															<td colspan="6" class="noinfo">暂无投注项</td>
															<td class="tl_li_rn" width="4"></td>
														</tr>
													</table>
												</div>
											</div>
										</div>
										<div class="tbn_bb_r">
											<div class="sub_txt">
												<p>
													总注数: <strong><span class='r' id="lt_cf_nums">0</span></strong>注
												</p>
												<p>
													总金额: <strong><span class='r' id="lt_cf_money">0</span></strong>元
												</p>
												<p>
													未来期: <span id="lt_issues"></span>
												</p>
											</div>
											<div class="g_submit" id="lt_buy">
												<span></span>
											</div>
										</div>
										<div class="clear"></div>
									</div>
								</div>
							</div>
						</div>
						<!--投注选号区结束-->
						<div class="clear"></div>
						<!--追号区开始-->
						<div class="zh_body">
							<div class="unit">
								<div class="unit_title">
									<div class="ut_l"></div>
									<h4>
										<label class="zh_title" name="lt_trace_if"> <input type="checkbox" name="lt_trace_if" id="lt_trace_if" value="yes" /> 我要追号</label>
									</h4>
									<div class="ut_zh">
										<label class="zh_continue" name="lt_trace_stop"> <input type="checkbox" name="lt_trace_stop" id="lt_trace_stop" disabled="disabled" value="yes" /> 中奖后停止追号
										</label>
									</div>
									<div class="ut_r"></div>
								</div>
								<div id="lt_trace_box" style='display: none' class="trace_box">
									<div class="tabcon_n">
										<div class="nl_lt"></div>
										<div class="nl_rt"></div>
										<div class="nl_lb"></div>
										<div class="nl_rb"></div>
										<div class="unit1">
											<div class="unit_title2">
												<div class="u_tab_div" id="tab02">
													<div class="bd">
														<div class="bd2" id="general_txt_100">
															<table class="tabbar-div-s3" width='100%'>
																<tr>
																	<td id="lt_trace_label"></td>
																</tr>
															</table>
															<div class="bl3p"></div>
														</div>
													</div>
												</div>
												<div class="clear"></div>
											</div>
											<div class="clear"></div>
											<div class="zhgen">
												<table width="100%" border="0" cellspacing="0" cellpadding="0">
													<tr>
														<td>追号期数: <select id="lt_trace_qissueno">
																<option value="">请选择</option>
																<option value="5">5期</option>
																<option value="10" selected="selected">10期</option>
																<option value="15">15期</option>
																<option value="20">20期</option>
																<option value="25">25期</option>
																<option value="all">全部</option>
														</select> 总期数: <span class="y" id="lt_trace_count">0</span> 期 追号总金额:
															<span class="y" id="lt_trace_hmoney">0</span> 元 <br />
															追号计划: <span id="lt_trace_labelhtml"></span></td>
														<td rowspan="2" valign="bottom" align="right"><div class="g_submit" id="lt_trace_ok"><span></span></div></td>
													</tr>
												</table>
											</div>
											<div class="zhlist" id="lt_trace_issues"></div>
											<input type="hidden" name="lotteryid" id="lotteryid" value="<%:ViewData["GameID"] %>" />
                                            <input type="hidden" name="flag" id="flag" value="save" />
                                            <input type="hidden" name="method" id="method" value="save" />
										</div>
									</div>
								</div>
							</div>
						</div>
						<!--追号区结束-->
					</div>
				</div>
				<div class="clear"></div>
				<div class="gm_con">
					<div class="gm_con_lt"></div>
					<div class="gm_con_rt"></div>
					<div class="gm_con_lb"></div>
					<div class="gm_con_rb"></div>
					<div class="gm_con_to">
						<div class="yx_body">
							<div class="unit">
								<div class="unit_title">
									<div class="ut_l"></div>
									<h4>
										<label class="yx_title">游戏记录</label>
									</h4>
									<div class="ut_r"></div>
								</div>
							</div>
							<div class="yx_box">
								<div class="nl_lt"></div>
								<div class="nl_rt"></div>
								<div class="nl_lb"></div>
								<div class="nl_rb"></div>
								<div class="yxlist">
									<div class="nl_lt"></div>
									<div class="nl_rt"></div>
									<div class="nl_lb"></div>
									<div class="nl_rb"></div>
									<table width="100%" border="0" cellspacing="0" cellpadding="0">
										<tr>
											<th>注单编号</th>
											<th>投注时间</th>
											<th>玩法</th>
											<th>期号</th>
											<th>投注内容</th>
											<th>倍数</th>
											<th>模式</th>
											<th>总金额</th>
											<th>奖金</th>
											<th>状态</th>
										</tr>
										<tbody class="projectlist">
											<%--<tr class="no-records">
												<td height="37" colspan="10" align="center">没有找到指定条件的投注记录</td>
											</tr>--%>
                                            <%
                                                if (null != historyOrderList)
                                                { 
                                                    foreach(var histItem in historyOrderList)
                                                    {
                                            %>
                                            <tr class="<%:histItem.iscancel > 0 ? "cancel" : "" %>">
                                                <td><a title="查看投注详情" class="blue" href="javascript:" rel="projectinfo"><%:histItem.projectid %></a></td>
                                                <td><%:histItem.writetime %></td>
                                                <td><%:histItem.methodname %></td>
                                                <td><%:histItem.issue %></td>
                                                <td title="<%:histItem.code %>"><%:histItem.codeShort %></td>
                                                <td><%:histItem.multiple %></td>
                                                <td><%:histItem.modes %></td>
                                                <td><%:histItem.totalprice %></td>
                                                <td><%:histItem.bonus %></td>
                                                <td><%:histItem.statusdesc %></td>
                                            </tr>
                                            <%
                                                     }
                                                }/*if*/
                                                else
                                                {  
                                              %>
                                            <tr class="no-records">
												<td height="37" colspan="10" align="center">没有找到指定条件的投注记录</td>
											</tr>
                                            <%
                                            }/*else end*/ 
                                            %>
										</tbody>
									</table>
								</div>
							</div>
						</div>
					</div>
				</div>
				<div id="copyright">
					<p><%:ViewData["GlobalTitle"] %></p>
				</div>
			</form>
		</div>
	</div>
	<script type="text/javascript">
	    $('#history_result').click(function(){
	        $.ajax({dataType: "json", type: "GET", url: "/HistoryResult.html?Type=ajax&GameID=<%:ViewData["GameID"]%>", success: function (a) {
	                //_check_auth(a);
	                //eval("var _robj=" + a + ";");
	                //alert(_robj);
	                var _robj = a;
	                var html = "";
	                if (1 == _robj.Code) 
	                {
	                    var results = _robj.Data.split('_');
	                    for(var i = 0; i < results.length;i++)
	                    {
	                        var row = results[i].split('|');
	                        html += "<p>"+row[0] + "期开奖结果"+row[1]+"</p>";
	                    }
	                }
	                $.alert("<p style='style='text-align:center; font-size:14px; background:#fff;'>"+html+"</p>", "近期开奖","", 250,false);
	            }
	        });
	    });
	    <%if( setInfoSplit[0] == "1") {%>
	    var pri_user_data = [{ methodid: 2, prize: { 1: '18000.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 18000 }, { "point": 0, "prize": 19280 }] }] }, { methodid: 4, prize: { 1: '18000.00', 2: '1800.00', 3: '180.00', 4: '18.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 18000 }, { "point": 0, "prize": 19280 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 18000 }, { "point": 0, "prize": 19280 }, { "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }, { "level": 3, "prize": [{ "point": "0.064", "prize": 18000 }, { "point": 0, "prize": 19280 }, { "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }, { "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }, { "level": 4, "prize": [{ "point": "0.064", "prize": 18000 }, { "point": 0, "prize": 19280 }, { "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }, { "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }, { "point": "0.064", "prize": 18 }, { "point": 0, "prize": 19.28 }] }] }, { methodid: 6, prize: { 1: '750.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 750 }, { "point": 0, "prize": 803.33 }] }] }, { methodid: 7, prize: { 2: '1500.00' }, dyprize: [{ "level": 2, "prize": [{ "point": "0.064", "prize": 1500 }, { "point": 0, "prize": 1606.66 }] }] }, { methodid: 8, prize: { 3: '3000.00' }, dyprize: [{ "level": 3, "prize": [{ "point": "0.064", "prize": 3000 }, { "point": 0, "prize": 3213.33 }] }] }, { methodid: 9, prize: { 4: '4500.00' }, dyprize: [{ "level": 4, "prize": [{ "point": "0.064", "prize": 4500 }, { "point": 0, "prize": 4820 }] }] }, { methodid: 11, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 12, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 13, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 15, prize: { 1: '1800.00', 2: '180.00', 3: '18.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }, { "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }, { "level": 3, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }, { "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }, { "point": "0.064", "prize": 18 }, { "point": 0, "prize": 19.28 }] }] }, { methodid: 17, prize: { 1: '600.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }] }, { methodid: 18, prize: { 2: '300.00' }, dyprize: [{ "level": 2, "prize": [{ "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 19, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 20, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 21, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 23, prize: { 1: '18.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 18 }, { "point": 0, "prize": 19.28 }] }] }, { methodid: 25, prize: { 1: '180.00', 2: '30.00', 3: '6.60' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }, { "point": "0.064", "prize": 30 }, { "point": 0, "prize": 32.13 }] }, { "level": 3, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }, { "point": "0.064", "prize": 30 }, { "point": 0, "prize": 32.13 }, { "point": "0.064", "prize": 6.6 }, { "point": 0, "prize": 7.07 }] }] }, { methodid: 27, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 28, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 29, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 31, prize: { 1: '1800.00', 2: '180.00', 3: '18.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }, { "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }, { "level": 3, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }, { "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }, { "point": "0.064", "prize": 18 }, { "point": 0, "prize": 19.28 }] }] }, { methodid: 33, prize: { 1: '600.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }] }, { methodid: 34, prize: { 2: '300.00' }, dyprize: [{ "level": 2, "prize": [{ "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 35, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 36, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 37, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 39, prize: { 1: '18.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 18 }, { "point": 0, "prize": 19.28 }] }] }, { methodid: 41, prize: { 1: '180.00', 2: '30.00', 3: '6.60' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }, { "point": "0.064", "prize": 30 }, { "point": 0, "prize": 32.13 }] }, { "level": 3, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }, { "point": "0.064", "prize": 30 }, { "point": 0, "prize": 32.13 }, { "point": "0.064", "prize": 6.6 }, { "point": 0, "prize": 7.07 }] }] }, { methodid: 43, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 44, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 45, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 47, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 48, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 49, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 51, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 52, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 53, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 55, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 56, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 57, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 59, prize: { 1: '18.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 18 }, { "point": 0, "prize": 19.28 }] }] }, { methodid: 60, prize: { 1: '18.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 18 }, { "point": 0, "prize": 19.28 }] }] }, { methodid: 61, prize: { 1: '18.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 18 }, { "point": 0, "prize": 19.28 }] }] }, { methodid: 62, prize: { 1: '18.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 18 }, { "point": 0, "prize": 19.28 }] }] }, { methodid: 63, prize: { 1: '18.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 18 }, { "point": 0, "prize": 19.28 }] }] }, { methodid: 65, prize: { 1: '6.60' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 6.6 }, { "point": 0, "prize": 7.07 }] }] }, { methodid: 66, prize: { 1: '6.60' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 6.6 }, { "point": 0, "prize": 7.07 }] }] }, { methodid: 68, prize: { 1: '33.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 33 }, { "point": 0, "prize": 35.37 }] }] }, { methodid: 69, prize: { 1: '33.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 33 }, { "point": 0, "prize": 35.37 }] }] }, { methodid: 71, prize: { 1: '5.20' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 5.2 }, { "point": 0, "prize": 5.57 }] }] }, { methodid: 73, prize: { 1: '18.40' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 18.4 }, { "point": 0, "prize": 19.71 }] }] }, { methodid: 75, prize: { 1: '12.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 12 }, { "point": 0, "prize": 12.87 }] }] }, { methodid: 77, prize: { 1: '41.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 41 }, { "point": 0, "prize": 43.94 }] }] }, { methodid: 79, prize: { 1: '7.20' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 7.2 }, { "point": 0, "prize": 7.71 }] }] }, { methodid: 80, prize: { 1: '7.20' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 7.2 }, { "point": 0, "prize": 7.71 }] }] }, { methodid: 82, prize: { 1: '14.40' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 14.4 }, { "point": 0, "prize": 15.42 }] }] }, { methodid: 83, prize: { 1: '14.40' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 14.4 }, { "point": 0, "prize": 15.42 }] }] }, { methodid: 85, prize: { 1: '4.30' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 4.3 }, { "point": 0, "prize": 4.61 }] }] }, { methodid: 86, prize: { 2: '22.00' }, dyprize: [{ "level": 2, "prize": [{ "point": "0.064", "prize": 22 }, { "point": 0, "prize": 23.57 }] }] }, { methodid: 87, prize: { 3: '210.00' }, dyprize: [{ "level": 3, "prize": [{ "point": "0.064", "prize": 210 }, { "point": 0, "prize": 224.95 }] }] }, { methodid: 88, prize: { 4: '3900.00' }, dyprize: [{ "level": 4, "prize": [{ "point": "0.064", "prize": 3900 }, { "point": 0, "prize": 4178.26 }] }] }, { methodid: 645, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 646, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 647, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 648, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 649, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 650, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 651, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 652, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 653, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 654, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 746, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 747, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 748, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 749, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 750, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 751, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 752, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 753, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 754, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 755, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }] }, { methodid: 655, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 656, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 657, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 658, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 659, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 660, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 661, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 662, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 663, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 664, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 756, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 757, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 758, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 759, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 760, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 761, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 762, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 763, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 764, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 765, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 90 }, { "point": 0, "prize": 96.4 }] }] }, { methodid: 667, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 668, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 669, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 670, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 671, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 672, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 673, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 674, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 675, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 676, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 699, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 700, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 701, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 702, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 703, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 704, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 705, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 706, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 707, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 708, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }] }, { methodid: 677, prize: { 1: '600.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }] }, { methodid: 678, prize: { 1: '600.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }] }, { methodid: 679, prize: { 1: '600.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }] }, { methodid: 680, prize: { 1: '600.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }] }, { methodid: 681, prize: { 1: '600.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }] }, { methodid: 682, prize: { 1: '600.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }] }, { methodid: 683, prize: { 1: '600.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }] }, { methodid: 684, prize: { 1: '600.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }] }, { methodid: 685, prize: { 1: '600.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }] }, { methodid: 686, prize: { 1: '600.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }] }, { methodid: 687, prize: { 2: '300.00' }, dyprize: [{ "level": 2, "prize": [{ "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 688, prize: { 2: '300.00' }, dyprize: [{ "level": 2, "prize": [{ "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 689, prize: { 2: '300.00' }, dyprize: [{ "level": 2, "prize": [{ "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 690, prize: { 2: '300.00' }, dyprize: [{ "level": 2, "prize": [{ "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 691, prize: { 2: '300.00' }, dyprize: [{ "level": 2, "prize": [{ "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 692, prize: { 2: '300.00' }, dyprize: [{ "level": 2, "prize": [{ "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 693, prize: { 2: '300.00' }, dyprize: [{ "level": 2, "prize": [{ "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 694, prize: { 2: '300.00' }, dyprize: [{ "level": 2, "prize": [{ "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 695, prize: { 2: '300.00' }, dyprize: [{ "level": 2, "prize": [{ "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 696, prize: { 2: '300.00' }, dyprize: [{ "level": 2, "prize": [{ "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 709, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 710, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 711, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 712, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 713, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 714, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 715, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 716, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 717, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 718, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 342.66 }] }] }, { methodid: 766, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 767, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 768, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 769, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 770, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 771, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 772, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 773, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 774, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 775, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 600 }, { "point": 0, "prize": 642.66 }, { "point": "0.064", "prize": 300 }, { "point": 0, "prize": 321.33 }] }] }, { methodid: 721, prize: { 1: '18000.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 18000 }, { "point": 0, "prize": 19280 }] }] }, { methodid: 722, prize: { 1: '18000.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 18000 }, { "point": 0, "prize": 19280 }] }] }, { methodid: 723, prize: { 1: '18000.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 18000 }, { "point": 0, "prize": 19280 }] }] }, { methodid: 724, prize: { 1: '18000.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 18000 }, { "point": 0, "prize": 19280 }] }] }, { methodid: 725, prize: { 1: '18000.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 18000 }, { "point": 0, "prize": 19280 }] }] }, { methodid: 726, prize: { 1: '750.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 750 }, { "point": 0, "prize": 803.33 }] }] }, { methodid: 727, prize: { 1: '750.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 750 }, { "point": 0, "prize": 803.33 }] }] }, { methodid: 728, prize: { 1: '750.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 750 }, { "point": 0, "prize": 803.33 }] }] }, { methodid: 729, prize: { 1: '750.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 750 }, { "point": 0, "prize": 803.33 }] }] }, { methodid: 730, prize: { 1: '750.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 750 }, { "point": 0, "prize": 803.33 }] }] }, { methodid: 731, prize: { 2: '1500.00' }, dyprize: [{ "level": 2, "prize": [{ "point": "0.064", "prize": 1500 }, { "point": 0, "prize": 1606.66 }] }] }, { methodid: 732, prize: { 2: '1500.00' }, dyprize: [{ "level": 2, "prize": [{ "point": "0.064", "prize": 1500 }, { "point": 0, "prize": 1606.66 }] }] }, { methodid: 733, prize: { 2: '1500.00' }, dyprize: [{ "level": 2, "prize": [{ "point": "0.064", "prize": 1500 }, { "point": 0, "prize": 1606.66 }] }] }, { methodid: 734, prize: { 2: '1500.00' }, dyprize: [{ "level": 2, "prize": [{ "point": "0.064", "prize": 1500 }, { "point": 0, "prize": 1606.66 }] }] }, { methodid: 735, prize: { 2: '1500.00' }, dyprize: [{ "level": 2, "prize": [{ "point": "0.064", "prize": 1500 }, { "point": 0, "prize": 1606.66 }] }] }, { methodid: 736, prize: { 3: '3000.00' }, dyprize: [{ "level": 3, "prize": [{ "point": "0.064", "prize": 3000 }, { "point": 0, "prize": 3213.33 }] }] }, { methodid: 737, prize: { 3: '3000.00' }, dyprize: [{ "level": 3, "prize": [{ "point": "0.064", "prize": 3000 }, { "point": 0, "prize": 3213.33 }] }] }, { methodid: 738, prize: { 3: '3000.00' }, dyprize: [{ "level": 3, "prize": [{ "point": "0.064", "prize": 3000 }, { "point": 0, "prize": 3213.33 }] }] }, { methodid: 739, prize: { 3: '3000.00' }, dyprize: [{ "level": 3, "prize": [{ "point": "0.064", "prize": 3000 }, { "point": 0, "prize": 3213.33 }] }] }, { methodid: 740, prize: { 3: '3000.00' }, dyprize: [{ "level": 3, "prize": [{ "point": "0.064", "prize": 3000 }, { "point": 0, "prize": 3213.33 }] }] }, { methodid: 741, prize: { 4: '4500.00' }, dyprize: [{ "level": 4, "prize": [{ "point": "0.064", "prize": 4500 }, { "point": 0, "prize": 4820 }] }] }, { methodid: 742, prize: { 4: '4500.00' }, dyprize: [{ "level": 4, "prize": [{ "point": "0.064", "prize": 4500 }, { "point": 0, "prize": 4820 }] }] }, { methodid: 743, prize: { 4: '4500.00' }, dyprize: [{ "level": 4, "prize": [{ "point": "0.064", "prize": 4500 }, { "point": 0, "prize": 4820 }] }] }, { methodid: 744, prize: { 4: '4500.00' }, dyprize: [{ "level": 4, "prize": [{ "point": "0.064", "prize": 4500 }, { "point": 0, "prize": 4820 }] }] }, { methodid: 745, prize: { 4: '4500.00' }, dyprize: [{ "level": 4, "prize": [{ "point": "0.064", "prize": 4500 }, { "point": 0, "prize": 4820 }] }] }, { methodid: 865, prize: { 1: '180000.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180000 }, { "point": 0, "prize": 192800 }] }] }, { methodid: 867, prize: { 1: '180000.00', 2: '18000.00', 3: '1800.00', 4: '180.00', 5: '18.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 180000 }, { "point": 0, "prize": 192800 }] }, { "level": 2, "prize": [{ "point": "0.064", "prize": 180000 }, { "point": 0, "prize": 192800 }, { "point": "0.064", "prize": 18000 }, { "point": 0, "prize": 19280 }] }, { "level": 3, "prize": [{ "point": "0.064", "prize": 180000 }, { "point": 0, "prize": 192800 }, { "point": "0.064", "prize": 18000 }, { "point": 0, "prize": 19280 }, { "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }] }, { "level": 4, "prize": [{ "point": "0.064", "prize": 180000 }, { "point": 0, "prize": 192800 }, { "point": "0.064", "prize": 18000 }, { "point": 0, "prize": 19280 }, { "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }, { "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }] }, { "level": 5, "prize": [{ "point": "0.064", "prize": 180000 }, { "point": 0, "prize": 192800 }, { "point": "0.064", "prize": 18000 }, { "point": 0, "prize": 19280 }, { "point": "0.064", "prize": 1800 }, { "point": 0, "prize": 1928 }, { "point": "0.064", "prize": 180 }, { "point": 0, "prize": 192.8 }, { "point": "0.064", "prize": 18 }, { "point": 0, "prize": 19.28 }] }] }, { methodid: 869, prize: { 1: '1500.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.064", "prize": 1500 }, { "point": 0, "prize": 1606.66 }] }] }, { methodid: 870, prize: { 2: '3000.00' }, dyprize: [{ "level": 2, "prize": [{ "point": "0.064", "prize": 3000 }, { "point": 0, "prize": 3213.33 }] }] }, { methodid: 871, prize: { 3: '6000.00' }, dyprize: [{ "level": 3, "prize": [{ "point": "0.064", "prize": 6000 }, { "point": 0, "prize": 6426.66 }] }] }, { methodid: 872, prize: { 4: '9000.00' }, dyprize: [{ "level": 4, "prize": [{ "point": "0.064", "prize": 9000 }, { "point": 0, "prize": 9640 }] }] }, { methodid: 873, prize: { 5: '18000.00' }, dyprize: [{ "level": 5, "prize": [{ "point": "0.064", "prize": 18000 }, { "point": 0, "prize": 19280 }] }] }, { methodid: 874, prize: { 6: '36000.00' }, dyprize: [{ "level": 6, "prize": [{ "point": "0.064", "prize": 36000 }, { "point": 0, "prize": 38560 }] }] }];
	    <%}else if( setInfoSplit[0] == "5" ){%>
	    var pri_user_data = [{ methodid: 569, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 1800 }, { "point": 0, "prize": 1898 }] }] }, { methodid: 570, prize: { 1: '1800.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 1800 }, { "point": 0, "prize": 1898 }] }] }, { methodid: 572, prize: { 1: '600.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 600 }, { "point": 0, "prize": 632.66 }] }] }, { methodid: 573, prize: { 2: '300.00' }, dyprize: [{ "level": 2, "prize": [{ "point": "0.049", "prize": 300 }, { "point": 0, "prize": 316.33 }] }] }, { methodid: 574, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 600 }, { "point": 0, "prize": 632.66 }] }, { "level": 2, "prize": [{ "point": "0.049", "prize": 600 }, { "point": 0, "prize": 632.66 }, { "point": "0.049", "prize": 300 }, { "point": 0, "prize": 316.33 }] }] }, { methodid: 575, prize: { 1: '600.00', 2: '300.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 600 }, { "point": 0, "prize": 632.66 }] }, { "level": 2, "prize": [{ "point": "0.049", "prize": 600 }, { "point": 0, "prize": 632.66 }, { "point": "0.049", "prize": 300 }, { "point": 0, "prize": 316.33 }] }] }, { methodid: 577, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 180 }, { "point": 0, "prize": 189.8 }] }] }, { methodid: 579, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 90 }, { "point": 0, "prize": 94.9 }] }] }, { methodid: 581, prize: { 1: '180.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 180 }, { "point": 0, "prize": 189.8 }] }] }, { methodid: 583, prize: { 1: '90.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 90 }, { "point": 0, "prize": 94.9 }] }] }, { methodid: 585, prize: { 1: '18.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 18 }, { "point": 0, "prize": 18.98 }] }] }, { methodid: 586, prize: { 1: '18.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 18 }, { "point": 0, "prize": 18.98 }] }] }, { methodid: 587, prize: { 1: '18.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 18 }, { "point": 0, "prize": 18.98 }] }] }, { methodid: 589, prize: { 1: '6.60' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 6.6 }, { "point": 0, "prize": 6.96 }] }] }, { methodid: 591, prize: { 1: '33.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 33 }, { "point": 0, "prize": 34.81 }] }] }, { methodid: 593, prize: { 1: '7.20' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 7.2 }, { "point": 0, "prize": 7.59 }] }] }, { methodid: 594, prize: { 1: '7.20' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 7.2 }, { "point": 0, "prize": 7.59 }] }] }];
	    <%}else if( setInfoSplit[0] == "3"){%>
	    var pri_user_data = [{ methodid: 483, prize: { 1: '1782.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 1782 }, { "point": 0, "prize": 1879.02 }] }] }, { methodid: 485, prize: { 1: '297.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 297 }, { "point": 0, "prize": 313.17 }] }] }, { methodid: 486, prize: { 1: '297.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 297 }, { "point": 0, "prize": 313.17 }] }] }, { methodid: 488, prize: { 1: '198.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 198 }, { "point": 0, "prize": 208.78 }] }] }, { methodid: 490, prize: { 1: '99.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 99 }, { "point": 0, "prize": 104.39 }] }] }, { methodid: 491, prize: { 1: '99.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 99 }, { "point": 0, "prize": 104.39 }] }] }, { methodid: 493, prize: { 1: '6.60' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 6.6 }, { "point": 0, "prize": 6.95 }] }] }, { methodid: 495, prize: { 1: '19.80' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 19.8 }, { "point": 0, "prize": 20.87 }] }] }, { methodid: 496, prize: { 1: '19.80' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 19.8 }, { "point": 0, "prize": 20.87 }] }] }, { methodid: 497, prize: { 1: '19.80' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 19.8 }, { "point": 0, "prize": 20.87 }] }] }, { methodid: 499, prize: { 1: '831.00', 2: '138.00', 3: '27.70', 4: '11.00', 5: '5.50', 6: '4.10' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 831 }, { "point": 0, "prize": 876.27 }] }, { "level": 2, "prize": [{ "point": "0.049", "prize": 831 }, { "point": 0, "prize": 876.27 }, { "point": "0.049", "prize": 138 }, { "point": 0, "prize": 145.54 }] }, { "level": 3, "prize": [{ "point": "0.049", "prize": 831 }, { "point": 0, "prize": 876.27 }, { "point": "0.049", "prize": 138 }, { "point": 0, "prize": 145.54 }, { "point": "0.049", "prize": 27.7 }, { "point": 0, "prize": 29.2 }] }, { "level": 4, "prize": [{ "point": "0.049", "prize": 831 }, { "point": 0, "prize": 876.27 }, { "point": "0.049", "prize": 138 }, { "point": 0, "prize": 145.54 }, { "point": "0.049", "prize": 27.7 }, { "point": 0, "prize": 29.2 }, { "point": "0.049", "prize": 11 }, { "point": 0, "prize": 11.6 }] }, { "level": 5, "prize": [{ "point": "0.049", "prize": 831 }, { "point": 0, "prize": 876.27 }, { "point": "0.049", "prize": 138 }, { "point": 0, "prize": 145.54 }, { "point": "0.049", "prize": 27.7 }, { "point": 0, "prize": 29.2 }, { "point": "0.049", "prize": 11 }, { "point": 0, "prize": 11.6 }, { "point": "0.049", "prize": 5.5 }, { "point": 0, "prize": 5.8 }] }, { "level": 6, "prize": [{ "point": "0.049", "prize": 831 }, { "point": 0, "prize": 876.27 }, { "point": "0.049", "prize": 138 }, { "point": 0, "prize": 145.54 }, { "point": "0.049", "prize": 27.7 }, { "point": 0, "prize": 29.2 }, { "point": "0.049", "prize": 11 }, { "point": 0, "prize": 11.6 }, { "point": "0.049", "prize": 5.5 }, { "point": 0, "prize": 5.8 }, { "point": "0.049", "prize": 4.1 }, { "point": 0, "prize": 4.32 }] }] }, { methodid: 501, prize: { 1: '29.70', 2: '13.20', 3: '9.20', 4: '8.30' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 29.7 }, { "point": 0, "prize": 31.31 }] }, { "level": 2, "prize": [{ "point": "0.049", "prize": 29.7 }, { "point": 0, "prize": 31.31 }, { "point": "0.049", "prize": 13.2 }, { "point": 0, "prize": 13.91 }] }, { "level": 3, "prize": [{ "point": "0.049", "prize": 29.7 }, { "point": 0, "prize": 31.31 }, { "point": "0.049", "prize": 13.2 }, { "point": 0, "prize": 13.91 }, { "point": "0.049", "prize": 9.2 }, { "point": 0, "prize": 9.7 }] }, { "level": 4, "prize": [{ "point": "0.049", "prize": 29.7 }, { "point": 0, "prize": 31.31 }, { "point": "0.049", "prize": 13.2 }, { "point": 0, "prize": 13.91 }, { "point": "0.049", "prize": 9.2 }, { "point": 0, "prize": 9.7 }, { "point": "0.049", "prize": 8.3 }, { "point": 0, "prize": 8.75 }] }] }, { methodid: 503, prize: { 1: '3.90' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 3.9 }, { "point": 0, "prize": 4.11 }] }] }, { methodid: 505, prize: { 1: '9.90' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 9.9 }, { "point": 0, "prize": 10.43 }] }] }, { methodid: 506, prize: { 1: '9.90' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 9.9 }, { "point": 0, "prize": 10.43 }] }] }, { methodid: 508, prize: { 1: '29.70' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 29.7 }, { "point": 0, "prize": 31.31 }] }] }, { methodid: 509, prize: { 1: '29.70' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 29.7 }, { "point": 0, "prize": 31.31 }] }] }, { methodid: 511, prize: { 1: '118.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 118 }, { "point": 0, "prize": 124.46 }] }] }, { methodid: 512, prize: { 1: '118.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 118 }, { "point": 0, "prize": 124.46 }] }] }, { methodid: 514, prize: { 1: '831.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 831 }, { "point": 0, "prize": 876.27 }] }] }, { methodid: 515, prize: { 1: '831.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 831 }, { "point": 0, "prize": 876.27 }] }] }, { methodid: 517, prize: { 1: '138.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 138 }, { "point": 0, "prize": 145.54 }] }] }, { methodid: 518, prize: { 1: '138.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 138 }, { "point": 0, "prize": 145.54 }] }] }, { methodid: 520, prize: { 1: '39.60' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 39.6 }, { "point": 0, "prize": 41.75 }] }] }, { methodid: 521, prize: { 1: '39.60' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 39.6 }, { "point": 0, "prize": 41.75 }] }] }, { methodid: 523, prize: { 1: '14.80' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 14.8 }, { "point": 0, "prize": 15.6 }] }] }, { methodid: 524, prize: { 1: '14.80' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 14.8 }, { "point": 0, "prize": 15.6 }] }] }];
	    <%}%>

        var pri_cur_issue = <%=ViewData["CurGS"]%>;
	    var pri_issues = <%=ViewData["GSList"]%>;
	    var pri_lastopen = <%=ViewData["LastGSOpen"]%>;
	    var pri_issuecount = <%=currentGSDayCount%>;
	    pri_issuecount = 120;
	    var pri_servertime = '<%=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")%>';
	    var pri_lotteryid = <%=ViewData["GameID"]%>;
	    var pri_isdynamic = 1;
	    var pri_showhistoryrecord = 1;
	    var pri_ajaxurl = '/UI/PlayInfo';
	    var sys_prize_data = <%=ViewData["PrizeData"]%>;
	    var sys_max_point = <%=ViewData["SysMaxPoint"]%>;
	    var sys_my_max_point = <%=ViewData["MyMaxPoint"]%>;
	    var combuy_setting = {min_money:<%=ViewData["CombuyMinMoney"]%>, min_percent:<%=ViewData["CombuyMin"]%>};
	    var game_class_id = <%:ViewData["GameClassID"] %>;
        </script>
</body>
</html>
