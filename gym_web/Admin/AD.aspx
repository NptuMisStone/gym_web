<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AD.aspx.cs" Inherits="Admin_AD_review" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>廣告管理</title>
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
        .button-group {
            margin-bottom: 20px;
            display: flex;
            gap: 10px;
        }
        .button-group button, .button-group .asp-button {
            padding: 10px 20px;
            background-color: #007bff;
            color: white;
            border: none;
            border-radius: 4px;
            font-size: 14px;
            cursor: pointer;
            transition: background-color 0.3s;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);
        }
        .button-group button:hover, .button-group .asp-button:hover {
            background-color: #0056b3;
        }
        .ad-list {
            display: flex;
            flex-direction: column;
            gap: 10px;
        }
        .ad-item {
            display: flex;
            align-items: center;
            border: 1px solid #ddd;
            border-radius: 4px;
            padding: 10px;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
            background-color: #fff;
        }
        .ad-item img {
            width: 720px;
            height: 240px;
            object-fit: cover;
            margin-right: 10px;
        }
        .ad-item .ad-details {
            flex: 1;
        }
        .ad-item .ad-details h4 {
            margin: 0;
            font-size: 16px;
            font-weight: bold;
        }
        .ad-item .ad-details p {
            margin: 5px 0;
        }
        .ad-item .ad-details .date {
            font-size: 14px;
            color: #555;
        }
        .edit-panel {
            padding: 20px;
            background-color: rgba(255, 255, 255, 0.9);
            border-radius: 8px;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
        }
        .back-button {
            display: inline-block;
            margin-bottom: 20px;
            padding: 10px 20px;
            background-color: #007bff;
            color: #fff;
            text-decoration: none;
            border-radius: 4px;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);
            font-size: 14px;
        }
        .back-button:hover {
            background-color: #0056b3;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <a href="Admin_index.aspx" class="home-link"><span class="arrow">&larr;</span> 返回首頁</a>
            <h1>廣告管理</h1>
            <div class="button-group">
                <asp:Button ID="Button2" runat="server" Text="新增廣告" OnClick="Button2_Click" CausesValidation="false" CssClass="asp-button" />
                <asp:Button ID="Button3" runat="server" Text="有效廣告" OnClick="Button3_Click" CausesValidation="false" CssClass="asp-button" />
                <asp:Button ID="Button4" runat="server" Text="過期廣告" OnClick="Button4_Click" CausesValidation="false" CssClass="asp-button" />
                <asp:Button ID="Button5" runat="server" Text="未來廣告" OnClick="Button5_Click" CausesValidation="false" CssClass="asp-button" />
            </div>

            <asp:Panel ID="Panel1" runat="server" Visible="false">
                <h2>新增廣告</h2>
                <label>廣告名稱：</label>
                <asp:TextBox ID="tb_name" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_name" runat="server" ControlToValidate="tb_name" ErrorMessage="廣告名稱不得為空" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                <br />
                <label>廣告圖片：</label>
                <asp:FileUpload ID="FileUpload1" runat="server" />
                <asp:RequiredFieldValidator ID="rfv_fileupload" runat="server" ControlToValidate="FileUpload1" InitialValue="" ErrorMessage="請選擇廣告圖片" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                <br />
                <label>上架日期：</label>
                <asp:TextBox ID="tb_date_start" runat="server" TextMode="Date"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_date_start" runat="server" ControlToValidate="tb_date_start" ErrorMessage="上架日期不得為空" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                <br />
                <label>下架日期：</label>
                <asp:TextBox ID="tb_date_end" runat="server" TextMode="Date"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_date_end" runat="server" ControlToValidate="tb_date_end" ErrorMessage="下架日期不得為空" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                <br />
                <label>廣告連結：</label>
                <asp:TextBox ID="tb_url" runat="server" Width="654px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_url" runat="server" ControlToValidate="tb_url" ErrorMessage="廣告連結不得為空" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                <br />
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="新增" />
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" />
            </asp:Panel>

            <asp:Panel ID="Panel2" runat="server" Visible="false">
                <h2>有效廣告</h2>
                <asp:Repeater ID="rp_ads" runat="server" OnItemCommand="rp_ads_ItemCommand">
                    <ItemTemplate>
                        <div class="ad-list">
                            <div class="ad-item">
                                <asp:Image ID="Image1" runat="server" ImageUrl='<%# GetImageUrl(Eval("圖片"), 15) %>' CssClass="ad-image" /><br>
                                <div class="ad-details">
                                    <h4><%# Eval("名稱") %></h4>
                                    <p><%# Eval("連結") %></p>
                                    <p class="date">上架日期: <%# Eval("上架日", "{0:yyyy/MM/dd}") %></p>
                                    <p class="date">下架日期: <%# Eval("下架日", "{0:yyyy/MM/dd}") %></p>
                                    <asp:Button ID="btn_ad_edit" runat="server" class="btn btn-primary" Text="編輯" CausesValidation="false" CommandName="edit_ad" CommandArgument='<%# Eval("編號") %>' />&nbsp;
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </asp:Panel>

            <asp:Panel ID="Panel3" runat="server" Visible="false">
                <h2>過期廣告</h2>
                <asp:Repeater ID="rp_expired_ads" runat="server" OnItemCommand="rp_ads_ItemCommand">
                    <ItemTemplate>
                        <div class="ad-list">
                            <div class="ad-item">
                                <asp:Image ID="Image1" runat="server" ImageUrl='<%# GetImageUrl(Eval("圖片"), 15) %>' CssClass="ad-image" /><br>
                                <div class="ad-details">
                                    <h4><%# Eval("名稱") %></h4>
                                    <p><%# Eval("連結") %></p>
                                    <p class="date">上架日期: <%# Eval("上架日", "{0:yyyy/MM/dd}") %></p>
                                    <p class="date">下架日期: <%# Eval("下架日", "{0:yyyy/MM/dd}") %></p>
                                    <asp:Button ID="btn_ad_edit" runat="server" class="btn btn-primary" Text="編輯" CausesValidation="false" CommandName="edit_ad" CommandArgument='<%# Eval("編號") %>' />
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </asp:Panel>

            <asp:Panel ID="Panel4" runat="server" Visible="false">
                <h2>未來廣告</h2>
                <asp:Repeater ID="rp_future_ads" runat="server" OnItemCommand="rp_ads_ItemCommand">
                    <ItemTemplate>
                        <div class="ad-list">
                            <div class="ad-item">
                                <asp:Image ID="Image1" runat="server" ImageUrl='<%# GetImageUrl(Eval("圖片"), 15) %>' CssClass="ad-image" /><br>
                                <div class="ad-details">
                                    <h4><%# Eval("名稱") %></h4>
                                    <p><%# Eval("連結") %></p>
                                    <p class="date">上架日期: <%# Eval("上架日", "{0:yyyy/MM/dd}") %></p>
                                    <p class="date">下架日期: <%# Eval("下架日", "{0:yyyy/MM/dd}") %></p>
                                    <asp:Button ID="btn_ad_edit" runat="server" class="btn btn-primary" Text="編輯" CausesValidation="false" CommandName="edit_ad" CommandArgument='<%# Eval("編號") %>' />&nbsp;
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </asp:Panel>

            <asp:Panel ID="Panel_Edit" runat="server" CssClass="edit-panel" Visible="false">
                <h2>編輯廣告</h2>
                <label>廣告名稱：</label>
                <asp:TextBox ID="tb_edit_name" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_edit_name" runat="server" ControlToValidate="tb_edit_name" ErrorMessage="廣告名稱不得為空" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                <br />
                <label>廣告圖片：</label>
                <asp:FileUpload ID="FileUpload2" runat="server" />
                <br />
                <label>上架日期：</label>
                <asp:TextBox ID="tb_edit_date_start" runat="server" TextMode="Date"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_edit_date_start" runat="server" ControlToValidate="tb_edit_date_start" ErrorMessage="上架日期不得為空" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                <br />
                <label>下架日期：</label>
                <asp:TextBox ID="tb_edit_date_end" runat="server" TextMode="Date"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_edit_date_end" runat="server" ControlToValidate="tb_edit_date_end" ErrorMessage="下架日期不得為空" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                <br />
                <label>廣告連結：</label>
                <asp:TextBox ID="tb_edit_url" runat="server" Width="654px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_edit_url" runat="server" ControlToValidate="tb_edit_url" ErrorMessage="廣告連結不得為空" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                <br />
                <asp:Button ID="btn_update" runat="server" OnClick="btn_update_Click" Text="更新" CausesValidation="true" />
                <asp:Button ID="btn_cancel" runat="server" OnClick="btn_cancel_Click" Text="取消" CausesValidation="false" />
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ForeColor="Red" />
                <asp:HiddenField ID="hf_ad_id" runat="server" />
            </asp:Panel>
        </div>
    </form>
</body>
</html>
