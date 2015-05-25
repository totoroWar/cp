<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs021>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var dctList = (Dictionary<int, DBModel.SysDataChangeType>)ViewData["DCTList"];
        var dctType = (byte)ViewData["DCTType"];
        var gmList = (List<DBModel.wgs002>)ViewData["GameMethodList"];
        var gList = (List<DBModel.wgs001>)ViewData["GameList"];
        var gmDicList = gmList.ToDictionary(key => key.gm001);
        var gDicList = gList.ToDictionary(key => key.g001);
    %>
    <div class="cjlsoft-body-header">
        <h1>账变</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
    </div>
    <div class="blank-line"></div>
    <div class="xtool">
        <form action="/AM/DataChange" method="get">
        <span>
            <select name="dcttype">
                <option value="0">全部</option>
                <%foreach(var item in dctList){ %>
                <option value="<%:item.Value.ID %>" <%=dctType == item.Value.ID ? "selected='selected'" : "" %>><%:item.Value.Name %></option>
                <%} %>
            </select>
        </span>
        <span>
            账号<input type="text" class="input-text w100px" name="account" value="<%:ViewData["Account"] %>" />
        </span>
        <span>
            账号<input type="text" class="input-text w100px" name="key" value="<%:ViewData["Key"] %>" />
        </span>
        <span>
            开始日期<input id="i_dts" name="dts" type="text" class="input-text w80px" value="<%:((DateTime)ViewData["DTS"]).ToString(ViewContext.ViewData["SysDateFormat"].ToString()) %>" />
        </span>
        <span>
            结束日期<input id="i_dte" name="dte" type="text" class="input-text w80px" value="<%:((DateTime)ViewData["DTE"]).ToString(ViewContext.ViewData["SysDateFormat"].ToString()) %>" />
        </span>
        <input id="btn_query" type="submit" value="查找" class="btn-normal" />
        </form>
    </div>
    <div class="blank-line"></div>
    <form action="/AM/Menu" method="post">
        <%:Html.AntiForgeryToken()%>
        <input type="hidden" name="method" value="updateList" />
        <table width="100%" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th>编号</th>
                    <th>类型</th>
                    <th>账号</th>
                    <th>金额</th>
                    <th>改变前金额</th>
                    <th>改变后金额</th>
                    <th>冻结金额</th>
                    <th>改变前冻结金额</th>
                    <th>改变后冻结金额</th>
                    <th>积分</th>
                    <th>改变前积分</th>
                    <th>改变后积分</th>
                    <th>订单编号</th>
                    <th>游戏</th>
                    <th>玩法</th>
                    <th>充值编号</th>
                    <th>提现编号</th>
                    <th>账变时间</th>
                    <th>备注</th>
                </tr>
            </thead>
            <tbody>
                <%if (null != Model)
                  {
                      int listIndex = 0;
                      foreach (var item in Model)
                      { %>
                <tr>
                    <td><%:item.uxf001 %></td>
                    <td><%:item.uxf015 %></td>
                    <td title="<%:item.u003 != null ? item.u003.Trim() : "" %>"><%:item.u002.Trim() %></td>
                    <td class="fc-red"><%:item.uxf003.ToString(ViewContext.ViewData["SysMoneyFormat"].ToString()) %></td>
                    <td><%:item.uxf002.ToString(ViewContext.ViewData["SysMoneyFormat"].ToString()) %></td>
                    <td><%:item.uxf007.ToString(ViewContext.ViewData["SysMoneyFormat"].ToString()) %></td>
                    <td><%:item.uxf012.ToString(ViewContext.ViewData["SysMoneyFormat"].ToString()) %></td>
                    <td><%:item.uxf010.ToString(ViewContext.ViewData["SysMoneyFormat"].ToString()) %></td>
                    <td><%:item.uxf009.ToString(ViewContext.ViewData["SysMoneyFormat"].ToString()) %></td>
                    <td><%:item.uxf005.ToString(ViewContext.ViewData["SysMoneyFormat"].ToString()) %></td>
                    <td><%:item.uxf004.ToString(ViewContext.ViewData["SysMoneyFormat"].ToString()) %></td>
                    <td><%:item.uxf008.ToString(ViewContext.ViewData["SysMoneyFormat"].ToString()) %></td>
                    <td><%:item.so001 %></td>
                    <td><%=item.g001 == 0 ? "0" : gDicList[item.g001].g003.Trim() %></td>
                    <td><%=item.gm001 == 0 ? "0" : gmDicList[item.gm001].gm004.Trim() %></td>
                    <td><%:item.uc001 %></td>
                    <td><%:item.uw001 %></td>
                    <td><%:item.uxf014.ToString(ViewContext.ViewData["SysDateTimeFormat"].ToString()) %></td>
                    <td><a href="javascript:void(0);" class="show_data" data="<%:item.uxf017 == null ? string.Empty : item.uxf017.Trim() %>">查看</a></td>
                </tr>
                <%
                      listIndex++;
                  }/*foreach*/
              }/*if*/ %>
            </tbody>
        </table>
        <%=ViewData["PageList"] %>
    </form>
    <div id="dlg_detial_info" class="dom-hide ui_block_dlg h100px">
    </div>
        <script type="text/javascript">
            $(document).ready(function ()
            {
                jQuery('#i_dts').datetimepicker({
                    format: 'Y/m/d',
                    lang: "ch",
                    onShow: function (ct) {
                        this.setOptions({
                            maxDate: jQuery('#i_dte').val() ? jQuery('#i_dte').val() : false
                        })
                    },
                    timepicker: false
                });
                jQuery('#i_dte').datetimepicker({
                    format: 'Y/m/d',
                    lang: "ch",
                    onShow: function (ct) {
                        this.setOptions({
                            minDate: jQuery('#i_dts').val() ? jQuery('#i_dts').val() : false
                        })
                    },
                    timepicker: false
                });

                $(".show_data").click(function ()
                {
                    var data = $(this).attr("data");
                    $("#dlg_detial_info").html(data);
                    _global_ui("dlg_detial_info", 300, 30, 20, 0, "备注");
                });
            });
    </script>
</asp:Content>