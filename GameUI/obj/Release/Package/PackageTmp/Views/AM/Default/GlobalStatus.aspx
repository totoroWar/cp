<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    GlobalStatus
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="cjlsoft-body-header">
        <h1>系统概况</h1>
        <div class="left-nav">
        </div>
    <span class="right-nav">
        <a id="cjlsoft-a-refresh" href="javascript:;" title="刷新" class="cjlsoft-post-loading">[刷新]</a><a id="cjlsoft-a-back" href="javascript:;" title="返回">[返回]</a>
    </span>
</div>
<div class="blank-line"></div>
<div class="cjlsoft-body-header tools">
    <a href="/AM/Notify?method=finance">财务</a>
    <a href="/AM/Notify?method=game">游戏</a>
    <a href="/AM/Notify?method=online">会员</a>
</div>
<%
    var methodType = (string)ViewData["MethodType"];
%>
<div class="blank-line"></div>

<% if ("finance" == methodType)
   {
       var crList = (List<DBModel.ChargeReport>)ViewData["CRList"];
       var wdList = (List<DBModel.WithDrawReport>)ViewData["WDList"];
       crList = crList.OrderByDescending(exp => exp.uc003).ToList();
       wdList = wdList.OrderByDescending(exp => exp.SumMoney).ToList();
       var dataString = string.Empty;
       var dataStringWD = string.Empty;
       foreach (var item in crList)
       {
           dataString += string.Format("['{0}',{1}],", item.ChargeTypeName, item.uc003); 
       }
       if (false == string.IsNullOrEmpty(dataString))
       {
           dataString = dataString.Substring(0, dataString.Length - 1); 
       }

       foreach (var item in wdList)
       {
           dataStringWD += string.Format("['{0}',{1}],", item.Bank, item.SumMoney);
       }
       if (false == string.IsNullOrEmpty(dataStringWD))
       {
           dataStringWD = dataStringWD.Substring(0, dataStringWD.Length - 1);
       }
       %>
<form method="get" action="/AM/Notify">
    <input type="hidden" value="finance" name="method" />
时间<input type="text" name="dts" class="input-text w150px" value="<%:ViewData["DTS"] %>" id="i_dts" />-<input type="text" name="dte" class="input-text w150px" value="<%:ViewData["DTE"] %>" id="i_dte" /><input type="submit" value="统计" class="btn-normal" />
<div id="charge" style="min-width: 500px; height: 500px; margin: 0 auto"></div>
    <table width="100%" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th>充值方式</th>
                    <th>金额</th>
                </tr>
            </thead>
            <tbody>
                <%
                decimal sum = 0.0000m;
                %>
                <%if( crList != null)
                  {
                      foreach (var item in crList) {
                          sum += item.uc003;
                       %>
                <tr>
                    <td><%:item.ChargeTypeName %></td>
                    <td><%:item.uc003.ToString("N4") %></td>
                </tr>
                <%
                }} %>
            </tbody>
            <tfoot>
                <tr>
                    <td></td>
                    <td><%=sum.ToString("N4") %></td>
                </tr>
            </tfoot>
        </table>
<div id="withdraw" style="min-width: 500px; height: 500px; margin: 0 auto"></div>
    <table width="100%" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th>提现银行</th>
                    <th>金额</th>
                </tr>
            </thead>
            <tbody>
                <%
                sum = 0.0000m;
                %>
                <%if( wdList != null)
                  {
                      foreach (var item in wdList)
                      {
                          sum += item.SumMoney;
                       %>
                <tr>
                    <td><%:item.Bank %></td>
                    <td><%:item.SumMoney.ToString("N4") %></td>
                </tr>
                <%
                }} %>
            </tbody>
            <tfoot>
                <tr>
                    <td></td>
                    <td><%=sum.ToString("N4") %></td>
                </tr>
            </tfoot>
        </table>
<script type="text/javascript">
    $(function () {
        $('#charge').highcharts({
            chart: { type: 'column' },
            title: { text: '充值' },
            xAxis: { type: 'category', labels: { rotation: -45, style: { fontSize: '15px', fontFamily: 'Verdana' } } },
            yAxis: { min: 0, title: { text: '充值' } },
            legend: { enabled: false },
            tooltip: { pointFormat: '充值金额<b>{point.y:.4f}元</b>', },
            series: [{ name: '充值金额', data: [<%=dataString%>], dataLabels: { enabled: true, rotation: -45, color: '#FF0000', align: 'right', x: 20, y:-50, style: { fontSize: '14px', fontFamily: 'Verdana', textShadow: '0 0 3px #ea9c06' } } }]
        });

        $('#withdraw').highcharts({
            chart: { type: 'column' },
            title: { text: '提现' },
            xAxis: { type: 'category', labels: { rotation: -45, style: { fontSize: '15px', fontFamily: 'Verdana' } } },
            yAxis: { min: 0, title: { text: '提现' } },
            legend: { enabled: false },
            tooltip: { pointFormat: '提现金额<b>{point.y:.4f}元</b>', },
            series: [{ name: '提现金额', data: [<%=dataStringWD%>], dataLabels: { enabled: true, rotation: -45, color: '#00ff00', align: 'right', x: 20, y: -50, style: { fontSize: '14px', fontFamily: 'Verdana', textShadow: '0 0 3px #ffffff' } } }]
        });

        jQuery('#i_dts').datetimepicker({
            format: 'Y/m/d H:i:s',
            lang: "ch",
            onShow: function (ct) {
                this.setOptions({
                    maxDate: jQuery('#i_dte').val() ? jQuery('#i_dte').val() : false
                })
            },
            timepicker: true
        });
        jQuery('#i_dte').datetimepicker({
            format: 'Y/m/d H:i:s',
            lang: "ch",
            onShow: function (ct) {
                this.setOptions({
                    minDate: jQuery('#i_dts').val() ? jQuery('#i_dts').val() : false
                })
            },
            timepicker: true
        });
    });
</script>
</form>
<%} else if( "game" == methodType)
  {
      var grList = (List<DBModel.GameReport>)ViewData["GRList"];
      grList = grList.OrderByDescending(exp => exp.WinAmount).ToList();
      var gDicList = ((List<DBModel.wgs001>)ViewData["GameList"]).ToDictionary(exp=>exp.g001);
      var gNames = string.Empty;
      foreach (var item in grList)
      {
          gNames += string.Format("'{0}',", gDicList[item.g001].g002); 
      }
      if (false == string.IsNullOrEmpty(gNames))
      {
          gNames = gNames.Substring(0, gNames.Length - 1); 
      }

      var _countList = string.Empty;
      var _betList = string.Empty;
      var _winList = string.Empty;
      var _pointList = string.Empty;

      for (int col = 0; col < grList.Count(); col++)
      {
          _countList += string.Format("{0},",grList[col].Count);
          _betList += string.Format("{0},", grList[col].BetAmount);
          _winList += string.Format("{0},", grList[col].WinAmount);
          _pointList += string.Format("{0},", grList[col].Point);
      }
      if( false == string.IsNullOrEmpty(_countList) )
      {
          _countList = _countList.Substring(0, _countList.Length - 1);
          _betList = _betList.Substring(0, _betList.Length - 1);
          _winList = _winList.Substring(0, _winList.Length - 1);
          _pointList = _pointList.Substring(0, _pointList.Length - 1);
      }     
      
      %>
<form method="get" action="/AM/Notify">
    <input type="hidden" value="game" name="method" />
时间<input type="text" name="dts" class="input-text w150px" value="<%:ViewData["DTS"]%>" id="i_dts" />-<input type="text" name="dte" class="input-text w150px" value="<%:ViewData["DTE"] %>" id="i_dte" /><input type="submit" value="统计" class="btn-normal" />
<div id="gamereport" style="min-width: 500px; height: 600px; margin: 0 auto"></div>
<script type="text/javascript">
    $(function () {
        $('#gamereport').highcharts({
            chart: {
                type: 'bar'
            },
            title: {
                text: '游戏'
            },
            subtitle: {
                text: ''
            },
            xAxis: {
                categories: [<%=gNames%>],
                title: {
                    text: null
                }
            },
            yAxis: {
                min: 0,
                title: {
                    text: '',
                    align: 'high'
                },
                labels: {
                    overflow: 'justify'
                }
            },
            tooltip: {
                valueSuffix: ' '
            },
            plotOptions: {
                bar: {
                    dataLabels: {
                        enabled: true
                    }
                }
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'top',
                x: -40,
                y: 100,
                floating: true,
                borderWidth: 1,
                backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor || '#FFFFFF'),
                shadow: false
            },
            credits: {
                enabled: false
            },
            series: [{
                name: '有效订单数',
                data: [<%=_countList%>]
            },
            {
                name: '下注金额',
                data: [<%=_betList%>]
            },
            {
                name: '输赢金额',
                data: [<%=_winList%>]
            }]
        });
        jQuery('#i_dts').datetimepicker({
            format: 'Y/m/d H:i:s',
            lang: "ch",
            onShow: function (ct) {
                this.setOptions({
                    maxDate: jQuery('#i_dte').val() ? jQuery('#i_dte').val() : false
                })
            },
            timepicker: true
        });
        jQuery('#i_dte').datetimepicker({
            format: 'Y/m/d H:i:s',
            lang: "ch",
            onShow: function (ct) {
                this.setOptions({
                    minDate: jQuery('#i_dts').val() ? jQuery('#i_dts').val() : false
                })
            },
            timepicker: true
        });
    });
