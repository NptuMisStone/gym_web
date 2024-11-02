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
            <!-- 使用 col-md-6 控制每個課程寬度為6格 (半行寬) -->
            <div class="col-md-6 mb-4" style="display: flex; justify-content: center;">
                <!-- 調整 linkbtn 的 style 確保不會影響內部元素 -->
                <!-- 將這裡的框線改為黑色且加粗 -->
                <div style="width: 100%; transition: background-color 0.3s ease; border: 2px solid black; border-radius: 8px; overflow: hidden;"
                    onmouseover="this.style.backgroundColor='#f0f0f0'"
                    onmouseout="this.style.backgroundColor=''">
                    <!-- 將背景色過渡效果與懸停效果直接加在 style 和事件屬性中 -->
                    <asp:LinkButton ID="lb_class" runat="server" CommandName="see_detail" CommandArgument='<%# Eval("課程編號") %>'
                        CssClass="unstyled-link"
                        Style="display: block; text-align: left; text-decoration: none; cursor: pointer;">
                        <div class="row align-items-center" style="padding: 20px;">
                            <div class="col-sm-6" style="padding: 10px 15px;">
                                <asp:Image ID="Image1" runat="server" ImageUrl='<%# GetImageUrl(Eval("課程圖片"),60) %>' CssClass="img-fluid mb-3 mb-sm-0" Style="object-fit: cover; height: 130px; width: 100%;" />
                                <%# Convert.ToInt32(Eval("上課人數")) == 1 ? 
                                    "<i style='font-size:20px; font-weight: bold;'>一對一</i>" : 
                                    "<i style='font-size:20px; font-weight: bold;'>團體</i>" %>
                            </div>
                            <div class="col-sm-6">
                            <h4 class="font-weight-bold"><%# Eval("課程名稱") %></h4>
                            <p><%# Eval("分類名稱") %></p>
                            <p><%# Eval("課程內容介紹") %></p>
                            <p><%# "課程時長：" +Eval("課程時間長度") %></p>
                            <p><%# "人數：" + Eval("上課人數") + "人"%></p>
                            <p><%# "所需設備：" + Eval("所需設備") %></p>
                            <p><%# "$" + Convert.ToDouble(Eval("課程費用")).ToString("F0") %></p>
                        </div>
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
</asp:Content>
