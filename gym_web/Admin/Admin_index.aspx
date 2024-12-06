<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Admin_index.aspx.cs" Inherits="Admin_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>管理員主頁</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background: url('/page/img/bg.jpg') no-repeat center center fixed; /* 使用圖片作為背景 */
            background-size: cover;
            margin: 0;
            padding: 0;
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: flex-start;
            height: 100vh;
            overflow: hidden;
        }

        .header {
            width: 100%;
            max-width: 1400px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 60px 40px; /* 加大頂部距離 */
            background-color: rgba(128, 128, 128, 0.9); /* 修改為灰色半透明背景 */
            border-radius: 10px;
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
            margin-top: 30px; /* 增加整個 header 與頁面頂端的距離 */
            margin-bottom: 20px;
            position: sticky;
            top: 0;
        }



        .header img {
            height: 50px;
            max-width: 100%;
        }

        .logout-btn {
    background-color: #e74c3c;
    color: #ffffff;
    padding: 10px 40px; /* 加大右側距離 */
    font-size: 14px;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    text-transform: uppercase;
    transition: background-color 0.3s;
    text-decoration: none; /* 去掉底線 */
}

.btn {
    background-color: #4CAF50;
    color: #ffffff;
    padding: 15px;
    font-size: 16px;
    border: none;
    border-radius: 10px;
    cursor: pointer;
    text-transform: uppercase;
    transition: background-color 0.3s, transform 0.3s;
    text-align: center;
    display: inline-block;
    text-decoration: none; /* 去掉底線 */
}


        .logout-btn:hover {
            background-color: #c0392b;
        }

        .container {
            width: 90vw;
            max-width: 1400px; /* 最大寬度增加，滿足大螢幕需求 */
            background-color: rgba(255, 255, 255, 0.95); /* 白色半透明背景 */
            border-radius: 20px; /* 圓角框 */
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
            padding: 40px;
            display: flex;
            flex-direction: row;
            gap: 20px;
            overflow: auto;
        }

        .info-panel {
            flex: 1;
            background-color: rgba(245, 245, 245, 0.9);
            border-radius: 10px;
            padding: 20px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        }

        .info-panel h2 {
            font-size: 18px;
            font-weight: bold;
            color: #333;
            margin-bottom: 15px;
        }

        .info-panel h3 {
            font-size: 16px;
            font-weight: bold;
            color: #333;
            margin-bottom: 5px;
        }

        .info-panel .stat {
            display: block;
            font-size: 14px;
            color: #555;
            margin-bottom: 15px;
        }

        .button-group {
            flex: 1;
            display: flex;
            flex-direction: column;
            justify-content: center;
            gap: 20px;
        }

        .btn {
            background-color: #4CAF50;
            color: #ffffff;
            padding: 15px;
            font-size: 16px;
            border: none;
            border-radius: 10px;
            cursor: pointer;
            text-transform: uppercase;
            transition: background-color 0.3s, transform 0.3s;
            text-align: center;
            display: inline-block;
        }

        .btn:hover {
            background-color: #45a045;
            transform: scale(1.05);
        }

        @media (max-width: 480px) {
            .container {
                flex-direction: column;
                padding: 30px;
            }

            .btn {
                font-size: 14px;
                padding: 12px;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <img src="/page/img/main_logo.png" alt="主Logo">
            <button type="button" class="logout-btn" onclick="window.location.href='Admin_login.aspx'">登出</button>

        </div>
        <div class="container">
            <div class="info-panel">
                <h2>廣告</h2>
                <h3>有效廣告</h3>
                <asp:Label ID="lblActiveAds" runat="server" CssClass="stat">Loading...</asp:Label>
                
                <h3>未來廣告</h3>
                <asp:Label ID="lblFutureAds" runat="server" CssClass="stat">Loading...</asp:Label>
                
                <h3>過期廣告</h3>
                <asp:Label ID="lblExpiredAds" runat="server" CssClass="stat">Loading...</asp:Label>
            </div>
            <div class="info-panel">
                <h2>健身教練</h2>
                <h3>待審核</h3>
                <asp:Label ID="lblPendingCoaches" runat="server" CssClass="stat">Loading...</asp:Label>
                
                <h3>合約內</h3>
                <asp:Label ID="lblActiveCoaches" runat="server" CssClass="stat">Loading...</asp:Label>
                
                <h3>已到期</h3>
                <asp:Label ID="lblExpiredCoaches" runat="server" CssClass="stat">Loading...</asp:Label>
            </div>
            <div class="info-panel">
                <h2>評論</h2>
                <h3>待審核</h3>
                <asp:Label ID="lblPendingComments" runat="server" CssClass="stat">Loading...</asp:Label>
            </div>
            <div class="button-group">
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Admin/coach_review.aspx" CssClass="btn">健身教練管理</asp:HyperLink>
                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Admin/AD.aspx" CssClass="btn">廣告管理</asp:HyperLink>
                <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Admin/comment_review.aspx" CssClass="btn">評論管理</asp:HyperLink>
            </div>
        </div>
    </form>
</body>
</html>