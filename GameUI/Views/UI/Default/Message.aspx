<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var msgList = (List<DBModel.wgs044>)ViewData["MessageList"];
    ViewData["PageAF"] = 0;
    ViewData["PageAT"] = 30;
%>
<table class="table-pro tp5 table-color-row g_nco w100ps">
    <thead>
        <tr class="table-pro-head">
            <th class="w120px">发件人</th>
            <th class="w120px">收件人</th>
            <th>标题</th>
            <th class="w150px">时间</th>
            <th class="w100px dom-hide"></th>
        </tr>
    </thead>
    <tbody>
        <%if( null != msgList){
                  foreach(var item in msgList)
                  { %>
        <tr key="<%:item.msg001 %>">
            <td><%:item.msg004 == 0 ? "系统" : item.msg008.Trim() %><textarea rows="10" cols="10" class="dom-hide" id="read<%=item.msg001 %>"><%=Newtonsoft.Json.JsonConvert.SerializeObject(item) %></textarea></td>
            <td><%:item.msg005 == 0 ? "平台成员" : item.msg009.Trim() %></td>
            <td class="hover_link show_des <%=item.msg005 == 0 ? "fc-red" : "" %>" data="<%:item.msg001 %>"> <span class="<%=item.msg007 == 0? "fc-green" : "" %>"><%:item.msg002.Trim() %></span></td>
            <td><%:item.msg006.ToString() %></td>
            <td class="link-tools dom-hide"><a href="javascript:;" title="阅读" class="show_des" data="<%:item.msg001 %>">查看</a><a href="javascript:;" class="dom-hide" title="删除">删除</a></td>
        </tr>
        <%
        }/*foreach*/
          }/*if*/ %>
    </tbody>
</table>
<%=ViewData["PageList"] %>
<div id="r_msg" class="dom-hide">
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
