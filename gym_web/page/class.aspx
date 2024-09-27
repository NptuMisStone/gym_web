<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="class.aspx.cs" Inherits="page_class" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Page Header Start -->
    <div class="container-fluid page-header mb-5">
        <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 400px">
            <h4 class="display-4 mb-3 mt-0 mt-lg-5 text-white text-uppercase font-weight-bold">尋找課程</h4>
        </div>
    </div>
    <!-- Page Header End -->

    <!-- GYM Class Start -->
    <div class="container feature pt-5 w-100">
        <div style="display: flex;justify-content: center; align-items: center; border: 2px solid #000000; border-radius: 10px; padding: 5px; width:60%; margin: 0 auto;">
            <asp:TextBox ID="SearchText" runat="server" placeholder="搜尋..." style="border: none; outline: none; width: 100%;"></asp:TextBox>
            <asp:ImageButton ID="SearchBtn" runat="server" ImageUrl="~/page/images/search.png" style="width: 20px; height: 20px;" OnClick="SearchBtn_Click" />
        </div>
        <br />
        <br />
        <!-- 評論按鈕面板 --> <!-- 尚未建置完成 -->
        <div style="display: flex; justify-content: center; margin-bottom: 20px; padding-bottom: 20px;">
            <asp:Panel ID="pn_comment_btn" runat="server">
                <h4 class="font-weight-bold">篩選</h4>
                <asp:Button ID="btn_my_comment" runat="server" Text="全部" CssClass="btn btn-outline-primary mt-2 px-3" Font-Size="Large" Height="50px" Width="120px" />
                &nbsp;&nbsp;
        <asp:Button ID="btn_new_comment" runat="server" Text="評分" CssClass="btn btn-outline-primary mt-2 px-3" Font-Size="Large" Height="50px" Width="120px" />
                &nbsp;&nbsp;
        <asp:Button ID="btn_higher_comment" runat="server" Text="人數" CssClass="btn btn-outline-primary mt-2 px-3" Font-Size="Large" Height="50px" Width="120px" />
                &nbsp;&nbsp;
                <asp:Button ID="btn_location" runat="server" Text="地區" CssClass="btn btn-outline-primary mt-2 px-3" Font-Size="Large" Height="50px" Width="120px" />
                &nbsp;&nbsp;
        <asp:Button ID="btn_low_comment" runat="server" Text="分類" CssClass="btn btn-outline-primary mt-2 px-3" Font-Size="Large" Height="50px" Width="120px" />
                &nbsp;&nbsp;
            </asp:Panel>

        </div>

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
                                    <div class="col-sm-6" style="padding: 10px 15px;" >
                                        <h4 class="font-weight-bold"><%# Eval("課程名稱") %></h4>
                                        <h4 class="font-weight-bold mb-4" style="color: #e31c25"><%# "$ " + Convert.ToDouble(Eval("課程費用")).ToString("F0") + " /堂"%></h4>
                                        <p><%# Eval("健身教練姓名") + " 教練" %></p>
                                        <p><%# Eval("課程內容介紹") %></p>
                                        <p><%# "人數：" + Eval("上課人數") + "人"%></p>
                                    </div>
                                </div>
                            </asp:LinkButton>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
    <!-- GYM Class End -->


</asp:Content>

