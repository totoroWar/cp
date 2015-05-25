<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="cjlsoft-body-header">
    <h1>分红审核</h1>
</div>
<div class="blank-line"></div>
    <div class="xtool">
        <input type="button" id="a-table-select" value="全选" />
        <input type="button" id="a-table-unselect" value="反选选" />
        <input type="button" id="a-table-clear-select" value="取消选择" />
        <input type="button" id="PPass" value="批量通过" />
        <input type="button" id="PClose" value="批量取消" />
        </div>
    <div id="parent_tree" class="dom-hide" data-options="lines:true,animate:false"></div>
    <div class="blank-line"></div>
    
    <table width="100%" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th>分红账号</th>
                    <th>分红比例</th>
                    <th>分红周期</th>
                    <th>周期盈亏</th>
                    <th>分红金额</th>
                    <th>审核</th>
                    
                </tr>
            </thead>
            <tbody id="addrows">
            <%=ViewData["FHLLog"] %>
        </tbody>


        </table>

    <div id="user_send_point">
        
            <input type="hidden" name="method" value="sendPoint" />
            <input type="hidden" name="key" value="0" />
            <table class="table-pro g_nco tp5" width="100%">
                <tr>
                    <td class="title">账号</td>
                    <td id="send_to_user"></td>
                </tr>
                <tr>
                    <td class="title">分红金额</td>
                    <td id="send_to_money"></td>
                </tr>
                <tr>
                    <td class="title">取消理由</td>
                    <td><input type="text" name="about" id="about" class="input-text w250px" value="手工取消" /><input style="display:none;" type="text" id="mid" class="input-text w250px" value="" /></td>
                </tr>
                <tr><td colspan="2" style="text-align:center;" ><input type="button" value="确认" class="btn-normal btn_send_point" /></td></tr>
            </table>
    </div>

        <%=ViewData["PageList"] %>



        <script type="text/javascript">

            $(document).ready(function () {

                $("#user_send_point").dialog({ width: 400, height: 200, title: "取消分红", closed: true, modal: true, position: { my: "center", at: "center", of: window } });

                $("a[name='send_point']").click(function () {
                    $("#user_send_point").dialog("open");
                    $("#send_to_user").html($(this).attr("data"));
                    $("#send_to_money").html($(this).attr("data2"));
                    $("#mid").val($(this).attr("data3"));
                });

                $(".btn_send_point").click(function () {
                    CheckUser($("#mid").val(), $("#about").val(), 2);
                    $("#about").val("手工取消");
                    $("#mid").val("-1");
                });



             });

            function CheckUser(values, about, isps) {
              
                var num = -99;
                $.ajax({
                    url: "/am/PassMoney?fid=" + values + "&about="+ about +"&isps="+ isps +"&t=" + new Date(),
                    cache: false,
                    async: true,
                    success: function (data) {
                        num = data.Data;
                        if (num == "") {
                            refresh_current_page();
                        } else {
                            alert(num);
                        }
                    }
                    
                })
            };


            $("#PPass").click(function () {
                var key = get_table_row_keys();
                
                if (key.length == 0) { alert("您没有选中任何记录"); return;}

                var status = $("#set_status").val();
                if (confirm("确认将批量通过选中记录？")) {
                    CheckUser(key, '批量通过', 1);
                }
            });


            $("#PClose").click(function () {
                var key = get_table_row_keys();

                if (key.length == 0) { alert("您没有选中任何记录"); return; }

                var status = $("#set_status").val();
                if (confirm("确认将批量取消选中记录？")) {
                    CheckUser(key, '批量取消', 2);
                }
            });


            
        </script>
     
    <div class="blank-line"></div>

    
        
</asp:Content>

