<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Coach_login.aspx.cs" Inherits="Coach_Coach_login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>教練登入</title>
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
    justify-content: flex-start; /* 水平置左 */
    align-items: center; /* 垂直置中 */
    padding: 30px; /* 為了讓內容不靠太近邊緣 */
    overflow: hidden; /* 隱藏滾動條 */
}

.main {
    width: 100%;
    max-width: 400px;
    background: rgba(255, 255, 255, 0.1); /* 半透明背景 */
    backdrop-filter: blur(10px); /* 霧面玻璃效果 */
    border-radius: 15px;
    padding: 30px;
    text-align: left; /* 置左對齊 */
    max-height: calc(100% - 100px); /* 控制主區塊最大高度，避免超出視窗 */
    overflow-y: auto; /* 若內容超出主區塊，顯示滾動條 */
}

.main label {
    font-size: 2em;
    color: #ff0000; /* 改為單一的紅色 */
    text-shadow: 0px 0px 5px rgba(0, 0, 0, 0.5);
    margin-bottom: 20px;
    display: block;
    font-weight: bold; /* 加粗文字 */
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
    box-shadow: 0 0 10px #ff4d4d, 0 0 20px #606060; /* 聚焦時霓虹燈效果 */
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
    color: #ff4d4d; /* 使用更亮的紅色 */
    text-decoration: none;
    font-size: 0.9em;
    transition: color 0.3s ease-in-out;
}

.main a:hover {
    color: #ff80ff; /* 改為更亮的紫紅色 */
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
            <label for="chk" aria-hidden="true">教練登入</label>
            <asp:TextBox ID="coach_acc" placeholder="請輸入帳號" runat="server" required="required"></asp:TextBox>
            <asp:TextBox ID="coach_pwd" placeholder="請輸入密碼" runat="server" TextMode="Password" required="required"></asp:TextBox>
            <asp:Button ID="btn_coach_login" runat="server" Text="登入" CssClass="btn" OnClick="btn_coach_login_Click" />

            <div class="extra-links">
                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Coach/Coach_forgetpwd.aspx">忘記密碼?</asp:HyperLink><br />
                <asp:Label ID="Label1" runat="server" Text="沒有帳號嗎?" ForeColor="White"></asp:Label>
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Coach/Coach_register.aspx">立即註冊教練帳號</asp:HyperLink><br />
                <asp:Label ID="Label2" runat="server" Text="切換為會員：" ForeColor="White"></asp:Label>
                <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/User/User_login.aspx">立即切換</asp:HyperLink>
            </div>
        </div>
    </form>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js" type="text/javascript"></script>
</body>
</html>
