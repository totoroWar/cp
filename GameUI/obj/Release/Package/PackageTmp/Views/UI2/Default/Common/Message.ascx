<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div class="main_box main_tbg">
	<div class="main_table_bg">
    	<div class="main_table_box">
        	<div class="user_info_box">
<div class="ui_error_block user_info_tab">

	<h1>平台提示</h1>
    <div class="error_right"><span class="error_ct_time"></span>秒后自动<a href="javascript:refresh_current_page();">刷新</a></div>
    <div class="ui_error_content">
    <%=ViewData["Message"] %>
    </div>
    <script type="text/javascript">
        var _r_countdown = 30;
        $(".error_ct_time").html("30");
        var _o_countdown_obj = window.setInterval(function ()
        {
            if (0 >= _r_countdown) {
                refresh_current_page();
                _r_countdown = 30;
                window.clearInterval(_o_countdown_obj);
            }
            $(".error_right span").html(--_r_countdown);
        }, 1000);
    </script>
</div>
                </div>
            </div>
        </div>
    </div>