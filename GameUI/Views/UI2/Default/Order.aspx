<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI2/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
                        <li class="on"><a href="/UI2/Record?method=orderDefault" title="游戏记录" <%=methodType == "orderDefault" ? "class='item-select'" : "" %>>游戏记录</a></li>
                        <li><a href="/UI2/Record?method=orderTrace" title="追号记录" <%=methodType == "orderTrace" ? "class='item-select'" : "" %>>追号记录</a></li>
                        <li><a href="/UI2/Record?method=orderCombine" title="合买记录" <%=methodType == "orderCombine" ? "class='item-select'" : "" %>>合买记录</a></li>
                    </ul>
                </div>
<%if ("orderDefault" == methodType)
    {
        %>
<%
        var gcList = (List<DBModel.wgs006>)ViewData["GameClassList"];
        var gList = (List<DBModel.wgs001>)ViewData["GameList"];
        var gmList = (List<DBModel.wgs002>)ViewData["GameMethodList"];
        var gDicList = gList.ToDictionary(key => key.g001);
        var gmDicList = gmList.ToDictionary(key=>key.gm001);
        var orderList = (List<DBModel.wgs045>)ViewData["OrderList"];
        var orderShowList = (List<DBModel.LotteryHistoryOrder>)ViewData["OrderShowList"];
        var orderGame = (int)ViewData["OrderGame"];
        var orderGameClass = (int)ViewData["OrderGameClass"];
        var orderMethod = (int)ViewData["OrderGameMethod"];
        var orderCancel = (int)ViewData["OrderCancel"];
        var orderUserType = (int)ViewData["OrderUserType"];
        var orderModes = (int)ViewData["OrderModes"];
        var orderID = (long)ViewData["OrderID"];
        var orderIsWin = (int)ViewData["OrderIsWin"];
        var priceSum = 0m;
        var boundSum = 0m;
        var pointSum = 0m;
%>

<div class="user_info_data_box">
    <div class="user_add_bank">
        <div class="search_game_recode">
            <form action="/UI2/Record" id="form_order_default" method="get">
            <div class="gd_search_input">
                <ul>
                    <li><span class="select_span">游戏：</span>
                        <select name="orderGame" class="drop-select-to-url select_2015">
                            <option value="0" title="所有" tourl="/UI2/Record?method=orderDefault&orderGameClass=0&orderGame=0">所有</option>
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
                            <option value="<%:gameItem %>" <%:orderGame==int.Parse(gameItem) ? "selected='selected'" : "" %> tourl="/UI2/Record?method=orderDefault&orderGameClass=<%:gc.gc001 %>&orderGame=<%:gameItem %>"><%:gDicList[int.Parse(gameItem)].g003.Trim() %></option>
                            <%
                                    }/*game*/
                                }
                            %>
                        </select>
                        <input type="hidden" name="orderGameClass" value="<%:ViewData["orderGameClass"] %>" />
                    </li>
                    <li><span class="select_span">玩法：</span>
                        <select name="orderMethod" id="orderMethod" class="select_2015">
                            <option value="0" title="所有">所有</option>
                            <%
                            if (0 < orderGame)
                                {
                                    var nGmList = gmList.Where(exp => exp.g001 == orderGameClass && exp.gm002 > 0).ToList();
                                    foreach (var gm in nGmList)
                                    {
                            %>
                            <option value="<%:gm.gm001 %>" <%:gm.gm001 == orderMethod ? "selected='selected'" : "" %>><%:gmDicList[gm.gm002].gm004 %>-<%:gm.gm004.Trim() %></option>
                            <%
                            }
                                } 
                                %>
                        </select>
                    </li>
                    <li><span class="select_span">模式：</span>
                        <select name="orderModes" id="orderModes" class="select_2015">
                            <option value="0" title="所有">所有</option>
                            <option value="1" <%:orderModes == 1? "selected='selected'" : "" %>>元</option>
                            <option value="2" <%:orderModes == 2? "selected='selected'" : "" %>>角</option>
                            <option value="3" <%:orderModes == 3? "selected='selected'" : "" %>>分</option>
                        </select>
                    </li>
                    <li>期号：<input type="text" name="issue" class="gd_txt" value="<%:ViewData["Issue"] %>" /></li>
                    <li><span class="select_span">用户：</span>
                            <select name="orderUser" id="orderUser" class="select_2015">
                            <option value="0" title="自己" <%:orderUserType == 0? "selected='selected'" : "" %>>自己</option>
                            <option value="1" title="直接下级" <%:orderUserType == 1? "selected='selected'" : "" %>>直接下级</option>
                            <option value="2" title="所有下级" <%:orderUserType == 2? "selected='selected'" : "" %>>所有下级</option>
                        </select>
                    </li>
                </ul>
                <div class="clear"></div>
                <ul class="margin_top12">
                    
                    <li>单号：<input type="text" class="gd_txt"  name="orderID" id="orderID" value="<%:orderID == 0 ? "" : orderID.ToString() %>" /></li>
                    <li>时间范围：<input type="text" name="orderDTS" id="orderDTS" class="gd_txt gd_time" value="<%:ViewData["DTS"].ToString().Replace("-","/") %>" />-<input type="text" name="orderDTE" id="orderDTE" class="gd_txt gd_time" value="<%:ViewData["DTE"].ToString().Replace("-","/") %>" /></li>
                    <li><span class="select_span">中奖：</span>
                            <select name="orderIsWin" id="orderIsWin" class="select_2015">
                            <option value="0" <%:orderIsWin == 0 ? "selected='selected'" : "" %>>所有</option>
                            <option value="1" <%:orderIsWin == 1 ? "selected='selected'" : "" %>>已中奖</option>
                            <option value="2" <%:orderIsWin == 2 ? "selected='selected'" : "" %>>未中奖</option>
                        </select>
                    </li>
                    <li><span class="select_span">撤单：</span>
                        <select name="orderCancel" id="orderCancel" class="select_2015">
                            <option value="-1">所有</option>
                            <option value="0" <%:orderCancel == 0 ? "selected='selected'" : "" %>>没有撤单</option>
                            <option value="1" <%:orderCancel == 1 ? "selected='selected'" : "" %>>本人撒单</option>
                            <option value="2" <%:orderCancel == 2 ? "selected='selected'" : "" %>>管理员撤单</option>
                            <option value="3" <%:orderCancel == 3 ? "selected='selected'" : "" %>>开错奖撤单</option>
                        </select>
                    </li>
                    <li><input class="gd_sbtn ui-post-loading" value="查找" type="submit"></li>          
                </ul>
            </div>
            </form>
            <div class="clear"></div>
            <div class="gd_search_selet">
                <ul>
                    <li><a href="#" onclick="table_row_selecct();">全选</a></li>
                    <li><a href="#" onclick="table_row_unselect();">反选</a></li>
                    <li><a href="#" onclick="table_row_clear_select();">取消选中</a></li>
                    <li><a href="#"  id="btn_cancel_order">撤单</a></li>
                </ul>
            </div>
        </div>
        <div class="clear"></div>
        <div class="bank_list_box">
            <div class="bank_list">
                <table class="ctable_box youxi_table_list" width="100%" border="0" cellpadding="0" cellspacing="0">
                  <thead><tr class="table-pro-head">
                        <th>单号</th>
                        <th>账号</th>
                        <th>游戏</th>
                        <th class="dom-hide">玩法</th>
                        <th>期号</th>
                        <th>内容</th>
                        <th>金额</th>
                        <th>倍数</th>
                        <th>注数</th>
                        <th>模式</th>
                        <th>奖金</th>
                        <th>返点</th>
                        <th>时间</th>
                        <th>位置</th>
                        <th>状态</th>
                    </tr>
                  </thead>
                    <tbody>
                        <%if(null != orderShowList)
            {
                foreach( var item in orderShowList)
                {
                    priceSum += item.totalprice;
                    boundSum += item.bonus;
                    pointSum += item.resultpoint;
                %>
        <tr key="<%:item.projectid %>">
            <td>
                <a class="show_ord_des"  href="javascript:;" title="查看详情" data="<%:item.projectid %>"><%:item.projectid %></a>
                <textarea class="dom-hide" style="display:none" rows="10" cols="10" id="json_<%:item.projectid %>"><%=Newtonsoft.Json.JsonConvert.SerializeObject(item) %></textarea>
            </td>
            <td><%:item.username %></td>
            <td><%:item.cnname %><%=item.taskid != 0 ? "<span class='o_trace' title='追号' data='"+item.taskid+"'>[追号]</span>" : ""  %><%=item.combineOrderID != 0 && item.combineType == 0 ? "<span class='o_combuy' title='发起合买' data='"+item.combineOrderID+"'>[发起合买]</span>" : "" %><%=item.combineOrderID != 0 && item.combineType == 1 ? "<span class='o_jcombuy' title='参与合买' data='"+item.combineOrderID+"'>[参与合买]</span>" : "" %></td>
            <td class="dom-hide"><%:item.methodname %></td>
            <td><%:item.issue %></td>
            <td title="<%:item.code %>"><a href="javascript:;" data="<%:item.projectid %>" class="show_ord_des"><%:item.codeShort %></a></td>
            <td class="fc-yellow"><%:item.totalprice.ToString("N4") %></td>
            <td><%:item.multiple%></td>
            <td>
                <%if( item.combineOrderID != 0 && item.combineType == 0){  %>
                <%:item.times %>
                <%} else if( item.combineOrderID != 0 && item.combineType == 1 ){%>
                <%:item.times %>份
                <%}else{ %>
                <%:item.times %>
                <%} %>
            </td>
            <td><%:item.modes %></td>
            <td class="<%:item.bonus > 0 ? "fc-red" : "" %>"><%:item.bonus.ToString("N4")%></td>
            <td><%:item.resultpoint.ToString("N4") %></td>
            <td><%:item.writeDateTime.ToString() %></td>
            <%
                var statusClass= string.Empty;
                statusClass = item.iscancel == 1 ? "ord_cancel" : "";
                if (item.isgetprize == 0)
                {
                    statusClass = "ord_not_open";
                    if (item.iscancel == 0)
                    {
                        statusClass = item.prizestatus == 1 ? "ord_cancel" : "";
                    }
                }
                else
                {
                    if (item.isgetprize == 0)
                    {
                        statusClass = "ord_not_open";
                    }
                    else if (item.isgetprize == 1)
                    {
                        statusClass = "ord_win";
                    }
                    else if (item.isgetprize == 2)
                    {
                        statusClass = "ord_lose"; 
                    }
                }
                %>
            <td>
            <%
                var str = new string[]{"万", "千", "百", "十", "个"};
                for (var i = 0; i < 5; i++)
                {
                    item.pos = item.pos.Replace(i.ToString(), str[i]);
                }
            %>
            <%:item.pos%></td>
            <td class="<%:statusClass %>"><%:item.statusdesc %></td>
        </tr>
        <%}
            } %>
        </tbody>
        <tfoot>
        <tr class="table-pro-head">
            <td>合计</td>
            <td>-</td>
            <td>-</td>
            <td>-</td>
            <td>-</td>
            <td class="fc-yellow"><%:priceSum %></td>
            <td>-</td>
            <td>-</td>
            <td>-</td>
            <td><%:boundSum %></td>
            <td><%:pointSum %></td>
            <td>-</td>
            <td>-</td>
            <td>-</td>
            <td class="dom-hide">-</td>
        </tr>
    </tfoot>
                </table>

            </div>
            <div class="wp_page fl_r">
                <%=ViewData["PageList"] %>
            </div>
        </div>
    </div>
