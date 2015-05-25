<%@ Page Language="C#" %><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml"><%var UITheme = (string)ViewData["UITheme"]; %>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title><%:ViewData["GlobalTitle"] %></title>
    <link rel="stylesheet" type="text/css" href="/CSS/<%=UITheme%>/UI/jquery.fs.scroller.css" />
    <link href="/CSS/CSSReset.css" rel="stylesheet" type="text/css" />
    <link href="/CSS/<%=UITheme%>/UI/Main.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="/CSS/<%=UITheme%>/UI/jqueryeasyui/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/CSS/<%=UITheme%>/UI/jqueryeasyui/icon.css" />
    <link rel="stylesheet" type="text/css" href="/CSS/<%=UITheme%>/UI/jqueryui_base/jquery.ui.all.css" />
    <script type="text/javascript" src="/Scripts/jquery/jquery.js"></script>
    <script type="text/javascript" src="/Scripts/jquery/jquery-ui.min.js"></script>
    <script type="text/javascript" src="/Scripts/jquery/jquery.fs.scroller.js"></script>
    
    


    <script type="text/javascript" src="/Scripts/UI/<%=UITheme%>/jquery-easyui/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="/Scripts/UI/<%=UITheme%>/jquery-easyui/locale/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript" src="/Scripts/UI/<%=UITheme%>/WGSBase.js"></script>
    <script type="text/javascript" src="/Scripts/Common/swfobject_modified.js"></script>
    <script type="text/javascript" src="/Scripts/Common/All.js"></script>
    <script type="text/javascript" src="/Scripts/jquery/jquery.SuperSlide.js"></script>
</head>
<body class="easyui-layout" id="main_body" onload="IsChange()" >
    <%
        var gcList = (List<DBModel.wgs006>)ViewData["GCList"];
        var gList = (List<DBModel.wgs001>)ViewData["GList"];
        var gListDic = gList.ToDictionary(key => key.g001);
    %>
    <noscript>
        <p>Your browser does not support JavaScript!</p>
        <p>您的浏览器不支持JavaScript！</p>
    </noscript>
    <div id="system-header" data-options="region:'north',border:false,split:true">
        <h1 class="ui-header"></h1>
        <div class="ui-header-logo">
