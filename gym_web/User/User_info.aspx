<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="User_info.aspx.cs" Inherits="User_User_info" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Page Header Start -->
<div class="container-fluid page-header mb-5">
    <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 130px">
    </div>
</div>
<!-- Page Header End -->
    <section class="team  py-lg-4 py-md-3 py-sm-3 py-3" id="team">
        <div class="container py-lg-5 py-md-4 py-sm-4 py-3">
            <h3 class="title text-center clr mb-lg-5 mb-md-4 mb-sm-4 mb-3">維護個人資料</h3>
            <div class="row text-center ">
                <div class="col-lg-9 col-md-6 col-sm-6 news-img aos-init aos-animate" data-aos="fade-up" data-aos-duration="3000">
                    <div class="team-agile-group">
                        <h4>個人資料</h4>
                        <div style="display: flex; padding: 1rem;">
                            <div style="flex: 1; margin-right: 1rem; position: relative;">
                                <asp:Image ID="user_img" runat="server" Height="200px" Width="200px" CssClass="circular-image"
                                    ImageUrl="~/User/images/account_default.png" />
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
                                    <div style="margin-bottom: 1rem;">
                                        <span style="font-size: x-large;">性別：</span>
                                        <asp:Label ID="lb_gender" runat="server" Text="Label" Font-Size="X-Large" Font-Bold="True" Style="display: inline-block;"></asp:Label>
                                        <asp:DropDownList ID="tb_gender" runat="server" Visible="False"  SelectionMode="Single" Font-Size="X-Large" Font-Bold="True" Style="display: inline-block;">
                                            <asp:ListItem Text="男生" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="女生" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="無性別" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
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
                        <asp:Button ID="Btn_delete" runat="server" Text="刪除帳號" CssClass="btn sent-butnn form-control" Style="width: 100%;" OnClick="Btn_delete_Click"/>
                    </div>

                    
                </div>
            </div>
        </div>
    </section>
</asp:Content>


