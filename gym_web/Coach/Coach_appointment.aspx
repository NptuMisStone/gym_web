<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="Coach_appointment.aspx.cs" Inherits="Coach_Coach_appointment" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .course-btn {
            background-color: #007bff; /* 背景顏色 */
            color: white; /* 文字顏色 */
            border-radius: 50px; /* 圓角邊框 */
            padding: 10px 20px; /* 調整內邊距 */
            font-size: 1rem; /* 調整字體大小 */
            font-weight: bold; /* 使字體加粗 */
            border: none; /* 去掉預設邊框 */
            display: inline-block;
            text-align: center;
            cursor: pointer;
            transition: background-color 0.3s ease; /* 過渡效果 */
        }

            .course-btn:hover,
            .course-btn:focus {
                background-color: #28a745; /* 懸停或選取時的顏色 */
                color: white; /* 文字顏色 */
                text-decoration: none; /* 去掉下劃線 */
                outline: none; /* 去掉選取時的外框 */
            }

            .course-btn.active {
                background-color: #28a745; /* 活躍狀態背景顏色 */
                color: white; /* 文字顏色 */
            }

        .date-circle {
            display: inline-block;
            background-color: #333; /* 圓形背景顏色 */
            color: white; /* 文字顏色 */
            padding: 10px 20px; /* 調整內邊距 */
            border-radius: 50%; /* 圓形效果 */
            font-size: medium; /* 調整字體大小 */
            text-align: center; /* 文字居中 */
            width: 100px; /* 調整圓形寬度 */
            height: 100px; /* 調整圓形高度 */
            line-height: 100px; /* 調整文字垂直居中 */
            margin-bottom: 10px; /* 與其他元素的間距 */
        }
        /* 紅色樣式 */
        .course-card-red {
            border: 2px solid #e31c25; /* 邊框顏色 */
            border-radius: 10px;
            background-color: rgba(227, 28, 37, 0.1);
            padding: 0px;
            margin: 6px 0;
            width: 100%;
            min-height: 60px;
            box-sizing: border-box;
        }

        .course-time-red {
            background-color: #e31c25; /* 背景顏色 */
            color: white; /* 文字顏色 */
            display: flex; /* 使用 flexbox */
            justify-content: center; /* 水平居中 */
            align-items: center; /* 垂直居中 */
            text-align: center; /* 文字水平居中 */
            font-size: 22px; /* 調整字體大小 */
            font-weight: bold; /* 字體加粗 */
            padding: 10px; /* 添加內邊距 */
            height: 100%; /* 高度填滿容器 */
            flex-direction: column; /* 垂直排列內容 */
        }

        /* 藍色樣式 */
        .course-card-blue {
            border: 2px solid #1e90ff;
            border-radius: 10px;
            background-color: rgba(30, 144, 255, 0.1);
            padding: 0px;
            margin: 6px 0;
            width: 100%;
            min-height: 60px;
            box-sizing: border-box;
        }

        .course-time-blue {
            background-color: #1e90ff; /* 背景顏色 */
            color: white; /* 文字顏色 */
            display: flex; /* 使用 flexbox */
            justify-content: center; /* 水平居中 */
            align-items: center; /* 垂直居中 */
            text-align: center; /* 文字水平居中 */
            font-size: 22px; /* 調整字體大小 */
            font-weight: bold; /* 字體加粗 */
            padding: 10px; /* 添加內邊距 */
            height: 100%; /* 高度填滿容器 */
            flex-direction: column; /* 垂直排列內容 */
        }

        .course-details {
            padding-left: 15px;
        }

        .course-people {
            display: flex;
            justify-content: left; /* 水平居中 */
            align-items: center; /* 垂直居中 */
        }

        .course-APbtn {
            display: flex;
            justify-content: center; /* 水平居中 */
            align-items: center; /* 垂直居中 */
            flex-wrap: nowrap; /* 禁止子項目換行 */
        }


        .enroll-button {
            white-space: nowrap; /* 禁止換行 */
            background-color: #007bff; /* 背景顏色 */
            color: white; /* 文字顏色 */
            border-radius: 50px; /* 圓角邊框 */
            padding: 10px 20px; /* 調整內邊距 */
            font-size: 1rem; /* 調整字體大小 */
            font-weight: bold; /* 字體加粗 */
            border: none; /* 無邊框 */
            text-align: center;
            cursor: pointer;
            transition: background-color 0.3s ease; /* 過渡效果 */
        }

            .enroll-button:hover {
                background-color: #28a745; /* 懸停時的顏色 */
                color: white;
            }



        .btn-back-home {
            background-color: #007bff; /* 背景顏色 */
            color: white; /* 文字顏色設為白色 */
            border-radius: 50px; /* 圓角邊框 */
            padding: 10px 20px; /* 調整內邊距 */
            font-size: 1rem; /* 調整字體大小 */
            font-weight: bold; /* 使字體加粗 */
            border: none; /* 去掉預設邊框 */
            display: inline-block;
            text-align: center;
            transition: background-color 0.3s ease; /* 增加過渡效果 */
        }

            .btn-back-home:hover,
            .btn-back-home:focus {
                background-color: #28a745; /* 懸停或選取時的顏色 */
                color: white; /* 文字顏色設為白色 */
                text-decoration: none; /* 去掉下劃線 */
                outline: none; /* 去掉選取時的外框 */
            }
        .modal-dialog {
            max-width: 50%; /* 設定模態視窗寬度為 80% 視窗寬度 */
            margin: 200px auto;   /* 保持水平置中 */
        }
        .card {
                border: 1px solid #ddd;
                border-radius: 10px;
                overflow: hidden;
                box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
            }

            .card-img {
                height: 200px; /* 填充整個容器高度 */
                width: 200px; /* 自適應寬度，保持比例 */
                object-fit: cover; /* 填滿容器並裁剪多餘部分 */
                display: block; /* 確保圖片作為塊級元素顯示 */
            }

            .card-body {
                padding: 15px;
            }

            .card-title {
                font-size: 1.25rem;
                font-weight: bold;
            }

            .card-text {
                font-size: 0.9rem;
                color: #555;
            }

            .btn {
                border-radius: 20px;
                padding: 10px 20px;
                font-size: 0.9rem;
            }

            .btn-danger {
                background-color: #e31c25;
                border: none;
            }

            .btn-success {
                background-color: #28a745;
                border: none;
            }

    </style>

    <!-- Page Header Start -->
    <div class="container-fluid page-header mb-1">
        <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 400px">
            <h4 class="display-4 mb-3 mt-0 mt-lg-5 text-white text-uppercase font-weight-bold">預約紀錄</h4>
        </div>
    </div>

    <div class="container mt-3">
        <div class="mb-3" style="overflow: auto;">
            <div class="home-btn-container" style="float: left;">
                <a class="btn-back-home px-4 py-2" href="<%= ResolveUrl("~/Coach/Coach_index.aspx") %>">←返回首頁</a>
            </div>

            <div class="course-btn-container" style="float: right;">
                <asp:Button ID="btnToday" runat="server" Text="今日課程" CssClass="course-btn active" OnClick="btnToday_Click" OnClientClick="ShowProgressBar();" />
                <asp:Button ID="btnFuture" runat="server" Text="未來課程" CssClass="course-btn" OnClick="btnFuture_Click" OnClientClick="ShowProgressBar();" />
                <asp:Button ID="btnPast" runat="server" Text="過去課程" CssClass="course-btn" OnClick="btnPast_Click" OnClientClick="ShowProgressBar();" />
            </div>
        </div>


        <div class="tab-content" id="myTabContent">
            <div class="tab-pane fade show active" id="tab1" aria-labelledby="tab1-tab">
                <asp:Repeater ID="RP" runat="server" OnItemCommand="RP_ItemCommand" >
                    <ItemTemplate>
                        <!-- 根據地點類型設置不同樣式 -->
                        <asp:Label ID="whatPlacetype" runat="server" Text='<%# Eval("地點類型") %>' Visible="false"></asp:Label>
                        <div class='<%# Convert.ToInt32(Eval("地點類型")) == 2 ? "course-card-blue" : "course-card-red" %> mb-3'>
                            <div class="row">
                                <!-- 日期底色填滿 -->
                                <div class="col-md-2 course-time-container">
                                    <div class='<%# Convert.ToInt32(Eval("地點類型")) == 2 ? "course-time-blue" : "course-time-red" %>'>
                                        <asp:Label ID="TD_date" runat="server" Text='<%# Eval("日期", "{0:yyyy/MM/dd}") %>'></asp:Label>
                                        <asp:Label ID="Label1" runat="server" Text='<%#"("+ Eval("星期幾") +")"%>'></asp:Label>
                                    </div>
                                </div>
                                <!-- 課程名稱及詳細資訊 -->
                                <div class="col-md-6 course-details p-2">
                                    <!-- 課程名稱 -->
                                    <h3>
                                        <asp:Label ID="TD_Course_Name" runat="server" Text='<%# Eval("課程名稱") %>'></asp:Label>
                                    </h3>
                                    <!-- 時段 -->
                                    <img src="img/clock.png" alt="Clock Icon" style="width: 16px;" draggable="false"/>
                                    <asp:Label ID="TD_ST_time" runat="server" Text='<%# Eval("開始時間") %>'></asp:Label>
                                    <span>~</span>
                                    <asp:Label ID="TD_ED_time" runat="server" Text='<%# Eval("結束時間") %>'></asp:Label><br />

                                    <!-- 地點 -->
                                    <img src="img/maps.png" alt="Maps Icon" style="width: 16px;" draggable="false"/>
                                    <asp:Label ID="TD_Place" runat="server" Text='<%# Eval("地點名稱") %>'></asp:Label><br />
                                </div>
                                <!-- 預約人數 -->
                                <div class="col-md-2 course-people">
                                    <img src="img/courses.png" alt="Courses Icon" style="width: 32px;" class="mr-3" draggable="false"/>
                                    <h3>
                                        <asp:Label ID="TD_Last_People" runat="server" Text='<%# Eval("預約人數") %>'></asp:Label>
                                        <span>/</span>
                                        <asp:Label ID="TD_Max_People" runat="server" Text='<%# Eval("上課人數") %>'></asp:Label>
                                        <span>人</span>
                                    </h3>
                                </div>
                                <!-- 查看預約名單按鈕 -->
                                <div class="col-md-2 course-APbtn">
                                    <asp:Button ID="TD_AP_Chcek" runat="server" Text="查看預約名單" CssClass="enroll-button" CommandName="AP" CommandArgument='<%# Eval("課表編號") %>' />
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Label ID="lblNoCourses" runat="server" Text="無安排課程" Visible="false" ForeColor="Red" Font-Bold="True" Font-Size="36px"></asp:Label>
            </div>
        </div>
    </div>

    <asp:Panel ID="Panel1" runat="server" Visible="false" CssClass="modal" TabIndex="-1" role="dialog" aria-labelledby="ModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="ModalLabel">預約名單</h5>
                </div>
                <div class="modal-body">
                    <asp:Label ID="lblNoData" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Repeater ID="AP_Detail" runat="server" OnItemCommand="AP_Detail_ItemCommand" OnItemDataBound="AP_Detail_ItemDataBound" OnItemCreated="AP_Detail_ItemCreated">
                        <ItemTemplate>
                            <div class="card mb-3">
                                <div class="row no-gutters">
                                    <!-- 圖片部分 -->
                                    <div class="image-container" style="flex: 0 0 30%; display: flex; align-items: center; justify-content: center; overflow: hidden;">
                                        <asp:Image ID="Image2" runat="server"  CssClass="card-img" ImageUrl='<%# GetImageUrl(Eval("使用者圖片"),20) %>' />
                                    </div>
                                    <!-- 內容部分 -->
                                    <div class="col-md-8">
                                        <div class="card-body">
                                            <h5 class="card-title"><%# Eval("使用者姓名") %></h5>
                                            <asp:Label ID="schedule_id" runat="server" Text=<%# Eval("課表編號") %> ></asp:Label>
                                            <asp:Label ID="coach_id" runat="server" Text=<%# Eval("健身教練編號") %> ></asp:Label>
                                            <asp:Label ID="status" runat="server" Text=<%# Eval("預約狀態") %> ></asp:Label>
                                            <span class="card-text">
                                                性別：<%# GetGenderText(Eval("使用者性別")) %><br />
                                                電話：<%# Eval("使用者電話") %><br />
                                                信箱：<%# Eval("使用者郵件") %><br />
                                                備註：<%# Eval("備註") %><br />
                                            </span>
                                            <asp:Label ID="ap_detail_placeName_label" runat="server" Text="地點名稱：" Font-Size="Small"></asp:Label>
                                            <asp:Label ID="ap_detail_placeName" runat="server" Font-Size="Small" Text=<%# Eval("地點名稱") %> />
                                            <asp:Label ID="ap_detail_Userplace_label" runat="server" Font-Size="Small" Text="客戶到府地址：" ></asp:Label>
                                            <asp:Label ID="ap_detail_area" runat="server" Font-Size="Small" Text=<%# Eval("縣市") %> />
                                            <asp:Label ID="ap_detail_city" runat="server" Font-Size="Small" Text=<%# Eval("行政區") %> />
                                            <asp:Label ID="ap_detail_Userplace" runat="server" Font-Size="Small" Text=<%# Eval("客戶到府地址") %> />
                                            <div class="d-flex justify-content-between">
                                                <!-- 取消預約按鈕 -->
                                                <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" CommandArgument='<%# Eval("預約編號") %>' Text="取消預約" CssClass="btn btn-danger" />
                                                <!-- 完成預約按鈕 -->
                                                <asp:Button ID="btnFinish" runat="server" CommandName="Finish" CommandArgument='<%# Eval("預約編號") %>' Text="完成預約" CssClass="btn btn-success" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>


                    <div class="modal-footer">
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

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
</asp:Content>

