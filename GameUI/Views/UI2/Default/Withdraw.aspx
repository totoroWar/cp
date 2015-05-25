<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI2/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var methodType = (string)ViewData["MethodType"];
    var wcList = (List<DBModel.wgs023>)ViewData["WCList"];
    var dicBankList = (Dictionary<int, DBModel.wgs010>)ViewData["BankDicList"];
    var wtDicList = (Dictionary<int,DBModel.wgs024>)ViewData["WTDicList"];
    var authStatus = (int)ViewData["AuthStatus"];
    if (methodType == "") methodType = "GetWit";    
%>
<div class="m_body_bg">
<div class="main_box main_tbg">
	<div class="main_table_bg">
    	<div class="main_table_box">
        	<!---个人资料 start-->
        	<div class="user_info_box">
            	<div class="user_info_tab">
                	<ul>
                    	<span><a class="info_close" href="#"></a></span>
                        <li class="on"><a href="/UI2/Withdraw?method=GetWit" title="余额提现" <%=methodType == "GetWit" ? "class='item-select'" : "" %>>余额提现</a></li>
                        <li><a href="/UI2/Withdraw?method=FHWit" title="分红提现" <%=methodType == "FHWit" ? "class='item-select'" : "" %>>分红提现</a></li>
                        <li><a href="/UI2/Withdraw?method=YJWit" title="佣金提现" <%=methodType == "YJWit" ? "class='item-select'" : "" %>>佣金提现</a></li>
                    </ul>
                </div>


                <div class="user_info_data_box">
                     <div class="bank_list_box">   
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

<form action="/UI2/Withdraw" method="post" id="form_withdraw">
<%:Html.AntiForgeryToken() %>
                        <div class="bank_list">
                            <table class="ctable_box youxi_table_list" border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tbody>
             <%if( "1" == (string)ViewData["NCWVCode"]){ %>
            <tr>
                <td class="title" style="border-top:1px solid #dad8c5">验证码</td>
                <td style="text-align:left;border-top:1px solid #dad8c5"><input type="text" class="input-text w200px" name="cw_vcode" value="" /><input class="cw_vcode cvc h-point" type="text" style="width:150px; height:50px; border:none;" readonly="readonly" /><span class="cvc h-point">[换一张]</span></td>
            </tr>
            <%} %>
<tr>
                <td class="title">提现银行</td>
                <td style="text-align:left">
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
                <td style="text-align:left"><%:((decimal)ViewData["AvailableMoney"]).ToString("N2") %><span class="tips"><script type="text/javascript">
                                                                                                     document.write(_global_D2B(<%:ViewData["AvailableMoney"] %>));
                                                                       </script></span></td>
            </tr>
            <tr>
                <td class="title">冻结金额</td>
                <td style="text-align:left"><%:((decimal)ViewData["LockMoney"]).ToString("N2") %><span class="tips"><script type="text/javascript">
                                                                                                document.write(_global_D2B(<%:ViewData["LockMoney"] %>));
                                                                       </script></span></td>
            </tr>
            <tr>
                <td class="title">可用佣金</td>
                <td style="text-align:left"><%:((decimal)ViewData["CommissionMoney"]).ToString("N2") %><span class="tips"><script type="text/javascript">
                                                                                                      document.write(_global_D2B(<%:ViewData["CommissionMoney"] %>));
                                                                       </script></span></td>
            </tr>
            <tr>
                <td class="title">资金密码</td>
                <td style="text-align:left">
                    <input type="password" class="input-text w200px" name="char_password" value="" /></td>
            </tr>
            <tr style="display:none">
                <td class="title"></td>
                <td class="cn-money"></td>
            </tr>
            <tr>
                <td class="title">提现金额</td>
                <td style="text-align:left"><input type="text" class="input-text w200px  fc-yellow" id="wc_money" name="wc_money" value="" /><span class="tipsline"><%:ViewData["WCMin"]  %></span></td>
            </tr>
            <tr>
                <td class="title">提现金类型</td>
                <td style="text-align:left"><input type="radio" value="0" name="MoneyType" checked="checked" />可用金额</td>
            </tr>
            <tr>
                <td class="title"></td>
                <td style="text-align:left">
                    <input id="btn_withdraw" type="button" class="btn-normal" value="确认" /></td>
            </tr>
                            </tbody></table>
                        </div>
