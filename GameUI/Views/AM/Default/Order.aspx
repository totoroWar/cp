<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">订单</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var methodType = (string)ViewData["MethodType"];
    var forgetUser = (List<string>)ViewData["ForgetUser"];
%>
<div class="cjlsoft-body-header">
        <h1>订单</h1>
        <div class="left-nav">
        </div>
    <span class="right-nav">
        <a id="cjlsoft-a-refresh" href="javascript:;" title="刷新" class="cjlsoft-post-loading">[刷新]</a><a id="cjlsoft-a-back" href="javascript:;" title="返回">[返回]</a>
    </span>
</div>
<div class="blank-line"></div>
<div class="cjlsoft-body-header tools">
    <a href="/AM/Order?method=defaultOrder">普通订单</a>
    <a href="/AM/Order?method=traceOrder">追号订单</a>
    <a href="/AM/Order?method=combineOrder">合买订单</a>
    <a href="/AM/Order?method=manageOrder">多功能撤单</a>
</div>
<div class="blank-line"></div>
<%if( "defaultOrder" == methodType){ %>
<%
      var gcList = (List<DBModel.wgs006>)ViewData["GameClassList"];
      var gList = (List<DBModel.wgs001>)ViewData["GameList"];
      var gmList = (List<DBModel.wgs002>)ViewData["GameMethodList"];
      var gDicList = gList.ToDictionary(key => key.g001);
      var gmDicList = gmList.ToDictionary(key => key.gm001);
      var orderList = (List<DBModel.wgs045>)ViewData["OrderList"];
      var orderGame = (int)ViewData["OrderGame"];
      var orderGameClass = (int)ViewData["OrderGameClass"];
      var orderMethod = (int)ViewData["OrderGameMethod"];
      var orderCancel = (int)ViewData["OrderCancel"];
      var orderIsWin = (int)ViewData["OrderIsWin"];
      var orderAmountQ = (int)ViewData["OrderAmountQ"];
      var orderAmount = (decimal)ViewData["OrderAmount"];
      var orderWinAmountQ = (int)ViewData["OrderWinAmountQ"];
      var orderWinAmount = (decimal)ViewData["OrderWinAmount"];
      var orderUserType = (int)ViewData["OrderUserType"];
      var orderModes = (int)ViewData["OrderModes"];
      var orderID = (long)ViewData["OrderID"];
      var orderIssue = (string)ViewData["OrderIssue"];
      var userName = (string)ViewData["OrderUserName"];
      var orderShowList = (List<DBModel.LotteryHistoryOrder>)ViewData["OrderShowList"];
              var betSum = 0.0000m;
              var winSum = 0.0000m;
              var pointSum = 0.0000m;
%>
<div class="xtool">
    <form action="/AM/Order" id="form_order_default" method="get">
        游戏
        <input type="hidden" name="orderGameClass" value="<%:ViewData["orderGameClass"] %>" />
        <select name="orderGame" class="drop-select-to-url w100px">
            <option value="0" title="所有" tourl="/AM/Order?method=defaultOrder&orderGameClass=0&orderGame=0">所有</option>
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
            <option value="<%:gameItem %>" <%:orderGame==int.Parse(gameItem) ? "selected='selected'" : "" %> tourl="/AM/Order?method=defaultOrder&orderGameClass=<%:gc.gc001 %>&orderGame=<%:gameItem %>"><%:gDicList[int.Parse(gameItem)].g003.Trim() %></option>
            <%
                  }/*game*/
              }
            %>
        </select>
        玩法
        <select name="orderMethod" id="orderMethod" class="w80px">
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
        模式
        <select name="orderModes" id="orderModes">
            <option value="0" title="所有">所有</option>
            <option value="1" <%:orderModes == 1? "selected='selected'" : "" %>>元</option>
            <option value="2" <%:orderModes == 2? "selected='selected'" : "" %>>角</option>
            <option value="3" <%:orderModes == 3? "selected='selected'" : "" %>>分</option>
        </select>
        单号<input type="text" class="input-text w80px"  name="orderID" id="orderID" value="<%:orderID == 0 ? "" : orderID.ToString() %>" />
        期号<input type="text" class="input-text w80px" name="orderIssue" id="orderIssue" value="<%:orderIssue %>" />
        每页记录数<input type="text" class="input-text w30px" name="pageSize" value="<%:ViewData["PageSize"] %>" />
        <div class="blank-line"></div>
        账号<input name="userName" type="text" class="input-text w80px" value="<%:userName %>" />
        时间<input type="text" name="orderDTS" id="orderDTS" class="input-text w80px" value="<%:ViewData["DTS"] %>" />-<input type="text" name="orderDTE" id="orderDTE" class="input-text w80px" value="<%:ViewData["DTE"] %>" />
        中奖
        <select name="orderIsWin" id="orderIsWin">
            <option value="0" <%:orderIsWin == 0 ? "selected='selected'" : "" %>>所有</option>
            <option value="1" <%:orderIsWin == 1 ? "selected='selected'" : "" %>>已中奖</option>
            <option value="2" <%:orderIsWin == 2 ? "selected='selected'" : "" %>>未中奖</option>
        </select>
        金额
        <select name="orderAmountQ" id="orderAmountQ">
            <option value="0" <%:orderAmountQ == 0 ? "selected='selected'" : "" %>>所有</option>
            <option value="1" <%:orderAmountQ == 1 ? "selected='selected'" : "" %>>小于</option>
            <option value="2" <%:orderAmountQ == 2 ? "selected='selected'" : "" %>>等于</option>
            <option value="3" <%:orderAmountQ == 3 ? "selected='selected'" : "" %>>大于</option>
        </select>
        <input name="orderAmount" type="text" class="input-text w50px" value="<%:orderAmount %>" />
        中奖
        <select name="orderWinAmountQ" id="orderWinAmountQ">
            <option value="0" <%:orderWinAmountQ == 0 ? "selected='selected'" : "" %>>所有</option>
            <option value="1" <%:orderWinAmountQ == 1 ? "selected='selected'" : "" %>>小于</option>
            <option value="2" <%:orderWinAmountQ == 2 ? "selected='selected'" : "" %>>等于</option>
            <option value="3" <%:orderWinAmountQ == 3 ? "selected='selected'" : "" %>>大于</option>
        </select>
        <input name="orderWinAmount" type="text" class="input-text w50px" value="<%:orderWinAmount %>" />
        撤单
        <select name="orderCancel" id="orderCancel">
            <option value="-1">所有</option>
            <option value="0" <%:orderCancel == 0 ? "selected='selected'" : "" %>>没有撤单</option>
            <option value="1" <%:orderCancel == 1 ? "selected='selected'" : "" %>>本人撒单</option>
            <option value="2" <%:orderCancel == 2 ? "selected='selected'" : "" %>>管理员撤单</option>
            <option value="3" <%:orderCancel == 3 ? "selected='selected'" : "" %>>开错奖撤单</option>
            <option value="4" <%:orderCancel == 4 ? "selected='selected'" : "" %>>已删除</option>
        </select>
        <input type="submit" value="查找" class="btn-normal ui-post-loading" />
    </form>
