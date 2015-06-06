<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    StockReport
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var type = (int)ViewData["Type"];
    var drList = (List<DBModel.SysSumDRInfo>)ViewData["DRList"];
    var drdList = (List<DBModel.wgs042>)ViewData["DRDList"];
    if (null == drdList)
    {
        drdList = new List<DBModel.wgs042>();
    }

    int om = (int)ViewData["om"];
    int bp = (int)ViewData["bp"];
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
    <div class="xtool">
        <form method="get" action="/AM/StockReport">
            <input value="reportChildDetails" name="method" type="hidden" />
        <span>
            时间范围<input id="i_dts" name="dts" type="text" class="input-text w120px" value="<%:ViewData["DTS"] %>" />
            <input id="i_dte" name="dte" type="text" class="input-text w120px" value="<%:ViewData["DTE"] %>" />
       </span>
        <%--<span>
            时间范围<input  id="i_dts" type="text" name="dts" class="input-text w80px" value="<%:ViewData["DTS"] %>" id="i_dts" />-<input type="text" name="dte" class="input-text w120px" value="<%:((DateTime)ViewData["DTS"]).ToString(ViewContext.ViewData["SysDateFormat"].ToString()) %>" />
            <input id="i_dte" name="dte" type="text" class="input-text w80px" value="<%:((DateTime)ViewData["DTE"]).ToString(ViewContext.ViewData["SysDateFormat"].ToString()) %>" />
        </span>--%>
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
        <%--计算返点
            <select name="bp" id="bp">
            <option value="2" <%:bp == 2 ? "selected='selected'" : "" %>>不计算</option>
            <option value="1" <%:bp == 1 ? "selected='selected'" : "" %>>计算</option>--%>
        分红类型
        <select name="bp" id="bp">
            <option value="1" <%:bp == 1 ? "selected='selected'" : "" %>>奖金+返点+赠送-消费</option>
            <option value="2" <%:bp == 2 ? "selected='selected'" : "" %>>奖金+赠送-消费</option>
            <option value="3" <%:bp == 3 ? "selected='selected'" : "" %>>奖金-消费</option>
            <option value="4" <%:bp == 4 ? "selected='selected'" : "" %>>奖金+返点-消费</option>
        </select>
            <input type="text" class="input-text w50px" name="pmm" value="<%:pmm %>" />
        <input type="submit" class="btn-normal" value="查询" />
            <input type="button" class="btn-normal cjlsoft-a-back" value="返回" />

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
                <!--<th>获取积分</th>-->
                <!--<th>消费积分</th>-->
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
                      decimal lineSum = (item.dr006 + item.dr007 + item.dr011) - (item.dr004);
                      if (bp == 2)
                      {
                          lineSum = (item.dr006 + item.dr011) - (item.dr004);
                      }
                      if (bp == 3)
                      {
                          lineSum = item.dr006 - item.dr004;
                      }
                      if (bp == 4)
                      {
                          lineSum = item.dr006 + item.dr007 - item.dr004;
                      }
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
                    <a href="/AM/StockReport?type=<%:type %>&pacct=<%:item.u002.Trim() %>&dts=<%:ViewData["dts"] %>&dte=<%:ViewData["dte"] %>&bp=<%:ViewData["bp"] %>"><%:item.u002 %></a>
                    <%}else{ %>
                    <%:item.u002 %>
                    <%} %>
                </td>
                <td class="<%:item.dr005>0 ? "fc-red" : "fc-green" %>"><%:item.dr005.ToString("N4") %></td>
                <td class="<%:item.dr004>0 ? "fc-red" : "fc-green" %>"><%:item.dr004.ToString("N4") %></td>
                <td class="<%:item.dr006>0 ? "fc-red" : "fc-green" %>"><%:item.dr006.ToString("N4") %></td>
                <td class="<%:item.dr007>0 ? "fc-red" : "fc-green" %>"><%:item.dr007.ToString("N4") %></td>
                <td class="<%:item.dr010>0 ? "fc-red" : "fc-green" %>"><%:item.dr010.ToString("N4") %></td>
                <td class="<%:item.dr011>0 ? "fc-red" : "fc-green" %>"><%:item.dr011.ToString("N4") %></td>
                <td class="<%:item.dr016>0 ? "fc-red" : "fc-green" %>"><%:item.dr016.ToString("N4") %></td>
                <td class="<%:item.dr017>0 ? "fc-red" : "fc-green" %>"><%:item.dr017.ToString("N4") %></td>
                <td class="<%:item.dr018>0 ? "fc-red" : "fc-green" %>"><%:item.dr018.ToString("N4") %></td>
                <!--<td class="<%:item.dr008>0 ? "fc-red" : "fc-green" %>"><%:item.dr008.ToString("N4") %></td>-->
                <!--<td class="<%:item.dr013>0 ? "fc-red" : "fc-green" %>"><%:item.dr013.ToString("N4") %></td>-->
                <td class="<%:lineSum>0 ? "fc-red" : "fc-green" %>"><%:lineSum %></td>
            </tr>
            <%
            }/*foreach*/
            }/*if*/ %>
        </tbody>
        <tfoot>
            <tr>
                <td>-</td>
                <td>-</td>
                <td class="<%:coldr005>0 ? "fc-red" : "fc-green" %>"><%:coldr005 %></td>
                <td class="<%:coldr004>0 ? "fc-red" : "fc-green" %>"><%:coldr004 %></td>
                <td class="<%:coldr006>0 ? "fc-red" : "fc-green" %>"><%:coldr006.ToString("N4") %></td>
                <td class="<%:coldr007>0 ? "fc-red" : "fc-green" %>"><%:coldr007.ToString("N4") %></td>
                <td class="<%:coldr010>0 ? "fc-red" : "fc-green" %>"><%:coldr010.ToString("N4") %></td>
                <td class="<%:coldr011>0 ? "fc-red" : "fc-green" %>"><%:coldr011.ToString("N4") %></td>
                <td class="<%:coldr016>0 ? "fc-red" : "fc-green" %>"><%:coldr016.ToString("N4") %></td>
                <td class="<%:coldr017>0 ? "fc-red" : "fc-green" %>"><%:coldr017.ToString("N4") %></td>
                <td class="<%:coldr018>0 ? "fc-red" : "fc-green" %>"><%:coldr018.ToString("N4") %></td>
                <!--<td class="<%:coldr008>0 ? "fc-red" : "fc-green" %>"><%:coldr008.ToString("N4") %></td>-->
                <!--<td class="<%:coldr013>0 ? "fc-red" : "fc-green" %>"><%:coldr013.ToString("N4") %></td>-->
                <td class="<%:collinesum>0 ? "fc-red" : "fc-green" %>"><%:collinesum %></td>
            </tr>
        </tfoot>
    </table>
    <script type="text/javascript">
        jQuery('#i_dts').datetimepicker({
            format: 'Y/m/d H:i:s',
            lang: "ch",
            timepicker: true
        });
        jQuery('#i_dte').datetimepicker({
            format: 'Y/m/d H:i:s',
            lang: "ch",
            timepicker: true
        });
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
