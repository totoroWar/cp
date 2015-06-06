<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    ShopReport
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     <div class="ui-page-content-body-header tools">
        <div class="left-nav">
            <a href="/UI/shop" >商城</a>
            <a href="/UI/shopReport" >订单</a>
          
        </div>
    </div>
    <div class="blank-line"></div>
    <table class="table-pro table-color-row" width="100%">
    <thead >       <tr class="table-pro-head"><th >订单编号</th>
          
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
      
       </tr>
          </thead>
       <tbody>
   
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
       </tbody>
   </table>
    <%= ViewData["PageList"] %>
  
</asp:Content>

