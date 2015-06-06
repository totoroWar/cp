<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html
PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"
"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible"content="IE=9; IE=8; IE=7; IE=EDGE">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="shortcut icon" href="/favicon.ico" type="image/x-icon" />
<link href="/css/<%=ViewData["UITheme"] %>/base.css" rel="stylesheet" type="text/css" />
<link href="/css/<%=ViewData["UITheme"] %>/index.css" rel="stylesheet" type="text/css" />   
<script src="/Scripts/ui/<%=ViewData["UITheme"] %>/jquery_min.js" type="text/javascript"></script>
<script src="/Scripts/ui/<%=ViewData["UITheme"] %>/jquery.SuperSlide.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="/Scripts/jquery/CSS/jquery.simple-dtpicker.css" />
    <link rel="stylesheet" type="text/css" href="/CSS/<%=ViewData["UITheme"] %>/UI/jqueryeasyui/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/CSS/<%=ViewData["UITheme"] %>/UI/jqueryui_base/jquery.ui.all.css" />
    <script type="text/javascript" src="/Scripts/jquery/jquery-ui.min.js"></script>
    <script type="text/javascript" src="/Scripts/jquery/jquery.cookie.js"></script>
    <script type="text/javascript" src="/Scripts/ion-sound/ion.sound.min.js"></script>
    <script type="text/javascript" src="/Scripts/jquery/jquery.blockUI.js"></script>
    <script type="text/javascript" src="/Scripts/jquery/jquery.simple-dtpicker.js"></script>
    <script type="text/javascript"  src="/Scripts/jquery/jquery.form.js"></script>
    <script type="text/javascript" src="/Scripts/UI/<%=ViewData["UITheme"] %>/WGSBase.js"></script>

     <script type="text/javascript" src="/Scripts/UI/<%=ViewData["UITheme"] %>/jquery-easyui/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="/Scripts/UI/<%=ViewData["UITheme"] %>/jquery-easyui/locale/easyui-lang-zh_CN.js"></script>
        <script type="text/javascript" src="/Scripts/jquery/jquery.fs.scroller.js"></script>
    <script type="text/javascript" src="/Scripts/Common/swfobject_modified.js"></script>
    <script type="text/javascript" src="/Scripts/Common/All.js"></script>
    
    <title>九州娱乐</title>
    <style>
        .msgcount{color: white; display:none; font-size: 0.6em; display: block; float: right; line-height: 10px; height: 10px; background-color: red; width: 10px; margin-top: 5px; text-align: center;}
    </style>
</head>   
<body>
<div class="warp">
    <!--头部开始-->
    <div class="top_c">
<div class="top_m_box">
	<!--顶部会员信息模块 start-->
    <%:Html.AntiForgeryToken() %>
	<div class="top_user_box">
    	<div class="top_logo fl_l"><img src="/images/zj3/top_logo.png" /></div>
        <div class="top_user_info fl_r user_info_block">
        	<div class="t_u_img fl_l"><a href="/UI2/UCenter"><img src="/images/zj3/t_u_simg.png" /></a> <a href="/UI2/UCenter"  class="uname"><%=ViewData["UILoginUserAccount"] %></a></div>
            <div class="t_u_yue fl_l">余额：<a href="#" id="ag_money"></a></div>
            <div class="t_u_jifen fl_l">积分：<a href="#"class="point_amount"></a></div>
            <div class="t_u_junxian fl_l">头衔：<a href="#" class="pos_level"></a></div>
            <div class="t_u_dengji fl_l ">等级：<a href="#" class="my_level_n"></a></div>
            <div class="t_u_shaxin fl_l ">
            	<a href="#"><span class="refresh_uinfo any_link"><img src="/images/zj3/t_u_sx.png" /></span></a>
            	<a href="#"><span class="sign_today any_link"><img src="/images/zj3/t_u_qd.png" /></span></a>
            </div>
            <div class="top_login_out fl_r"><a href="/UI2/Logout">退出登录</a></div>
        </div>
    </div>
    <!--顶部菜单导航模块 start-->
	<div class="top_menu_box">
    	<div class="t_menu_x fl_l">
        	<ul>
            	<li>快捷导航</li>
                <li><a href="/UI2/Charge">充值</a></li>
                <li><a href="/UI2/Withdraw">提现</a></li>
                <li><a href="/UI2/Message">消息<span class="msgcount">0</span></a></li>
                <li><a href="/UI2/Record">游戏记录</a></li>
                <li><a href="/UI2/report">盈亏记录</a></li>
                <li><a href="/UI2/Member?method=createAccount">添加下级</a></li>
                <li><a href="/UI2/UCenter">个人资料</a></li>
            </ul>
        </div>
        <div class="t_news_x fl_r">
        	<div class="m_wnews">
                <div id="miniNewsRegion">
                    <div>
                    	<span>2014-12-25</span>
                         <a href="#">由于受“黑格比”和“瑜美”2个台风的影响会导致我地区网络不稳定或者断网的现象</a>
                    </div>
                </div>
                <div class="pagesize">
                    <a href="javascript:void(0)" class="prev">上一条</a><a href="javascript:void(0)" class="next">下一条</a>
                </div>
            </div>
        </div>
    </div>
