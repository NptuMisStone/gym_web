<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="User_Like.aspx.cs" Inherits="User_User_Like" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <!-- Team Start -->
    <div class="container-xxl py-5" style="width: 850px;">
        <div class="container" style="width: 850px; margin-right: 5px; margin-left: 5px; text-align: center;">
            <div class="text-center wow fadeInUp" data-wow-delay="0.1s">
                <%-- <h6 class="section-title bg-white text-center text-primary px-3">作品集</h6>--%>
                <h1 class="mb-5">我的收藏</h1>
                <asp:Label ID="lb_count" runat="server" Text="Label"></asp:Label>
            </div>
            <div style="text-align: center">
                <%--                <center>--%>
                <div style="text-align: left">
                    <asp:Repeater ID="Like_CoachRP" runat="server" OnItemCommand="Like_CoachRP_ItemCommand">
                        <ItemTemplate>
                            <table style="display: inline-block; margin-right: 10px;">
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="work_img" runat="server" ImageUrl='<%# GetImageUrl(Eval("健身教練圖片"),60) %>' Width="140px" Height="180px" CssClass="circular-commend" style="object-fit:cover;"/><br>
                                        <asp:Label ID="name_coach" runat="server" Text=<%# Eval("健身教練姓名") %>></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="LikeBtn" runat="server" ImageUrl="~/page/img/like.png" Width="25px" Height="25px" style="object-fit:cover;" CommandName="Edit_LikeBtn" CommandArgument='<%# Eval("健身教練編號") %>' />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <%--                </center>--%>
            </div>
        </div>
    </div>
    <!-- Team End -->
</asp:Content>