</div>
<div class="text-left dom-hide" id="show_des">
        <table class=" table-pro table-noborder">
            <tr>
                <td class="title">游戏/玩法</td>
                <td id="s_cnname"></td>
            </tr>
            <tr>
                <td class="title">期号/结果</td>
                <td id="s_issue"></td>
            </tr>
            <tr>
                <td class="title">模式</td>
                <td id="s_modes"></td>
            </tr>
            <tr>
                <td class="title">时间</td>
                <td id="s_time"></td>
            </tr>
            <tr>
                <td class="title">内容</td>
                <td>
                    <textarea rows="3" cols="35" class="input-text" id="s_code"></textarea>
                </td>
            </tr>
            <tr>
                <td class="title">注数/倍数</td>
                <td id="s_times_mul"></td>
            </tr>
            <tr>
                <td class="title">金额</td>
                <td id="s_totalprice" class="fc-yellow"></td>
            </tr>
            <tr>
                <td class="title">奖金/返点</td>
                <td id="s_result" class="fc-red"></td>
            </tr>
            <tr>
                <td class="title">投注返点</td>
                <td id="s_ord_point"></td>
            </tr>
            <tr>
                <td class="title">可中奖项</td>
                <td>
                    <textarea id="s_canp" rows="3" cols="35" class="input-text">
                    </textarea>
                </td>
            </tr>
        </table>
    </div>
