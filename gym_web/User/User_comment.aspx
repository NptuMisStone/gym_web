<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="User_comment.aspx.cs" Inherits="User_User_comment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Page Header Start -->
<div class="container-fluid page-header mb-5">
    <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 130px">
    </div>
</div>
<!-- Page Header End -->
          <br />  
    <center>
        <table style="border: 1px solid #ccc; border-radius: 5px; background-color: #fff; box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1); text-align: center; width: 350px; height:450px;">
            <tr>
                <td style="height: 47px">
                    <asp:Label ID="Label2" runat="server" Text="為健身教練留下評論"  Font-Bold="True" Font-Names="微軟正黑體" Font-Size="25px" ForeColor="Black"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; padding-right: 3px; ">
                    &nbsp;         
                    <asp:Label ID="lb_de_name" runat="server" Text="健身教練：" Font-Size="18px"></asp:Label>
                    <br />
                    <asp:Label ID="lb_service" runat="server" Text="服務項目：" Font-Size="18px"></asp:Label>
                    <br />
                    <asp:Label ID="lb_date" runat="server" Text="預約日期：" Font-Size="18px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 26px">
                    <asp:Panel ID="Panel1" runat="server">
                    <asp:ImageButton ID="img_star1" runat="server" Height="20px" Width="20px" ImageUrl="img/star.png" OnClick="img_star_Click" CommandArgument="1" />&nbsp;&nbsp;
                    <asp:ImageButton ID="img_star2" runat="server" Height="20px" Width="20px" ImageUrl="img/star.png" OnClick="img_star_Click" CommandArgument="2" />&nbsp;&nbsp;
                    <asp:ImageButton ID="img_star3" runat="server" Height="20px" Width="20px" ImageUrl="img/star.png" OnClick="img_star_Click" CommandArgument="3" />&nbsp;&nbsp;
                    <asp:ImageButton ID="img_star4" runat="server" Height="20px" Width="20px" ImageUrl="img/star.png" OnClick="img_star_Click" CommandArgument="4" />&nbsp;&nbsp;
                    <asp:ImageButton ID="img_star5" runat="server" Height="20px" Width="20px" ImageUrl="img/star.png" OnClick="img_star_Click" CommandArgument="5" />&nbsp;&nbsp;
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="tb_comment" runat="server" Height="150px" Placeholder="分享您對健身教練的寶貴意見，讓其他使用者及健身教練了解您的預約經驗" TextMode="MultiLine" Width="300px" BackColor="#F2F2F2"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="margin-bottom: 5px; padding-bottom: 7px;">
                    <asp:Button ID="Button1" runat="server" Text="取消" CssClass="button222" BackColor="#E187AB" BorderStyle="None" ForeColor="White" Height="30px" Width="80px"  OnClick="Button1_Click"/>&nbsp;&nbsp;
                    <asp:Button ID="Button2" runat="server" Text="提交" CssClass="button222" BackColor="#E187AB" BorderStyle="None" ForeColor="White" OnClick="btn_summit_Click" Height="30px" Width="80px"  />
             </td>
            </tr>
        </table>
    </center>
</asp:Content>

