<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="cjlsoft-body-header">
    <h1>分红设置</h1>
</div>
<div class="blank-line"></div>

    <div id="parent_tree" class="dom-hide" data-options="lines:true,animate:false"></div>
    <div class="blank-line"></div>
    <form action="/AM/GetMoneySet" method="get">
    <table width="100%" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th>分红账号</th>
                    <th>分红比例（百分比）</th>
                    <th>分红周期（天）</th>
                    <th>起始时间</th>
                    <th>发放时间</th>
                    <th>操作</th>
                    
                </tr>
            </thead>
            <tbody id="addrows">
            <%=ViewData["FHList"] %>
        </tbody>

<tr class="table-pro-head">
                   
                    <th ><input type="text" class="input-text w100px" name="ucode" id="ucode" value="" />
                        <font id="dc"  color="green" ></font>
                    </th>
                    <th><input type="text" class="input-text w150px NumDecText" name="ubl" id="ubl"  value="" /></th>
                    <th><input type="text" class="input-text w150px NumDecText" name="uday" id="uday" value="" /></th>
                    <th><input type="text" class="input-text w150px" id="utime" name="utime" value="" /></th>
                    <th><input type="text" class="input-text w150px" id="ftime" name="ftime" value="" /></th>
                    <th  ><input type="button" id="addThis"  class="btn-normal ui-post-loading" value="添加" />
                    <input type='text'  name='xindex' id='xindex' style='display:none;' value='0' />
                    </th>
                   
                </tr>
                <tr class="table-pro-head">
                    <th colspan="5" align="right" ></th>
                    <th ><input type="submit" id="sv" class="btn-normal ui-post-loading" value="保存" /></th>
                    </tr>

        </table>
     </form>
    <div id="user_send_point">
        
            <input type="hidden" name="method" value="sendPoint" />
            <input type="hidden" name="key" value="0" />
            <table class="table-pro g_nco tp5" width="100%">
                <tr>
                    <td class="title">账号</td>
                    <td id="send_to_user"></td>
                </tr>
                <tr>
                    <td class="title">分红比例</td>
                    <td ><input type="text" name="fbl" id="fbl" class="input-text w100px NumDecText" value="0" /> %</td>
                </tr>
                <tr>
                    <td class="title">分红周期</td>
                    <td><input type="text" name="fzq" id="fzq" class="input-text w100px NumDecText" value="0" /> 天<input style="display:none;" type="text" id="mid" class="input-text w250px" value="" /></td>
                </tr>
                <tr><td colspan="2" style="text-align:center;" ><input type="button" value="确认修改" class="btn-normal btn_send_point" /></td></tr>
            </table>
    </div>


    <%=ViewData["PageList"] %>
    <div class="blank-line"></div>
    
    <script type="text/javascript">
        $(document).ready(function () {

            $("#user_send_point").dialog({ width: 300, height: 150, title: "修改分红设置", closed: true, modal: true, position: { my: "center", at: "center", of: window } });

            $("a[name='send_point']").click(function () {
                $("#user_send_point").dialog("open");
                $("#send_to_user").html($(this).attr("data2"));

                $("#fbl").val($(this).attr("data3") * 100);
                $("#fzq").val($(this).attr("data4"));
                
                
                $("#mid").val($(this).attr("data"));
            });

            $(".btn_send_point").click(function () {
                
                var myurl = "/am/SetMoney?fid=" + $("#mid").val() + "&fbl=" + $("#fbl").val() + "&fzq=" + $("#fzq").val() + "&t=" + new Date();
                PostBF(myurl);
                
            });

            jQuery('#utime').datetimepicker({
                format: 'Y/m/d H:i:00',
                lang: 'ch',
                timepicker: true
            });

            jQuery('#ftime').datetimepicker({
                format: 'H:i:00',
                lang: 'ch',
                datepicker:false,
                timepicker: true
                
            });


        });


        function PostBF(myurl) {

            
            var num = -99;
            $.ajax({
                url: myurl,
                cache: false,
                async: true,
                success: function (data) {
                    num = data.Data;
                    if (num == "") {
                        $("#xindex").val("0");
                        refresh_current_page();
                    } else {
                        alert(num);
                        $("#user_send_point").dialog("close");
                        $("#fbl").val("0");
                        $("#fzq").val("0");
                        $("#mid").val("-1");
                    }
                }

            })
        };

        $("#addThis").click(function () {
            CheckUser($("#ucode").val(), 0);

            
        });
        
        $("#ucode").blur(function () {
            CheckUser($("#ucode").val(), 1);


        });


        function xaddThis(value,code)
        {

            try {
                if ($.isNumeric($("#ubl").val()) <= 0) {
                    alert("分红比例填写错误");
                    return false;
                }

                if ($.isNumeric($("#uday").val()) <= 0) {
                    alert("分红周期填写错误");
                    return false;
                }
               
                

            }
            catch (e) {
                alert("分红数据填写错误" + e);
                return false;
            }


            var index = $("#xindex").val();
            index++;

            var temp = "";

            temp += "<tr >";
            temp += "<td>" + $("#ucode").val() + "<input type='text'  name='u" + index + "' id='u" + index + "' style='display:none;' value='" + $("#ucode").val() + "' />";
            temp += "<input type='text'  name='i" + index + "' id='i" + index + "' style='display:none;' value='" + value + "' /></td>";
            temp += "<td>" + $("#ubl").val() + "%<input type='text'  name='b" + index + "' id='b" + index + "' style='display:none;' value='" + $("#ubl").val() + "' /></td>";
            temp += "<td>" + $("#uday").val() + "<input type='text'  name='d" + index + "' id='d" + index + "' style='display:none;' value='" + $("#uday").val() + "' /></td>";
            temp += "<td>" + $("#utime").val() + "<input type='text'  name='s" + index + "' id='s" + index + "' style='display:none;' value='" + $("#utime").val() + "' /></td>";
            temp += "<td>" + $("#ftime").val() + "<input type='text'  name='f" + index + "' id='f" + index + "' style='display:none;' value='" + $("#ftime").val() + "' /></td>";
            temp += "<td><a  onclick='javascript:$(this).parent().parent().remove();' style='cursor:pointer;'>删除</a></td>";
            temp += "</tr>";

            
            $("#xindex").val(index);
            document.getElementById("addrows").innerHTML += temp;

            cle();
        }

        function addNDays(date,n){
            
            var mydate = new Date(Date.parse(date.replace(/\-/g, "\/")));  
            
            var time = mydate.getTime();
            var myTime = time+n*24*60*60*1000;
            var newTime = new Date(myTime);
            
            var year = newTime.getYear() + 1900;   
            var month = newTime.getMonth() + 1;   
            var date = newTime.getDate();   
            var hour = newTime.getHours();   
            var minute = newTime.getMinutes();   
            var second = newTime.getSeconds();   
            return year + "-" + month + "-" + date + " " + hour + ":" + minute + ":" + second;  

        };

        $(function(){     
            /*JQuery 限制文本框只能输入数字*/  
            $(".NumText").keyup(function(){    
                    $(this).val($(this).val().replace(/D|^0/g,''));  
                }).bind("paste",function(){  //CTR+V事件处理    
                    $(this).val($(this).val().replace(/D|^0/g,''));     
                }).css("ime-mode", "disabled"); //CSS设置输入法不可用    
 
            /*JQuery 限制文本框只能输入数字和小数点*/  
            $(".NumDecText").keyup(function(){    
                    $(this).val($(this).val().replace(/[^0-9.]/g,''));    
                }).bind("paste",function(){  //CTR+V事件处理    
                    $(this).val($(this).val().replace(/[^0-9.]/g,''));     
                }).css("ime-mode", "disabled"); //CSS设置输入法不可用    
        });

        function cle()
        {
            $("#dc").html("");
            $("#ubl").val("");
            $("#uday").val("");
            $("#ucode").val("");
            $("#addThis").enable(true);
        }

        function CheckUser(values,x) {

            if (values == "") {
                $("#dc").html("");
                return;
            }

            var num = -99;
            $.ajax({
                url: "/am/CheckUser?001=" + values + "&t=" + new Date(),
                cache: false,
                async: true,
                success: function (data) {
                    num = data.Data;
                    if (num > 0) {

                        for (var i = 1; i <= $("#xindex").val() ; i++) {
                            if (typeof ($("#u" + i).val()) != "undefined") {
                                if ($("#u" + i).val() == values) {
                                    
                                    
                                    $("#dc").attr("color", "red");
                                    $("#dc").html("× 账号已添加");
                                    $("#addThis").enable(false);
                                    return false;
                                }
                            }
                        }

                        if (x == 0) {
                            $("#dc").attr("color", "green");
                            $("#dc").html("√");
                            $("#addThis").enable(true);
                            xaddThis(num, values);
                        }
                        else {
                            $("#dc").attr("color", "green");
                            $("#dc").html("√");
                            $("#addThis").enable(true);
                        }
                    }
                    else if (num == -1) {
                        //alert("账号不存在！");
                        $("#dc").attr("color", "red");
                        $("#dc").html("× 账号不存在");
                        $("#addThis").enable(false);
                        return false;
                    }
                    else if (num == -2) {
                        
                        $("#dc").attr("color", "red");
                        $("#dc").html("× 账号已分红");
                        $("#addThis").enable(false);
                        return false;
                    }
                    else
                    {
                        alert("登录错误！");
                        $("#addThis").enable(false);
                        return false;
                    }
                }
            });


        }


        


    </script>
</asp:Content>


