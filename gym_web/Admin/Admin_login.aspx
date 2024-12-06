<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Admin_login.aspx.cs" Inherits="Admin_Admin_login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>登入頁面</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background: url('/page/img/bg.jpg') no-repeat center center fixed; /* 使用圖片作為背景 */
            background-size: cover;
            margin: 0;
            padding: 0;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }

        .container {
            width: 100%;
            max-width: 400px;
            background-color: rgba(0, 0, 0, 0.7); /* 深色半透明背景 */
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.5);
            padding: 30px;
            text-align: center;
        }

        .logo {
            margin-bottom: 20px;
        }

        .logo img {
            width: 200px; /* 調整 logo 大小 */
            height: auto;
        }

        h2 {
            color: #ffffff;
            margin-bottom: 20px;
        }

        .input-group {
            margin-bottom: 20px;
            text-align: left;
        }

        .input-group label {
            display: block;
            font-size: 14px;
            color: #ffffff;
            font-weight: 600;
            margin-bottom: 5px;
        }

        .input-text {
            width: 100%;
            padding: 12px;
            border: 1px solid #555555;
            border-radius: 5px;
            font-size: 14px;
            outline: none;
            transition: border-color 0.3s;
            box-sizing: border-box; /* 確保寬度一致 */
        }

        .input-text:focus {
            border-color: #4CAF50;
        }

        .btn-login {
            width: 100%;
            background-color: #4CAF50;
            color: #ffffff;
            padding: 12px;
            font-size: 16px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            transition: background-color 0.3s;
        }

        .btn-login:hover {
            background-color: #45a045;
        }

        @media (max-width: 480px) {
            .container {
                padding: 20px;
            }

            .logo img {
                width: 150px;
            }
        }
    </style>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script>
        function showAlert1() {
            Swal.fire({
                title: '登入成功',
                text: '歡迎你!管理員',
                icon: 'success',
                confirmButtonText: 'OK'
            });
            setTimeout(function () {
                window.location.href = 'Admin_index.aspx';
            }, 1500);
        }

        function showAlert2() {
            Swal.fire({
                title: '登入失敗!',
                text: '帳號或密碼錯誤',
                icon: 'error',
                confirmButtonText: 'OK'
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="logo">
                <img src="/page/img/main_logo.png" alt="主標誌" />
            </div>
            <h2>管理員登入</h2>
            <div class="input-group">
                <label for="Admin_Account">帳號：</label>
                <asp:TextBox ID="Admin_Account" runat="server" CssClass="input-text"></asp:TextBox>
            </div>
            <div class="input-group">
                <label for="Admin_Password">密碼：</label>
                <asp:TextBox ID="Admin_Password" runat="server" TextMode="Password" CssClass="input-text"></asp:TextBox>
            </div>
            <asp:Button ID="Button1" runat="server" Text="登入" OnClick="login" CssClass="btn-login" />
        </div>
    </form>
</body>
</html>