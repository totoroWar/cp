<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var methodType = (string)ViewData["MethodType"];
    %>
    <div class="ui-page-content-body-header tools">
        <div class="left-nav">
            <a href="/UI/Report?method=reportDetails" title="自己盈亏" <%=methodType == "reportDetails" ? "class='item-select'" : "" %>>自己盈亏</a>
            <a href="/UI/Report?method=reportChildDetails" title="下级盈亏" <%=methodType == "reportChildDetails" ? "class='item-select'" : "" %>>下级盈亏</a>
            <a href="/UI/Report?method=dataChange" title="账变记录" <%=methodType == "dataChange" ? "class='item-select'" : "" %>>账变记录</a>
            <a href="/UI/Report?method=stockDetails" title="账变记录" <%=methodType == "stockDetails" ? "class='item-select'" : "" %>>分红记录</a>
        </div>
    </div>
    <div class="blank-line"></div>
    <%if ("reportDetails" == methodType)
      { %>
    <%
          var dayReportList = (List<DBModel.wgs042>)ViewData["DayReportList"];
          decimal linedr005Sum = 0;
          decimal linedr004Sum = 0;
          decimal linedr006Sum = 0;
          decimal linedr007Sum = 0;
          decimal linedr010Sum = 0;
          decimal linedr008Sum = 0;
          decimal linedr011Sum = 0;
          decimal linedr013Sum = 0;
          decimal linedr016Sum = 0;
          decimal linedr017Sum = 0;
          decimal linedr018Sum = 0;
          decimal lineSumTotal = 0;
    %>
    <div class="block_tools">
        <form method="get" action="/UI/Report">
        <input type="hidden" name="method" value="<%:methodType %>" />
        <span>
            时间范围<input id="i_dts" name="dts" type="text" class="input-text w80px" value="<%:((DateTime)ViewData["DTS"]).ToString(ViewContext.ViewData["SysDateFormat"].ToString()) %>" />
            <input id="i_dte" name="dte" type="text" class="input-text w80px" value="<%:((DateTime)ViewData["DTE"]).ToString(ViewContext.ViewData["SysDateFormat"].ToString()) %>" />
        </span>
        <input type="submit" class="btn-normal" value="统计" />
        </form>
    </div>
    <table class="table-pro tp5 w100ps g_nco">
        <thead>
            <tr class="table-pro-head">
                <th>账号</th>
                <th>更新时间</th>
                <th>充值金额</th>
                <th>下注金额</th>
                <th>奖金金额</th>
                <th>返点金额</th>
                <th>提现金额</th>
                <th>赠送金额</th>
                <th>分红金额</th>
                <th>转账-入</th>
                <th>转账-出</th>
                <th>获取积分</th>
                <th>消费积分</th>
                <th>盈利</th>
            </tr>
        </thead>
        <tbody>
            <%if( null != dayReportList)
              {
                  foreach( var item in dayReportList)
                  {
                 %>
            <tr>
              <%
                      //(奖金+返点+系统充值金额+系统分红+转账入)-下单金额
                      //decimal lineSum = (item.dr006 + item.dr007) - (item.dr004);
                      //奖金金额-下单金额
                      decimal lineSum = (item.dr006) - (item.dr004);
                      //decimal lineSum = (item.dr006 + item.dr011) - (item.dr004);
                  lineSumTotal += lineSum;
                  linedr005Sum += item.dr005;
                  linedr004Sum += item.dr004;
                  linedr006Sum += item.dr006;
                  linedr007Sum += item.dr007;
                  linedr010Sum += item.dr010;
                  linedr008Sum += item.dr008;
                  linedr011Sum += item.dr011;
                  linedr013Sum += item.dr013;
                  linedr016Sum += item.dr016;
                  linedr017Sum += item.dr017;
                  linedr018Sum += item.dr018;
               %>
                <td><%:item.u002.Trim() %></td>
                <td><%:item.dr003.ToString(ViewContext.ViewData["SysDateTimeFormat"].ToString()) %></td>
                <td class="<%:item.dr005>0 ? "fc-yellow" : "" %>"><%:item.dr005 %></td>
                <td class="<%:item.dr004>0 ? "fc-yellow" : "" %>"><%:item.dr004 %></td>
                <td class="<%:item.dr006>0 ? "fc-yellow" : "" %>"><%:item.dr006 %></td>
                <td class="<%:item.dr007>0 ? "fc-yellow" : "" %>"><%:item.dr007 %></td>
                <td class="<%:item.dr010>0 ? "fc-yellow" : "" %>"><%:item.dr010 %></td>
                <td class="<%:item.dr011>0 ? "fc-yellow" : "" %>"><%:item.dr011 %></td>
                <td class="<%:item.dr016>0 ? "fc-yellow" : "" %>"><%:item.dr016 %></td>
                <td class="<%:item.dr017>0 ? "fc-yellow" : "" %>"><%:item.dr017 %></td>
                <td class="<%:item.dr018>0 ? "fc-yellow" : "" %>"><%:item.dr018 %></td>
                <td class="<%:item.dr008>0 ? "fc-yellow" : "" %>"><%:item.dr008 %></td>
                <td class="<%:item.dr013>0 ? "fc-yellow" : "" %>"><%:item.dr013 %></td>
                <td class="<%:lineSum>0 ? "fc-yellow" : "" %>"><%:lineSum %></td>
            </tr>
            <%
            }/*foreach*/
            }/*if*/ %>
        </tbody>
        <tfoot>
            <tr>
                <td>合计</td>
                <td></td>
                <td class="<%:linedr005Sum>0 ? "fc-yellow" : "" %>"><%:linedr005Sum %></td>
                <td class="<%:linedr004Sum>0 ? "fc-yellow" : "" %>"><%:linedr004Sum %></td>
                <td class="<%:linedr006Sum>0 ? "fc-yellow" : "" %>"><%:linedr006Sum %></td>
                <td class="<%:linedr007Sum>0 ? "fc-yellow" : "" %>"><%:linedr007Sum %></td>
                <td class="<%:linedr010Sum>0 ? "fc-yellow" : "" %>"><%:linedr010Sum %></td>
                <td class="<%:linedr011Sum>0 ? "fc-yellow" : "" %>"><%:linedr011Sum %></td>
                <td class="<%:linedr016Sum>0 ? "fc-yellow" : "" %>"><%:linedr016Sum %></td>
                <td class="<%:linedr017Sum>0 ? "fc-yellow" : "" %>"><%:linedr017Sum %></td>
                <td class="<%:linedr018Sum>0 ? "fc-yellow" : "" %>"><%:linedr018Sum %></td>
                <td class="<%:linedr008Sum>0 ? "fc-yellow" : "" %>"><%:linedr008Sum %></td>
                <td class="<%:linedr013Sum>0 ? "fc-yellow" : "" %>"><%:linedr013Sum %></td>
                <td class="<%:lineSumTotal>0 ? "fc-yellow" : "" %>"><%:lineSumTotal %></td>
            </tr>
        </tfoot>
    </table>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#i_dts').appendDtpicker({ locale: "cn", dateFormat: "YYYY/MM/DD" });
            $('#i_dte').appendDtpicker({ locale: "cn", dateFormat: "YYYY/MM/DD" });
        });
    </script>
    <%}/*reportDetails*/ %>
    <%else if ("dataChange" == methodType)
      { %>
    <%
          var gameListDic = ((List<DBModel.wgs001>)ViewData["GameList"]).ToDictionary(key => key.g001);
          var gameMethodListDic = ((List<DBModel.wgs002>)ViewData["GameMethodList"]).ToDictionary(key => key.gm001);
          var dctList = (Dictionary<int, DBModel.SysDataChangeType>)ViewData["DCTList"];
          var dctType = (byte)ViewData["DCTType"];
    %>
    <div class="block_tools">
        <form action="/UI/Report" method="get">
            <input type="hidden" name="method" value="<%:methodType %>" />
            <span>
                <select name="dcttype">
                    <option value="0">全部</option>
                    <%foreach (var item in dctList)
                      { %>
                    <option value="<%:item.Value.ID %>" <%=dctType== item.Value.ID ? "selected='selected'" : "" %>><%:item.Value.Name %></option>
                    <%} %>
                </select>
            </span>
            <span>时间范围<input id="i_dts" name="dts" type="text" class="input-text w120px" value="<%:((DateTime)ViewData["DTS"]).ToString(ViewContext.ViewData["SysDateTimeFormat"].ToString()) %>" />
                <input id="i_dte" name="dte" type="text" class="input-text w120px" value="<%:((DateTime)ViewData["DTE"]).ToString(ViewContext.ViewData["SysDateTimeFormat"].ToString()) %>" />
            </span>
            <input type="submit" class="btn-normal" value="查找" />
            <input type="button" class="btn-normal show-table-all-col" value="显示切换" />
        </form>
    </div>
    <div class="blank-line"></div>
    <table class="table-pro table-color-row tp5 g_nco" width="100%">
        <thead>
            <tr class="table-pro-head">
                <th class="w50px">编号</th>
                <th>备注</th>
                <th class="hide-col hide-col-eml">充值编号</th>
                <th class="hide-col hide-col-eml">提现编号</th>
                <th class="hide-col hide-col-eml">订单编号</th>
                <th class="hide-col hide-col-eml">游戏</th>
                <th class="hide-col hide-col-eml">期号</th>
                <th class="hide-col hide-col-eml">玩法</th>
                <th>变动金额</th>
                <th>变动前金额</th>
                <th>变动后金额</th>
                <th>变动前冻结金额</th>
                <th>变动后冻结金额</th>
                <th>变动积分</th>
                <th>变动前积分</th>
                <th>变动后积分</th>
                <th>时间</th>
            </tr>
        </thead>
        <tbody>
            <%
                      var dataChangeList = (List<DBModel.wgs021>)ViewData["DataChangeList"];
                      if (null != dataChangeList)
                      {
                          foreach (var item in dataChangeList)
                          {
            %>
            <tr>
                <td><%:item.uxf001 %></td>
                <td><%:item.uxf015 %></td>
                <td class="hide-col hide-col-eml"><%:item.uc001 %></td>
                <td class="hide-col hide-col-eml"><%:item.uw001 %></td>
                <td class="hide-col hide-col-eml"><%:item.so001 %></td>
                <td class="hide-col hide-col-eml"><%:item.g001 != 0 ? gameListDic[item.g001].g003 : "0" %></td>
                <td class="hide-col hide-col-eml"><%:item.gs002 != null ? item.gs002.Trim() : "-" %></td>
                <td class="hide-col hide-col-eml"><%:item.gm001 != 0 ? gameMethodListDic[item.gm001].gm004 : "0" %></td>
                <td class="fc-red"><%:item.uxf003.ToString("N4") %></td>
                <td class="fc-blue"><%:item.uxf002.ToString("N4") %></td>
                <td><%:item.uxf007.ToString("N4") %></td>
                <td class="fc-blue"><%:item.uxf010.ToString("N4") %></td>
                <td><%:item.uxf009.ToString("N4") %></td>
                <td class="fc-red"><%:item.uxf005.ToString("N4") %></td>
                <td class="fc-blue"><%:item.uxf004.ToString("N4") %></td>
                <td><%:item.uxf008.ToString("N4") %></td>
                <td><%:item.uxf014 %></td>
            </tr>
            <%
              }/*foreach*/
          }/*if*/ %>
        </tbody>
    </table>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#i_dts').appendDtpicker({ locale: "cn", dateFormat: "YYYY/MM/DD hh:mm" });
            $('#i_dte').appendDtpicker({ locale: "cn", dateFormat: "YYYY/MM/DD hh:mm" });
        });
    </script>
    <%=ViewData["PageList"] %>
    <%} %>
    <%else if ("pointRecord" == methodType)
      { %>
    <%
        var data = new ViewDataDictionary();
        data.Add("Message", "未开放");
    %>
    <%=Html.Partial(ViewData["ControllerTheme"]+"/Common/Message", data) %>
    <%}/*pointRecord*/ %>
    <%else if ("creditsRecord" == methodType)
      { %>
    <%
        var data = new ViewDataDictionary();
        data.Add("Message", "未开放");
    %>
    <%=Html.Partial(ViewData["ControllerTheme"]+"/Common/Message", data) %>
    <%}/*creditsRecord*/ %>
    <%else if( "reportChildDetails" == methodType)
      {
          var type = (int)ViewData["Type"];
          var drList = (List<DBModel.SysSumDRInfo>)ViewData["DRList"];
          var drdList = (List<DBModel.wgs042>)ViewData["DRDList"];
          if (null == drdList)
          {
              drdList = new List<DBModel.wgs042>(); 
          }

          int om = (int)ViewData["om"];
          decimal omm = (decimal)ViewData["omm"];
          int cm = (int)ViewData["cm"];
          decimal cmm = (decimal)ViewData["cmm"];
          int pm = (int)ViewData["pm"];
          decimal pmm = (decimal)ViewData["pmm"];
          string acct = (string)ViewData["acct"];
          string pacct = (string)ViewData["pacct"];
          
          var total = from dl in drdList
                          group dl by new { dl.u002, dl.u001 } into ndl
                          select new
                          {
                              u001 = ndl.Key.u001,
                              u002 = ndl.Key.u002,
                              dr004 = ndl.Sum(exp => exp.dr004),
                              dr005 = ndl.Sum(exp => exp.dr005),
                              dr006 = ndl.Sum(exp => exp.dr006),
                              dr007 = ndl.Sum(exp => exp.dr007),
                              dr008 = ndl.Sum(exp => exp.dr008),
                              dr009 = ndl.Sum(exp => exp.dr009),
                              dr010 = ndl.Sum(exp => exp.dr010),
                              dr011 = ndl.Sum(exp => exp.dr011),
                              dr012 = ndl.Sum(exp => exp.dr012),
                              dr013 = ndl.Sum(exp => exp.dr013),
                              dr014 = ndl.Sum(exp => exp.dr014),
                              dr015 = ndl.Sum(exp => exp.dr015),
                              dr016 = ndl.Sum(exp => exp.dr016),
                              dr017 = ndl.Sum(exp => exp.dr017),
                              dr018 = ndl.Sum(exp => exp.dr018)
                          };
          
          decimal linedr005Sum = 0;
          decimal linedr004Sum = 0;
          decimal linedr006Sum = 0;
          decimal linedr007Sum = 0;
          decimal linedr008Sum = 0;
          decimal linedr011Sum = 0;
          decimal linedr013Sum = 0;
          decimal linedr016Sum = 0;
          decimal linedr017Sum = 0;
          decimal linedr018Sum = 0;
          decimal lineSumTotal = 0;
           %>
    <div class="block_tools">
        <form method="get" action="/UI/Report">
            <input value="reportChildDetails" name="method" type="hidden" />
        <span>时间范围<input id="i_dts" name="dts" type="text" class="input-text w80px" value="<%:((DateTime)ViewData["DTS"]).ToString(ViewContext.ViewData["SysDateFormat"].ToString()) %>" />
                <input id="i_dte" name="dte" type="text" class="input-text w80px" value="<%:((DateTime)ViewData["DTE"]).ToString(ViewContext.ViewData["SysDateFormat"].ToString()) %>" />
            </span>
        类型
        <select name="type">
            <%--<option class="dom-hide" value="0" <%=type == 0 ? "selected='selected'" : "" %>>直接下级（包含直接下级）</option>
            <option class="dom-hide" value="1" <%=type == 1 ? "selected='selected'" : "" %>>直接下级（不包含直接下级）</option>
            <option class="dom-hide" value="2" <%=type == 2 ? "selected='selected'" : "" %>>所有下级（包含直接下级）</option>
            <option class="dom-hide" value="3" <%=type == 3 ? "selected='selected'" : "" %>>所有下级（不包含直接下级）</option>--%>
            <option value="4" <%=type == 4 ? "selected='selected'" : "" %>>个人查询</option>
            <option value="5" <%=type == 5 ? "selected='selected'" : "" %>>团队查询</option>
        </select>
        <div class="blank-line"></div>
        账号<input type="text" class="input-text w80px" name="acct" value="<%:acct %>" />
        下注
        <select name="om" id="om">
            <option value="0" <%:om == 0 ? "selected='selected'" : "" %>>所有</option>
            <option value="1" <%:om == 1 ? "selected='selected'" : "" %>>小于</option>
            <option value="2" <%:om == 2 ? "selected='selected'" : "" %>>等于</option>
            <option value="3" <%:om == 3 ? "selected='selected'" : "" %>>大于</option>
        </select>
            <input type="text" class="input-text w50px" name="omm" value="<%:omm %>" />
        充值
        <select name="cm" id="cm">
            <option value="0" <%:cm == 0 ? "selected='selected'" : "" %>>所有</option>
            <option value="1" <%:cm == 1 ? "selected='selected'" : "" %>>小于</option>
            <option value="2" <%:cm == 2 ? "selected='selected'" : "" %>>等于</option>
            <option value="3" <%:cm == 3 ? "selected='selected'" : "" %>>大于</option>
        </select>
            <input type="text" class="input-text w50px" name="cmm" value="<%:cmm %>" />
        中奖
        <select name="pm" id="pm">
            <option value="0" <%:pm == 0 ? "selected='selected'" : "" %>>所有</option>
            <option value="1" <%:pm == 1 ? "selected='selected'" : "" %>>小于</option>
            <option value="2" <%:pm == 2 ? "selected='selected'" : "" %>>等于</option>
            <option value="3" <%:pm == 3 ? "selected='selected'" : "" %>>大于</option>
        </select>
            <input type="text" class="input-text w50px" name="pmm" value="<%:pmm %>" />
        <input type="submit" class="btn-normal" value="查询" />
            <input type="button" class="btn-normal ui-a-back" value="返回" />
        </form>
    </div>
    <%
          decimal coldr004 = 0.0000m;
          decimal coldr005 = 0.0000m;
          decimal coldr006 = 0.0000m;
          decimal coldr007 = 0.0000m;
          decimal coldr008 = 0.0000m;
          decimal coldr009 = 0.0000m;
          decimal coldr010 = 0.0000m;
          decimal coldr011 = 0.0000m;
          decimal coldr012 = 0.0000m;
          decimal coldr013 = 0.0000m;
          decimal coldr014 = 0.0000m;
          decimal coldr015 = 0.0000m;
          decimal coldr016 = 0.0000m;
          decimal coldr017 = 0.0000m;
          decimal coldr018 = 0.0000m;
          decimal collinesum = 0.0000m;
          int rowIndex = 1;
    %>
    <table class="table-pro tp5 w100ps">
        <thead>
            <tr class="table-pro-head">
                <th>序号</th>
                <th class="w100px">账号</th>
                <th class="fc-green">充值金额</th>
                <th class="fc-red">下注金额</th>
                <th>奖金金额</th>
                <th>返点金额</th>
                <th>提现金额</th>
                <th>赠送金额</th>
                <th>分红金额</th>
                <th>转账-入</th>
                <th>转账-出</th>
                <th>获取积分</th>
                <th>消费积分</th>
                <th>盈利</th>
            </tr>
        </thead>
        <tbody>
            <%if (null != total)
              {
                  total = total.OrderByDescending(exp => exp.dr004).ToList();
                  foreach (var item in total)
                  {
                      //(奖金+返点+系统充值金额+系统分红+转账入)-下单金额
                      //盈亏：输出该会员或该团队的盈利和亏损额（奖金+返点+赠送）-消费金额
                  //decimal lineSum = (item.dr006 + item.dr007) - (item.dr004);
                      //decimal lineSum = (item.dr006) - (item.dr004);
                      decimal lineSum = (item.dr006 + item.dr011) - (item.dr004);
                  collinesum += lineSum;
                  coldr004 += item.dr004; 
                  coldr005 += item.dr005;
                  coldr006 += item.dr006;
                  coldr007 += item.dr007;
                  coldr008 += item.dr008;
                  coldr009 += item.dr009;
                  coldr010 += item.dr010;
                  coldr011 += item.dr011;
                  coldr012 += item.dr012;
                  coldr013 += item.dr013;
                  coldr014 += item.dr014;
                  coldr015 += item.dr015;
                  coldr016 += item.dr016;
                  coldr017 += item.dr017;
                  coldr018 += item.dr018;  
                 %>
            <tr>
                <td><%:rowIndex++ %></td>
                <td>
                    <%if( type == 5){ %>
                    <a href="/UI/Report?method=reportChildDetails&type=<%:type %>&pacct=<%:item.u002.Trim() %>&dts=<%:ViewData["dts"] %>&dte=<%:ViewData["dte"] %>"><%:item.u002 %></a>
                    <%}else{ %>
                    <%:item.u002 %>
                    <%} %>
                </td>
                <td class="<%:item.dr005>0 ? "fc-yellow" : "" %>"><%:item.dr005.ToString("N4") %></td>
                <td class="<%:item.dr004>0 ? "fc-yellow" : "" %>"><%:item.dr004.ToString("N4") %></td>
                <td class="<%:item.dr006>0 ? "fc-yellow" : "" %>"><%:item.dr006.ToString("N4") %></td>
                <td class="<%:item.dr007>0 ? "fc-yellow" : "" %>"><%:item.dr007.ToString("N4") %></td>
                <td class="<%:item.dr010>0 ? "fc-yellow" : "" %>"><%:item.dr010.ToString("N4") %></td>
                <td class="<%:item.dr011>0 ? "fc-yellow" : "" %>"><%:item.dr011.ToString("N4") %></td>
                <td class="<%:item.dr016>0 ? "fc-yellow" : "" %>"><%:item.dr016.ToString("N4") %></td>
                <td class="<%:item.dr017>0 ? "fc-yellow" : "" %>"><%:item.dr017.ToString("N4") %></td>
                <td class="<%:item.dr018>0 ? "fc-yellow" : "" %>"><%:item.dr018.ToString("N4") %></td>
                <td class="<%:item.dr008>0 ? "fc-yellow" : "" %>"><%:item.dr008.ToString("N4") %></td>
                <td class="<%:item.dr013>0 ? "fc-yellow" : "" %>"><%:item.dr013.ToString("N4") %></td>
                <td class="<%:lineSum>0 ? "fc-yellow" : "" %>"><%:lineSum %></td>
            </tr>
            <%
            }/*foreach*/
            }/*if*/ %>
        </tbody>
        <tfoot>
            <tr>
                <td>-</td>
                <td>-</td>
                <td class="<%:coldr005>0 ? "fc-yellow" : "" %>"><%:coldr005 %></td>
                <td class="<%:coldr004>0 ? "fc-yellow" : "" %>"><%:coldr004 %></td>
                <td class="<%:coldr006>0 ? "fc-yellow" : "" %>"><%:coldr006.ToString("N4") %></td>
                <td class="<%:coldr007>0 ? "fc-yellow" : "" %>"><%:coldr007.ToString("N4") %></td>
                <td class="<%:coldr010>0 ? "fc-yellow" : "" %>"><%:coldr010.ToString("N4") %></td>
                <td class="<%:coldr011>0 ? "fc-yellow" : "" %>"><%:coldr011.ToString("N4") %></td>
                <td class="<%:coldr016>0 ? "fc-yellow" : "" %>"><%:coldr016.ToString("N4") %></td>
                <td class="<%:coldr017>0 ? "fc-yellow" : "" %>"><%:coldr017.ToString("N4") %></td>
                <td class="<%:coldr018>0 ? "fc-yellow" : "" %>"><%:coldr018.ToString("N4") %></td>
                <td class="<%:coldr008>0 ? "fc-yellow" : "" %>"><%:coldr008.ToString("N4") %></td>
                <td class="<%:coldr013>0 ? "fc-yellow" : "" %>"><%:coldr013.ToString("N4") %></td>
                <td class="<%:collinesum>0 ? "fc-yellow" : "" %>"><%:collinesum %></td>
            </tr>
        </tfoot>
    </table>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#i_dts').appendDtpicker({ locale: "cn", dateFormat: "YYYY/MM/DD" });
            $('#i_dte').appendDtpicker({ locale: "cn", dateFormat: "YYYY/MM/DD" });
        });
    </script>
    <%}/*reportChildDetails*/ %>
    <%else if ("stockDetails" == methodType)
      { %>
    <%
          var stockList = (List<DBModel.wgs051>)ViewData["StockList"];
    %>
    <table class="table-pro table-color-row tp5" width="100%">
        <thead>
            <tr class="table-pro-head">
                <%--<th>编号</th>
                <th>分红金额</th>
                <th>创红下级</th>
                <th>自己占分红比</th>
                <th>设置下级分红比</th>
                <th>统计时间</th>--%>

                
                
                
                
                <th>分红金额</th>
                <th>分红盈亏</th>
                <th>分红比例</th>
                <th>周期天数</th>
                <th>周期开始时间</th>
                <th>周期结束时间</th>
                

                
                

                

                
                <th>审核状态</th>


            </tr>
        </thead>
        <tbody>

            <%=ViewData["FHLog"] %>
           <%-- <%if( null != stockList)
              {
                  foreach(var item in stockList)
                  { %>
            <tr>
                <td><%:item.us001 %></td>
                <td><%:item.us008 %></td>
                <td><%:item.us006 %></td>
                <td><%:(int)item.us009 %>%</td>
                <td><%:(int)item.us016 %>%</td>
                <td><%:item.us011 %>到<%:item.us012 %></td>
            </tr>
            <%}
              }/*if*/ %>--%>
        </tbody>
            <%=ViewData["FHfoot"] %>
        <tfoot>
            
        </tfoot>
    </table>
    <%=ViewData["PageList"] %>
    <%} /*stockDetails*/%>
</asp:Content>