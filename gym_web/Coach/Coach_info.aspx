<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="Coach_info.aspx.cs" Inherits="Coach_Coach_info1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Page Header Start -->
    <div class="container-fluid page-header mb-5">
        <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 400px">
            <h4 class="display-4 mb-3 mt-0 mt-lg-5 text-white text-uppercase font-weight-bold">維護個人資料</h4>
        </div>
    </div>
    <!-- Page Header End -->

    <!-- Blog Detail Start -->
    <div class="container">
        <div class="row">
            <div class="col-12 pt-2">
                <!-- 將 pt-4 改成 pt-2，減少上方距離 -->
                <div id="divStatusBlock" runat="server" class="media bg-secondary text-white mb-3 p-4">
                    <!-- 減少外邊距和內邊距 -->
                    <div class="row w-100">
                        <!-- 左邊圖示區塊，佔2份 -->
                        <div class="col-md-2 d-flex align-items-center justify-content-center">
                            <asp:Image ID="img_Status" runat="server" Style="width: 100px; height: 100px;" draggable="false" />
                        </div>
                        <!-- 中間文字區塊，佔6份 -->
                        <div class="col-md-6 d-flex align-items-center">
                            <div class="media-body">
                                <asp:Label ID="lblReviewStatus" runat="server" class="text-primary mb-2" Font-Size="X-Large"></asp:Label><br />
                                <!-- 將 mb-3 改成 mb-2，縮小文字區塊的外邊距 -->
                                <asp:Label ID="lblReviewStatusText" runat="server" class="m-0"></asp:Label>
                            </div>
                        </div>
                        <!-- 右邊按鈕區塊，佔4份 -->
                        <div class="col-md-4 d-flex align-items-center justify-content-end">
                            <asp:Button ID="btn_verify" runat="server" Text="立即驗證" CssClass="btn btn-outline-primary mt-1 px-4 py-2 btn-lg" OnClick="btn_verify_Click" />
                            <!-- 減少按鈕上下和左右的間距 -->
                        </div>

                        <div class="container-fluid pt-2">
                            <asp:Repeater ID="rp_coach" runat="server" OnItemDataBound="rp_coach_ItemDataBound">
                                <ItemTemplate>
                                    <asp:Panel ID="Panel_store" runat="server">
                                        <div class="row py-3 mx-auto">
                                            <!-- 服務地點名稱 -->
                                            <div class="col-sm-6 col-md-3 mb-3">
                                                <div class="d-flex align-items-start">
                                                    <img src="img/home.png" class="mr-2" style="height: 30px; flex-shrink: 0;" draggable="false">
                                                    <div style="display: flex; flex-direction: column;">
                                                        <h5 class="font-weight-bold mb-0 text-white" style="white-space: nowrap;">服務店家</h5>
                                                        <p class="mb-0"><%# Eval("服務地點名稱") %></p>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- 服務地點地址 -->
                                            <div class="col-sm-6 col-md-3 mb-3">
                                                <div class="d-flex align-items-start">
                                                    <img src="img/maps.png" class="mr-2" style="height: 30px; flex-shrink: 0;" draggable="false">
                                                    <div style="display: flex; flex-direction: column;">
                                                        <h5 class="font-weight-bold mb-0 text-white" style="white-space: nowrap;">店家地址</h5>
                                                        <p class="mb-0"><%# Eval("服務地點地址") %></p>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- 服務地點電話 -->
                                            <div class="col-sm-6 col-md-3 mb-3">
                                                <div class="d-flex align-items-start">
                                                    <img src="img/telephone.png" class="mr-2" style="height: 30px; flex-shrink: 0;" draggable="false">
                                                    <div style="display: flex; flex-direction: column;">
                                                        <h5 class="font-weight-bold mb-0 text-white" style="white-space: nowrap;">店家電話</h5>
                                                        <p class="mb-0" style="white-space: normal; word-wrap: break-word; word-break: break-all;">
                                                            <%# Eval("服務地點電話") %>
                                                        </p>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- 服務地點郵件 -->
                                            <div class="col-sm-6 col-md-3 mb-3">
                                                <div class="d-flex align-items-start">
                                                    <img src="img/email.png" class="mr-2" style="height: 30px; flex-shrink: 0;" draggable="false">
                                                    <div style="display: flex; flex-direction: column;">
                                                        <h5 class="font-weight-bold mb-0 text-white" style="white-space: nowrap;">店家郵件</h5>
                                                        <p class="mb-0" style="white-space: normal; word-wrap: break-word; word-break: break-all;">
                                                            <%# Eval("服務地點郵件") %>
                                                        </p>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Blog Detail End -->





    <section class="team  py-lg-4 py-md-3 py-sm-3 py-3" id="team">
        <div class="container py-lg-5 py-md-4 py-sm-4 py-3">
            <div class="row text-center ">
                <div class="col-lg-9 col-md-6 col-sm-6 news-img aos-init aos-animate" data-aos="fade-up" data-aos-duration="3000">
                    <div class="team-agile-group">
                        <h4>個人資料</h4>
                        <div style="display: flex; padding: 1rem;">
                            <div style="flex: 1; margin-right: 1rem; position: relative;">
                                <asp:Image ID="coach_img" runat="server" Height="200px" Width="200px" CssClass="circular-image"
                                    ImageUrl="~/Coach/images/account_default.png" />
                                <asp:FileUpload ID="FileUpload1" runat="server" Visible="false" />
                            </div>
                            <div style="flex: 2;" class="form-group contact-forms">
                                <div style="text-align: left;">
                                    <div style="margin-bottom: 1rem;">
                                        <span style="font-size: x-large;">帳號：</span>
                                        <asp:Label ID="lb_account" runat="server" Text="Label" Font-Size="X-Large" Font-Bold="True" Style="display: inline-block;"></asp:Label>
                                    </div>
                                    <div style="margin-bottom: 1rem;">
                                        <span style="font-size: x-large;">姓名：</span>
                                        <asp:Label ID="lb_name" runat="server" Text="Label" Font-Size="X-Large" Font-Bold="True" Style="display: inline-block;"></asp:Label>
                                        <asp:TextBox ID="tb_name" TextMode="SingleLine" runat="server" CssClass="form-control" Visible="False" Style="display: inline-block; width: 300px; height: 40px; font-size: x-large"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv_name" runat="server" ControlToValidate="tb_name" ErrorMessage="姓名不得為空" ForeColor="Red" Display="Dynamic" />
                                    </div>
                                    <div style="margin-bottom: 1rem;">
                                        <span style="font-size: x-large;">電話：</span>
                                        <asp:Label ID="lb_phone" runat="server" Text="Label" Font-Size="X-Large" Font-Bold="True" Style="display: inline-block;"></asp:Label>
                                        <asp:TextBox ID="tb_phone" TextMode="SingleLine" runat="server" CssClass="form-control" Visible="False" Style="display: inline-block; width: 300px; height: 40px; font-size: x-large"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv_phone" runat="server" ControlToValidate="tb_phone" ErrorMessage="電話不得為空" ForeColor="Red" Display="Dynamic" />
                                    </div>
                                    <div style="margin-bottom: 1rem;">
                                        <span style="font-size: x-large;">信箱：</span>
                                        <asp:Label ID="lb_email" runat="server" Text="Label" Font-Size="X-Large" Font-Bold="True" Style="display: inline-block;"></asp:Label>
                                        <asp:TextBox ID="tb_email" TextMode="SingleLine" runat="server" CssClass="form-control" Visible="False" Style="display: inline-block; width: 300px; height: 40px; font-size: x-large"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv_email" runat="server" ControlToValidate="tb_email" ErrorMessage="信箱不得為空" ForeColor="Red" Display="Dynamic" />
                                    </div>
                                    <div id="type" runat="server" style="margin-bottom: 1rem;">
                                        <span style="font-size: x-large;">身分：</span>
                                        <asp:Label ID="lb_type" runat="server" Text="Label" Font-Size="X-Large" Font-Bold="True" Style="display: inline-block;"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="display: flex; padding: 1rem; border-radius: 0.5rem; margin-bottom: 1.5rem; background-color: whitesmoke;" class="form-group contact-forms">
                            <div>
                                <span style="font-size: x-large;">介紹：</span>
                                <asp:Label ID="lb_about" runat="server" Text="Label" Font-Size="X-Large"></asp:Label>
                                <asp:TextBox ID="tb_about" TextMode="MultiLine" runat="server" CssClass="form-control" Visible="False" Style="resize: none; display: inline-block; width: 700px; height: 100px; font-size: x-large"></asp:TextBox>
                            </div>
                        </div>
                        <asp:Panel ID="Panel_edit" runat="server">
                            <asp:Button ID="BtnEdit" runat="server" Text="修改" OnClick="BtnEdit_Click" CssClass="btn sent-butnn" />
                        </asp:Panel>
                        <asp:Panel ID="Panel_save" runat="server" Visible="false">
                            <asp:Button ID="BtnSave" runat="server" Text="儲存" OnClick="BtnSave_Click" CssClass="btn sent-butnn" />
                            <asp:Button ID="BtnCancel" runat="server" Text="取消" OnClick="BtnCancel_Click" CssClass="btn sent-butnn" CausesValidation="false" />
                        </asp:Panel>
                    </div>
                </div>

                <div class="col-lg-3 col-md-6 col-sm-6 news-img aos-init aos-animate" data-aos="fade-up" data-aos-duration="3000">
                    <div class="team-agile-group">
                        <h4 class="title text-center mb-lg-5 mb-md-4 mb-sm-4 mb-3">更改密碼</h4>
                        <div class="container col-lg-12 p-0">
                            <div class="form-group contact-forms mb-2">
                                <asp:TextBox ID="Txtpassword" TextMode="Password" runat="server" CssClass="form-control" placeholder="請輸入原密碼"></asp:TextBox>
                            </div>
                            <div class="form-group contact-forms mb-2">
                                <asp:TextBox ID="Txtnewpassword" TextMode="Password" runat="server" CssClass="form-control" placeholder="請輸入新密碼"></asp:TextBox>
                            </div>
                            <div class="form-group contact-forms mb-2">
                                <asp:TextBox ID="Txtnewpassword2" TextMode="Password" runat="server" CssClass="form-control" placeholder="請再次輸入新密碼"></asp:TextBox>
                                <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="新密碼不一致" ControlToCompare="Txtnewpassword" ControlToValidate="Txtnewpassword2" ForeColor="Red" SetFocusOnError="True" Display="Dynamic"></asp:CompareValidator>
                                <asp:CustomValidator ID="CustomValidator1" runat="server"
                                    ErrorMessage="密碼長度至少6個字元且不可與帳號相同"
                                    ControlToValidate="Txtnewpassword"
                                    ForeColor="Red"
                                    SetFocusOnError="True"
                                    OnServerValidate="CustomValidator1_ServerValidate" Display="Dynamic"></asp:CustomValidator>
                            </div>
                            <div>
                                <asp:Button ID="PWDBtnSave" runat="server" Text="更新" OnClick="PWDBtnSave_Click" CssClass="btn sent-butnn form-control" Style="width: 100%;" />
                            </div>
                        </div>
                    </div>

                    <div>
                        <asp:Button ID="Btn_delete" runat="server" Text="刪除帳號" CssClass="btn sent-butnn form-control" Style="width: 100%;" PostBackUrl="Coach_delete.aspx" />
                    </div>

                </div>
            </div>
        </div>
    </section>
</asp:Content>
