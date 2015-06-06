<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI2/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var methodType = (string)ViewData["MethodType"];
        var traceObject = (DBModel.TraceOrderDetail)ViewData["TraceObject"];
        var gameList = (List<DBModel.wgs001>)ViewData["GameList"];
        var gameDicList = gameList.ToDictionary(exp => exp.g001);
    %>
<div class="main_box main_tbg">
	<div class="main_table_bg">
    	<div class="main_table_box">
        	<!---个人资料 start-->
        	<div class="user_info_box">
<div class="user_info_data_box">
    <div class="user_add_bank">
<div class="bank_list_box">
            <div class="bank_list">
    <input type="hidden" name="key" value="<%:traceObject.TraceOrder.sto001 %>" />
    <table  class="ctable_box youxi_table_list" width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr >
            <td style="border-top:1px solid #dad8c5" class="title">追号单号</td>
            <td style="border-top:1px solid #dad8c5" class="fl_l"><%:traceObject.TraceOrder.sto001 %></td>
        </tr>
        <tr>
            <td class="title">账号</td>
            <td class="fl_l"><%:traceObject.TraceOrder.u002.Trim() %></td>
        </tr>
        <tr>
            <td class="title">游戏</td>
            <td class="fl_l"><%:gameDicList[traceObject.TraceOrder.g001].g003.Trim() %></td>
        </tr>
        <tr>
            <td class="title">时间</td>
            <td class="fl_l"><%:traceObject.TraceOrder.sto004.ToString(ViewContext.ViewData["SysDateTimeFormat"].ToString()) %></td>
        </tr>
        <tr>
            <td class="title">开始期号</td>
            <td class="fl_l"><%:traceObject.TraceOrder.sto010.Trim() %></td>
        </tr>
        <tr>
            <td class="title">结束期号</td>
            <td class="fl_l"><%:traceObject.TraceOrder.sto012.Trim() %></td>
        </tr>
        <tr>
            <td class="title">总单数</td>
            <td class="fl_l"><%:traceObject.OrderShowList.Count() %></td>
        </tr>
        <tr>
            <td class="title">总金额</td>
            <td class="fl_l"><%:traceObject.TraceOrder.sto002.ToString(ViewContext.ViewData["SysMoneyFormat"].ToString())%></td>
        </tr>
        <tr>
            <td class="title">实际金额</td>
            <td class="fl_l"><%:traceObject.TraceOrder.sto007.ToString(ViewContext.ViewData["SysMoneyFormat"].ToString())%></td>
        </tr>
        <tr>
            <td class="title">奖金</td>
            <td class="fl_l"><%:traceObject.TraceOrder.sto008.ToString(ViewContext.ViewData["SysMoneyFormat"].ToString())%></td>
        </tr>
        <tr>
            <td class="title">状态</td>
            <td class="fl_l">
                    <%
                      var toStatus = string.Empty;
                      switch (traceObject.TraceOrder.sto005)
                      {
                          case 0:
                              toStatus = "<span class='fc-red'>未完成</span>";
                              break;
                          case 1:
                              toStatus = "<span class='fc-green'>已完成</span>";
                              break;
                          case 2:
                              toStatus = "<span class='fc-gray'>已撤单</span>";
                              break; 
                      }
                        %><%=toStatus %>
                <%if( 0 == traceObject.TraceOrder.sto005){ %>
                <input type="button" id="cancel_all" class="btn_w60h25" value="全撤" />
                <%} %>
            </td>
        </tr>
        <tr>
            <td class="title">类型</td>
            <td class="fl_l"><%=traceObject.TraceOrder.sto009 == 0 ? "连续追号" : "追中即停" %></td>
        </tr>
        <tr>
            <td class="title">详情</td>
            <td class="fl_l">
                <table class="table-pro w100ps">
                    <tr class="table-pro-head">
                        <th class="w100px">单号</th>
                        <th class="w100px">玩法</th>
                        <th class="w80px">期号</th>
                        <th class="w100px">结果</th>
                        <th class="w30px">倍数</th>
                        <th class="w30px">注数</th>
                        <th class="w80px">总金额</th>
                        <th class="w80px">总奖金</th>
                        <th>内容</th>
                        <th>状态</th>
                        <th></th>
                    </tr>
                    <%
                        var pageKeys = string.Empty;
                        foreach(var item in traceObject.OrderShowList){ %>
                    <tr key="<%:item.projectid %>">
                        <td><%:item.projectid %></td>
                        <td><%:item.methodname %></td>
                        <td><%:item.issue %></td>
                        <td><%:item.nocode %></td>
                        <td><%:item.multiple %></td>
                        <td><%:item.times %></td>
                        <td class="fc-red"><%:item.totalprice.ToString("N2") %></td>
                        <td><%:item.bonus.ToString("N2")%></td>
                        <td>
                            <%:NETCommon.StringHelper.GetShortString(item.code,20, 20, "...") %>
                        </td>
                        <td><%:item.statusdesc %></td>
                        <td class="link-tools"><%if( item.iscancel == 0 && item.prizestatus == 0){ %><span name="cancel_one" class="btn_w60h25" data="<%:item.projectid %>">撤单</span><%} %></td>
                    </tr>
                    <%
                            pageKeys += item.projectid + ",";
                    }
                        pageKeys = pageKeys.Substring(0, pageKeys.Length - 1); 
                    %>
                </table>
                <input name="keyall" type="hidden" value="<%:pageKeys %>" />
            </td>
        </tr>
    </table>
    <%=ViewData["PageList"] %>
                </div>
    </div>
        </div>
    </div>
                </div>
            </div>
        </div>
    </div>
<script type="text/javascript">
        $(document).ready(function ()
        {
            function cancel_order(key)
            {
                if (false == confirm("确认执行？")) {
                    return;
                }
                $.ajax({
                    timeout: _global_ajax_timeout, cache: false, type: "POST", data: { lotteryid: 0, id: key, method:"cancelorder", type: "list" }, url: "/PlayInfo.html", success: function (a) {
                        _check_auth(a);
                        var _robj = eval(a);
                        if ("error" == _robj.stats) {
                            alert(_robj.data);
                        }
                        else if ("success" == _robj.stats) {
                            refresh_current_page();
                        }
                    }
                });
            }

            $("span[name='cancel_one']").click(function () {
                var key = $(this).attr("data");
                cancel_order(key);
            });

            $("#cancel_all").click(function ()
            {
                var key = $("input[name='keyall']").val();
                $.ajax({
                    timeout: _global_ajax_timeout, cache: false, type: "POST", data: { lotteryid: 0, id: key, method: "cancelTOrder", type: "list" }, url: "/PlayInfo.html", success: function (a) {
                        _check_auth(a);
                        var _robj = eval(a);
                        if ("error" == _robj.stats) {
                            alert(_robj.data);
                        }
                        else if ("success" == _robj.stats) {
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
