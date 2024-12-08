<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="Coach_info.aspx.cs" Inherits="Coach_Coach_info1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function previewImage() {
            var fileInput = document.getElementById('<%= FileUpload1.ClientID %>');
            var image = document.getElementById('<%= coach_img.ClientID %>');

            if (fileInput.files && fileInput.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    image.src = e.target.result;
                }
                reader.readAsDataURL(fileInput.files[0]);
            }
        }
    </script>
    <style>
        .step-title {
            font-size: 1.5rem;
            font-weight: bold;
            color: #e31c25;
            border-bottom: 2px solid #e31c25;
            padding-bottom: 10px;
            margin-bottom: 15px;
            text-align: left;
        }

        .form-label {
            font-size: 1.5rem;
            font-weight: bold;
            display: block;
            text-align: left;
        }

        .form-control-custom {
            width: 300px;
            height: 40px;
            font-size: 1.5rem;
            display: inline-block;
        }

        .circular-image {
            border-radius: 50%; /* 確保圖片為圓形 */
            border: 4px solid #e31c25;
            width: 150px; /* 固定寬度 */
            height: 150px; /* 固定高度 */
            object-fit: cover; /* 保持圖片比例並裁剪溢出的部分 */
            display: block; /* 使圖片在父容器內顯示為塊元素，避免外邊距影響形狀 */
            margin: 0 auto; /* 使圖片水平居中 */
        }

        .btn-back-home {
            background-color: #007bff; /* 背景顏色 */
            color: white; /* 文字顏色設為白色 */
            border-radius: 50px; /* 圓角邊框 */
            padding: 10px 20px; /* 調整內邊距 */
            font-size: 1rem; /* 調整字體大小 */
            font-weight: bold; /* 使字體加粗 */
            border: none; /* 去掉預設邊框 */
            display: inline-block;
            text-align: center;
            transition: background-color 0.3s ease; /* 增加過渡效果 */
        }

            .btn-back-home:hover,
            .btn-back-home:focus {
                background-color: #28a745; /* 懸停或選取時的顏色 */
                color: white; /* 文字顏色設為白色 */
                text-decoration: none; /* 去掉下劃線 */
                outline: none; /* 去掉選取時的外框 */
            }
    </style>


    <div class="container-fluid page-header mb-1">
        <div class="d-flex flex-column align-items-center justify-content-center" style="min-height: 400px">
            <h4 class="display-4 mb-3 mt-0 text-white text-uppercase font-weight-bold">維護個人資料</h4>
        </div>
    </div>
    <div class="container gym-feature py-2">
        <div class="mb-3">
            <a class="btn-back-home px-4 py-2" href="<%= ResolveUrl("~/Coach/Coach_index.aspx") %>">←返回首頁</a>
        </div>
        <div class="row">
            <!-- 左邊欄位：個人資料 -->
            <div class="col-lg-9 col-md-6">
                <h5 class="mb-4 step-title">個人資料</h5>
                <div class="row">
                    <div class="col-5 d-flex justify-content-center align-items-center">
                        <div class="mr-3 text-center">
                            <asp:Image ID="coach_img" runat="server" CssClass="circular-image" Style="width: 300px; height: 300px;" draggable="false" />
                            <asp:FileUpload ID="FileUpload1" runat="server" Visible="false" onchange="previewImage();" />
                        </div>
                    </div>
                    <div class="col-7 d-flex justify-content-center align-items-center">
                        <div class="form-group">
                            <div class="mb-3 d-flex align-items-center">
                                <span class="form-label">帳號：</span>
                                <asp:Label ID="lb_account" runat="server" Text="Label" CssClass="d-block" Font-Size="X-Large" Style="white-space: nowrap;" />
                            </div>
                            <div class="d-flex align-items-center">
                                <span class="form-label">姓名：</span>
                                <asp:Label ID="lb_name" runat="server" Text="Label" CssClass="d-block" Font-Size="X-Large" Style="white-space: nowrap;" />
                                <asp:TextBox ID="tb_name" runat="server" CssClass="form-control" Visible="False" Style="width: 80%; font-size: 1.5rem; display: inline-block;" />
                            </div>
                            <div class="mb-3 text-right">
                                <asp:RequiredFieldValidator ID="rfv_name" runat="server" ControlToValidate="tb_name" ErrorMessage="姓名不得為空" ForeColor="Red" Display="Dynamic" />
                            </div>
                            <div class="d-flex align-items-center">
                                <span class="form-label">性別：</span>
                                <asp:Label ID="lb_gender" runat="server" Text="Label" Font-Size="X-Large" Style="display: inline-block;"></asp:Label>
                                <asp:DropDownList ID="tb_gender" runat="server"  CssClass="form-control" Visible="False"  Style="width: 80%; font-size: 1.5rem; display: inline-block;">
                                    <asp:ListItem Text="男生" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="女生" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="不願透露" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="mb-3 text-right">
                            </div>
                            <div class="d-flex align-items-center">
                                <span class="form-label">電話：</span>
                                <asp:Label ID="lb_phone" runat="server" Text="Label" CssClass="d-block" Font-Size="X-Large" Style="white-space: nowrap;" />
                                <asp:TextBox ID="tb_phone" runat="server" CssClass="form-control" Visible="False" Style="width: 80%; font-size: 1.5rem; display: inline-block;" />
                            </div>
                            <div class="mb-3 text-right">
                                <asp:RequiredFieldValidator ID="rfv_phone" runat="server" ControlToValidate="tb_phone" ErrorMessage="電話不得為空" ForeColor="Red" Display="Dynamic" />
                            </div>
                            <div class="d-flex align-items-center">
                                <span class="form-label">信箱：</span>
                                <asp:Label ID="lb_email" runat="server" Text="Label" CssClass="d-block" Font-Size="X-Large" Style="white-space: nowrap;" />
                                <asp:TextBox ID="tb_email" runat="server" CssClass="form-control" Visible="False" Style="width: 80%; font-size: 1.5rem; display: inline-block;" />
                            </div>
                            <div class="mb-3 text-right">
                                <asp:RequiredFieldValidator ID="rfv_email" runat="server" ControlToValidate="tb_email" ErrorMessage="信箱不得為空" ForeColor="Red" Display="Dynamic" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- 右邊欄位：更改密碼 -->
            <div class="col-lg-3 col-md-6">
                <h5 class="mb-4 step-title">更改密碼</h5>
                <div class="container p-0 mb-5">
                    <div class="form-group contact-forms mb-4">
                        <asp:TextBox ID="Txtpassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="請輸入原密碼" />
                    </div>
                    <div class="form-group contact-forms mb-4">
                        <asp:TextBox ID="Txtnewpassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="請輸入新密碼" />
                    </div>
                    <div class="form-group contact-forms mb-4">
                        <asp:TextBox ID="Txtnewpassword2" runat="server" CssClass="form-control" TextMode="Password" placeholder="請再次輸入新密碼" />
                        <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="新密碼不一致" ControlToCompare="Txtnewpassword" ControlToValidate="Txtnewpassword2" ForeColor="Red" Display="Dynamic" />
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="密碼長度至少6個字元且不可與帳號相同" ControlToValidate="Txtnewpassword" ForeColor="Red" Display="Dynamic" OnServerValidate="CustomValidator1_ServerValidate" />
                    </div>
                    <asp:Button ID="PWDBtnSave" runat="server" Text="更新" CssClass="btn btn-outline-primary px-4 py-2" Style="width: 100%;" OnClick="PWDBtnSave_Click" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-9 col-md-6">
                <!-- 介紹部分 -->
                <h5 class="mt-4 step-title">個人介紹</h5>
                <div class="p-3 bg-light rounded">
                    <asp:Label ID="lb_about" runat="server" Text="Label" CssClass="d-block" Font-Size="X-Large" />
                    <asp:TextBox ID="tb_about" runat="server" CssClass="form-control" Visible="False" TextMode="MultiLine" Style="resize: none; width: 100%; height: 100px; font-size: 1.5rem;" />
                </div>

                <asp:Panel ID="Panel_edit" runat="server" CssClass="mt-3 text-center">
                    <asp:Button ID="BtnEdit" runat="server" Text="修改" CssClass="btn btn-outline-primary px-4 py-2" OnClick="BtnEdit_Click" />
                </asp:Panel>
                <asp:Panel ID="Panel_save" runat="server" Visible="false" CssClass="mt-3 text-center">
                    <asp:Button ID="BtnSave" runat="server" Text="儲存" CssClass="btn btn-outline-primary px-4 py-2 mr-2" OnClick="BtnSave_Click" />
                    <asp:Button ID="BtnCancel" runat="server" Text="取消" CssClass="btn btn-outline-primary px-4 py-2" CausesValidation="false" OnClick="BtnCancel_Click" />
                </asp:Panel>
            </div>
            <div class="col-lg-3 col-md-6">
                <!-- 刪除帳號 -->
                <h5 class="mt-4 step-title">刪除帳號</h5>
                <asp:Button ID="Btn_delete" runat="server" Text="刪除帳號" CssClass="btn btn-outline-primary px-4 py-2" Style="width: 100%;" OnClick="Btn_delete_Click" />
            </div>
        </div>
    </div>
</asp:Content>
