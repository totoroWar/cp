$(document).ready(function (e) {
    $("#cjlsoft-a-refresh").click(function (e) {
        refresh_current_page();
    });

    $("#cjlsoft-a-back,.cjlsoft-a-back").click(function (e) {
        window.history.back(-1);
    });

    /*表格选中变色*/
    $(".table-color-row tr[class!='table-pro-head']:even").addClass("table-color-row-even");

    /*移动到变色*/
    $(".table-color-row tr[class!='table-pro-head']").mouseover(function () {
        $(this).addClass("table-color-row-over");
        $(this).mouseout(function ()
        {
            $(this).removeClass("table-color-row-over");
        });
    });

    /*表格行选中*/
    $("tr[class!='table-pro-head']").click(function () {
        try
        {
            if (event.srcElement.nodeName == null || event.srcElement.nodeName != "TD")
            {
                return;
            }
        }
        catch (error)
        {
        }
        if ($(this).parent("table").hasClass("g_nco"))
        {
            return;
        }
        var row = $(this);
        if (row.children(".title").length == 0) {
            if (row.hasClass("table-row-select")) {
                row.removeClass("table-row-select");
            }
            else {
                row.addClass("table-row-select");
            }
        }
    });

    $(".input-text").mouseover(function ()
    {
        $(this).addClass("input-text-hover");
        $(this).mouseout(function ()
        {
            $(this).removeClass("input-text-hover");
        });
    });

    //$(this).attr("cjlsoft-input-text-old-value", $(this).val());
    //$(".input-text").attr("cjlsoft-input-text-old-value", $(this).val());

    $(".drop-select-to-url").change(function () {
        cjlsoft_mask_panel();
        var tourl = $(this).find("option:selected").attr("tourl");
        location.href = tourl;
    });

    $(".cjlsoft-post-loading").click(function (e) {
        cjlsoft_mask_panel();
    });

    $("#a-table-select").click(function () {
        table_row_selecct();
    });

    $("#a-table-unselect").click(function () {
        table_row_unselect();
    });

    $("#a-table-clear-select").click(function () {
        table_row_clear_select();
    });

    var cjlsoft_input_text_list = $(".input-text");
    $.each(cjlsoft_input_text_list, function (i, n)
    {
        $(n).attr("cjlsoft-input-text-old-value", $(this).val());
    });
    $(".input-text").change(function ()
    {
        var old_value = $(this).attr("cjlsoft-input-text-old-value");
        //alert("new "+$(this).val() + "- old " + old_value);
        if (old_value != $(this).val() && "" != old_value)
        {
            $(this).addClass("input-text-update");
        }
        else
        {
            $(this).removeClass("input-text-update");
        }
    });

    $('.input-textx').tooltip({
        content: "<div class='show-old-value-tips'></div>",
        showEvent: 'mouseover',
        onShow: function ()
        {
            var t = $(this);
            if (false == t.hasClass("input-text-update"))
            {
                return;
            }
            $(".show-old-value-tips").html("旧值为：" + t.attr("cjlsoft-input-text-old-value"));
            t.tooltip('tip').unbind().bind('mouseover', function ()
            {
                t.tooltip('show');
            }).bind('mouseout', function ()
            {
                t.tooltip('hide');
            });
        }
    });

    $(".show-table-all-col").bind("click", function ()
    {
        $(".hide-col-eml").toggleClass("dom-hide");
    });

    $(".hide-col-eml").addClass("dom-hide");

});

var _global_ajax_timeout = 1000 * 10;

function ui_show_tab(title, href, closable) {
    if ($("#system-body-content").tabs("exists", title)) {
        $('#system-body-content').tabs("select", title);
    }
    else {
        var content = '<iframe scrolling="auto" frameborder="0"  src="' + href + '" style="width:100%;height:100%;"></iframe>';
        $('#system-body-content').tabs("add", { title: title, closable: closable, content: content });
    }
}

function table_row_selecct() {
    $("tr[class!='table-pro-head']").addClass("table-row-select");
}

function table_row_unselect() {
    var list = $("tr[class!='table-pro-head']");
    $.each(list, function (i, n) {
        if ($(n).hasClass("table-row-select")) {
            $(n).removeClass("table-row-select");
        }
        else {
            $(n).addClass("table-row-select");
        }
    });
}

function table_row_clear_select() {
    $("tr[class!='table_pro_head']").removeClass("table-row-select");
}

function get_table_row_keys() {
    var list = $(".table-row-select");
    var keys = "";
    $.each(list, function (i, n) {
        if (typeof ($(n).attr("key")) == "undefined") {
            return false;
        }
        keys += $(n).attr("key") + ",";
    });
    keys = keys.substr(0, keys.length - 1);
    return keys;
}

function get_table_selected_rows() {
    return $(".table-row-select");
}

function refresh_current_page() {
    location = location;
}

function _global_ui(a, b, c, d, e, f, g) {
    /*
    a ctrl
    b with
    c left
    d top
    e padding
    f title
    g left top model % or px
    */
    if (b == 0) {
        b = $("#" + a).width();
    }
    if (b != 0 && b < 300)
    {
        b = 300;
    }
    $.blockUI({ draggable: true, message: $("#" + a), css: { border: "1px solid #eee", width: b.toString() + "px", left: c.toString() + "%", top: d.toString() + "%", padding: e, cursor: "default" }, overlayCSS: { cursor: "default" } });
    $("#" + a).before("<div class='dlg_title_bar'><span class='dlg_ui_title'>"+(typeof(f) == "undefined" || f=="" ? "对话框" : f)+"</span><span class='dlg_ui_title_bar_close close_bui' title='关闭'></span></div>");
    $(".dlg_ui_title_bar_close").hover(function () {
        $(this).toggleClass("dlg_ui_close_h");
    });
    $(".close_bui").click(function () {
        _global_close_ui();
    });
}

function _global_close_ui() {
    $.unblockUI();
}

function _json_date(time) {
    if (time != null) {
        var date = new Date(parseInt(time.replace("/Date(", "").replace(")/", ""), 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var current_date = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var current_time = date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds()
        return date.getFullYear() + "-" + month + "-" + current_date + " " + current_time;
    }
    return "";
}

function cjlsoft_mask_panel() {
    var post_panel = $("<div id=\"cjlsoft-post-loading\" style=\"width:100%; height:100%; z-index:1000; left:0; top:0; position:absolute; background:#000; opacity:0.5; filter:alpha(opacity=50)\">");
    $(post_panel).css("width", $(document).width() + "px");
    $(post_panel).css("height", $(document).height() + "px");
    $(post_panel).append("<div id='cjlsoft-post-loading-image' style='width:125px; height:125px; position:fixed;position:fixed;left:0px;right:0px; background:url(/Images/Common/cjlsoft_button_post_loading.gif) no-repeat top center; margin-left:auto;margin-right:auto;top:100px'></div>");
    $(document.body).append(post_panel);

    window.setTimeout(function () {
        cjlsoft_mask_panel_close();
    }, 1000 * 60 * 1);
}
function cjlsoft_mask_panel_close() {
    $("#cjlsoft-post-loading").remove();
}

function _check_auth(status)
{
    if (999 == status)
    {
        alert('登录超时！请重新登录。');
    }
}

var _global_ajax_timeout = 1000 * 5;