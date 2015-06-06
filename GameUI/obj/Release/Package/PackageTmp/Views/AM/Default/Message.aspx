<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DBModel.wgs004>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="/Scripts/ueditor/ueditor.config.js"></script>
    <script type="text/javascript" src="/Scripts/ueditor/ueditor.all.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var ue = UE.getEditor('content');
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cjlsoft-body-header">
        <h1>消息</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
    </div>
    <div class="blank-line"></div>
    <form action="#" method="post" id="form_message">
        <table class="table-pro w100ps">
            <tr>
                <td class="title">标题</td>
                <td>
                    <input type="text" name="title" class="input-text w200px" /></td>
            </tr>
            <tr>
                <td class="title">时间</td>
                <td>
                    <input type="text" name="dt" class="input-text w200px" value="<%=DateTime.Now.ToString() %>" /></td>
            </tr>
            <tr>
                <td class="title">账号</td>
                <td>
                    <textarea rows="5" name="users" cols="100" class="input-text"></textarea>
                    <div class="blank-line"></div>
                    <label for="t1">
                        <input id="t1" name="type" type="radio" value="1" checked="checked" />指定用户</label>
                    <label for="t2">
                        <input id="t2" name="type" type="radio" value="2" />指定用户及其下级</label>
                    <label for="t3">
                        <input id="t3" name="type" type="radio" value="3" />所有用户</label>
                </td>
            </tr>
            <tr>
                <td class="title">内容</td>
                <td>
                    <script id="content" name="content" type="text/plain">
                    </script>
                </td>
            </tr>
            <tr>
                <td class="title"></td>
                <td>
                    <input type="button" id="btn_send" value="发送" class="btn-normal" /></td>
            </tr>
        </table>
    </form>
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $("#btn_send").click(function ()
            {
                var form_data = $("#form_message").serialize();
                $.ajax({
                    timeout: _global_ajax_timeout, dataType: "text", cache: false, type: "POST", data: form_data, url: "/AM/Message?method=send", success: function (a) {
                        _check_auth(a);
                        eval("var _robj =" + a + ";");
                        if (0 == _robj.Code) {
                            alert(_robj.Message);
                        }
                        else if (1 == _robj.Code) {
                            alert("发送成功");
                        }
                    }
                });
            });
        });
    </script>
</asp:Content>
