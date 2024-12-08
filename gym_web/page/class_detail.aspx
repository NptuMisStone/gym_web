<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="class_detail.aspx.cs" Inherits="page_class_detail" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        function scrollToSchedule() {
            // 使用 scrollIntoView 方法將目標元素滾動到可視區域
            document.getElementById('course-schedule').scrollIntoView({
                behavior: 'smooth' // 平滑滾動
            });
        }
    </script>
    <style>
    /* 整體行事曆樣式 */
    #course-schedule {
        display: flex;
        flex-direction: column;
        align-items: center;
        margin-top: 25px;
        width: 100%; /* 使其滿版 */
        padding: 20px; /* 添加內邊距 */
        background-color: #f9f9f9; /* 淺色背景 */
        border-radius: 10px; /* 圓角 */
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1); /* 添加陰影 */
    }

    /* 課程時段標題 */
    #course-schedule h3 {
        font-size: 2rem;
        color: #333;
        margin-bottom: 20px;
    }

    /* 重新設計 repeater 的 item */
    .item {
        margin: 15px auto; /* 垂直間距 */
        width: 100%; /* 滿版 */
        max-width: 800px; /* 限制最大寬度 */
        padding: 20px; /* 添加內邊距 */
        background-color: #fff; /* 白色背景 */
        border: 2px solid #e31c25; /* 加粗的邊框 */
        border-radius: 8px; /* 圓角 */
        box-shadow: 0 1px 5px rgba(0, 0, 0, 0.1); /* 陰影 */
        transition: transform 0.3s; /* 添加過渡效果 */
    }

    .item:hover {
        transform: scale(1.02); /* 鼠標懸停時放大 */
    }

    .btn:hover {
        background-color: #c01a1b; /* 鼠標懸停時的背景色 */
    }
    /* 調整行事曆整體樣式 */
    .aspNetCalendar {
        width: 100%; /* 使行事曆滿版 */
        border-collapse: collapse; /* 取消邊框間隙 */
        border: 2px solid #e31c25; /* 外邊框 */
        border-radius: 10px; /* 圓角 */
        overflow: hidden; /* 隱藏溢出的內容 */
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1); /* 陰影 */
    }

    .aspNetCalendar td {
        padding: 10px; /* 單元格內邊距 */
        text-align: center; /* 文字居中 */
        border: 1px solid #ddd; /* 單元格邊框 */
        transition: background-color 0.3s; /* 背景色過渡 */
    }

    .aspNetCalendar th {
        background-color: #e31c25; /* 標題行背景色 */
        color: white; /* 標題文字顏色 */
        padding: 10px; /* 標題行內邊距 */
    }

    /* 當前日期的樣式 */
    .aspNetCalendar .today {
        background-color: #ffd700; /* 當前日期的背景色 */
        border-radius: 50%; /* 圓角 */
    }

    /* 鼠標懸停時的樣式 */
    .aspNetCalendar td:hover {
        background-color: #f0f0f0; /* 懸停時的背景色 */
    }

    /* 已選中日期的樣式 */
    .aspNetCalendar .selected {
        background-color: #4CAF50; /* 已選中的背景色 */
        color: white; /* 已選中時的文字顏色 */
        border-radius: 50%; /* 圓角 */
    }
