<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var methodType = (string)ViewData["MethodType"];
    %>
    <div class="ui-page-content-body-header tools">
        <div class="left-nav">
            <a href="/UI/UCenter?method=changePassword" title="修改密码" <%=methodType == "changePassword" ? "class='item-select'" : "" %>>修改密码</a>
            <a href="/UI/UCenter?method=withdrawBank" title="提现银行" <%=methodType == "withdrawBank" ? "class='item-select'" : "" %>>提现银行</a>
            <a href="/UI/UCenter?method=accountInfo" title="账户资料" <%=methodType == "accountInfo" ? "class='item-select'" : "" %>>账户资料</a>
            <a href="/UI/UCenter?method=loginHistory" title="登录日志" <%=methodType == "loginHistory" ? "class='item-select'" : "" %>>登录日志</a>
        </div>
    </div>
    <div class="blank-line"></div>
    <%if ("changePassword" == methodType)
      {%>
    <form method="post" action="./" id="form_data_1">
        <input type="hidden" name="method" value="changePassword" />
        <input type="hidden" name="target" value="loginPassword" />
        <%:Html.AntiForgeryToken() %>
        <table class="table-pro tp5" width="100%">
            <tr>
                <td class="title"></td>
                <td>登录密码</td>
            </tr>
            <tr>
                <td class="title">输入旧密码：</td>
                <td>
                    <input type="password" name="old_pwd" class="input-text w200px" /></td>
            </tr>
            <tr>
                <td class="title">输入新密码：</td>
                <td>
                    <input type="password" name="new_pwd" class="input-text w200px" /></td>
            </tr>
            <tr>
                <td class="title">确认新密码：</td>
                <td>
                    <input type="password" name="new_pwd_ok" class="input-text w200px" /></td>
            </tr>
            <tr>
                <td class="title"></td>
                <td>
                    <input id="btn_change_pwd_1" type="button" class="btn-normal ui-post-loading" value="确认" /></td>
            </tr>
        </table>
    </form>
    <div class="blank-line"></div>
    <form method="post" action="./" id="form_data_2">
        <input type="hidden" name="method" value="changePassword" />
        <input type="hidden" name="target" value="cashPassword" />
        <%:Html.AntiForgeryToken() %>
        <table class="table-pro tp5" width="100%">
            <tr>
                <td class="title"></td>
                <td>资金密码</td>
            </tr>
            <tr>
                <td class="title">输入旧密码：</td>
                <td>
                    <input type="password" name="old_pwd" class="input-text w200px" /><span class="tips">如果还未设置过密码，旧密码可随意输入</span></td>
            </tr>
            <tr>
                <td class="title">输入新密码：</td>
                <td>
                    <input type="password" name="new_pwd" class="input-text w200px" /></td>
            </tr>
            <tr>
                <td class="title">确认新密码：</td>
                <td>
                    <input type="password" name="new_pwd_ok" class="input-text w200px" /></td>
            </tr>
            <tr>
                <td class="title"></td>
                <td>
                    <input id="btn_change_pwd_2" type="button" class="btn-normal ui-post-loading" value="确认" /></td>
            </tr>
        </table>
    </form>
    <div class="blank-line"></div>
    <form method="post" action="./" id="form1">
        <input type="hidden" name="method" value="changePassword" />
        <input type="hidden" name="target" value="welcomeMessage" />
        <%:Html.AntiForgeryToken() %>
        <table class="table-pro tp5" width="100%">
            <tr>
                <td></td>
                <td>问候语</td>
            </tr>
            <tr>
                <td class="title">问候语：</td>
                <td>
                    <input type="text" name="login_message" class="input-text w200px" maxlength="10" /></td>
            </tr>
            <tr>
                <td class="title"></td>
                <td>
                    <input id="btn_save_lm" type="button" class="btn-normal ui-post-loading" value="确认" /></td>
            </tr>
        </table>
    </form>
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $("#btn_change_pwd_1,#btn_change_pwd_2,#btn_save_lm").click(function ()
            {
                var form = $(this).parents("form");
                var form_data = $(this).parents("form").serialize();
                var this_id = $(this).attr("id");
                $.ajax({
                    async: false, timeout: 5, type: "POST", url: "/UI/UCenter", data: form_data, dataType: "json",
                    success: function (a, b)
                    {
                        _check_auth(a.Code);
                        if (0 == a.Code)
                        {
                            $.messager.alert('提示', a.Message, 'question');
                        }
                        else if (1 == a.Code)
                        {
                            $.messager.confirm('提示', a.Message, function (r)
                            {
                                if (r)
                                {
                                    if ("btn_change_pwd_1" == this_id)
                                    {
                                        top.location.href = "/UI/Logout";
                                    }
                                }
                            });
                            ui_mask_panel_close();
                            form[0].reset();
                        }
                    },
                    complete: function (a, b)
                    {
                        ui_mask_panel_close();
                    }
                });
            });
        });
    </script>
    <%}/*changePassword*/ %>
    <%else if ("withdrawBank" == methodType)
      { %>
    <div class="ui-page-content-body-block">
        <a href="javascript:void(0);" id="btn_withdraw_edit">添加</a>
    </div>
    <div class="blank-line"></div>
    <div id="dlg_withdraw_edit" class="ui-page-dialog-panel">
        <form action="./" method="post" id="form_data_withdraw_info">
        <input type="hidden" name="uwi001" value="0" />
        <input type="hidden" name="method" value="withdrawBank" />
        <%:Html.AntiForgeryToken() %>
        <table class="table-pro tp5" width="100%">
            <tr>
                <td class="title">资金密码</td>
                <td>
                    <input type="password" name="cash_password" class="input-text w300px i-t-fs14" value="" /></td>
            </tr>
            <tr>
                <td class="title">银行类型</td>
                <td>
                    <select name="uwt001">
                        <%
          var wtList = (List<DBModel.wgs024>)ViewData["WTList"];
          foreach (var item in wtList)
          {
                        %>
                        <option value="<%:item.uwt001 %>"><%:item.uwt003 %></option>
                        <%} %>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="title">开户人姓名</td>
                <td>
                    <input type="text" name="uwi004" class="input-text w300px i-t-fs14" value="" /></td>
            </tr>
            <tr>
                <td class="title">卡号/账号</td>
                <td>
                    <input type="text" name="uwi005" class="input-text w300px i-t-fs14" value="" /></td>
            </tr>
            <tr>
                <td class="title">确认卡号/账号</td>
                <td>
                    <input type="text" name="uwi005_confirm" class="input-text w300px i-t-fs14" value="" /></td>
            </tr>
            <tr>
                <td class="title">地区选择</td>
                <td>省区<select id="select_p"></select>
                    城市<select id="select_c"></select>
                </td>
            </tr>
            <tr>
                <td class="title">开户所在地区</td>
                <td>
                    <input type="text" id="wd_region" name="uwi006" class="input-text w300px i-t-fs14" value="" readonly="readonly" /></td>
            </tr>
            <tr>
                <td class="title">开户行</td>
                <td>
                    <input type="text" name="uwi003" class="input-text w300px i-t-fs14" value="" /></td>
            </tr>
            <tr>
                <td class="title"></td>
                <td><input type="button" id="btn_save_wb" class="btn-normal" value="保存" /></td>
            </tr>
        </table>
        </form>
    </div>
    <div class="ui-page-tips-block fs12px">
        <p><span>1</span>银行卡绑定成功后，平台任何区域都不会<span class="fs16px">出现</span>您的完整银行账号，开户姓名等信息。</p>
        <p><span>2</span>每个账号最多<span class="fs16px">“一行一卡”</span>，即每间银行只能绑定一张卡。您已成功绑定<span class="fs16px"><%:ViewData["BindMWTCount"] %></span>张。</p>
        <p><span>3</span>新绑定的提款银行卡需要绑定时间超过<span class="fs16px"><%:ViewData["WDTime"] %></span>小时才能正常提款。</p>
        <p><span>4</span>一个账户只能绑定同一个开户人姓名的银行卡。</p>
        <p><span>5</span><span class="fs16px">第一次</span>使用的开户名将作为其他银行的用户名。</p>
    </div>
    <div class="blank-line"></div>
    <table class="table-pro tp5" width="100%">
        <thead>
            <tr class="table-pro-head">
                <th class="w50px">编号</th>
                <th>类型</th>
                <th>卡号/账号</th>
                <th>姓名</th>
                <th>开户行</th>
                <th>地区</th>
                <th>绑定时间</th>
            </tr>
        </thead>
        <tbody>
            <%
                var wtDicList = wtList.ToDictionary(exp => exp.uwt001);
                var mwtList = (List<DBModel.wgs023>)ViewData["MWTList"];
                if( null != mwtList)
                {
                    foreach(var item in mwtList)
                    {
            %>
            <tr>
                <td><%:item.uwi001 %></td>
                <td><%:wtDicList[item.uwt001].uwt003 %></td>
                <td><%:item.uwi005 %></td>
                <td><%:item.uwi004 %></td>
                <td><%:item.uwi003 %></td>
                <td><%:item.uwi006 %></td>
                <td><%:item.uwi010 %></td>
            </tr>

            <%
            }/*foreach*/
            }/*if*/%>
        </tbody>
    </table>
    <script type="text/javascript" src="/Scripts/UI/Default/Location.js"></script>
    <script type="text/javascript">
        //_set_lpc_select("select_p", "select_c", "wd_region");
        $(this).set_lpc_select("select_p", "select_c", "wd_region");
        $('#dlg_withdraw_edit').dialog(
            {
                title: '提现银行编辑',
                width: 480,
                height: 390,
                closed: true,
                cache: false,
                modal: true
            });
        $("#btn_withdraw_edit").click(function ()
        {
            $('#dlg_withdraw_edit').dialog("open");
        });
        $("#btn_save_wb").click(function ()
        {
            var form = $(this).parents("form");
            var form_data = $(this).parents("form").serialize();
            $.ajax({
                async: false, timeout: 5, type: "POST", url: "/UI/UCenter", data: form_data, dataType: "json",
                success: function (a, b)
                {
                    _check_auth(a.Code);
                    $.messager.alert('提示', a.Message, 'question');
                    if (0 == a.Code)
                    {
                    }
                    else if (1 == a.Code)
                    {
                        location.href = location.href;
                        ui_mask_panel_close();
                    }
                },
                complete: function (a, b)
                {
                    ui_mask_panel_close();
                }
            });
        });
    </script>
    <%}/*withdrawBank*/ %>
    <%else if( "accountInfo" == methodType)
      {
          var agPointList = (List<DBModel.wgs017>)ViewData["AGPoint"];
          var gList = (List<DBModel.wgs001>)ViewData["GList"];
          var gcList = (List<DBModel.wgs006>)ViewData["GCList"];
          var gpList = (List<DBModel.wgs007>)ViewData["GPList"];
          var gDicList = gList.ToDictionary(exp => exp.g001);
          var gpDicList = gpList.ToDictionary(exp => exp.gp001);
          var gcDicList = gcList.ToDictionary(exp => exp.gc001);
          %>
    <table class="table-pro tp5" width="100%">
        <tr>
            <td class="title">账号</td>
            <td><%:ViewData["UILoginAccount"] %><span class="tips"><%:ViewData["UILoginNickname"] %></span></td>
        </tr>
        <tr>
            <td class="title">余额</td>
            <td><%:string.Format("{0:N4}",ViewData["AGMoney"]) %></td>
        </tr>
        <tr>
            <td class="title">积分</td>
            <td><%:string.Format("{0:N4}",ViewData["AGMoney"]) %></td>
        </tr>
        <tr>
            <td class="title">返点</td>
            <td>
                <%foreach( var agp in agPointList)
                  {
                      var gpkey = agp.gp001;
                      var gcKey = gpDicList[(int)agp.gp001].gc001;
                      var gcName = gcDicList[gpDicList[(int)agp.gp001].gc001].gc003;
                      var gameIDs = gcDicList[gpDicList[(int)agp.gp001].gc001].gc004.Split(',');
                      
                      %>
                <span><%:gcName %></span>
                <%foreach( var g in gameIDs) {%>
                <span class="tips"><%:gDicList[int.Parse(g)].g003 %></span>
                <%} %><a href="javascript:void(0);" gpkey="<%:gpkey %>" gckey="<%:gcKey %>" class="show_agp_detail">[查看详细]</a>
                <br />
                <span class="fc-red"><%:string.Format("{0:N1}",agp.up003) %></span>
                <%
                }/*foreach gplist*/ %>
            </td>
        </tr>
    </table>
    <form action ="./" method="post" id="show_agp_detail">
        <%:Html.AntiForgeryToken() %>
        <%--<input type="hidden" name="target" value="show_agp_detail" />--%>
        <input type="hidden" id="agp_gpkey" name="gpkey" value="0" />
        <input type="hidden" id="agp_gckey" name="gckey" value="0" />
        <%--<input type="hidden" name="method" value="accountInfo" />--%>
    </form>
    <script type="text/javascript">
        $(".show_agp_detail").click(function ()
        {
            $("#agp_gpkey").val($(this).attr("gpkey"));
            $("#agp_gckey").val($(this).attr("gckey"));
            var form_data = $("#show_agp_detail").serialize();
            $.ajax({
                async: false, timeout: 5, type: "POST", url: "/UI/UserPrizeInfo", data: form_data, dataType: "html",
                success: function (a, b)
                {
                    $(".agup_info").html(a);
                },
                complete: function (a, b)
                {
                    ui_mask_panel_close();
                }
            });
        });
    </script>
    <div class="blank-line"></div>
    <div class="agup_info">

    </div>
    <%}/*accountInfo*/ %>
</asp:Content>
