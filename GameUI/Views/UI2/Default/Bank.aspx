<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI2/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var methodType = (string)ViewData["MethodType"];
    %>
<div class="m_body_bg">
<div class="main_box main_tbg">
	<div class="main_table_bg">
    	<div class="main_table_box">
        	<div class="user_info_box">
                
            	<div class="user_info_tab hd">
                	<ul>
                    	<span><a class="info_close" href="#"></a></span>
                        <li><a href="/UI2/Charge" <%=methodType == "crhl" ? "class='item-select'" : "" %>>充值</a></li>
                        <li><a href="/UI2/Withdraw?method=GetWit" <%=methodType == "wrhl" ? "class='item-select'" : "" %>>提现</a></li>
                        <li><a href="/UI2/Bank?method=ic" title="转账" <%=methodType == "ic" ? "class='item-select'" : "" %>>转账</a></li>
                        <li class="on"><a href="/UI2/Bank?method=crhl" <%=methodType == "crhl" ? "class='item-select'" : "" %>>充值记录</a></li>
                        <li><a href="/UI2/Bank?method=wrhl" <%=methodType == "wrhl" ? "class='item-select'" : "" %>>提现记录</a></li>
                        <li><a href="/UI2/Bank?method=ichl" <%=methodType == "ichl" ? "class='item-select'" : "" %>>转账记录</a></li>
                    </ul>
                </div>
                
<%if( "crhl" == methodType){ %>
    <%
          var crhlList = (List<DBModel.wgs019>)ViewData["CRHLList"];
          var bList = (Dictionary<int,DBModel.wgs010>)ViewData["BList"];
    %>
                <div class="user_info_data_box bd">
                    <div class="bank_list_box">
                        <div class="bank_list">
                            <table class="ctable_box youxi_table_list" width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <th>充值码</th>
                                    <th>充值类型</th>
                                    <th>金额</th>
                                    <th>到账金额</th>
                                    <th>充值时间</th>
                                    <th>状态</th>
                                    <th>备注</th>
                                </tr>
                              <tr>
              <%if( null != crhlList)
              { 
                  foreach(var item in crhlList)
                  {
                  %>
                                <td><%:item.uc005 %></td>
                                <td><%:bList[item.sb001].sb003 %></td>
                                <td><font class="f_red"><%:string.Format("{0:N2}",item.uc002) %></font></td>
                                <td><font class="zj_gr"><%:string.Format("{0:N2}",item.uc003) %></font></td>
                                <td><%:item.uc006 %></td>
                                <td><font class="zj_gr"><%switch (item.uc008) 
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
                                  } %></font>
                              </td>
                                <td><%:item.uc012 %></td>
                              </tr>
                     <%
                }/*foreach*/
              }/*if*/ %>
                            </table>
                        </div>
                        
                    </div>
                    
                    <div class="wp_page fl_r">
                        <%=ViewData["PageList"] %>
                    </div>
                </div>

<%}/*crhl*/ %>
<%else if( "wrhl" == methodType)
{
   var wcDataList = (List<DBModel.wgs020>)ViewData["WCDataList"];
%>
                 <div class="user_info_data_box">
                    <div class="bank_list_box">
                        <div class="bank_list">
                            <table class="ctable_box youxi_table_list" width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <th>编号</th>
                                    <th>提现类型</th>
                                    <th>金额</th>
                                    <th>提现时间</th>
                                    <th>状态</th>
                                    <th>备注</th>
                                </tr>
                              <tr>
              <%if (null != wcDataList)
              {
                  foreach (var item in wcDataList)
                  {
                  %>
                                <td><%:item.uw001 %></td>
                                <td><%:item.uw009 %></td>
                                <td><font class="f_red"><%:string.Format("{0:N2}",item.uw002) %></font></td>
                                <td><font class="zj_gr"><%:item.uw005 != null ? item.uw005.ToString() : "" %></font></td>
                                <td><%:item.uw006 %></td>
                                <td><font class="zj_gr"><%switch (item.uw006) 
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
                                      } %></font>
                              </td>
                                <td><%:item.uw008 %></td>
                              </tr>
                     <%
                }/*foreach*/
              }/*if*/ %>
                            </table>
                        </div>
                        <div class="wp_page fl_r">
                            <%=ViewData["PageList"] %>
                        </div>
                    </div>
                </div>

