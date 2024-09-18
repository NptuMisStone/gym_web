<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage.master" AutoEventWireup="true" CodeFile="User_index.aspx.cs" Inherits="User_User_index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div>
    <asp:HyperLink ID="HyperLink" runat="server" NavigateUrl="/User/User_info.aspx" ForeColor="Black" Font-Size="XX-Large">使用者帳號檢視</asp:HyperLink>
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="/User/User_appointment_record.aspx" ForeColor="Black" Font-Size="XX-Large">預約紀錄</asp:HyperLink>
</div>
</asp:Content>

