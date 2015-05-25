<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs007>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var gameClassList = (List<DBModel.wgs006>)ViewData["GameClassList"];
    var gameClassID = (int)ViewData["GameClassID"];
    var gameMethodGroupList = (List<DBModel.wgs003>)ViewData["GameMethodGroupList"];
    var gameMethodList = (List<DBModel.wgs002>)ViewData["GameMethodList"];
    var gameMethodPrizeDataList = (Dictionary<int, DBModel.wgs008>)ViewData["GameMethodPrizeDataDicList"];
%>
<div class="cjlsoft-body-header dom-hide">
        <h1></h1>
</div>
        <div class="blank-line"></div>
    <form action="/AM/GameClassPrize" method="post">
        <%:Html.AntiForgeryToken()%>
        <input type="hidden" name="method" value="updateGameMethodPrize" />
    <div class="blank-line"></div>
    <%int listIndex = 0; foreach (var gmg in gameMethodGroupList)
      { %>
    <table class="table-pro table-color-row" width="100%">
        <thead>
            <tr class="table-pro-head">
                <th><%:gmg.gmg003%></th>
                <th>编号</th>
                <th>最大奖金<input type="text" class="input-text w100px p_max" value="0" /></th>
                <th>最小奖金<input type="text" class="input-text w100px p_min" value="0" /></th>
                <th>差值</th>
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
                <td class="text-right w100px">
                    <%:gm.gm004%>
                    <input type="hidden" name="[<%:listIndex %>].gpd001" value="<%:gameMethodPrizeDataList[gm.gm001].gpd001%>" />
                    <input type="hidden" name="[<%:listIndex %>].gc001" value="<%:gameMethodPrizeDataList[gm.gm001].gc001%>" />
                    <input type="hidden" name="[<%:listIndex %>].gp001" value="<%:gameMethodPrizeDataList[gm.gm001].gp001%>" />
                    <input type="hidden" name="[<%:listIndex %>].gm001" value="<%:gameMethodPrizeDataList[gm.gm001].gm001%>" />
                    <input type="hidden" name="[<%:listIndex %>].gm002" value="<%:gameMethodPrizeDataList[gm.gm001].gm002%>" />
                    <input type="hidden" name="[<%:listIndex %>].gpd004" value="<%:gameMethodPrizeDataList[gm.gm001].gpd004%>" />
                    <input type="hidden" name="[<%:listIndex %>].gpd005" value="<%:gameMethodPrizeDataList[gm.gm001].gpd005%>" />
                    <input type="hidden" name="[<%:listIndex %>].gpd006" value="<%:gameMethodPrizeDataList[gm.gm001].gpd006%>" />
                    <input type="hidden" name="[<%:listIndex %>].gpd007" value="<%:gameMethodPrizeDataList[gm.gm001].gpd007%>" />
                    <input type="hidden" name="[<%:listIndex %>].gpd008" value="<%:gameMethodPrizeDataList[gm.gm001].gpd008%>" />
                    <input type="hidden" name="[<%:listIndex %>].gpd009" value="<%:gameMethodPrizeDataList[gm.gm001].gpd009%>" />
                </td>
                <td class="w100px"><span class="tips"><%:gm.gm001%></span></td>
                <td class="w150px"><input type="text" name="[<%:listIndex %>].gpd002" class="input-text w100px value_max" value="<%:gameMethodPrizeDataList[gm.gm001].gpd002%>" /></td>
                <td class="w150px"><input type="text" name="[<%:listIndex %>].gpd003" class="input-text w100px value_min" value="<%:gameMethodPrizeDataList[gm.gm001].gpd003%>" /></td>
                <td class="w100px fc-red"><%: gameMethodPrizeDataList[gm.gm001].gpd002-gameMethodPrizeDataList[gm.gm001].gpd003%></td>
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
    <div id="cjlsoft-bottom-function">
        <input type="submit" value="保存" class="cjlsoft-post-loading btn-normal" />
    </div>
    </form>
    <script type="text/javascript">
        $(".p_max").change(function ()
        {
            $(this).parents("table").find(".value_max").val($(this).val());
        });
        $(".p_min").change(function ()
        {
            $(this).parents("table").find(".value_min").val($(this).val());
        });
    </script>
</asp:Content>
