<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Coach_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.6/dist/sweetalert2.all.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ImageButton runat="server" ID="imgCaptcha" ImageUrl="~/Captcha.ashx" onclick="imgCaptcha_Click"/>
            <asp:TextBox runat="server" ID="txtAnswer" />
            <asp:Button Text="輸入" runat="server" ID="btnSubmit" onclick="btnSubmit_Click" />
        </div>
    </form>
</body>
</html>
