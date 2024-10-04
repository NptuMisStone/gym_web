<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="Coach_editclass.aspx.cs" Inherits="Coach_Coach_editclass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function previewImage() {
            var fileInput = document.getElementById('<%= fuCourseImage.ClientID %>');
            var image = document.getElementById('<%= Image1.ClientID %>');

            if (fileInput.files && fileInput.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    image.src = e.target.result;
                }
                reader.readAsDataURL(fileInput.files[0]);
            }

            // 當上傳圖片後，觸發隱藏的按鈕
            document.getElementById('<%= btnHiddenUpload.ClientID %>').click();
        }

        function scrollToControl() {
            var element = document.getElementById('<%= tbCourseFee.ClientID %>');
            if (element) {
                element.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }
        }

        function confirmDelete() {
            Swal.fire({
                title: '您確定要刪除課程嗎？',
                text: "刪除後將無法恢復！",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: '確認刪除',
                cancelButtonText: '取消'
            }).then((result) => {
                if (result.isConfirmed) {
                    // 用戶確認刪除後，手動觸發伺服器端按鈕的點擊事件
                    __doPostBack('<%= btnDeleteCourse.UniqueID %>', '');
            }
        });
        }

    </script>

    <!-- Page Header Start -->
    <div class="container-fluid page-header mb-5">
        <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 400px">
            <h4 class="display-4 mb-3 mt-0 mt-lg-5 text-white text-uppercase font-weight-bold">編輯課程</h4>
        </div>
    </div>

    <div class="container pt-5">
        <div class="row">
            <div class="col-md-6 pb-5 d-flex flex-column align-items-center">
                <h4 class="font-weight-boder">課程圖片</h4>
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Coach/img/upload.png"
                    Style="width: 100%; height: auto; max-width: 500px; max-height: 500px; object-fit: cover;"
                    draggable="false" />
                <asp:FileUpload ID="fuCourseImage" runat="server" onchange="previewImage();" />

                <!-- 隱藏按鈕 -->
                <asp:Button ID="btnHiddenUpload" runat="server" Text="隱藏按鈕" OnClick="btnHiddenUpload_Click" Style="display: none;" CausesValidation="false" />
            </div>

            <div class="col-md-6 pb-5">
                <div class="contact-form">
                    <div id="success"></div>
                    <div class="control-group mb-2">
                        <asp:TextBox ID="tbCourseName" runat="server" class="form-control" placeholder="課程名稱" required="required"></asp:TextBox>
                        <p class="help-block text-danger"></p>
                    </div>
                    <div class="control-group mb-2">
                        <asp:DropDownList ID="ddlCourseType" runat="server" class="custom-select text-muted" required="required"></asp:DropDownList>
                        <p class="help-block text-danger"></p>
                    </div>
                    <div class="control-group mb-2">
                        <asp:DropDownList ID="ddlCourseTime" runat="server" class="custom-select text-muted" required="required">
                            <asp:ListItem Text="30 分鐘" Value="30"></asp:ListItem>
                            <asp:ListItem Text="60 分鐘" Value="60"></asp:ListItem>
                            <asp:ListItem Text="90 分鐘" Value="90"></asp:ListItem>
                            <asp:ListItem Text="120 分鐘" Value="120"></asp:ListItem>
                            <asp:ListItem Text="150 分鐘" Value="150"></asp:ListItem>
                            <asp:ListItem Text="180 分鐘" Value="180"></asp:ListItem>
                            <asp:ListItem Text="210 分鐘" Value="210"></asp:ListItem>
                            <asp:ListItem Text="240 分鐘" Value="240"></asp:ListItem>
                        </asp:DropDownList>
                        <p class="help-block text-danger"></p>
                    </div>
                    <div class="control-group mb-2">
                        <asp:TextBox ID="tbCourseDescription" runat="server" TextMode="MultiLine" class="form-control" placeholder="課程介紹" required="required"></asp:TextBox>
                        <p class="help-block text-danger"></p>
                    </div>
                    <div class="control-group mb-2">
                        <asp:TextBox ID="tbRequiredEquipment" runat="server" class="form-control" placeholder="所需設備" required="required"></asp:TextBox>
                        <p class="help-block text-danger"></p>
                    </div>
                    <div class="control-group mb-2">
                        <asp:TextBox ID="tbCourseFee" runat="server" class="form-control" placeholder="課程費用" required="required"></asp:TextBox>
                        <p class="help-block text-danger"></p>
                    </div>

                    <!-- 新增上課模式選擇（單人或團體） -->
                    <div class="control-group mb-2">
                        <h4 class="font-weight-boder">上課人數</h4>
                        <asp:RadioButtonList ID="rblClassSize" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblClassSize_SelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem Value="1" Selected="True">一對一課程</asp:ListItem>
                            <asp:ListItem Value="2">團體課程</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="control-group mb-2">
                        <asp:TextBox ID="tbClassSize" runat="server" class="form-control mt-2" placeholder="團體課程人數" Visible="False"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvClassSize" runat="server" ControlToValidate="tbClassSize" ErrorMessage="上課人數不得為空" ForeColor="Red" Display="Dynamic" Enabled="false" />
                        <asp:CustomValidator ID="cvClassSize" runat="server" ControlToValidate="tbClassSize" ErrorMessage="團體課程人數必須大於 1" OnServerValidate="cvClassSize_ServerValidate" ForeColor="Red" Display="Dynamic" Enabled="false" />
                    </div>

                    <!-- 新增上課地點選擇 -->
                    <div class="control-group mb-2">
                        <h4 class="font-weight-boder">上課地點</h4>
                        <asp:RadioButtonList ID="rblLocation" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdlLocation_SelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem Value="2" Selected="True">到府(客戶指定地點)</asp:ListItem>
                            <asp:ListItem Value="3">其他(教練指定地點)：</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:TextBox ID="tbClassLocation" runat="server" class="form-control mt-2" placeholder="請輸入地點名稱" Visible="False"></asp:TextBox>

                        <div class="row" style="margin-top: 10px;">
                            <div class="col-md-6">
                                <asp:DropDownList ID="ddl_city" runat="server" AutoPostBack="True" class="custom-select text-muted"
                                    OnDataBound="ddl_city_DataBound" OnSelectedIndexChanged="ddl_city_SelectedIndexChanged"
                                    Visible="False" required="required">
                                </asp:DropDownList>
                            </div>

                            <div class="col-md-6">
                                <asp:DropDownList ID="ddl_area" runat="server" class="custom-select text-muted"
                                    Visible="False" required="required">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <asp:TextBox ID="tbClassAddress" runat="server" class="form-control mt-2" placeholder="請輸入詳細地址（不須輸入縣市行政區）" Visible="False"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvClassLocation" runat="server" ControlToValidate="tbClassLocation" ErrorMessage="地名不得為空" ForeColor="Red" Display="Dynamic" Enabled="false" />
                        <asp:RequiredFieldValidator ID="rfvClassAddress" runat="server" ControlToValidate="tbClassAddress" ErrorMessage="地址不得為空" ForeColor="Red" Display="Dynamic" Enabled="false" />
                    </div>
                    <div class="d-flex justify-content-between">
                        <asp:Button ID="btnUpdateCourse" runat="server" Text="更新課程" OnClick="btnUpdateCourse_Click" class="btn btn-outline-primary" Style="font-size: 20px" />

                        <button type="button" class="btn btn-outline-secondary" style="font-size: 20px;" onclick="window.location.href='Coach_class.aspx';">取消</button>

                        <asp:Button ID="btnDeleteCourse" runat="server" Text="刪除課程"
                            OnClick="btnDeleteCourse_Click"
                            OnClientClick="confirmDelete(); return false;"
                            CssClass="btn btn-danger" />

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

