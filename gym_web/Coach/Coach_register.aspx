<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Coach_register.aspx.cs" Inherits="Coach_Coach_register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
   
    <style>
        .radioButtonList {
            margin:auto;
        }
        .radioButtonList label {
            font-size: 18px;
            margin:20px;
        }
    </style>
    
    <title>教練註冊</title>
    <link rel="stylesheet" href=" css/login.css">
    <link rel="stylesheet" type="text/css" href="slide navbar login.css">
    <link href="https://fonts.googleapis.com/css2?family=Jost:wght@500&display=swap" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="ScriptManager" />
        <div class="main">
            <input type="checkbox" id="chk" aria-hidden="true">
            <div class="signup">
                <div style="max-height: 500px; overflow-y: auto; padding-bottom: 10px;" class="custom-scrollbar">
                    <label for="chk" aria-hidden="true">教練註冊</label>
                    <asp:Label ID="Label2" runat="server" Text="請輸入姓名" ForeColor="White" Style="margin-left: 70px;" Font-Bold="True"></asp:Label>
                    <asp:Label ID="Label7" runat="server" Text="*(必填)" ForeColor="red" Font-Bold="True"></asp:Label>
                    <asp:TextBox ID="tb_coach_name" required="required" runat="server"></asp:TextBox>
                    <asp:Label ID="Label1" runat="server" Text="請輸入帳號" ForeColor="White" Style="margin-left: 70px;" Font-Bold="True"></asp:Label>
                    <asp:Label ID="Label8" runat="server" Text="*(必填)" ForeColor="red" Font-Bold="True"></asp:Label>
                    <asp:TextBox ID="tb_coach_acc" required="required" runat="server"></asp:TextBox>
                    <asp:Label ID="Label3" runat="server" Text="請輸入密碼" ForeColor="White" Style="margin-left: 70px;" Font-Bold="True"></asp:Label>
                    <asp:Label ID="Label9" runat="server" Text="*(必填)" ForeColor="red" Font-Bold="True"></asp:Label>
                    <asp:TextBox ID="tb_coach_pwd" required="required" runat="server" TextMode="Password" ControlToCompare="tb_coach_pwd" CausesValidation="True"></asp:TextBox>
                    <asp:Label ID="Label4" runat="server" Text="再次輸入密碼" ForeColor="White" Style="margin-left: 70px;" Font-Bold="True"></asp:Label>
                    <asp:Label ID="Label10" runat="server" Text="*(必填)" ForeColor="red" Font-Bold="True"></asp:Label>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="*密碼不一致~" ControlToCompare="tb_coach_pwd" ControlToValidate="tb_coach_pwd2" ForeColor="Red" SetFocusOnError="True"></asp:CompareValidator>
                    <asp:TextBox ID="tb_coach_pwd2" required="required" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:Label ID="Label5" runat="server" Text="請輸入電話" ForeColor="White" Style="margin-left: 70px;" Font-Bold="True"></asp:Label>
                    <asp:Label ID="Label11" runat="server" Text="*(必填)" ForeColor="red" Font-Bold="True"></asp:Label>
                    <asp:TextBox ID="tb_phone" required="required" runat="server"></asp:TextBox>
                    <asp:Label ID="Label6" runat="server" Text="請輸入email" ForeColor="White" Style="margin-left: 70px;" Font-Bold="True"></asp:Label>
                    <asp:Label ID="Label12" runat="server" Text="*(必填)" ForeColor="red" Font-Bold="True"></asp:Label>
                    <asp:TextBox ID="tb_email" runat="server" required="required"></asp:TextBox>
                    <asp:Label ID="Label13" runat="server" Text="請勾選性別" ForeColor="White" Style="margin-left: 70px;" Font-Bold="True"></asp:Label>
                    <asp:Label ID="Label14" runat="server" Text="*(必勾)" ForeColor="red" Font-Bold="True"></asp:Label>
                    
                    <asp:RadioButtonList ID="tb_gender" runat="server" CssClass="radioButtonList" RepeatDirection="Horizontal" >
                        <asp:ListItem Value="1" >男生</asp:ListItem>
                        <asp:ListItem Value="2" >女生</asp:ListItem>
                        <asp:ListItem Value="3" >其他</asp:ListItem>
                    </asp:RadioButtonList>

                    <asp:Button ID="btn_coach_login" runat="server" Text="註冊" OnClick="btn_coach_login_Click" CssClass="btn" />
                    <asp:HyperLink ID="HyperLink1" runat="server" Style="margin-left: 140px;" NavigateUrl="~/Coach/Coach_login.aspx" ForeColor="White">返回登入頁面</asp:HyperLink>
                    <%-- <button>Sign up</button>--%>
                </div>
            </div>
        </div>
    </form>
</body>
<script src="https://code.jquery.com/jquery-3.6.0.min.js" type="text/javascript"></script>
</html>

