<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI2/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        #dialog table {
            width: 100%;
            height: 120px;
        }



        #address {
            width: 340px;
        }

        #shopClass li {
            margin-left: 25px;
            font-size: 16px;
        }

        #single_money2_s, #sum_money2_s {
            font-family: arial, sans-serif;
            text-decoration: line-through;
        }
    </style>
    <script type="text/javascript">
        vipDiscount = {<%=ViewData["vipDiscount"]%>};
        CurrentUserVip=<%=ViewData["CurrentUserVip"]%>;
        function buy(id,title,single_money,isDiscount)
        {
            $("#num").val(1);
            $("#title").html(title);
            $("#ProductId").val(id);
            if (isDiscount==1) {
                $("#single_money2_s").css("display","");
                $("#sum_money2_s").css("display","");
                $("#single_money2").html(single_money);
                $("#sum_money2").html(single_money);
                if (vipDiscount[CurrentUserVip]!=0) {
                    single_money=single_money*vipDiscount[CurrentUserVip]/10;
                }
            }
            else {
                $("#single_money2_s").css("display","none");
                $("#sum_money2_s").css("display","none");
            }

            $("#single_money").html(single_money);
            $("#sum_money").html(single_money);
            $("#dialog").dialog("open");
        }
        function check()
        {
            var message = "";
            if ($("#name").val() == "") {
                message += "收件人不能为空\n";
            }
            if ($("#phoneNumber").val() == "") {
                message += "电话不能为空\n";
            }
            if ($("#address").val() == "") {
                message += "收货地址不能为空\n";
            }
            if ($("#zip").val() == "") {
                message += "邮编不能为空\n";
            }
            if (message != "")
            {
                alert(message);
                return false;
            }
            else
            {
                return true;
            }

        }
        function Finishe()
        {
            if (check()) {
                $("#dialog-form").ajaxSubmit({
                    success: function (data) {
                        alert(data.Message);
                        if (data.Code == 1) {
                            $("#dialog").dialog("close");
                            window.location.reload();
                        }
                    },
                    error: function () {
                        alert("出错了");
                    }
                });
            }

        }
        $(function () {
            $("#dialog").dialog({

                title: "购物车",
                width: 530,
                height: 380,
                autoOpen:false,
                modal: true,
                buttons: {
                    '确定':function () { Finishe(); }
                ,
                    '关闭': function () {
                        $("#dialog").dialog("close");
                    }
                }
            });
            $("#num").on("change", function () {
                $("#sum_money").html(Number($("#single_money").html())*Number($("#num").val()));
                $("#sum_money2").html(Number($("#single_money2").html())*Number($("#num").val()));
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">积分商城</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="m_body_bg">
        <div class="main_box main_tbg">
            <div class="main_table_bg">
                <div class="main_table_box">
                    <!---个人资料 start-->
                    <div class="user_info_box">
                        <div class="user_info_tab">
                            <ul>
                                <span><a class="info_close" href="#"></a></span>
                                <li class="on"><a href="/UI2/shop">平台商店</a></li>
                                <%foreach (var item in (List<DBModel.wgs032>)ViewData["shopClassList"])
                                  {%>
                                <li>
                                    <a href="/UI2/Shop?classId=<%:item.rc001 %>" title="<%:item.rc002 %>"><%:item.rc002 %></a>
                                </li>
                                <%}%>
                                <li class="line">|</li>
                                <li><a href="/UI2/shopReport">订单管理</a></li>
                            </ul>
                        </div>

                        <div class="user_info_data_box">
                            <!--商品列表 start-->
                            <div class="products_list_box">
                                <div class="products_list">
                                    <div class="product_d">
                                        <%
                                            foreach (var item in (List<DBModel.wgs033>)ViewData["shopProductList"])
                                            {
                                                if (item.r009 == 0)
                                                {
                                                    continue;
                                                }
                                        %>
                                        <dl>
                                            <dt><a href="#">
                                                <img alt="未找到图片" src="<%:"/images/shop/"+item.r006 %>" title="<%:item.r002 %>" width="120px" /></a></dt>
                                            <dd>
                                                <h3 class="p-name"><%:item.r002 %></h3>
                                                <p>
                                                    <%if (item.r011 == 1)
                                                          {%><strong style="color: red;">(vip折扣)</strong>

                                                    <%} %>
                                                </p>
                                                <p id="djd1006887">价格：<font><%:item.r003.ToString("N0") %></font>积分</p>
                                                <p class="extra">
                                                    <span>人气：
                                                    </span>
                                                    <span>
                                                        <%for (int i = 1; i < item.r004; i++)
                                                          {%>
                                                        <img alt="" src="/Images/Common/xing_h.jpg" />
                                                        <%} %>
                                                    </span>
                                                </p>
                                                <p>库存：剩余 <%:item.r005 %> 件</p>
                                                <br>
                                                <a class="buy_btn" href="javascript:buy(<%:item.r001 %>,'<%:item.r002 %>',<%:item.r003 %>,<%:item.r011 %>)">立即购买</a>
                                            </dd>
                                        </dl>
                                        <%} %>
                                        <div class="clear"></div>
                                    </div>
                                </div>

                            </div>
                            <!--商品列表 end-->
                        </div>
                    </div>
                    <!---个人资料 end-->

                </div>
            </div>
        </div>


    </div>
    <div id="dialog">
        <form id="dialog-form" action="/ui/shop?method=buy" method="post">

            <input id="ProductId" name="ProductId" type="hidden" value="" />
            <table cellspacing="1" class="table-pro w100ps table-noborder">
                <tr>
                    <td>标题：</td>
                    <td id="title"></td>
                </tr>
                <tr>
                    <td>购买数量：</td>
                    <td>
                        <select id="num" name="num">
                            <option value="1" selected="selected">1</option>
                            <option value="2">2</option>
                            <option value="3">3</option>
                            <option value="4">4</option>
                            <option value="5">5</option>
                            <option value="6">6</option>
                            <option value="7">7</option>
                            <option value="8">8</option>
                            <option value="9">9</option>
                            <option value="10">10</option>
                        </select></td>
                </tr>
                <tr>
                    <td>单价：</td>
                    <td><strong id="single_money"></strong><span id="single_money2_s">原：<strong id="single_money2"></strong></span></td>
                </tr>
                <tr>
                    <td>总计：</td>
                    <td><strong id="sum_money"></strong><span id="sum_money2_s">原：<strong id="sum_money2"></strong></span></td>
                </tr>
                <tr>
                    <td>收件人：</td>
                    <td>
                        <input type="text" maxlength="10" id="name" name="name" /></td>
                </tr>
                <tr>
                    <td>电话号码：</td>
                    <td>
                        <input type="text" id="phoneNumber" maxlength="15" name="phoneNumber" /></td>
                </tr>
                <tr>
                    <td>收货地址：</td>
                    <td>
                        <input type="text" id="address" maxlength="50" name="address" /></td>
                </tr>
                <tr>
                    <td>邮编：</td>
                    <td>
                        <input type="text" maxlength="6" onkeyup="value=value.replace(/\D/,'') " id="zip" name="zip" /></td>
                </tr>
            </table>
        </form>
    </div>
</asp:Content>