</div>

    </div>
    <!--头部结束-->
    <!--左边菜单开始-->
    <div class="left_c left">

        <div class="u_left_box">
            <div class="u_left_nav">
                <ul>
                    <li class="active"><span class="left_n_ico01"></span><a href="/UI2/Index">游戏大厅</a></li>
                    <li><span class="left_n_ico02"></span><a href="/UI2/Notify">平台公告</a></li>
                    <li><span class="left_n_ico03"></span><a href="/Promotion.html">平台活动</a></li>
                    <li><span class="left_n_ico04"></span><a href="/UI2/Shop">平台商店</a></li>
                    <li><span class="left_n_ico05"></span><a href="/Combine.html">合买大厅</a></li>
                    <li><span class="left_n_ico06"></span><a href="/UI2/Bank">充值提现</a></li>
                    <li><span class="left_n_ico07"></span><a href="/UI2/Record">游戏记录</a></li>
                    <li><span class="left_n_ico08"></span><a href="/UI2/Member">团队管理</a></li>
                    <li><span class="left_n_ico09"></span><a href="/UI2/Report">报表管理</a></li>
                    <li><span class="left_n_ico010"></span><a href="/UI2/UCenter">账户中心</a></li>
                    <li><span class="left_n_ico011"></span><a href="/UI2/Help">帮助中心</a></li>
                    <li class="l_out_ico"><span class="left_n_ico012"></span><a href="/UI2/Logout">退出平台</a></li>
                </ul>
            </div>
            <div class="u_left_out">
                <a href="#">客户端下载</a>
            </div>
            <div class="left_f_bg"></div>
        </div>

    </div>
    <!--左边菜单结束-->
    <!--右边框架开始-->
    <div class="Conframe">
       
        <%=ViewData["RightIfram"] %>

    </div>
    <!--右边框架结束-->
            <!--底部开始-->
    <div class="bottom_c foot_bg" style="float:left">
         <%:Html.AntiForgeryToken() %>
    	<div class="footer_box">
            <div class="fl_l">
                <div class="foot_news fl_l ">幸运儿</div>
                <div class="foot_news fl_l prize_list">
                   <ul id="showlist" class="pl_right" style="position:absolute;top:15px"><p><font>最新中奖：</font><a href="#">harry在分分彩201412151325期中奖4,269.00，内容：[定位胆_定位胆] ,16,,,</a></p></ul>
                </div>
            </div>
            <div class="foot_kefu_box fl_l" style="margin-left:550px">
                <div class="foot_kefu">
                    <a href="#" id="kefu" title="平台客服" target="_blank"><span style="padding-left:50px">在线客服</span></a>
                    |
                    <a href="/UI2/ChangeLine?Choose=OK"><span style="padding-left:60px">切换线路</span></a>
                </div>
            </div>
            <div class="foot_copy fl_r">
                <p>Copyright © 2014 jz5178.com All Rights Reserved</p>
            </div>
        </div>
    

    </div>
    <!--底部结束-->
</div>



<style>
    .hide {display:none}
               .fl_l ui{ float:left;}
.fl_l ui p{ float:left;}
    
</style>
<div id="sign_block" >
    <div id="btn_sign_update"><input type="button" class="hide" id="btn_save_sign" value="我要签到，签到获得积分" /></div>
    <div id="c_sign"></div>
</div>
<div id="xxx" title="温馨提示">中奖了中奖了！</div>
<script type="text/javascript">
    jQuery(".m_wnews").slide({ mainCell:"#miniNewsRegion", effect:"topLoop", autoPlay:true});
    $(document).ready(function(){     
        $('.u_left_nav li a').each(function(){
            var url=window.location.pathname;
            if(url=="/UI2/Message")
            {
                $(this).parent(".u_left_nav li").removeClass("active");
            }
            if(url==$(this).attr("href"))
            {
                $(this).parent(".u_left_nav li").addClass("active").siblings().removeClass("active");
            }
        });
        $(".user_info_tab li a").each(function(){
            var url=window.location.href.split('=');
            var aurl=$(this).attr("href").split('=');
            if(url.length==2){
                if(url[1]==aurl[1]){
                    $(this).parent(".user_info_tab li").addClass("on").siblings().removeClass("on");
                }
            }
        });
    })
