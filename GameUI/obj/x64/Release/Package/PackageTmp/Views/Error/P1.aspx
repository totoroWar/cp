<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<HandleErrorInfo>" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>请求不存在</title>
    <style>
        body {
            margin:0;
            padding:0;
        }
        .info_block {
            padding:5px;
            margin:5px;
            border:1px dotted #eee;
            background:#efefef;
        }
    </style>
</head>
<body>
    <div>
        <h1>请求不存在</h1>
        <div class="info_block">
            <pre>
            <%:Model.Exception.GetType() %>
            </pre>
        </div>
        <div class="info_block">
            <pre>
            <%:Model.Exception.Message %>
            </pre>
        </div>
        <div class="info_block">
            <pre>
            <%:Model.Exception.StackTrace %>
            </pre>
        </div>
        <div class="info_block">
            <pre>
            <%:Model.Exception.TargetSite %>
            </pre>
        </div>
    </div>
</body>
</html>
