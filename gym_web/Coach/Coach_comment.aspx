<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="Coach_comment.aspx.cs" Inherits="Coach_Coach_comment"  MaintainScrollPositionOnPostback="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style>
        .user-avatar {
            width: 75px;
            height: 75px;
            border-radius: 50%;
            margin-right: 10px;
        }

        .rating {
            font-weight: bold;
            color: #f0a500;
        }


        .user-name {
            font-weight: bold;
            margin-top: 5px;
        }

        .reply-button, .submit-reply-button {
            margin-top: 10px;
            background-color: #007bff;
            color: #fff;
            border: none;
            padding: 5px 10px;
            cursor: pointer;
        }

        .submit-reply-button {
            background-color: #28a745;
        }

        .reply-textbox {
            margin-top: 10px;
            padding: 5px;
            width: 100%;
        }
        .review-card {
            border: 1px solid #ccc;
            padding: 10px;
            margin: 10px;
            background-color: #f9f9f9;
            border-radius: 10px;
            position: relative;
        }
         .edit-button {
                position: absolute;
                top: 5px;
                right: 10px;
                z-index: 1; /* 以确保编辑按钮位于其他内容之上 */
            }

    </style>
    <!-- Page Header Start -->
<div class="container-fluid page-header mb-5">
    <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 400px">
        <h4 class="display-4 mb-3 mt-0 mt-lg-5 text-white text-uppercase font-weight-bold">我的評論</h4>
    </div>
</div>
<!-- Page Header End -->

<!-- Team Start -->
<div class="container pt-5 team">
    <div class="d-flex flex-column text-center mb-5">
        <h4 class="text-primary font-weight-bold">NPTU GYM</h4>
        <h4 class="display-4 font-weight-bold">使用者評論</h4>
    </div>
    <div  class="container">
        <asp:Repeater ID="UserReviewsRepeater" runat="server">
            <ItemTemplate>
                <div class="review-card">
                    <div class="edit-button">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Coach/img/comment-edit.png" Width="30px" Height="30px" OnClick="ImageButton1_Click" />
                    </div>
                    <asp:Image ID="UserAvatarImage" runat="server" CssClass="user-avatar" ImageUrl='<%# GetAvatarUrl(Eval("使用者圖片")) %>' />
                    <p class="user-name"><%# Eval("使用者姓名") %></p>
                    <p style=" text-transform: lowercase; font-style: italic">課程名稱：<%# Eval("課程名稱") %></p>
                    <div class="rating">
                        評分: <%# GenerateStars((int)Eval("評分")) %>
                    </div>
                    <p class="review-content">評論內容: <%# Eval("評論內容") %></p>
                    <p class="review-date">評論日期: <%# DataBinder.Eval(Container.DataItem, "評論日期", "{0:yyyy/MM/dd}") %></p>
                    <p class="reply-text">回覆: <%# Eval("回覆") %></p>
                    <asp:TextBox ID="ReplyTextBox" runat="server" placeholder="請輸入回覆" CssClass="reply-textbox" Text='<%# Eval("回覆") %>'
                        Visible='<%# string.IsNullOrEmpty(Eval("回覆").ToString()) %>'></asp:TextBox>

                    <asp:Button ID="SubmitReplyButton" runat="server" Text="送出" OnClick="SubmitReplyButton_Click"
                        CommandArgument='<%# Eval("評論編號") %>' CssClass="submit-reply-button"
                        Visible='<%# string.IsNullOrEmpty(Eval("回覆").ToString()) %>' />
                    <asp:Button ID="CancelReplyButton" runat="server" Text="取消編輯" OnClick="CancelReplyButton_Click"
                    CssClass="submit-reply-button"
                    Visible="false" />
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    

</div>
<!-- Team End -->
    
    
</asp:Content>

