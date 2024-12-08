<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="User_info.aspx.cs" Inherits="User_User_info" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function previewImage() {
            var fileInput = document.getElementById('<%= FileUpload1.ClientID %>');
            var image = document.getElementById('<%= user_img.ClientID %>');

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
            border-radius: 50%;
            border: 4px solid #e31c25;
            width: 150px;
            height: 150px;
            object-fit: cover;
            display: block;
            margin: 0 auto;
        }

        .btn-back-home {
            background-color: #007bff;
            color: white;
            border-radius: 50px;
            padding: 10px 20px;
            font-size: 1rem;
            font-weight: bold;
            border: none;
            display: inline-block;
            text-align: center;
            transition: background-color 0.3s ease;
        }

            .btn-back-home:hover,
            .btn-back-home:focus {
                background-color: #28a745;
                color: white;
                text-decoration: none;
                outline: none;
            }
    </style>

    <div class="container-fluid page-header mb-1">
        <div class="d-flex flex-column align-items-center justify-content-center" style="min-height: 400px">
            <h4 class="display-4 mb-3 mt-0 text-white text-uppercase font-weight-bold">維護個人資料</h4>
        </div>
    </div>
    <div class="container gym-feature py-2">
        <div class="mb-3">
            <a class="btn-back-home px-4 py-2" href="<%= ResolveUrl("~/User/User_index.aspx") %>">←返回首頁</a>
        </div>
        <div class="row">
            <!-- 左邊欄位：個人資料 -->
            <div class="col-lg-9 col-md-6">
                <h5 class="mb-4 step-title">個人資料</h5>
                <div class="row">
                    <div class="col-5 d-flex justify-content-center align-items-center">
                        <div class="text-center">
                            <asp:Image ID="user_img" runat="server" CssClass="circular-image" Style="width: 300px; height: 300px;" draggable="false" />
                            <asp:FileUpload ID="FileUpload1" runat="server" Visible="false" onchange="previewImage();" />
                        </div>
                    </div>
                    <div class="col-7">
                        <div class="form-group">
                            <!-- 帳號 -->
                            <div class="mb-3 d-flex align-items-center">
                                <span class="form-label" style="white-space: nowrap;">帳號：</span>
                                <asp:Label ID="lb_account" runat="server" Text="Label" CssClass="d-inline-block" Font-Size="X-Large" Style="white-space: nowrap;" />
                            </div>
                            <!-- 姓名 -->
                            <div class="mb-3 d-flex align-items-center">
                                <span class="form-label" style="white-space: nowrap;">姓名：</span>
                                <asp:Label ID="lb_name" runat="server" Text="Label" CssClass="d-inline-block" Font-Size="X-Large" Style="white-space: nowrap;" />
                                <asp:TextBox ID="tb_name" runat="server" CssClass="form-control" Visible="False" Style="width: 80%; font-size: 1.5rem; display: inline-block;" />
                            </div>
                            <!-- 性別 -->
                            <div class="mb-3 d-flex align-items-center">
                                <span class="form-label">性別：</span>
                                <asp:Label ID="lb_gender" runat="server" Text="Label" Font-Size="X-Large" Style="display: inline-block;"></asp:Label>
                                <asp:DropDownList ID="tb_gender" runat="server" CssClass="form-control" Visible="False" Style="width: 80%; font-size: 1.5rem; display: inline-block;">
                                    <asp:ListItem Text="男生" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="女生" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="不願透露" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <!-- 電話 -->
                            <div class="mb-3 d-flex align-items-center">
                                <span class="form-label" style="white-space: nowrap;">電話：</span>
                                <asp:Label ID="lb_phone" runat="server" Text="Label" CssClass="d-inline-block" Font-Size="X-Large" Style="white-space: nowrap;" />
                                <asp:TextBox ID="tb_phone" runat="server" CssClass="form-control" Visible="False" Style="width: 80%; font-size: 1.5rem;" />
                            </div>
                            <!-- 信箱 -->
                            <div class="mb-3 d-flex align-items-center">
                                <span class="form-label" style="white-space: nowrap;">信箱：</span>
                                <asp:Label ID="lb_email" runat="server" Text="Label" CssClass="d-inline-block" Font-Size="X-Large" Style="white-space: nowrap;" />
                                <asp:TextBox ID="tb_email" runat="server" CssClass="form-control" Visible="False" Style="width: 80%; font-size: 1.5rem;" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- 右邊欄位：更改密碼 -->
            <div class="col-lg-3 col-md-6">
                <h5 class="mb-4 step-title">更改密碼</h5>
                <div class="container p-0">
                    <div class="form-group mb-4">
                        <asp:TextBox ID="Txtpassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="請輸入原密碼" />
                    </div>
                    <div class="form-group mb-4">
                        <asp:TextBox ID="Txtnewpassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="請輸入新密碼" />
                    </div>
                    <div class="form-group mb-4">
                        <asp:TextBox ID="Txtnewpassword2" runat="server" CssClass="form-control" TextMode="Password" placeholder="請再次輸入新密碼" />
                        <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="新密碼不一致" ControlToCompare="Txtnewpassword" ControlToValidate="Txtnewpassword2" ForeColor="Red" Display="Dynamic" />
                    </div>
                    <asp:Button ID="PWDBtnSave" runat="server" Text="更新" CssClass="btn btn-outline-primary w-100" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-9 col-md-6">

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
