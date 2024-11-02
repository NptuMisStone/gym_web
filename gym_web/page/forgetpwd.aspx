<%@ Page Language="C#" AutoEventWireup="true" CodeFile="forgetpwd.aspx.cs" Inherits="page_forgetpwd" %>

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
        /* 通用樣式設置 */
        body, html {
            height: 100%;
            margin: 0;
            font-family: 'Jost', sans-serif;
            background: linear-gradient(to right, rgba(36, 36, 36, 0.7), rgba(36, 36, 36, 0.7)), url('img/bg.jpg') no-repeat center center fixed;
            background-size: cover;
            display: flex;
            justify-content: center; /* 水平置右 */
            align-items: center; /* 垂直置中 */
            padding: 30px; /* 為了讓內容不靠太近邊緣 */
            overflow: hidden; /* 隱藏滾動條 */
        }

        .main {
            width: 100%;
            max-width: 400px;
            background: rgba(255, 255, 255, 0.1);
            backdrop-filter: blur(10px);
            border-radius: 15px;
            padding: 30px;
            text-align: left;
            max-height: calc(100% - 100px);
            overflow-y: auto;
            box-sizing: border-box;
        }

            .main label {
                font-size: 2em;
                color: #007bff; /* 藍色 */
                text-shadow: 0px 0px 5px rgba(0, 0, 0, 0.5); /* 文字陰影 */
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
                background: rgba(255, 255, 255, 0.3); /* 半透明白色 */
                color: #ffffff;
                box-shadow: inset 0 0 10px rgba(0, 0, 0, 0.5); /* 內部陰影效果 */
                transition: all 0.3s ease-in-out;
            }

                .main input[type="text"]::placeholder,
                .main input[type="password"]::placeholder {
                    color: #bbb; /* 預設文字顏色 */
                }

                .main input[type="text"]:focus,
                .main input[type="password"]:focus {
                    background: rgba(255, 255, 255, 0.5); /* 聚焦時背景變亮 */
                    box-shadow: 0 0 10px #007bff, 0 0 20px #606060; /* 聚焦時霓虹燈效果 */
                    outline: none; /* 移除外框 */
                }

            .main .btn {
                width: 100%;
                padding: 15px;
                margin: 20px 0;
                border: none;
                border-radius: 5px;
                box-sizing: border-box;
                font-size: 1em;
                background: linear-gradient(45deg, #007bff, #303030); /* 藍黑漸層顏色 */
                color: white;
                cursor: pointer; /* 顯示手型指標 */
                transition: all 0.3s ease-in-out;
                box-shadow: 0 0 10px #007bff, 0 0 20px #303030; /* 藍黑霓虹燈效果 */
            }

                .main .btn:hover {
                    box-shadow: 0 0 50px #007bff, 0 0 40px #303030; /* 懸停時的霓虹燈效果放大 */
                }

            .main a {
                color: #007bff !important; /* 藍色連結 */
                text-decoration: none;
                font-size: 0.9em;
                transition: color 0.3s ease-in-out;
            }

                .main a:hover {
                    color: #80d4ff !important; /* 懸停時變淺藍色 */
                }

            .main .extra-links {
                margin-top: 20px;
                color: #ffffff; /* 白色字體 */
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
                        <asp:Button ID="Btn_step1" runat="server" Text="獲取驗證碼" OnClick="Btn_step1_Click" CssClass="btn" OnClientClick="ShowProgressBar();" />
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/page/Login.aspx" ForeColor="White">返回登入頁面</asp:HyperLink>
                    </div>
                </div>
            </asp:Panel>

            <!-- Step 2: 驗證碼輸入 -->
            <asp:Panel ID="Panel2" runat="server" Visible="false">
                <div class="signup">
                    <div class="custom-scrollbar">
                        <label for="chk" aria-hidden="true">驗證信已寄出</label>
                        <asp:Label ID="Label2" runat="server" ForeColor="White" Text="請輸入驗證碼（有效期限：3分鐘）"></asp:Label>
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

    <!-- LOADING進度條 START-->
    <div id="divProgress" style="text-align: center; display: none; position: fixed; top: 50%; left: 50%;">
        <asp:Image ID="imgLoading" runat="server" ImageUrl="~/page/img/loading.gif" />
        <br />
        <font color="#1B3563" size="2px">資料處理中</font>
    </div>
    <div id="divMaskFrame" style="background-color: #F2F4F7; display: none; left: 0px; position: absolute; top: 0px;">
    </div>
    <script>
        // 顯示讀取遮罩
        function ShowProgressBar() {
            displayProgress();
            displayMaskFrame();
        }

        // 隱藏讀取遮罩
        function HideProgressBar() {
            var progress = $('#divProgress');
            var maskFrame = $("#divMaskFrame");
            progress.hide();
            maskFrame.hide();
        }
        // 顯示讀取畫面
        function displayProgress() {
            var w = $(document).width();
            var h = $(window).height();
            var progress = $('#divProgress');
            progress.css({ "z-index": 999999, "top": (h / 2) - (progress.height() / 2), "left": (w / 2) - (progress.width() / 2) });
            progress.show();
        }
        // 顯示遮罩畫面
        function displayMaskFrame() {
            var w = $(window).width();
            var h = $(document).height();
            var maskFrame = $("#divMaskFrame");
            maskFrame.css({ "z-index": 999998, "opacity": 0.7, "width": w, "height": h });
            maskFrame.show();
        }
    </script>
    <!-- LOADING進度條 END-->
</body>
</html>
