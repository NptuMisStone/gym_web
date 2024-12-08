<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="Coach_class.aspx.cs" Inherits="Coach_Coach_class" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .step-title {
            font-size: 1.5rem;
            font-weight: bold;
            color: #e31c25;
            border-bottom: 2px solid #e31c25;
            padding-bottom: 10px;
            margin-bottom: 15px;
            text-align: left;
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
                text-decoration: none; /* 去掉下劃線 */
                outline: none; /* 去掉選取時的外框 */
            }
    </style>
    <!-- Page Header Start -->
    <div class="container-fluid page-header mb-1">
        <div class="d-flex flex-column align-items-center justify-content-center" style="min-height: 400px">
            <h4 class="display-4 mb-3 mt-0 text-white text-uppercase font-weight-bold">課程維護</h4>
        </div>
    </div>

    <!-- GYM Class Start -->
    <div class="container feature py-2">
        <div class="mb-3">
            <asp:Button ID="BtnBack" runat="server" Text="←返回首頁" CssClass="btn-back-home px-4 py-2" PostBackUrl="~/Coach/Coach_index.aspx" />
        </div>
        <h5 class="mb-4 step-title">我的所有課程</h5>
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
                                    <p class="class-price">$ <%# Convert.ToDouble(Eval("課程費用")).ToString("F0") %> /堂</p>
                                    <p><%# Eval("分類名稱") %></p>
                                    <p><%# "課程時長：" +Eval("課程時間長度") %></p>
                                    <p><%# "人數：" + Eval("上課人數") + "人"%></p>
                                    <p><%# "所需設備：" + Eval("所需設備") %></p>
                                    <p class="class-description"><%# Eval("課程內容介紹") %></p>
                                </div>
                            </asp:LinkButton>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </div>


        <div class="text-center">
            <asp:Button ID="btnAdd" runat="server" Text="＋ 新增課程" CssClass="btn btn-outline-primary mt-2 px-3" Style="height: 70px; width: 320px; font-size: 36px; font-weight: bold;" OnClick="btnAdd_Click" />
        </div>
    </div>

    <style>
        /* 卡片樣式 */
        .card-class {
            display: flex;
            flex-direction: column;
            width: 100%;
            max-width: 485px;
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

        .label-blue {
            background-color: #007bff; /* 到府課程 (藍底) */
        }

        .label-red {
            background-color: #dc3545; /* 團體課程 (紅底) */
        }

        /* 課程資訊 */
        .card-info {
            padding: 15px;
            height: 300px; /* 固定高度 */
            overflow: hidden;
            display: flex;
            flex-direction: column;
            justify-content: space-between;
        }
        .card-info p {
    margin: 5px 0; /* 設置上下邊距為5px，左右邊距為0 */
    line-height: 1.5; /* 調整行高 */
    font-size: 14px; /* 調整字體大小，視需要而定 */
}


        .class-title {
            font-size: 20px;
            font-weight: bold;
            margin-bottom: 10px;
        }

        .class-description {
    display: -webkit-box; /* 必須啟用 Flexbox */
    -webkit-line-clamp: 1; /* 限制行數為三行 */
    -webkit-box-orient: vertical; /* 垂直排列 */
    overflow: hidden; /* 隱藏超出部分 */
    text-overflow: ellipsis; /* 顯示省略號 */
    line-height: 1.5; /* 行高 */
    max-height: calc(1.5em * 1); /* 計算三行的總高度 */
}


        .class-price {
            font-size: 18px;
            font-weight: bold;
            color: #e31c25;
        }

        /* 按鈕區域 */
        .card-buttons {
            display: flex;
            justify-content: center;
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
            text-decoration: none;
            color: inherit;
        }

            .no-underline:hover {
                text-decoration: none;
            }
    </style>
</asp:Content>
