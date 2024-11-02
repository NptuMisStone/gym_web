<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="Coach_index.aspx.cs" Inherits="Coach_Coach_home1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        /* 提示文字樣式 */
        .tooltip-text {
            display: none; /* 初始隱藏 */
            color: red; /* 提示文字顏色為紅色 */
            font-size: 18px; /* 字體大小 */
            position: absolute; /* 使用絕對定位 */
            top: 75%; /* 垂直居中 */
            left: 50%; /* 水平居中 */
            transform: translate(-50%, -50%); /* 移動到中心 */
            white-space: nowrap; /* 不換行 */
        }

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
        /* 編輯容器，圖片包裹 */
        .edit-container {
            position: relative;
            display: inline-block;
        }

        /* 編輯圖示默認隱藏，設置透明度淡入淡出效果 */
        .edit-icon {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            width: 40px;
            height: 40px;
            opacity: 0;
            transition: opacity 0.3s ease;
        }

        /* 圓形圖片 */
        .circular-image {
            transition: background-color 0.3s ease, filter 0.3s ease;
        }

        /* 當鼠標懸停在圖片上時顯示編輯圖示和灰色背景 */
        .edit-container:hover .circular-image {
            background-color: rgba(0, 0, 0, 0.5);
            filter: brightness(50%);
        }

        /* 鼠標懸停時顯示編輯圖標，淡入效果 */
        .edit-container:hover .edit-icon {
            opacity: 1;
        }
    </style>
    <!-- Page Header Start -->
    <div class="container-fluid page-header mb-5">
        <div class="d-flex flex-column align-items-center justify-content-center pt-0" style="min-height: 400px">
            <!-- 個人圖片，點擊導向到 coach_info.aspx -->
            <a href="/Coach/Coach_info.aspx" class="edit-container">
                <!-- 圖片 -->
                <asp:Image ID="coach_img" runat="server" CssClass="circular-image" Style="border-radius: 50%; width: 200px; height: 200px; object-fit: cover;" draggable="false" />
                <!-- 隱藏的 edit 圖片 -->
                <img src="img/edit.png" alt="Edit Icon" class="edit-icon" />
            </a>

            <h1 class='text-white font-weight-bold mt-3 mb-3'>
                <asp:Label ID="CoachName" runat="server"></asp:Label>
            </h1>

            <asp:Button ID="Btn_logout" runat="server" Text="登出" OnClick="Btn_logout_Click" class="btn btn-outline-primary px-4 py-2" />
        </div>
    </div>
    <!-- Page Header End -->

    <!-- Blog Detail Start -->
    <div class="container">
        <div class="row">
            <div class="col-12 pt-2">
                <!-- 將 pt-4 改成 pt-2，減少上方距離 -->
                <div id="divStatusBlock" runat="server" class="d-flex justify-content-center align-items-center bg-secondary text-white mb-3 p-4">
                    <!-- 減少外邊距和內邊距 -->
                    <div class="row w-100">
                        <!-- 左邊圖示區塊，佔2份 -->
                        <div class="col-md-2 d-flex align-items-center justify-content-center">
                            <asp:Image ID="img_Status" runat="server" Style="width: 100px; height: 100px;" draggable="false" />
                        </div>
                        <!-- 中間文字區塊，佔6份 -->
                        <div class="col-md-6 d-flex align-items-center">
                            <div class="media-body">
                                <asp:Label ID="lblReviewStatus" runat="server" class="text-primary mb-2" Font-Size="X-Large"></asp:Label><br />
                                <asp:Label ID="lblReviewStatusText" runat="server" class="m-0"></asp:Label>
                            </div>
                        </div>
                        <!-- 右邊按鈕區塊，佔4份 -->
                        <div class="col-md-4 d-flex justify-content-end align-items-center">
                            <div>
                                <asp:Button ID="btn_verify" runat="server" Text="立即驗證" CssClass="btn btn-outline-primary mt-1 px-4 py-2 btn-lg" OnClick="btn_verify_Click" />
                            </div>
                            <div id="EndDate" runat="server" visible="false" class="ms-3">
                                <div>合約到期日</div>
                                <!-- 將 mb-3 改成 mb-2，縮小文字區塊的外邊距 -->
                                <asp:Label ID="lblContractEndDate" runat="server" CssClass="m-0" Font-Size="X-Large"></asp:Label>
                            </div>
                        </div>
                        <div class="container-fluid pt-2">
                            <asp:Repeater ID="rp_coach" runat="server" OnItemDataBound="rp_coach_ItemDataBound">
                                <ItemTemplate>
                                    <asp:Panel ID="Panel_store" runat="server">
                                        <div class="row mt-2  mb-0">
                                            <!-- 服務地點名稱 -->
                                            <div class="col-sm-6 col-md-3">
                                                <div class="d-flex align-items-start">
                                                    <img src="img/home.png" class="mr-2" style="height: 30px; flex-shrink: 0;" draggable="false">
                                                    <div style="display: flex; flex-direction: column;">
                                                        <h5 class="font-weight-bold mb-1 text-white" style="white-space: nowrap;">服務店家</h5>
                                                        <p class="mb-0"><%# Eval("服務地點名稱") %></p>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- 服務地點地址 -->
                                            <div class="col-sm-6 col-md-3">
                                                <div class="d-flex align-items-start">
                                                    <img src="img/maps.png" class="mr-2" style="height: 30px; flex-shrink: 0;" draggable="false">
                                                    <div style="display: flex; flex-direction: column;">
                                                        <h5 class="font-weight-bold mb-1 text-white" style="white-space: nowrap;">店家地址</h5>
                                                        <p class="mb-0"><%# Eval("縣市") %><%# Eval("行政區") %><%# Eval("服務地點地址") %></p>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- 服務地點電話 -->
                                            <div class="col-sm-6 col-md-3">
                                                <div class="d-flex align-items-start">
                                                    <img src="img/telephone.png" class="mr-2" style="height: 30px; flex-shrink: 0;" draggable="false">
                                                    <div style="display: flex; flex-direction: column;">
                                                        <h5 class="font-weight-bold mb-1 text-white" style="white-space: nowrap;">店家電話</h5>
                                                        <p class="mb-0" style="white-space: normal; word-wrap: break-word; word-break: break-all;">
                                                            <%# Eval("服務地點電話") %>
                                                        </p>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- 服務地點郵件 -->
                                            <div class="col-sm-6 col-md-3">
                                                <div class="d-flex align-items-start">
                                                    <img src="img/email.png" class="mr-2" style="height: 30px; flex-shrink: 0;" draggable="false">
                                                    <div style="display: flex; flex-direction: column;">
                                                        <h5 class="font-weight-bold mb-1 text-white" style="white-space: nowrap;">店家郵件</h5>
                                                        <p class="mb-0" style="white-space: normal; word-wrap: break-word; word-break: break-all;">
                                                            <%# Eval("服務地點郵件") %>
                                                        </p>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Blog Detail End -->


    <!-- Horizontal Large Buttons -->
    <div id="Coach_Btn" runat="server" class="container text-center" visible="false">
        <div class="row justify-content-center">
            <!-- 預約紀錄按鈕 -->
            <div class="col-md-3">
                <asp:HyperLink ID="HyperLink_ViewAppointments" runat="server" NavigateUrl="/Coach/Coach_appointment.aspx" CssClass="custom-button">
                <div class="icon">
                    <img src="img/C4.png" alt="Appointment Icon" />
                </div>
                <div class="text">
                    預約紀錄
            </div>
                </asp:HyperLink>
            </div>

            <!-- 教練班表按鈕 -->
            <div class="col-md-3">
                <asp:HyperLink ID="HyperLink_ViewSchedule" runat="server" NavigateUrl="/Coach/Coach_schedule.aspx" CssClass="custom-button">
                <div class="icon">
                    <img src="img/C2.png" alt="Schedule Icon" />
                </div>
                <div class="text">
                    教練班表
                </div>
                </asp:HyperLink>
            </div>

            <!-- 課程維護按鈕 -->
            <div class="col-md-3">
                <asp:HyperLink ID="HyperLink_ClassMaintenance" runat="server" NavigateUrl="/Coach/Coach_class.aspx" CssClass="custom-button">
                <div class="icon">
                    <img src="img/C3.png" alt="Class Maintenance Icon" />
                </div>
                <div class="text">
                    課程維護
                </div>
                </asp:HyperLink>
            </div>

            <!-- 評論管理按鈕 -->
            <div class="col-md-3">
                <asp:HyperLink ID="HyperLink_Comment" runat="server" NavigateUrl="/Coach/Coach_Comment.aspx" CssClass="custom-button">
                <div class="icon">
                    <img src="img/C5.png" alt="Comment Icon" />
                </div>
                <div class="text">
                    評價管理
                </div>
                </asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>
