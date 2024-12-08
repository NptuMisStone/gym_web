<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="coach_detail.aspx.cs" Inherits="page_coach_detail" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function scrollToReviewSection() {
            var reviewSection = document.getElementById('reviewSection'); // 確保 ID 匹配
            if (reviewSection) {
                reviewSection.scrollIntoView({ behavior: 'smooth' });
            }
        }
    </script>


    <style>
        .step-title {
            font-size: 2.5rem;
            font-weight: bold;
            color: #e31c25;
            border-bottom: 2px solid #e31c25;
            padding-bottom: 10px;
            margin-bottom: 15px;
        }

        .progress-container div {
            width: 0%;
            height: 20px;
            background-color: #ffcc00;
            transition: width 0.5s;
        }

        .progress-container {
            width: 100%;
            background-color: #e0e0e0;
            border-radius: 5px;
            overflow: hidden;
            text-align: left;
        }

        .btn-edit {
            background-color: pink;
        }

        .btn-editclick {
            background-color: darkred;
        }

        .datapagerStyle {
            color: gray;
        }
    </style>

    <!-- Page Header Start -->
    <div class="container-fluid page-header mb-5">
        <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 130px">
        </div>
    </div>
    <!-- Page Header End -->

    <asp:Repeater ID="rp_coach" runat="server" OnItemDataBound="rp_coach_ItemDataBound">
        <ItemTemplate>
            <div class="container pt-5">
                <div class="row">
                    <div class="col-md-6 pb-5 d-flex flex-column align-items-center">
                        <asp:Image ID="img_coach" runat="server" ImageUrl='<%# GetImageUrl(Eval("健身教練圖片"),60) %>'
                            CssClass="rounded-circle p-1"
                            Style="width: 400px; height: 400px; border: 5px solid #000; object-fit: cover;"
                            draggable="false" />
                    </div>
                    <div class="col-md-6 pb-5">
                        <h2 class="display-4 font-weight-bold mb-4"><%# Eval("健身教練姓名") %> 教練</h2>
                        <!-- 將愛心按鈕移到這裡 -->
                        <div style="display: flex; margin-bottom: 20px;">
                            <asp:ImageButton ID="LikeBtn" runat="server" ImageUrl="~/page/img/dislike2.png"
                                Style="width: 50px; height: 50px;" OnClick="LikeBtn_Click"
                                CommandArgument='<%# Eval("健身教練編號") %>' />

                        </div>
                        <p><%# Eval("健身教練介紹") %></p>
                        <!-- 健身教練性別 -->
                        <div class="d-flex align-items-center mb-4">
                            <img src="img/gender-fluid.png" class="mr-2" style="height: 30px; flex-shrink: 0;" draggable="false">
                            <div style="display: flex; flex-direction: column; justify-content: center;">
                                <h5 class="font-weight-bold mb-0" style="white-space: nowrap;"><%# GetGenderDescription(Eval("健身教練性別")) %></h5>
                            </div>
                        </div>

                        <!-- 健身教練電話 -->
                        <div class="d-flex align-items-center mb-4">
                            <img src="img/telephone.png" class="mr-2" style="height: 30px; flex-shrink: 0;" draggable="false">
                            <div style="display: flex; flex-direction: column; justify-content: center;">
                                <h5 class="font-weight-bold mb-0" style="white-space: nowrap;"><%# Eval("健身教練電話") %></h5>
                            </div>
                        </div>

                        <!-- 健身教練郵件 -->
                        <div class="d-flex align-items-center mb-4">
                            <img src="img/email.png" class="mr-2" style="height: 30px; flex-shrink: 0;" draggable="false">
                            <div style="display: flex; flex-direction: column; justify-content: center;">
                                <h5 class="font-weight-bold mb-0" style="white-space: nowrap;"><%# Eval("健身教練郵件") %></h5>
                            </div>
                        </div>

                        <!-- 健身教練身份 -->
                        <div class="d-flex align-items-center mb-4">
                            <img src="img/identification.png" class="mr-2" style="height: 30px; flex-shrink: 0;" draggable="false">
                            <div style="display: flex; flex-direction: column; justify-content: center;">
                                <h5 class="font-weight-bold mb-0" style="white-space: nowrap;"><%# Eval("註冊類型") %></h5>
                            </div>
                        </div>

                        <asp:Panel ID="Panel_store" runat="server">
                            <div class="row py-3">
                                <!-- 服務地點名稱 -->
                                <div class="col-sm-6 col-md-3 mb-3">
                                    <div class="d-flex align-items-start">
                                        <img src="img/home.png" class="mr-2" style="height: 30px; flex-shrink: 0;" draggable="false">
                                        <div style="display: flex; flex-direction: column;">
                                            <h5 class="font-weight-bold mb-0" style="white-space: nowrap;">服務店家</h5>
                                            <p class="mb-0"><%# Eval("服務地點名稱") %></p>
                                        </div>
                                    </div>
                                </div>
                                <!-- 服務地點地址 -->
                                <div class="col-sm-6 col-md-3 mb-3">
                                    <div class="d-flex align-items-start">
                                        <img src="img/maps.png" class="mr-2" style="height: 30px; flex-shrink: 0;" draggable="false">
                                        <div style="display: flex; flex-direction: column;">
                                            <h5 class="font-weight-bold mb-0" style="white-space: nowrap;">店家地址</h5>
                                            <p class="mb-0"><%# Eval("服務地點地址") %></p>
                                        </div>
                                    </div>
                                </div>
                                <!-- 服務地點電話 -->
                                <div class="col-sm-6 col-md-3 mb-3">
                                    <div class="d-flex align-items-start">
                                        <img src="img/telephone.png" class="mr-2" style="height: 30px; flex-shrink: 0;" draggable="false">
                                        <div style="display: flex; flex-direction: column;">
                                            <h5 class="font-weight-bold mb-0" style="white-space: nowrap;">店家電話</h5>
                                            <p class="mb-0" style="white-space: normal; word-wrap: break-word; word-break: break-all;">
                                                <%# Eval("服務地點電話") %>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                                <!-- 服務地點郵件 -->
                                <div class="col-sm-6 col-md-3 mb-3">
                                    <div class="d-flex align-items-start">
                                        <img src="img/email.png" class="mr-2" style="height: 30px; flex-shrink: 0;" draggable="false">
                                        <div style="display: flex; flex-direction: column;">
                                            <h5 class="font-weight-bold mb-0" style="white-space: nowrap;">店家郵件</h5>
                                            <p class="mb-0" style="white-space: normal; word-wrap: break-word; word-break: break-all;">
                                                <%# Eval("服務地點郵件") %>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <div class="container feature">
        <h5 class="step-title">教練課程</h5>
    </div>

    <!-- 教練尚未安排課程訊息 -->
