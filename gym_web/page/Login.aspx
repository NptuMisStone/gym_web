<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="page_Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>登入</title>
    <link href="https://fonts.googleapis.com/css2?family=Jost:wght@500&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.6/dist/sweetalert2.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.6/dist/sweetalert2.all.min.js"></script>
    <style>
        body, html {
            height: 100%;
            margin: 0;
            font-family: 'Jost', sans-serif;
            background: linear-gradient(to right, rgba(36, 36, 36, 0.7), rgba(36, 36, 36, 0.7)), url('img/bg.jpg') no-repeat center center fixed;
            background-size: cover;
            display: flex;
            justify-content: center;
            align-items: center;
            overflow: hidden;
        }

        .main {
            width: 100%;
            max-width: 400px;
            background: rgba(255, 255, 255, 0.1);
            backdrop-filter: blur(10px);
            border-radius: 15px;
            padding: 30px;
            text-align: left;
            margin: 50px auto;
            overflow: hidden;
        }

            .main label {
                font-size: 2em;
                color: #0066cc; /* 改為藍色 */
                text-shadow: 0px 0px 5px rgba(0, 0, 0, 0.5);
                margin-bottom: 20px;
                display: block;
                font-weight: bold;
            }

            .main input[type="text"],
            .main input[type="password"] {
                width: 100%;
                padding: 15px;
                margin: 10px 0;
                border: none;
                border-radius: 5px;
                box-sizing: border-box;
                font-size: 1em;
                background: rgba(255, 255, 255, 0.3);
                color: #ffffff;
                box-shadow: inset 0 0 10px rgba(0, 0, 0, 0.5);
                transition: all 0.3s ease-in-out;
            }

                .main input[type="text"]::placeholder,
                .main input[type="password"]::placeholder {
                    color: #bbb;
                }

                .main input[type="text"]:focus,
                .main input[type="password"]:focus {
                    background: rgba(255, 255, 255, 0.5);
                    box-shadow: 0 0 10px #3399ff, 0 0 20px #606060; /* 聚焦時霓虹燈效果 */
                    outline: none;
                }

            .main .btn {
                width: 100%;
                padding: 15px;
                margin: 20px 0;
                border: none;
                border-radius: 5px;
                box-sizing: border-box;
                font-size: 1em;
                background: linear-gradient(45deg, #3399ff, #303030); /* 藍黑漸層 */
                color: white;
                cursor: pointer;
                transition: all 0.3s ease-in-out;
                box-shadow: 0 0 10px #3399ff, 0 0 20px #303030; /* 藍黑霓虹燈效果 */
            }

                .main .btn:hover {
                    box-shadow: 0 0 50px #3399ff, 0 0 40px #303030; /* 懸停時的霓虹燈效果放大 */
                }

            .main a {
                color: #3399ff; /* 藍色 */
                text-decoration: none;
                font-size: 0.9em;
                transition: color 0.3s ease-in-out;
            }

                .main a:hover {
                    color: #80d4ff; /* 更亮的藍色 */
                }

            .main .extra-links {
                margin-top: 20px;
                color: #ffffff;
            }
    </style>


</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="ScriptManager" />
        <div class="main">
            <label for="chk" aria-hidden="true">登入</label>
            <asp:TextBox ID="acc" placeholder="請輸入帳號" runat="server" required="required"></asp:TextBox>

            <asp:TextBox ID="pwd" placeholder="請輸入密碼" runat="server" TextMode="Password" required="required"></asp:TextBox>
            <asp:Button ID="btn_login" runat="server" Text="登入" CssClass="btn" OnClick="btn_login_Click" />

            <div class="extra-links">
                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/page/forgetpwd.aspx">忘記密碼?</asp:HyperLink><br />
                <asp:Label ID="Label1" runat="server" Text="沒有帳號嗎?" ForeColor="White"></asp:Label>
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/page/Register.aspx">立即註冊加入會員</asp:HyperLink><br />
                <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/page/coach_apply.aspx">加入NPTU GYM健身教練</asp:HyperLink><br /><br />
                <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/page/Home.aspx">返回首頁</asp:HyperLink>
            </div>
        </div>
    </form>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js" type="text/javascript"></script>
</body>
</html>
