<%@ Page Language="C#" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=gbk" />
<title>在线支付</title>
</head>
<body>
<form id="yeepay" name="yeepay" action="<%=ViewData["reqURL_onLine"]%>" method="post">
		        <input type="hidden" name="p0_Cmd"					value="Buy"/>
		        <input type="hidden" name="p1_MerId"				value="<%=ViewData["p1_MerId"]%>"/>
		        <input type="hidden" name="p2_Order"				value="<%=ViewData["p2_Order"]%>"/>
		        <input type="hidden" name="p3_Amt"					value="<%=ViewData["p3_Amt"]%>"/>
		        <input type="hidden" name="p4_Cur"					value="<%=ViewData["p4_Cur"]%>"/>
		        <input type="hidden" name="p5_Pid"					value="<%=ViewData["p5_Pid"]%>"/>
		        <input type="hidden" name="p6_Pcat"				    value="<%=ViewData["p6_Pcat"]%>"/>
		        <input type="hidden" name="p7_Pdesc"				value="<%=ViewData["p7_Pdesc"]%>"/>
		        <input type="hidden" name="p8_Url"				    value="<%=ViewData["p8_Url"]%>"/>
		        <input type="hidden" name="p9_SAF"					value="<%=ViewData["p9_SAF"]%>"/>
		        <input type="hidden" name="pa_MP"		            value="<%=ViewData["pa_MP"]%>"/>
		        <input type="hidden" name="pd_FrpId"			    value="<%=ViewData["pd_FrpId"]%>"/>		
		        <input type="hidden" name="pr_NeedResponse"			value="<%=ViewData["pr_NeedResponse"]%>"/>
		        <input type="hidden" name="hmac"					value="<%=ViewData["hmac"]%>"/>
</form>
            <%if("0" == (string)ViewData["Debug"]){ %>
            <script type="text/javascript">
                document.getElementById("yeepay").submit();
            </script>
            <%}else{ %>
            <%=ViewData["SignInfo"] %>
            <%} %>
</body>
</html>