<div class="d-flex align-items-center justify-content-center py-3 w-100">
    <asp:Label ID="lb_noClasses" runat="server" Text="教練尚未安排課程" ForeColor="Red" Font-Bold="True" Visible="False" Font-Size="36px"></asp:Label>
</div>
<!-- 課程列表 -->
<div class="container feature pt-1 w-100">
    <div class="row">
        <asp:ListView ID="lv_classes" runat="server" OnItemCommand="lv_classes_ItemCommand">
            <LayoutTemplate>
                <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
            </LayoutTemplate>
            <ItemTemplate>
                <div class="col-md-6 mb-3 d-flex justify-content-center">
                    <!-- 卡片 -->
                    <div class="card-class">
                        <!-- 點擊進入課程詳細頁面 -->
                        <asp:LinkButton ID="lb_class" runat="server" CommandName="ViewDetails" CommandArgument='<%# Eval("課程編號") %>' class="card-content no-underline">
                            <!-- 圖片與類型標籤 -->
                            <div class="card-image-container">
                                <asp:Image ID="Image1" runat="server" ImageUrl='<%# GetImageUrl2(Eval("課程圖片"),60) %>' class="card-image" />
                                <span class="class-type-label <%# Convert.ToInt32(Eval("地點類型")) == 2 ? "label-blue" : "label-red" %>">
                                    <%# Convert.ToInt32(Eval("地點類型")) == 2 ? "到府課程" : "團體課程" %>
                                </span>
                            </div>
                            <!-- 課程資訊 -->
                            <div class="card-info">
                                <h4 class="class-title"><%# Eval("課程名稱") %></h4>
                                <h4 class="class-price">$ <%# Convert.ToDouble(Eval("課程費用")).ToString("F0") %> /堂</h4>
                                <p class="class-description"><%# Eval("課程內容介紹") %></p>
                            </div>
                        </asp:LinkButton>
                    </div>
                </div>
            </ItemTemplate>
        </asp:ListView>
    </div>
