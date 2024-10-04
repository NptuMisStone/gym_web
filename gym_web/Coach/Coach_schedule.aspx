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
        function scrollToControl() {
            var element = document.getElementById('<%= btnCurrentWeek.ClientID %>');
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
    </style>

    <!-- Page Header Start -->
    <div class="container-fluid page-header mb-5">
        <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 400px">
            <h4 class="display-4 mb-3 mt-0 mt-lg-5 text-white text-uppercase font-weight-bold">課程安排</h4>
        </div>
    </div>

    <div class="container gym-feature py-2">
        <div class="row">
            <div class="col-md-12 text-center mb-4">
                <button id="btnCurrentWeek" runat="server" onserverclick="btnCurrentWeek_Click" class="btn btn-primary">本週</button>
                <button id="btnPreviousWeek" runat="server" onserverclick="btnPreviousWeek_Click" class="btn btn-primary">上週</button>
                <button id="btnNextWeek" runat="server" onserverclick="btnNextWeek_Click" class="btn btn-primary">下週</button>

                <asp:TextBox ID="txtSelectedDate" runat="server" TextMode="Date"></asp:TextBox>
                <button id="btnQueryWeek" runat="server" onserverclick="btnQueryWeek_Click" class="btn btn-secondary">查詢那週</button>

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
    <asp:Label ID="lblMessage" runat="server"></asp:Label>

    <!-- 刪除彈窗結構 -->
    <div id="simplePopup" class="popup-overlay" style="display: none;">
        <div class="popup-content">
            <span class="close-btn" onclick="closeSimplePopup()">&times;</span>
            <h2></h2>
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
                            <asp:LinkButton ID="lb_class" runat="server" CommandName="see_detail" CommandArgument='<%# Eval("課程編號") %>'
                                CssClass="unstyled-link"
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
                                        <!-- 字體大小調整 -->
                                    </div>
                                    <div class="col-sm-7">
                                        <!-- 內距縮小 -->
                                        <h4 class="font-weight-bold" style="font-size: 16px; margin: 0;"><%# Eval("課程名稱") %></h4>
                                        <p style="font-size: 14px; margin: 0;"><%# "時長：" + Eval("課程時間長度") + "分鐘"%></p>
                                        <p style="font-size: 14px; margin: 0;"><%# "人數：" + Eval("上課人數") + "人"%></p>
                                    </div>
                                </div>
                            </asp:LinkButton>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>

    <button type="button" class="btn btn-success" onclick="showCoursePopup()">安排課程</button>

</asp:Content>

