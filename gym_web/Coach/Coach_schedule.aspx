<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="Coach_schedule.aspx.cs" Inherits="Coach_Coach_schedule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/tempusdominus-bootstrap-4/5.39.0/css/tempusdominus-bootstrap-4.min.css" />
    <script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        function showSimplePopup() {
            document.getElementById("simplePopup").style.display = "block";
            localStorage.setItem("popupVisible", "true"); // 儲存彈窗顯示狀態
            document.body.classList.add('no-scroll'); // 禁用背景滾動
        }

        function closeSimplePopup() {
            document.getElementById("simplePopup").style.display = "none";
            localStorage.removeItem("popupVisible"); // 清除彈窗顯示狀態
            document.body.classList.remove('no-scroll'); // 恢復背景滾動
        }
        function scrollToControl(controlId) {
            var element = document.getElementById(controlId);
            if (element) {
                element.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }
        }

        function handleCourseClick(scheduleId) {
            event.preventDefault(); // 防止頁面重載
            $.ajax({
                type: "POST",
                url: "Coach_schedule.aspx/GetScheduleDetails",
                data: JSON.stringify({ scheduleId: scheduleId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    showSimplePopup();
                    var data = response.d; // 根據 ASP.NET WebMethod 返回的格式，使用 response.d 獲取結果
                    if (data.error) {
                        alert(data.error);
                    } else {
                        document.querySelector('.popup-content h2').innerHTML = `
                            ${data.date || 'N/A'}<br>
                            ${data.courseName || 'N/A'}
                            `;
                        document.querySelector('.popup-content p').innerHTML = `
                            <strong>課程時間長度：</strong> ${data.duration || 'N/A'}<strong>分鐘</strong><br>
                            <strong>開始時間：</strong> ${data.startTime || 'N/A'}<br>
                            <strong>結束時間：</strong> ${data.endTime || 'N/A'}
                            `;
                    }
                    console.log(response);
                },


                error: function (error) {
                    console.log("Error:", error);
                }
            });
            showSimplePopup();
        }

        // 檢查 Local Storage 中的顯示狀態
        window.onload = function () {
            if (localStorage.getItem("popupVisible") === "true") {
                showSimplePopup();
            }
        }

        function confirmDelete() {
            Swal.fire({
                title: '您確定要刪除課程嗎？',
                text: "刪除後將無法恢復！",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: '確認刪除',
                cancelButtonText: '取消'
            }).then((result) => {
                if (result.isConfirmed) {
                    // 用戶確認刪除後，手動觸發伺服器端按鈕的點擊事件
                    __doPostBack('<%= btnDeleteSchedule.UniqueID %>', '');
                    // 刪除後關閉彈窗
                    closeSimplePopup();
                }
            });
        }
        function showCoursePopup() {
            document.getElementById('coursePopup').style.display = 'block';
            document.body.classList.add('no-scroll'); // 禁用背景滾動
        }

        function closeCoursePopup() {
            document.getElementById('coursePopup').style.display = 'none';
            document.body.classList.remove('no-scroll'); // 恢復背景滾動
        }

        $(document).ready(function () {
            // 自動設置初始值為 6:30
            var initialStartTime = '06:30';
            $('#<%= tbCourseStartTime.ClientID %>').val(initialStartTime);

            // 初始化 datetimepicker
            $('#<%= tbCourseStartTime.ClientID %>').datetimepicker({
                format: 'HH:mm', // 僅顯示時間
                icons: {
                    time: 'fa fa-clock',
                    up: 'fa fa-chevron-up',
                    down: 'fa fa-chevron-down',
                    previous: 'fa fa-chevron-left',
                    next: 'fa fa-chevron-right',
                    today: 'fa fa-calendar-check-o',
                    clear: 'fa fa-trash',
                    close: 'fa fa-times'
                },
                useCurrent: false, // 不自動填入當前時間
                stepping: 30, // 每次按鍵跳30分鐘
                minDate: moment({ hour: 6, minute: 30 }), // 設定最早時間 6:30
            });

            // 獲取課程時長，並設置預設結束時間
            var courseDuration = parseInt($('.course-duration').val());
            var endTime = moment(initialStartTime, 'HH:mm').add(courseDuration, 'minutes');
            var formattedEndTime = endTime.format('HH:mm');

            // 設置 tbCourseEndTime 的預設值
            $('#<%= tbCourseEndTime.ClientID %>').val(formattedEndTime);
            $('#<%= hiddenCourseEndTime.ClientID %>').val(formattedEndTime);

            // 當點擊文本框時顯示時間選擇器
            $('#<%= tbCourseStartTime.ClientID %>').on('focus', function () {
                // 計算最大開始時間（22:30 減去課程時長）
                var maxStartTime = moment({ hour: 22, minute: 30 }).subtract(courseDuration, 'minutes');
                $('#<%= tbCourseStartTime.ClientID %>').datetimepicker('maxDate', maxStartTime);
                $(this).datetimepicker('show');
            });

            // 當開始時間發生改變時，計算結束時間
            $('#<%= tbCourseStartTime.ClientID %>').on('change.datetimepicker', function (e) {
                var startTime = e.date;  // 獲取選中的開始時間

                if (startTime) {
                    // 計算結束時間
                    var endTime = startTime.clone().add(courseDuration, 'minutes');
                    var formattedEndTime = endTime.format('HH:mm');

                    // 將結束時間設置到 tbCourseEndTime 的文本框中
                    $('#<%= tbCourseEndTime.ClientID %>').val(formattedEndTime);

                    // 同時更新隱藏的 input
                    $('#<%= hiddenCourseEndTime.ClientID %>').val(formattedEndTime);
                }
            });

            // 點擊旁邊時隱藏選擇器
            $(document).on('click', function (e) {
                if (!$(e.target).closest('#<%= tbCourseStartTime.ClientID %>').length && !$(e.target).closest('#confirmButton').length) {
                    $('#<%= tbCourseStartTime.ClientID %>').datetimepicker('hide');
                }
            });
        });



    </script>
    <style>
        /* 紅色樣式 */
        .course-card-red {
            border: 2px solid #e31c25; /* 邊框顏色 */
            border-radius: 10px; /* 圓弧邊框 */
            background-color: rgba(227, 28, 37, 0.1); /* 背景顏色 */
            padding: 0px; /* 內邊距 */
            margin: 6px 0;
            width: 100%; /* 設置卡片寬度為父容器的100% */
            min-height: 60px; /* 設置最小高度，確保卡片不會太小 */
            box-sizing: border-box; /* 包括內邊距和邊框在內的大小 */
        }

        .course-time-red {
            background-color: #e31c25; /* 時間的背景顏色 */
            color: white; /* 時間的文字顏色 */
            padding: 5px; /* 內邊距 */
            border-top-left-radius: 5px; /* 上左圓角 */
            border-top-right-radius: 5px; /* 上右圓角 */
            text-align: center; /* 文字置中 */
            padding: 5px 10px; /* 添加左右內邊距，確保內容不擁擠 */
            font-size: 14px; /* 確保文字大小一致 */
        }

        /* 藍色樣式 */
        .course-card-blue {
            border: 2px solid #1e90ff; /* 邊框顏色 */
            border-radius: 10px; /* 圓弧邊框 */
            background-color: rgba(30, 144, 255, 0.1); /* 背景顏色 */
            padding: 0px; /* 內邊距 */
            margin: 6px 0;
            width: 100%; /* 設置卡片寬度為父容器的100% */
            min-height: 60px; /* 設置最小高度，確保卡片不會太小 */
            box-sizing: border-box; /* 包括內邊距和邊框在內的大小 */
        }

        .course-time-blue {
            background-color: #1e90ff; /* 時間的背景顏色 */
            color: white; /* 時間的文字顏色 */
            padding: 5px; /* 內邊距 */
            border-top-left-radius: 5px; /* 上左圓角 */
            border-top-right-radius: 5px; /* 上右圓角 */
            text-align: center; /* 文字置中 */
            padding: 5px 10px; /* 添加左右內邊距，確保內容不擁擠 */
            font-size: 14px; /* 確保文字大小一致 */
        }

        .course-name {
            text-align: center; /* 課程名稱置中 */
            margin-top: 5px; /* 課程名稱與時間的間距 */
            padding: 5px 10px; /* 內邊距，讓課程名稱有點空間 */
            font-size: 14px; /* 保持文字大小一致 */
        }
        /* 覆蓋層的樣式 */
        .popup-overlay {
            display: none; /* 初始狀態為隱藏 */
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.5); /* 半透明黑色背景 */
            z-index: 1000; /* 保證彈窗位於最上層 */
        }

        /* 彈窗內容的樣式 */
        .popup-content {
            position: relative;
            margin: 5% auto; /* 讓彈窗居中 */
            padding: 30px; /* 增加內邊距 */
            width: 90%; /* 增大彈窗寬度 */
            max-width: 800px; /* 增加最大寬度 */
            max-height: 80vh; /* 設置最大高度為視口高度的 80% */
            overflow-y: auto; /* 當內容超出時，顯示垂直滾動條 */
            background-color: white;
            border-radius: 10px;
            box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3);
            text-align: left; /* 保持文字靠左對齊 */
        }


        /* 關閉按鈕樣式 */
        .close-btn {
            position: absolute;
            top: 15px;
            right: 20px;
            font-size: 30px; /* 增加按鈕大小 */
            cursor: pointer;
        }

        .no-scroll {
            overflow: hidden; /* 禁用滾動 */
        }

        .time-card-black {
            border: 2px solid #000; /* 邊框顏色變為黑色 */
            border-radius: 10px; /* 圓弧邊框 */
            background-color: rgba(0, 0, 0, 0.05); /* 背景顏色 */
            padding: 0px; /* 內邊距 */
            margin: 6px 0;
            width: 100%; /* 設置卡片寬度為父容器的100% */
            min-height: 60px; /* 設置最小高度 */
            box-sizing: border-box; /* 包括內邊距和邊框在內的大小 */
        }

        .time-range-black {
            background-color: #000; /* 時間的背景顏色變為黑色 */
            color: white; /* 時間文字顏色為白色 */
            padding: 5px; /* 內邊距 */
            border-top-left-radius: 5px; /* 上左圓角 */
            border-top-right-radius: 5px; /* 上右圓角 */
            text-align: center; /* 文字置中 */
            padding: 10px 15px; /* 添加左右內邊距 */
            font-size: 16px; /* 時間字體變小 */
            font-weight: bold; /* 時間字加粗 */
        }

        .time-content {
            text-align: center; /* 時間內容置中 */
            margin-top: 5px; /* 課程名稱與時間的間距 */
            padding: 10px 15px; /* 內邊距 */
            font-size: 24px; /* 時間字體變小 */
            font-weight: bold; /* 時間字加粗 */
            color: #000; /* 時間字顏色為黑色 */
        }

        .time-dash {
            flex: 2; /* 中間的 "-" 占2份 */
            text-align: center; /* 文字置中 */
            font-size: 20px; /* 字體變小 */
            font-weight: bold; /* 加粗 */
            color: black; /* 字體顏色 */
        }
        /* 日期圓角框樣式 */
        .date-badge {
            display: inline-block;
            background-color: Green; /* 綠色背景 */
            color: #fff; /* 白色文字 */
            padding: 5px 10px; /* 內間距 */
            margin: 5px; /* 每個日期之間的間距 */
            border-radius: 15px; /* 圓角 */
            border: 1px solid Green; /* 綠色邊框與背景一致 */
            font-size: 14px; /* 文字大小 */
            font-weight: bold; /* 粗體 */
        }
        /* 類型圓角框樣式 */
        .type-badge {
            display: inline-block;
            background-color: #1e90ff; /* 綠色背景 */
            color: #fff; /* 白色文字 */
            padding: 5px 10px; /* 內間距 */
            border-radius: 15px; /* 圓角 */
            border: 1px solid Green; /* 綠色邊框與背景一致 */
            font-size: 14px; /* 文字大小 */
            font-weight: bold; /* 粗體 */
        }

        .step-title {
            font-size: 1.5rem; /* 字體大小 */
            font-weight: bold; /* 粗體字 */
            color: #e31c25; /* 標題顏色 */
            border-bottom: 2px solid #e31c25; /* 底部邊框 */
            padding-bottom: 10px; /* 底部內邊距 */
            margin-bottom: 15px; /* 與下方內容的間距 */
            text-align: left; /* 左對齊 */
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
            <h4 class="display-4 mb-3 mt-0 mt-lg-5 text-white text-uppercase font-weight-bold">課程安排</h4>
        </div>
    </div>

    <!-- 課表周次及顏色塊的部分 -->
    <div id="Week" runat="server" class="container gym-feature py-2">
        <div class="mb-3">
            <a class="btn-back-home px-4 py-2" href="<%= ResolveUrl("~/Coach/Coach_index.aspx") %>">←返回首頁</a>
        </div>
        <div class="row">
            <div class="col-md-12 d-flex justify-content-between align-items-center">
                <!-- 左側：周次按鈕 -->
                <div>
                    <button id="btnPreviousWeek" runat="server" onserverclick="btnPreviousWeek_Click" class="btn btn-primary">上週</button>
                    <button id="btnCurrentWeek" runat="server" onserverclick="btnCurrentWeek_Click" class="btn btn-primary">本週</button>
                    <button id="btnNextWeek" runat="server" onserverclick="btnNextWeek_Click" class="btn btn-primary">下週</button>
                    <asp:TextBox ID="txtSelectedDate" runat="server" TextMode="Date" CssClass="ml-2"></asp:TextBox>
                    <button id="btnQueryWeek" runat="server" onserverclick="btnQueryWeek_Click" class="btn btn-secondary ml-2">查詢該週</button>
                </div>

                <!-- 右側：顏色塊 -->
                <div>
                    <span class="badge" style="background-color: #e31c25; color: white; padding: 10px 15px; font-size: 1.2em; border-radius: 5px; margin-right: 15px;">團體課程</span>
                    <span class="badge" style="background-color: #1e90ff; color: white; padding: 10px 15px; font-size: 1.2em; border-radius: 5px;">到府課程</span>
                </div>
            </div>
        </div>
    </div>


    <!-- Class Timetable Start -->
    <div class="container gym-feature py-2">
        <div class="table-responsive">
            <table class="table table-bordered table-lg m-0">
                <thead class="bg-secondary text-white text-center">
                    <tr>
                        <th style="width: 100px;" id="MondayHeader" runat="server">星期一<br />
                            <asp:Label ID="lblMondayDate" runat="server"></asp:Label>
                        </th>
                        <th style="width: 100px;" id="TuesdayHeader" runat="server">星期二<br />
                            <asp:Label ID="lblTuesdayDate" runat="server"></asp:Label>
                        </th>
                        <th style="width: 100px;" id="WednesdayHeader" runat="server">星期三<br />
                            <asp:Label ID="lblWednesdayDate" runat="server"></asp:Label>
                        </th>
                        <th style="width: 100px;" id="ThursdayHeader" runat="server">星期四<br />
                            <asp:Label ID="lblThursdayDate" runat="server"></asp:Label>
                        </th>
                        <th style="width: 100px;" id="FridayHeader" runat="server">星期五<br />
                            <asp:Label ID="lblFridayDate" runat="server"></asp:Label>
                        </th>
                        <th style="width: 100px;" id="SaturdayHeader" runat="server">星期六<br />
                            <asp:Label ID="lblSaturdayDate" runat="server"></asp:Label>
                        </th>
                        <th style="width: 100px;" id="SundayHeader" runat="server">星期日<br />
                            <asp:Label ID="lblSundayDate" runat="server"></asp:Label>
                        </th>
                    </tr>
                </thead>
                <tbody class="text-center">
                    <tr>
                        <td id="MondayCell" runat="server"></td>
                        <td id="TuesdayCell" runat="server"></td>
                        <td id="WednesdayCell" runat="server"></td>
                        <td id="ThursdayCell" runat="server"></td>
                        <td id="FridayCell" runat="server"></td>
                        <td id="SaturdayCell" runat="server"></td>
                        <td id="SundayCell" runat="server"></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>

    <!-- 刪除彈窗結構 -->
    <div id="simplePopup" class="popup-overlay" style="display: none;">
        <div class="popup-content">
            <span class="close-btn" onclick="closeSimplePopup()">&times;</span>
            <h2>Loading...</h2>
            <p></p>

            <!-- 使用 ASP.NET 按鈕 -->
            <div class="popup-actions">
                <asp:Button ID="btnDeleteSchedule" runat="server" Text="刪除課表"
                    OnClick="btnDeleteSchedule_Click"
                    OnClientClick="confirmDelete(); return false;"
                    CssClass="btn btn-danger" />
            </div>
        </div>
    </div>

    <!-- 排課彈窗結構 -->
    <div id="coursePopup" class="popup-overlay" style="display: none;">
        <div class="popup-content">
            <span class="close-btn" onclick="closeCoursePopup()">&times;</span>
            <h2>選擇課程</h2>

            <!-- 在彈窗內放入 ListView -->
            <asp:ListView ID="lv_class" runat="server" OnItemCommand="lv_class_ItemCommand">
                <LayoutTemplate>
                    <div class="row">
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <!-- 使用 col-md-4 控制每個課程寬度為4格 (三欄寬) -->
                    <div class="col-md-4 mb-4 d-flex justify-content-center">
                        <div style="width: 100%; transition: background-color 0.3s ease; border: 2px solid black; border-radius: 8px; overflow: hidden;"
                            onmouseover="this.style.backgroundColor='#f0f0f0'"
                            onmouseout="this.style.backgroundColor=''">
                            <asp:LinkButton ID="lb_class" runat="server" CommandName="show" CommandArgument='<%# Eval("課程編號") + "," + Eval("地點類型") %>'
                                CssClass="unstyled-link" OnClientClick="closeCoursePopup();ShowProgressBar();"
                                Style="display: block; text-align: left; text-decoration: none; cursor: pointer;">
                                <div class="row align-items-center" style="padding: 5px;">
                                    <!-- 內距縮小 -->
                                    <div class="col-sm-5 text-center" style="padding: 5px;">
                                        <!-- 內距縮小 -->
                                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# GetImageUrl(Eval("課程圖片"),60) %>'
                                            CssClass="img-fluid rounded-circle"
                                            Style="object-fit: cover; height: 60px; width: 60px;" />
                                        <!-- 圖片縮小 -->
                                        <br />
                                        <!-- 添加換行 -->
                                        <%# Convert.ToInt32(Eval("上課人數")) == 1 ? 
                                 "<i style='font-size:14px; font-weight: bold;'>一對一</i>" : 
                                 "<i style='font-size:14px; font-weight: bold;'>團體</i>" %>
                                        <br />
                                        <%# Convert.ToInt32(Eval("地點類型")) == 2 ? 
                                 "<i class='type-badge'>到府</i>" : "" %>
                                        <!-- 字體大小調整 -->
                                    </div>
                                    <div class="col-sm-7">
                                        <!-- 內距縮小 -->
                                        <h4 class="font-weight-bold" style="font-size: 16px; margin: 0;"><%# Eval("課程名稱") %></h4>
                                        <p style="font-size: 14px; margin: 0;"><%# "時長：" + Eval("課程時間長度") + "分鐘"%></p>
                                        <p style="font-size: 14px; margin: 0;"><%# "人數：" + Eval("上課人數") + "人"%></p>
                                        <p style="font-size: 14px; margin: 0;"><%# "地點：" + GetLocation(Eval("課程編號")) %></p>
                                    </div>
                                </div>
                            </asp:LinkButton>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>

    <div id="Addclass" runat="server" class="container gym-feature py-4">
        <div class="row">
            <!-- 左方日曆區塊 -->
            <div class="col-md-6">
                <!-- Step 1: 選擇日期 -->
                <h5 class="mb-4 step-title">Step 1: 選擇日期</h5>
                <asp:Calendar ID="Calendar1" runat="server" OnSelectionChanged="Calendar1_SelectionChanged" OnDayRender="Calendar1_DayRender" Style="width: 100%;"></asp:Calendar>
                <asp:Label ID="SelectedDatesLabel" runat="server" CssClass="mt-3 d-block" Text="選擇的日期：未選擇"></asp:Label>
            </div>
            <!-- 右方課程及排課 -->
            <div class="col-md-6">
                <!-- Step 2: 選擇課程 -->
                <div class="mb-4">
                    <h5 class="mb-4 step-title">Step 2: 選擇課程</h5>
                    <div class="row">
                        <!-- 課程容器佔 8 格 -->
                        <div class="col-md-8">
                            <div id="CourseContainer" runat="server">
                                <p class="text-center border-dashed" style="border: 2px dashed black; border-radius: 8px; padding: 15px; height: 100px;">
                                    尚未選擇課程
                                </p>
                            </div>
                        </div>
                        <!-- 按鈕容器佔 4 格 -->
                        <div class="col-md-4 d-flex align-items-center justify-content-center">
                            <button type="button" class="btn btn-outline-primary px-4 py-2" onclick="showCoursePopup()">選擇課程</button>
                        </div>
                    </div>
                </div>
                <!-- Step 3: 選擇開始時間 -->
                <div class="mb-4">
                    <h5 class="mb-4 step-title">Step 3: 設定開始時間</h5>
                    <div class="row">
                        <!-- 時間容器佔 8 格 -->
                        <div class="col-md-8">
                            <div id="TimeContainer" runat="server">
                                <div class="time-card-black">
                                    <div class="time-range-black mb-2">開始時間　—　結束時間</div>
                                    <div class="time-content d-flex justify-content-between">
                                        <asp:TextBox ID="tbCourseStartTime" runat="server" CssClass="form-control" Style="width: 40%; text-align: center; font-size: x-large" />
                                        <span>—</span>
                                        <asp:TextBox ID="tbCourseEndTime" runat="server" ReadOnly="true" CssClass="form-control" Style="width: 40%; text-align: center; font-size: x-large" />
                                        <input type="hidden" id="hiddenCourseEndTime" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- 按鈕容器佔 4 格 -->
                        <div class="col-md-4 d-flex flex-column align-items-center justify-content-center">
                            <div class="mb-2">
                                <asp:Button ID="btnAddSchedule" runat="server" Text="新增課表" OnClick="btnAddSchedule_Click" class="btn btn-outline-primary px-4 py-2" OnClientClick="ShowProgressBar();" />
                            </div>
                            <div>
                                <asp:Button ID="btnCancel" runat="server" Text="全部取消" OnClick="btnCancel_Click" class="btn btn-outline-secondary px-4 py-2" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


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

