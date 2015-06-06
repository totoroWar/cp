<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<DBModel.wgs012>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var cpl = (List<DBModel.wgs009>)ViewData["CPList"];
        var bankList = (Dictionary<int,DBModel.wgs010>)ViewData["BankDicLidt"];
    %>
    <div class="cjlsoft-body-header">
        <h1>添加代理</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
    </div>
    <div class="blank-line"></div>
    <%if (null != ViewData["ErrorMessage"])
      { %>
    <div class="cjlsoft-tips-block">
        <p><span>错误</span><%:ViewData["ErrorMessage"] %></p>
    </div>
    <div class="blank-line"></div>
    <%} %>
    <form action="/AM/MemberAgent" method="post" id="form_add_agent">
        <%:Html.AntiForgeryToken()%>
        <input type="hidden" name="u001" value="0" />
        <input type="hidden" name="method" value="add" />
        <input type="hidden" name="u023" value="0" />
        <input type="hidden" name="u018" value="0" />
        <input type="hidden" name="u019" value="0" />
        <table class="table-pro" width="100%">
            <tbody>
                <tr>
                    <td class="title">上级</td>
                    <td>
                        <input type="hidden" id="u012" name="u012" class="input-text w300px" value="<%:Model.u012 %>" /><span id="span_select_parent" style="color: #0094ff; padding: 0 5px;"></span><input type="button" id="btn_select_parent" value="选择..." class="btn-normal" /><input type="button" id="set_default_0" value="顶级" class="btn-normal" /></td>
                </tr>
                <tr>
                    <td class="title">账号</td>
                    <td>
                        <input type="text" id="u002" name="u002" class="input-text w300px" value="<%:Model.u002 %>" /><span class="tips" id="span_account"></span></td>
                </tr>
                <tr>
                    <td class="title">昵称</td>
                    <td>
                        <input type="text" name="u003" class="input-text w300px" value="<%:Model.u003 %>" /></td>
                </tr>
                <tr>
                    <td class="title">密码</td>
                    <td>
                        <input type="text" name="u009" class="input-text w300px" value="" /></td>
                </tr>
                <tr>
                    <td class="title">状态</td>
                    <td>
                        <select name="u008">
                            <option value="1">正常</option>
                            <option value="0">停用</option>
                            <option value="2">暂停</option>
                            <option value="3">冻结</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td class="title">允许开下级</td>
                    <td>
                        <select name="u020">
                            <option value="1">允许</option>
                            <option value="0">不允许</option>
                        </select>
                    </td>
                </tr>
                <tr class="tr_ct">
                    <td class="title">支付方式</td>
                    <td>
                        <%foreach (var cp in cpl)
                          { %>
                        <label style="padding: 5px;" for="l_<%:cp.ct001 %>" title="<%:bankList[cp.sb001].sb003 %>"><%:bankList[cp.sb001].sb003 %><input id="l_<%:cp.ct001 %>" type="checkbox" name="u017" value="<%:cp.ct001 %>" /></label>
                        <%} %>
                    </td>
                </tr>
                <tr class="tr_root_parent">
                    <td class="title">游戏及奖金</td>
                    <td class="parent_pd_data">
                        <table class="table-pro" width="100%">

                            <%
                                /*游戏类型*/
                                var gcList = (List<DBModel.wgs006>)ViewData["GCList"];
                                var dicGList = (ViewData["GList"] as List<DBModel.wgs001>).ToDictionary(exp => exp.g001);
                                var gpIndex = 0;
                                foreach (var gc in gcList)
                                { %>
                            <tr>
                                <td class="title w150px"><%:gc.gc003 %></td>
                                <td>
                                    <%
                                    /*游戏奖金组*/
                                    var gcpList = (List<DBModel.wgs007>)ViewData["GCPList"];
                                    /*某类型游戏的奖金组*/
                                    var gcpEList = gcpList.Where(exp => exp.gc001 == gc.gc001).ToList();
                                    /*游戏ID组*/
                                    List<string> gIDsList = new List<string>();
                                    if (gcpEList.Count() > 0)
                                    {
                                        gIDsList = gc.gc004.Split(',').ToList();
                                    }
                                    %>
                                    <input type="hidden" name="[<%:gpIndex %>].gc001" value="<%:gc.gc001 %>" />
                                    <input type="hidden" name="[<%:gpIndex %>].up001" value="0" />
                                    <select name="[<%:gpIndex%>].gp001" class="w150px pd_select">
                                        <%foreach (var gcpe in gcpEList)
                                          { %>
                                        <option value="<%:gcpe.gp001 %>" data="<%:gcpe.gp008 %>"><%:gcpe.gp003 %>/<%:gcpe.gp008 %>%</option>
                                        <%} %>
                                    </select>赚下级定位返点<input type="text" name="[<%:gpIndex %>].up002" value="0" class="input-text w50px pd_input" />%，不定位返点<input type="text" name="[<%:gpIndex %>].up004" value="0" class="input-text w50px pd_input" />%
                                </td>
                            </tr>
                            <%
                                          gpIndex++;
                                }/*foreach end 游戏分类*/ %>
                        </table>
                    </td>
                </tr>
                <tr class="tr_parent dom-hide">
                    <td class="title">游戏及奖金</td>
                    <td>
                        <table class="table-pro" width="100%" id="pd_info">
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="title">管理员</td>
                    <td><%:ViewData["AMLoginUserAccount"] %></td>
                </tr>
                <tr>
                    <td class="title"></td>
                    <td>
                        <input id="save" type="submit" value="保存" class="cjlsoft-post-loading btn-normal" /></td>
                </tr>
            </tbody>
        </table>

    </form>

    <div id="dlg_parent_user" class="cjlsoft-dialog-panel">
        <input type="hidden" id="u12" name="u012" value="0" />
        <ul id="parent_tree" class="easyui-tree" data-options="lines:true,animate:false">
        </ul>
    </div>

    <script type="text/javascript">
        var defaultType = "";
        function check_account(accout)
        {
            var bool = false;
            $.ajax({
                async: false, cache: false, type: "POST", timeout: 10, url: "/AM/MemberAgent?method=checkAgentAccount", dataType: "json",
                data: { u002: accout },
                beforeSend: function (a)
                {
                    cjlsoft_mask_panel();
                },
                success: function (a, b)
                {
                    _check_auth(a.Code);
                    $("#span_account").html(a.Message);
                    if (0 == a.Code)
                    {
                    }
                    else if (1 == a.Code)
                    {
                        bool = true;
                    }
                },
                complete: function (a, b)
                {
                    cjlsoft_mask_panel_close();
                }
            });
            return bool;
        }

        $(document).ready(function ()
        {
            defaultType = $(".parent_pd_data").html();
            $('#dlg_parent_user').dialog(
            {
                title: '上级选择',
                width: 390,
                height: 390,
                closed: true,
                cache: false,
                modal: true
            });

            $("#btn_select_parent").click(function ()
            {
                $('#dlg_parent_user').dialog('open','center');
                $("#parent_tree").tree({ url: "/AM/MemberAgent?method=getAGUIDs" });
            });

            $('#parent_tree').tree({
                onClick: function (node)
                {
                    $("#span_select_parent").html(node.text);
                    $("#u012").val(node.id);
                    $("#pd_info").html("");
                    if (node.id == 0)
                    {
                        $(".parent_pd_data").html(defaultType);
                        $(".tr_root_parent").show();
                        $(".tr_parent").hide();
                    }
                    else
                    {
                        $(".parent_pd_data").html("");
                        $(".tr_parent").show();
                        $(".tr_root_parent").hide();
                    }
                    if (node.id > 0)
                    {
                        $(".tr_ct").hide();
                        /*找出上级反点*/
                        $.ajax({
                            async: false, cache: false, type: "POST", timeout: 10, url: "/AM/MemberAgent?method=getParentPoint",data:{parentID:node.id},dataType: "json",
                            beforeSend: function (a)
                            {
                                cjlsoft_mask_panel();
                            },
                            success: function (a, b)
                            {
                                _check_auth(a.Code);
                                for (var i = 0; i < a.Data.length; i++)
                                {
                                    var item = a.Data[i];
                                    var row = "<tr>";
                                    row += "<td class='w100px'>"
                                        + item.GameClassName + "-"+ item.ShowPointName
                                        + "<input type='hidden' name='[" + i + "].gp001' value='"+ item.GameClassPrizeID + "' />"
                                        + "<input type='hidden' name='[" + i + "].gc001' value='" + item.GameClassID + "' />" + "</td>";
                                    row += "<td>" + "系统定位返点"
                                        + item.SystemPoint
                                        + "%,不定位返点"
                                        + item.SystemPointX + "<br />可分配定位返点" +
                                        item.CurrentHavePoint + "%不定位返点"
                                        + item.CurrentHavePointX + "<br />赚下级定位返点<input name='[" + i + "].up002' type='text' class='input-text w50px' value='0.0000'>%，不定位返点<input name='[" + i + "].up004' type='text' class='input-text w50px' value='0.0000'>%<br />" + "</td>";
                                    row += "</tr>";
                                    $("#pd_info").append(row);
                                }
                            },
                            complete: function (a, b)
                            {
                                cjlsoft_mask_panel_close();
                            }
                        });
                    }
                    else
                    {
                        $(".tr_ct").show();
                    }
                    //$.ajax({ async: false, cache: false, type: "POST", data: { id: node.id }, timeout: 10, url: "/AM/MemberAgent?method=getAGUIDs", dataType: "json" });
                }
            });
            $("#set_default_0").click(function ()
            {
                $("#span_select_parent").html("顶级");
                $("#u012").val(0);
            });
            $("#set_default_0").click();
        });
    </script>
</asp:Content>