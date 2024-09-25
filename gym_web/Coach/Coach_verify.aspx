<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="Coach_verify.aspx.cs" Inherits="Coach_Coach_verify" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>健身房註冊</title>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10/dist/sweetalert2.all.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10/dist/sweetalert2.all.min.js"></script>

    <style>
        .container {
            max-width: 300px;
        }

        .form-group {
            margin-bottom: 15px;
        }

        .label {
            display: block;
            font-weight: bold;
        }

        .input {
            width: 50%;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 5px;
        }

        .button {
            background-color: #007bff;
            color: #fff;
            padding: 10px 20px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
        }
    </style>
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <h2>驗證健身教練身分</h2>
        <div class="form-group">
            <label class="label" for="txtPhone">類型：</label>
            <asp:RadioButtonList ID="rdtype" runat="server" AutoPostBack="True" CssClass="radioButtonList" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdVerifyMode_SelectedIndexChanged">
                <asp:ListItem Value="店家健身教練" Selected="True">店家健身教練</asp:ListItem>
                <asp:ListItem Value="私人健身教練">私人健身教練</asp:ListItem>
            </asp:RadioButtonList>
            <asp:RequiredFieldValidator ID="rfvtype" runat="server" ControlToValidate="rdtype"
                InitialValue="" ErrorMessage="請選擇類型" Display="Dynamic" ForeColor="Red" ValidationGroup="gp1" />
        </div>
        <div class="form-group" id="search_type" runat="server">
            <asp:RadioButtonList ID="rdVerifyMode" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rdVerifyMode_SelectedIndexChanged">
                <asp:ListItem Text="依照地區查詢店家" Value="search" Selected="True"></asp:ListItem>
                <asp:ListItem Text="手動輸入我的服務單位" Value="manual"></asp:ListItem>
            </asp:RadioButtonList>
        </div>

        <div class="form-group" id="search_form" runat="server">
            <asp:Label ID="Label1" runat="server" Text="依照地區查詢店家"></asp:Label>
            <asp:DropDownList ID="ddl_city" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource2" DataTextField="縣市" DataValueField="縣市id" OnDataBound="ddl_city_DataBound" OnSelectedIndexChanged="ddl_city_SelectedIndexChanged"></asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ManagerConnectionString %>" SelectCommand="SELECT * FROM [縣市]"></asp:SqlDataSource>
            <asp:DropDownList ID="ddl_area" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_area_SelectedIndexChanged" OnDataBound="ddl_area_DataBound"></asp:DropDownList>
            <asp:Label ID="lb_no_result" runat="server" Text="*查無資料" ForeColor="Red" Font-Size="X-Large" Visible="false"></asp:Label>
        </div>

        <div class="form-group" id="search_form_detail" runat="server">
            <div style="text-align: center">
                <div style="margin-left: 100px;">
                    <asp:DataList ID="dl_shop" runat="server" RepeatDirection="Horizontal" RepeatColumns="5" OnItemCommand="dl_shop_ItemCommand">
                        <itemtemplate>
                            <asp:LinkButton ID="lnkButton" runat="server" CommandName="select_shop" CommandArgument='<%# Eval("服務地點名稱") + ";" + Eval("服務地點電話") + ";" + Eval("服務地點郵件") + ";" + Eval("服務地點地址") %>'>
                                <div class="card repeater-item" style="display: inline-block; width: 250px; height: 80px; text-align: center;">
                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("服務地點名稱") %>' Font-Bold="True" Font-Size="20px"></asp:Label>
                                    <br />
                                    <asp:Label ID="Label8" runat="server" Text='<%# Eval("服務地點地址") %>'></asp:Label>
                                    <br />
                                </div>
                            </asp:LinkButton>
                        </itemtemplate>
                    </asp:DataList>

                </div>
            </div>
        </div>


        <div class="form-group" id="Name" runat="server">
            <label class="label" for="txtName">名稱：</label>
            <asp:TextBox ID="txtName" runat="server" CssClass="input"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                InitialValue="" ErrorMessage="請輸入名稱" Display="Dynamic" ForeColor="Red" ValidationGroup="gp1" />
        </div>

        <div class="form-group" id="Phone" runat="server">
            <label class="label" for="txtPhone">電話：</label>
            <asp:TextBox ID="txtPhone" runat="server" CssClass="input"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhone"
                InitialValue="" ErrorMessage="請輸入電話" Display="Dynamic" ForeColor="Red" ValidationGroup="gp1" />
        </div>
        <div class="form-group" id="Email" runat="server">
            <label class="label" for="txtEmail">Email：</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="input"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                InitialValue="" ErrorMessage="請輸入Email" Display="Dynamic" ForeColor="Red" ValidationGroup="gp1" />
        </div>
        <div class="form-group" id="Address" runat="server">
            <label class="label" for="txtAddress">地址：</label>
            <asp:TextBox ID="txtAddress" runat="server" CssClass="input"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddress"
                InitialValue="" ErrorMessage="請輸入地址" Display="Dynamic" ForeColor="Red" ValidationGroup="gp1" />
        </div>
        <div class="form-group">
            <label class="label" for="fuVerificationData">上傳審核資料</label>
            <label class="label" for="fuVerificationData">健身房教練：在職證明/工作證/相關文件</label>
            <label class="label" for="fuVerificationData">工作室教練：在職證明/營業登記證/相關文件</label>
            <label class="label" for="fuVerificationData">私人教練：體育證照/相關經歷/相關文件</label>
            <asp:FileUpload ID="fuVerificationData" runat="server" CssClass="input" multiple="multiple" accept=".pdf"/>
            <asp:RequiredFieldValidator ID="rfvVerificationData" runat="server" ControlToValidate="fuVerificationData"
                InitialValue="" ErrorMessage="請上傳審核資料" Display="Dynamic" ForeColor="Red" ValidationGroup="gp1" />
        </div>

        <div class="form-group">
            <asp:Button ID="btnSubmit" runat="server" Text="提交" CssClass="button" OnClick="btnSubmit_Click" ValidationGroup="gp1" />
        </div>
    </div>
</asp:Content>