</script>
</form>
<%}else if("online" == methodType){ %>
<%
      var ORList = (List<DBModel.OnlineReport>)ViewData["ORList"];
      var RegList = (List<DBModel.OnlineReport>)ViewData["RegList"];
      var days = string.Empty;
      var daysCount = string.Empty;
      var daysAvg = 0;
      var daysSum = 0;
      var dayMax = 0;
      var dayMin = 0;
      var reg = string.Empty;
      var regCount = string.Empty;
      var regSum = 0;
      var regAvg = 0;
      var regMax = 0;
      var regMin = 0;
      foreach (var item in ORList)
      {
          days += string.Format("'{0}',", item.Date.ToString("MM/dd"));
          daysCount += string.Format("{0},", item.RecordCount);
          daysSum += item.RecordCount;
      }
      foreach (var item in RegList)
      {
          reg += string.Format("'{0}',", item.Date.ToString("MM/dd"));
          regCount += string.Format("{0},", item.RecordCount);
          regSum += item.RecordCount;
      }

      if (false == string.IsNullOrEmpty(days))
      {
          days = days.Substring(0, days.Length - 1);
          daysCount = daysCount.Substring(0, daysCount.Length - 1);
          daysAvg = daysSum / ORList.Count();
          dayMax = ORList.OrderByDescending(exp => exp.RecordCount).FirstOrDefault().RecordCount;
          dayMin = ORList.OrderBy(exp => exp.RecordCount).FirstOrDefault().RecordCount;
      }

      if (false == string.IsNullOrEmpty(reg))
      {
          reg = reg.Substring(0, reg.Length - 1);
          regCount = regCount.Substring(0, regCount.Length - 1);
          regAvg = regSum / RegList.Count();
          regMax = RegList.OrderByDescending(exp => exp.RecordCount).FirstOrDefault().RecordCount;
          regMin = RegList.OrderBy(exp => exp.RecordCount).FirstOrDefault().RecordCount;
      }
      
%>
<form method="get" action="/AM/Notify">
<input type="hidden" value="online" name="method" />
月份<input type="text" name="dts" class="input-text w100px" value="<%:ViewData["DTS"]%>" id="i_dts" /><input type="submit" value="统计" class="btn-normal" />
<div id="onlinereport" style="min-width: 800px; height: 200px; margin: 0 auto"></div>
<p>合计：<%=daysSum %>，平均：<%=daysAvg %>，最高：<%=dayMax %>，最低：<%=dayMin %></p>
<div id="regreport" style="min-width: 800px; height: 200px; margin: 0 auto"></div>
<p>合计：<%=regSum %>，平均：<%=regAvg %>，最高：<%=regMax %>，最低：<%=regMin %></p>
</form>
<script type="text/javascript">
    $(function () {
        $('#onlinereport').highcharts({
            chart: {
                type: 'line'
            },
            title: {
                text: '每月累计登录分析'
            },
            subtitle: {
                text: ''
            },
            xAxis: {
                categories: [<%=days%>]
            },
            yAxis: {
                title: {
                    text: '人数'
                }
            },
            plotOptions: {
                line: {
                    dataLabels: {
                        enabled: true
                    },
                    enableMouseTracking: false
                }
            },
            series: [{
                name: '每月累计登录分析',
                data: [<%=daysCount%>]
            }]
        });
        $('#regreport').highcharts({
            chart: {
                type: 'line'
            },
            title: {
                text: '每月注册人数分析'
            },
            subtitle: {
                text: ''
            },
            xAxis: {
                categories: [<%=reg%>]
            },
            yAxis: {
                title: {
                    text: '人数'
                }
            },
            plotOptions: {
                line: {
                    dataLabels: {
                        enabled: true
                    },
                    enableMouseTracking: false
                }
            },
            series: [{
                name: '每月注册人数分析',
                data: [<%=regCount%>]
            }]
        });
        jQuery('#i_dts').datetimepicker({
            format: 'Y/m/d H:i:s',
            lang: "ch",
            onShow: function (ct) {
                this.setOptions({
                    maxDate: jQuery('#i_dte').val() ? jQuery('#i_dte').val() : false
                })
            },
            timepicker: true
        });
    });
</script>
<%} %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript" src="/Scripts/Chart/js/highcharts.js"></script>
<script type="text/javascript" src="/Scripts/Chart/js/modules/exporting.js"></script>
<style type="text/css">
${demo.css}
</style>
</asp:Content>

