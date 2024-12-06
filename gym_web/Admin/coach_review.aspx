<%@ Page Language="C#" AutoEventWireup="true" CodeFile="coach_review.aspx.cs" Inherits="system_administrator_Registration_Approval" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>健身教練核准</title>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10"></script>

    <style>
        body {
            font-family: 'Roboto', sans-serif;
            background-image: url('/page/img/bg.jpg');
            background-size: cover;
            background-repeat: no-repeat;
            background-attachment: fixed;
            margin: 0;
            padding: 0;
            display: flex;
            align-items: center;
            justify-content: center;
            height: 100vh;
        }

        .container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
            background-color: rgba(255, 255, 255, 0.9);
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
            border-radius: 10px;
        }

        h1, h3 {
            color: #333;
            font-weight: 700;
        }

        .details-view {
            width: 800px;
            max-width: 100%;
            height: auto;
            border: 1px solid #ddd;
            background-color: #fff;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
            margin: 20px auto;
            text-align: left;
        }

        .details-view h2 {
            font-size: 24px;
            color: #333;
            margin-bottom: 20px;
        }

        .details-label {
            font-weight: bold;
            color: #555;
        }

        .details-value {
            margin-bottom: 15px;
        }

        .button {
            background-color: #ff6b6b;
            color: #fff;
            padding: 12px 25px;
            border: none;
            border-radius: 30px;
            cursor: pointer;
            font-size: 16px;
            transition: background-color 0.3s ease;
        }

        .button:hover {
            background-color: #ee5253;
        }

        .approval-box {
            border: 1px solid #ddd;
            background-color: #ffffff;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
            text-align: center;
            max-width: 500px;
            margin: 40px auto;
        }

        .approval-box h2 {
            font-size: 28px;
            color: #333;
            margin-bottom: 30px;
            font-weight: 700;
        }

        .arrow-down {
            width: 0;
            height: 0;
            border-left: 20px solid transparent;
            border-right: 20px solid transparent;
            border-top: 20px solid #333;
            margin: 20px auto;
        }

        .home-link {
            text-decoration: none;
            color: #007bff;
            font-size: 16px;
            font-weight: 500;
            display: inline-block;
            margin-bottom: 20px;
            transition: color 0.3s ease;
        }

        .home-link:hover {
            color: #0056b3;
        }

        .arrow {
            margin-right: 5px;
        }
    </style>
</head>
<body>
    <div class="container">
        <form id="form1" runat="server">

            <a href="Admin_index.aspx" class="home-link"><span class="arrow">&larr;</span> 返回首頁</a>

            <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                ConnectionString="<%$ ConnectionStrings:ManagerConnectionString %>"
                SelectCommand="SELECT * FROM [健身教練審核] WHERE [審核狀態] = @ReviewStatus">
                <SelectParameters>
                    <asp:Parameter DefaultValue="0" Name="ReviewStatus" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>

            <h1>健身教練註冊核准</h1>
            <asp:DetailsView ID="dvDesignerApproval" runat="server" AutoGenerateRows="False" CssClass="details-view" DataKeyNames="編號" DataSourceID="SqlDataSource1" AllowPaging="True">
                <Fields>
                    <asp:BoundField DataField="編號" HeaderText="註冊編號" ItemStyle-CssClass="details-label" InsertVisible="False" ReadOnly="True" SortExpression="RegistrationID">
                        <ItemStyle CssClass="details-label" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="健身教練編號" SortExpression="Coach_id">
                        <ItemTemplate>
                            <asp:Label ID="Coach_id_Label" runat="server" Text='<%# Eval("健身教練編號") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="註冊類型" SortExpression="Coach_type">
                        <ItemTemplate>
                            <asp:Label ID="Coach_type_Label" runat="server" Text='<%# Eval("註冊類型") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="服務地點名稱" SortExpression="Name">
                        <ItemTemplate>
                            <asp:Label ID="NameLabel" runat="server" Text='<%# Eval("服務地點名稱") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="服務地點電話" SortExpression="Phone">
                        <ItemTemplate>
                            <asp:Label ID="PhoneLabel" runat="server" Text='<%# Eval("服務地點電話") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="服務地點Email" SortExpression="Phone">
                        <ItemTemplate>
                            <asp:Label ID="EmailLabel" runat="server" Text='<%# Eval("服務地點郵件") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="服務地點地址" SortExpression="Address">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("服務地點地址") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("服務地點地址") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="addressLabel" runat="server" Text='<%# Bind("服務地點地址") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="details-label" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="審核狀態" HeaderText="審核狀態" SortExpression="ReviewStatus" Visible="False" />
                    <asp:TemplateField HeaderText="資料" SortExpression="VerificationData">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDownload" runat="server" Text="審核資料" OnClick="DownloadPDF" CommandArgument='<%# Eval("編號") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Fields>
            </asp:DetailsView>

            <div class="arrow-down"></div>
            <div class="approval-box">
                <h2>審核結果</h2>
                <asp:Button ID="btnApprove" runat="server" Text="同意" CssClass="button" OnClick="btnApprove_Click" />
                <asp:Button ID="btncancel" runat="server" Text="否決" CssClass="button" OnClick="btncancel_Click" />
            </div>

        </form>
    </div>
</body>
</html>
