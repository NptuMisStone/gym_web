<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage.master" AutoEventWireup="true" CodeFile="Coach_info.aspx.cs" Inherits="Coach_Coach_info1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="team  py-lg-4 py-md-3 py-sm-3 py-3" id="team">
        <div class="container py-lg-5 py-md-4 py-sm-4 py-3">
            <h3 class="title text-center clr mb-lg-5 mb-md-4 mb-sm-4 mb-3">維護個人資料</h3>
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
                        <div id="store" runat="server" style="display: flex; padding: 1rem; border-radius: 0.5rem; margin-bottom: 1.5rem; background-color: whitesmoke;">
                            <div>
                                <span style="font-size: x-large;">服務店家：</span>
                                <asp:Label ID="lb_store" runat="server" Text="Label" Font-Size="X-Large"></asp:Label>
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
                            </div>
                            <div>
                                <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="新密碼不一致" ControlToCompare="Txtnewpassword" ControlToValidate="Txtnewpassword2" ForeColor="Red" SetFocusOnError="True"></asp:CompareValidator>
                            </div>
                            <div>
                                <asp:Button ID="PWDBtnSave" runat="server" Text="更新" OnClick="PWDBtnSave_Click" CssClass="btn sent-butnn form-control" Style="width: 100%;" />
                            </div>
                        </div>
                    </div>

                    <div>
                        <asp:Button ID="Btn_delete" runat="server" Text="刪除帳號" CssClass="btn sent-butnn form-control" Style="width: 100%;" PostBackUrl="Coach_delete.aspx"/>
                    </div>

                    <div id="reviewStatusDiv" runat="server" style="background-color: lightgray; padding: 20px; border-radius: 5px; margin-top: 20px;">
                        <asp:Label ID="lblReviewStatus" runat="server" Font-Size="X-Large"></asp:Label><br />
                        <asp:HyperLink ID="hlVerify" runat="server" NavigateUrl="Coach_verify.aspx" Text="立即驗證健身教練身分" Font-Size="Large" />
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>


