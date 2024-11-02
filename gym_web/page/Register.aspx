<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="page_register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>會員註冊</title>
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
            overflow-y: auto;
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
            .main input[type="password"],
            .main input[type="email"] {
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
                .main input[type="password"]::placeholder,
                .main input[type="email"]::placeholder {
                    color: #bbb;
                }

                .main input[type="text"]:focus,
                .main input[type="password"]:focus,
                .main input[type="email"]:focus {
                    background: rgba(255, 255, 255, 0.5);
                    box-shadow: 0 0 10px #3399ff, 0 0 20px #606060; /* 聚焦時霓虹燈效果 */
                    outline: none;
                }

        .radioButtonList {
            display: flex; /* 使所有項目以 flex 排列 */
            align-items: center; /* 確保所有項目在垂直方向上對齊 */
            gap: 30px; /* 控制每個選項的間距 */
            justify-content: center; /* 確保在容器中居中 */
            margin: 20px 0; /* 添加一些上下邊距 */
        }

            .radioButtonList label {
                font-size: 18px;
                color: #fff; /* 根據需要調整顏色 */
                margin: 0; /* 移除外邊距 */
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
                box-shadow: 0 0 50px #3399ff, 0 0 40px #303030; /* 懸停時霓虹燈效果放大 */
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

        .captcha-container {
            display: flex;
            align-items: center;
            gap: 10px; /* 控制驗證碼圖片和輸入框之間的間距 */
        }
    </style>
    <script>
        function getCaptchaFromServer(callback) {
            $.ajax({
                type: "POST",
                url: "coach_apply.aspx/GetCaptchaText",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    callback(response.d);
                },
                error: function (error) {
                    console.log("Error getting captcha from server:", error);
                }
            });
        }

        function updateCaptchaImage() {
            getCaptchaFromServer(function (captcha) {
                var captchaImage = document.getElementById('imgCaptcha');
                captchaImage.src = "<%= ResolveUrl("~/Captcha.ashx") %>?t=" + new Date().getTime();
            });
        }

        window.onload = function () {
            updateCaptchaImage();
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="ScriptManager" />
        <div class="main">
            <label aria-hidden="true">會員註冊</label>
            <asp:TextBox ID="tb_user_name" placeholder="請輸入姓名" runat="server" required="required"></asp:TextBox>
            <asp:TextBox ID="tb_user_acc" placeholder="請輸入帳號" runat="server" required="required"></asp:TextBox>
            <asp:TextBox ID="tb_user_pwd" placeholder="請輸入密碼" runat="server" TextMode="Password" required="required"></asp:TextBox>
            <asp:TextBox ID="tb_user_pwd2" placeholder="再次輸入密碼" runat="server" TextMode="Password" required="required"></asp:TextBox>
            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="密碼不一致" ControlToCompare="tb_user_pwd" ControlToValidate="tb_user_pwd2" ForeColor="Red" SetFocusOnError="True" Display="Dynamic"></asp:CompareValidator>

            <asp:CustomValidator ID="CustomValidator1" runat="server"
                ErrorMessage="密碼長度至少6個字元且不可與帳號相同"
                ControlToValidate="tb_user_pwd"
                ForeColor="Red"
                SetFocusOnError="True"
                OnServerValidate="CustomValidator1_ServerValidate" Display="Dynamic"></asp:CustomValidator>

            <asp:TextBox ID="tb_phone" placeholder="請輸入電話" runat="server" required="required"></asp:TextBox>
            <asp:RegularExpressionValidator ID="revPhone" runat="server"
                ControlToValidate="tb_phone"
                ErrorMessage="請輸入正確的聯絡電話（格式: 09XXXXXXXX）"
                ForeColor="Red"
                ValidationExpression="^09\d{8}$"
                SetFocusOnError="True" Display="Dynamic"></asp:RegularExpressionValidator>

            <asp:TextBox ID="tb_email" placeholder="請輸入email" runat="server" required="required" TextMode="Email"></asp:TextBox>
            <asp:RegularExpressionValidator ID="revEmail" runat="server"
                ControlToValidate="tb_email"
                ErrorMessage="請輸入正確的電子郵件地址"
                ForeColor="Red"
                ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                SetFocusOnError="True" Display="Dynamic"></asp:RegularExpressionValidator>

            <asp:RadioButtonList ID="tb_gender" runat="server" CssClass="radioButtonList" RepeatDirection="Horizontal" RepeatLayout="Flow">
                <asp:ListItem Value="1" Selected="True">男生</asp:ListItem>
                <asp:ListItem Value="2">女生</asp:ListItem>
                <asp:ListItem Value="3">不願透露</asp:ListItem>
            </asp:RadioButtonList>

            <div class="captcha-container">
                <img id="imgCaptcha" src="" style="cursor: pointer;" onclick="updateCaptchaImage();" />
                <img id="refresh" src="img/refresh.png" style="cursor: pointer; height:20px;" onclick="updateCaptchaImage();" />
                <asp:TextBox runat="server" ID="txtAnswer" placeholder="請輸入驗證碼" />
                <asp:RequiredFieldValidator ID="revCaptcha" runat="server"
                    ControlToValidate="txtAnswer"
                    ErrorMessage="驗證碼不可為空"
                    ForeColor="Red"
                    ValidationGroup="vg1"
                    Display="Dynamic"></asp:RequiredFieldValidator>
            </div>

            <asp:Button ID="btn_user_login" runat="server" Text="註冊" OnClick="btn_user_login_Click" CssClass="btn" />

            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/page/Login.aspx">返回登入頁面</asp:HyperLink>
        </div>
    </form>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js" type="text/javascript"></script>
</body>
</html>
