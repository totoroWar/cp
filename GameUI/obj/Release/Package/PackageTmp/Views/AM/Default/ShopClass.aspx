<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AM/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    商城-分类
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <script type="text/javascript">
       function add()
       {
       $("#addform").ajaxSubmit({
           success: function (data) {
               alert(data.Message);
           },
           error: function ()
           {
               alert("出错");
           }
       });
       }
   </script>
     <%if (((string)ViewBag.MethodType).ToLower()=="add")
      {%>
          <div class="cjlsoft-body-header">
        <h1>商城-分类-添加</h1>
<%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %>    </div>
    <form id="addform" action="/AM/ShopClass" method="post">
  <div class="blank-line"></div>
    <input type="hidden" name="method" value="<%:ViewData["MethodType"] %>" />
    <table class="table-pro" width="100%">
        <tbody>
       
            <tr>
                <td class="title">名称</td><td><input type="text" name="rc002" class="input-text w300px" value="" /></td>
            </tr>
           
            <tr>
                <td class="title">排序</td><td><input type="text" name="rc003" class="input-text w300px" value="0" /></td>
            </tr>
            <tr>
                <td class="title"></td><td><input type="button" value="保存" onclick="add();" /></td>
            </tr>
        </tbody>
    </table>
    </form>
    <%
      }else{ %>
   
     <div class="cjlsoft-body-header">
        <h1>商城-分类</h1>
        <div class="left-nav">
            <a id="a-menu" href="/AM/ShopClass?method=add" title="添加分类">添加分类</a>
        </div>
        <%Html.RenderPartial("~/Views/AM/Default/Common/RightLinks.ascx"); %> 
    </div>
    <form action="/AM/ShopClass" method="post">
        
  <div class="blank-line"></div>
    <table width="100%" class="table-pro table-color-row">
            <thead>
                <tr class="table-pro-head">
                    <th>编号</th>
                    <th>名称</th>
                    <th>排序</th>
                </tr>
            </thead>
            <tbody>
                <% 
                    int i = 0;
                    foreach (var item in (List<DBModel.wgs032>)ViewData["ModelList"])
                   {
                        %>

                  <tr class="table-color-row-even">
                    <td>
                        <input name="[<%:i %>].rc001" type="hidden" value="<%:item.rc001.ToString().Trim() %>"><%:item.rc001 %></td>
                    <td>
                        <input name="[<%:i %>].rc002" class="input-text w100px" type="text" value="<%:item.rc002.Trim() %>" cjlsoft-input-text-old-value="<%:item.rc002.Trim() %>"></td>
                    <td>
                        <input name="[<%:i %>].rc003" class="input-text w100px" type="text" value="<%:item.rc003.ToString().Trim() %>" cjlsoft-input-text-old-value="<%:item.rc003 %>"></td>
                    <td>
                        </td>
                </tr>
                <%
                       i++;
                   } %>
              
               </tbody> </table>
        <div class="blank-line"></div>

        <div id="cjlsoft-bottom-function">
            <input type="submit" value="保存" class="cjlsoft-post-loading btn-normal" />
        </div>
        </form>
    <%} %>
    
</asp:Content>
