<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %><asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content><asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var methodType = (string)ViewData["MethodType"];
    var wcList = (List<DBModel.wgs023>)ViewData["WCList"];
    var dicBankList = (Dictionary<int, DBModel.wgs010>)ViewData["BankDicList"];
    var wtDicList = (Dictionary<int,DBModel.wgs024>)ViewData["WTDicList"];
    var authStatus = (int)ViewData["AuthStatus"];

    if (methodType == "") methodType = "GetWit";
        
%>

    <div class="ui-page-content-body-header tools">
        <div class="left-nav">
            <a href="/UI/Withdraw?method=GetWit" title="余额提现" <%=methodType == "GetWit" ? "class='item-select'" : "" %>>余额提现</a>
            <a href="/UI/Withdraw?method=FHWit" title="分红提现" <%=methodType == "FHWit" ? "class='item-select'" : "" %>>分红提现</a>
            <!--<a href="/UI/Withdraw?method=YJWit" title="佣金提现" <%=methodType == "YJWit" ? "class='item-select'" : "" %>>佣金提现</a>-->
        </div>
    </div>
    


<%if( 0 == authStatus){

    var data = new ViewDataDictionary();
    data.Add("Message", ViewData["AuthMessage"]);
%>
<%=Html.Partial(ViewData["ControllerTheme"]+"/Common/Message",data) %>

<%} %>