<%}/*wrhl*/ %>
<%else if( "ic"== methodType) {%>
    <form action="/UI2/Bank" method="post" id="form_charge">
        <%:Html.AntiForgeryToken() %>
        <input type="hidden" name="method" value="ic" />
    
        <div class="ajax_tbox" style="width:512px;margin-top:30PX">
                        <div class="ajax_content">
                            <div class="gd_details zhuanzhang">
                                <dl>
                                	<dt>验证码：</dt>
                                    <dd>
                                    	<input class="zz_txt_img fl_l" type="text" name="cw_vcode" value="">
                                        <img class="code_img cvc cw_vcode" id="validate" width="84" height="31"/>
                                        <a class="cvc" href="#">换一个</a>
<%--                                        <input class="cw_vcode cvc h-point" type="text" style="width:150px; height:50px; border:none;" readonly="readonly" /><br /><span class="cvc h-point">[换一张]</span><br />
                                        <input type="text" class="input-text w200px" name="cw_vcode" value="" />--%>
                                    </dd>
                                </dl>
                                <dl>
                                	<dt>资金密码：</dt>
                                    <dd>
                                    	<input class="zz_txt" type="password" name="cw_pwd" value="">
                                    </dd>
                                </dl>
                                <dl>
                                	<dt>收款账号：</dt>
                                    <dd>
                                    	<input class="zz_txt" type="text" name="cw_acct" value="" >
                                    </dd>
                                </dl>
                                <dl>
                                	<dt>确认账号：</dt>
                                    <dd>
                                    	<input class="zz_txt" type="text" name="cw_facct" value="">
                                    </dd>
                                </dl>
                                <dl>
                                	<dt>转账金额：</dt>
                                    <dd>
                                    	<input class="zz_txt" style="width:165px;" type="text"  name="cw_amt" value="">
                                        <label>*提交后不能修改！</label>
                                    </dd>
                                </dl>
                                <input class="zz_sbtn" value="确认转账" type="button" id="btn_charge">
                            </div>
                        </div>
                    </div>
   
    </form>
    <script type="text/javascript">
        function set_charge_code() {
            $(".cw_vcode").css("background-image", "url(/UI2/CWVCode?r=" + Math.random() + ")");
        }

        $(document).ready(function () {
            set_charge_code();

            $(".cvc").click(function () { set_charge_code(); });
            //$("#cg-money").keyup(function () { $(".cn-money").html(_global_D2B($(this).val())); });
            $("#btn_charge").click(function () {
                var form_data = $("#form_charge").serialize();
                $.ajax({
                    async: false, timeout: _global_ajax_timeout, type: "POST", cache: false, url: "/UI2/Bank", data: form_data, dataType: "text",
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
<%}/*ic*/ %>
<%else if("ichl" == methodType) {%>
<%
          var icList = (List<DBModel.wgs043>)ViewData["ICList"];
          var type = (int)ViewData["Type"];
          var dts = (DateTime?)ViewData["DTS"];
          var dte = (DateTime?)ViewData["DTE"];
          Dictionary<int, string> icStatus = new Dictionary<int, string>() { { 0, "未审核" }, { 1, "已审核" }, { 2, "已取消" } };
%>
<div class="user_info_data_box">
  <form action="/UI2/Bank" id="form_order_default" method="get">
    <input name="method" type="hidden" value="ichl" />
         <div class="search_game_recode">
            <div class="gd_search_input">
                <ul>
                    <li><span class="select_span">类型：</span>
                         <select name="type" id="type" class="select_2015">
            <option value="0" title="所有">所有</option>
            <option value="1" title="转出" <%=type == 1 ? "selected='selected'" : "" %>>转出</option>
            <option value="2" title="收到" <%=type == 2 ? "selected='selected'" : "" %>>收到</option>
        </select>
                    </li>                                                                                               
                    <li>时间范围：<input class="gd_txt gd_time" type="text" name="dts" id="dts" value="<%=dts %>" />- <input class="gd_txt gd_time" type="text" name="dte" id="dte" value="<%:dte %>"></li>
                     <li><input class="gd_sbtn" value="查找" type="submit"></li>
                </ul>
            </div>
          </div>
 </form>
            <div class="clear"></div>
            <hr class="line_01">       	    
         <div class="bank_list_box">
            <div class="bank_list">
                <table class="ctable_box youxi_table_list" width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tbody><tr>
                        <th>编号</th>
                        <th>转账者</th>
                        <th>收款者</th>
                        <th>金额</th>
                        <th>时间</th>
                        <th>状态</th>
                        <th>备注</th>
                    </tr>
              <%if( null != icList) 
              {
                  foreach(var item in icList)
                      {
                  %>
                    <tr>
                    <td><%:item.tf001 %></td>
                    <td><%:item.tf003 %></td>
                    <td><%:item.tf006 %></td>
                    <td><font class="f_red"><%:item.tf008.ToString("N2") %></font></td>
                    <td><%:item.tf009 %></td>
                    <td><font class="zj_gr"><%:icStatus[item.tf012] %></font></td>
                    <td>暂无</td>
                    </tr>
                    <%} /*for*/
              }/*if*/%>
                </tbody></table>
            </div>
               <div class="wp_page fl_r">
                <%=ViewData["PageList"] %>
            </div>
         </div> 

</div>


<%}/*ichl*/ %>

            </div>
        </div>
    </div>
</div>
</div>
<script type="text/javascript">

    $('#dts,#dte').appendDtpicker({ locale: "cn", dateFormat: "YYYY/MM/DD hh:mm" });


</script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">


</asp:Content>

