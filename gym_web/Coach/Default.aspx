<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Coach_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Image runat="server" ID="image" ImageUrl="~/Captcha.ashx" />
            <asp:TextBox runat="server" ID="txtAnswer" />
            <asp:Button Text="輸入" runat="server" ID="btnSubmit" onclick="btnSubmit_Click" />
        </div>
    </form>
</body>
</html>