<%else if( 1 == authStatus){
      if ("GetWit" == methodType)
      {
            %>
      
      
<form action="/UI/Withdraw" method="post" id="form_withdraw">
        <%:Html.AntiForgeryToken() %>
        <table class="table-pro tp5 w100ps">
            <%if( "1" == (string)ViewData["NCWVCode"]){ %>
            <tr>
                <td class="title">验证码</td>
                <td><input class="cw_vcode cvc h-point" type="text" style="width:150px; height:50px; border:none;" readonly="readonly" /><br /><span class="cvc h-point">[换一张]</span><br /><input type="text" class="input-text w200px" name="cw_vcode" value="" /></td>
            </tr>
            <%} %>
            <tr>
                <td class="title">提现银行</td>
                <td>
                    <select name="wct">
                        <%foreach(var item in wcList)
                          { %>
                        <option value="<%:item.uwi001 %>"><%:wtDicList[item.uwt001].uwt002 %>/<%:item.uwi003 %>/<%:item.uwi004 %>/<%:item.uwi005 %></option>
                        <%} %>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="title">可用余额</td>
                <td><%:((decimal)ViewData["AvailableMoney"]).ToString("N2") %><span class="tips"><script type="text/javascript">
                                                                           document.write(_global_D2B(<%:ViewData["AvailableMoney"] %>));
                                                                       </script></span></td>
            </tr>
            <tr>
                <td class="title">冻结金额</td>
                <td><%:((decimal)ViewData["LockMoney"]).ToString("N2") %><span class="tips"><script type="text/javascript">
                                                                      document.write(_global_D2B(<%:ViewData["LockMoney"] %>));
                                                                       </script></span></td>
            </tr>
            <tr>
                <td class="title">可用佣金</td>
                <td><%:((decimal)ViewData["CommissionMoney"]).ToString("N2") %><span class="tips"><script type="text/javascript">
                                                                                                      document.write(_global_D2B(<%:ViewData["CommissionMoney"] %>));
                                                                       </script></span></td>
            </tr>
            <tr>
                <td class="title">资金密码</td>
                <td>
                    <input type="password" class="input-text w200px" name="char_password" value="" /></td>
            </tr>
            <tr>
                <td class="title"></td>
                <td class="cn-money"></td>
            </tr>
            <tr>
                <td class="title">提现金额</td>
                <td><input type="text" class="input-text w200px  fc-yellow" id="wc_money" name="wc_money" value="" /><span class="tipsline"><%:ViewData["WCMin"]  %></span></td>
            </tr>
            <tr>
                <td class="title">提现金类型</td>
                <td><input type="radio" value="0" name="MoneyType" checked="checked" />可用金额</td>
            </tr>
            <tr>
                <td class="title"></td>
                <td>
                    <input id="btn_withdraw" type="button" class="btn-normal" value="确认" /></td>
            </tr>
        </table>
    </form>
    <script type="text/javascript">
        $("#wc_money").keyup(function () { $(".cn-money").html(_global_D2B($(this).val())); });
        function set_charge_code()
        {
            $(".cw_vcode").css("background-image", "url(/UI/CWVCode?r=" + Math.random() + ")");
        }
        $(".cvc").click(function () { set_charge_code(); });
        set_charge_code();
        $("#btn_withdraw").click(function ()
        {
            var form_data = $(this).parents("form").serialize();
            $.ajax({
                async: false, timeout: _global_ajax_timeout, type: "POST", cache: false, url: "/UI/Withdraw", data: form_data, dataType: "json",
                success: function (a, b)
                {
                    _check_auth(a);
                    alert(a.Message);
                    if( 1 == a.Code)
                    {
                        location.reload();
                    }
                },
                complete: function (a, b)
                {
                    //alert(b);
                    ui_mask_panel_close();
                    set_charge_code();
                }
            });
        });
    </script>
<%

    }
        else if ("FHWit" == methodType)
  {
  %>
    
    <form action="/UI/Withdraw" method="post" id="form1">
        <%:Html.AntiForgeryToken() %>
        <table class="table-pro tp5 w100ps">
            <%if( "1" == (string)ViewData["NCWVCode"]){ %>
            <tr>
                <td class="title">验证码</td>
                <td><input class="cw_vcode cvc h-point" type="text" style="width:150px; height:50px; border:none;" readonly="readonly" /><br /><span class="cvc h-point">[换一张]</span><br /><input type="text" class="input-text w200px" name="cw_vcode" value="" /></td>
            </tr>
            <%} %>
            <tr>
                <td class="title">提现银行</td>
                <td>
                    <select name="wct">
                        <%foreach(var item in wcList)
                          { %>
                        <option value="<%:item.uwi001 %>"><%:wtDicList[item.uwt001].uwt002 %>/<%:item.uwi003 %>/<%:item.uwi004 %>/<%:item.uwi005 %></option>
                        <%} %>
                    </select>
                </td>
            </tr>

            <tr>
                <td class="title">分红金额</td>
                <td><%:((decimal)ViewData["FHMoney"]).ToString("N2") %><span class="tips"><script type="text/javascript">
                                                                                              document.write(_global_D2B(<%:ViewData["FHMoney"] %>));
                                                                       </script></span></td>
            </tr>
            <tr>
                <td class="title">资金密码</td>
                <td>
                    <input type="password" class="input-text w200px" name="char_password" value="" /></td>
            </tr>
            <tr>
                <td class="title"></td>
                <td class="cn-money"></td>
            </tr>
            <tr>
                <td class="title">提取分红金额</td>
                <td><input type="text" class="input-text w200px " id="wc_money" name="wc_money" value="" value="" /><span class="tipsline"><%:ViewData["WCMin"]  %></span></td>
            </tr>
            <tr>
                <td class="title"></td>
                <td>
                    <input id="btn_withdraw" type="button" class="btn-normal" value="确认" /></td>
            </tr>
        </table>
    </form>
    <script type="text/javascript">
        $("#wc_money").keyup(function () { $(".cn-money").html(_global_D2B($(this).val())); });
        function set_charge_code() {
            $(".cw_vcode").css("background-image", "url(/UI/CWVCode?w=1&r=" + Math.random() + ")");
        }
        $(".cvc").click(function () { set_charge_code(); });
        set_charge_code();
        $("#btn_withdraw").click(function () {
            var form_data = $(this).parents("form").serialize();
            $.ajax({
                async: false, timeout: _global_ajax_timeout, type: "POST", cache: false, url: "/UI/Withdraw?w=1", data: form_data, dataType: "json",
                success: function (a, b) {
                    _check_auth(a);
                    alert(a.Message);
                    if (1 == a.Code) {
                        location.reload();
                    }
                },
                complete: function (a, b) {
                    //alert(b);
                    ui_mask_panel_close();
                    set_charge_code();
                }
            });
        });
    </script>

<%  }
      else if ("YJWit" == methodType)
      {
      %>
<form action="/UI/Withdraw" method="post" id="form_withdraw">
        <%:Html.AntiForgeryToken() %>
        <table class="table-pro tp5 w100ps">
            <%if( "1" == (string)ViewData["NCWVCode"]){ %>
            <tr>
                <td class="title">验证码</td>
                <td><input class="cw_vcode cvc h-point" type="text" style="width:150px; height:50px; border:none;" readonly="readonly" /><br /><span class="cvc h-point">[换一张]</span><br /><input type="text" class="input-text w200px" name="cw_vcode" value="" /></td>
            </tr>
            <%} %>
            <tr>
                <td class="title">提现银行</td>
                <td>
                    <select name="wct">
                        <%foreach(var item in wcList)
                          { %>
                        <option value="<%:item.uwi001 %>"><%:wtDicList[item.uwt001].uwt002 %>/<%:item.uwi003 %>/<%:item.uwi004 %>/<%:item.uwi005 %></option>
                        <%} %>
                    </select>
                </td>
            </tr>

            <tr>
                <td class="title">冻结金额</td>
                <td><%:((decimal)ViewData["CommissionMoney"]).ToString("N2") %><span class="tips"><script type="text/javascript">
                                                                                                document.write(_global_D2B(<%:ViewData["LockMoney"] %>));
                                                                       </script></span></td>
            </tr>
            <tr>
                <td class="title">资金密码</td>
                <td>
                    <input type="password" class="input-text w200px" name="char_password" value="" /></td>
            </tr>
            <tr>
                <td class="title"></td>
                <td class="cn-money"></td>
            </tr>
            <tr>
                <td class="title">提现金额</td>
                <td><input type="text" class="input-text w200px  fc-yellow" id="wc_money" name="wc_money" value="" /><span class="tipsline"><%:ViewData["WCMin"]  %></span></td>
            </tr>
            <tr>
                <td class="title">提现金类型</td>
                <td><input type="radio" value="1" name="MoneyType" checked="checked"/>佣金金额</td>
            </tr>
            <tr>
                <td class="title"></td>
                <td>
                    <input id="btn_withdraw" type="button" class="btn-normal" value="确认" /></td>
            </tr>
        </table>
    </form>
    <script type="text/javascript">
        $("#wc_money").keyup(function () { $(".cn-money").html(_global_D2B($(this).val())); });
        function set_charge_code() {
            $(".cw_vcode").css("background-image", "url(/UI/CWVCode?r=" + Math.random() + ")");
        }
        $(".cvc").click(function () { set_charge_code(); });
        set_charge_code();
        $("#btn_withdraw").click(function () {
            var form_data = $(this).parents("form").serialize();
            $.ajax({
                async: false, timeout: _global_ajax_timeout, type: "POST", cache: false, url: "/UI/Withdraw", data: form_data, dataType: "json",
                success: function (a, b) {
                    _check_auth(a);
                    alert(a.Message);
                    if (1 == a.Code) {
                        location.reload();
                    }
                },
                complete: function (a, b) {
                    //alert(b);
                    ui_mask_panel_close();
                    set_charge_code();
                }
            });
        });
    </script>
    <%
      }
  }

   %>
</asp:Content>