<%}/*orderDefault*/ %>
<%else if ("orderTrace" == methodType)
{ %>
<%
    var gcList = (List<DBModel.wgs006>)ViewData["GameClassList"];
    var gList = (List<DBModel.wgs001>)ViewData["GameList"];
    var gmList = (List<DBModel.wgs002>)ViewData["GameMethodList"];
    var gDicList = gList.ToDictionary(key => key.g001);
    var orderGame = (int)ViewData["OrderGame"];
    var tOrderList = (List<DBModel.wgs030>)ViewData["TOrderList"];
    var orderStatus = (int)ViewData["OrderStatus"];
    var orderUserType = (int)ViewData["OrderUserType"];
    var orderUserName = (string)ViewData["OrderUserName"];
%>
<div class="user_info_data_box">
<div class="user_add_bank">
    <div class="search_game_recode">
        <form action="/UI2/Record?method=orderTrace" id="form_order_trace" method="get">
        <div class="gd_search_input">
            <ul>
                <li><span class="select_span">游戏：</span>
                    
                    <input type="hidden" name="method" value="<%:methodType %>" />
                    <input type="hidden" name="orderGameClass" value="<%:ViewData["orderGameClass"] %>" />
                    <select name="orderGame" class="drop-select-to-url select_2015">
                        <option value="0" title="所有" tourl="/UI2/Record?method=orderTrace&orderGameClass=0&orderGame=0">所有</option>
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
                        <option value="<%:gameItem %>" <%:orderGame==int.Parse(gameItem) ? "selected='selected'" : "" %> tourl="/UI2/Record?method=orderTrace&orderGameClass=<%:gc.gc001 %>&orderGame=<%:gameItem %>"><%:gDicList[int.Parse(gameItem)].g003.Trim() %></option>
                        <%
                              }/*game*/
                          }
                        %>
                    </select>
                </li>
                <li><span class="select_span">用户：</span>
                    <select name="orderUser" id="orderUser" class="select_2015">
                        <option value="0" title="自己" <%:orderUserType == 0? "selected='selected'" : "" %>>自己</option>
                        <option value="1" title="直接下级" <%:orderUserType == 1? "selected='selected'" : "" %>>直接下级</option>
                        <option value="2" title="所有下级" <%:orderUserType == 2? "selected='selected'" : "" %>>所有下级</option>
                    </select>
                </li>
                <li>账号：<input type="text" class="gd_txt" name="orderUserName" id="orderUserName" value="<%:orderUserName %>" /></li>
                <div class="clear"></div>
                <li>单号：<input type="text" class="gd_txt"  name="orderID" id="orderID" value="<%:ViewData["OrderID"] %>" /></li>
                <li>时间范围：<input type="text" name="orderDTS" id="orderDTS" class="gd_txt gd_time" value="<%:ViewData["DTS"].ToString().Replace("-","/") %>" />-<input type="text" name="orderDTE" id="orderDTE" class="gd_txt gd_time" value="<%:ViewData["DTE"].ToString().Replace("-","/") %>" /></li>
                <li><input class="gd_sbtn ui-post-loading" value="查找" type="submit"></li>
            </ul>
        </div>
        </form>
        <div class="clear"></div>
        <div class="gd_search_selet">
            <ul>
                <li><a href="#" onclick="table_row_selecct();">全选</a></li>
                <li><a href="#" onclick="table_row_unselect();">反选</a></li>
                <li><a href="#" onclick="table_row_clear_select();">取消选中</a></li>
                <li><a href="#"  id="btn_cancel_torder">撤单</a></li>
            </ul>
        </div>
    </div>
    <div class="clear"></div>
    <div class="bank_list_box">
        <div class="bank_list">
            <table class="ctable_box youxi_table_list" width="100%" border="0" cellpadding="0" cellspacing="0">
                <tbody><tr class="table-pro-head">
                    <th>追号单号</th>
                    <th>账号</th>
                    <th>游戏</th>
                    <th>起始期号</th>
                    <th>总金额</th>
                    <th>实际金额</th>
                    <th>中奖金额</th>
                    <th>时间</th>
                    <th>状态</th>
                    <th>类型</th>
                    <th></th>
                </tr>
                 <%if( null != tOrderList)
              { 
                  foreach(var item in tOrderList)
                  {
                  %>
            <tr key="<%:item.sto001 %>">
                <td><%:item.sto001 %></td>
                <td><%:item.u002.Trim() %></td>
                <td><%:gDicList[item.g001].g003.Trim() %></td>
                <td><%:item.sto010.Trim() %></td>
                <td><%:item.sto002.ToString("N2") %></td>
                <td><%:item.sto007.ToString("N2") %></td>
                <td><%:item.sto008.ToString("N2") %></td>
                <td><%:item.sto004.ToString(ViewContext.ViewData["SysDateTimeFormat"].ToString()) %></td>
                <td><%
                      var toStatus = string.Empty;
                      switch (item.sto005)
                      {
                          case 0:
                              toStatus = "<span class='fc-red'>未完成</span>";
                              break;
                          case 1:
                              toStatus = "<span class='fc-green'>已完成</span>";
                              break;
                          case 2:
                              toStatus = "<span class='fc-gray'>已撤单</span>";
                              break; 
                      }
                        %><%=toStatus %></td>
                <td><%=item.sto009 == 0 ? "连续追号" : "追中即停" %></td>
                <td class="link-tools"><a href="/UI2/Record?method=orderTraceDetail&key=<%:item.sto001 %>" title="详细">详细</a><a href="javascript:;" class="dom-hide" name="cancel_order" data="<%:item.sto001 %>">撤单</a></td>
            </tr>
            <%}
              }/*if*/ %>
            
            </tbody></table>
            
        </div>
        <div class="wp_page fl_r">
            <%=ViewData["PageList"] %>
        </div>
    </div>
