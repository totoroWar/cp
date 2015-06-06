<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI2/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var msgList = (List<DBModel.wgs044>)ViewData["MessageList"];
    ViewData["PageAF"] = 0;
    ViewData["PageAT"] = 30;
%>
<div class="m_body_bg">
<div class="main_box main_tbg">
	<div class="main_table_bg">
    	<div class="main_table_box">
        	<!---个人资料 start-->
        	<div class="user_info_box">
            	<div class="user_info_tab hd">
                	<ul>
                    	<span><a class="info_close" href="#"></a></span>
                        <li class="on"><a href="/UI2/Message">平台消息</a></li>
                    </ul>
                </div>
                
                <div class="user_info_data_box">
                	<!--提现银行 start-->
                    <div class="user_add_bank">

                        <div class="bank_list_box">
                        	<div class="bank_list">
                                <table class="ctable_box youxi_table_list" border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tbody><tr>
                                        <th>发件人</th>
                                        <th>收件人</th>
                                        <th>标题</th>
                                        <th>时间</th>
                                    </tr>
                                   <%if( null != msgList){
                  foreach(var item in msgList)
                  { %>
                    <tr key="<%:item.msg001 %>">
                        <td><%:item.msg004 == 0 ? "系统" : item.msg008.Trim() %><textarea rows="10" cols="10" style="display:none" id="read<%=item.msg001 %>"><%=Newtonsoft.Json.JsonConvert.SerializeObject(item) %></textarea></td>
                        <td><%:item.msg005 == 0 ? "平台成员" : item.msg009.Trim() %></td>
                        <td class="hover_link show_des <%=item.msg005 == 0 ? "fc-red" : "" %>" data="<%:item.msg001 %>"> <a href="javascript:void(0)" class="<%=item.msg007 == 0? "fc-green" : "" %>"><%:item.msg002.Trim() %></a></td>
                        <td><%:item.msg006.ToString() %></td>
                        <%--<td class="link-tools dom-hide"><a href="javascript:;" title="阅读" class="show_des" data="<%:item.msg001 %>">查看</a><a href="javascript:;" class="dom-hide" title="删除">删除</a></td>--%>
                    </tr>
                    <%
                    }/*foreach*/
                      }/*if*/ %>

                                </tbody></table>

                            </div>
                            <div class="wp_page fl_r">
                           <%=ViewData["PageList"] %>
                                </div>
                        </div>
                    </div>
                    <!--提现银行 end-->
                </div>
            </div>
            <!---个人资料 end-->
            
        </div>
    </div>
</div>
</div>

<div id="r_msg" style="display:none">
    <table class="table-pro tp5 table-noborder w100ps">
        <tr>
            <td class="title">发件人</td>
            <td id="msg_u"></td>
        </tr>
        <tr>
            <td class="title">标题</td>
            <td id="msg_t"></td>
        </tr>
        <tr>
            <td class="title">时间</td>
            <td id="msg_dt"></td>
        </tr>
    </table>
    <div class="blank-line"></div>
    <div id="msg_c">
    </div>
</div>

<script type="text/javascript">
    var games = <%=ViewData["JsonGames"]%>;
    function get_read(key) {
        $.ajax({
            timeout: _global_ajax_timeout, dataType: "text", type: "POST", data: {key:key}, url: "/UI/Message", success: function (a) {
                _check_auth(a);
                eval("var _robj=" + a + ";");
                if (0 == _robj.Code)
                {
                    alert(_robj.Message);
                }
                else if (1 == _robj.Code)
                {
                    var msg = $("#read" + key).val();
                    eval("var _msg=" + msg + ";");
                    var send_name = "";
                    if (0 == _msg.msg004)
                    {
                        send_name = "系统";
                    }
                    $("#msg_u").html(send_name);
                    $("#msg_t").html(_msg.msg002);
                    $("#msg_c").html(_msg.msg003);
                    $("#msg_dt").html(_msg.msg006.toString().replace("T"," "));
                    for(var i = 0; i < games.length; i++)
                    {
                        if( 0 < _msg.msg003.indexOf(games[i].key) )
                        {
                            var n = _msg.msg003.replace(games[i].key, games[i].name);
                            $("#msg_c").html(n);
                        }
                    }
                    $("#r_msg").dialog({ width: 600, height: 350, title: _msg.msg002, modal: true, resizable: false, position: { my: "center", at: "center", of: window } });
                }
            }
        });
    }
    $(document).ready(function ()
    {
        $(".show_des").click(function ()
        {
            var key = $(this).attr("data");
            get_read(key);
            $(this).children("span").removeClass("fc-green");
        });
    });
</script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
