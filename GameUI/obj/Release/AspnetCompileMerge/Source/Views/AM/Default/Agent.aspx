<%@ Page Language="C#" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/CSS/CSSReset.css" rel="stylesheet" type="text/css" />
    <link href="/CSS/Default/AM/PageDefalut.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="/Scripts/jquery-easyui/themes/gray/easyui.css" />
    <script type="text/javascript" src="/Scripts/jquery/jquery.js"></script>
    <script type="text/javascript" src="/Scripts/jquery-easyui/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="/Scripts/AM/Default/WGSBase.js"></script>
</head>

    <body class="easyui-layout">
        <div id="agent_block_top" style="height:60px;" data-options="region:'north',border:false"></div>
        <div data-options="region:'west',split:true,border:false" title="代理树" style="width: 180px;">
            <div id="parent_tree" data-options="lines:true,animate:false"></div>
        </div>
        <div data-options="region:'center',title:'下级列表',border:false" style="overflow:hidden;">
            <iframe scrolling="auto" frameborder="0" id="if_agent_list" style="width:100%;height:100%;"></iframe>
        </div>
    </body>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#parent_tree").tree({ url: "/AM/MemberAgent?method=getAGUIDs" });
            $('#parent_tree').tree({
                onClick: function (node)
                {
                    $("#if_agent_list").attr("src", "/AM/AgentList?parentID="+node.id );
                }
            });
            $("#if_agent_list").attr("src", "/AM/AgentList?parentID=0");
        });
    </script>
</html>
