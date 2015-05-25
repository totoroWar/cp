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
    <a href="/AM/CommissionList?method=CommissionList">佣金查询</a>
    <a href="/AM/CommissionList?method=SendMessage">发送消息</a>
</div>
<div class="blank-line"></div>
<%if( "defaultOrder" == methodType){ %>
        <%
                                       var List = (List<DBModel.OrderDayAccSumMoney>)ViewData["OrderDay"];
%>
<div class="xtool">
    <form action="/AM/CommissionList" id="form_order_default" method="get">
        <input type="hidden" name="method" value="<%:methodType %>" />
        时间范围<input type="text" name="orderDTS" id="orderDTS" class="input-text w80px" value="<%:ViewData["DTS"].ToString().Replace("-","/") %>"  />
        <input type="submit" value="查找" class="btn-normal ui-post-loading" />
    </form>
</div>
<div class="xtool">
    <input type="button" value="全选" class="btn-normal" onclick="table_row_selecct();" /><input id="btnCommissionList" type="button" name="btnCommissionList" value="佣金处理" />
</div>
<table class="table-pro w100ps keep-line table-color-row">
    <thead>
        <tr class="table-pro-head">
            <th class="w100px">ID</th>
            <th class="w100px">账号</th>
            <th>当天消费的金额</th>
            <th>当天奖金的金额</th>
            <th>当天系统送金额</th>
            <th>当天亏损的金额</th>
            <th class="dom-hide"></th>
        </tr>
    </thead>
    <tbody>
        <%if( null != List)
          {              
              foreach(var item in List)
              {
               %>
        <tr key="<%:item.id+"|"+item.ordermoney+"|"+item.lostmoney%>" class="notselect">
            <td><%:item.id %></td>
            <td><%:item.name %></td>
            <td><%:item.ordermoney%></td>
            <td><%:item.winmoney%></td>
            <td><%:item.sendmoney%></td>
            <td><%:item.lostmoney%></td>
        </tr>
        <%}
          }/*if*/ %>
    </tbody>
    
</table>
    
<%}/*defaultOrder*/ %>
<%else if ("CommissionList" == methodType)
  { %>
    <%
      var UserName = (string)ViewData["UserName"];
      var CommissionList = (List<DBModel.wgs057>)ViewData["CommissionList"];
      
    %>
<form action="/AM/CommissionList?method=CommissionList" id="form_order_trace" method="get">
    <input type="hidden" name="method" value="<%:methodType %>" />
        账号
        <input type="text" class="input-text w80px" name="UserName" id="UserName" value="<%:UserName %>" />
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
            <th>操作人</th>
            <th>状态</th>
        </tr>
    </thead>
    <tbody>
        <%if (null != CommissionList)
          {
              foreach (var item in CommissionList)
              {
               %>
        <tr key="<%:item.c001 %>" class="notselect">
            <td><%:item.c001 %></td>
            <td><%:item.u001 %></td>
            <td><%:item.u002 %></td>
            <td><%:item.u003 %></td>
            <td><%:item.u004 %></td>
            <td>原<%:item.cm001.ToString("0") %>+消<%:item.cm002.ToString("0") %>+亏<%:item.cm003.ToString("0") %>=<%:(item.cm001+item.cm002+item.cm003).ToString("0") %></td>
            <td>原<%:item.cm004.ToString("0") %>+消<%:item.cm005.ToString("0") %>+亏<%:item.cm006.ToString("0") %>=<%:(item.cm004+item.cm005+item.cm006).ToString("0") %></td>
            <td>原<%:item.cm007.ToString("0") %>+消<%:item.cm008.ToString("0") %>+亏<%:item.cm009.ToString("0") %>=<%:(item.cm007+item.cm008+item.cm009).ToString("0") %></td>
            <td><%:item.c003 %></td>
            <td><%:item.c002 %></td>
            <td><%:item.c004 %></td>
            <td><%:item.c006 %></td>
            <td>
                <%if (item.c007 == false)
                  {
                      Response.Write("未发送");
                  }if(item.c007 == null)
                  {
                      Response.Write("已发送");
                  } if (item.c007 == true)
                  {
                      Response.Write("已发送");
                  }%>
                </td>
        </tr>
        <%}
          }/*if*/ %>
    </tbody>
        <tfoot>
        </tfoot>
    </table>
    <%=ViewData["PageList"] %>
<%}/*CommissionList*/  %>
<%else if ("SendMessage" == methodType)
  { %>
    <%
      var UserName = (string)ViewData["UserName"];
      var CDDaySendMessage = (List<DBModel.CommissionDaySendMessage>)ViewData["CDDaySendMessage"];
      
    %>
<form action="/AM/CommissionList?method=SendMessage" id="form_order_trace" method="get">
    <input type="hidden" name="method" value="<%:methodType %>" />
        时间范围<input type="text" name="orderDTS" id="orderDTS" class="input-text w80px" value="<%:ViewData["DTS"].ToString().Replace("-","/") %>" />
        <input type="submit" value="查找" class="btn-normal ui-post-loading" />
    </form>
    <div class="blank-line"></div>
    <div class="xtool">
    <input type="button" value="全选" class="btn-normal" onclick="table_row_selecct();" /><input id="btnSendMessage"  type="button" name="btnSendMessage" value="发送消息" />
</div>
    <table class="table-pro tp3 table-color-row" width="100%">
            <thead>
        <tr class="table-pro-head">
            <th>账号</th>
            <th>当天消费</th>
            <th>当天亏损</th>
            <th>消费返佣金</th>
            <th>亏损返佣金</th>
            <th>今天返的佣金</th>
        </tr>
    </thead>
    <tbody>
        <%if (null != CDDaySendMessage)
          {
              foreach (var item in CDDaySendMessage)
              {
               %>
        <tr key="<%:item.name+"|"+item.DayConsumeMoney+"|"+item.DayLossMoney+"|"+item.DayConsume+"|"+item.DayLoss%>" class="notselect">
            <td><%:item.name %></td>
            <td><%:item.DayConsumeMoney %></td>
            <td><%:item.DayLossMoney %></td>
            <td><%:item.DayConsume %></td>
            <td><%:item.DayLoss %></td>
            <td><%:item.DayConsume+item.DayLoss %></td>
        </tr>
        <%}
          }/*if*/ %>
    </tbody>
        <tfoot>
        </tfoot>
    </table>
<%}/*SendMessage*/  %>
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

            if (false == confirm('单击确认后,自动返送佣金。')) {
                return;
            }
            $('#btnCommissionList').attr('disabled', 'disabled');
            $.ajax({
                timeout: _global_ajax_timeout, cache: false, type: "POST", data: { data: get_table_row_keys(), orderDTS: $("#orderDTS").val(), orderDTE: $("#orderDTE").val() }, url: "/AM/CommissionList?method=defaultOrder", success: function (a) {
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

        $("#btnSendMessage").click(function () {
            if (get_table_row_keys().length <= 0)
            {
                alert("请选择数据！");
                return;
            }
            if (false == confirm('单击确认后,发送消息。')) {
                return;
            }
            $('#btnSendMessage').attr('disabled', 'disabled');
            $.ajax({
                timeout: _global_ajax_timeout, cache: false, type: "POST", data: { data: get_table_row_keys(), orderDTS: $("#orderDTS").val(), orderDTE: $("#orderDTE").val() }, url: "/AM/CommissionList?method=SendMessage", success: function (a) {
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
