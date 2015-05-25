<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%
    var gameClassList = (List<DBModel.wgs006>)ViewData["GameClassList"];
    var gameMethodGroupList = (List<DBModel.wgs003>)ViewData["GameMethodGroupList"];
    var gameMethodList = (List<DBModel.wgs002>)ViewData["GameMethodList"];
    var gameMethodPrizeDataList = (Dictionary<int, DBModel.wgs008>)ViewData["GameMethodPrizeDataDicList"];
    decimal sysMaxPoint = (decimal)ViewData["SysMaxPoint"];
    decimal myMaxPoint = (decimal)ViewData["MyMaxPoint"];
%>
<%int listIndex = 0; foreach (var gmg in gameMethodGroupList)
  { %>
<table class="table-pro table-color-row user_gp_info" width="100%">
    <thead>
        <tr class="table-pro-head">
            <th class="w50px"><%:gmg.gmg003%></th>
            <th class="dom-hide">编号</th>
            <th>奖金上限</th>
            <th>奖金下限</th>
        </tr>
    </thead>
    <tbody>
        <% foreach (var gm in gameMethodList)
           {
               if (gm.gmg001 != gmg.gmg001)
               {
                   continue;
               }
        %>
        <tr key="<%:gameMethodPrizeDataList[gm.gm001].gpd001%>">
            <td class="text-right">
                <%:gm.gm004%>
            </td>
            <td class="w100px dom-hide"><span class="tips"><%:gm.gm001%></span></td>
            <td class="w150px"><%:Html.GMCalcMaxPrize(gameMethodPrizeDataList[gm.gm001].gpd002, gameMethodPrizeDataList[gm.gm001].gpd003, sysMaxPoint, myMaxPoint) %></td>
            <td class="w150px"><%:string.Format("{0:N4}",gameMethodPrizeDataList[gm.gm001].gpd003)%></td>
        </tr>
        <%
                   listIndex++;
               } %>
    </tbody>
</table>
<br />
<%
          //listIndex++;
      } %>