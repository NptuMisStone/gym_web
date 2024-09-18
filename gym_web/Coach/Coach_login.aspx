<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Coach_login.aspx.cs" Inherits="Coach_Coach_login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <title>教練登入</title>
    <link rel="stylesheet" href=" css/login.css" />
    <link rel="stylesheet" type="text/css" href="slide navbar login.css" />
    <link href="https://fonts.googleapis.com/css2?family=Jost:wght@500&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.6/dist/sweetalert2.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.6/dist/sweetalert2.all.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="ScriptManager" />
        <div class="main">
            <input type="checkbox" id="chk" aria-hidden="true" />

            <div class="signup">
                <div>
                    <label for="chk" aria-hidden="true">教練登入</label>
                    <asp:TextBox ID="coach_acc" placeholder="請輸入帳號" runat="server" required="required"></asp:TextBox>
                    <asp:TextBox ID="coach_pwd" placeholder="請輸入密碼" runat="server" TextMode="Password" required="required"></asp:TextBox>
                    <asp:Button ID="btn_coach_login" runat="server" Text="登入" CssClass="btn" OnClick="btn_coach_login_Click" />
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Coach/Coach_forgetpwd.aspx" ForeColor="White" Style="margin-left: 150px;">忘記密碼?</asp:HyperLink><br />

                    <asp:Label ID="Label1" runat="server" Text="沒有帳號嗎?" ForeColor="White" Style="margin-left: 90px;"></asp:Label>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Coach/Coach_register.aspx" ForeColor="White">立即註冊教練帳號</asp:HyperLink>
                    <asp:Label ID="Label2" runat="server" Text="切換為會員：" ForeColor="White" Style="margin-left: 90px;"></asp:Label>
                    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/User/User_login.aspx" ForeColor="White">立即切換</asp:HyperLink>
                </div>
            </div>
        </div>
    </form>
</body>
<script src="https://code.jquery.com/jquery-3.6.0.min.js" type="text/javascript"></script>
</html>
