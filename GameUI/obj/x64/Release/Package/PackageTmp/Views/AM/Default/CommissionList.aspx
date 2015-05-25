<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    CommissionList
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var methodType = (string)ViewData["MethodType"];
    var forgetUser = (List<string>)ViewData["ForgetUser"];
%>
<div class="cjlsoft-body-header">
        <h1>佣金审核</h1>
        <div class="left-nav">
        </div>
    <span class="right-nav">
        <a id="cjlsoft-a-refresh" href="javascript:;" title="刷新" class="cjlsoft-post-loading">[刷新]</a><a id="cjlsoft-a-back" href="javascript:;" title="返回">[返回]</a>
    </span>
</div>
<div class="blank-line"></div>
<div class="cjlsoft-body-header tools">
    <a href="/AM/CommissionList?method=defaultOrder">佣金审核</a>
    <a href="/AM/CommissionList?method=CommissionOrder">佣金查询</a>
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
      var umList = (List<DBModel.wgs014>)ViewData["UserMoneyList"];
      var orderUserName = (string)ViewData["OrderUserName"];
      var orderShowList = (List<DBModel.LotteryHistoryOrder>)ViewData["OrderShowList"];

              var betSum = 0.0000m;
              var winSum = 0.0000m;
              var pointSum = 0.0000m;
%>
<div class="xtool">
    <form action="/AM/CommissionList" id="form_order_default" method="get">

        
    </form>
</div>
<div class="xtool">
    <input type="button" value="全选" class="btn-normal" onclick="table_row_selecct();" /><input id="btnCommissionList" class="cjlsoft-post-loading" type="button" name="btnCommissionList" value="佣金处理" />(不计算当天返送的佣金)
</div>
<table class="table-pro w100ps keep-line table-color-row">
    <thead>
        <tr class="table-pro-head">
            <th class="w100px">编号</th>
            <th class="w100px">账号</th>
            <th>佣金</th>
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
            <th>佣金</th>
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
        <tr key="<%:item.projectid %>" class="notselect">
            <td><%:item.projectid %></td>
            <td><%:item.username %></td>
            <td><%foreach (var um in umList)
              {
                  if (item.userId == um.u001)
                  {
                 %>
                      <%:um.uf012%>
                <%
                      continue;
                  }
              }%>
            </td>
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
            <td><%if (item.commission == false) { Response.Write("未处理"); } else { Response.Write("已处理"); } %></td>
        </tr>
        <%}
          }/*if*/ %>
    </tbody>
    
</table>
    <%=ViewData["PageList"] %>
<%}/*defaultOrder*/ %>
<%else if ("CommissionOrder" == methodType)
  { %>
    <%
      var gcList = (List<DBModel.wgs006>)ViewData["GameClassList"];
      var gList = (List<DBModel.wgs001>)ViewData["GameList"];
      var gmList = (List<DBModel.wgs002>)ViewData["GameMethodList"];
      var gDicList = gList.ToDictionary(key => key.g001);
      var orderGame = (int)ViewData["OrderGame"];
      var orderStatus = (int)ViewData["OrderStatus"];
      var orderUserType = (int)ViewData["OrderUserType"];
      
      var orderUserName = (string)ViewData["OrderUserName"];
      var COrderDayList = (List<DBModel.CommissionOrderDay>)ViewData["CommissionOrder"];
              
    %>
<form action="/AM/CommissionList?method=CommissionOrder" id="form_order_trace" method="get">
    <input type="hidden" name="method" value="<%:methodType %>" />
        账号
        <input type="text" class="input-text w80px" name="orderUserName" id="orderUserName" value="<%:orderUserName %>" />
        时间范围<input type="text" name="orderDTS" id="orderDTS" class="input-text w80px" value="<%:ViewData["DTS"].ToString().Replace("-","/") %>" />-<input type="text" name="orderDTE" id="orderDTE" class="input-text w80px" value="<%:ViewData["DTE"].ToString().Replace("-","/") %>" />
        <input type="submit" value="查找" class="btn-normal ui-post-loading" />
    </form>
    <div class="blank-line"></div>

    <table class="table-pro tp3 table-color-row" width="100%">
            <thead>
        <tr class="table-pro-head">
            <th>编号</th>
            <th>账号</th>
            <th>父级</th>
            <th>父父级</th>
            <th>父父父级</th>
            <th>父级佣金</th>
            <th>父父级佣金</th>
            <th>父父父级佣金</th>
            <th>当天消费</th>
            <th>当天亏损</th>
            <th class="w120px">时间</th>
            <th>状态</th>
            <th class="dom-hide"></th>
        </tr>
    </thead>
    <tbody>
        <%if (null != COrderDayList)
          {
              foreach (var item in COrderDayList)
              {
               %>
        <tr class="notselect">
            <td><%:item.UserID %></td>
            <td><%:item.UserName %></td>
            <td><%:item.level1 %></td>
            <td><%:item.level2 %></td>
            <td><%:item.level3 %></td>
            <td><%:item.level1Commission %></td>
            <td><%:item.level2Commission %></td>
            <td><%:item.level3Commission %></td>
            <td><%:item.totleMoneyCons %></td>
            <td><%:item.totleMoneyLoss %></td>
            <td><%:item.Day %></td>

            <td>已处理</td>
        </tr>
        <%}
          }/*if*/ %>
    </tbody>
        <tfoot>
        </tfoot>
    </table>
    <%=ViewData["PageList"] %>
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

        $(".notselect").unbind("click")
        $("#btnCommissionList").click(function () {
            if (0 == get_table_row_keys().length) {
                alert('没有选中任何记录');
                return;
            }
            if (false == confirm('单击确认后,以1天为单位消费和亏损金额，自动返送佣金。')) {
                return;
            }
            $.ajax({
                timeout: _global_ajax_timeout, cache: false, type: "POST", data: { id: get_table_row_keys(), orderDTS: $("#orderDTS").val(), orderDTE: $("#orderDTE").val() }, url: "/AM/CommissionList?method=Commission", success: function (a) {
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
    });
</script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
