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
                <asp:Repeater ID="RP" runat="server" OnItemCommand="RP_ItemCommand">
                    <ItemTemplate>
                        <!-- 根據地點類型設置不同樣式 -->
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
                    <asp:GridView ID="AP_Detail" runat="server" AutoGenerateColumns="False" OnRowCommand="AP_Detail_RowCommand" OnRowCreated="AP_Detail_RowCreated" OnRowCancelingEdit="AP_Detail_RowCancelingEdit" OnRowDataBound="AP_Detail_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="預約編號" HeaderText="預約編號" />
                            <%--改預約狀態用--%>
                            <asp:BoundField DataField="課表編號" HeaderText="課表編號" />
                            <%--改預約人數用--%>
                            <asp:BoundField DataField="健身教練編號" HeaderText="健身教練編號" />
                            <%--改教練次數用--%>
                            <asp:BoundField DataField="預約狀態" HeaderText="預約狀態" />
                            <%--判斷需不需按鈕用--%>
                            <asp:BoundField DataField="使用者姓名" HeaderText="客戶名稱" />
                            <asp:BoundField DataField="使用者性別" HeaderText="性別" />
                            <asp:BoundField DataField="使用者電話" HeaderText="電話" />
                            <asp:BoundField DataField="使用者郵件" HeaderText="信箱" />
                            <asp:BoundField DataField="備註" HeaderText="備註" />

                            <asp:ButtonField ButtonType="Button" CommandName="Cancel" Text="取消預約" />
                            <asp:ButtonField ButtonType="Button" CommandName="Finish" Text="完成預約" />
                        </Columns>
                    </asp:GridView>

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

