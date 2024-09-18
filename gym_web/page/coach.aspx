<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage.master" AutoEventWireup="true" CodeFile="coach.aspx.cs" Inherits="page_coach" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
<div class="container-xxl py-5">
    <div class="container">
        <div class="text-center  wow fadeInUp" data-wow-delay="0.1s">
            <h1 class="mb-5">健身教練</h1>
        </div>
    </div>
</div>
<div class="row  gx-4 ">
    <asp:Repeater ID="rp_coachdata" runat="server" OnItemCommand="rp_coachdata_ItemCommand">
        <ItemTemplate>
            <%# (Container.ItemIndex + 1) % 4 == 1 ? "<div class='row horizontal-row'>" : "" %>
            <div class="card repeater-de" style="display: inline-block; width: 275px; height: 260px; text-align: center; margin-bottom: 5px;">
                <asp:Image ID="Image1" runat="server" Height="120px" ImageUrl='<%# GetImageUrl(Eval("健身教練圖片"),15) %>' Width="120px" CssClass="circular-image" /><br>
                <asp:Label ID="Label1" runat="server" CssClass="mt-4" Text='<%# Eval("健身教練姓名") %>' Font-Size="Medium" Font-Bold="True"></asp:Label><br>
                <asp:Label ID="Label2" runat="server" CssClass="mb-0" Text='<%#  Eval("服務地點名稱") %>'></asp:Label><br />
                <!--<asp:Image ID="Image2" runat="server" ImageUrl="img/a star.png" Width="15px" Height="15px" /> -->
                <asp:Button ID="Button2" runat="server" class="btn btn-primary rounded-pill py-2 px-4  top-0 end-0 me-2" Text="查看健身教練資訊" CommandName="coach_detail" CommandArgument='<%# Eval("健身教練編號") %>' />&nbsp;
            </div>
            <br>
            <%# (Container.ItemIndex + 1) % 4 == 0 || Container.ItemIndex == rp_coachdata.Items.Count - 1 ? "</div>" : "" %>
        </ItemTemplate>
    </asp:Repeater>
</div>
</asp:Content>

