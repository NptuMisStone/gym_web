<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Admin_login.aspx.cs" Inherits="Admin_Admin_login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>登入頁面</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f2f2f2; /* 將背景色設為淡灰色 */
            margin: 0;
            padding: 0;
        }

        .container {
            width: 300px;
            margin: 0 auto;
            margin-top: 100px;
            background-color: #fff;
            border: 1px solid #ccc;
            padding: 20px;
            text-align: center;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }

        h2 {
            color: #333;
        }

        .input-group {
            margin-bottom: 15px;
        }

            .input-group label {
                display: block;
                text-align: left;
                margin-bottom: 5px;
                color: #555;
                font-weight: bold;
            }

        .input-text {
            width: 80%;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 5px;
            outline: none;
        }

        .btn-login {
            background-color: #4CAF50;
            color: white;
            padding: 10px 15px;
            border: none;
            cursor: pointer;
            border-radius: 5px;
        }

            .btn-login:hover {
                background-color: #45a045;
            }

        .register-link {
            margin-top: 10px;
            color: #333;
        }

        a {
            text-decoration: none;
            color: #007bff;
        }

            a:hover {
                text-decoration: underline;
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
            //等1.5秒後再跳頁
            setTimeout(function () {
                window.location.href = 'Admin_index.aspx';
            }, 1500);
            
        };
        function showAlert2() {
            Swal.fire({
                title: '登入失敗!',
                text: '帳號或密碼錯誤',
                icon: 'error',
                confirmButtonText: 'OK'
            });
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>管理員登入</h2>
            <div class="input-group">
                <label for="txtAccount">帳號：</label>
                <asp:TextBox ID="Admin_Account" runat="server"></asp:TextBox>
            </div>
            <div class="input-group">
                <label for="txtPassword">密碼：</label>
                <asp:TextBox ID="Admin_Password" runat="server" TextMode="Password"></asp:TextBox>
            </div>
            <asp:Button ID="Button1" runat="server" Text="登入→" OnClick="login" />
        </div>
    </form>
</body>
</html>
