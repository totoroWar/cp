<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<HandleErrorInfo>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>请联系客服</title>
<style type="text/css">
    body{ background:#eaeaea;}
</style>
</head>

<body>
    <h1>是否有不正确操作？</h1>
    <div>
        <p><strong>记录编号：</strong><%:ViewData["ID"] %></p>
        <p><strong>记录时间：</strong><%:ViewData["DateTime"] %></p>
    </div>
</body>
</html>
