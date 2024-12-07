<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="User_Like.aspx.cs" Inherits="User_User_Like" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Page Header Start -->
    <div class="container-fluid page-header mb-5">
        <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 400px">
            <h4 class="display-4 mb-3 mt-0 mt-lg-5 text-white text-uppercase font-weight-bold">我的收藏</h4>
        </div>
    </div>
    <!-- Page Header End -->

    <!-- Team Start -->
    <div class="container pt-5 team">
        <div class="d-flex flex-column text-center mb-5">
            <h4 class="text-primary font-weight-bold">NPTU GYM</h4>
            <h4 class="display-4 font-weight-bold">我的收藏教練</h4>
        </div>

        <asp:ListView ID="lv_coachdata" runat="server" OnItemCommand="lv_coachdata_ItemCommand">
            <LayoutTemplate>
                <div class="row gx-4">
                    <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                <div class="col-lg-3 col-md-6 mb-5">
                    <div class="card border-0 bg-secondary text-center text-white">
                        <!-- 顯示圖片 -->
                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# GetImageUrl(Eval("健身教練圖片"),60) %>' CssClass="card-img-top" Style="object-fit: cover; height: 250px; width: 100%;" />
                        <!-- 愛心圖示 -->
                        <div class="card-social d-flex align-items-center justify-content-center">
                            <asp:ImageButton
                                ID="LikeBtn"
                                runat="server"
                                CommandName="Like"
                                CommandArgument='<%# Eval("健身教練編號") %>'
                                OnClick="LikeBtn_Click"
                                CssClass="like-button btn btn-outline-light rounded-circle"
                                ImageUrl='<%# GetLikeImageUrl(Eval("健身教練編號")) %>'
                                Style="width: 60px; height: 60px; object-fit: contain;" />
                        </div>
                        <!-- 點擊區域，透過 LinkButton 觸發事件 -->
                        <asp:LinkButton ID="lb_coach" runat="server" CommandName="coach_detail" CommandArgument='<%# Eval("健身教練編號") %>' CssClass="card-body bg-secondary" Style="display: block; text-align: center; text-decoration: none; cursor: pointer;">
                        <h4 class="card-title text-primary font-weight-bold"><%# Eval("健身教練姓名") %></h4>
                        <p class="card-text" style="color:white"><%# Eval("註冊類型") %></p>
                        </asp:LinkButton>
                    </div>
                </div>
            </ItemTemplate>
        </asp:ListView>
        <div class="d-flex flex-column text-center mb-5">
            <h4 class="text-primary font-weight-bold">NPTU GYM</h4>
            <h4 class="display-4 font-weight-bold">我的收藏課程</h4>
        </div>
        <div class="container feature pt-5 w-100">
            <div class="row">
                <asp:ListView ID="lv_classdata" runat="server" OnItemCommand="lv_classdata_ItemCommand" >
                    <layouttemplate>
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                    </layouttemplate>
                    <itemtemplate>
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
                                    Style="position:relative; display: block; text-align: left; text-decoration: none; cursor: pointer;">
                                    <div class="row align-items-center" style="padding: 20px;">
                                        <div class="col-sm-6" style="padding: 10px 15px;">
                                            <asp:Image ID="Image1" runat="server" ImageUrl='<%# GetImageUrl2(Eval("課程圖片"),60) %>' CssClass="img-fluid mb-3 mb-sm-0" Style="object-fit: cover; height: 130px; width: 100%;" />
                                            <%# Convert.ToInt32(Eval("上課人數")) == 1 ? 
                                                "<i style='font-size:20px; font-weight: bold;'>一對一</i>" : 
                                                "<i style='font-size:20px; font-weight: bold;'>團體</i>" %>
                                        </div>
                                        <div class="col-sm-6" style="padding: 10px 15px;">
                                            <h4 class="font-weight-bold"><%# Eval("課程名稱") %></h4>
                                    
                                            <h4 class="font-weight-bold mb-4" style="color: #e31c25"><%# "$ " + Convert.ToDouble(Eval("課程費用")).ToString("F0") + " /堂"%></h4>
                                            <p><%# Eval("健身教練姓名") + " 教練" %></p>
                                            <p><%# Eval("課程內容介紹") %></p>
                                            <p><%# "人數：" + Eval("上課人數") + "人"%></p>
                                        </div>
                                    </div>
                                </asp:LinkButton>
                                <!-- 愛心圖示 -->
                                <div style="position: absolute; top: 5%; left: 5%;">
                                    <asp:ImageButton 
                                        ID="ClassLikeBtn"
                                        runat="server"
                                        CommandArgument='<%# Eval("課程編號") %>'
                                        OnClick="ClassLikeBtn_Click"
                                        CssClass="like-button btn btn-outline-light rounded-circle"
                                        ImageUrl='<%# GetClassLikeImageUrl(Eval("課程編號")) %>'
                                        Style="width: 60px; height: 60px; object-fit: contain;" />
                                </div>
                            </div>
                        </div>
                    </itemtemplate>
                </asp:ListView>
            </div>
        </div>
    <!-- Team End -->
</asp:Content>
