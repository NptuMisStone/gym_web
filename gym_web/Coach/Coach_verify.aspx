<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="Coach_verify.aspx.cs" Inherits="Coach_Coach_verify" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10/dist/sweetalert2.all.min.js"></script>

    <script>
        function previewImage(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    document.getElementById('imgPreview').src = e.target.result;
                }
                reader.readAsDataURL(input.files[0]);
            }
        }
        function scrollToControl(controlId) {
            var element = document.getElementById(controlId);
            if (element) {
                element.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }
        }
    </script>
    <style>
        .step-title {
            font-size: 1.5rem; /* 字體大小 */
            font-weight: bold; /* 粗體字 */
            color: #e31c25; /* 標題顏色 */
            border-bottom: 2px solid #e31c25; /* 底部邊框 */
            padding-bottom: 10px; /* 底部內邊距 */
            margin-bottom: 15px; /* 與下方內容的間距 */
            text-align: left; /* 左對齊 */
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Page Header Start -->
    <div class="container-fluid page-header mb-5">
        <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 400px">
            <h4 class="display-4 mb-3 mt-0 mt-lg-5 text-white text-uppercase font-weight-bold">驗證健身教練身分</h4>
        </div>
    </div>

    <div id="home" runat="server" class="container w-50">
        <div class="form-group">
            <h5 class="mb-4 step-title">類型</h5>
            <asp:RadioButtonList ID="rdtype" runat="server" AutoPostBack="True" CssClass="radioButtonList" OnSelectedIndexChanged="rdVerifyMode_SelectedIndexChanged">
                <asp:ListItem Value="店家健身教練" Selected="True">店家健身教練</asp:ListItem>
                <asp:ListItem Value="私人健身教練">私人健身教練</asp:ListItem>
            </asp:RadioButtonList>
            <asp:RequiredFieldValidator ID="rfvtype" runat="server" ControlToValidate="rdtype"
                InitialValue="" ErrorMessage="請選擇類型" Display="Dynamic" ForeColor="Red" ValidationGroup="gp1" />
        </div>

        <div id="Shop" runat="server">
            <div class="form-group" id="search_type" runat="server">
                <h5 class="mb-4 step-title">店家查詢方式</h5>
                <asp:RadioButtonList ID="rdVerifyMode" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rdVerifyMode_SelectedIndexChanged">
                    <asp:ListItem Text="地區查詢" Value="search" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="自行輸入" Value="manual"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div id="search_form" runat="server">
                <div class="form-group">
                    <h5 class="mb-4 step-title">地區查詢</h5>
                    <div class="row" style="margin-top: 10px;">
                        <div class="col-md-6">
                            <asp:DropDownList ID="ddl_city" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_city_SelectedIndexChanged" CssClass="custom-select text-muted"></asp:DropDownList>
                        </div>
                        <div class="col-md-6">
                            <asp:DropDownList ID="ddl_area" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_area_SelectedIndexChanged" OnDataBound="ddl_area_DataBound" CssClass="custom-select text-muted"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group text-center mt-2">
                    <asp:Label ID="lb_no_result" runat="server" Text="※查無資料" ForeColor="Red" Font-Size="X-Large" Visible="false"></asp:Label>
                </div>
                <div class="form-group text-center">
                    <!-- 使用 Bootstrap 的 text-center 控制水平對齊 -->
                    <div class="row justify-content-center">
                        <!-- 使用 Bootstrap 的 row 和 justify-content-center 來居中顯示 -->
                        <asp:DataList ID="dl_shop" runat="server" RepeatDirection="Horizontal" RepeatColumns="5" OnItemCommand="dl_shop_ItemCommand">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkButton" runat="server" CommandName="select_shop" CommandArgument='<%# Eval("服務地點名稱") + ";" + Eval("服務地點電話") + ";" + Eval("服務地點郵件") + ";" + Eval("服務地點地址") + ";" + Eval("縣市id") + ";" + Eval("行政區id") %>'>
                                    <div class="card m-2" style="width: 18rem;">
                                        <!-- m-2 提供 margin，控制卡片之間的間距 -->
                                        <div class="card-body text-center">
                                            <!-- 控制卡片內文字的對齊 -->
                                            <h5 class="card-title"><%# Eval("服務地點名稱") %></h5>
                                            <p class="card-text text-muted"><%# Eval("縣市") %><%# Eval("行政區") %><%# Eval("服務地點地址") %></p>
                                            <!-- 使用 class 控制樣式 -->
                                        </div>
                                    </div>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </div>
            </div>


            <div id="Shop_data" runat="server">
                <div class="form-group">
                    <h5 class="mb-4 step-title">店家資料</h5>
                    <p>店家名稱：</p>
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="請輸入店家名稱"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                        InitialValue="" ErrorMessage="請輸入名稱" Display="Dynamic" ForeColor="Red" ValidationGroup="gp1" />
                </div>

                <div class="form-group">
                    <p>店家電話：</p>
                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="請輸入店家電話"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhone"
                        InitialValue="" ErrorMessage="請輸入電話" Display="Dynamic" ForeColor="Red" ValidationGroup="gp1" />
                </div>

                <div class="form-group">
                    <p>店家Email：</p>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="請輸入店家Email"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                        InitialValue="" ErrorMessage="請輸入Email" Display="Dynamic" ForeColor="Red" ValidationGroup="gp1" />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server"
                        ControlToValidate="txtEmail"
                        ErrorMessage="請輸入正確的電子郵件地址"
                        ForeColor="Red"
                        ValidationGroup="gp1"
                        ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                        SetFocusOnError="True" Display="Dynamic"></asp:RegularExpressionValidator>
                </div>

                <div class="form-group">
                    <p>店家地址：</p>
                    <div class="row">
                        <div class="col-md-6">
                            <asp:DropDownList ID="ddl_city1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_city1_SelectedIndexChanged" CssClass="custom-select text-muted" required="required"></asp:DropDownList>
                        </div>
                        <div class="col-md-6">
                            <asp:DropDownList ID="ddl_area1" runat="server" CssClass="custom-select text-muted" required="required"></asp:DropDownList>
                        </div>
                    </div>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control mt-2" placeholder="請輸入詳細地址（不須輸入縣市行政區）"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddress"
                        InitialValue="" ErrorMessage="請輸入地址" Display="Dynamic" ForeColor="Red" ValidationGroup="gp1" />
                </div>
            </div>
        </div>

        <div class="form-group">
            <h5 class="mb-4 step-title">上傳審核資料</h5>
            <p>健身房教練：在職證明/工作證/相關文件</p>
            <p>工作室教練：在職證明/營業登記證/相關文件</p>
            <p>私人教練：體育證照/相關經歷/相關文件</p>
            <asp:FileUpload ID="fuVerificationData" runat="server" CssClass="form-control" />
            <asp:RequiredFieldValidator ID="rfvFile" runat="server" ControlToValidate="fuVerificationData"
                ErrorMessage="請上傳檔案" Display="Dynamic" ForeColor="Red" ValidationGroup="gp1" />
            <img id="imgPreview" src="" style="display: none; width: 200px; height: 150px; margin-top: 10px;" />
        </div>

        <div class="form-group text-center">
            <asp:Button ID="btnSubmit" runat="server" Text="提交申請" CssClass="btn btn-primary" OnClick="btnSubmit_Click" ValidationGroup="gp1" />
        </div>
    </div>
</asp:Content>
