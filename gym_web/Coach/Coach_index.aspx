<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="Coach_index.aspx.cs" Inherits="Coach_Coach_home1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Page Header Start -->
    <div class="container-fluid page-header mb-5">
        <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 400px">
            <h4 class="display-4 mb-3 mt-0 mt-lg-5 text-white text-uppercase font-weight-bold">健身教練</h4>
        </div>
    </div>
    <!-- Page Header End -->
    <div>
    <asp:HyperLink ID="HyperLink" runat="server" NavigateUrl="/Coach/Coach_info.aspx" ForeColor="Black" Font-Size="XX-Large">教練帳號檢視</asp:HyperLink>
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="/Coach/Coach_schedule.aspx" ForeColor="Black" Font-Size="XX-Large">教練班表</asp:HyperLink>
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="/Coach/Coach_class.aspx" ForeColor="Black" Font-Size="XX-Large">課程維護</asp:HyperLink>
        <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="/Coach/Coach_appointment.aspx" ForeColor="Black" Font-Size="XX-Large">預約紀錄</asp:HyperLink>
</div>
</asp:Content>