</div>
<div class="blank-line"></div>
<div class="xtool">
    <input type="button" value="全选" class="btn-normal" onclick="table_row_selecct();" /><input type="button" value="返选" class="btn-normal" onclick="table_row_unselect();" /><input type="button" value="取消选中" class="btn-normal" onclick="table_row_clear_select();" /><input type="button" value="撤单" class="btn-normal btn_cancel f_cancel" /><input type="button" value="删除" class="btn-normal btn_cancel f_delete" /><input type="button" value="重新结算" class="btn-normal" /><input type="button" value="显示切换" class="show-table-all-col btn-normal" />
</div>
<table class="table-pro w100ps keep-line table-color-row">
    <thead>
        <tr class="table-pro-head">
            <th class="w100px">编号</th>
            <th class="w100px">账号</th>
            <th>游戏</th>
            <th>期号</th>
            <th>玩法</th>
            <th class="hide-col-eml dom-hide">开奖结果</th>
            <th class="w30px">倍/注</th>
            <th class="w80px">金额</th>
            <th class="w80px">奖金</th>
			<th class="w80px">输赢</th>
            <th class="w80px">返点</th>
            <th>投注返点</th>
            <th class="hide-col-eml dom-hide">内容</th>
            <th>模式</th>
            <th class="w120px">时间</th>
            <th>位置</th>
            <th>状态</th>
            <th class="dom-hide"></th>
        </tr>
    </thead>
    <tbody>
        <%if( null != orderShowList)
          {              
              foreach(var item in orderShowList)
              {
                  var fgCount = forgetUser.Count(exp => exp == item.username.Trim());
                  if (0 == fgCount)
                  {
                      betSum += item.totalprice;
                      winSum += item.bonus;
                      pointSum += item.resultpoint;
                  }
                  else
                  {
                      item.username += "（不计算）"; 
                  }
               %>
        <tr key="<%:item.projectid %>">
            <td><%:item.projectid %></td>
            <td><%:item.username %></td>
            <td><%:item.cnname %><%=item.taskid != 0 ? "<span class='o_trace' title='追号' data='"+item.taskid+"'>[追号]</span>" : ""  %><%=item.combineOrderID != 0 && item.combineType == 0 ? "<span class='o_combuy' title='发起合买' data='"+item.combineOrderID+"'>[发起合买]</span>" : "" %><%=item.combineOrderID != 0 && item.combineType == 1 ? "<span class='o_jcombuy' title='参与合买' data='"+item.combineOrderID+"'>[参与合买]</span>" : "" %></td>
            <td><%:item.issue.Trim() %></td>
            <td><%:item.methodname %></td>
            <td class="hide-col-eml dom-hide"><%:item.nocode %></td>
            <td><%:item.multiple %>/<%:item.times %></td>
            <td><%:item.totalprice %></td>
            <td><%:item.bonus %></td>
			<td class="<%=item.bonus-item.totalprice > 0 ? "fc-red" : "fc-green" %>"><%:item.bonus-item.totalprice %></td>
            <td><%:item.resultpoint %></td>
            <td><%:(item.point * 100.0m).ToString("N1") %></td>
            <td class="hide-col-eml dom-hide"><textarea class="input-text" rows="6" cols="30"><%:item.code %></textarea></td>
            <td><%:item.modes %></td>
            <td><%:item.writetimeori %></td>
            <td>
                <%
                  var str = new string[]{"万", "千", "百", "十", "个"};
                  for (var i = 0; i < 5; i++)
                  {
                      item.pos = item.pos.Replace(i.ToString(), str[i]);
                  }
                %>
                <%:item.pos%></td>
            <%
                  var statusClass = "";
                  switch (item.statusdesc)
                  {
                      case "已派奖":
                          statusClass = "fc-red";
                          break;
                      case "未中奖":
                          statusClass = "fc-green";
                          break; 
                      case "未开奖":
                          statusClass = "fc-blue";
                          break;
                      default:
                          statusClass = "fc-delete";
                          break;
                  }
            %>
            <td class="<%=statusClass %>"><%:item.statusdesc %></td>
            <td class="link-tools dom-hide"><a href="#">撤单</a><a href="#">删除</a><a href="#">重新结算</a></td>
        </tr>
        <%}
          }/*if*/ %>
    </tbody>
    <tfoot>
        <tr>
            <td>合计</td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td class="hide-col-eml dom-hide"></td>
            <td></td>
            <td><%=betSum.ToString("N4")%></td>
            <td><%=winSum.ToString("N4") %></td>
			<td class="<%=winSum-betSum > 0 ? "fc-red" : "fc-green" %>"><%:(winSum-betSum).ToString("N4") %></td>
            <td><%=pointSum.ToString("N4") %></td>
            <td class="hide-col-eml dom-hide"></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td class="link-tools dom-hide"></td>
        </tr>
    </tfoot>
