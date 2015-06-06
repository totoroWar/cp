<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var methodType = (string)ViewData["MethodType"];
    %>
    <div class="ui-page-content-body-header tools">
        <div class="left-nav">
            <a href="javascript:window.parent.parent.ui_show_tab('充值','/UI/Charge',true);" title="充值">充值</a>
            <a href="javascript:window.parent.parent.ui_show_tab('提现','/UI/Withdraw',true,false);" title="提现">提现</a>
            <a href="/UI/Bank?method=ic" title="转账" <%=methodType == "ic" ? "class='item-select'" : "" %>>转账</a>
            <a href="/UI/Bank?method=crhl" title="充值记录" <%=methodType == "crhl" ? "class='item-select'" : "" %>>充值记录</a>
            <a href="/UI/Bank?method=wrhl" title="提现记录" <%=methodType == "wrhl" ? "class='item-select'" : "" %>>提现记录</a>
            <a href="/UI/Bank?method=ichl" title="转账记录" <%=methodType == "ichl" ? "class='item-select'" : "" %>>转账记录</a>
        </div>
    </div>
    <div class="blank-line"></div>
    <%if( "crhl" == methodType){ %>
    <%
          var crhlList = (List<DBModel.wgs019>)ViewData["CRHLList"];
          var bList = (Dictionary<int,DBModel.wgs010>)ViewData["BList"];
    %>
    <table class="table-pro" width="100%">
        <tr>
            <td class="title">所有充值金额</td>
            <td><%:((decimal)ViewData["csumx0"]).ToString("N2") %></td>
        </tr>
        <tr>
            <td class="title">成功充值金额</td>
            <td><%:((decimal)ViewData["csum10"]).ToString("N2") %></td>
        </tr>
        <tr>
            <td class="title">成功实际金额</td>
            <td><%:((decimal)ViewData["csum11"]).ToString("N2") %></td>
        </tr>
        <tr>
            <td class="title">取消充值金额</td>
            <td><%:((decimal)ViewData["csum20"]).ToString("N2") %></td>
        </tr>
    </table>
    <div class="blank-line"></div>
    <table class="table-pro table-color-row tp5" width="100%">
        <thead>
        <tr class="table-pro-head">
            <th>充值码</th>
            <th>充值类型</th>
            <th>金额</th>
            <th>到账金额</th>
            <th>充值时间</th>
            <th>状态</th>
            <th>备注</th>
        </tr>
        </thead>
        <tbody>
            <%if( null != crhlList)
              { 
                  foreach(var item in crhlList)
                  {
                  %>
            <tr>
            <td class="w100px"><%:item.uc005 %></td>
            <td class="w100px"><%:bList[item.sb001].sb003 %></td>
            <td class="fc-red w150px"><%:string.Format("{0:N2}",item.uc002) %></td>
            <td class="fc-green w150px"><%:string.Format("{0:N2}",item.uc003) %></td>
            <td class="w120px"><%:item.uc006 %></td>
            <td><%switch (item.uc008) 
                  {
                      case 0:
                          Response.Write("<span style='color:red;'>处理中</span>");
                      break;
                      case 1:
                      Response.Write("<span style='color:green;'>已到账</span>");
                      break;
                      case 2:
                      Response.Write("<span style='color:gray;'>已取消</span>");
                      break; 
                  } %></td>
                <td><%:item.uc012 %></td>
                </tr>
            <%
            }/*foreach*/
            }/*if*/ %>
        </tbody>
        <tfoot class="dom-hide">
            <tr>
                <td colspan="8"></td>
            </tr>
        </tfoot>
    </table>
    <%=ViewData["PageList"] %>
    <%}/*crhl*/ %>
    <%else if( "wrhl" == methodType)
      {
          var wcDataList = (List<DBModel.wgs020>)ViewData["WCDataList"];
           %>
    <table class="table-pro table-color-row tp5" width="100%">
        <thead>
        <tr class="table-pro-head">
            <th>编号</th>
            <th>提现类型</th>
            <th>金额</th>
            <th>提现时间</th>
            <th>状态</th>
            <th>备注</th>
        </tr>
        </thead>
        <tbody>
            <%if (null != wcDataList)
              {
                  foreach (var item in wcDataList)
                  {
                  %>
            <tr>
            <td class="w100px"><%:item.uw001 %></td>
            <td class="w120px"><%:item.uw009 %></td>
            <td class="fc-red w150px"><%:string.Format("{0:N2}",item.uw002) %></td>
            <td class="w120px"><%:item.uw005 != null ? item.uw005.ToString() : "" %></td>
            <td><%switch (item.uw006) 
                  {
                      case 0:
                          Response.Write("<span style='color:red;'>处理中</span>");
                      break;
                      case 1:
                      Response.Write("<span style='color:green;'>已到账</span>");
                      break;
                      case 2:
                      Response.Write("<span style='color:gray;'>已取消</span>");
                      break; 
                  } %></td>
                <td><%:item.uw008 %></td>
                </tr>
            <%
            }/*foreach*/
            }/*if*/ %>
        </tbody>
        <tfoot class="dom-hide">
            <tr>
                <td colspan="8"></td>
            </tr>
        </tfoot>
    </table>
    <%=ViewData["PageList"] %>
    <%} %>
    <%else if( "ic"== methodType) {%>
        <form action="/UI/Bank" method="post" id="form_charge">
        <%:Html.AntiForgeryToken() %>
        <input type="hidden" name="method" value="ic" />
    <table class="table-pro w100ps tp5">
        <tr>
            <td class="title">验证码</td>
            <td>
                <input class="cw_vcode cvc h-point" type="text" style="width:150px; height:50px; border:none;" readonly="readonly" /><br /><span class="cvc h-point">[换一张]</span><br /><input type="text" class="input-text w200px" name="cw_vcode" value="" />
            </td>
        </tr>
        <tr>
            <td class="title">资金密码</td>
            <td><input type="password" class="input-text w200px" name="cw_pwd" value="" /></td>
        </tr>
        <tr>
            <td class="title">收款账号</td>
            <td><input type="text" class="input-text w200px" name="cw_acct" value="" /></td>
        </tr>
        <tr>
            <td class="title">确认账号</td>
            <td><input type="text" class="input-text w200px" name="cw_facct" value="" /></td>
        </tr>
        <tr>
            <td class="title">转账金额</td>
            <td><input type="text" class="input-text w200px" name="cw_amt" value="" /></td>
        </tr>
        <tr>
            <td class="title"></td>
            <td><input type="button" id="btn_charge" class="btn-normal" value="确认" /></td>
        </tr>
    </table>
            </form>
    <script type="text/javascript">
        function set_charge_code() {
            $(".cw_vcode").css("background-image", "url(/UI/CWVCode?r=" + Math.random() + ")");
        }

        $(document).ready(function () {
            set_charge_code();

            $(".cvc").click(function () { set_charge_code(); });
            //$("#cg-money").keyup(function () { $(".cn-money").html(_global_D2B($(this).val())); });
            $("#btn_charge").click(function () {
                var form_data = $("#form_charge").serialize();
                $.ajax({
                    async: false, timeout: _global_ajax_timeout, type: "POST", cache: false, url: "/UI/Bank", data: form_data, dataType: "text",
                    success: function (a, b) {
                        _check_auth(a);
                        var _robj;
                        eval("_robj=" + a + ";");
                        if (0 == _robj.Code)
                        {
                            alert(_robj.Message);
                        }
                        else if (1 == _robj.Code)
                        {
                            alert("转账申请已经提交");
                            refresh_current_page();
                        }
                    },
                    complete: function (a, b) {
                        ui_mask_panel_close();
                        set_charge_code();
                    }
                });
            });
        });
    </script>
    <%} %>
    <%else if("ichl" == methodType) {%>
    <%
          var icList = (List<DBModel.wgs043>)ViewData["ICList"];
          var type = (int)ViewData["Type"];
          var dts = (DateTime?)ViewData["DTS"];
          var dte = (DateTime?)ViewData["DTE"];
          Dictionary<int, string> icStatus = new Dictionary<int, string>() { { 0, "未审核" }, { 1, "已审核" }, { 2, "已取消" } };
    %>
     <div class="block_tools">
        <form action="/UI/Bank" id="form_order_default" method="get">
            <input name="method" type="hidden" value="ichl" />
        类型
        <select name="type" id="type">
            <option value="0" title="所有">所有</option>
            <option value="1" title="转出" <%=type == 1 ? "selected='selected'" : "" %>>转出</option>
            <option value="2" title="收到" <%=type == 2 ? "selected='selected'" : "" %>>收到</option>
        </select>
        时间范围<input type="text" name="dts" id="dts" class="input-text 120px" value="<%=dts %>" />-<input type="text" name="dte" id="dte" class="input-text 120px" value="<%:dte %>" />
        <input type="submit" value="查找" class="btn-normal ui-post-loading" />
    </form>
    </div>
    <div class="block_tools"></div>
    <table class="table-pro w100ps tp5">
        <thead>
            <tr class="table-pro-head">
                <th class="w100px">编号</th>
                <th class="w120px">转账者</th>
                <th class="w120px">收账者</th>
                <th class="w120px">金额</th>
                <th>时间</th>
                <th>状态</th>
            </tr>
        </thead>
        <tbody>
            <%if( null != icList) 
              {
                  foreach(var item in icList)
                      {
                  %>
            <tr>
                <td><%:item.tf001 %></td>
                <td><%:item.tf003 %></td>
                <td><%:item.tf006 %></td>
                <td><%:item.tf008.ToString("N2") %></td>
                <td><%:item.tf009 %></td>
                <td><%:icStatus[item.tf012] %></td>
            </tr>
            <%} /*for*/
              }/*if*/%>
        </tbody>
    </table>
    <%=ViewData["PageList"] %>
    <%} %>
    <script type="text/javascript">
        $('#dts,#dte').appendDtpicker({ locale: "cn", dateFormat: "YYYY/MM/DD hh:mm" });
    </script>
</asp:Content>