<% if( ViewData["TOFFlash"].ToString() == "1"){ %>
<object id="flash_logo" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" width="200" height="71">
  <param name="movie" value="/Scripts/Common/ui-logo-flash.swf" />
  <param name="quality" value="high" />
  <param name="wmode" value="transparent" />
  <param name="swfversion" value="8.0.35.0" />
  <!-- 此 param 标签提示使用 Flash Player 6.0 r65 和更高版本的用户下载最新版本的 Flash Player。如果您不想让用户看到该提示，请将其删除。 -->
  <param name="expressinstall" value="/Scripts/Common/expressInstall.swf" />
  <!-- 下一个对象标签用于非 IE 浏览器。所以使用 IECC 将其从 IE 隐藏。 -->
  <!--[if !IE]>-->
  <object type="application/x-shockwave-flash" data="/Scripts/Common/ui-logo-flash.swf" width="200" height="71">
    <!--<![endif]-->
    <param name="quality" value="high" />
    <param name="wmode" value="transparent" />
    <param name="swfversion" value="8.0.35.0" />
    <param name="expressinstall" value="/Scripts/Common/expressInstall.swf" />
    <!-- 浏览器将以下替代内容显示给使用 Flash Player 6.0 和更低版本的用户。 -->
    <div>
      <h4>此页面上的内容需要较新版本的 Adobe Flash Player。</h4>
      <p><a href="http://www.adobe.com/go/getflashplayer"><img src="http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif" alt="获取 Adobe Flash Player" width="200" height="33" /></a></p>
    </div>
    <!--[if !IE]>-->
  </object>
  <!--<![endif]-->
</object>
<%} %>
        </div>
        <div class="right_info">
            <a title="帮助中心" href="javascript:ui_show_tab('帮助中心', '/UI/Help', true, false);" class="help_general">
                <span></span>
            </a>
            <a title="账户中心" href="javascript:ui_show_tab('账户中心', '/UI/UCenter', true, false);" class="users_info">
                <span></span>
            </a>
            <a title="报表管理" href="javascript:ui_show_tab('报表管理', '/UI/Report', true, false);" class="report_list">
                <span></span>
            </a>
            <a title="用户管理" href="javascript:ui_show_tab('用户管理', '/UI/Member', true, false);" class="users_list">
                <span></span>
            </a>
            <a title="游戏记录" href="javascript:ui_show_tab('游戏记录', '/UI/Record', true, false);" class="history_playlist">
                <span></span>
            </a>
            <a title="充值提现" href="javascript:ui_show_tab('充值提现', '/UI/Bank', true, false);" class="account_autosave">
                <span></span>
            </a>
            <a title="平台大厅" href="javascript:ui_show_tab('系统公告', '/UI/Notify', false, false);" class="help_security">
                <span></span>
            </a>
        </div>
        <div id="ms_navigation">
            <%
                var menuList = (List<DBModel.SysUIMenu>)ViewData["UIMenuList"];
            %>
            <%foreach(var item in menuList)
              {
                  if (0 == item.Show)
                      continue;
                   %>
                <a class="<%:item.CSS %>" href="javascript:ui_show_tab('<%:item.Text %>', '<%:item.URL %>', true, false);" title="<%:item.Title %>"><%:item.Text %></a>
            <%} %>
        </div>
    </div>
    <div id="system-left" data-options="region:'west',split:true">
        <div class="user_info_block">
            <%:Html.AntiForgeryToken() %>
            <div><img class="to_my_center" src="/Images/<%=UITheme%>/UI/_member_login.png" alt="<%:ViewData["UILoginNickname"] %>您好" title="<%:ViewData["UILoginNickname"] %>您好" />
            <span class="txt_account" title="<%:ViewData["UILoginNickname"] %>您好"><%:ViewData["UILoginNickname"] %></span>
            <span class="refresh_uinfo any_link">刷新</span><span class="sign_today any_link">签到</span></div>
            
            <p class="money_line sag_money"><img src="/Images/Common/_num_mm.png" alt="余额" title="余额" class="sag_money" /><img alt="￥" title="￥" class="sag_money" src="/Images/Common/_num_m.png" /><span id="ag_money" title=""></span></p>
            <p class="point_line">积分：<span class="point_amount"><%:ViewData["AGPoint"] %></span></p>
            <div>军衔：<span class="pos_level"><%:ViewData["AGPosName"] %></span><span class="my_level_n">等级：</span><span class="my_level"><span id="set_my_level_img" title="<%:ViewData["AGLevelName"] %>"></span></span></div>
            <div class="level_line"></div>
            <div class="clear_all"></div>
            <p class="p_l_btn"><a href="javascript:ui_show_tab('充值','/UI/Charge',true,false);" class="l_btn_wcw l_btn_wckkw" title="充值"></a><a href="javascript:ui_show_tab('提现','/UI/Withdraw?method=GetWit',true,false);" class="l_btn_wcc" title="提现"></a><a href="javascript:ui_show_tab('平台消息','/UI/Message',true,false);" class="l_btn_msg" id="l_btn_msg" title="消息"></a></p>
        </div>
        <div class="game_list_block">
        <div class="game_select_title"></div>
        <ul>
        <%foreach (var gc in gcList)
          {
              if (0 == gc.gc006)
              {
                  continue; 
              }
              %>
        <%
              var gids = gc.gc004.Split(',');
              var gDicList = (Dictionary<int, DBModel.wgs001>)ViewData["GDicList"];
              foreach (var gid in gids)
              {
                  var game = gListDic[int.Parse(gid)];
                  if (null == game)
                  {
                      continue; 
                  }
        %>
            <li class="game_item" id="game_item_<%=gid %>"><a id="game_item_link_<%=gid %>" class="game_item_link" href="javascript:ui_show_tab('<%:gDicList[int.Parse(gid)].g003 %>','<%:Url.Action("Play", "UI", new {gameID=gid,gameClassID=gc.gc001 })%>',true,false);"><%=gDicList[int.Parse(gid)].g003 %></a></li>
        <%
             } %>
        <%
          } %>
        </ul>
        </div>
    </div>
    <div id="system-bottom" data-options="region:'south',border:false">
        <div class="prize_list">
            <div class="pl_left">幸运儿</div>
            <div id="showlist" class="pl_right">
                <%--<ul>

                    <%
                        ViewData["GetPrizeTop"] = null;
                        if( null != ViewData["GetPrizeTop"])
                      {
                          var topList = (List<DBModel.wgs053>)ViewData["GetPrizeTop"];
                          foreach(var item in topList)
                          {
                              var prizeCnt = string.Format("<span style='color:red;'>{0}</span>在{1}{2}期中奖<span style='color:#FFCC00'>{3}</span>，内容：{4}", item.tpi003.Trim(), gListDic[item.g001].g003, item.gs002.Trim(), item.tpi007.ToString("N2"), item.tpi005);
                              var prizeTitle = string.Format("{0}在{1}{2}期中奖{3}，内容：{4}", item.tpi003.Trim(), gListDic[item.g001].g003, item.gs002.Trim(), item.tpi007.ToString("N2"), item.tpi005);
                           %>
                    <li title="<%=prizeTitle %>"><%=prizeCnt %></li>
                    <%}
                      } %>
                    <%if (ViewData["ADWinList"] != null)
                      {
                          var winList = ViewData["ADWinList"].ToString().Split(new string[] { "___|___" }, StringSplitOptions.None);
                          foreach (var item in winList)
                          { 
                          %>
                    <li><%=item %></li>
                    <%}
                      } %>
                </ul>--%>
                
            </div>
        </div>
        
        <script type="text/javascript">
            function ShowList() {
                
                var temp = ""
                $.ajax({
                    url: "ui/showlist?t=" + new Date(),
                    async: false,
                    success: function (data) {
                        temp = data.Data;
                    }
                });

                $("#showlist").html(temp);
                
                jQuery(".prize_list").slide({ mainCell: ".pl_right ul", effect: "topLoop", autoPlay: true, vis: 1, scroll: 1, trigger: "click" });
                setTimeout("ShowList()", 60000);
                return true;
            }

            ShowList();

         </script>

        <span class="left_combuy_num" title="进行中的合买"><a href="javascript:ui_show_tab('合买大厅', '/UI/Combine', true, false);">可参与合买订单一共<strong id="allow_combuy">0</strong>笔</a></span>
        <span id="onlineservice"><a href="javascript:ui_show_tab('平台客服', '<%=(string)ViewData["customerServiceLink"] %>', true, false);" title="平台客服">平台客服</a></span>
        <span id="changeline"><a href="javascript:ui_show_tab('切换线路','/UI/ChangeLine?Choose=OK', true, false);">不够快？切换线路</a></span>
        <%--<span id="Span1">中奖了中奖了~~！@！#！@#</span>--%>
        <div id="current_datetime"></div>
    </div>
    <div id="zzf" data-options="region:'center',border:false">
        <div id="system-body-content" class="easyui-tabs" data-options="tools:'#tab-tools',fit:true" style="overflow: hidden;"></div>
        
    </div>
    <div id="sign_block">
        <div id="btn_sign_update"><input type="button" id="btn_save_sign" value="我要签到，签到获得积分" /></div>
        <div id="c_sign"></div>
        
    </div>
    
  <div id="xxx" title="温馨提示">中奖了中奖了！dfsdfsdfdfsdf！</div>


    <script type="text/javascript">
        if (window.top !== window.self) { window.top.location = window.location; }
        /*0---just click, 1---auto*/
        function get_ag_money(type)
        {
            //$("#ag_money,.point_amount,.pos_level").addClass("mny_loading");
            //$("#ag_money,.point_amount,.pos_level").html("");
            $.ajax({
                timeout: _global_ajax_timeout, cache: false, dataType: "text", type: "POST", url: "/UI/MyInfo", success: function (a)
                {
                    _check_auth(a);
                    eval("var _robj=" + a + ";");
                    if (1 == _robj.Code)
                    {
                        //$("#ag_money").html(a.Data);
                        if (1 == type)
                        {
                            $("#ag_money").html(_global_set_img_money(_robj.Data.M));
                            $(".point_amount").html(_robj.Data.P);
                            $(".point_amount").attr("title", _robj.Data.P);
                        }
                        //$(".my_level").html(_robj.Data.LN);
                        $("#set_my_level_img").removeAttr("class");
                        // $("#set_my_level_img").addClass("level" + _robj.Data.L);
                        $("#set_my_level_img").html(_robj.Data.LN);
                        $(".my_level").attr("title", _robj.Data.LN);
                        $(".pos_level").html(_robj.Data.POS);
                        $(".pos_level").attr("title",_robj.Data.POS);
                        $("#ag_money,.sag_money").attr("title", _robj.Data.M + "\n" + _global_D2B(_robj.Data.M));
                        if (0 < _robj.Data.MSG) {
                            if (!$(".l_btn_msg").hasClass("flash_word")) {
                                $(".l_btn_msg").addClass("flash_word");
                            }
                            //$(".l_btn_msg").html($(".l_btn_msg").attr("title") + "(" + _robj.Data.MSG.toString() + ")");
                        }
                        else
                        {
                            //$(".l_btn_msg").html($(".l_btn_msg").attr("title"));
                            $(".l_btn_msg").removeClass("flash_word");
                        }
                        $("#allow_combuy").html(_robj.Data.CBC);
                        $("#ag_money,.point_amount,.my_level,.pos_level").removeClass("mny_loading");
                    }
                }
            });
        }

        
        function IsChange() {
            var mymoney = $("#ag_money").html();
            var myamount = $(".point_amount").html();

            
            var reg = /_num_\d|dot2/g;
            var strlist = mymoney.match(reg);
            
            strlist = strlist.join("");
            strlist = strlist.replace(/_num_/g, "")
            strlist = strlist.replace("dot2", ".");
            strlist = strlist.replace(",", ".");
            
            

            $.ajax({
                url: "ui/IsChange?001=" + strlist + "&004=" + myamount + "&t=" + new Date(),
                cache: false,
                
                async: true,
                success: function (data) {
                    var num = -1;
                    num = data.Data;
                    
                  if (num == 0) {
                    
                    get_ag_money(1);
                 
                    }
                }
            });
            
             
            

            setTimeout("IsChange()", 2500);
            return true;
        }

        function set_online()
        {
            $.ajax({
                timeout: _global_ajax_timeout, dataType: "text", type: "post", url: "/UI/SetOnline", success: function (a) {
                    _check_auth(a);
                    eval("var _robj=" + a + ";");
                    if (-1 == _robj.Code) {
                        alert(_robj.Message);
                    }
                }
            });
        }

        function set_sign_c(y,m)
        {
            var result;
            var ymstring = y.toString() + "_" + m.toString();
            var oldym = $("#c_sign").data("oldym");
            if (ymstring != oldym) {
                $.ajax({
                    async: false, timeout: _global_ajax_timeout, dataType: "text", type: "post", data: { y: y, m: m, method: "getsign" }, url: "/UI/Sign?method=getsign&y=" + y.toString() + "&m=" + m.toString(), success: function (a) {
                        _check_auth(a);
                        eval("var _robj=" + a + ";");
                        result = _robj;
                        $("#c_sign").data("oldym", ymstring);
                        $("#c_sign").data(ymstring, result);
                    }
                });
            }
            else
            {
                result = $("#c_sign").data(ymstring);
            }
            return result;
        }

        function create_c()
        {
            $("#c_sign").html("");
            $("#c_sign").remove();
            $("#btn_sign_update").after('<div id="c_sign"></div>');
            $("#c_sign").calendar(
                {
                    width: 470,
                    height: 330,
                    border: false,
                    formatter: function (date) {
                        var m = date.getMonth() + 1;
                        var d = date.getDate();
                        var opts = $(this).calendar('options');
                        var result = set_sign_c(date.getFullYear(), opts.month);
                        for (var i = 0; i < result.Data.length; i++) {
                            if (d == result.Data[i].day && m == result.Data[i].month) {
                                return '<div class="md sign_days">' + d + '</div>';
                            }
                        }
                        return d;
                    }
                    , onChange: function () {
                    }
                });
            $(".calendar-prevmonth,.calendar-nextmonth").click(function ()
            {
                set_c_select();
            });
            set_c_select();
        }

        function create_xxx()
        {
            $("#xxx").html("");
            $("#xxx").remove();
            //$("#btn_sign_update").after('<div id="xxx">xxxxxxxxx</div>');
            $("#xxx").calendar(
                {
                    width: 470,
                    height: 330,
                    //border: false,
                    
                });
        }

        function set_c_select()
        {
            $(".sign_days").parent("td").addClass("set_sign_day");
        }

        $(document).ready(function ()
        {
            
            //$("#dialog-confirm").position({
            //    my: "center",
            //    at: "center",
            //    //of: "#main_body"
            //    of: window
            //});

            //$(function() {
            //    $("#dialog-confirm").dialog({
            //        autoOpen: false,
            //        show: { effect: "blind", duration: 1000 },
            //        hide: { effect: "explode", duration: 1000 }
            //    });
               // $("#dialog-confirm").dialog("open");
            //$("#xxx").dialog({ bgiframe: true, modal: false, autoOpen: false, draggable: false, bgiframe: true, height: 150, width: 250, position: { my: "300px", at: "500px", of: window } });
            
            //var top = $(obj).offset().top + 16;  
            //var left = $(obj).offset().left - 290;  
            //$("#xxx").dialog("option", "position", [left, top]);
            //$("#xxx").dialog("open");  


            //$(function () {

            //    setTimeout(function () {

            //        $("#dialog-confirm").dialog("destroy");

            //    }, 3000);

            //})

            //$.messager.show(0, '一秒钟关闭消息', 1000);

            $("#system-left").scroller();

            $("#nick_name").dblclick(function ()
            {
                $(this).hide();
                $(".p_edit_name").show();
                $("#edit_nick_name").val($(this).html());
            });

            $(".nn_cc").click(function () { $("#nick_name").show(); $(".p_edit_name").hide() });
            $(".nn_up").click(function ()
            {
                var nn = $("#edit_nick_name").val();
                $.ajax({
                    timeout: _global_ajax_timeout, dataType: "text", type: "POST", url: "/UI/Member", data: { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(), nn: nn, method: "updateNickName" }, success: function (a) {
                        _check_auth(a);
                        eval("var _robj=" + a + ";");
                        if (0 == _robj.Code)
                        {
                            alert(_robj.Message);
                        }
                        else
                        {
                            $("#nick_name").show();
                            $(".p_edit_name").hide();
                            $("#nick_name").html(_robj.Message);
                        }
                    }
                });
            });

            get_ag_money(1);
            
            $(".to_my_center").click(function ()
            {
                ui_show_tab("账户中心", "/UI/UCenter", true, false);
            });

            $(".refresh_uinfo").click(function () {
                get_ag_money(1);
            });

            
            window.setInterval(function () {
                set_online();
                //get_ag_money(0);

            }, 1000 * 10);

            

            window.setInterval(function () {
                var hc = $(".l_btn_msg").hasClass("flash_word");
                if (hc) {
                    $("#l_btn_msg").toggleClass("l_btn_msg_read");
                }
                else {
                    $(".l_btn_msg").removeClass("l_btn_msg_read");
                }
            }, 500);


            

            <%
        var popupList = (List<DBModel.SysFirstLoadURL>)ViewData["UIFirstLoad"];
        foreach(var p in popupList)
        {%>
            ui_show_tab("<%:p.Text%>", "<%:p.URL%>", true, false);
            <%}%>
            $(".panel-tool").append("<a class='sys_refresh'>刷新</a><a class='sys_exit'>退出</a>");
            $(".panel-tool a.sys_exit").attr("href", "javascript:void(0);");
            $(".panel-tool a.sys_exit").attr("title", "退出平台");
            $(".panel-tool a.sys_refresh").attr("href", "javascript:void(0);");
            $(".panel-tool a.sys_refresh").attr("title", "刷新");
            $(".panel-tool a.layout-button-left").attr("title", "收起");
            $(".panel-tool a.layout-button-right").attr("title", "扩展");

            $(".panel-tool a").click(function ()
            {
                if ($(this).hasClass("sys_refresh")) {
                    top.location.href = top.location.href;
                }
                else if ($(this).hasClass("sys_exit"))
                {
                    $.messager.confirm('平台温馨提示', "是否退出平台？", function (r) {
                        if (r) {
                            top.location.href = "/UI/Logout";
                        }
                    });
                }
            });

            $("#btn_save_sign").click(function ()
            {
                $.ajax({
                    timeout: _global_ajax_timeout, dataType: "text", type: "POST", url: "/UI/Sign", success: function (a) {
                        _check_auth(a);
                        eval("var _robj=" + a + ";");
                        if (0 == _robj.Code)
                        {
                            alert(_robj.Message);
                        }
                        else if (1 == _robj.Code)
                        {
                            create_c();
                            alert("签到成功");
                        }
                    }
                });
            });

            $(".sign_today").click(function ()
            {
                $("#sign_block").dialog({ width: 490, height: 420, title: "签到", modal: true, resizable: false, position: { my: "center", at: "center", of: window } });
                create_c();
            });

            
            $(function () {

                //$("#xxx").dialog("option", "position", [500,500]); 
                //$("#xxx").dialog({ height: 150, width: 250, title: "重要提示", modal: false, resizable: false, position: { my: "right top", at: "right top", of: "#system-body-content" } });
                //$("#xxx").dialog("option");
                //create_c();

                //$("#xxx").html("");
                create_xxx();
            });

           
        });
    </script>
</body>
</html>