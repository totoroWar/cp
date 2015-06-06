<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
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
        <title>娱乐平台  - 开始游戏[广东十一选五]</title>
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
                <script language="javascript">
                    function ResumeError() { return true; } window.onerror = ResumeError;
                    document.onselectstart = function (event) {
                        if (window.event) {
                            event = window.event;
                        }
                        try {
                            var the = event.srcElement;
                            if (!((the.tagName == "INPUT" && the.type.toLowerCase() == "text") || the.tagName == "TEXTAREA")) {
                                return false;
                            }
                            return true;
                        } catch (e) {
                            return false;
                        }
                    }
        </script>
    </head>
    <body>
        <div id="rightcon">
            <div id="msgbox" class="win_bot" style="display:none;">
    <h5 id="msgtitle"></h5> <div class="wb_close" onclick="javascript:msgclose();"></div>
    <div class="clear"></div>
    <div class="wb_con">
            <p id="msgcontent"></p>
    </div>
    <div class="clear"></div>
    <a class="wb_p" href="#" onclick="javascript:prenotice();" id="msgpre">上一条</a><a class="wb_n" href="#" onclick="javascript:nextnotice();">下一条</a>
</div>            <div class=" game_rc"> 
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
								<a class="gct_menu_yl" href='javascript:window.parent.parent.ui_show_tab("<%:ViewData["GameName"] %>遗漏", "/GameResult.html?gameID=<%=ViewData["GameID"] %>", true);'></a>
							</div>
						</div>
						<div class="gct_r">
							<h3>
								<%:ViewData["Title"] %>&nbsp;第<b><span class="nn" id="lt_gethistorycode"><%:lastGSItem.gs002.Trim() %></span></b>期 
                                <span id="lt_opentimebox" style="display: none;">&nbsp;&nbsp;
                                <span id="waitopendesc">等待开奖</span>&nbsp;<span style="color: #F9CE46;" id="lt_opentimeleft"></span></span>
                                <span id="lt_opentimebox2" style="display: none; color: #F9CE46;"><strong>&nbsp;&nbsp;正在开奖</strong></span>
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
            var pri_user_data = [{ methodid: 483, prize: { 1: '1782.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 1782 }, { "point": 0, "prize": 1879.02 }] }] }, { methodid: 485, prize: { 1: '297.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 297 }, { "point": 0, "prize": 313.17 }] }] }, { methodid: 486, prize: { 1: '297.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 297 }, { "point": 0, "prize": 313.17 }] }] }, { methodid: 488, prize: { 1: '198.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 198 }, { "point": 0, "prize": 208.78 }] }] }, { methodid: 490, prize: { 1: '99.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 99 }, { "point": 0, "prize": 104.39 }] }] }, { methodid: 491, prize: { 1: '99.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 99 }, { "point": 0, "prize": 104.39 }] }] }, { methodid: 493, prize: { 1: '6.60' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 6.6 }, { "point": 0, "prize": 6.95 }] }] }, { methodid: 495, prize: { 1: '19.80' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 19.8 }, { "point": 0, "prize": 20.87 }] }] }, { methodid: 496, prize: { 1: '19.80' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 19.8 }, { "point": 0, "prize": 20.87 }] }] }, { methodid: 497, prize: { 1: '19.80' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 19.8 }, { "point": 0, "prize": 20.87 }] }] }, { methodid: 499, prize: { 1: '831.00', 2: '138.00', 3: '27.70', 4: '11.00', 5: '5.50', 6: '4.10' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 831 }, { "point": 0, "prize": 876.27 }] }, { "level": 2, "prize": [{ "point": "0.049", "prize": 831 }, { "point": 0, "prize": 876.27 }, { "point": "0.049", "prize": 138 }, { "point": 0, "prize": 145.54 }] }, { "level": 3, "prize": [{ "point": "0.049", "prize": 831 }, { "point": 0, "prize": 876.27 }, { "point": "0.049", "prize": 138 }, { "point": 0, "prize": 145.54 }, { "point": "0.049", "prize": 27.7 }, { "point": 0, "prize": 29.2 }] }, { "level": 4, "prize": [{ "point": "0.049", "prize": 831 }, { "point": 0, "prize": 876.27 }, { "point": "0.049", "prize": 138 }, { "point": 0, "prize": 145.54 }, { "point": "0.049", "prize": 27.7 }, { "point": 0, "prize": 29.2 }, { "point": "0.049", "prize": 11 }, { "point": 0, "prize": 11.6 }] }, { "level": 5, "prize": [{ "point": "0.049", "prize": 831 }, { "point": 0, "prize": 876.27 }, { "point": "0.049", "prize": 138 }, { "point": 0, "prize": 145.54 }, { "point": "0.049", "prize": 27.7 }, { "point": 0, "prize": 29.2 }, { "point": "0.049", "prize": 11 }, { "point": 0, "prize": 11.6 }, { "point": "0.049", "prize": 5.5 }, { "point": 0, "prize": 5.8 }] }, { "level": 6, "prize": [{ "point": "0.049", "prize": 831 }, { "point": 0, "prize": 876.27 }, { "point": "0.049", "prize": 138 }, { "point": 0, "prize": 145.54 }, { "point": "0.049", "prize": 27.7 }, { "point": 0, "prize": 29.2 }, { "point": "0.049", "prize": 11 }, { "point": 0, "prize": 11.6 }, { "point": "0.049", "prize": 5.5 }, { "point": 0, "prize": 5.8 }, { "point": "0.049", "prize": 4.1 }, { "point": 0, "prize": 4.32 }] }] }, { methodid: 501, prize: { 1: '29.70', 2: '13.20', 3: '9.20', 4: '8.30' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 29.7 }, { "point": 0, "prize": 31.31 }] }, { "level": 2, "prize": [{ "point": "0.049", "prize": 29.7 }, { "point": 0, "prize": 31.31 }, { "point": "0.049", "prize": 13.2 }, { "point": 0, "prize": 13.91 }] }, { "level": 3, "prize": [{ "point": "0.049", "prize": 29.7 }, { "point": 0, "prize": 31.31 }, { "point": "0.049", "prize": 13.2 }, { "point": 0, "prize": 13.91 }, { "point": "0.049", "prize": 9.2 }, { "point": 0, "prize": 9.7 }] }, { "level": 4, "prize": [{ "point": "0.049", "prize": 29.7 }, { "point": 0, "prize": 31.31 }, { "point": "0.049", "prize": 13.2 }, { "point": 0, "prize": 13.91 }, { "point": "0.049", "prize": 9.2 }, { "point": 0, "prize": 9.7 }, { "point": "0.049", "prize": 8.3 }, { "point": 0, "prize": 8.75 }] }] }, { methodid: 503, prize: { 1: '3.90' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 3.9 }, { "point": 0, "prize": 4.11 }] }] }, { methodid: 505, prize: { 1: '9.90' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 9.9 }, { "point": 0, "prize": 10.43 }] }] }, { methodid: 506, prize: { 1: '9.90' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 9.9 }, { "point": 0, "prize": 10.43 }] }] }, { methodid: 508, prize: { 1: '29.70' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 29.7 }, { "point": 0, "prize": 31.31 }] }] }, { methodid: 509, prize: { 1: '29.70' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 29.7 }, { "point": 0, "prize": 31.31 }] }] }, { methodid: 511, prize: { 1: '118.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 118 }, { "point": 0, "prize": 124.46 }] }] }, { methodid: 512, prize: { 1: '118.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 118 }, { "point": 0, "prize": 124.46 }] }] }, { methodid: 514, prize: { 1: '831.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 831 }, { "point": 0, "prize": 876.27 }] }] }, { methodid: 515, prize: { 1: '831.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 831 }, { "point": 0, "prize": 876.27 }] }] }, { methodid: 517, prize: { 1: '138.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 138 }, { "point": 0, "prize": 145.54 }] }] }, { methodid: 518, prize: { 1: '138.00' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 138 }, { "point": 0, "prize": 145.54 }] }] }, { methodid: 520, prize: { 1: '39.60' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 39.6 }, { "point": 0, "prize": 41.75 }] }] }, { methodid: 521, prize: { 1: '39.60' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 39.6 }, { "point": 0, "prize": 41.75 }] }] }, { methodid: 523, prize: { 1: '14.80' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 14.8 }, { "point": 0, "prize": 15.6 }] }] }, { methodid: 524, prize: { 1: '14.80' }, dyprize: [{ "level": 1, "prize": [{ "point": "0.049", "prize": 14.8 }, { "point": 0, "prize": 15.6 }] }] }];
            var pri_cur_issue = { issue: '14042679', endtime: '2014-04-26 22:07:00', opentime: '2014-04-26 22:11:30' };
            var pri_issues = [{ issue: '14042601', endtime: '2014-04-26 09:07:00' }, { issue: '14042602', endtime: '2014-04-26 09:17:00' }, { issue: '14042603', endtime: '2014-04-26 09:27:00' }, { issue: '14042604', endtime: '2014-04-26 09:37:00' }, { issue: '14042605', endtime: '2014-04-26 09:47:00' }, { issue: '14042606', endtime: '2014-04-26 09:57:00' }, { issue: '14042607', endtime: '2014-04-26 10:07:00' }, { issue: '14042608', endtime: '2014-04-26 10:17:00' }, { issue: '14042609', endtime: '2014-04-26 10:27:00' }, { issue: '14042610', endtime: '2014-04-26 10:37:00' }, { issue: '14042611', endtime: '2014-04-26 10:47:00' }, { issue: '14042612', endtime: '2014-04-26 10:57:00' }, { issue: '14042613', endtime: '2014-04-26 11:07:00' }, { issue: '14042614', endtime: '2014-04-26 11:17:00' }, { issue: '14042615', endtime: '2014-04-26 11:27:00' }, { issue: '14042616', endtime: '2014-04-26 11:37:00' }, { issue: '14042617', endtime: '2014-04-26 11:47:00' }, { issue: '14042618', endtime: '2014-04-26 11:57:00' }, { issue: '14042619', endtime: '2014-04-26 12:07:00' }, { issue: '14042620', endtime: '2014-04-26 12:17:00' }, { issue: '14042621', endtime: '2014-04-26 12:27:00' }, { issue: '14042622', endtime: '2014-04-26 12:37:00' }, { issue: '14042623', endtime: '2014-04-26 12:47:00' }, { issue: '14042624', endtime: '2014-04-26 12:57:00' }, { issue: '14042625', endtime: '2014-04-26 13:07:00' }, { issue: '14042626', endtime: '2014-04-26 13:17:00' }, { issue: '14042627', endtime: '2014-04-26 13:27:00' }, { issue: '14042628', endtime: '2014-04-26 13:37:00' }, { issue: '14042629', endtime: '2014-04-26 13:47:00' }, { issue: '14042630', endtime: '2014-04-26 13:57:00' }, { issue: '14042631', endtime: '2014-04-26 14:07:00' }, { issue: '14042632', endtime: '2014-04-26 14:17:00' }, { issue: '14042633', endtime: '2014-04-26 14:27:00' }, { issue: '14042634', endtime: '2014-04-26 14:37:00' }, { issue: '14042635', endtime: '2014-04-26 14:47:00' }, { issue: '14042636', endtime: '2014-04-26 14:57:00' }, { issue: '14042637', endtime: '2014-04-26 15:07:00' }, { issue: '14042638', endtime: '2014-04-26 15:17:00' }, { issue: '14042639', endtime: '2014-04-26 15:27:00' }, { issue: '14042640', endtime: '2014-04-26 15:37:00' }, { issue: '14042641', endtime: '2014-04-26 15:47:00' }, { issue: '14042642', endtime: '2014-04-26 15:57:00' }, { issue: '14042643', endtime: '2014-04-26 16:07:00' }, { issue: '14042644', endtime: '2014-04-26 16:17:00' }, { issue: '14042645', endtime: '2014-04-26 16:27:00' }, { issue: '14042646', endtime: '2014-04-26 16:37:00' }, { issue: '14042647', endtime: '2014-04-26 16:47:00' }, { issue: '14042648', endtime: '2014-04-26 16:57:00' }, { issue: '14042649', endtime: '2014-04-26 17:07:00' }, { issue: '14042650', endtime: '2014-04-26 17:17:00' }, { issue: '14042651', endtime: '2014-04-26 17:27:00' }, { issue: '14042652', endtime: '2014-04-26 17:37:00' }, { issue: '14042653', endtime: '2014-04-26 17:47:00' }, { issue: '14042654', endtime: '2014-04-26 17:57:00' }, { issue: '14042655', endtime: '2014-04-26 18:07:00' }, { issue: '14042656', endtime: '2014-04-26 18:17:00' }, { issue: '14042657', endtime: '2014-04-26 18:27:00' }, { issue: '14042658', endtime: '2014-04-26 18:37:00' }, { issue: '14042659', endtime: '2014-04-26 18:47:00' }, { issue: '14042660', endtime: '2014-04-26 18:57:00' }, { issue: '14042661', endtime: '2014-04-26 19:07:00' }, { issue: '14042662', endtime: '2014-04-26 19:17:00' }, { issue: '14042663', endtime: '2014-04-26 19:27:00' }, { issue: '14042664', endtime: '2014-04-26 19:37:00' }, { issue: '14042665', endtime: '2014-04-26 19:47:00' }, { issue: '14042666', endtime: '2014-04-26 19:57:00' }, { issue: '14042667', endtime: '2014-04-26 20:07:00' }, { issue: '14042668', endtime: '2014-04-26 20:17:00' }, { issue: '14042669', endtime: '2014-04-26 20:27:00' }, { issue: '14042670', endtime: '2014-04-26 20:37:00' }, { issue: '14042671', endtime: '2014-04-26 20:47:00' }, { issue: '14042672', endtime: '2014-04-26 20:57:00' }, { issue: '14042673', endtime: '2014-04-26 21:07:00' }, { issue: '14042674', endtime: '2014-04-26 21:17:00' }, { issue: '14042675', endtime: '2014-04-26 21:27:00' }, { issue: '14042676', endtime: '2014-04-26 21:37:00' }, { issue: '14042677', endtime: '2014-04-26 21:47:00' }, { issue: '14042678', endtime: '2014-04-26 21:57:00' }, { issue: '14042679', endtime: '2014-04-26 22:07:00' }, { issue: '14042680', endtime: '2014-04-26 22:17:00' }, { issue: '14042681', endtime: '2014-04-26 22:27:00' }, { issue: '14042682', endtime: '2014-04-26 22:37:00' }, { issue: '14042683', endtime: '2014-04-26 22:47:00' }, { issue: '14042684', endtime: '2014-04-26 22:57:00' }, { issue: '14042701', endtime: '2014-04-27 09:07:00' }, { issue: '14042702', endtime: '2014-04-27 09:17:00' }, { issue: '14042703', endtime: '2014-04-27 09:27:00' }, { issue: '14042704', endtime: '2014-04-27 09:37:00' }, { issue: '14042705', endtime: '2014-04-27 09:47:00' }, { issue: '14042706', endtime: '2014-04-27 09:57:00' }, { issue: '14042707', endtime: '2014-04-27 10:07:00' }, { issue: '14042708', endtime: '2014-04-27 10:17:00' }, { issue: '14042709', endtime: '2014-04-27 10:27:00' }, { issue: '14042710', endtime: '2014-04-27 10:37:00' }, { issue: '14042711', endtime: '2014-04-27 10:47:00' }, { issue: '14042712', endtime: '2014-04-27 10:57:00' }, { issue: '14042713', endtime: '2014-04-27 11:07:00' }, { issue: '14042714', endtime: '2014-04-27 11:17:00' }, { issue: '14042715', endtime: '2014-04-27 11:27:00' }, { issue: '14042716', endtime: '2014-04-27 11:37:00' }, { issue: '14042717', endtime: '2014-04-27 11:47:00' }, { issue: '14042718', endtime: '2014-04-27 11:57:00' }, { issue: '14042719', endtime: '2014-04-27 12:07:00' }, { issue: '14042720', endtime: '2014-04-27 12:17:00' }, { issue: '14042721', endtime: '2014-04-27 12:27:00' }, { issue: '14042722', endtime: '2014-04-27 12:37:00' }, { issue: '14042723', endtime: '2014-04-27 12:47:00' }, { issue: '14042724', endtime: '2014-04-27 12:57:00' }, { issue: '14042725', endtime: '2014-04-27 13:07:00' }, { issue: '14042726', endtime: '2014-04-27 13:17:00' }, { issue: '14042727', endtime: '2014-04-27 13:27:00' }, { issue: '14042728', endtime: '2014-04-27 13:37:00' }, { issue: '14042729', endtime: '2014-04-27 13:47:00' }, { issue: '14042730', endtime: '2014-04-27 13:57:00' }, { issue: '14042731', endtime: '2014-04-27 14:07:00' }, { issue: '14042732', endtime: '2014-04-27 14:17:00' }, { issue: '14042733', endtime: '2014-04-27 14:27:00' }, { issue: '14042734', endtime: '2014-04-27 14:37:00' }, { issue: '14042735', endtime: '2014-04-27 14:47:00' }, { issue: '14042736', endtime: '2014-04-27 14:57:00' }, { issue: '14042737', endtime: '2014-04-27 15:07:00' }, { issue: '14042738', endtime: '2014-04-27 15:17:00' }, { issue: '14042739', endtime: '2014-04-27 15:27:00' }, { issue: '14042740', endtime: '2014-04-27 15:37:00' }, { issue: '14042741', endtime: '2014-04-27 15:47:00' }, { issue: '14042742', endtime: '2014-04-27 15:57:00' }, { issue: '14042743', endtime: '2014-04-27 16:07:00' }, { issue: '14042744', endtime: '2014-04-27 16:17:00' }, { issue: '14042745', endtime: '2014-04-27 16:27:00' }, { issue: '14042746', endtime: '2014-04-27 16:37:00' }, { issue: '14042747', endtime: '2014-04-27 16:47:00' }, { issue: '14042748', endtime: '2014-04-27 16:57:00' }, { issue: '14042749', endtime: '2014-04-27 17:07:00' }, { issue: '14042750', endtime: '2014-04-27 17:17:00' }, { issue: '14042751', endtime: '2014-04-27 17:27:00' }, { issue: '14042752', endtime: '2014-04-27 17:37:00' }, { issue: '14042753', endtime: '2014-04-27 17:47:00' }, { issue: '14042754', endtime: '2014-04-27 17:57:00' }, { issue: '14042755', endtime: '2014-04-27 18:07:00' }, { issue: '14042756', endtime: '2014-04-27 18:17:00' }, { issue: '14042757', endtime: '2014-04-27 18:27:00' }, { issue: '14042758', endtime: '2014-04-27 18:37:00' }, { issue: '14042759', endtime: '2014-04-27 18:47:00' }, { issue: '14042760', endtime: '2014-04-27 18:57:00' }, { issue: '14042761', endtime: '2014-04-27 19:07:00' }, { issue: '14042762', endtime: '2014-04-27 19:17:00' }, { issue: '14042763', endtime: '2014-04-27 19:27:00' }, { issue: '14042764', endtime: '2014-04-27 19:37:00' }, { issue: '14042765', endtime: '2014-04-27 19:47:00' }, { issue: '14042766', endtime: '2014-04-27 19:57:00' }, { issue: '14042767', endtime: '2014-04-27 20:07:00' }, { issue: '14042768', endtime: '2014-04-27 20:17:00' }, { issue: '14042769', endtime: '2014-04-27 20:27:00' }, { issue: '14042770', endtime: '2014-04-27 20:37:00' }, { issue: '14042771', endtime: '2014-04-27 20:47:00' }, { issue: '14042772', endtime: '2014-04-27 20:57:00' }, { issue: '14042773', endtime: '2014-04-27 21:07:00' }, { issue: '14042774', endtime: '2014-04-27 21:17:00' }, { issue: '14042775', endtime: '2014-04-27 21:27:00' }, { issue: '14042776', endtime: '2014-04-27 21:37:00' }, { issue: '14042777', endtime: '2014-04-27 21:47:00' }, { issue: '14042778', endtime: '2014-04-27 21:57:00' }, { issue: '14042779', endtime: '2014-04-27 22:07:00' }, { issue: '14042780', endtime: '2014-04-27 22:17:00' }, { issue: '14042781', endtime: '2014-04-27 22:27:00' }, { issue: '14042782', endtime: '2014-04-27 22:37:00' }, { issue: '14042783', endtime: '2014-04-27 22:47:00' }, { issue: '14042784', endtime: '2014-04-27 22:57:00' }];
            var pri_lastopen = { issue: '14042678', endtime: '2014-04-26 21:58:00', opentime: '2014-04-26 22:01:30', statuscode: '0' };
            var pri_issuecount = 84;
            var pri_servertime = '2014-04-26 21:58:00';
            var pri_lotteryid = parseInt(9, 10);
            var pri_isdynamic = 1;
            var pri_showhistoryrecord = 1;
            var pri_ajaxurl = './play_gd115.shtml';
            var _gaq = _gaq || [];
            _gaq.push(['_setAccount', 'UA-36352852-1']);
            _gaq.push(['_trackPageview']);
            (function () {
                var $bn = $("#bonusnotice", window.top.document);
                var $mainbox = $("#mainbox", window.top.document);
                var $leftbox = $("#leftbox", window.top.document);
                $bn.css("height", "0");
                $mainbox.height($leftbox.height());
                var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
                ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
                var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
            })();
        </script>
    </body>
</html>