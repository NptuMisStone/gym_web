<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="class.aspx.cs" Inherits="page_class" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        /* 灰色覆蓋層 */
        .overlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.5);
            z-index: 999;
        }

        .close-button {
            float: right;
            font-size: 50px;
            background: none;
            border: none;
            cursor: pointer;
        }
        /* 側邊彈窗 */
        .side-panel {
            position: fixed;
            top: 0;
            left: -300px; /* 初始位置在頁面外 */
            width: 300px;
            height: 100%;
            background-color: white;
            box-shadow: -2px 0 5px rgba(0,0,0,0.5);
            z-index: 1000;
            transition: left 0.5s; /* 動畫效果 */
        }

        /* 弹窗内容 */
        .panel-content {
            padding: 20px;
            max-height: 100%; /* 內容的最大高度 */
            overflow-y: auto; /* 內容超出時，啟用滾動 */
        }

        .btn-hide {
            display: none;
        }

        .cancel-btn,
        .submit-btn {
            padding: 12px 20px;
            border: none;
            border-radius: 5px;
            font-size: 16px;
            cursor: pointer;
            width: 48%; /* 确保按钮不会超出父容器的宽度 */
        }

        /* 提交按钮 */
        .submit-btn {
            background-color: #28a745;
            color: white;
        }

            .submit-btn:hover {
                background-color: #218838;
            }

        /* 取消按钮 */
        .cancel-btn {
            background-color: #dc3545;
            color: white;
        }

            .cancel-btn:hover {
                background-color: #c82333;
            }

        .filter-item input, .filter-item select, .filter-item textarea {
            width: 100%;
            padding: 5px;
            border: 2px solid #ccc;
            border-radius: 5px;
            font-size: 14px;
        }
    </style>
    <style>
        .side-panel {
            background-color: #f5f8fa; /* 柔和的淺藍背景色 */
            border: 1px solid #cce5ff; /* 淺藍邊框 */
            border-radius: 8px;
            padding: 20px;
            width: 300px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

        .panel-header {
            text-align: center;
            margin-bottom: 15px;
            color: #0056b3; /* 深藍標題 */
            font-weight: bold;
        }

        .filter-label {
            font-weight: bold;
            color: #333; /* 深灰標籤色 */
        }

        .filter-input, .filter-dropdown {
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 5px;
            font-size: 14px;
            transition: border-color 0.3s ease;
        }

            .filter-input:focus, .filter-dropdown:focus {
                border-color: #80bdff; /* 聚焦淺藍色 */
                outline: none;
            }

        .filter-radio {
            display: flex;
            gap: 10px;
        }

        .filter-range {
            display: flex;
            align-items: center;
            gap: 5px;
        }

        .filter-buttons {
            position: relative; /* 相對位置 */
            margin-top: 20px;
            padding: 10px 0;
            background: #f5f8fa; /* 與整體背景一致 */
            display: flex;
            justify-content: space-between;
            gap: 10px;
            margin-bottom: 30px; /* 增加按鈕與底部距離 */
            border-top: 1px solid #cce5ff; /* 分隔線 */
        }

        .cancel-btn {
            background-color: #f5f5f5;
            color: #333;
            font-weight: bold;
            padding: 12px 20px;
        }

        .submit-btn {
            background-color: #007bff;
            color: white;
            font-weight: bold;
            padding: 12px 20px;
        }

            .submit-btn:hover {
                background-color: #0056b3;
            }

        .cancel-btn:hover {
            background-color: #e0e0e0;
        }

        .side-panel {
            background-color: #f9f9f9; /* 較柔和的背景色 */
            border: 1px solid #ddd;
            border-radius: 8px;
            padding: 20px;
            width: 300px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

        .panel-header {
            text-align: center;
            margin-bottom: 15px;
            color: #333;
            font-weight: bold;
        }

        .panel-content {
            display: flex;
            flex-direction: column;
            gap: 15px;
        }

        .filter-section {
            display: flex;
            flex-direction: column;
            gap: 5px;
        }

        .filter-label {
            font-weight: bold;
            color: #555;
        }

        .filter-input, .filter-dropdown {
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 5px;
            font-size: 14px;
            transition: border-color 0.3s ease;
        }

            .filter-input:focus, .filter-dropdown:focus {
                border-color: #007bff;
                outline: none;
            }

        .filter-radio {
            display: flex;
            gap: 10px;
        }

        .filter-buttons {
            position: sticky;
            bottom: 0; /* 固定位置 */
            background: #f9f9f9;
            padding: 10px 0;
            display: flex;
            justify-content: space-between;
            gap: 10px;
            border-top: 1px solid #ddd; /* 分隔線 */
        }

        .cancel-btn {
            background-color: #f5f5f5;
            color: #555;
            font-weight: bold;
        }

        .submit-btn {
            background-color: #007bff;
            color: #fff;
            font-weight: bold;
        }

            .submit-btn:hover {
                background-color: #0056b3;
            }

        .cancel-btn:hover {
            background-color: #e0e0e0;
        }
        /* 區塊分隔線樣式 */
        .section-divider {
            border-bottom: 2px solid red; /* 紅色底線 */
            margin-bottom: 20px; /* 區塊之間的距離 */
            padding-bottom: 15px; /* 增加區塊內的下方間距 */
        }
    </style>
    <script>
        // 顯示弹窗
        function showPopup() {
            document.getElementById('overlay').style.display = 'block';
            var panel = document.getElementById('<%= sidePanel.ClientID %>');
            panel.style.display = 'block';
            document.body.style.overflow = 'hidden'; // 禁止頁面滾動
            setTimeout(function () {
                panel.style.left = '0'; // 右邊滑出
            }, 10);
        }

        // 隐藏弹窗
        function hidePopup() {
            document.getElementById('overlay').style.display = 'none';
            var panel = document.getElementById('<%= sidePanel.ClientID %>');
            panel.style.left = '-300px'; // 滑回去XD
            setTimeout(function () {
                panel.style.display = 'none';
                document.body.style.overflow = ''; // 恢復頁面滾動
            }, 500); // 延遲
        }
        function triggerPostBack() {
            __doPostBack('overlayClicked', '');
        }
    </script>
    <script>
        function validateAndHidePopup() {
            var minMoney = document.getElementById('<%= MinMoney.ClientID %>').value;
            var maxMoney = document.getElementById('<%= MaxMoney.ClientID %>').value;
            if (minMoney !== "" && maxMoney !== "") {
                var min = parseInt(minMoney, 10);
                var max = parseInt(maxMoney, 10);

                if (min > max) {
                    alert('最小值不能大於最大值');
                    return false;
                }
            }

            // 如果验证通过，隐藏弹窗
            hidePopup();
            return true;
        }
    </script>
    <script type="text/javascript">
        function validateInput(textbox) {
            var value = parseInt(textbox.value, 10);

            // 如果輸入不是數字或為空，直接返回，不修改值
            if (isNaN(value) || textbox.value === "") {
                return;
            }
            // 檢查是否超出範圍
            if (value > 9999) {
                textbox.value = 9999;  // 將值限制為9999
            } else if (value < 0) {
                textbox.value = 0;     // 將值限制為0
            }
        }
    </script>

    <!-- Page Header Start -->
    <div class="container-fluid page-header mb-5">
        <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 400px">
            <h4 class="display-4 mb-3 mt-0 mt-lg-5 text-white text-uppercase font-weight-bold">尋找課程</h4>
        </div>
    </div>
    <!-- Page Header End -->

    <!-- GYM Class Start -->
    <div class="container feature pt-5 w-100" style="position: relative;">
        <div style="display: flex; justify-content: center; align-items: center; border: 2px solid #000000; border-radius: 10px; padding: 10px; width: 70%; margin: 0 auto;">
            <!-- 調整 TextBox 大小 -->
            <asp:TextBox ID="SearchText" runat="server" placeholder="搜尋..."
                Style="border: none; outline: none; width: 90%; font-size: 18px; padding: 10px;" />
            <!-- 調整 ImageButton 大小 -->
            <asp:ImageButton ID="SearchBtn" runat="server" ImageUrl="~/page/img/search.png"
                Style="width: 30px; height: 30px;" OnClick="SearchBtn_Click" />
        </div>
        <asp:ImageButton ID="FilterBtn" runat="server" ImageUrl="~/page/img/filter.png"
            AlternateText="篩選" Style="position: absolute; top: 67.5%; right: 10%; transform: translateY(-50%); width: 50px; height: 50px;"
            OnClientClick="showPopup(); return false;" OnClick="FilterBtn_Click" />
    </div>

    <!-- 灰色覆蓋層 -->
    <div id="overlay" class="overlay" style="display: none;" onclick="hidePopup();return false; ">
        <!--點灰色部分也可以關閉，但會清空篩選-->

    </div>
    <!-- 彈窗內容 -->
    <!-- 彈窗內容 -->
    <asp:Panel ID="sidePanel" runat="server" CssClass="side-panel" Style="display: none;">
        <div class="panel-header">
            <h2>篩選</h2>
        </div>
        <div class="panel-content">
            <!-- 課程類型區塊 -->
            <div class="filter-section section-divider">
                <asp:Label ID="Label1" runat="server" Text="課程類型" CssClass="filter-label"></asp:Label>
                <asp:DropDownList ID="ClassTypeDDL" runat="server" CssClass="filter-dropdown"></asp:DropDownList>
            </div>

            <div class="filter-section section-divider">
                <asp:Label ID="Label2" runat="server" Text="教練性別" CssClass="filter-label"></asp:Label>
                <asp:RadioButtonList ID="CoachGenderRB" runat="server" RepeatDirection="Horizontal" CssClass="filter-radio">
                    <asp:ListItem Value="" Selected="True">全部</asp:ListItem>
                    <asp:ListItem Value="1">男</asp:ListItem>
                    <asp:ListItem Value="2">女</asp:ListItem>
                    <asp:ListItem Value="3">其他</asp:ListItem>
                </asp:RadioButtonList>
            </div>

            <!-- 課程費用區塊 -->
            <div class="filter-section section-divider">
                <asp:Label ID="Label3" runat="server" Text="課程費用" CssClass="filter-label"></asp:Label>
                <div class="filter-range">
                    <asp:TextBox ID="MinMoney" runat="server" placeholder="最小值" CssClass="filter-input" TextMode="Number" min="0" max="9999" oninput="validateInput(this)"></asp:TextBox>
                    <span class="range-separator">~</span>
                    <asp:TextBox ID="MaxMoney" runat="server" placeholder="最大值" CssClass="filter-input" TextMode="Number" min="0" max="9999" oninput="validateInput(this)"></asp:TextBox>
                </div>
            </div>

            <!-- 課程人數區塊 -->
            <div class="filter-section  section-divider">
                <asp:Label ID="Label4" runat="server" Text="課程人數" CssClass="filter-label"></asp:Label>
                <asp:RadioButtonList ID="ClassPeopleRBL" runat="server" RepeatDirection="Horizontal" CssClass="filter-radio">
                    <asp:ListItem Value="0" Selected="True">全部</asp:ListItem>
                    <asp:ListItem Value="1">一對一</asp:ListItem>
                    <asp:ListItem Value="2">團體</asp:ListItem>
                </asp:RadioButtonList>
            </div>

            <!-- 課程地點區塊 -->
            <div class="filter-section">
                <asp:Label ID="Label5" runat="server" Text="課程地點" CssClass="filter-label"></asp:Label>
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" CssClass="filter-update-panel">
                    <ContentTemplate>
                        <asp:TreeView ID="CityTreeView" runat="server" OnTreeNodeCheckChanged="CityTreeView_TreeNodeCheckChanged" ShowCheckBoxes="All" EnableViewState="true" CssClass="filter-tree"></asp:TreeView>
                        <asp:Button ID="Button1" runat="server" Text="" CssClass="btn-hide" OnClick="Button1_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <!-- 按鈕區塊 -->
            <div class="filter-buttons">
                <asp:Button ID="Button2" runat="server" Text="重置" OnClientClick="hidePopup(); triggerPostBack();" CssClass="cancel-btn" />
                <asp:Button ID="SearchFilterBtn" runat="server" Text="查詢" OnClientClick="return validateAndHidePopup();" OnClick="SearchFilterBtn_Click" ValidationGroup="FilterValidation" CssClass="submit-btn" />
            </div>
        </div>
    </asp:Panel>


    <div class="container feature pt-5 w-100">
    <div class="row">
        <asp:ListView ID="lv_class" runat="server" OnItemCommand="lv_class_ItemCommand">
            <LayoutTemplate>
                <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
            </LayoutTemplate>
            <ItemTemplate>
                <div class="col-md-6 mb-3 d-flex justify-content-center">
                    <!-- 卡片 -->
                    <div class="card-class">
                        <!-- 點擊進入課程詳細頁面 -->
                        <asp:LinkButton ID="lb_class" runat="server" CommandName="see_detail" CommandArgument='<%# Eval("課程編號") %>' class="card-content no-underline">
                            <!-- 圖片與類型標籤 -->
                            <div class="card-image-container">
                                <asp:Image ID="Image1" runat="server" ImageUrl='<%# GetclassImageUrl(Eval("課程圖片"),60) %>' class="card-image" />
                                <span class="class-type-label <%# Convert.ToInt32(Eval("地點類型")) == 2 ? "label-blue" : "label-red" %>">
                                    <%# Convert.ToInt32(Eval("地點類型")) == 2 ? "到府課程" : "團體課程" %>
                                </span>
                            </div>
                            <!-- 課程資訊 -->
                            <div class="card-info">
                                <h4 class="class-title"><%# Eval("課程名稱") %></h4>
                                <div class="coach-info">
                                    <img src='<%# GetcoachImageUrl(Eval("健身教練圖片"),20) %>' alt="教練圖片" class="coach-image">
                                    <span class="coach-name"><%# Eval("健身教練姓名") %> 教練</span>
                                </div>
                                <p class="class-price">$ <%# Convert.ToDouble(Eval("課程費用")).ToString("F0") %> /堂</p>
                                <p class="class-description"><%# Eval("課程內容介紹") %></p>
                            </div>
                        </asp:LinkButton>
                        <!-- 按鈕區域 -->
                        <div class="card-buttons">
                            <asp:ImageButton
                                ID="LikeBtn"
                                runat="server"
                                CommandArgument='<%# Eval("課程編號") %>'
                                OnClick="LikeBtn_Click"
                                ImageUrl='<%# GetLikeImageUrl(Eval("課程編號")) %>'
                                class="btn-icon" />
                        </div>
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
    height: 250px; /* 設置固定高度 */
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

.coach-info {
    display: flex;
    align-items: center;
    margin-bottom: 10px;
}

.coach-image {
    width: 36px;
    height: 36px;
    border-radius: 50%;
    margin-right: 8px;
    object-fit: cover;
    border: 2px solid #ddd;
}

.coach-name {
    font-size: 16px;
    color: #555;
}

.class-price {
    font-size: 18px;
    font-weight: bold;
    color: #e31c25;
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
    -webkit-line-clamp: 3; /* 限制行數 */
    -webkit-box-orient: vertical; /* 設置為垂直方向 */
    overflow: hidden; /* 隱藏超出範圍的內容 */
    text-overflow: ellipsis; /* 顯示省略號 */
    line-height: 1.5; /* 設置行高，與文字間距保持一致 */
    max-height: calc(1.5em * 3); /* 計算總高度，1.5em 是行高 */
}
    </style>
</asp:Content>
