<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>系统管理</title>
    <link href="/CSS/CSSReset.css" rel="stylesheet" type="text/css" />
    <link href="/CSS/Default/AM/Main.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="/Scripts/jquery-easyui/themes/gray/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Scripts/jquery-easyui/themes/icon.css" />
    <script type="text/javascript" src="/Scripts/jquery/jquery.js"></script>
    <script type="text/javascript" src="/Scripts/jquery-easyui/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="/Scripts/AM/Default/WGSBase.js"></script>
    <script type="text/javascript">
        var online_count = 0;
        function ui_show_tab(title, href, closable, cache)
        {
            if ($("#system-body-content").tabs("exists", title))
            {
                $('#system-body-content').tabs("select", title);
            }
            else
            {
                var content = '<iframe scrolling="auto" frameborder="0"  src="' + href + '" style="width:100%;height:100%;"></iframe>';
                $('#system-body-content').tabs("add", {
                    title: title, closable: closable, content: content, cache: cache
                });
                //, tools:[{iconCls:"tab-base-window"}]
            }
        }

        function get_online_count()
        {
            $.ajax({
                async:true,timeout: _global_ajax_timeout, type: "POST", cache: false, url: "/AM/Online?method=count&rand=" + Math.random(), data: { "method": "count" }, dataType: "json",
                success: function (a, b)
                {
                    //online_count = a.Data;
                    //$(".online_count").html(online_count);
                    $(".online_count").html(a.Data);
                }
            });
        }

        $(document).ready(function ()
        {
            get_online_count();
            ui_show_tab('系统概况', '<%:Url.Action("Notify", "AM")%>', false);
        window.setInterval(get_online_count, 1000 * 30);
    });
    </script>
</head>
<body class="easyui-layout">
    <noscript>
        <p>Your browser does not support JavaScript!</p>
        <p>您的浏览器不支持JavaScript！</p>
    </noscript>
    <div id="system-header" data-options="region:'north',border:false">
        <h1 class="cjlsoft-header">系统管理</h1>
        <%--<div class="cjlsoft-header-logo"></div>--%>
        <div class="right_info">
            <span class="user_group"><%:ViewData["MGU"] %></span><span class="user_account"><%:ViewData["MGUX"] %></span>
            <a href="javascript:;" title="在线人数">在线人数<span class="f_number online_count"><%:ViewData["OnlineCount"] %></span></a>
            <a href="<%:Url.Action("Logout","AM") %>" title="退出" onclick="return confirm('是否退出？');">退出</a>
        </div>
    </div>
    <div id="system-left" data-options="region:'west',split:true,title:'&nbsp;'">
        <%Html.RenderAction("CommonLeft", "AM"); %>
    </div>
    <div id="system-bottom" data-options="region:'south',border:false">
    </div>
    <div data-options="region:'center',border:false">
        <div id="system-body-content" class="easyui-tabs" data-options="tools:'#tab-tools',fit:true" style="overflow: hidden;"></div>
    </div>
</body>
</html>
