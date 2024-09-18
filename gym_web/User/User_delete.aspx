<%@ Page Language="C#" AutoEventWireup="true" CodeFile="User_delete.aspx.cs" Inherits="User_User_delete" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <!--//stylesheets-->
    <link href="//nptu.sharepoint.com///fonts.googleapis.com/css?family=Oswald:400,500,600" rel="stylesheet">
    <link href="//nptu.sharepoint.com///fonts.googleapis.com/css?family=PT+Sans" rel="stylesheet">
    <!--//stylesheets補加-->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.6/dist/sweetalert2.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.6/dist/sweetalert2.all.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            刪除帳號<br />
            <br />
            如要刪除帳號請輸入密碼<br />
            <asp:TextBox ID="TextBox1" runat="server" TextMode="Password" Width="170px"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="確認刪除" />
            <br />
            <br />
            <asp:Button ID="Button2" runat="server" Text="取消" Width="75px" PostBackUrl="~/User/User_info.aspx" />
        </div>
    </form>
</body>
</html>
