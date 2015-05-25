$(document).ready(function (e)
{
    $("#ui-a-refresh").click(function (e)
    {
        location.href = location.href;
    });

    $("#ui-a-back,.ui-a-back").click(function (e)
    {
        window.history.back(-1);
    });

    $(".ui-page-back").click(function () { window.history.back(-1); });

    /*表格选中变色*/
    $(".table-color-row tr[class!='table-pro-head']:even").addClass("table-color-row-even");

    _global_set_table_color_row();

    /*表格行选中*/
    $("tr[class!='table-pro-head']").click(function ()
    {
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
        if ($(this).parents("table").hasClass("g_nco"))
        {
            return;
        }
        var row = $(this);
        if (row.children(".title").length == 0)
        {
            if (row.hasClass("table-row-select"))
            {
                row.removeClass("table-row-select");
                $(row).children().removeClass("table-td-select");
            }
            else
            {
                row.addClass("table-row-select");
                $(row).children().addClass("table-td-select");
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

    $(".drop-select-to-url").change(function ()
    {
        ui_mask_panel();
        var tourl = $(this).find("option:selected").attr("tourl");
        location.href = tourl;
    });

    $(".ui-post-loading").click(function (e)
    {
        ui_mask_panel();
    });

    $("#a-table-select").click(function ()
    {
        table_row_selecct();
    });

    $("#a-table-unselect").click(function ()
    {
        table_row_unselect();
    });

    $("#a-table-clear-select").click(function ()
    {
        table_row_clear_select();
    });

    var cjlsoft_input_text_list = $(".input-text");
    $.each(cjlsoft_input_text_list, function (i, n)
    {
        $(n).attr("ui-input-text-old-value", $(this).val());
    });
    $(".input-text").change(function ()
    {
        var old_value = $(this).attr("ui-input-text-old-value");
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
            $(".show-old-value-tips").html("旧值为：" + t.attr("ui-input-text-old-value"));
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
        $(".hide-col-eml").toggleClass("hide-col");
        $("#ui-pc-body").css("min-width","1300px");
    });

    $(".hide-col-eml").addClass("hide-col");

    _global_input_only_int();

    window.setInterval(function ()
    {
        $("#current_datetime").html(__global_current_datetime());
    }, 1000);

    $(".check_number").keyup(function ()
    {
        var pattern = /^[0-9]*[1-9][0-9]*$/;
        var num = $(this).val();
        if (false == pattern.test(num))
        {
        }
    });

    $("#system-body-content").tabs({
        onSelect: function (title,index)
        {
            var cur_tab = $("#system-body-content").tabs("getTab", index);
            var cur_tab_opts = $("#system-body-content").tabs("options", cur_tab);
            var ori_url = $(cur_tab).find("iframe").attr("src");
            if (/彩/.test(title))
            {
            }
            if (/游戏记录|合买|消息|账户|报表|平台消息/.test(title))
            {
                var content = '<iframe scrolling="auto" frameborder="0"  src="' + ori_url + '" style="width:100%;height:100%;"></iframe>';
                $("#system-body-content").tabs("update", { tab: cur_tab, options: { title: title, content:content } });
                //console.log($(cur_tab).find("iframe"));
            }
        },
        onAdd: function (title, index)
        {
        },
        onContextMenu: function (e, title, index)
        {
        }
    });
});

var _global_ajax_timeout = 1000 * 10;

function _global_D2B(currencyDigits)
{
    var MAXIMUM_NUMBER = 99999999999.99;
    // Predefine the radix characters and currency symbols for output:
    var CN_ZERO = "零";
    var CN_ONE = "壹";
    var CN_TWO = "贰";
    var CN_THREE = "叁";
    var CN_FOUR = "肆";
    var CN_FIVE = "伍";
    var CN_SIX = "陆";
    var CN_SEVEN = "柒";
    var CN_EIGHT = "捌";
    var CN_NINE = "玖";
    var CN_TEN = "拾";
    var CN_HUNDRED = "佰";
    var CN_THOUSAND = "仟";
    var CN_TEN_THOUSAND = "万";
    var CN_HUNDRED_MILLION = "亿";
    //var CN_SYMBOL = "人民币";
    var CN_SYMBOL = "";
    //var CN_DOLLAR = "元";
    var CN_DOLLAR  = "";
    var CN_TEN_CENT = "角";
    var CN_CENT = "分";
    //var CN_INTEGER = "整";
    var CN_INTEGER = "";
    // Variables:
    var integral; // Represent integral part of digit number.
    var decimal; // Represent decimal part of digit number.
    var outputCharacters; // The output result.
    var parts;
    var digits, radices, bigRadices, decimals;
    var zeroCount;
    var i, p, d;
    var quotient, modulus;
    // Validate input string:
    currencyDigits = currencyDigits.toString();
    if (currencyDigits == "")
    {
        return "";
        //return "还没有输入数字！";
    }
    if (currencyDigits.match(/[^,.\d]/) != null)
    {
        return "";
        //return "请输入有效数字！";
    }
    if ((currencyDigits).match(/^((\d{1,3}(,\d{3})*(.((\d{3},)*\d{1,3}))?)|(\d+(.\d+)?))$/) == null)
    {
        return "";
        //return "请输入有效格式数字！";
    }
    // Normalize the format of input digits:
    currencyDigits = currencyDigits.replace(/,/g, ""); // Remove comma delimiters.
    currencyDigits = currencyDigits.replace(/^0+/, ""); // Trim zeros at the beginning.
    // Assert the number is not greater than the maximum number.
    if (Number(currencyDigits) > MAXIMUM_NUMBER)
    {
        return "";
        //return "数值太大！";
    }
    // Process the coversion from currency digits to characters:
    // Separate integral and decimal parts before processing coversion:
    parts = currencyDigits.split(".");
    if (parts.length > 1)
    {
        integral = parts[0];
        decimal = parts[1];
        // Cut down redundant decimal digits that are after the second.
        decimal = decimal.substr(0, 2);
    }
    else
    {
        integral = parts[0];
        decimal = "";
    }
    // Prepare the characters corresponding to the digits:
    digits = new Array(CN_ZERO, CN_ONE, CN_TWO, CN_THREE, CN_FOUR, CN_FIVE, CN_SIX, CN_SEVEN, CN_EIGHT, CN_NINE);
    radices = new Array("", CN_TEN, CN_HUNDRED, CN_THOUSAND);
    bigRadices = new Array("", CN_TEN_THOUSAND, CN_HUNDRED_MILLION);
    decimals = new Array(CN_TEN_CENT, CN_CENT);
    // Start processing:
    outputCharacters = "";
    // Process integral part if it is larger than 0:
    if (Number(integral) > 0)
    {
        zeroCount = 0;
        for (i = 0; i < integral.length; i++)
        {
            p = integral.length - i - 1;
            d = integral.substr(i, 1);
            quotient = p / 4;
            modulus = p % 4;
            if (d == "0")
            {
                zeroCount++;
            }
            else
            {
                if (zeroCount > 0)
                {
                    outputCharacters += digits[0];
                }
                zeroCount = 0;
                outputCharacters += digits[Number(d)] + radices[modulus];
            }
            if (modulus == 0 && zeroCount < 4)
            {
                outputCharacters += bigRadices[quotient];
            }
        }
        outputCharacters += CN_DOLLAR;
    }
    // Process decimal part if there is:
    if (decimal != "")
    {
        for (i = 0; i < decimal.length; i++)
        {
            d = decimal.substr(i, 1);
            if (d != "0")
            {
                outputCharacters += digits[Number(d)] + decimals[i];
            }
        }
    }
    // Confirm and return the final output string:
    if (outputCharacters == "")
    {
        outputCharacters = CN_ZERO + CN_DOLLAR;
    }
    if (decimal == "")
    {
        outputCharacters += CN_INTEGER;
    }
    outputCharacters = CN_SYMBOL + outputCharacters;
    return outputCharacters;
}

function _global_input_only_int()
{
    $(".input_only_int").keydown(function ()
    {
        var k = window.event ? event.keyCode : event.which;
        if (((k >= 48) && (k <= 57)) || ((k >= 96) && (k <= 105)) || k == 8 || k == 0)
        {
        }
        else
        {
            if (window.event)
            {
                window.event.returnValue = false;
            }
            else
            {
                e.preventDefault(); //for firefox 
            }
        }
    });
}

/*移动到变色*/
var _global_set_table_color_row = function ()
{
    $(".table-color-row tr[class!='table-pro-head']").mouseover(function ()
    {
        if ($(this).parentsUntil("table").hasClass("g_nco")) {
            return;
        }
        $(this).addClass("table-color-row-over");
        $(this).mouseout(function ()
        {
            $(this).removeClass("table-color-row-over");
        });
    });
};

function ui_show_tab(title, href, closable)
{
    if ($("#system-body-content").tabs("exists", title))
    {
        $('#system-body-content').tabs("select", title);
    }
    else
    {
        var content = '<iframe scrolling="auto" frameborder="0"  src="' + href + '" style="width:100%;height:100%;"></iframe>';
        $('#system-body-content').tabs("add", {
            title: title, closable: closable, content: content});
    }
}

function table_row_selecct()
{
    $("tr[class!='table-pro-head']").addClass("table-row-select");
    var rows = $("tr[class!='table-pro-head']");
    $.each(rows, function(i,n)
    {
        $(n).children().addClass("table-td-select");
    });
}

function table_row_unselect()
{
    var list = $("tr[class!='table-pro-head']");
    $.each(list, function (i, n)
    {
        if ($(n).hasClass("table-row-select"))
        {
            $(n).removeClass("table-row-select");
            $(n).children().removeClass("table-td-select");
        }
        else
        {
            $(n).addClass("table-row-select");
            $(n).children().addClass("table-td-select");
        }
    });
}

function table_row_clear_select()
{
    $("tr[class!='table_pro_head']").removeClass("table-row-select");
    $("tr[class!='table_pro_head']").find("td").removeClass("table-td-select");
}

function get_table_row_keys()
{
    var list = $(".table-row-select");
    var keys = "";
    $.each(list, function (i, n)
    {
        if (typeof ($(n).attr("key")) == "undefined")
        {
            return false;
        }
        keys += $(n).attr("key") + ",";
    });
    keys = keys.substr(0, keys.length - 1);
    return keys;
}

function get_table_selected_rows()
{
    return $(".table-row-select");
}

function refresh_current_page()
{
    location.href = location.href;
}

function ui_mask_panel()
{
    var post_panel = $("<div id=\"ui-post-loading\" style=\"width:100%; height:100%; z-index:1000; left:0; top:0; position:absolute; background:#000; opacity:0.5; filter:alpha(opacity=50)\">");
    $(post_panel).css("width", $(document).width() + "px");
    $(post_panel).css("height", $(document).height() + "px");
    $(post_panel).append("<div id='ui-post-loading-image' style='width:125px; height:125px; position:fixed;position:fixed;left:0px;right:0px; background:url(/Images/Common/cjlsoft_button_post_loading.gif) no-repeat top center; margin-left:auto;margin-right:auto;top:100px'></div>");
    $(document.body).append(post_panel);
    $(document.body).css("overflow", "hidden");
    //$.blockUI({ message: '<h1><img src="/Images/Common/loading_small.gif" style="padding-top:10px;" /><span style="padding:0 5px;">处理中，请稍候<span></h1>', css: {padding:10}});

    window.setTimeout(function ()
    {
        ui_mask_panel_close();
    }, 1000 * 30 * 1);
}

function ui_mask_panel_close()
{
    $(document.body).css("overflow", "auto");
    $("#ui-post-loading").remove();
    //$.unblockUI();
}

function _check_auth(a)
{
    if (/<script.*>.*<\/script>/.test(a) )
    {
        alert("登陆超时，或者帐户在其他地方登陆，请重新登陆");
        ui_mask_panel_close();
    }
}

function _global_ui(a, b, c, d, e, f)
{
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
    if (b != 0 && b < 300) {
        b = 300;
    }
    $.blockUI({ draggable: true,theme:true,allowBodyStretch:true, message: $("#" + a), css: { border: "1px solid #eee", width: b.toString() + "px", left: c.toString() + "%", top: d.toString() + "%", padding: e, cursor: "default" }, overlayCSS: { cursor: "default" } });
    $("#" + a).before("<div class='dlg_title_bar'><span class='dlg_ui_title'>" + (typeof (f) == "undefined" || f == "" ? "" : f) + "</span><span class='dlg_ui_title_bar_close close_bui' title='关闭'></span></div>");
    $(".dlg_ui_title_bar_close").hover(function () {
        $(this).toggleClass("dlg_ui_close_h");
    });
    $(".close_bui").click(function () {
        _global_close_ui();
    });
}

function _global_close_ui()
{
    $.unblockUI();
}

function __global_current_datetime()
{
    var now = new Date();

    var year = now.getFullYear();
    var month = now.getMonth() + 1;
    var day = now.getDate();

    var hh = now.getHours();
    var mm = now.getMinutes();
    var ss = now.getSeconds();

    var clock = year + "-";

    if (month < 10)
        clock += "0";

    clock += month + "-";

    if (day < 10)
        clock += "0";

    clock += day + " ";

    if (hh < 10)
        clock += "0";

    clock += hh + ":";
    if (mm < 10) clock += '0';
   
    clock += mm + ":";
    if (ss < 10)
    {
        clock += '0';
    }
    clock += ss;

    return (clock);
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

function _g_js_call(url)
{
    try {
        if (typeof (js_call) == "function") {
            js_call(url);
        }
    }
    catch (error)
    {
        //alert(error);
    }
}