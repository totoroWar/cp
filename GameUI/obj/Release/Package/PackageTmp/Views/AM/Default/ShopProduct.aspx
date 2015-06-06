<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>



<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    商城-物品
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <script type="text/javascript"> 
        function ChangeDateFormat(time) {
            if (time != null) {
                var date = new Date(parseInt(time.replace("/Date(", "").replace(")/", ""), 10));
                var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
                var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
                return date.getFullYear() + "/" + month + "/" + currentDate;
            }
            return "";
        }
       function ProductUpdate(id)
       {
          
           $("#dialog").dialog("open");
            $.ajax({
                type: "post",
                url: "/AM/ShopProduct",
                data: { "method": "get","key":id },
                success: function (data) {
                   
                    $("#r001").val(data.r001);
                    $("#r002").val(data.r002);
                    $("#r003").val(data.r003);
                    $("#r004").val(data.r004);
                    $("#r005").val(data.r005);
                   
                    $("#r007").val(ChangeDateFormat(data.r007));
                    $("#rc001").val(data.rc001);
                    $("#r008").val(data.r008);
                    $("#r009").val(data.r009);
                    $("#r010").val(ChangeDateFormat(data.r010));
                    $("#r011").val(r011);
                    $("#method").val("update");
                }
            });
       }
       function ProductAdd() {

           $("#dialog").dialog("open");
           $("#r001").val("");
           $("#r002").val("");
           $("#r003").val("");
           $("#r004").val("");
           $("#r005").val("");
           $("#r007").val("");
           $("#rc001").val("");
           $("#r008").val("");
           $("#r009").val("");
           $("#r010").val("");
           $("#method").val("add");
       }
       function FinisheEdit() {
           var bar = $('.bar');
           var percent = $('.percent');
         
           $('#dialog-form').ajaxForm({
               beforeSend: function () {
                 
                   var percentVal = '0%';
                   bar.width(percentVal)
                   percent.html(percentVal);
               },
               uploadProgress: function (event, position, total, percentComplete) {
                   var percentVal = percentComplete + '%';
                   bar.width(percentVal)
                   percent.html(percentVal);
                   console.log(percentVal, position, total);
               },
               success: function () {
               
                   var percentVal = '100%';
                   bar.width(percentVal)
                   percent.html(percentVal);
               },
               complete: function (xhr) {
                   var result = JSON.parse(xhr.responseText);
                   alert(result.Message);
                   if (result.Code==1) {
                       $("#dialog").dialog("close");
                       window.location.reload();
                   }
               }
           });
       }
	
    
       function ProductDelete(id)
        {
           
            $.ajax({
                type: "POST",
                url: "/AM/ShopProduct",
                data: { "method": "delete", "id": id },
                success: function (result) {
                    alert(result.Message);
                    if (result.Code==1) {
                        window.location.reload();
                    }    
                }
            });
        }

       $(function () {
           $("#dialog").dialog({
               top:"20px",
               title: "编辑",
               width: 600,
               height: 423,
               closed: true,
               cache: false,
               modal: true,
               buttons:[{
                   text:'确定',
                   handler: function () { FinisheEdit();$("#dialog-form").submit(); }
               },{
                   text:'关闭',
                   handler: function () { $("#dialog").dialog("close");  }
               }]

           });
        jQuery('input[name="r007"]').datetimepicker({
            format: 'Y/m/d',
            lang: 'ch',
            onShow: function (ct) {
                this.setOptions({
                    maxDate: jQuery('input[name="r007"]').val() ? jQuery('input[name="r007"]').val() : false
                })
            },
            timepicker: false
        });
        jQuery('input[name="r010"]').datetimepicker({
            format: 'Y/m/d',
            lang: 'ch',
            onShow: function (ct) {
                this.setOptions({
                    minDate: jQuery('input[name="r010"]').val() ? jQuery('input[name="r007"]').val() : false
                })
            },
            timepicker: false
        });
        });
     </script>
   
     
  
     <div class="cjlsoft-body-header">
        <h1>商城-物品</h1>
        <div class="left-nav">
            <a id="a-menu" href="javascript:void(0)" onclick="ProductAdd()" title="添加分类">添加物品</a>
        </div>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %> 
    </div>
    <form action="/AM/ShopClass" method="get">

        <div class="blank-line"></div>      
        <div class="cjlsoft-body-header">  
      <ul>
          <li style="float:left;">选择分类：</li>  
      <%foreach (var item in (List<DBModel.wgs032>)ViewData["shopClassList"])
        {%>
      <li style="float:left;padding-left:15px;" >
         <a href="/AM/ShopProduct?shopClassId=<%:item.rc001 %>" title="<%:item.rc002 %>"><%:item.rc002 %></a> 
      </li>
      <%}%>
         
  </ul> </div>
        <div class="blank-line"></div>
 <div>
   <table  class="table-pro table-color-row" width="100%">
       <thead>
           <tr class="table-pro-head"><th class="title">编号</th>
               <th>类别</th>
               <th>标题</th>
               <th>价格</th>
               <th>评分（星级）</th>
               <th>件数</th>
               <th>图片</th>
               <th>上架时间</th>
               <th>下架时间</th>
               <th>VIP是否打折</th>
               <th>是否显示(1显示，0不显示)</th>
               <th>排序</th>
                <th>操作</th>
           </tr>
       </thead>
           <% 
               
               int flag = 0;
                    foreach (var item in (List<DBModel.wgs033>)ViewData["shopProductList"])
                   {
                        %>
       <tr <%:flag%2==0?" class=\"table-color-row-even\"":""%>>
        
            <td><%: item.r001%></td>
            <td><%:item.rc001 %></td>
             <td><%:item.r002 %></td>
             <td><%:item.r003 %></td>
             <td><%:item.r004 %></td>
             <td><%:item.r005 %></td>
             <td><img src="<%:"/images/shop/"+item.r006 %>" style="width:70px; height:80px; "  alt="图片已失效" /></td>
            <td><%:item.r007 %></td>
            <td><%:item.r010 %></td>
           <td><%:item.r011==0?"否":"是" %></td>
             <td><%:item.r009 %></td>
            <td><%:item.r008 %></td>
            <td><a href="javascript:void(0);" onclick="ProductUpdate(<%:item.r001 %>);">更改</a><a href="javascript:void(0);" onclick="ProductDelete(<%:item.r001 %>);">删除</a></td>
            </tr>
                <%
                      
                   } %>
    </table>
              
              
           </div>
        <div class="blank-line"></div>

    
        </form>

   <div id="dialog">
    <form id="dialog-form" action="/AM/ShopProduct" method="post" enctype="multipart/form-data">
      <div id="dialog-div">
  <div class="blank-line"></div>
    <input type="hidden" id="method" name="method" value="<%:ViewData["MethodType"] %>" />
          <input type="hidden" id="r001" name="r001" value="" />
    <table class="table-pro" width="100%">
        <tbody>
        <tr>
                <td class="title">分类</td><td><select id="rc001" name="rc001"  >
                    <%foreach (var item in (List<DBModel.wgs032>)ViewData["shopClassList"])
                      {
                          %>
                    <option value="<%:item.rc001 %>" ><%:item.rc002 %></option>
                    <%
                      } %>
          </select> </td> </tr>

            <tr>
              
                <td class="title">标题</td><td><input type="text" id="r002" name="r002" class="input-text w300px" value="" maxlength="50" /></td>
            </tr>
          <tr>
                <td class="title">价格(积分)</td><td><input type="text" id="r003" name="r003" class="input-text w300px" value=""  /></td>
            </tr>
             <tr>
                <td class="title">评分</td><td><select name="r004" id="r004">
                    <option value="1">1</option>
                    <option value="2">2</option>
                    <option value="3">3</option>
                    <option value="4">4</option>
                    <option value="5">5</option>
                     </select></td>
            </tr>
             <tr>
                <td class="title">件数(数字)</td><td><input type="text" id="r005" name="r005" class="input-text w300px" value="" /></td>
            </tr>
             
              <tr>
                <td class="title">图片</td><td><input type="file" id="picture