</div>
    <style>
        /* 卡片樣式 */
.card-class {
    display: flex;
    flex-direction: column;
    width: 100%;
    max-width: 460px;
    border-radius: 8px;
    overflow: hidden;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    background-color: #fff;
    transition: transform 0.2s;
}

.card-class:hover {
    transform: translateY(-5px);
}

/* 圖片區域 */
.card-image-container {
    position: relative;
    height: 240px;
    overflow: hidden;
}

.card-image {
    width: 100%;
    height: 100%;
    object-fit: cover;
}

/* 動態標籤 */
.class-type-label {
    position: absolute;
    top: 12px;
    right: 12px;
    color: #fff;
    padding: 4px 8px;
    font-size: 14px;
    font-weight: bold;
    border-radius: 4px;
}

/* 到府課程 (藍底) */
.label-blue {
    background-color: #007bff; /* 藍色 */
}

/* 團體課程 (紅底) */
.label-red {
    background-color: #dc3545; /* 紅色 */
}

/* 課程資訊 */
.card-info {
    padding: 15px;
    height: 160px; /* 設置固定高度 */
    overflow: hidden; /* 隱藏超出高度的內容 */
    display: flex; /* 使用 Flexbox 保持內部元素整齊 */
    flex-direction: column; /* 確保內容垂直排列 */
    justify-content: space-between; /* 在可用空間中分配內容 */
}

.class-title {
    font-size: 20px;
    font-weight: bold;
    margin-bottom: 10px;
}

.class-price {
    font-size: 18px;
    font-weight: bold;
    color: #e31c25;
    margin-bottom: 10px;
}

/* 按鈕區域 */
.card-buttons {
    display: flex;
    justify-content: center; /* 只顯示愛心按鈕置中 */
    padding: 10px;
    background-color: #f9f9f9;
    border-top: 1px solid #ddd;
}

/* 按鈕 */
.btn-icon {
    background: none;
    border: none;
    cursor: pointer;
    width: 30px;
    height: 30px;
    object-fit: contain;
    transition: transform 0.2s;
}

.btn-icon:hover {
    transform: scale(1.1);
}

/* 卡片間距縮小 */
.col-md-6 {
    padding-left: 8px;
    padding-right: 8px;
    padding-bottom: 20px;
}

/* 移除選取課程的下劃線 */
.no-underline {
    text-decoration: none; /* 移除下劃線 */
    color: inherit; /* 繼承文字顏色 */
}

