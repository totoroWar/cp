<%@ Page Title="" Language="C#" MasterPageFile="~/Views/UI/Default/Common/PageDefault.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %><asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"><%:ViewData["GameName"] %>-开奖历史</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    var gsrList = (List<DBModel.wgs038>)ViewData["GSRList"];
    var count = (int)ViewData["Count"];
    var gameID = (int)ViewData["GameID"];
    var gameClassID = (int)ViewData["GameClassID"];
    gsrList = gsrList.OrderBy(exp => exp.gs005).ToList();
%>
    <div class="ui-page-content-body-header tools">
        <div class="left-nav">
        <a href="/GameResult.html?gameID=<%:ViewData["GameID"] %>&gameClassID=<%:ViewData["GameClassID"] %>&count=30" <%=count== 30 ? "class='item-select'" : "" %>>最近30期</a>
        <a href="/GameResult.html?gameID=<%:ViewData["GameID"] %>&gameClassID=<%:ViewData["GameClassID"] %>&count=60" <%=count== 60 ? "class='item-select'" : "" %>>最近60期</a>
        <a href="/GameResult.html?gameID=<%:ViewData["GameID"] %>&gameClassID=<%:ViewData["GameClassID"] %>&count=120" <%=count== 120 ? "class='item-select'" : "" %>>最近120期</a>
        </div>
    </div>
    <div class="blank-line"></div>
    <div class="block_tools">
        <label for="chk_miss_hide">隐藏遗漏<input type="checkbox" id="chk_miss_hide" name="chk_miss_hide" value="1" /></label>
    </div>
    <div class="blank-line"></div>
    <div class="gsrhistory_record">
        <%if( 1 == gameClassID) { %>
    <table class="table-pro w100ps tp5 g_nco">
        <thead>
            <tr class="table-pro-head">
                <th style="text-align:center;">期号</th>
                <th colspan="5" style="text-align:center;">结果</th>
                <th colspan="10" style="text-align:center;">万位</th>
                <th colspan="10" style="text-align:center;">千位</th>
                <th colspan="10" style="text-align:center;">百位</th>
                <th colspan="10" style="text-align:center;">十位</th>
                <th colspan="10" style="text-align:center;">个位</th>
            </tr>
        </thead>
        <tbody>
            <tr class="text-cent fc-blue">

                <td colspan="6"></td>

                <td>0</td>
                <td>1</td>
                <td>2</td>
                <td>3</td>
                <td>4</td>
                <td>5</td>
                <td>6</td>
                <td>7</td>
                <td>8</td>
                <td>9</td>

                <td>0</td>
                <td>1</td>
                <td>2</td>
                <td>3</td>
                <td>4</td>
                <td>5</td>
                <td>6</td>
                <td>7</td>
                <td>8</td>
                <td>9</td>

                <td>0</td>
                <td>1</td>
                <td>2</td>
                <td>3</td>
                <td>4</td>
                <td>5</td>
                <td>6</td>
                <td>7</td>
                <td>8</td>
                <td>9</td>

                <td>0</td>
                <td>1</td>
                <td>2</td>
                <td>3</td>
                <td>4</td>
                <td>5</td>
                <td>6</td>
                <td>7</td>
                <td>8</td>
                <td>9</td>

                <td>0</td>
                <td>1</td>
                <td>2</td>
                <td>3</td>
                <td>4</td>
                <td>5</td>
                <td>6</td>
                <td>7</td>
                <td>8</td>
                <td>9</td>
            </tr>
            <%if( null != gsrList) 
              {
                  Dictionary<string, int> dicMiss = new Dictionary<string, int>();
                  for (int x = 0; x < 5; x++)
                  {
                      for (int j = 0; j < 10; j++)
                      {
                          dicMiss.Add(x.ToString() + j, 0);
                      }
                  }
                      foreach (var item in gsrList)
                      {
                          var resultSplit = item.gs007.Trim().Split(',');
                  %>
            <tr class="text-cent">
                <td><%=item.gs002.Trim()%></td>

                <%foreach (var num in resultSplit)
                  { %>
                <td class="fc-yellow"><%="<div class='gsrbx'>" + num + "</div>"%></td>
                <%} %>

                <%
                    var dicPos = new Dictionary<int, string>() {{0,"万位，第一球"},{1,"千位，第二球"},{2,"百位，第三球"},{3,"十位，第四球"},{4,"个位，第五球"}};
                    for (int col = 0; col < 5; col++)
                  {
                      %>
                    <%for (int i = 0; i < 10; i++)
                      {
                          int miss_num = 0;
                          bool miss_exists = dicMiss.TryGetValue(col.ToString() + i, out miss_num);
                          miss_num = resultSplit[col] == i.ToString() ? 0 : ++miss_num;
                          if (miss_exists)
                          {
                              dicMiss[col.ToString() + i] = miss_num;
                          }
                           %>
                <td title="<%:dicPos[col] %>" class="poscol<%:col %>"><%=resultSplit[col] == i.ToString() ? "<div class='gsrb'>" + resultSplit[col] + "</div>" : "<span class='miss_number'>"+miss_num.ToString()+"</span>"%></td>
                    <%} %>
                <%}/*out col*/ %>

            </tr>
            <%
                      }/*foreach*/
            }/*no*/ %>
        </tbody>
    </table>
        <%} %>
        <%else if(5 == gameClassID){ %>
    <table class="table-pro w100ps tp5 g_nco">
        <thead>
            <tr class="table-pro-head">
                <th style="text-align:center;">期号</th>
                <th colspan="5" style="text-align:center;">结果</th>
                <th colspan="10" style="text-align:center;">百位</th>
                <th colspan="10" style="text-align:center;">十位</th>
                <th colspan="10" style="text-align:center;">个位</th>
            </tr>
        </thead>
        <tbody>
            <tr class="text-cent fc-blue">

                <td colspan="6"></td>

                <td>0</td>
                <td>1</td>
                <td>2</td>
                <td>3</td>
                <td>4</td>
                <td>5</td>
                <td>6</td>
                <td>7</td>
                <td>8</td>
                <td>9</td>

                <td>0</td>
                <td>1</td>
                <td>2</td>
                <td>3</td>
                <td>4</td>
                <td>5</td>
                <td>6</td>
                <td>7</td>
                <td>8</td>
                <td>9</td>

                <td>0</td>
                <td>1</td>
                <td>2</td>
                <td>3</td>
                <td>4</td>
                <td>5</td>
                <td>6</td>
                <td>7</td>
                <td>8</td>
                <td>9</td>
            </tr>
            <%if( null != gsrList) 
              {
                  Dictionary<string, int> dicMiss = new Dictionary<string, int>();
                  for (int x = 0; x < 3; x++)
                  {
                      for (int j = 0; j < 10; j++)
                      {
                          dicMiss.Add(x.ToString() + j, 0);
                      }
                  }
                      foreach (var item in gsrList)
                      {
                          var resultSplit = item.gs007.Trim().Split(',');
                  %>
            <tr class="text-cent">
                <td><%=item.gs002.Trim()%></td>

                <%foreach (var num in resultSplit)
                  { %>
                <td class="fc-yellow"><%="<div class='gsrbx'>" + num + "</div>"%></td>
                <%} %>
                <td>-</td>
                <td>-</td>

                <%
                    var dicPos = new Dictionary<int, string>() {{0,"万位，第一球"},{1,"千位，第二球"},{2,"百位，第三球"},{3,"十位，第四球"},{4,"个位，第五球"}};
                    for (int col = 0; col < 3; col++)
                  {
                      %>
                    <%for (int i = 0; i < 10; i++)
                      {
                          int miss_num = 0;
                          bool miss_exists = dicMiss.TryGetValue(col.ToString() + i, out miss_num);
                          miss_num = resultSplit[col] == i.ToString() ? 0 : ++miss_num;
                          if (miss_exists)
                          {
                              dicMiss[col.ToString() + i] = miss_num;
                          }
                           %>
                <td title="<%:dicPos[col] %>" class="poscol<%:col %>"><%=resultSplit[col] == i.ToString() ? "<div class='gsrb'>" + resultSplit[col] + "</div>" : "<span class='miss_number'>"+miss_num.ToString()+"</span>"%></td>
                    <%} %>
                <%}/*out col*/ %>

            </tr>
            <%
                      }/*foreach*/
            }/*no*/ %>
        </tbody>
    </table>
        <%} %>
        <%else if( 3 == gameClassID){ %>
    <table class="table-pro w100ps tp5 g_nco">
        <thead>
            <tr class="table-pro-head">
                <th style="text-align:center;">期号</th>
                <th colspan="5" style="text-align:center;">结果</th>
                <th colspan="11" style="text-align:center;">万位</th>
                <th colspan="11" style="text-align:center;">千位</th>
                <th colspan="11" style="text-align:center;">百位</th>
                <th colspan="11" style="text-align:center;">十位</th>
                <th colspan="11" style="text-align:center;">个位</th>
            </tr>
        </thead>
        <tbody>
            <tr class="text-cent fc-blue">

                <td colspan="6"></td>

                <td>01</td>
                <td>02</td>
                <td>03</td>
                <td>04</td>
                <td>05</td>
                <td>06</td>
                <td>07</td>
                <td>08</td>
                <td>09</td>
                <td>10</td>
                <td>11</td>

                <td>01</td>
                <td>02</td>
                <td>03</td>
                <td>04</td>
                <td>05</td>
                <td>06</td>
                <td>07</td>
                <td>08</td>
                <td>09</td>
                <td>10</td>
                <td>11</td>

                <td>01</td>
                <td>02</td>
                <td>03</td>
                <td>04</td>
                <td>05</td>
                <td>06</td>
                <td>07</td>
                <td>08</td>
                <td>09</td>
                <td>10</td>
                <td>11</td>

                <td>01</td>
                <td>02</td>
                <td>03</td>
                <td>04</td>
                <td>05</td>
                <td>06</td>
                <td>07</td>
                <td>08</td>
                <td>09</td>
                <td>10</td>
                <td>11</td>

                <td>01</td>
                <td>02</td>
                <td>03</td>
                <td>04</td>
                <td>05</td>
                <td>06</td>
                <td>07</td>
                <td>08</td>
                <td>09</td>
                <td>10</td>
                <td>11</td>
            </tr>
            <%if( null != gsrList) 
              {
                  Dictionary<string, int> dicMiss = new Dictionary<string, int>();
                  for (int x = 0; x < 5; x++)
                  {
                      for (int j = 1; j < 12; j++)
                      {
                          dicMiss.Add(x.ToString() + j, 0);
                      }
                  }
                      foreach (var item in gsrList)
                      {
                          var resultSplit = item.gs007.Trim().Split(',');
                  %>
            <tr class="text-cent">
                <td><%=item.gs002.Trim()%></td>

                <%foreach (var num in resultSplit)
                  {
                      var dNum = int.Parse(num);
                       %>
                <td class="fc-yellow"><%="<div class='gsrbx'>" + (dNum < 10 ? "0"+dNum.ToString() : dNum.ToString()) + "</div>"%></td>
                <%} %>

                <%
                    var dicPos = new Dictionary<int, string>() {{0,"万位，第一球"},{1,"千位，第二球"},{2,"百位，第三球"},{3,"十位，第四球"},{4,"个位，第五球"}};
                    for (int col = 0; col < 5; col++)
                  {
                      %>
                    <%for (int i = 0; i < 11; i++)
                      {
                          int miss_num = 0;
                          bool miss_exists = dicMiss.TryGetValue(col.ToString() + i, out miss_num);
                          miss_num = resultSplit[col] == i.ToString() ? 0 : ++miss_num;
                          if (miss_exists)
                          {
                              dicMiss[col.ToString() + i] = miss_num;
                          }
                          int addNum = int.Parse(resultSplit[col]);
                           %>
                <td title="<%:dicPos[col] %>" class="poscol<%:col %>"><%=resultSplit[col] == i.ToString() ? "<div class='gsrb'>" + (addNum < 10 ? "0"+addNum : addNum.ToString()) + "</div>" : "<span class='miss_number'>"+miss_num.ToString()+"</span>"%></td>
                    <%} %>
                <%}/*out col*/ %>

            </tr>
            <%
                      }/*foreach*/
            }/*no*/ %>
        </tbody>
    </table>
        <%} %>
    </div>
    <script type="text/javascript">
        $("#chk_miss_hide").click(function ()
        {
            $(".miss_number").toggleClass("dom-hide");
        });
        var trs = $("tbody tr");
        $.each(trs, function (i, n)
        {
            $(n).children(".poscol0:first").css("border-left", "2px solid #ef6a2a");
            $(n).children(".poscol1:first").css("border-left", "2px solid #adc532");
            $(n).children(".poscol2:first").css("border-left", "2px solid #cd302c");
            $(n).children(".poscol3:first").css("border-left", "2px solid #59c4d5");
            $(n).children(".poscol4:first").css("border-left", "2px solid #d04588");
        });
    </script>
</asp:Content>