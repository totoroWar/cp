<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs007>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var gameClassList = (List<DBModel.wgs006>)ViewData["GameClassList"];
    var gameClassID = (int)ViewData["GameClassID"];
    var groupList = (List<DBModel.wgs028>)ViewData["GroupList"];
    var gpdDataList = (List<DBModel.wgs029>)ViewData["GPDDataList"];
        //gpdDataList = gpdDataList.Where(exp => exp.gtp009 == 1).ToList();
    var dicItemList = (Dictionary<int, DBModel.wgs028>)ViewData["DicItemList"];
%>
<div class="cjlsoft-body-header dom-hide">
        <h1></h1>
</div>
        <div class="blank-line"></div>
    <form action="/AM/GamePrizeData" method="post">
        <%:Html.AntiForgeryToken()%>
        <input type="hidden" name="method" value="updateSetGamePrizeData" />
    <div class="blank-line"></div>
    <%int listIndex = 0; foreach (var group in groupList)
      { %>
    <table class="table-pro table-color-row" width="100%">
        <thead>
            <tr class="table-pro-head">
                <th><%:group.gtp003.Trim() %>（<%:group.gtp001 %>）</th>
                <th>编号</th>
                <th>最大奖金<input type="text" class="input-text w100px p_max" value="0" /></th>
                <th>最小奖金<input type="text" class="input-text w100px p_min" value="0" /></th>
                <th>差值</th>
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
            <tr key="<%:gpd.gtp001 %>">
                <td class="text-right w80px fc-red">
                    <%:gpd.gtp001%>
                    <input type="hidden" name="[<%:listIndex %>].gc001" value="<%:gpd.gc001 %>" />
                    <input type="hidden" name="[<%:listIndex %>].gtp001" value="<%:gpd.gtp001 %>" />
                    <input type="hidden" name="[<%:listIndex %>].gp001" value="<%:gpd.gp001 %>" />                    
                    <input type="hidden" name="[<%:listIndex %>].gtpd001" value="<%:gpd.gtpd001 %>" />
                    <input type="hidden" name="[<%:listIndex %>].gtpd004" value="<%:gpd.gtpd004 %>" />
                    <input type="hidden" name="[<%:listIndex %>].gtpd005" value="<%:gpd.gtpd005 %>" />
                    <input type="hidden" name="[<%:listIndex %>].gtpd006" value="<%:gpd.gtpd006 %>" />
                </td>
                <td class="w100px fc-green"><%:dicItemList[gpd.gtp001].gtp003%></td>
                <td class="w150px"><input type="text" name="[<%:listIndex %>].gtpd002" class="input-text w100px value_max" value="<%:gpd.gtpd002 %>" /></td>
                <td class="w150px"><input type="text" name="[<%:listIndex %>].gtpd003" class="input-text w100px value_min" value="<%:gpd.gtpd003 %>" /></td>
                <td class="w100px fc-red"><%:gpd.gtpd002-gpd.gtpd003 %></td>
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
        <input type="button" id="btn_save_list" value="保存" class="btn-normal" />
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
        $("#btn_save_list").click(function ()
        {
            var form_data = $(this).parents("form").serialize();
            $.ajax({
                timeout: _global_ajax_timeout, cache: false, data: form_data, type: "POST", url: "/AM/GamePrizeData", dataType: "json", success: function (a)
                {
                    alert(a.Message);
                    if (0 == a.Code)
                    {
                    }
                    else if (1 == a.Code)
                    {
                    }
                }
            });
        });
    </script>
</asp:Content>