</table>
    <%=ViewData["PageList"] %>
    <script type="text/javascript">
        function cancel_order(t) {
            if (0 == get_table_row_keys().length) {
                alert('没有选中任何记录');
                return;
            }
            var op_t = "none";
            var op_tv = "";
            switch (t)
            {
                case 0:
                    op_t = "cancel";
                    op_tv = "撤单";
                    break;
                case 1:
                    op_t = "delete";
                    op_tv = "删除";
                    break;
            }
            if (false == confirm('单击确认后'+op_tv+'，请检查是否选中正确记录')) {
                return;
            }
            $.ajax({
                timeout: _global_ajax_timeout, cache: false, type: "POST", data: { id: get_table_row_keys() }, url: "/AM/Order?method=" + op_t, success: function (a) {
                    _check_auth(a);
                    var _robj = eval(a);
                    if (0 == _robj.Code)
                    {
                        alert(_robj.Message);
                    }
                    else if (1 == _robj.Code)
                    {
                        refresh_current_page();
                    }
                }
            });
        }
        $(document).ready(function () {
            $(".btn_cancel").click(function ()
            {
                var is_cancel = $(this).hasClass("f_cancel");
                cancel_order(is_cancel == true ? 0 : 1);
            });
        });
    </script>
<%}/*defaultOrder*/ %>
<%else if( "traceOrder" == methodType){ %>
<%
      var gcList = (List<DBModel.wgs006>)ViewData["GameClassList"];
      var gList = (List<DBModel.wgs001>)ViewData["GameList"];
      var gmList = (List<DBModel.wgs002>)ViewData["GameMethodList"];
      var gDicList = gList.ToDictionary(key => key.g001);
      var orderGame = (int)ViewData["OrderGame"];
      var orderStatus = (int)ViewData["OrderStatus"];
      var orderUserType = (int)ViewData["OrderUserType"];
      var orderUserName = (string)ViewData["OrderUserName"];
      var tOrderList = (List<DBModel.wgs030>)ViewData["TOrderList"];
%>
<form action="/AM/Order?method=orderTrace" id="form_order_trace" method="get">
        游戏
        <input type="hidden" name="orderGameClass" value="<%:ViewData["orderGameClass"] %>" />
        <input type="hidden" name="method" value="<%:methodType %>" />
        <select name="orderGame" class="drop-select-to-url w100px">
            <option value="0" title="所有" tourl="/AM/Order?method=traceOrder&orderGameClass=0&orderGame=0">所有</option>
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
            <option value="<%:gameItem %>" <%:orderGame==int.Parse(gameItem) ? "selected='selected'" : "" %> tourl="/AM/Order?method=traceOrder&orderGameClass=<%:gc.gc001 %>&orderGame=<%:gameItem %>"><%:gDicList[int.Parse(gameItem)].g003.Trim() %></option>
            <%
                  }/*game*/
              }
            %>
        </select>
        账号
        <input type="text" class="input-text w80px" name="orderUserName" id="orderUserName" value="<%:orderUserName %>" />
        单号<input type="text" class="input-text w80px"  name="orderID" id="orderID" value="" />
        时间范围<input type="text" name="orderDTS" id="orderDTS" class="input-text w80px" value="<%:ViewData["DTS"].ToString().Replace("-","/") %>" />-<input type="text" name="orderDTE" id="orderDTE" class="input-text w80px" value="<%:ViewData["DTE"].ToString().Replace("-","/") %>" />
        <input type="submit" value="查找" class="btn-normal ui-post-loading" />
    </form>
    <div class="blank-line"></div>
    <div class="xtool">
