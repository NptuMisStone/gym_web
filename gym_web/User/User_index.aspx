﻿<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="User_index.aspx.cs" Inherits="User_User_index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Page Header Start -->
<div class="container-fluid page-header mb-5">
    <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 130px">
    </div>
</div>
<!-- Page Header End -->
        <div>
    <asp:HyperLink ID="HyperLink" runat="server" NavigateUrl="/User/User_info.aspx" ForeColor="Black" Font-Size="XX-Large">使用者帳號檢視</asp:HyperLink>
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="/User/User_appointment_record.aspx" ForeColor="Black" Font-Size="XX-Large">預約紀錄</asp:HyperLink>
    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="/User/User_Like.aspx" ForeColor="Black" Font-Size="XX-Large">我的收藏</asp:HyperLink>
</div>
</asp:Content>