</form>
    <script type="text/javascript">
        $("#wc_money").keyup(function () { $(".cn-money").html(_global_D2B($(this).val())); });
        function set_charge_code() {
            $(".cw_vcode").css("background-image", "url(/UI2/CWVCode?r=" + Math.random() + ")");
        }
        $(".cvc").click(function () { set_charge_code(); });
        set_charge_code();
        $("#btn_withdraw").click(function () {
            var form_data = $(this).parents("form").serialize();
            $.ajax({
                async: false, timeout: _global_ajax_timeout, type: "POST", cache: false, url: "/UI2/Withdraw", data: form_data, dataType: "json",
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
        else if ("FHWit" == methodType)
  {
  %>
<div class="bank_list">
    <form action="/UI/Withdraw" method="post" id="form1">
        <%:Html.AntiForgeryToken() %>
        <table  class="ctable_box youxi_table_list" border="0" cellpadding="0" cellspacing="0" width="100%">
            <%if( "1" == (string)ViewData["NCWVCode"]){ %>
            <tr>
                <td class="title" style="border-top:1px solid #dad8c5">验证码</td>
                <td style="text-align:left;border-top:1px solid #dad8c5"><input type="text" class="input-text w200px" name="cw_vcode" value="" /><input class="cw_vcode cvc h-point" type="text" style="width:150px; height:50px; border:none;" readonly="readonly" /><span class="cvc h-point">[换一张]</span></td>
            </tr>
            <%} %>
            <tr>
                <td class="title">提现银行</td>
                <td style="text-align:left">
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
                <td style="text-align:left"><%:((decimal)ViewData["FHMoney"]).ToString("N2") %><span class="tips"><script type="text/javascript">
                                                                                              document.write(_global_D2B(<%:ViewData["FHMoney"] %>));
                                                                       </script></span></td>
            </tr>
            <tr>
                <td class="title">资金密码</td>
                <td style="text-align:left">
                    <input type="password" class="input-text w200px" name="char_password" value="" /></td>
            </tr>
            <tr style="display:none">
                <td class="title"></td>
                <td class="cn-money"></td>
            </tr>
            <tr>
                <td class="title">提取分红金额</td>
                <td style="text-align:left"><input type="text" class="input-text w200px " id="wc_money" name="wc_money" value="" value="" /><span class="tipsline"><%:ViewData["WCMin"]  %></span></td>
            </tr>
            <tr>
                <td class="title"></td>
                <td style="text-align:left">
                    <input id="btn_withdraw" type="button" class="btn-normal" value="确认" /></td>
            </tr>
        </table>
    </form>
    </div>
    <script type="text/javascript">
        $("#wc_money").keyup(function () { $(".cn-money").html(_global_D2B($(this).val())); });
        function set_charge_code() {
            $(".cw_vcode").css("background-image", "url(/UI2/CWVCode?w=1&r=" + Math.random() + ")");
        }
        $(".cvc").click(function () { set_charge_code(); });
        set_charge_code();
        $("#btn_withdraw").click(function () {
            var form_data = $(this).parents("form").serialize();
            $.ajax({
                async: false, timeout: _global_ajax_timeout, type: "POST", cache: false, url: "/UI2/Withdraw?w=1", data: form_data, dataType: "json",
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

   %>
<%
        else if ("YJWit" == methodType)
  {
  %>
<div class="bank_list">
    <form action="/UI/Withdraw" method="post" id="form1">
        <%:Html.AntiForgeryToken() %>
        <table  class="ctable_box youxi_table_list" border="0" cellpadding="0" cellspacing="0" width="100%">
            <%if( "1" == (string)ViewData["NCWVCode"]){ %>
            <tr>
                <td class="title" style="border-top:1px solid #dad8c5">验证码</td>
                <td style="text-align:left;border-top:1px solid #dad8c5"><input type="text" class="input-text w200px" name="cw_vcode" value="" /><input class="cw_vcode cvc h-point" type="text" style="width:150px; height:50px; border:none;" readonly="readonly" /><span class="cvc h-point">[换一张]</span></td>
            </tr>
            <%} %>
            <tr>
                <td class="title">提现银行</td>
                <td style="text-align:left">
                    <select name="wct">
                        <%foreach(var item in wcList)
                          { %>
                        <option value="<%:item.uwi001 %>"><%:wtDicList[item.uwt001].uwt002 %>/<%:item.uwi003 %>/<%:item.uwi004 %>/<%:item.uwi005 %></option>
                        <%} %>
                    </select>
                </td>
            </tr>

            <tr>
                <td class="title">佣金金额</td>
                <td style="text-align:left"><%:((decimal)ViewData["CommissionMoney"]).ToString("N2") %><span class="tips"><script type="text/javascript">
                                                                                                                      document.write(_global_D2B(<%:ViewData["FHMoney"] %>));
                                                                       </script></span></td>
            </tr>
            <tr>
                <td class="title">资金密码</td>
                <td style="text-align:left">
                    <input type="password" class="input-text w200px" name="char_password" value="" /></td>
            </tr>
            <tr style="display:none">
                <td class="title"></td>
                <td class="cn-money"></td>
            </tr>
            <tr>
                <td class="title">提取佣金金额</td>
                <td style="text-align:left"><input type="text" class="input-text w200px " id="wc_money" name="wc_money" value="" value="" /><span class="tipsline"><%:ViewData["WCMin"]  %></span></td>
            </tr>
              <tr>
                <td class="title">提现金类型</td>
                <td style="text-align:left"><input type="radio" checked="checked" value="1" name="MoneyType" />佣金金额</td>
            </tr>
            <tr>
                <td class="title"></td>
                <td style="text-align:left">
                    <input id="btn_withdraw" type="button" class="btn-normal" value="确认" /></td>
            </tr>
        </table>
    </form>
    </div>
    <script type="text/javascript">
        $("#wc_money").keyup(function () { $(".cn-money").html(_global_D2B($(this).val())); });
        function set_charge_code() {
            $(".cw_vcode").css("background-image", "url(/UI2/CWVCode?r=" + Math.random() + ")");
        }
        $(".cvc").click(function () { set_charge_code(); });
        set_charge_code();
        $("#btn_withdraw").click(function () {
            var form_data = $(this).parents("form").serialize();
            $.ajax({
                async: false, timeout: _global_ajax_timeout, type: "POST", cache: false, url: "/UI2/Withdraw", data: form_data, dataType: "json",
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
  }
   %>
                    </div>

                </div>

            </div>
            <!---个人资料 end-->
            
        </div>
    </div>
</div>


</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
