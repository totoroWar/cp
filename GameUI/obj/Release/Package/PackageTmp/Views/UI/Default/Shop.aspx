<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
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
    <style type="text/css">
        .p-img img {
            width: 246px;
            height: 220px;
        }

        a {
            text-decoration: none;
        }

        .p-name {
            font-size: 14px;
        }

        strong {
            font-weight: bolder;
        }

        ul {
            list-style-type: none;
            display: block;
        }

        li {
            float: left;
        }

        .btns span {
            margin-top: 10px;
            padding: 3px 5px 3px 5px;
            font-size: 15px;
        }

        .btns a {
            color: #fff;
        }

        a:hover {
            color: #26b1cf;
        }

        .btns span:hover {
            background-color: #fff;
        }

        #content ul {
            color: rgb(102, 102, 102);
            font-family: Arial, Verdana, 宋体;
            line-height: 18px;
            list-style-image: none;
            list-style-position: outside;
            list-style-type: none;
            margin-bottom: 10px;
            margin-left: 0px;
            margin-right: 0px;
            margin-top: 0px;
            padding-bottom: 0px;
            padding-left: 0px;
            padding-right: 0px;
            padding-top: 0px;
            width: 1130px;
            zoom: 1;
        }

            #content ul li {
                /*border-radius: 18px;*/
                color: #ffffff;
                background-color: #7e0001;
                float: left;
                text-align: left;
                width: 245px;
                margin-left: 20px;
                margin-top: 20px;
                padding: 5px 5px 5px 5px;
            }

        #dialog table {
            width: 100%;
            height: 120px;
            background-color: #e1e1e1;
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
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    积分商城
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="ui-page-content-body-header tools">
        <div class="left-nav">
            <a href="/UI/shop">商店</a>
            <a href="/UI/shopReport">订单</a>
        </div>
    </div>
    <div class="blank-line"></div>
    <div class="cjlsoft-body-header">
    </div>
    <form action="/ui/Shop" method="post">
        <div class="blank-line"></div>
        <div class="ui-page-content-body-header tools">
            <div class="left-nav">
                <ul>
                    <%foreach (var item in (List<DBModel.wgs032>)ViewData["shopClassList"])
                      {%>
                    <li>
                        <a href="/ui/Shop?classId=<%:item.rc001 %>" title="<%:item.rc002 %>"><%:item.rc002 %></a>
                    </li>
                    <%}%>
                </ul>
            </div>
        </div>
        <div class="blank-line"></div>
        <div id="content">
            <ul>
                <%
                    foreach (var item in (List<DBModel.wgs033>)ViewData["shopProductList"])
                    {
                        if (item.r009 == 0)
                        {
                            continue;
                        }
                %>
                <li>
                    <div class="lh-wrap">
                        <div class="p-img">
                            <a href="#">
                                <img alt="未找到图片" src="<%:"/images/shop/"+item.r006 %>" title="<%:item.r002 %>" />
                            </a>
                        </div>
                        <div class="p-name">
                            <%:item.r002 %>
                        </div>
                        <div id="djd1006887" class="p-price">
                            价格：
        <strong>
            <%:item.r003 %> 积分
        </strong>
                            <%if (item.r011 == 1)
                              {%><strong style="color: red; font-size: 18px;">(vip折扣)</strong>

                            <%} %>
                        </div>
                        <div class="extra">
                            <span>人气：
                            </span>
                            <span>
                                <%for (int i = 0; i < item.r004; i++)
                                  {%>
                                <img alt="" src="/Images/Common/xing.png" />
                                <%} %>
                            </span>
                        </div>
                        <div>
                            <span>上架时间:</span>   <span><%:item.r007.ToString("yyyy-MM-dd") %></span>
                        </div>
                        <div>
                            <span>下架时间:</span>   <span><%:item.r010.ToString("yyyy-MM-dd") %></span>
                        </div>
                        <div class="stocklist">
                            <span class="st33">剩余<%:item.r005 %>件
                            </span>
                        </div>
                        <div class="btns">
                            <%if (item.r010 < DateTime.Now)
                              {%>
                            <span style="color: red;">商品已下架</span>
                            <%}
                              else
                              { %>
                            <a href="javascript:buy(<%:item.r001 %>,'<%:item.r002 %>',<%:item.r003 %>,<%:item.r011 %>)">
                                <span>立即购买</span>
                            </a>
                            <%} %>
                        </div>
                        <div class="p-shopnum">
                        </div>
                    </div>
                </li>
                <%

                    } %>
            </ul>
        </div>
        <div class="blank-line"></div>
    </form>
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