" name="picture"  class="input-text w300px" value="" /></td>
            </tr>
             <tr>
                <td class="title">上架时间</td><td><input type="text" id="r007" name="r007" class="input-text w300px" value="" /></td>
            </tr>
           
             <tr>
                <td class="title">下架时间</td><td><input type="text" id="r010" name="r010" class="input-text w300px" value="" /></td>
            </tr>
               <tr>
                <td class="title">VIP折扣</td><td>
                    <select id="r011" name="r011">
                        <option value="1">是</option>
                        <option value="0">否</option>
                    </select>
                                           </td>
            </tr>
             <tr>
                <td class="title">是否显示</td><td>
                    <select id="r009" name="r009">
                        <option value="1">显示</option>
                        <option value="0">不显示</option>
                    </select>
                                           </td>
            </tr>
            <tr>
                <td class="title">排序</td><td><input type="text" id="r008" name="r008" class="input-text w300px" value="0" /></td>
            </tr>
            
        </tbody>

    </table>
          <div class="progress" style=" position:relative; width:400px; border: 1px solid #ddd; padding: 1px; border-radius: 3px; ">
        <div class="bar" style="background-color: #B4F5B4; width:0%; height:20px; border-radius: 3px;"></div >
        <div class="percent" style=" position:absolute; display:inline-block; top:3px; left:48%;" >0%</div >
              
    </div>
    
    <div id="status"></div>
           </div>
    </form>
      
  </div>
</asp:Content>
