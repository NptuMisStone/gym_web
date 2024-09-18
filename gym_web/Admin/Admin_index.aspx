<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Admin_index.aspx.cs" Inherits="Admin_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Admin/coach_review.aspx">健身教練管理</asp:HyperLink>
            <br />
            <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Admin/AD.aspx">廣告管理</asp:HyperLink>
            <br />
            <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Admin/comment_review.aspx">評論管理</asp:HyperLink>
        </div>
    </form>
</body>
</html>
