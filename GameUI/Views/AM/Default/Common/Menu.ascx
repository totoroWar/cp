<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<DBModel.wgs004>>" %>
<ul class="easyui-tree" data-options="lines:false">
    <%foreach( var root in Model){ %>
    <%
          if (root.sm002 != 0)
              continue;
    %>
    <li title="<%:root.sm004 %>">
        <span class="root_icon"><%:root.sm004 %></span>
        <ul>
            <%foreach(var sub in Model)
              {
                  if (sub.sm002 != root.sm001)
                      continue;
                   %>
            <li class="sub_icon"><a href="javascript:ui_show_tab('<%:sub.sm004 %>','/<%:sub.sm006 %>/<%:sub.sm007 %>',true);" title="<%:sub.sm004 %>"><%:sub.sm004 %></a></li>
            <%}/*sub*/ %>
        </ul>
    </li>
    <%}/*root*/ %>
</ul>