</style>


    <!-- Page Header Start -->
    <div class="container-fluid page-header mb-5">
        <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 130px">
        </div>
    </div>
    <!-- Page Header End -->

    <asp:Repeater ID="rp_class" runat="server" OnItemCommand="rp_class_ItemCommand" OnItemDataBound="rp_class_ItemDataBound">
        <ItemTemplate>
            <!-- Class Start -->
            <div class="container py-5">
                <div class="row align-items-center">
                    <div class="col-lg-6">
                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# GetclassImageUrl(Eval("課程圖片"),80) %>' class="img-fluid mb-4 mb-lg-0" Style="object-fit: cover; height: 500%; width: 500%;" />
                    </div>
                    <div class="col-lg-6">
                        <h2 class="display-4 font-weight-bold mb-4"><%# Eval("課程名稱") %></h2>
                        <!-- 將愛心按鈕移到這裡 -->
                        <div style="display: flex; margin-bottom: 20px;">
                            <asp:ImageButton ID="LikeBtn" runat="server" ImageUrl="~/page/img/dislike2.png"
                                Style="width: 50px; height: 50px;" OnClick="LikeBtn_Click"
                                CommandArgument='<%# Eval("課程編號") %>' />

                        </div>
                        <p><%# Eval("課程內容介紹") %></p>
                        <h3 class="font-weight-bold mb-4" style="color: #e31c25"><%# "$ " + Convert.ToDouble(Eval("課程費用")).ToString("F0") + " /堂"%></h3>

                        <div class="row py-3">
                            <!-- 課程人數 -->
                            <div class="col-sm-6 col-md-3 mb-3">
                                <div class="d-flex align-items-start">
                                    <img src="img/courses.png" alt="Barbell Icon" class="mr-2" style="height: 30px; flex-shrink: 0;" draggable="false">
                                    <div style="display: flex; flex-direction: column;">
                                        <h5 class="font-weight-bold mb-0" style="white-space: nowrap;">課程人數</h5>
                                        <p class="mb-0"><%# Eval("上課人數") %> 人</p>
                                    </div>
                                </div>
                            </div>

                            <!-- 課程時間 -->
                            <div class="col-sm-6 col-md-3 mb-3">
                                <div class="d-flex align-items-start">
                                    <img src="img/time-left.png" alt="Time Icon" class="mr-2" style="height: 30px; flex-shrink: 0;" draggable="false">
                                    <div style="display: flex; flex-direction: column;">
                                        <h5 class="font-weight-bold mb-0" style="white-space: nowrap;">課程時間</h5>
                                        <p class="mb-0"><%# Eval("課程時間長度") %> 分鐘</p>
                                    </div>
                                </div>
                            </div>

                            <!-- 上課地點 -->
                            <div class="col-sm-6 col-md-3 mb-3">
                                <div class="d-flex align-items-start">
                                    <img src="img/maps.png" alt="Map Icon" class="mr-2" style="height: 30px; flex-shrink: 0;" draggable="false">
                                    <div style="display: flex; flex-direction: column;">
                                        <h5 class="font-weight-bold mb-0" style="white-space: nowrap;">上課地點</h5>
                                        <p class="mb-0" style="white-space: normal; word-wrap: break-word; word-break: break-all;">
                                            <%# Eval("顯示地點名稱") %>
                                        </p>
                                    </div>
                                </div>
                            </div>

                            <!-- 所需設備 -->
                            <div class="col-sm-6 col-md-3 mb-3">
                                <div class="d-flex align-items-start">
                                    <img src="img/sport-bottle.png" alt="Equipment Icon" class="mr-2" style="height: 30px; flex-shrink: 0;" draggable="false">
                                    <div style="display: flex; flex-direction: column;">
                                        <h5 class="font-weight-bold mb-0" style="white-space: nowrap;">所需設備</h5>
                                        <p class="mb-0" style="white-space: normal; word-wrap: break-word; word-break: break-all;">
                                            <%# Eval("所需設備") %>
                                        </p>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <a class="btn btn-outline-primary mt-2 px-3" href="javascript:void(0);" onclick="scrollToSchedule()" style="height: 70px; width: 320px; font-size: 36px; border-color: #e31c25">立即預約課程　<i class="fa fa-angle-right"></i></a>
                    </div>
                </div>
            </div>
            <!-- Class End -->

            <div class="col-md-12 bg-light">
    <div class="d-flex align-items-center p-4" style="min-height: 100px; background-color: #f8f9fa;">
        <!-- 調整背景顏色及間距 -->
        <div class="container w-100">
            <!-- 使用 d-flex 將所有內容放置在同一排 -->
            <div class="d-flex align-items-center w-100">
                <!-- 教練圖片及資訊 -->
                <div class="d-flex align-items-center">
                    <asp:LinkButton ID="lb_image" runat="server" CommandName="select_coach" CommandArgument='<%# Eval("健身教練編號") %>'>
                        <asp:Image ID="Image2" runat="server" Style="object-fit: cover" Height="150px" Width="150px" class="rounded-circle shadow-sm" ImageUrl='<%# GetcoachImageUrl(Eval("健身教練圖片"),60) %>' />
                    </asp:LinkButton>
                    <div class="pl-4">
                        <h4 class="text-dark font-weight-bold mb-2"><%# Eval("健身教練姓名") %> 教練</h4>
                        <p class="text-muted mb-3"><%# Eval("健身教練介紹") %></p>
                    </div>
                </div>
                <!-- 新增的電話和 Email 區塊 -->
                <div class="ml-auto d-flex justify-content-end align-items-center">
                    <div class="text-center mx-4">
                        <img src="img/gender-fluid.png" alt="Gender Icon" class="icon-size" style="height: 40px;" draggable="false">
                        <h5 class="text-dark font-weight-bold mt-2">性別</h5>
                        <p class="text-muted"><%# GetGenderDescription(Eval("健身教練性別")) %></p>
                    </div>
                    <div class="text-center mx-4">
                        <img src="img/telephone.png" alt="Phone Icon" class="icon-size" style="height: 40px;" draggable="false">
                        <h5 class="text-dark font-weight-bold mt-2">電話</h5>
                        <p class="text-muted"><%# Eval("健身教練電話") %></p>
                    </div>
                    <div class="text-center mx-4">
                        <img src="img/email.png" alt="Email Icon" class="icon-size" style="height: 40px;" draggable="false">
                        <h5 class="text-dark font-weight-bold mt-2">Email</h5>
                        <p class="text-muted"><%# Eval("健身教練郵件") %></p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

        </ItemTemplate>
    </asp:Repeater>


    <div id="course-schedule">
    <div style="text-align: left; width: 100%; max-width: 800px;">
        <h3>課程時段</h3>
        <div class="repeater-container">
            
                    <asp:Calendar ID="Course_date_choose" runat="server" 
                                  OnSelectionChanged="Course_date_choose_SelectionChanged" 
                                  OnDayRender="Course_date_choose_DayRender" 
                                  CssClass="aspNetCalendar"
                        OnClientClick="ShowProgressBar();">
                    </asp:Calendar>
                    <br />
                    <asp:Repeater ID="TimeRepeater" runat="server" OnItemCommand="TimeRepeater_ItemCommand">
                        <ItemTemplate>
                            <div class="item">
                                <asp:Label ID="Course_all_date" runat="server" Text='<%#  Eval("日期 ","{0:yyyy/MM/dd}")  %>' Font-Size="Medium"></asp:Label>
                                <br />
                                <asp:Label ID="Course_all_week" runat="server" Text='<%#  Eval("星期幾") %>' Font-Size="Medium"></asp:Label>
                                <br />
                                <asp:Label ID="Course_all_stTime" runat="server" Text='<%#  Eval("開始時間") %>' Font-Size="Medium"></asp:Label>
                                <span>~</span>
                                <asp:Label ID="Course_all_edTime" runat="server" Text='<%#  Eval("結束時間") %>' Font-Size="Medium"></asp:Label>
                                <br />
                                <span>目前人數：</span>
                                <asp:Label ID="ap_all_people" runat="server" Text='<%# Eval("預約人數") %>' Font-Size="Medium"></asp:Label>
                                <span>/</span>
                                <asp:Label ID="course_all_people" runat="server" Text='<%#  Eval("上課人數") %>' Font-Size="Medium"></asp:Label>
                                <br />
                                <asp:Button ID="Appointment_btn" runat="server" Text="預約" CssClass="btn btn-outline-primary mt-2 px-3" CommandName="ap" CommandArgument='<%# Eval("課表編號") %>'  />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Label ID="noshow" runat="server" Text="本日無安排時段" Font-Size="40px" Visible="false"></asp:Label>
                
        </div>
    </div>
