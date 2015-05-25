function __global_add_sb_prize(g_max, g_min, dv, params) {
    /*Init sb*/
    //$("#__td_silder_ctrl").html('<span id="__prize_silder_bar"></span>');

    $('#prize_silder_bar').slider({
        mode: 'h',
        width: "190",
        showTip: false,
        max: 100,
        min: 0,
        step: 1,
        //value: 100,
        value: dv,
        tipFormatter: function (value) {
            return value + '%';
        },
        onChange: function (v1, v2) {
            //console.log(v1);
            //console.log(v1 + "|" + (sys_my_max_point / 100).toString() + "-" + (v1*(sys_my_max_point / 100)).toFixed(1).toString() );
            var r_point = (v1 * (sys_my_max_point / 100)).toFixed(1);
            var r_prize = parseFloat(g_max) - parseFloat(g_max - g_min) * parseFloat(r_point) / parseFloat(sys_my_max_point);
            $("#prize_change_info").html(r_prize.toFixed(2) + "/" + r_point + "%");
            var opts = $("#lt_sel_dyprize option");
            $("#lt_sel_dyprize option").attr("selected", false);
            __global_save_prize(params + "_" + pri_lotteryid, r_point);
            $.each(opts, function (i, n)
            {
                var key = $(n).attr("key");
                if (parseFloat(key).toFixed(1) == r_point) {
                    $(n).attr("selected", true);
                    return true;
                }
            });
        }
    });
}

function __global_set_prize(params)
{
    if (20 == pri_lotteryid || 23 == pri_lotteryid || 26 == pri_lotteryid || 28 == pri_lotteryid)
    {
        $("#prize_change_info").hide();
        $("#prize_silder_bar_c").hide();
        //return;
    }

    if (0 == params)
    {
        $("#function_bar_right").hide();
        return;
    }

    $("#function_bar_right").show();
    //console.log("method id = " + params);
    function __this_get_max(max, min, maxp, p) {
        var sum = (max - min) * p / maxp;
        var maxPrize = min + sum;
        if (0 >= sum)
        {
            maxPrize = min;
        }
        return maxPrize.toFixed(2);
    }
    $("#lt_sel_dyprize").empty();
    var find_target = null;
    for (var mi = 0 ; mi < sys_prize_data.length; mi++) {
        var obj = sys_prize_data[mi];
        if ("NONE" == obj.IncludeSJID) {
            continue;
        }
        var split_id = obj.IncludeSJID.split(',');
        for (var i = 0; i < split_id.length; i++) {
            if (split_id[i] == params) {
                find_target = obj;
                break;
            }
        }
    }

    var g_max = 0.0;
    var g_min = 0.0;

    if (null != find_target) {
        /*get max*/
        var r_max = __this_get_max(parseFloat(find_target.Max), parseFloat(find_target.Min), sys_max_point, sys_my_max_point);
        /*pass to bar*/
        g_max = r_max;
        g_min = parseFloat(find_target.Min);
        g_dv = 0;

        var dfv = __global_get_prize(params + "_" + pri_lotteryid);

        for (var i = 0; i.toFixed(1) <= sys_my_max_point; i += 0.1)
        {
            if (5 == game_class_id)
            {
                if (i != 0)
                {
                    if (sys_my_max_point == i.toFixed(1))
                    {
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            var have_point = parseFloat(r_max) - (parseFloat(r_max) - parseFloat(find_target.Min)) * parseFloat(i) / parseFloat(sys_my_max_point);
            //var max_prize = parseFloat(find_target.Max) - have_point;
            //var set_win = parseFloat(find_target.Min).toFixed(2) + max_prize.toFixed(2);
            //console.log(have_point.toFixed(2));
            //console.log(i.toFixed(1));
            //console.log(r_max);
            //console.log(i);
            if (0 == sys_my_max_point) {
                $("#lt_sel_dyprize").append("<option key='" + i.toFixed(1) + "' title='" + i.toFixed(1) + "' value='" + find_target.Min + "|" + (i.toFixed(1) / 100).toFixed(4) + " " + (dfv == i.toFixed(1) ? 'selected="selected"' : "") + "'>" + find_target.Min.toFixed(4) + "-" + i.toFixed(1) + "%</option>");
            }
            else {
                $("#lt_sel_dyprize").append("<option key='" + i.toFixed(1) + "' title='" + i.toFixed(1) + "' value='" + have_point.toFixed(4) + "|" + (i.toFixed(1) / 100).toFixed(4) + "' " + (dfv == i.toFixed(1) ? 'selected="selected"' : "") + ">" + have_point.toFixed(4) + "-" + i.toFixed(1) + "%</option>");
            }

            //$("#lt_sel_dyprize").append("<option key='" + i.toFixed(1) + "' title='" + i.toFixed(1) + "' value='" + have_point.toFixed(2) + "|" + i.toFixed(1) + "'>" + have_point.toFixed(2) + "-" + i.toFixed(1) + "%</option>");
            /*fist init*/
            if (0 == i) {
                g_dv = i;
                $("#__prize_change_info").html(find_target.Min.toFixed(2) + "/" + i.toFixed(1) + "%");
            }
            else {
                g_dv = i;
                $("#__prize_change_info").html(have_point.toFixed(2) + "/" + i.toFixed(1) + "%");
            }
        }
        /*when selected*/
        $("#lt_sel_dyprize").change(function () {
            var select_value = $("#lt_sel_dyprize").find("option:selected").attr("key");
            var select_text = $("#lt_sel_dyprize").find("option:selected").text();
            var selecct_data = $("#lt_sel_dyprize").find("option:selected").val();
            var show_percent = select_value / (sys_my_max_point / 100);
            //console.log(parseInt(show_percent));
            $("#prize_change_info").html(select_text.replace("-", "/"));
            __global_add_sb_prize(g_max, g_min, show_percent);
            /*下单时用*/
            __global_save_prize(params + "_" + pri_lotteryid, select_value);
        });
        $("#prize_change_info").html($("#lt_sel_dyprize").find("option:selected").text().replace("-", "/"));

        if (dfv != "NONE") {
            dfv = dfv / (sys_my_max_point / 100);
            __global_add_sb_prize(g_max, g_min, dfv, params);
        }
        else {
            __global_add_sb_prize(g_max, g_min, 0, params);
        }
    }
}

function __global_save_prize(params, data) {
    //SetCookie("dypoint", data);
    SetCookie("lsp_" + params, data);
}

function __global_get_prize(params) {
    var data = getCookie("lsp_" + params);
    if (typeof (data) != "undefined") {
        return data;
    }
    return "NONE";
}