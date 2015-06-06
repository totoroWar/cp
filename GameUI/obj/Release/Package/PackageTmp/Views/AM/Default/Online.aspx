<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var onlineList = (List<DBModel.wgs025>)ViewData["OList"];
    var ipLOC = new NETCommon.IPPhyLoc(ViewData["IPFP"].ToString());
%>
    <div class="xtool">
        <form action="/AM/Online" method="get">
            账号<input name="account" class="w100px input-text" type="text" value="<%:ViewData["Account"] %>" />
            状态
            <select name="status">
                <option value="-1" <%=(int)ViewData["Status"] == -1 ? "selected='selected'" : "" %>>所有</option>
                <option value="0" <%=(int)ViewData["Status"] == 0 ? "selected='selected'" : "" %>>不在线</option>
                <option value="1" <%=(int)ViewData["Status"] == 1 ? "selected='selected'" : "" %>>在线</option>
            </select>
            域名<input name="domain" class="w100px input-text" type="text" value="<%:ViewData["Domain"] %>" />
            网络地址<input name="ip" class="w100px input-text" type="text" value="<%:ViewData["IP"] %>" />
            <input class="btn-normal" type="submit" value="查找" />
        </form>
    </div>
<div class="blank-line"></div>
    <table class="table-pro w100ps">
        <thead>
            <tr class="table-pro-head">
                <th>账号</th>
                <th>昵称</th>
                <th>状态</th>
                <th>唯一码</th>
                <th>登录时间</th>
                <th>更新时间</th>
                <th>网络地址</th>
                <th>域名</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            <%if(null != onlineList)
              {
                  foreach( var item in onlineList)
                  { 
                   %>
            <tr>
                <td><%:item.u002.Trim() %></td>
                <td><%:item.u003 == null ? string.Empty : item.u003.Trim() %></td>
                <td><%=item.onl006 == 0 ? "<span class='fc-gray'>离线状态</span>" : "<span class='fc-green'>在线</span>" %></td>
                <td><%=item.onl002.Trim() %></td>
                <td><%:item.onl003 %></td>
                <td><%:item.onl004 %></td>
                <td <%:GameUI.Controllers.AMController.IsHaveRepeatIp(item.onl005.Trim())?"style=color:red":""%>>
                    
                    <%:item.onl005.Trim() %>/<%:ipLOC.GetIPAll(item.onl005.Trim()) %>

                </td>
                <td><%:item.onl007 %></td>
                <td class="link-tools"><a href="javascript:;" name="set_offline" data="<%:item.u001 %>">强制下线</a></td>
            </tr>
            <%} 
              }%>
        </tbody>
    </table>
    <%=ViewData["PageList"] %>
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $("a[name='set_offline']").click(function ()
            {
                var data = $(this).attr("data");
                $.ajax({
                    timeout: _global_ajax_timeout, cache: false, type: "POST", data: {key:data}, url: "/AM/Online?method=set_offline", success: function (a) {
                        _check_auth(a);
                        refresh_current_page();
                    }
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>