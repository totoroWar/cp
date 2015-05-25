<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs019>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var dicCTList = (Dictionary<int,DBModel.wgs009>)ViewData["CTList"];
    var dicBList = (Dictionary<int, DBModel.wgs010>)ViewData["BList"];
%>
    <div class="cjlsoft-body-header">
        <h1>充值</h1>
        <div class="left-nav">声音提醒<input id="chk_sound_alert" type="checkbox" value="1" checked="checked" /></div>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
    </div>
    <div class="blank-line"></div>
    <form action="/AM/ChargeType" method="post">
        <%:Html.AntiForgeryToken()%>
        <input type="hidden" name="method" value="updateList" />
        <table width="100%" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th class="w50px">编号</th>
                    <th>用户</th>
                    <th>充值方式</th>
                    <th>金额</th>
                    <th>实际到账</th>
                    <th>手续费</th>
                    <th>充值编号</th>
                    <th>充值时间</th>
                    <th>处理时间</th>
                    <th>状态</th>
                    <th>操作人</th>
                    <th>处理人</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <%if (null != Model)
                  {
                      int listIndex = 0;
                      foreach (var item in Model)
                      { %>
                <tr>
                    <td><%:item.uc001 %></td>
                    <td><%:item.u002 %></td>
                    <td><%:dicBList[item.sb001].sb003 %>-<%:dicCTList[item.ct001].ct003 %></td>
                    <td class="fc-red"><%:item.uc002 %></td>
                    <td class="fc-green"><%:item.uc003 %></td>
                    <td class="fc-blue"><%:item.uc013 %></td>
                    <td><%:item.uc005.Trim() %></td>
                    <td><%:item.uc006 %></td>
                    <td><%:item.uc007 %></td>
                    <td>
                        <%switch (item.uc008) 
                          {
                              case 0:
                                  Response.Write("<span style='color:red;'>未处理</span>");
                                  break;
                              case 1:
                                  Response.Write("<span style='color:green;'>已完成</span>");
                                  break;
                              case 2:
                                  Response.Write("<span style='color:black;'>取消</span>");
                                  break;        
                          } %>

                    </td>
                    <td><%:item.mu002 %></td>
                    <td><%:item.mu002x %></td>
                    <td class="link-tools"><a href="javascript:void(0);" class="link-handle" data="<%:item.uc001 %>">处理</a></td>
                </tr>
                <%
                      listIndex++;
                  }/*foreach*/
              }/*if*/ %>
            </tbody>
        </table>
        <%=ViewData["PageList"] %>
    </form>
    <div id="dlg_charge_handle" class="cjlsoft-dialog-panel">
        <form id="form_confirm" action="/AM/Charge?method=confirmCharge" method="post">
        <input type="hidden" id="auth_key" name="auth_key" value="" />
        <table class="table-pro" width="100%">
            <tbody>
                <tr>
                    <td class="title">充值方式</td>
                    <td class="s_ct"></td>
                </tr>
                <tr>
                    <td class="title">充值编号</td>
                    <td class="s_cn"></td>
                </tr>
                <tr>
                    <td class="title">账号</td>
                    <td class="s_account"></td>
                </tr>
                <tr>
                    <td class="title">金额</td>
                    <td class="s_amount fc-red"></td>
                </tr>
                <tr>
                    <td class="title">实际到账</td>
                    <td class="s_trueamount "><input type="text" id="uc003" name="uc003" class="input-text w200px fc-green" value="0" /></td>
                </tr>
                <tr>
                    <td class="title">手续费</td>
                    <td class="s_fee"><input type="text" id="uc013" name="uc013" class="input-text w200px fc-blue" value="0" /></td>
                </tr>
                <tr>
                    <td class="title fc-red">取消</td>
                    <td><input type="checkbox" id="item_cancel" name="item_cancel" value="1" /></td>
                </tr>
                <tr>
                    <td class="title">备注</td>
                    <td class="s_commect">
                        <textarea cols="5" rows="3" name="uc012" class="input-text w200px"></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="title">操作人</td>
                    <td class="s_op"></td>
                </tr>
                <tr>
                    <td class="title"></td>
                    <td class=""><input type="button" id="btn_confirm_handle" value="确认" /></td>
                </tr>
                </tbody>
            </table>
            </form>
    </div>
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $("#btn_confirm_handle").click(function ()
            {
                var form_data = $("#form_confirm").serialize();
                $.ajax({
                    async: false, type: "POST", timeout: 5, url: "/AM/Charge?method=confirmCharge", data: form_data, dataType: "json",
                    success: function (a, b)
                    {
                        _check_auth(a.Code);
                        if (0 == a.Code)
                        {
                            alert(a.Message);
                        }
                        else if (1 == a.Code)
                        {
                            alert('处理成功！');
                            location.href = location.href;
                        }
                    },
                    complete: function (a, b)
                    {
                    }
                });
            });
            if ("notset" == $.cookie("chk_sound_alert"))
            {
                $("#chk_sound_alert").prop("checked", false);
            }
            $("#chk_sound_alert").change(function ()
            {
                $.cookie("chk_sound_alert", $(this).prop("checked") == true ? "set" : "notset", { expires: 128 });
            });

            $('#dlg_charge_handle').dialog(
           {
               title: '充值处理',
               width: 500,
               height: 370,
               closed: true,
               cache: false,
               modal: true
           });

            $(".link-handle").click(function ()
            {
                var data = $(this).attr("data");
                $.ajax({
                    async: false, type: "POST", timeout: 5, url: "/AM/Charge?method=edit", data:{uc001:data}, dataType: "json",
                    success: function (a, b)
                    {
                        _check_auth(a.Code);
                        if (0 == a.Code)
                        {
                            alert(a.Message);
                            return;
                        }
                        $(".s_account").html(a.Data.u002);
                        $(".s_amount").html(a.Data.uc002);
                        $(".s_op").html(a.Data.mu002);
                        $(".s_cn").html(a.Data.uc005);
                        $(".s_ct").html(a.Data.sb003 +"-"+ a.Data.ct003);
                        $("#auth_key").val(a.Data.AuthKey);
                        $("#uc003").val(a.Data.uc003);
                        if (2 == a.Data.uc008)
                        {
                            $("#item_cancel").prop("checked", true);
                        }
                        $('#dlg_charge_handle').dialog("open");
                    },
                    complete: function (a, b)
                    {
                    }
                });
            });

            //$.ionSound.play("bell_ring");
            window.setInterval(function ()
            {
                if (false == $("#chk_sound_alert").prop("checked"))
                {
                    return ;
                }
                $.ajax({
                    async: false, cache: true,timeout:5, type: "POST", timeout: 10, url: "/AM/CheckCCash",data:{status:0}, dataType: "json",
                    success: function (a, b)
                    {
                        _check_auth(a.Code);
                        if (0 < a.Data)
                        {
                            $.ionSound.play("bell_ring");
                        }
                    },
                    complete: function (a, b)
                    {
                    }
                });
            }, 1000*5);
        });
    </script>
</asp:Content>
