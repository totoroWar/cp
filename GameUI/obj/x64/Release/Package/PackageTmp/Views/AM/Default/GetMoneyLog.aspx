<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

       <div class="cjlsoft-body-header">
    <h1>分红记录</h1>
</div>
<div class="blank-line"></div>

    <div id="parent_tree" class="dom-hide" data-options="lines:true,animate:false"></div>
    <div class="blank-line"></div>
    <form action="/AM/GetMoneySet" method="get">
    <table width="100%" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th>分红账号</th>
                    <th>分红比例</th>
                    <th>分红周期</th>
                    <th>周期盈亏</th>
                    <th>分红金额</th>
                    <th>操作时间</th>
                    <th>状态</th>
                    <th>理由</th>
                    
                </tr>
            </thead>
            <tbody id="addrows">
            <%=ViewData["FHLog"] %>
        </tbody>


        </table>
        <%=ViewData["PageList"] %>
     </form>
    <div class="blank-line"></div>
</asp:Content>