<input type="button" value="全选" class="btn-normal" onclick="table_row_selecct();" /><input type="button" value="返选" class="btn-normal" onclick="table_row_unselect();" /><input type="button" value="取消选中" class="btn-normal" onclick="table_row_clear_select();" /><input type="button" value="撤单" class="btn-normal btn_cancel f_cancel" />
    </div>
    <table class="table-pro tp3 table-color-row" width="100%">
        <thead>
            <tr class="table-pro-head">
                <th class="w100px">单号</th>
                <th class="w100px">账号</th>
                <th>游戏</th>
                <th>起始期号</th>
                <th>结束期号</th>
                <th>总金额</th>
                <th>实际金额</th>
                <th>奖金</th>
                <th>时间</th>
                <th>状态</th>
                <th>类型</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
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
                <td><%:item.sto012.Trim() %></td>
                <td><%:item.sto002.ToString("N2") %></td>
                <td><%:item.sto007.ToString("N2") %></td>
                <td><%:item.sto008.ToString("N2") %></td>
                <td><%:item.sto004.ToString("yyyy-MM-dd HH:mm:ss") %></td>
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
                <td class="link-tools"><a href="javascript:window.parent.parent.ui_show_tab('<%:item.sto010.Trim() %>_<%:item.sto001 %>追号','/AM/Order?method=traceOrderDetail&key=<%:item.sto001 %>',true);" title="详细">详细</a><a href="javascript:;" name="cancel_order" data="<%:item.sto001 %>">撤单</a></td>
            </tr>
            <%}
              }/*if*/ %>
        </tbody>
        <tfoot>
        </tfoot>
    </table>
    <%=ViewData["PageList"] %>
