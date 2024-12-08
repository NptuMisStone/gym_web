<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="coach.aspx.cs" Inherits="page_coach" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Page Header Start -->
    <div class="container-fluid page-header mb-5">
        <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 400px">
            <h4 class="display-4 mb-3 mt-0 mt-lg-5 text-white text-uppercase font-weight-bold">健身教練</h4>
        </div>
    </div>
    <!-- Page Header End -->

    <!-- Team Start -->
    <div class="container pt-5 team">
        <div class="d-flex flex-column text-center mb-5">
            <h4 class="text-primary font-weight-bold">NPTU FIT</h4>
            <h4 class="display-4 font-weight-bold">我們的合作夥伴</h4>
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
    </div>
    <!-- Team End -->
</asp:Content>

