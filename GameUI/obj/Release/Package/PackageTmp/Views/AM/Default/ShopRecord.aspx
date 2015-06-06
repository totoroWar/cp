<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
   <script type="text/javascript">
       var dialog;
       function EditRecord(id)
       {
           dialog.dialog("open");
           $("#recordId").val(id);
           $("#recordId2").html(id);
       }
       $(function () {
           dialog= $("#dialog").dialog({
               title: '状态编辑',    
               width: 400,    
               height: 230,    
               closed: true,    
               cache: false,     
               modal: true,
               buttons: [{
                   text:"保存",
                   handler: function () {
                       $("#dialog-form").ajaxSubmit({
                           success: function (data) {
                               alert(data.Message);
                               if (data.Code==1) {
                                   dialog.dialog("close");
                                   window.location.reload();
                               }
                           },
                           error: function () {
                               alert("服务器出错！");
                           }
                       });
                   }
               }, {
                   text: "取消",
                   handler: function () {
                       dialog.dialog("close");
                   }
               }
               ]
           });
           $("#status").change(function () {
              
               if ($(this).val() == "2") {
                   $("#tr1").css("display", "none");
                   $("#tr2").css("display", "none");
                   $("#tr3").css("display", "none");
                   $("#tr4").css("display", "");
               } else {
                   $("#tr1").css("display", "");
                   $("#tr2").css("display", "");
                   $("#tr3").css("display", "");
                   $("#tr4").css("display", "none");
               }
              
           });
       });
   </script>
    <style type="text/css">
       
        #dialog-form table{
            width:100%;
        }
        #dialog-form input{
          
            width:222px;
            height:20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   商城-订单
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

  
   <table class="table-pro table-color-row" width="100%">
    <thead >       <tr class="table-pro-head"><th >订单编号</th>
           <th>用户编号</th>
           <th>用户帐号</th>
           <th>用户昵称</th>
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
           <th>操作</th>
       </tr>
          </thead>
       <tbody>
   
           <%foreach (var item in (List<DBModel.wgs039>)ViewData["shopRecordList"])
             {%>
           <tr class="table-color-row-even">
           <td><%:item.sr001 %></td>
           <td><%:item.u001 %></td>
           <td><%:item.sr006 %></td>
           <td><%:item.sr007 %></td>
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
           <td>
               <%if (item.sr004==0)
                 {%>
                     <a href="javascript:EditRecord(<%:item.sr001 %>)">编辑</a>
              <% } %>      
               </td>
           </tr>
            <%} %>
       </tbody>
   </table>
       <%=ViewData["PageList"] %>
    <div id="dialog">
      <form id="dialog-form" action="/AM/ShopRecord" method="post">
        <input id="recordId" name="recordId" type="hidden" />
          <table>
              <tr><td>编号：</td><td><strong id="recordId2"></strong></td></tr>
                <tr><td>操作：</td><td><select name="status" id="status" >
              <option value="1">完成</option>
              <option value="2">撤销</option>
          </select></td></tr>
                <tr id="tr1"><td>物流公司：</td><td><input type="text" name="streamCompany" /></td></tr>
                <tr id="tr2"><td>快递查询网址：</td><td><input type="text" name="searchUrl" /></td></tr>
                <tr id="tr3"><td>快递单号：</td><td><input type="text" name="num" /></td></tr>
                <tr id="tr4" style="display:none"><td>撒消原因：</td><td><input type="text" name="why" /></td></tr>
          </table>
      
      </form>
        </div>
</asp:Content>
