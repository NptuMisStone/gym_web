<%@ Page Language="C#" AutoEventWireup="true" CodeFile="comment_review.aspx.cs" Inherits="Admin_comment_review" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.6/dist/sweetalert2.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.6/dist/sweetalert2.all.min.js"></script>
   
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="暫無檢舉" Visible="false"></asp:Label>
            <asp:Gridview ID="gv_report" runat="server" AutoGenerateColumns="False" OnRowCommand="gv_report_RowCommand"  OnRowCancelingEdit="gv_report_RowCancelingEdit" OnRowDeleting="gv_report_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="評論編號" HeaderText="評論編號"  />
                    <asp:TemplateField HeaderText="使用者圖片">
                        <ItemTemplate>
                            <asp:Image ID="imgUser" runat="server" ImageUrl='<%# GetImageUrl(Eval("使用者圖片"), 15) %>' Width="100px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="使用者姓名" HeaderText="使用者姓名" />
                    <asp:BoundField DataField="評分" HeaderText="評分" />
                    <asp:BoundField DataField="評論日期" HeaderText="評論日期"/>
                    <asp:BoundField DataField="評論內容" HeaderText="評論內容" />

                    <asp:ButtonField ButtonType="Button" CommandName="Delete" Text="刪除評論" />
                    <asp:ButtonField ButtonType="Button" CommandName="Cancel" Text="撤銷檢舉" />
                </Columns>
            </asp:Gridview>        
        </div>
    </form>
</body>
</html>
