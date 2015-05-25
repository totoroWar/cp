<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI2/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="m_body_bg">
<div class="main_box main_tbg">
<div class="main_table_bg">
<div class="main_table_box">
    <div class="user_info_tab hd">
        <ul>
            <span><a class="info_close" href="#"></a></span>
            <li class="on"><a href="/UI2/shop">平台商店</a></li>
            <%foreach (var item in (List<DBModel.wgs032>)ViewData["shopClassList"])
            {%>
            <li>
                <a href="/UI2/Shop?classId=<%:item.rc001 %>" title="<%:item.rc002 %>"><%:item.rc002 %></a>
            </li>
            <%}%>
            <li class="on"><a href="/ui2/shopReport" title="订单管理">订单管理</a></li>
                        
        </ul>
    </div>
<form action="/UI/Charge" method="post" id="form_charge">
<%:Html.AntiForgeryToken() %>
<input type="hidden" name="target" value="check" />
<div class="bank_list">
    <table class="ctable_box ctable_box1" width="100%" border="0" cellpadding="0" cellspacing="0">
        <tbody >
            <th >订单编号</th>
           <th>物品编号</th>
           <th>物品标题</th>
           <th>单价</th>          
           <th>购买数量</th>
           <th>总价（积分）</th>
           <th>收件人</th>
        <th>电话</th>
        <th>地址</th>
        <th>邮编</th>
        <th>原价</th>
        <th>物流公司</th>
         <th>邮件查询网址</th>
        <th>快递单号</th>
        <th>折扣</th>
           <th>状态</th>
        
           <th>备注</th>
            <%foreach (var item in (List<DBModel.wgs039>)ViewData["shopRecordList"])
             {%>
           <tr class="table-color-row-even">
           <td><%:item.sr001 %></td>
          
           <td><%:item.r001 %></td>
           <td><%:item.sr009 %></td>
           <td><%:item.sr008 %></td>
           <td><%:item.sr002 %></td>
           <td><%:item.sr003 %></td>
               <td><%:item.sr011 %></td>
               <td><%:item.sr012 %></td>
               <td><%:item.sr010 %></td>
               <td><%:item.sr013 %></td>
                   <td><%:item.sr014 %></td>
        <td><%:item.sr015 %></td>
         <td><%:item.sr016 %></td>
        <td><%:item.sr017 %></td>
        <td><%:item.sr018 %></td>
           <td><%:item.sr004==0?"未处理":item.sr004==1?"已完成":"已撤销" %></td>
           <td><%:item.sr005 %></td>
           
           </tr>
            <%} %>
            
    </tbody></table>
    <%-- <%= ViewData["PageList"] %>--%>
</div>
</form>

</div></div></div></div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
