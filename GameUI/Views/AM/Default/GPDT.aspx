<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var gameClassList = (List<DBModel.wgs006>)ViewData["GameClassList"];
        var gameClassID = (int)ViewData["GameClassID"];
        var groupList = (List<DBModel.wgs028>)ViewData["GroupList"];
        var gpdDataList = (List<DBModel.wgs028>)ViewData["GPDDataList"];
        //gpdDataList = gpdDataList.Where(exp => exp.gtp009 == 1).ToList();
    %>
    <div class="cjlsoft-body-header">
        <h1>奖金数据</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
    </div>
    <div class="blank-line"></div>
    <form action="#" method="post">
        <%:Html.AntiForgeryToken() %>
        <input type="hidden" name="method" value="add" />
        <table class="table-pro" width="100%">
            <tr>
                <td class="title">游戏分类</td>
                <td>
                    <select id="game_class" name="gc001" class="drop-select-to-url">
                        <%foreach (var gc in gameClassList)
                          { %>
                        <option value="<%:gc.gc001 %>" title="<%:gc.gc003 %>" tourl="/AM/GamePrizeData?gameClassID=<%:gc.gc001 %>" <%:gameClassID == gc.gc001 ? "selected='selected'" : "" %>><%:gc.gc003 %></option>
                        <%} %>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="title">组名称</td>
                <td>
                    <select id="exists_group" name="gtp008">
                        <option value="0">请选择组</option>
                        <%foreach (var g in groupList)
                          { %>
                        <option value="<%:g.gtp001 %>"><%:g.gtp003 %></option>
                        <%} %>
                    </select>
                    <br />
                    <input type="text" name="gtp010" value="" />
                </td>
            </tr>
            <tr>
                <td class="title">名称</td>
                <td>
                    <input type="text" name="gtp002" value="" /></td>
            </tr>
            <tr>
                <td class="title">显示名称</td>
                <td>
                    <input type="text" name="gtp003" value="" /></td>
            </tr>
            <tr>
                <td class="title">最大奖金</td>
                <td>
                    <input type="text" name="gtp004" value="0" /></td>
            </tr>
            <tr>
                <td class="title">最小奖金</td>
                <td>
                    <input type="text" name="gtp005" value="0" /></td>
            </tr>
            <tr>
                <td class="title">父级</td>
                <td>
                    <input type="text" name="gtp008" value="0" /><span class="tips">父级，如果为0即是无父级</span> </td>
            </tr>
            <tr>
                <td class="title">作为父级</td>
                <td>
                    <input type="text" name="gtp009" value="0" /><span class="tips">1作为父级</span> </td>
            </tr>
            <tr>
                <td class="title"></td>
                <td>
                    <input type="button" id="init_data" value="确认" class="btn-normal" /></td>
            </tr>
        </table>
    </form>
    <div class="blank-line"></div>
    <form action="/AM/Menu" method="post">
        <%:Html.AntiForgeryToken()%>
        <input type="hidden" name="method" value="updateList" />
        <table width="100%" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th>编号</th>
                    <th>名称</th>
                    <th>显示名称</th>
                    <th class="dom-hide">Max</th>
                    <th class="dom-hide">Min</th>
                    <th>对应SJID</th>
                    <th>状态</th>
                    <th>父</th>
                    <th>排序</th>
                </tr>
            </thead>
            <tbody>
                <%if (null != groupList)
                  {
                      var listIndex = 0;
                      foreach (var group in groupList)
                      { 
                %>
                <tr class="table-pro-head">
                    <td colspan="7" class="fc-red fs14px"><%:group.gtp003 %>(<%:group.gtp001 %>)</td>
                </tr>
                <%
                          if (null != gpdDataList)
                          {
                              var newData = gpdDataList.Where(exp =>exp.gtp008 == group.gtp001).ToList();
                              foreach (var item in newData)
                              {
                %>
                <tr>
                    <td>
                        <input type="hidden" name="[<%:listIndex %>].gc001" value="<%:item.gc001 %>" />
                        <input type="hidden" name="[<%:listIndex %>].gtp009" value="<%:item.gtp009 %>" />
                        <input type="hidden" name="[<%:listIndex %>].gtp001" value="<%:item.gtp001 %>" /><%:item.gtp001 %></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].gtp002" value="<%:item.gtp002.Trim() %>" class="input-text w150px" /></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].gtp003" value="<%:item.gtp003.Trim() %>" class="input-text w150px" /></td>
                    <td class="dom-hide">
                        <input type="text" name="[<%:listIndex %>].gtp004" value="<%:item.gtp004 %>" class="input-text w100px" /></td>
                    <td class="dom-hide">
                        <input type="text" name="[<%:listIndex %>].gtp005" value="<%:item.gtp005 %>" class="input-text w100px" /></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].gtp010" value="<%:item.gtp010 %>" class="input-text w100px" /></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].gtp011" value="<%:item.gtp011 %>" class="input-text w50px" /></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].gtp008" value="<%:item.gtp008 %>" class="input-text w50px" /></td>
                    <td>
                        <input type="text" name="[<%:listIndex %>].gtp012" value="<%:item.gtp012 %>" class="input-text w80px" /></td>
                </tr>
                <%
                            listIndex++;
                        }/*foreach*/
                    }/*if*/ %>
                <%
                      }/*foreach group*/
                  } /*if group*/%>
            </tbody>
        </table>
        <div id="cjlsoft-bottom-function">
            <input type="button" id="btn_save_list" value="保存" class="btn-normal" />
        </div>
    </form>
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $("#game_class").change(function ()
            {
                $("#game_method").empty();
                var gcid = $(this).val();
                var gmlist = $("#game_method_hide option");
                for (var i = 0; i < gmlist.length; i++)
                {
                    if (gcid == $(gmlist[i]).attr("gc001"))
                    {
                        var value = $(gmlist[i]).val();
                        var text = $(gmlist[i]).text();
                        $("#game_method").append("<option value='" + value + "'>" + text + "</option>");
                    }
                }
            });

            $("#exists_group").change(function ()
            {
                $("input[name='gtp010']").val($(this).val());
            });

            $(".btn-normal").click(function ()
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
        });
    </script>
</asp:Content>
