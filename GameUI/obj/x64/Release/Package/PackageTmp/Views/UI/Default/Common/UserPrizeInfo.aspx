<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%
    var groupList = (List<DBModel.wgs028>)ViewData["GPDDataGroup"];
    var gpdDataList = (List<DBModel.wgs029>)ViewData["GPDDataList"];
    var dicItemList = (Dictionary<int, DBModel.wgs028>)ViewData["DicItemList"];
    decimal sysMaxPoint = (decimal)ViewData["SysMaxPoint"];
    decimal myMaxPoint = (decimal)ViewData["MyMaxPoint"];
%>
<%int listIndex = 0; foreach (var group in groupList)
  { %>
<table class="table-pro table-color-row tp5 user_gp_info">
    <thead>
        <tr class="table-pro-head">
            <th style="width:100px;"><%:group.gtp003%></th>
            <th style="width:30px; display:none;">编号</th>
            <th style="width:150px;">奖金上限</th>
            <th style="width:150px;">奖金下限</th>
        </tr>
    </thead>
    <tbody>
        <% foreach (var gpd in gpdDataList)
           {
               if (dicItemList[gpd.gtp001].gtp008 != group.gtp001)
               {
                   continue;
               }
        %>
        <tr>
            <td class="text-right" style="width:100px; color:#ff6a00; padding:3px;">
                <%:dicItemList[gpd.gtp001].gtp003%>
            </td>
            <td style="display:none;"><span class="tips"><%:gpd.gtpd001%></span></td>
            <td style="color:#ffd800;"><%:Html.GMCalcMaxPrize(gpd.gtpd002, gpd.gtpd003, sysMaxPoint, myMaxPoint) %></td>
            <td style="color:#ffd800;"><%:gpd.gtpd003.ToString("N4") %></td>
        </tr>
        <%
               listIndex++;
           } 
        %>
    </tbody>
</table>
<%
      //listIndex++;
  } 
%>
<script type="text/javascript">
    _global_set_table_color_row();
</script>