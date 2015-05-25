<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<List<KeyValuePair<string, object>>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">系统缓存</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="cjlsoft-body-header">
        <h1>系统缓存</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx" ); %>
    </div>
    <div class="blank-line"></div>
    <div class="cjlsoft-tips-block">
        <p><span>CacheMemoryLimit</span><%:ViewData["CacheMemoryLimit"] %></p>
        <p><span>PhysicalMemoryLimit</span><%:ViewData["PhysicalMemoryLimit"] %>%</p>
        <p><span>PollingInterval</span><%:ViewData["PollingInterval"] %></p>
        <p><span>Name</span><%:ViewData["Name"] %></p>
    </div>
    <div class="blank-line"></div>
    <%
            using (Html.BeginForm("SystemCache", "AM", FormMethod.Post, new { id = "form-game-method-data-list" }))
            { 
              /*BegionForm*/%>
        <input type="hidden" name="method" value="UpdateList" />
        <%:Html.AntiForgeryToken()%>
    <table width="100%" class="table-pro table-color-row">
        <thead>
            <tr class="table-pro-head">
                <th>Key</th>
                <th>Value</th>
            </tr>
        </thead>
        <tbody>
            <%if( Model != null){
                foreach(var item in Model){ %>
            <tr key="<%:item.Key %>">
                <td><%:item.Key %></td>
                <td><%:item.Value %></td>
            </tr>
            <%}/*foreach*/ }/*if*/ %>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="7" class="text_left">
                    <a href="javascript:;" id="a-table-select">全选</a><a href="javascript:;" id="a-table-unselect">反选</a><a href="javascript:;" id="a-table-clear-select">取消</a>
                </td>
            </tr>
        </tfoot>
    </table>
        <div id="cjlsoft-bottom-function">
        <input id="btn-delete" type="button" value="删除" class="btn-normal" />
    </div>
    <% }%>

    <script type="text/javascript">
        $("#btn-delete").click(function () {
            if (confirm("是否删除选定记录？") == 0) {
                return;
            }
            var keys = get_table_row_keys();
            if (keys.length > 0) {
                $.ajax({
                    async: true, url: "<%:Url.Action("SystemCache", "AM")%>", type: "POST", data: { "__RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val(), "keys": keys, "method": "DeleteList" }, dataType: "JSON", beforeSend: function (XMLHttpRequest) {
                        cjlsoft_mask_panel();
                    }, success: function (result) {
                        if (0 == result) {
                            alert(result.Message);
                        }
                    },
                    complete: function (XMLHttpRequest, textStatus) {
                        refresh_current_page();
                    }
                });
            }
        });
    </script>
</asp:Content>