</div>
</div>

<%}/*orderTrace*/ %>

<%else if( "orderCombine" == methodType)
{
    var gcList = (List<DBModel.wgs006>)ViewData["GameClassList"];
    var gList = (List<DBModel.wgs001>)ViewData["GameList"];
    var gmList = (List<DBModel.wgs002>)ViewData["GameMethodList"];
    var gDicList = gList.ToDictionary(key => key.g001);
    var orderGame = (int)ViewData["OrderGame"];
    var combuyList = (List<DBModel.wgs031>)ViewData["CombuyList"];
%>
<div class="user_info_data_box">
<div class="user_add_bank">
    <div class="search_game_recode">
        <form action="/UI2/Record?method=orderCombine" id="form_order_combuy" method="get">
        <div class="gd_search_input">
            <ul>
                <li><span class="select_span">游戏：</span>
                    
                    <input type="hidden" name="method" value="<%:methodType %>" />
                    <input type="hidden" name="orderGameClass" value="<%:ViewData["orderGameClass"] %>" />
                    <select name="orderGame" class="drop-select-to-url select_2015">
                        <option value="0" title="所有" tourl="/UI2/Record?method=orderCombine&orderGameClass=0&orderGame=0">所有</option>
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
                        <option value="<%:gameItem %>" <%:orderGame==int.Parse(gameItem) ? "selected='selected'" : "" %> tourl="/UI2/Record?method=orderCombine&orderGameClass=<%:gc.gc001 %>&orderGame=<%:gameItem %>"><%:gDicList[int.Parse(gameItem)].g003.Trim() %></option>
                        <%
                              }/*game*/
                          }
                        %>
                    </select>
                </li>
                <li>期号：<input type="text" class="gd_txt" name="issue" value="<%:ViewData["Issue"] %>" /></li>
                <li>时间范围：<input type="text" name="orderDTS" id="orderDTS" class="gd_txt gd_time" value="<%:ViewData["DTS"].ToString().Replace("-","/") %>" />-<input type="text" name="orderDTE" id="orderDTE" class="gd_txt gd_time" value="<%:ViewData["DTE"].ToString().Replace("-","/") %>" /></li>
                <li><input class="gd_sbtn ui-post-loading" value="查找" type="submit"></li>
            </ul>
        </div>
            </form>
        <div class="clear"></div>
        <div class="gd_search_selet">
            <ul>
                <li><a href="#" onclick="table_row_selecct();">全选</a></li>
                <li><a href="#" onclick="table_row_unselect();">反选</a></li>
                <li><a href="#" onclick="table_row_clear_select();">取消选中</a></li>
                <li><a href="#"  id="btn_cancel_corder">撤单</a></li>
            </ul>                        
        </div>
    </div>
    <div class="clear"></div>
    <div class="bank_list_box">
        <div class="bank_list">
            <table class="ctable_box youxi_table_list" width="100%" border="0" cellpadding="0" cellspacing="0">
                <tbody><tr class="table-pro-head">
                    <th>合买单号</th>
                    <th>账号</th>
                    <th>游戏</th>
                    <th>期号</th>
                    <th>结果</th>
                    <th>内容</th>
                    <th>总金额</th>
                    <th>每份</th>
                    <th>自己占份数</th>
                    <th>可合买份数</th>
                    <th>参与人数</th>
                    <th>时间</th>
                    <th>状态</th>
                    <th></th>
                </tr>
               <%if( null !=combuyList)
              {
                  Dictionary<int, string> dicStatus = new Dictionary<int, string>() { { 0, "进行中" }, { 1, "已满人" }, { 2, "已撤单" },{3,"已结算"} };
                  foreach(var item in combuyList)
                  {  %>
            <tr key="<%:item.so001 %>" >
                <td><a href="javascript:;" class="show_des" title="查看详情" data="<%:item.sco001 %>"><%:item.sco001 %></a>
                    <textarea style="display:none" class="dom-hide" rows="10" cols="10" id="json_<%:item.sco001 %>"><%=Newtonsoft.Json.JsonConvert.SerializeObject(item) %></textarea>
                </td>
                <td><%:item.u002.Trim() %></td>
                <td><%:gDicList[item.g001].g003 %></td>
                <td><%:item.gs002.Trim() %></td>
                <td><%:item.gs007 == null ? string.Empty : item.gs007.Trim() %></td>
                <td class="hover_link show_des" data="<%:item.sco001 %>" title="<%:item.sco013.Trim() %>"><%:NETCommon.StringHelper.GetShortString(item.sco013.Trim(), 15,15, "...") %></td>
                <td class="fc-red"><%:item.sco007.ToString("N2")%></td>
                <td><%= (item.sco007 /100.0000m).ToString("N2") %></td>
                <td><%:(int)item.sco004 %>%</td>
                <td class="fc-green"><%:item.sco011 %>%</td>
                <td><%:item.sco012 %></td>
                <td><%:item.sco017 %></td>
                <td><%:dicStatus[item.sco009] %></td>
                <td class="link-tools"><a class="show_des" href="javascript:void(0);" data="<%:item.sco001 %>">详细</a></td>
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
<div id="e_join" class="dom-hide">
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
                <td><textarea rows="3" cols="40" id="e_game_content"></textarea></td>
            </tr>
            <tr class="dom-hide">
                <td class="title">发起者</td>
                <td id="e_user"></td>
            </tr>
            <tr class="dom-hide">
                <td class="title">发起时间</td>
                <td id="e_posttime"></td>
            </tr>
            <tr>
                <td class="title">模式</td>
                <td id="e_mode"></td>
            </tr>
            <tr>
                <td class="title">总金额</td>
                <td id="e_total"></td>
            </tr>
            <tr>
                <td class="title">自己占份数</td>
                <td id="e_self"></td>
            </tr>
            <tr>
                <td class="title">自己承担金额</td>
                <td id="e_self_amt"></td>
            </tr>
            <tr>
                <td class="title">剩余份数</td>
                <td id="e_can_order" class="fc-green"></td>
            </tr>
            <tr>
                <td class="title">参与金额</td>
                <td id="e_join_total"></td>
            </tr>
            <tr>
                <td class="title">参与人数</td>
                <td id="e_join_nums" class="data"></td>
            </tr>
            <tr>
                <td class="title">剩余时间</td>
                <td id="e_countdown" class="fc-green"></td>
            </tr>
            <tr>
                <td class="title">单注倍数</td>
                <td id="e_order_time"></td>
            </tr>
            <tr>
                <td class="title">每份</td>
                <td id="e_per_sum" class="fc-red">0</td>
            </tr>
            <tr class="join_list">
                <td class="title">参与列表</td>
                <td>
                    <table class="table-pro table-noborder w100ps">
                        <thead>
                            <tr class="table-pro-head">
                                <th class="text-left">账号</th>
                                <th class="text-left">参与金额</th>
                                <th class="text-left">占总比例</th>
                                <th class="text-left">参与时间</th>
                            </tr>
                        </thead>
                        <tbody id="join_list_body">
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
        </form>
</div>
<%} %>
        	</div>
            <!---个人资料 end-->
            
        </div>
    </div>
</div>
</div>
<script type="text/javascript">
    function cancel_order(order_flag) {
        if (0 == get_table_row_keys().length) {
            alert('没有选中任何记录');
            return;
        }
        if (false == confirm('单击确认后撤单，请检查是否选中正确记录')) {
            return;
        }

        $.ajax({
            url: "/ui2/CancelOrder?type=" + order_flag + "&orderID=" + get_table_row_keys() + "&t=" + new Date(),
            cache: false,
            async: true,
            success: function (data) {

                if (data.Data == "success") {
                    alert("撤单完成");
                    refresh_current_page();
                }
                else {
                    alert(data.Data);
                }

            }
        }
        );
    }

    $(document).ready(function () {
        $(".show_ord_des").click(function () {
            $("#e_join").attr("display", "block")
            var order_id = $(this).attr("data");
            var content = $("#json_" + order_id).val();
            var obj = null;
            eval("obj = " + content + ";");
            $("#show_des").dialog({ title: "注单详细", width: 400, resizable: false, position: { my: "center", at: "center", of: window } });
            $("#s_cnname").html(obj.cnname + "/" + obj.methodname);
            $("#s_modes").html(obj.modes);
            $("#s_code").val(obj.code);
            $("#s_times_mul").html(obj.times + "/" + obj.multiple);
            $("#s_totalprice").html(obj.totalprice);
            $("#s_issue").html(obj.issue + "/" + (obj.nocode == "" ? obj.statusdesc : obj.nocode));
            $("#s_result").html(obj.bonus + "/" + obj.resultpoint);
            $("#s_time").html(obj.writetimeori);
            $("#s_ord_point").html((obj.point * 100.000).toFixed(1) + "%");
            
            try {
                var itemstr = "";
                var items = obj.canPrizeItem.split(',');
                for (var i = 0; i < items.length; i++) {
                    var names = items[i].split('|');
                    itemstr += names[0] + "\r\n";
                }
                $("#s_canp").html(itemstr);
            } catch (err) { $("#s_canp").html('动态'); }
        });
        $(".show_des").click(function () {
            var data = $(this).attr("data");
            $("#e_join").dialog({ width: 600, minHeight: 380, height: 400, title: "合买详细", modal: true, resizable: false, position: { my: "center", at: "center", of: window } });
            $.ajax({
                timeout: _global_ajax_timeout, cache: false, type: "POST", data: { method: "getCombine", key: data }, url: "/UI2/Combine", success: function (a) {
                    _check_auth(a);
                    var _robj = eval(a);
                    $("#e_game_name").html(_robj.Data.GameName.toString() + "/" + _robj.Data.GameMethod.toString());
                    $("#e_issue").html(_robj.Data.Issue + "/" + (_robj.Data.GameResult == "" ? "未开奖" : _robj.Data.GameResult));
                    $("#e_posttime").html(_robj.Data.PostTime);
                    $("#e_total").html(_robj.Data.TotalMoney);
                    $("#h_ori_money").val(_robj.Data.OriTotalMoney.toFixed(2));
                    $("#e_per_sum").html((_robj.Data.OriTotalMoney / 100.0000).toFixed(2));
                    $("#e_can_order").html(_robj.Data.CanBetNums + "%");
                    $("#e_order_time").html(_robj.Data.OrderTimes);
                    $("#e_game_content").html(_robj.Data.Code);
                    $("#e_join_total").html(_robj.Data.JoinMoney);
                    $("#e_join_nums").html(_robj.Data.JoinNums);
                    $("#e_self").html(_robj.Data.MyPercent + "%");
                    $("#e_self_amt").html((_robj.Data.OriTotalMoney - _robj.Data.JoinMoney).toFixed(2));
                    $("#e_mode").html(_robj.Data.ModeName);
                    $("#e_user").html(_robj.Data.User);
                    $("#bet_times").val("0");
                    $("#h_mode").val(_robj.Data.Mode);
                    $("#h_time").val(_robj.Data.OrderTimes);
                    $("#hash_key").val(_robj.Data.HashKey);
                    if (_robj.Data.JoinList.length > 0) {
                        $(".join_list").show();
                    }
                    else {
                        $(".join_list").hide();
                    }
                    var join_list = "";
                    for (var row = 0; row < _robj.Data.JoinList.length; row++) {
                        var r_d = _robj.Data.JoinList[row];
                        join_list += "<tr><td>" + r_d.username + "</td><td class='fc-red'>" + window.parseFloat(r_d.totalprice).toFixed(2).toString() + "</td><td>" + r_d.times + "%</td><td>" + r_d.writetimeori + "</td><tr>";

                    }
                    $("#join_list_body").html(join_list);
                }
            });
        });
        $("#btn_cancel_order").click(function () {
            cancel_order("cancelorder");
        });
        $("#btn_cancel_torder").click(function () {
            cancel_order("cancelTOrder");
        });
        $("#btn_cancel_corder").click(function () {
            cancel_order("cancelorder");
        });

        $('#orderDTS').appendDtpicker({ locale: "cn", dateFormat: "YYYY/MM/DD hh:mm" });
        $('#orderDTE').appendDtpicker({ locale: "cn", dateFormat: "YYYY/MM/DD hh:mm" });
        OrdeChange();
    });
    var count = -1;
    function OrdeChange() {
        var type = "<%=methodType%>";
            $.ajax({
                url: "/ui2/OrderChange?t=" + new Date(),
                cache: false,
                async: true,
                success: function (data) {

                    if (count == -1) {
                        count = data.Data;
                    }
                    else {
                        if (data.Data != count) {
                            count = data.Data;
                            refresh_current_page();
                        }
                    }
                }
            });
            setTimeout("OrdeChange()", 3000);
            return true;
        }
    </script>
<style>
    /*.ctable_box tr td*/
    .table-row-select,table-td-select {background:#2c7839;color:#2c7839}
</style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
