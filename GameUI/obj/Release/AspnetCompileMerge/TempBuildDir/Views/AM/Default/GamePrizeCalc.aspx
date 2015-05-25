<%@ Import Namespace="DBModel" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs004>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%" class="table-pro">
        <tr class="table-pro-head">
            <td>最大奖金：<input id="prize_max" type="text" value="1950" size="5" /></td>
            <td>最小奖金：<input id="prize_min" type="text" value="1700" size="5" /></td>
            <td>最大返点：<input id="rebate" type="text" value="12.5" size="5" /></td>
            <td>可分配返点：<span id="rebate_sum"></span></td>
        </tr>
        <tr class="table-pro-head">
            <td colspan="4">
                <input id="btn_calc" type="button" value="计算" /></td>
        </tr>
        <tr class="table-pro-head">
            <td colspan="4" style="height: 30px;">
                <div id="silder_bar" style="height: 30px; margin:0 5px; display:inline-block;"></div>
                <span id="show_p"></span>/<span id="show_r"></span>/<span id="xxx"></span></td>
        </tr>
    </table>
    <div class="blank-line"></div>
    <table width="100%" class="table-pro">
        <thead>
            <tr class="table-pro-head">
                <th>序号</th>
                <th>奖金</th>
                <th>返点</th>
                <th>赚</th>
                <th>最大返点</th>
            </tr>
        </thead>
        <tbody id="row_result">
        </tbody>
    </table>
    <script type="text/javascript">
        $("#btn_calc").click(function () {
            if ($("#prize_max").val() == "" || $("#prize_min").val() == "" || $("#rebate").val() == "") {
                alert('请填写数据！');
                return;
            }
            if ($("#prize_max").val() - $("#prize_min").val() <= 0) {
                alert('奖金差小于等于0！');
                return;
            }

            var p_max, p_min, prize_sum, rebate_sum = 0;
            p_max = $("#prize_max").val();
            p_min = $("#prize_min").val();
            rebate = $("#rebate").val();
            prize_sum = p_max - p_min;

            $('#silder_bar').slider({
                mode: 'h',
                width: "300",
                showTip: false,
                max: 100,
                min: 0,
                step: 1,
                value: 100,
                tipFormatter: function (value)
                {
                    return value + '%';
                },
                onChange: function (v1, v2)
                {
                    var r_point = (v1 * (rebate / 100)).toFixed(1);
                    $("#xxx").html(v1 /100);
                    $("#show_r").html(r_point);
                    var span_p = parseFloat(p_max) - parseFloat(prize_sum) * parseFloat(r_point) / parseFloat(rebate);
                    $("#show_p").html(span_p);
                }
            });


            $("#show_p").html(p_min);
            $("#show_r").html(rebate);

            $("#rebate_sum").html(prize_sum);
            $("#row_result").html("");

            var row_index = 1;

            for (var i = 0.0; i < rebate; i += 0.1) {
                var row_rebate = i;
                var row_prize = parseFloat(p_max) - parseFloat(prize_sum) * parseFloat(i) / parseFloat(rebate);
                var row = "<tr><td>col1</td><td>col2</td><td>col3</td><td>col4</td><td>col5</td></tr>";
                row = row.replace("col1", row_index);
                row = row.replace("col2", row_prize.toFixed(2));
                row = row.replace("col3", row_rebate.toFixed(2));
                row = row.replace("col4", (p_max - row_prize).toFixed(2));
                row = row.replace("col5", (rebate - i).toFixed(2));
                $("#row_result").append(row);
                row_index++;
            }
        });
        function Combination(c, b) {
            b = parseInt(b);
            c = parseInt(c);
            if (b < 0 || c < 0) {
                return false
            }
            if (b == 0 || c == 0) {
                return 1
            }
            if (b > c) {
                return 0
            }
            if (b > c / 2) {
                b = c - b
            }
            var a = 0;
            for (i = c; i >= (c - b + 1) ; i--) {
                a += Math.log(i)
            }
            for (i = b; i >= 1; i--) {
                a -= Math.log(i)
            }
            a = Math.exp(a);
            return Math.round(a)
        }
    </script>
</asp:Content>
