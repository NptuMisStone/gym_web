<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage.master" AutoEventWireup="true" CodeFile="Coach_index.aspx.cs" Inherits="Coach_Coach_home1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
    <asp:HyperLink ID="HyperLink" runat="server" NavigateUrl="/Coach/Coach_info.aspx" ForeColor="Black" Font-Size="XX-Large">教練帳號檢視</asp:HyperLink>
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="/Coach/Coach_schedule.aspx" ForeColor="Black" Font-Size="XX-Large">教練班表</asp:HyperLink>
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="/Coach/Coach_class.aspx" ForeColor="Black" Font-Size="XX-Large">新增課程</asp:HyperLink>
</div>
</asp:Content>

