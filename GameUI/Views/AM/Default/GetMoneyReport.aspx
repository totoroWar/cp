<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
    var type = (int)ViewData["Type"];
    string acct = (string)ViewData["acct"];
    int om = (int)ViewData["om"];
    decimal omm = (decimal)ViewData["omm"];
    
    %>
<div class="xtool">
        <form method="get" action="/AM/GetMoneyReport">
            <input value="reportChildDetails" name="method" type="hidden" />
        <span>
            周期范围 <input id="i_dts" name="dts" type="text" class="input-text w120px" value="<%:ViewData["DTS"] %>" />
            <input id="i_dte" name="dte" type="text" class="input-text w120px" value="<%:ViewData["DTE"] %>" />
       </span>
        类型
        <select name="type">
            <%--<option class="dom-hide" value="0" <%=type == 0 ? "selected='selected'" : "" %>>直接下级（包含直接下级）</option>
            <option class="dom-hide" value="1" <%=type == 1 ? "selected='selected'" : "" %>>直接下级（不包含直接下级）</option>
            <option class="dom-hide" value="2" <%=type == 2 ? "selected='selected'" : "" %>>所有下级（包含直接下级）</option>
            <option class="dom-hide" value="3" <%=type == 3 ? "selected='selected'" : "" %>>所有下级（不包含直接下级）</option>--%>
            <option value="-1" <%=type == 0 ? "selected='selected'" : "" %>>全部</option>
            <option value="1" <%=type == 1 ? "selected='selected'" : "" %>>已通过</option>
            <option value="2" <%=type == 0 ? "selected='selected'" : "" %>>已取消</option>
            <option value="0" <%=type == 0 ? "selected='selected'" : "" %>>待审核</option>
        </select>

        <div class="blank-line"></div>
        账号<input type="text" class="input-text w80px" name="acct" value="<%:acct %>" />
            
        分红金额
        <select name="om" id="om">
            <option value="0" <%:om == 0 ? "selected='selected'" : "" %>>所有</option>
            <option value="1" <%:om == 1 ? "selected='selected'" : "" %>>小于</option>
            <option value="2" <%:om == 2 ? "selected='selected'" : "" %>>等于</option>
            <option value="3" <%:om == 3 ? "selected='selected'" : "" %>>大于</option>
        </select>
            <input type="text" class="input-text w50px" name="omm" value="<%:omm %>" />
        <input type="submit" class="btn-normal" value="查询" />
            <input type="button" class="btn-normal cjlsoft-a-back" value="返回" />

        </form>

    <div class="blank-line"></div>

    <div id="parent_tree" class="dom-hide" data-options="lines:true,animate:false"></div>
    <div class="blank-line"></div>
    
    <table width="100%" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th>分红账号</th>
                    <th>分红比例</th>
                    <th>分红周期</th>
                    <th>周期中奖额</th>
                    <th>周期返点额</th>
                    <th>周期投注额</th>
                    <th>累计盈亏</th>
                    <th>本期盈亏</th>
                    <th>可分红盈亏</th>
                    <th>分红金额</th>
                    <th>周期开始时间</th>
                    <th>周期结束时间</th>
                    <th>状态</th>
                    <th>操作时间</th>
                    <th>理由</th>
                    
                </tr>
            </thead>
            <tbody id="addrows">
            <%=ViewData["FHLog"] %>
        </tbody>


        </table>
        
     
    <div class="blank-line"></div>
    </div>

    <%=ViewData["PageList"] %>
    <script type="text/javascript">
        jQuery('#i_dts').datetimepicker({
            format: 'Y/m/d H:i:s',
            lang: "ch",
            timepicker: true
        });
        jQuery('#i_dte').datetimepicker({
            format: 'Y/m/d H:i:s',
            lang: "ch",
            timepicker: true
        });
    </script>
</asp:Content>