</div>


    <div style="display: flex; flex-direction: column; align-items: center; margin-top: 25px;">
        <div style="text-align: left; width: 100%; max-width: 800px;">
            <h3>課程地點</h3>
            <asp:Label ID="Course_all_Address" runat="server" Font-Size="Medium"></asp:Label><br>
            <asp:Label ID="Course_all_mapADD" runat="server" Font-Size="Medium"></asp:Label>
            <asp:HyperLink runat="server" ID="map" Target="_blank"><img src="img/maps.png" / height="25px" width="25px"></asp:HyperLink><br>
        </div>
    </div>

    <asp:Panel ID="AP_Panel" runat="server" CssClass="modal fade" tabindex="-1" role="dialog" aria-labelledby="ap_ModalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header bg-primary text-white">
                <h5 class="modal-title font-weight-bold text-white" id="ap_ModalLabel">請確認您的預約項目</h5>
            </div>
            <div class="modal-body">
                <div class="mb-3">
                    <span class="font-weight-bold">名稱：</span>
                    <asp:Label ID="ap_course_name" runat="server" Text='<%# Eval("課程名稱") %>' CssClass="text-large text-primary"></asp:Label>
                </div>
                <div class="mb-3">
                    <span class="font-weight-bold">時長：</span>
                    <asp:Label ID="ap_course_time" runat="server" Text='<%# Eval("課程時間長度") %>' CssClass="text-large text-primary"></asp:Label>
                    <span>分鐘</span>
                </div>
                <asp:Panel ID="ADD_Panel" runat="server" CssClass="mb-3">
                    <div class="mb-2">
                        <span class="font-weight-bold">地點：</span>
                        <asp:Label ID="ap_add_name" runat="server" Text='<%# Eval("地點名稱") %>' CssClass="text-large text-secondary"></asp:Label>
                    </div>
                    <div class="mb-2">
                        <span class="font-weight-bold">地址：</span>
                        <asp:Label ID="ap_add_city" runat="server" Text='<%# Eval("縣市") %>' CssClass="text-large"></asp:Label>
                        <asp:Label ID="ap_add_area" runat="server" Text='<%# Eval("行政區") %>' CssClass="text-large"></asp:Label>
                        <asp:Label ID="ap_course_add" runat="server" Text='<%# Eval("地點地址") %>' CssClass="text-large"></asp:Label>
                    </div>
                </asp:Panel>
                <div class="mb-3">
                    <asp:Label ID="Label1" runat="server" Text="預約時段：" CssClass="font-weight-bold text-dark"></asp:Label>
                    <asp:Label ID="ap_course_date" runat="server" Text='<%# Eval("日期") %>' CssClass="text-large"></asp:Label>
                    <span>(</span>
                    <asp:Label ID="ap_course_stTime" runat="server" Text='<%# Eval("開始時間") %>' CssClass="text-large"></asp:Label>
                    <span>~</span>
                    <asp:Label ID="ap_course_edTime" runat="server" Text='<%# Eval("結束時間") %>' CssClass="text-large"></asp:Label>
                    <span>)</span>
                </div>
                <div class="mb-3">
                    <asp:Label ID="Label3" runat="server" Text="給教練的留言：" CssClass="font-weight-bold text-dark"></asp:Label><br>
                    <asp:TextBox ID="ap_text" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="mb-3">
                    <asp:Label ID="Label4" runat="server" Text="到府服務地址：" CssClass="font-weight-bold text-dark"></asp:Label><br>
                    <asp:Label ID="home_city" runat="server" Text='<%# Eval("縣市") %>' Visible="false" CssClass="text-large"></asp:Label>
                    <asp:Label ID="home_area" runat="server" Text='<%# Eval("行政區") %>' Visible="false" CssClass="text-large"></asp:Label>
                    <asp:TextBox ID="ap_location" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="modal-footer">
                <asp:Button ID="ap_btn" runat="server" Text="確認預約" OnClick="ap_btn_Click" CssClass="btn btn-outline-primary form-control" OnClientClick="ShowProgressBar()" />
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

