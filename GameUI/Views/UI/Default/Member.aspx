<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var methodType = (string)ViewData["MethodType"];
    %>
    <div class="ui-page-content-body-header tools">
        <div class="left-nav">
            <a href="/UI/Member?method=accountList" title="账号管理" <%=methodType == "accountList" ? "class='item-select'" : "" %>>账号管理</a>
            <a href="/UI/Member?method=createAccount" title="增加账号" <%=methodType == "createAccount" ? "class='item-select'" : "" %>>增加账号</a>
            <a href="/UI/Member?method=reportTotalMoney" title="团队余额" <%=methodType == "reportTotalMoney" ? "class='item-select'" : "" %>>团队余额</a>
            <!--<a href="/UI/Member?method=accountAuto" title="推广链接" <%=methodType == "accountAuto" ? "class='item-select'" : "" %>>推广链接</a>-->
        </div>
    </div>
    <div class="blank-line"></div>
    <%if ("accountList" == methodType)
      {
          %>
    <%
          List<DBModel.UserPack> userPackList = (List<DBModel.UserPack>)ViewData["UserPackList"];
          var userStatus = (int)ViewData["UserStatus"];
          var amtT = (int)ViewData["AmtT"];
          var amtV = (int)ViewData["AmtV"];
          var pntT = (int)ViewData["PntT"];
          var pntV = (int)ViewData["PntV"];
          var posList = (List<DBModel.SysPositionLevel>)ViewData["PosList"];
          var posDicList = posList.ToDictionary(exp => exp.Level);
          var vipDicList = (Dictionary<int,string>)ViewData["AGLevelNameList"]; 
    %>
    <div class="block_tools">
    <form action="/UI/Member" id="form_account_list" method="get">
        账号
        <input type="text" class="input-text w80px" name="userName" value="<%:ViewData["UserName"] %>" />
        注册时间
        <input type="text" class="input-text w80px" id="regDTS" name="regDTS" value="<%:ViewData["RDTS"] %>" />-<input type="text" class="input-text w80px" id="regDTE" name="regDTE" value="<%:ViewData["RDTE"] %>" />
        登录时间
        <input type="text" class="input-text w80px" id="loginDTS" name="loginDTS" value="<%:ViewData["LDTS"] %>" />-<input type="text" class="input-text w80px" id="loginDTE" name="loginDTE" value="<%:ViewData["LDTE"] %>" />
        <div class="blank-line"></div>
        状态
        <select name="userStatus">
            <option value="-1">所有</option>
            <option value="0" <%=userStatus==0? "selected='selected'" : "" %>>停用</option>
            <option value="1" <%=userStatus==1? "selected='selected'" : "" %>>正常</option>
            <option value="2" <%=userStatus==2? "selected='selected'" : "" %>>暂停</option>
            <option value="3" <%=userStatus==3? "selected='selected'" : "" %>>冻结</option>
        </select>
        金额
        <select name="amountType">
            <option value="0" <%=amtT==0? "selected='selected'" : "" %>>所有</option>
            <option value="1" <%=amtT==1? "selected='selected'" : "" %>>等于</option>
            <option value="2" <%=amtT==2? "selected='selected'" : "" %>>小于</option>
            <option value="3" <%=amtT==3? "selected='selected'" : "" %>>大于</option>
        </select>
        <input type="text" class="input-text w50px" name="amountTypeV" value="<%:amtV %>" />
        <!--积分
        <select name="pointType">
            <option value="0" <%=pntT==0? "selected='selected'" : "" %>>所有</option>
            <option value="1" <%=pntT==1? "selected='selected'" : "" %>>等于</option>
            <option value="2" <%=pntT==2? "selected='selected'" : "" %>>小于</option>
            <option value="3" <%=pntT==3? "selected='selected'" : "" %>>大于</option>
        </select>-->
        <input type="text" class="input-text w50px" name="pointTypeV" value="<%:pntV %>" />
        <input type="submit" class="btn-normal ui-post-loading" value="查找" />
        <input type="button" id="reset_form" class="btn-normal" value="重置" />
        <input type="button" class="btn-normal ui-post-loading ui-page-back" value="返回" />
    </form>
    </div>
    <div class="blank-line"></div>
    <table class="table-pro tp5 table-color-row" width="100%">
        <thead>
            <tr class="table-pro-head">
                <th>账号</th>
                <th>下级数</th>
                <th>可用余额</th>
                <th>冻结金额</th>
                <!--积分<th>积分</th>-->
                <th>注册时间</th>
                <th>登录时间/地址</th>
                <th>状态</th>
                <!--<th>VIP</th>
                <th>军衔</th>-->
                <th>分红比</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            <%if( null != userPackList)
              { 
                  foreach(var item in userPackList)
                  { 
                  %>
            <tr>
                <td><%:item.User.u002.Trim() %></td>
                <td><%:item.ChildCount %></td>
                <td class="fc-red"><%:item.User.wgs014.uf001.ToString("N2") %></td>
                <td><%:item.User.wgs014.uf003.ToString("N2") %></td>
                <!--<td><%:item.User.wgs014.uf004.ToString("N2") %></td>-->
                <td><%:item.User.u005 %></td>
                <td><%:item.User.u007 %>/<%:item.User.u022 %></td>
                <td>
                    <%
                      var status = string.Empty;
                      switch (item.User.u008)
                      {
                          case 0:
                              status = "<span class='fc-red'>停用</span>";
                              break;
                          case 1:
                              status = "<span class='fc-green'>正常</span>";
                              break;
                          case 2:
                              status = "<span class='fc-blue'>暂停</span>";
                              break;
                          case 3:
                              status = "<span class='fc-gray'>冻洁</span>";
                              break; 
                      }
                    %>
                    <%=status %>
                </td>
                <!--<td><%=vipDicList[item.User.u015] %></td>
                <td><%=posDicList[item.User.u013].Name %></td>-->
                <td><%=item.User.u024 * 100 %>%</td>
                <td class="link-tools"><a href="javascript:void(0);" name="edit_user" data="<%:item.User.u001 %>" title="<%:item.User.u002.Trim() %>">修改</a><%if( item.ChildCount > 0){ %><a href="/UI/Member?method=accountList&parentID=<%:item.User.u001 %>" title="查看下级">下级</a><%} %></td>
            </tr>
            <%
            }/*foreach*/
            }/*if*/ %>
        </tbody>
            <tfoot class="dom-hide">
                <tr>
                    <td colspan="10"></td>
                </tr>
            </tfoot>
    </table>
    <%=ViewData["PageList"] %>
    <div id="edit_user_dlg" class="dom-hide">
        <form action="#" method="post" id="form_edit_user">
            <%:Html.AntiForgeryToken() %>
            <input type="hidden" id="edit_key" name="edit_key" value="" />
        <table class="table-pro table-noborder g_nco tp5" width="100%">
            <tr>
                <td class="title">账号</td>
                <td id="ui_acct"></td>
            </tr>
            <tr>
                <td class="title">昵称</td>
                <td><input name="nickname" id="ui_nkn" type="text" class="input-text w150px" value="" /></td>
            </tr>
            <tr class="dom-hide">
                <td class="title">状态</td>
                <td>
                    <select id="status" name="status">
                        <option value="0">停用</option>
                        <option value="1">正常</option>
                        <option value="2">暂停</option>
                        <option value="3">冻结</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="title">注册时间</td>
                <td id="ui_rd"></td>
            </tr>
            <tr class="dom-hide">
                <td class="title">登录时间</td>
                <td id="ui_ld"></td>
            </tr>
            <tr class="dom-hide">
                <td class="title">登录地址</td>
                <td id="ui_ip"></td>
            </tr>
            <tr>
                <td class="title">分红</td>
                <td><input id="ui_red" name="red_percent" type="text" class="input-text w50px" value=""  readonly="readonly" />%<font color="'red'">(分红比例需由管理员修改)</font></td>
            </tr>
            <tr class="dom-hide">
                <td class="title">开下级</td>
                <td>
                    <select id="can_child" name="can_child">
                        <option value="0">不允许</option>
                        <option value="1">允许</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="title">返点</td>
                <td>
                    <table class="table-pro table-noborder w100ps">
                        <thead>
                            <tr class="table-pro-head">
                                <th>游戏</th>
                                <th>返点</th>
                            </tr>
                        </thead>
                        <tbody id="point_list">
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="title"></td>
                <td><input type="button" value="保存" class="btn-normal btn_update_user" /></td>
            </tr>
        </table>
        </form>
    </div>
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $("a[name='edit_user']").click(function ()
            {
                var key = $(this).attr("data");
                var edit_name = $(this).attr("title");
                $.ajax({
                    async: false, timeout: _global_ajax_timeout, type: "POST", cache: false, url: "/UI/Member?method=editUser", data: { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(), key: key },
                    success: function (a, b) {
                        _check_auth(a);
                        $("#edit_user_dlg").dialog({ width: 560, height: 500, modal: true, title: "修改账号-" + edit_name, resizable: false, position: { my: "center", at: "center", of: window } });
                        var _robj = eval(a);
                        $("#edit_key").val(_robj.Data.UpdateKey);
                        $("#ui_acct").html(_robj.Data.UserName);
                        $("#ui_nkn").val(_robj.Data.UserNickname);
                        $("#ui_rd").html(_robj.Data.RegDate);
                        $("#ui_ld").html(_robj.Data.LoginDate);
                        $("#ui_ip").html(_robj.Data.LoginIP);
                        $("#status option[value='" + _robj.Data.UserState.toString() + "']").prop("selected", true);
                        $("#can_child option[value='" + _robj.Data.CanCreate.toString() + "']").prop("selected", true);
                        $("#ui_red").val(_robj.Data.RedPercent);
                        $("#point_list").empty();
                        for (var i = 0; i < _robj.Data.PDL.length; i++)
                        {
                            $("#point_list").append('<tr><td class="title">' + '<input name="pid" type="hidden" value="' + _robj.Data.PDL[i].PID + '" />' + _robj.Data.PDL[i].GameClassName + "</td><td>" + "现返点：" + _robj.Data.PDL[i].Point + '<span>，返点：<input name="point' + _robj.Data.PDL[i].PID + '" type="text" class="input-text w30px" value="' + _robj.Data.PDL[i].Point + '" />' + "</span>>=" + _robj.Data.PDL[i].MaxPoint + "</td></tr>");
                        }
                    },
                    complete: function (a, b) {
                    }
                });
            });
            $(".btn_update_user").click(function ()
            {
                var form_data = $("#form_edit_user").serialize();
                $.ajax({
                    async: false, timeout: _global_ajax_timeout, type: "POST", cache: false, url: "/UI/Member?method=updateUser", data: form_data,
                    success: function (a, b)
                    {
                        _check_auth(a);
                        var _robj = eval(a);
                        if (0 == _robj.Code)
                        {
                            alert(_robj.Message);
                        }
                        else
                        {
                            refresh_current_page();
                        }
                    }
                });
            });
        });
        $("#reset_form").click(
            function ()
            {
                $("#regDTS,#regDTE,#loginDTS,#loginDTE,input[name='amountType'],input[name='amountTypeV'],input[name='pointType'],input[name='pointTypeV'],input[name='userName']").val("");
        });
    </script>
    <%}/*accountList*/ %>
    <%if ("reportTotalMoney" == methodType)
      {
          %>
    <%
          var tAmt = (decimal)ViewData["TAmt"];
          var tPnt = (decimal)ViewData["TPnt"];
          var tHon = (decimal)ViewData["THon"];
          var self = (int)ViewData["Self"];
          
    %>
    <form action="/UI/Member" method="get">
        <input type="hidden" name="method" value="reportTotalMoney" />
        <table class="table-pro tp5" width="100%">
            <tr>
                <td class="title">团队余额</td>
                <td><%:tAmt.ToString("N2") %></td>
            </tr>
            <!--<tr>
                <td class="title">团队积分</td>
                <td><%:tPnt.ToString("N2") %></td>
            </tr>-->
            <tr>
                <td class="title">团队冻结</td>
                <td><%:tHon.ToString("N2") %></td>
            </tr>
            <tr>
                <td class="title">自己</td>
                <td>
                    <select name="self">
                        <option value="1" <%=self == 1 ? "selected='selected'" : "" %>>包含自己</option>
                        <option value="2" <%=self == 2 ? "selected='selected'" : "" %>>不包含自己</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="title"></td>
                <td><input type="submit" value="读取" class="btn-normal ui-post-loading" /></td>
            </tr>
        </table>
    </form>
    <%}/*reportTotalMoney*/ %>
    <%else if ("accountAuto" == methodType)
      { %>
    <%
          var autoRegList = (List<DBModel.wgs035>)ViewData["AutoRegList"];
          var myPDList = (List<DBModel.wgs017>)ViewData["MyPDList"];
          //var gList = (List<DBModel.wgs001>)ViewData["GList"];
          //var gmList = (List<DBModel.wgs002>)ViewData["GMList"];
          var gcList = (List<DBModel.wgs006>)ViewData["GCList"];
          var gcDicList = gcList.ToDictionary(exp => exp.gc001);
         %>
    <form action="/UI/Member?method=accountAuto" method="post" id="form_url">
    <%:Html.AntiForgeryToken() %>
    <input type="hidden" name="ar001" value="" />
    <input type="hidden" name="ar002" value="" />
    <input type="hidden" name="ar003" value="" />
    <input type="hidden" name="ar004" value="" />
    <table class="table-pro tp5 g_nco" width="100%">
        <tr>
            <td class="title"></td>
            <td class="fc-red" id="error_message"></td>
        </tr>
        <tr>
            <td class="title">允许开下级</td>
            <td>
                <select name="allowcreateaccount">
                    <option value="0" title="不允许">不允许</option>
                    <option value="1" title="允许" selected="selected">允许</option>
                </select>
            </td>
        </tr>
        <tr>
            <td class="title">返点</td>
            <td>
                <table class="table-pro g_nco">
                    <%foreach(var mygp in myPDList){ %>
                    <tr>
                        <td class="title"><%:gcDicList[mygp.gc001].gc003 %></td>
                        <td class="w100px"><input type="hidden" name="pointid" value="<%:mygp.up001 %>" />我的返点<span class="fc-red"><%:mygp.up003.ToString("N1") %></span></td>
                        <td class="w150px">保留返点<input name="point" type="text" class="input-text w50px" value="0" /></td>
                    </tr>
                    <%} %>
                </table>
            </td>
        </tr>
        <tr>
            <td class="title"></td>
            <td><%=ViewData["CreateMax"] %></td>
        </tr>
        <tr>
            <td class="title"></td>
            <td><input type="button" id="createurl" class="btn-normal" value="生成推广链接" /></td>
        </tr>
    </table>
    </form>
    <script type="text/javascript">
        $("#createurl").click(function ()
        {
            var form_data = $("#form_url").serialize();
            $.ajax({
                async: false, cache: false, type: "POST", timeout: _global_ajax_timeout, url: "/UI/Member?method=accountAuto", data: form_data,
                success: function (a, b) {
                    _check_auth(a);
                    var _robj = eval(a);
                    if (0 == _robj.Code)
                    {
                        $("#error_message").html(_robj.Message); ui_mask_panel_close();
                    }
                    else if (1 == _robj.Code)
                    {
                        refresh_current_page();
                    }
                },
                complete: function () {
                }
            });
        });
    </script>
    <div class="blank-line"></div>
    <table class="table-pro table-color-row tp5 g_nco" width="100%">
        <thead>
            <tr class="table-pro-head">
                <th>推广地址</th>
                <th>返点信息</th>
                <th>已注册人数</th>
                <th>允许开下级</th>
                <th>创建日期</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            <%if( null != autoRegList)
              {
                  foreach(var item in autoRegList)
                  {
                      var url = Request.Url.Host + (Request.Url.Port == 80 ? ":8081" : ":" + Request.Url.Port.ToString());
                      url = (string)ViewData["AutoRegURL"];
                  %>
            <tr>
                <td>
                    <a target="_blank" href="<%:"http://"+ url +"/Register.html?code="+item.ar002 %>" title="推广注册链接"><%:"http://"+url  +"/Register.html?code="+item.ar002 %></a></td>
                <td>
                    <table class="table-pro">
                        <%
                      var pointInfo = item.ar003.Split(',');
                      foreach(var pi in pointInfo)
                      { 
                          var p = pi.Split('|');
                        %>
                        <tr>
                            <td class="title"><%:gcDicList[int.Parse(p[0])].gc003 %></td>
                            <td class="w150px">抽取返点<%:p[1] %></td>
                        </tr>
                        <%} %>
                    </table>
                </td>
                <td><%:item.ar008 %></td>
                <td><%=item.ar009 == 0 ? "<span class='fc-red'>不允许</span>" : "<span class='fc-green'>允许</span>" %></td>
                <td><%:item.ar004 %></td>
                <td><a href="/UI/Member?method=accountAuto&key=<%:item.ar001 %>">删除</a></td>
            </tr>
            <%}/*for*/
              }/*foreach*/ %>
        </tbody>
    </table>
    <%} /*accountAuto*/%>
    <%else if ("createAccount" == methodType)
      { %>
    <%
          var myPDList = (List<DBModel.wgs017>)ViewData["MyPDList"];
          var gcList = (List<DBModel.wgs006>)ViewData["GCList"];
          var gcDicList = gcList.ToDictionary(exp => exp.gc001);
    %>
    <form action="#" method="post" id="form_register">
    <%:Html.AntiForgeryToken() %>
    <table class="table-pro w100ps tp5">
        <tr>
            <td class="title"></td>
            <td class="fc-red" id="error_message"></td>
        </tr>
        <tr>
            <td class="title">账号</td>
            <td><input type="text" class="input-text w250px" name="username" /></td>
        </tr>
        <tr>
            <td class="title">昵称</td>
            <td><input type="text" class="input-text w250px" name="nickname" /></td>
        </tr>
        <tr>
            <td class="title">密码</td>
            <td><input type="text" class="input-text w250px" name="password" /></td>
        </tr>
        <tr>
            <td class="title">允许开下级</td>
            <td>
                <select name="allowcreateaccount">
                    <option value="0" title="不允许">不允许</option>
                    <option value="1" title="允许" selected="selected">允许</option>
                </select>
            </td>
        </tr>
        <tr>
            <td class="title">返点</td>
            <td>
                <table class="table-pro">
                    <%foreach(var mygp in myPDList){ %>
                    <tr>
                        <td class="title"><%:gcDicList[mygp.gc001].gc003 %></td>
                        <td class="w100px"><input type="hidden" name="pointid" value="<%:mygp.up001 %>" />我的返点<span class="fc-red"><%:mygp.up003.ToString("N1") %></span></td>
                        <td class="w150px">注册返点<input name="point" type="text" class="input-text w50px" value="<%:(mygp.up003-0.1m).ToString("N1") %>" /></td>
                    </tr>
                    <%} %>
                </table>
            </td>
        </tr>
        <tr>
            <td class="title"></td>
            <td><input id="btn_register" type="button" class="btn-normal ui-post-loading" value="注册" /></td>
        </tr>
    </table>
    </form>
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $("#btn_register").click(function () {
                var form_data = $("#form_register").serialize();
                $.ajax({
                    async: false, cache: false, type: "POST", timeout: _global_ajax_timeout, url: "/UI/Member?method=createAccount", data: form_data,
                    success: function (a, b) {
                        _check_auth(a);
                        var _robj = eval(a);
                        if (0 == _robj.Code) {
                            $("#error_message").html(_robj.Message);
                        }
                        else if (1 == _robj.Code) {
                            $("#error_message").removeClass("fc-red");
                            $("#error_message").html("<p>账号：" + $("input[name='username']").val() + "</p><p>密码：" + $("input[name='password']").val() + "</p>");
                            $("input[name='username'],input[name='password'],input[name='nickname']").val("");
                        }
                    },
                    complete: function () {
                        ui_mask_panel_close();
                    }
                });
            });
        });
    </script>
    <%
    }/*accountAuto*/
        %>
</asp:Content>