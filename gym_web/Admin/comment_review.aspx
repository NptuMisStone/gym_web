<%@ Page Language="C#" AutoEventWireup="true" CodeFile="comment_review.aspx.cs" Inherits="Admin_comment_review" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>評論檢舉審核</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.6/dist/sweetalert2.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10.16.6/dist/sweetalert2.all.min.js"></script>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            background: url('/page/img/bg.jpg') no-repeat center center fixed;
            background-size: cover;
            color: #333;
        }
        .container {
            max-width: 1200px;
            margin: 20px auto;
            padding: 20px;
            background-color: rgba(255, 255, 255, 0.9);
            border-radius: 8px;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
        }
        .header {
            margin-bottom: 20px;
            font-size: 24px;
            text-align: center;
            font-weight: bold;
        }
        .gridview {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }
        .gridview th, .gridview td {
            border: 1px solid #ddd;
            padding: 8px;
            text-align: center;
        }
        .gridview th {
            background-color: #f2f2f2;
            color: #333;
        }
        .gridview tr:nth-child(even) {
            background-color: #f9f9f9;
        }
        .gridview tr:hover {
            background-color: #f1f1f1;
        }
        .button {
            padding: 8px 16px;
            background-color: #007bff;
            color: #fff;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            transition: background-color 0.3s;
        }
        .button:hover {
            background-color: #0056b3;
        }
        .no-data {
            text-align: center;
            font-size: 16px;
            color: #888;
            margin-top: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <a href="Admin_index.aspx" class="home-link"><span class="arrow">&larr;</span> 返回首頁</a>
            <div class="header">評論檢舉審核</div>
            <asp:Label ID="Label1" runat="server" Text="暫無檢舉" CssClass="no-data" Visible="false"></asp:Label>
            <asp:GridView ID="gv_report" runat="server" CssClass="gridview" AutoGenerateColumns="False" OnRowCommand="gv_report_RowCommand" OnRowCancelingEdit="gv_report_RowCancelingEdit" OnRowDeleting="gv_report_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="評論編號" HeaderText="評論編號" />
                    <asp:TemplateField HeaderText="使用者圖片">
                        <ItemTemplate>
                            <asp:Image ID="imgUser" runat="server" ImageUrl='<%# GetImageUrl(Eval("使用者圖片"), 15) %>' Width="100px" Style="border-radius: 50%;" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="使用者姓名" HeaderText="使用者姓名" />
                    <asp:BoundField DataField="評分" HeaderText="評分" />
                    <asp:BoundField DataField="評論日期" HeaderText="評論日期" DataFormatString="{0:yyyy/MM/dd}" />
                    <asp:BoundField DataField="評論內容" HeaderText="評論內容" />
                    <asp:BoundField DataField="檢舉原因" HeaderText="檢舉原因" />

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" Text="刪除評論" CssClass="button" OnClick="btnDelete_Click"/>
                            <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="撤銷檢舉" CssClass="button" OnClick="btnCancel_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
