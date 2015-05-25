<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="cjlsoft-body-header">
        <h1>游戏期数</h1>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>
</div>
    <div class="blank-line"></div>
    <form action="/AM/GameSession" method="post">
        <%:Html.AntiForgeryToken()%>
    <input type="hidden" name="gs001" value="0" />
    <input type="hidden" name="g001" value="<%:ViewData["GameID"] %>" />
    <input type="hidden" name="method" value="add" />
    <table class="table-pro" width="100%">
        <tbody>
            <tr>
                <td class="title">序列式</td><td>
                 <select name="start_type">
                     <option value="0">日期式</option>
                     <option value="1">序号式</option>
                 </select>
                    <span class="tips">序列式时期数日期将忽略</span></td>
            </tr>
            <tr>
                <td class="title">序列号</td><td><input type="text" name="ser_no" value="0" class="input-text w300px" /></td>
            </tr>
            <tr>
                <td class="title">期数日期</td><td><input type="text" name="start_date" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">开始期号</td><td><input type="text" name="start_no" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">期号数量</td><td><input type="text" name="end_no" class="input-text w300px" value="" /></td>
            </tr>
            <tr>
                <td class="title">开盘时间</td><td><input type="text" name="start_time" class="input-text w300px" value="" />下一期偏移<input type="text" name="start_time2" class="input-text w50px" value="" />秒</td>
            </tr>
            <tr>
                <td class="title">封盘时间</td><td><input type="text" name="close_time" class="input-text w300px" value="" />下一期偏移<input type="text" name="close_time2" class="input-text w50px" value="" />秒</td>
            </tr>
            <tr>
                <td class="title">开奖时间</td><td><input type="text" name="open_time" class="input-text w300px" value="" />下一期偏移<input type="text" name="open_time2" class="input-text w50px" value="" />秒</td>
            </tr>
            <tr>
                <td class="title"></td><td><input type="submit" value="保存" class="cjlsoft-post-loading btn-normal" /></td>
            </tr>
        </tbody>
    </table>
    </form>
</asp:Content>