<%} %>
<%else if("combineOrder" == methodType)
  {
      var gcList = (List<DBModel.wgs006>)ViewData["GameClassList"];
      var gList = (List<DBModel.wgs001>)ViewData["GameList"];
      var gmList = (List<DBModel.wgs002>)ViewData["GameMethodList"];
      var gDicList = gList.ToDictionary(key => key.g001);
      var orderGame = (int)ViewData["OrderGame"];
      var combuyList = (List<DBModel.wgs031>)ViewData["CombuyList"];      
       %>
    <div class="xtool">
        <form action="/AM/Order?method=combineOrder" id="form_order_combuy" method="get">
        游戏
        <input type="hidden" name="orderGameClass" value="<%:ViewData["orderGameClass"] %>" />
        <input type="hidden" name="method" value="<%:methodType %>" />
        <select name="orderGame" class="drop-select-to-url w100px">
            <option value="0" title="所有" tourl="/AM/Order?method=combineOrder&orderGameClass=0&orderGame=0">所有</option>
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
            <option value="<%:gameItem %>" <%:orderGame==int.Parse(gameItem) ? "selected='selected'" : "" %> tourl="/AM/Order?method=combineOrder&orderGameClass=<%:gc.gc001 %>&orderGame=<%:gameItem %>"><%:gDicList[int.Parse(gameItem)].g003.Trim() %></option>
            <%
                  }/*game*/
              }
            %>
        </select>
        账号<input type="text" class="input-text w100px" name="account" value="<%:ViewData["Account"] %>" />
        期号<input type="text" class="input-text w100px" name="issue" value="<%:ViewData["Issue"] %>" />
        时间范围<input type="text" name="orderDTS" id="orderDTS" class="input-text w80px" value="<%:ViewData["DTS"].ToString().Replace("-","/") %>" />-<input type="text" name="orderDTE" id="orderDTE" class="input-text w80px" value="<%:ViewData["DTE"].ToString().Replace("-","/") %>" />
        <input type="submit" value="查找" class="btn-normal ui-post-loading" />
    </form>
    </div>
    <div class="blank-line"></div>
    <div class="xtool">
    <input type="button" value="全选" class="btn-normal" onclick="table_row_selecct();" /><input type="button" value="返选" class="btn-normal" onclick="    table_row_unselect();" /><input type="button" value="取消选中" class="btn-normal" onclick="    table_row_clear_select();" /><input id="btn_cancel_corder" type="button" value="撒单" class="btn-normal"  />
    </div>
    <table class="table-pro table-color-row tp5" width="100%">
        <thead>
            <tr class="table-pro-head">
                <th class="w100px">合买单号</th>
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
                <th>发起时间</th>
                <th>状态</th>
                <th class="dom-hide"></th>
            </tr>
        </thead>
        <tbody>
            <%if( null !=combuyList)
              {
                  Dictionary<int, string> dicStatus = new Dictionary<int, string>() { { 0, "进行中" }, { 1, "已满人" }, { 2, "已撤单" },{3,"已结算"} };
                  foreach(var item in combuyList)
                  {  %>
            <tr key="<%:item.so001 %>">
                <td><%:item.sco001 %></td>
                <td><%:item.u002.Trim() %></td>
                <td><%:gDicList[item.g001].g003 %></td>
                <td><%:item.gs002.Trim() %></td>
                <td><%:item.gs007 == null ? string.Empty : item.gs007.Trim() %></td>
                <td title="<%:item.sco013.Trim() %>"><%:NETCommon.StringHelper.GetShortString(item.sco013.Trim(), 15,15, "...") %></td>
                <td class="fc-red"><%:item.sco007.ToString("N2")%></td>
                <td><%= (item.sco007 /100.0000m).ToString("N2") %></td>
                <td><%:(int)item.sco004 %>%</td>
                <td class="fc-green"><%:item.sco011 %>%</td>
                <td><%:item.sco012 %></td>
                <td><%:item.sco017 %></td>
                <td><%=dicStatus[item.sco009] %></td>
                <td class="link-tools dom-hide"><a href="javascript:void(0);">详细</a></td>
            </tr>
            <%}
              } %>
        </tbody>
    </table>
    <%=ViewData["PageList"] %>
