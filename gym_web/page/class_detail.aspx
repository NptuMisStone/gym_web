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

    <asp:Repeater ID="rp_class" runat="server" OnItemCommand="rp_class_ItemCommand">
        <ItemTemplate>
            <!-- Class Start -->
            <div class="container py-5">
                <div class="row align-items-center">
                    <div class="col-lg-6">
                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# GetImageUrl(Eval("課程圖片"),80) %>' class="img-fluid mb-4 mb-lg-0" Style="object-fit: cover; height: 500%; width: 500%;" />
                    </div>
                    <div class="col-lg-6">
                        <h2 class="display-4 font-weight-bold mb-4"><%# Eval("課程名稱") %></h2>
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

            <div class="col-md-12 bg-secondary">
                <div class="d-flex align-items-center px-3" style="min-height: 100px;">
                    <!-- 調整 min-height -->
                    <div class="container py-3 w-100">
                        <!-- 使用 d-flex 將所有內容放置在同一排 -->
                        <div class="d-flex align-items-center text-white w-100">
                            <!-- 教練圖片及資訊 -->
                            <div class="d-flex align-items-center">
                                <asp:LinkButton ID="lb_image" runat="server" CommandName="select_coach" CommandArgument='<%# Eval("健身教練編號") %>'>
                                    <asp:Image ID="Image2" runat="server" Style="object-fit: cover" Height="200px" Width="200px" class="rounded-circle bg-dark p-1" ImageUrl='<%# GetImageUrl(Eval("健身教練圖片"),60) %>' />
                                </asp:LinkButton>
                                <div class="pl-4 d-flex flex-column justify-content-center">
                                    <h4 class="text-primary mb-3" style="font-weight: bold;"><%# Eval("健身教練姓名") %> 教練</h4>
                                    <p class="m-0 mb-3"><%# Eval("健身教練介紹") %></p>
                                </div>
                            </div>
                            <!-- 新增的電話和 Email 區塊 -->
                            <div class="ml-auto d-flex">
                                <div class="text-center mx-5">
                                    <!-- 調整 mx-3 為 mx-5 -->
                                    <img src="img/gender-fluid.png" alt="Barbell Icon" class="display-2 text-primary" style="height: 50px" draggable="false">
                                    <h4 class="font-weight-bold" style="color: #e31c25">性別</h4>
                                    <p><%# GetGenderDescription(Eval("健身教練性別")) %></p>
                                </div>
                                <div class="text-center mx-5">
                                    <!-- 調整 mx-3 為 mx-5 -->
                                    <img src="img/telephone.png" alt="Barbell Icon" class="display-2 text-primary" style="height: 50px" draggable="false">
                                    <h4 class="font-weight-bold" style="color: #e31c25">電話</h4>
                                    <p><%# Eval("健身教練電話") %></p>
                                </div>
                                <div class="text-center mx-5">
                                    <!-- 調整 mx-3 為 mx-5 -->
                                    <img src="img/email.png" alt="Barbell Icon" class="display-2 text-primary" style="height: 50px" draggable="false">
                                    <h4 class="font-weight-bold" style="color: #e31c25">Email</h4>
                                    <p><%# Eval("健身教練郵件") %></p>
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
            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Calendar ID="Course_date_choose" runat="server" 
                                  OnSelectionChanged="Course_date_choose_SelectionChanged" 
                                  OnDayRender="Course_date_choose_DayRender" 
                                  CssClass="aspNetCalendar">
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
                    <asp:Label ID="noshow" runat="server" Text="無時段" Font-Size="100px" Visible="false"></asp:Label>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="TimeRepeater" />
                </Triggers>
            </asp:UpdatePanel>
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
            <div class="modal-header">
                <h5 class="modal-title" id="ap_ModalLabel">請確認您的預約項目</h5>
            </div>
            <div class="modal-body">
                <span>名稱：</span>
                <asp:Label ID="ap_course_name" runat="server"  Text='<%#  Eval("課程名稱")  %>' Font-Size="Large"></asp:Label><br>
                <span>時長：</span>
                <asp:Label ID="ap_course_time" runat="server"  Text='<%#  Eval("課程時間長度")  %>' Font-Size="Large"></asp:Label>
                <span>分鐘</span>
                <br/>
                <asp:Panel ID="ADD_Panel" runat="server">
                    <span>地點：</span>
                    <asp:Label ID="ap_add_name" runat="server"  Text='<%#  Eval("地點名稱")  %>' Font-Size="Large"></asp:Label><br>
                    <span>地址：</span>
                    <asp:Label ID="ap_add_city" runat="server" Text='<%#  Eval("縣市")  %>' Font-Size="Large"></asp:Label>
                    <asp:Label ID="ap_add_area" runat="server" Text='<%#  Eval("行政區")  %>' Font-Size="Large"></asp:Label>
                    <asp:Label ID="ap_course_add" runat="server"  Text='<%#  Eval("地點地址")  %>' Font-Size="Large"></asp:Label><br>
                </asp:Panel>
                <asp:Label ID="Label1" runat="server"  Text="預約時段：" Font-Size="Large"></asp:Label>
                <asp:Label ID="ap_course_date" runat="server"  Text='<%#  Eval("日期")  %>' Font-Size="Large"></asp:Label>
                <span>(</span>
                <asp:Label ID="ap_course_stTime" runat="server"  Text='<%#  Eval("開始時間")  %>' Font-Size="Large"></asp:Label>
                <span>~</span>
                <asp:Label ID="ap_course_edTime" runat="server"  Text='<%#  Eval("結束時間")  %>' Font-Size="Large"></asp:Label>
                <span>)</span>
                <br />
                <asp:Label ID="Label3" runat="server"  Text="給教練的留言：" Font-Size="Large"></asp:Label><br>
                <asp:TextBox ID="ap_text" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="Label4" runat="server"  Text="到府服務地址：" Font-Size="Large"></asp:Label><br>
                <asp:Label ID="home_city" runat="server" Text='<%#  Eval("縣市")  %>' Visible="false" Font-Size="Large"></asp:Label>
                <asp:Label ID="home_area" runat="server" Text='<%#  Eval("行政區")  %>' Visible="false" Font-Size="Large"></asp:Label>
                <asp:TextBox ID="ap_location" runat="server" Enabled="false"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvlocation" runat="server" ControlToValidate="ap_location" ErrorMessage="到府地址不得為空" ForeColor="Red" Display="Dynamic" Enabled="false"  />
                <br />
                <br />
        
            <div class="modal-footer">
                <asp:Button ID="ap_btn" runat="server" Text="確認預約" OnClick="ap_btn_Click" CssClass="btn btn-outline-primary mt-2 px-3" />
            </div>
        </div>
    </div>
</div>
</asp:Panel>

</asp:Content>

