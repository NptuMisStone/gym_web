<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Coach_forgetpwd.aspx.cs" Inherits="Coach_Coach_forgetpwd" EnableEventValidation="false" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>忘記密碼</title>
    <link href="https://fonts.googleapis.com/css2?family=Jost:wght@500&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.6/dist/sweetalert2.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.6/dist/sweetalert2.all.min.js"></script>
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <style>
        body, html {
            height: 100%;
            margin: 0;
            font-family: 'Jost', sans-serif;
            background: linear-gradient(to right, rgba(36, 36, 36, 0.7), rgba(36, 36, 36, 0.7)), url('img/bg.jpg') no-repeat center center fixed;
            background-size: cover;
            display: flex; /* 使用 flex 進行排版 */
            justify-content: center; /* 水平置中 */
            align-items: center; /* 垂直置中 */
            overflow-y: auto; /* 如果內容超過頁面高度，允許滾動 */
        }

        .main {
            width: 100%;
            max-width: 400px; /* 最大寬度 */
            background: rgba(255, 255, 255, 0.1); /* 半透明背景 */
            backdrop-filter: blur(10px); /* 霧面玻璃效果 */
            border-radius: 15px;
            padding: 30px;
            text-align: left; /* 置左對齊 */
            margin: 0 auto; /* 水平居中 */
            box-shadow: 0 0 15px rgba(0, 0, 0, 0.3); /* 添加一些陰影以突出元素 */
            box-sizing: border-box;
        }

            .main label {
                font-size: 2em;
                color: #ff0000;
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
                    box-shadow: 0 0 10px #ff4d4d, 0 0 20px #606060;
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
                background: linear-gradient(45deg, #e31c25, #303030); /* 原始漸層顏色 */
                color: white;
                cursor: pointer; /* 顯示手型指標 */
                transition: all 0.3s ease-in-out;
                box-shadow: 0 0 10px #e31c25, 0 0 20px #303030; /* 原始霓虹燈效果 */
            }

                .main .btn:hover {
                    box-shadow: 0 0 50px #e31c25, 0 0 40px #303030; /* 懸停時的霓虹燈效果放大 */
                }

            .main a {
                color: #ff4d4d !important; /* 使用更亮的紅色 */
                text-decoration: none;
                font-size: 0.9em;
                transition: color 0.3s ease-in-out;
            }

                .main a:hover {
                    color: #ff80ff !important; /* 改為更亮的紫紅色 */
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
            <!-- Step 1: 輸入帳號和Email -->
            <asp:Panel ID="Panel1" runat="server" Visible="true">

                <div class="signup">
                    <div class="custom-scrollbar">
                        <label for="chk" aria-hidden="true">忘記密碼</label>
                        <asp:Label ID="Label6" runat="server" ForeColor="White" Text="請輸入您的帳號"></asp:Label>
                        <asp:TextBox ID="tb_acc" runat="server" required="required" placeholder="請輸入帳號"></asp:TextBox>
                        <asp:Label ID="Label1" runat="server" ForeColor="White" Text="請輸入您的E-mail"></asp:Label>
                        <asp:TextBox ID="tb_email" runat="server" required="required" placeholder="請輸入E-mail"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revEmail" runat="server"
                            ControlToValidate="tb_email"
                            ErrorMessage="請輸入正確的電子郵件地址"
                            ForeColor="Red"
                            ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                            SetFocusOnError="True" Display="Dynamic"></asp:RegularExpressionValidator>
                        <asp:Button ID="Btn_step1" runat="server" Text="獲取驗證碼" OnClick="Btn_step1_Click" CssClass="btn" />
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Coach/Coach_login.aspx" ForeColor="White">返回登入頁面</asp:HyperLink>
                    </div>
                </div>
            </asp:Panel>

            <!-- Step 2: 驗證碼輸入 -->
            <asp:Panel ID="Panel2" runat="server" Visible="false">
                <div class="signup">
                    <div class="custom-scrollbar">
                        <label for="chk" aria-hidden="true">驗證信已寄出</label>
                        <asp:Label ID="Label2" runat="server" ForeColor="White" Text="請輸入驗證碼"></asp:Label>
                        <asp:TextBox ID="tb_pwd1" runat="server" required="required" placeholder="請輸入驗證碼"></asp:TextBox>
                        <asp:Button ID="Btn_step2" runat="server" Text="確認" CssClass="btn" OnClick="Btn_step2_Click" />
                    </div>
                </div>
            </asp:Panel>

            <!-- Step 3: 重設密碼 -->
            <asp:Panel ID="Panel3" runat="server" Visible="false">
                <div class="signup">
                    <div class="custom-scrollbar">
                        <label for="chk" aria-hidden="true">重設密碼</label>
                        <asp:Label ID="Label3" runat="server" ForeColor="White" Text="請輸入新密碼"></asp:Label>
                        <asp:TextBox ID="tb_new_pwd" runat="server" TextMode="Password" placeholder="請輸入新密碼" required="required"></asp:TextBox>
                        <asp:Label ID="Label4" runat="server" ForeColor="White" Text="請再次輸入新密碼"></asp:Label>
                        <asp:TextBox ID="tb_new_pwd2" runat="server" TextMode="Password" placeholder="請再次輸入新密碼" required="required"></asp:TextBox>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="密碼不一致" ControlToCompare="tb_new_pwd" ControlToValidate="tb_new_pwd2" SetFocusOnError="True" ForeColor="Red" Display="Dynamic"></asp:CompareValidator>
                        <asp:CustomValidator ID="CustomValidator1" runat="server"
                            ErrorMessage="密碼長度至少6個字元且不可與帳號相同"
                            ControlToValidate="tb_new_pwd"
                            ForeColor="Red"
                            SetFocusOnError="True"
                            OnServerValidate="CustomValidator1_ServerValidate" Display="Dynamic"></asp:CustomValidator>
                        <asp:Button ID="Btn_step3" runat="server" Text="送出" CssClass="btn" OnClick="Btn_step3_Click" />
                    </div>
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
