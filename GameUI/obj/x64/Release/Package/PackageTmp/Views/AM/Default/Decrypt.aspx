<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form action="/AM/Menu" method="post">
        <%:Html.AntiForgeryToken()%>
    <p>Key</p>
    <p><input type="text" name="key" class="input-text w300px fs16px" value="" /></p>
    <p>Content</p>
    <p><textarea id="content" name="content" rows="10" cols="10" class="input-text w300px fs16px"></textarea></p>
    <p>Result</p>
    <p><textarea id="result" name="result" rows="10" cols="10" class="input-text w300px fs16px"></textarea></p>
    <p><input type="button" id="btn_get_result" value="Decrypt" /></p>
    </form>
    <script type="text/javascript">
        $("#btn_get_result").click(function ()
        {
            var form = $(this).parents("form");
            var form_data = $(this).parents("form").serialize();
            $.ajax({
                async: false, timeout: 5, type: "POST", url: "/AM/Decrypt", data: form_data, dataType: "json",
                success: function (a, b)
                {
                    _check_auth(a.Code);
                    if (0 == a.Code)
                    {
                        $.messager.alert('提示', a.Message, 'question');
                    }
                    else if (1 == a.Code)
                    {
                        $("#result").text(a.Data);
                        cjlsoft_mask_panel_close();
                    }
                },
                complete: function (a, b)
                {
                    cjlsoft_mask_panel_close();
                }
            });
        });
    </script>
</asp:Content>
