<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="User_index.aspx.cs" Inherits="User_User_index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style>
        .custom-button {
    display: flex;
    flex-direction: column; /* 圖示在上，文字在下 */
    border: 4px solid black;
    text-align: center;
    text-decoration: none; /* 移除超連結底線 */
    width: 100%; /* 讓按鈕適應容器寬度 */
    max-width: 250px; /* 控制按鈕最大寬度 */
    margin: auto; /* 確保按鈕在容器內水平居中 */
}

    .custom-button:hover {
        text-decoration: none; /* 確保懸停時沒有底線 */
    }

    .custom-button .icon {
        width: 100%; /* 圖示區塊寬度填滿 */
        height: 150px; /* 調高圖示區塊高度 */
        display: flex;
        justify-content: center; /* 水平置中 */
        align-items: center; /* 垂直置中 */
        border-bottom: 2px solid black; /* 圖示與文字區域之間的線條 */
    }

        .custom-button .icon img {
            width: 90px; /* 放大圖示大小 */
            height: 90px;
        }

    .custom-button .text {
        background-color: black;
        color: white;
        padding: 20px; /* 增加內邊距 */
        font-weight: bold;
        font-size: 22px; /* 將文字變大 */
        flex-grow: 1; /* 確保文字區塊填滿剩餘空間 */
        display: flex;
        justify-content: center; /* 水平居中 */
        align-items: center; /* 垂直居中 */
    }

    .custom-button a {
        text-decoration: none; /* 確保超連結沒有底線 */
    }
    </style>
    <!-- Page Header Start -->
    <div class="container-fluid page-header mb-5">
        <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 130px">
        </div>
    </div>
    <!-- Page Header End -->

    <!-- Horizontal Large Buttons -->
    <div id="User_Btn" runat="server" class="container text-center">
        <div class="row justify-content-center">
            <!-- 使用者帳號檢視按鈕 -->
            <div class="col-md-3">
                <asp:HyperLink ID="HyperLink_UserInfo" runat="server" NavigateUrl="/User/User_info.aspx" CssClass="custom-button">
                    <div class="icon">
                        <img src="img/U1.png" alt="User Info Icon" />
                    </div>
                    <div class="text">
                        使用者帳號檢視
                    </div>
                </asp:HyperLink>
            </div>

            <!-- 預約紀錄按鈕 -->
            <div class="col-md-3">
                <asp:HyperLink ID="HyperLink_UserAppointment" runat="server" NavigateUrl="/User/User_appointment_record.aspx" CssClass="custom-button">
                    <div class="icon">
                        <img src="img/U2.png" alt="Appointment Record Icon" />
                    </div>
                    <div class="text">
                        預約紀錄
                    </div>
                </asp:HyperLink>
            </div>

            <!-- 我的收藏按鈕 -->
            <div class="col-md-3">
                <asp:HyperLink ID="HyperLink_UserLike" runat="server" NavigateUrl="/User/User_Like.aspx" CssClass="custom-button">
                    <div class="icon">
                        <img src="img/U3.png" alt="My Favorites Icon" />
                    </div>
                    <div class="text">
                        我的收藏
                    </div>
                </asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>