</script>
<script type="text/javascript">
    function ShowList() {
                
        var temp = ""
        $.ajax({
            url: "ui2/showlist?t=" + new Date(),
            async: false,
            success: function (data) {
                temp = data.Data;
            }
        });

        $("#showlist").html(temp);
                
        jQuery(".prize_list").slide({ mainCell: ".pl_right ul", effect: "topLoop", autoPlay: true, vis: 1, scroll: 1, trigger: "click" });
        //setTimeout("ShowList()", 60000);
        return true;
    }

    //ShowList();

        </script>
    <%
        var autoRefresh = ViewData["PageAF"] == null ? 0 : 1;
        var autoRefreshTime = ViewData["PageAFT"] == null ? 30 : (int)ViewData["PageAFT"];
        if( 1 == autoRefresh )
        { 
    %>
    <script type="text/javascript">
        window.setTimeout(function () 
        {
            location.href=location.href;

        }, <%=autoRefreshTime * 1000%>);
    </script>
    <%
    }
    %>
<script type="text/javascript">
    if (window.top !== window.self) { window.top.location = window.location; }
    /*0---just click, 1---auto*/
    function get_ag_money(type)
    {
        //$("#ag_money,.point_amount,.pos_level").addClass("mny_loading");
        //$("#ag_money,.point_amount,.pos_level").html("");
        $.ajax({
            timeout: _global_ajax_timeout, cache: false, dataType: "text", type: "POST", url: "/UI2/MyInfo", success: function (a)
            {
                _check_auth(a);
                eval("var _robj=" + a + ";");
                if (1 == _robj.Code)
                
                {
                    //$("#ag_money").html(a.Data);
                    if (1 == type)
                    {
                        // $("#ag_money").html(_global_set_img_money(_robj.Data.M));
                        $("#ag_money").html(_robj.Data.M);
                        $(".point_amount").html(_robj.Data.P);
                        $(".point_amount").attr("title", _robj.Data.P);
                        $("#kefu").attr("href", _robj.Data.kf);
                        $(".uname").html(_robj.Data.N);                        
                    }
                    else if(2==type)
                    {
                        $("#ag_money").html(_robj.Data.M);
                        $(".point_amount").html(_robj.Data.P);
                        $(".point_amount").attr("title", _robj.Data.P);
                        $("#kefu").attr("href", _robj.Data.kf);
                        $(".uname").html(_robj.Data.N);     
                        $('#Conframe').attr('src', $('#Conframe').attr('src'));
                    }
                    //$(".my_level").html(_robj.Data.LN);
                    $("#set_my_level_img").removeAttr("class");
                    // $("#set_my_level_img").addClass("level" + _robj.Data.L);
                    $(".my_level_n").html(_robj.Data.LN);
                    $(".my_level_n").attr("title", _robj.Data.LN);
                    $(".pos_level").html(_robj.Data.POS);
                    $(".pos_level").attr("title",_robj.Data.POS);
                    $("#ag_money,.sag_money").attr("title", _robj.Data.M + "\n" + _global_D2B(_robj.Data.M));
                    $(".msgcount").html(""+_robj.Data.MSG.toString()+"");
                    if (0 < _robj.Data.MSG) {
                        if (!$(".l_btn_msg").hasClass("flash_word")) {
                            $(".l_btn_msg").addClass("flash_word");
                        }
                        $(".msgcount").css("display","block");
                        //$(".l_btn_msg").html($(".l_btn_msg").attr("title") + "(" + _robj.Data.MSG.toString() + ")");
                    }
                    else
                    {
                        //$(".l_btn_msg").html($(".l_btn_msg").attr("title"));
                        $(".l_btn_msg").removeClass("flash_word");
                        $(".msgcount").css("display","none");
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
            url: "ui2/IsChange?001=" + strlist + "&004=" + myamount + "&t=" + new Date(),
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
            timeout: _global_ajax_timeout, dataType: "text", type: "post", url: "/UI2/SetOnline", success: function (a) {
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
                async: false, timeout: _global_ajax_timeout, dataType: "text", type: "post", data: { y: y, m: m, method: "getsign" }, url: "/UI2/Sign?method=getsign&y=" + y.toString() + "&m=" + m.toString(), success: function (a) {
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
                timeout: _global_ajax_timeout, dataType: "text", type: "POST", url: "/UI2/Member", data: { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(), nn: nn, method: "updateNickName" }, success: function (a) {
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
            ui_show_tab("账户中心", "/UI2/UCenter", true, false);
        });

        $(".refresh_uinfo").click(function () {
            get_ag_money(2);
        });

            
        window.setInterval(function () {
            set_online();
            get_ag_money(0);

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


            

<%--            <%
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
            });--%>

        $("#btn_save_sign").click(function ()
        {
            $.ajax({
                timeout: _global_ajax_timeout, dataType: "text", type: "POST", url: "/UI2/Sign", success: function (a) {
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
            $("#btn_save_sign").removeClass("hide");
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