<%} %>
<%else if("manageOrder" == methodType){ %>
<form action="#" id="form_function">
    <table class="table-pro w100ps">
    <tr>
        <td class="title">功能</td>
        <td>
            <label><input type="radio" name="r_op" class="input-text" value="order_ids" checked="checked" />订单编号</label>
            <label><input type="radio" name="r_op" class="input-text" value="order_account" />下单账号</label>
            <label><input type="radio" name="r_op" class="input-text" value="order_issue" />游戏期号</label>
        </td>
    </tr>
    <tr>
        <td class="title">动作</td>
        <td>
            <label><input type="radio" name="r_op_type" class="input-text" value="cancel" checked="checked" />撤单</label>
            <label><input type="radio" name="r_op_type" class="input-text" value="delete" />删除</label>
        </td>
    </tr>
    <tr>
        <td class="title">内容</td>
        <td>
            <textarea rows="10" cols="80" class="input-text" name="r_content"></textarea>
        </td>
    </tr>
    <tr>
        <td class="title"></td>
        <td>
            <input type="button" value="执行" class="btn-normal" id="btn_run" />
        </td>
    </tr>
</table>
</form>
<div id="result" class="dom-hide">
    <p class="text-cent" id="result_cont"></p>
</div>
<%} %>
<script type="text/javascript">
    $(document).ready(function () {
        jQuery('#orderDTS').datetimepicker({
            format: 'Y/m/d',
            lang: "ch",
            onShow: function (ct) {
                this.setOptions({
                    maxDate: jQuery('#orderDTE').val() ? jQuery('#orderDTE').val() : false
                })
            },
            timepicker: false
        });
        jQuery('#orderDTE').datetimepicker({
            format: 'Y/m/d',
            lang: "ch",
            onShow: function (ct) {
                this.setOptions({
                    minDate: jQuery('#orderDTS').val() ? jQuery('#orderDTS').val() : false
                })
            },
            timepicker: false
        });

        function cancel_trace_order(ids)
        {
            if (0 == ids.length)
            {
                alert("没有编号数据");
                return;
            }
            if (false == confirm("是否撤销本单追号所有订单？"))
            {
                return;
            }
            $.ajax({
                timeout: _global_ajax_timeout, cache: false, type: "POST", data: {id: ids }, url: "/AM/Order?method=cancelTOrder", success: function (a)
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
        }

        function cancel_corder() {
            if (0 == get_table_row_keys().length) {
                alert('没有选中任何记录');
                return;
            }
            if (false == confirm('单击确认后撤单，请检查是否选中正确记录')) {
                return;
            }
            $.ajax({
                timeout: _global_ajax_timeout, cache: false, type: "POST", data: {id: get_table_row_keys() }, url: "/AM/Order?method=cancel", success: function (a) {
                    _check_auth(a);
                    var _robj = eval(a);
                    if (0 == _robj.Code) {
                        alert(_robj.Message);
                    }
                    else(1 == _robj.Code)
                    {
                        refresh_current_page();
                    }
                }
            });
        }

        $("a[name='cancel_order']").click(function ()
        {
            cancel_trace_order($(this).attr("data"));
        });
        $(".btn_cancel").click(function ()
        {
            cancel_trace_order(get_table_row_keys());
        });
        $("#btn_cancel_corder").click(function () {
            cancel_corder();
        });
        $("#btn_run").click(function ()
        {
            $.ajax({
                timeout: _global_ajax_timeout, cache: false, type: "POST", data: $("#form_function").serialize(), url: "/AM/Order?method=manageOrder", success: function (a) {
                    _check_auth(a);
                    var _robj = eval(a);
                    if (0 == _robj.Code) {
                        alert(_robj.Message);
                    }
                    else {
                        alert(_robj.Message);
                    }
                }
            });
        });
    });
</script>
</asp:Content>