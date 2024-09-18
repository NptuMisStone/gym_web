<%@ Page Language="C#" AutoEventWireup="true" CodeFile="coach_review.aspx.cs" Inherits="system_administrator_Registration_Approval" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>設計師註冊核准</title>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10"></script>

    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f2f2f2;
            margin: 0;
            padding: 0;
        }

        .container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
            background-color: #fff;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }

        h1, h3 {
            color: #333;
        }

        .details-view {
            width: 800px; /* 設定寬度，視情況調整 */
            max-width: 100%; /* 設定最大寬度，避免超出螢幕 */
            height: 600px; /* 設定高度，視情況調整 */
            max-height: 100%; /* 設定最大高度，避免超出螢幕 */
            border: 1px solid #e0e0e0;
            background-color: #f9f9f9;
            padding: 20px;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            margin: 0 auto; /* 水平置中 */
            text-align: center; /* 文字置中對齊 */
        }



            /* 標題的樣式 */
            .details-view h2 {
                font-size: 24px;
                color: #333;
                margin-bottom: 20px;
            }

        /* 標籤的樣式 */
        .details-label {
            font-weight: bold;
            color: #555;
        }

        /* 值的樣式 */
        .details-value {
            margin-bottom: 15px;
        }

        /* 按鈕的樣式 */
        .button {
            background-color: #007bff;
            color: #fff;
            padding: 10px 20px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 16px;
        }

        /* 高亮顏色的樣式 */
        .highlight {
            background-color: #ffd700;
            color: #333;
        }

        .approval-box {
            border: 1px solid #e0e0e0;
            background-color: f5f5dc;
            padding: 20px;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            text-align: center; /* 文字置中對齊 */
            max-width: 400px; /* 設定最大寬度，視情況調整 */
            margin: 0 auto; /* 水平置中 */
        }

            .approval-box h2 {
                font-size: 24px;
                color: #333;
                margin-bottom: 20px;
            }

        .button {
            background-color: #007bff;
            color: #fff;
            padding: 10px 20px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 16px;
            margin: 5px;
        }

            .button:hover {
                background-color: #0056b3;
            }

        .arrow-down {
            width: 0;
            height: 0;
            border-left: 20px solid transparent;
            border-right: 20px solid transparent;
            border-top: 20px solid #333; /* 深灰色背景 */
            margin: 0 auto;
        }

        .home-link {
            text-decoration: none; /* 移除連結的下劃線 */
            color: #333; /* 設定連結的文字顏色 */
            font-size: 16px; /* 設定文字大小 */
        }

        .arrow {
            margin-right: 5px; /* 設定箭頭符號和文字之間的距離 */
        }
    </style>
</head>
<body>
    <div class="container">
        <form id="form1" runat="server">

            <a href="Admin_index.aspx" class="home-link"><span class="arrow">&larr;</span> 返回首頁</a>

            <h1>設計師註冊核准</h1>
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



            <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                ConnectionString="<%$ ConnectionStrings:ManagerConnectionString %>"
                SelectCommand="SELECT * FROM [健身教練審核] WHERE [審核狀態] = @ReviewStatus">
                <SelectParameters>
                    <asp:Parameter DefaultValue="0" Name="ReviewStatus" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>

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