.no-underline:hover {
    text-decoration: none; /* 確保懸停時也沒有下劃線 */
}
/* 限制文字顯示為最多三行，並在超過時顯示省略號 */
.class-description {
    display: -webkit-box; /* 必須搭配，啟用彈性盒模型 */
    -webkit-line-clamp: 2; /* 限制行數 */
    -webkit-box-orient: vertical; /* 設置為垂直方向 */
    overflow: hidden; /* 隱藏超出範圍的內容 */
    text-overflow: ellipsis; /* 顯示省略號 */
    line-height: 1.5; /* 設置行高，與文字間距保持一致 */
    max-height: calc(1.5em * 2); /* 計算總高度，1.5em 是行高 */
}
    </style>



    <div class="container feature mt-3">
        <h5 class="step-title">評分與評論</h5>
    </div>

    <center>

        <div style="display: flex; flex-wrap: wrap; justify-content: center; padding-top: 50px; max-width: 1000px; margin: 0 auto;">

            <!-- 評分區塊 -->
            <div style="display: flex; flex-direction: column; align-items: center; padding-right: 20px;">
                <asp:Image ID="Image6" runat="server" Height="50px" Width="50px" ImageUrl="img/star_click.png" draggable="false" />
                <br />
                <asp:Label ID="lb_score" runat="server" Text='<%# Eval("平均評分") %>' Font-Size="X-Large" ForeColor="#FFC000" Font-Bold="True"></asp:Label>
                <br />
                <asp:Label ID="lb_comment_count" runat="server" Font-Bold="True" Font-Size="Medium" Text='<%# Eval("評論數量") %>' ForeColor="#FFC000"></asp:Label>
            </div>

            <!-- 星級評分區塊 -->
            <div style="flex: 1; display: flex; flex-direction: column; justify-content: space-around;">
                <!-- 每個評分條目 -->
                <div style="display: flex; align-items: center; margin-bottom: 5px;">
                    <span style="width: 30px; text-align: right;">5</span>
                    <div class="progress-container" style="flex: 1; margin-left: 10px;">
                        <div id="prog_star5"></div>
                    </div>
                    <asp:Literal ID="litProgStar5" runat="server" />
                </div>

                <div style="display: flex; align-items: center; margin-bottom: 5px;">
                    <span style="width: 30px; text-align: right;">4</span>
                    <div class="progress-container" style="flex: 1; margin-left: 10px;">
                        <div id="prog_star4"></div>
                    </div>
                    <asp:Literal ID="litProgStar4" runat="server" />
                </div>

                <div style="display: flex; align-items: center; margin-bottom: 5px;">
                    <span style="width: 30px; text-align: right;">3</span>
                    <div class="progress-container" style="flex: 1; margin-left: 10px;">
                        <div id="prog_star3"></div>
                    </div>
                    <asp:Literal ID="litProgStar3" runat="server" />
                </div>

                <div style="display: flex; align-items: center; margin-bottom: 5px;">
                    <span style="width: 30px; text-align: right;">2</span>
                    <div class="progress-container" style="flex: 1; margin-left: 10px;">
                        <div id="prog_star2"></div>
                    </div>
                    <asp:Literal ID="litProgStar2" runat="server" />
                </div>

                <div style="display: flex; align-items: center; margin-bottom: 5px;">
                    <span style="width: 30px; text-align: right;">1</span>
                    <div class="progress-container" style="flex: 1; margin-left: 10px;">
                        <div id="prog_star1"></div>
                    </div>
                    <asp:Literal ID="litProgStar1" runat="server" />
                </div>
            </div>
        </div>



        <div style="max-width: 1000px; margin: 50px auto; text-align: center;">
            <!-- 評論按鈕面板 -->
            <div style="display: flex; justify-content: center; margin-bottom: 20px;">
                <asp:Panel ID="pn_comment_btn" runat="server">
                    <asp:Label ID="Label6" runat="server" Text="排序"></asp:Label><br />
                    <asp:Button ID="btn_my_comment" runat="server" Text="我的評論" CssClass="btn btn-outline-primary mt-2 px-3" Font-Size="Large" Height="50px" Width="120px" OnClick="btn_my_comment_Click" />
                    &nbsp;&nbsp;
                   
                    <asp:Button ID="btn_new_comment" runat="server" Text="最新評分" CssClass="btn btn-outline-primary mt-2 px-3" Font-Size="Large" Height="50px" Width="120px" OnClick="btn_new_comment_Click" />
                    &nbsp;&nbsp;
                   
                    <asp:Button ID="btn_higher_comment" runat="server" Text="最高評分" CssClass="btn btn-outline-primary mt-2 px-3" Font-Size="Large" Height="50px" Width="120px" OnClick="btn_higher_comment_Click" />
                    &nbsp;&nbsp;
                   
                    <asp:Button ID="btn_low_comment" runat="server" Text="最低評分" CssClass="btn btn-outline-primary mt-2 px-3" Font-Size="Large" Height="50px" Width="120px" OnClick="btn_low_comment_Click" />
                </asp:Panel>
            </div>


            <!-- 評論列表 -->
            <div style="text-align: left; margin: 0 auto;">
                <asp:ListView ID="rp_comment" runat="server" OnItemCommand="rp_comment_ItemCommand" OnItemDeleting="rp_comment_ItemDeleting" OnPagePropertiesChanging="rp_comment_PagePropertiesChanging">
                    <ItemTemplate>
                        <div style="border-bottom: 1px solid #C9C9C9; padding-bottom: 20px; margin-bottom: 20px;">
                            <!-- 用戶信息 -->
                            <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 15px;">
                                <div style="display: flex; align-items: center;">
                                    <asp:Image ID="Image2" runat="server" Height="50px" Width="50px" CssClass="circular-image" draggable="false"
                                        ImageUrl='<%# GetImageUrl(Eval("使用者圖片"),20) %>' ImageAlign="Bottom" Style="object-fit: cover; border-radius: 50%; margin-right: 15px;" />
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("使用者姓名") %>' Font-Size="Large" Font-Bold="True"></asp:Label>
                                </div>
                                <!-- 修改刪除按鈕 -->
                                <asp:PlaceHolder ID="ph_user_edit" runat="server" Visible='<%# has_comment(Eval("評論編號"))%>'>
                                    <div style="display: flex; gap: 15px;">
                                        <asp:Button ID="Button1" runat="server" Text="修改" CssClass="btn btn-outline-primary px-4 py-2" CommandName="edit" CommandArgument='<%# Eval("預約編號") %>' />
                                        <asp:Button ID="Button2" runat="server" Text="刪除" CssClass="btn btn-outline-danger px-4 py-2" CommandName="delete" CommandArgument='<%# Eval("預約編號") %>' />
                                    </div>
                                </asp:PlaceHolder>
                                <!-- 檢舉按鈕 -->
                                <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                                    <div style="display: flex; gap: 15px;">
                                        <asp:ImageButton ID="ImageButton1" runat="server" AlternateText="檢舉" ImageUrl="~/page/img/report.png" CommandName="report" CommandArgument='<%# Eval("評論編號") %>' Width="30px" Height="30px" />
                                    </div>
                                </asp:PlaceHolder>
                            </div>

                            <!-- 評分及日期 -->
                            <div style="display: flex; align-items: center; gap: 15px;">
                                <asp:Image ID="Image7" runat="server" Height="25" Width="150" ImageUrl='<%# Getstar_img(Eval("評分"))  %>' draggable="false" />
                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("評論日期", "{0:yyyy/MM/dd}") %>' Font-Size="Large"></asp:Label>
                            </div>

                            <!-- 課程名稱及評論內容 -->
                            <asp:Label ID="Label13" runat="server" Text='<%# "課程名稱："+Eval("課程名稱") %>' Font-Size="Large" ForeColor="#ACACAC" Style="display: block; margin-top: 15px;"></asp:Label>
                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("評論內容") %>' Font-Size="Large" Style="display: block; margin-top: 10px;"></asp:Label>

                            <!-- 教練回覆 -->
                            <asp:PlaceHolder ID="phCoachReply" runat="server" Visible='<%# Has_reply(Eval("評論編號"))%>'>
                                <div style="background-color: #E6E3BB; border-radius: 10px; padding: 15px; margin-top: 15px;">
                                    <asp:Label ID="Label3" runat="server" Text="健身教練的回覆：" Font-Size="Large" Font-Bold="True" Style="display: block; font-weight: bold;"></asp:Label>
                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("回覆") %>' Font-Size="Large" Style="display: block; word-wrap: break-word;"></asp:Label>
                                </div>
                            </asp:PlaceHolder>
                        </div>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <!-- 分頁控制 -->
            <div style="margin-top: 20px;">
                <asp:DataPager ID="DataPager2" runat="server" PagedControlID="rp_comment" PageSize="3">
                    <Fields>
                        <asp:NumericPagerField NumericButtonCssClass="datapagerStyle" />
                    </Fields>
                </asp:DataPager>
            </div>
        </div>
    </center>
    <br />
</asp:Content>

