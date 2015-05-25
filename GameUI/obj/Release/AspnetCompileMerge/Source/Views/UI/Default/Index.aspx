<%@ Page Language="C#" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>娱乐平台</title>
    <link href="/CSS/CSSReset.css" rel="stylesheet" type="text/css" />
    <link href="/CSS/Default/UI/Main.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="/Scripts/jquery-easyui/themes/default-ui/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/Scripts/jquery-easyui/themes/icon.css" />
    <script type="text/javascript" src="/Scripts/jquery/jquery.js"></script>
<script type="text/javascript" src="/Scripts/jquery-easyui/jquery.easyui.min.js"></script>
<script type="text/javascript" src="/Scripts/jquery-easyui/locale/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript" src="/Scripts/UI/Default/WGSBase.js"></script>
</head>
<body class="easyui-layout">
    <%
        var gcList = (List<DBModel.wgs006>)ViewData["GCList"];
    %>
    <noscript>
        <p>Your browser does not support JavaScript!</p>
        <p>您的浏览器不支持JavaScript！</p>
    </noscript>
    <div id="system-header" data-options="region:'north',border:false">
        <h1 class="ui-header"></h1>
        <div class="ui-header-logo"></div>
        <div class="right_info">
            <a href="javascript:cjlsoft_show_tab('帮助中心', '/UI/Notify', true, false);" class="help_general">
                <span></span>
            </a>
            <a href="javascript:cjlsoft_show_tab('账户中心', '/UI/UCenter', true, false);" class="users_info">
                <span></span>
            </a>
            <a href="javascript:cjlsoft_show_tab('报表管理', '/UI/Notify', true, false);" class="report_list">
                <span></span>
            </a>
            <a href="javascript:cjlsoft_show_tab('用户管理', '/UI/Notify', true, false);" class="users_list">
                <span></span>
            </a>
            <a href="javascript:cjlsoft_show_tab('游戏记录', '/UI/Notify', true, false);" class="history_playlist">
                <span></span>
            </a>
            <a href="javascript:cjlsoft_show_tab('充值提现', '/UI/Notify', true, false);" class="account_autosave">
                <span></span>
            </a>
            <a href="javascript:cjlsoft_show_tab('首页', '/UI/Notify', false, false);" class="help_security">
                <span></span>
            </a>
        </div>
    </div>
    <div id="system-left" data-options="region:'west',split:true,title:'用户中心<span></span>'">
        <div class="user_info_block">
            <p>账号：<%:ViewData["UILoginAccount"] %><span class="nickname"><%:ViewData["UILoginNickname"] != "" ? ""+ViewData["UILoginNickname"]+"" : "" %></span></p>
            <p>余额：<span id="ag_money" class="fc_red"><%:string.Format("{0:N4}",ViewData["AGMoney"]) %></span></p>
            <p class="p_l_btn"><a href="javascript:cjlsoft_show_tab('充值','/UI/Charge',true,false);" class="easyui-linkbutton" title="充值">充值</a><a href="javascript:cjlsoft_show_tab('提现','/UI/Withdraw',true,false);" class="easyui-linkbutton" title="提现">提现</a></p>
        </div>
        <ul>
        <%foreach (var gc in gcList)
          { %>
        <%
              var gids = gc.gc004.Split(',');
              var gDicList = (Dictionary<int, DBModel.wgs001>)ViewData["GDicList"];
              foreach (var gid in gids)
              {
        %>
            <li class="game_item"><a class="game_item_link" href="#"><%:gDicList[int.Parse(gid)].g003 %></a></li>
        <%
             } %>
        <%
          } %>
        </ul>
    </div>
    <div id="system-bottom" data-options="region:'south',border:false">
    </div>
    <div data-options="region:'center',border:false">
        <div id="system-body-content" class="easyui-tabs" data-options="tools:'#tab-tools',fit:true" style="overflow: hidden;"></div>
    </div>
    <script type="text/javascript">
        function cjlsoft_show_tab(title, href, closable, cache)
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
        $(document).ready(function ()
        {
            cjlsoft_show_tab("首页", "/UI/Notify", false, false);
            $(".panel-title span").html("<a>退出</a>");
            $(".panel-title span a").attr("href", "javascript:void(0);");
            $(".panel-title span a").attr("title", "退出平台");
            $(".panel-title span a").click(function ()
            {
                $.messager.confirm('提示', "是否退出平台？", function (r)
                {
                    if (r)
                    {
                        top.location.href = "/UI/Logout";
                    }
                });
            });

            window.setInterval(function ()
            {
                $.ajax({timeout: 5, type: "POST", url: "/UI/SetOnline", dataType: "html" });
            }, 1000 * 60);
        });
    </script>
</body>
</html>
