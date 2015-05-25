<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%
    var curGSItem = (DBModel.wgs005)ViewData["CurGSItem"];
    var lastGSItem = (DBModel.wgs005)ViewData["LastGSItem"];
    var historyOrderList = (List<DBModel.LotteryHistoryOrder>)ViewData["HistoryOrderList"];
    var currentGSDayCount = (int)ViewData["GSDayCount"];
    var UITheme = (string)ViewData["UITheme"];
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
<script type="text/javascript" src="/StaticGame/Theme/SSC_<%=UITheme%>/Scripts/face.1.js"></script>
<script type="text/javascript" src="/StaticGame/Theme/SSC_<%=UITheme%>/Scripts/methods.1.js"></script>
<script type="text/javascript" src="/StaticGame/Theme/SSC_<%=UITheme%>/Scripts/jquery.game.0.js"></script>
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
                              { %>
                            <div class="gct_r_nub" id="showcodebox">
								<div class="gr_s" flag="move"></div>
								<div class="gr_s" flag="move"></div>
								<div class="gr_s" flag="move"></div>
								<div class="gr_s" flag="move"></div>
								<div class="gr_s" flag="move"></div>
                            </div>
                            <%}else
                              { %>
                            <%
                                var result = lastGSItem.gs007.Trim().Split(',');
                            %>
							<div class="gct_r_nub" id="showcodebox">
                                <%foreach(var n in result)
                                  { %>
								<div class="gr_s gr_s<%:n %>" flag="normal"></div>
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
					<p>建议使用IE6以上版本浏览器或Firefox、Google，在1024*768或以上屏幕分辨率下浏览本网站可得到最佳视觉</p>
				</div>
			</form>
		</div>
	</div>
	<script type="text/javascript">
        var pri_cur_issue = <%=ViewData["CurGS"]%>;
	    var pri_issues = <%=ViewData["GSList"]%>;
	    var pri_lastopen = <%=ViewData["LastGSOpen"]%>;
	    var pri_issuecount = 120;
	    var pri_servertime = '<%=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")%>';
	    var pri_lotteryid = <%=ViewData["GameID"]%>;
	    var pri_isdynamic = 1;
	    var pri_showhistoryrecord = 1;
	    var pri_ajaxurl = '/PlayInfo.html';
	    var sys_prize_data = <%=ViewData["PrizeData"]%>;
	    var sys_max_point = <%=ViewData["SysMaxPoint"]%>;
	    var sys_my_max_point = <%=ViewData["MyMaxPoint"]%>;
	    var combuy_setting = {min_money:<%=ViewData["CombuyMinMoney"]%>, min_percent:<%=ViewData["CombuyMin"]%>};
        </script>
</body>
</html>
