<%@ Page Language="C#" AutoEventWireup="true" CodeFile="User_forgetpwd.aspx.cs" Inherits="User_User_forgetpwd" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <title>忘記密碼</title>
    <link rel="stylesheet" href=" css/login.css">
    <link rel="stylesheet" type="text/css" href="slide navbar login.css">
    <link href="https://fonts.googleapis.com/css2?family=Jost:wght@500&display=swap" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="ScriptManager" />
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="View1" runat="server">
                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                    <ContentTemplate>
                        <div class="main">
                            <div class="signup">
                                <div class="custom-scrollbar">
                                    <label for="chk" aria-hidden="true">忘記密碼?</label>
                                    <asp:Label ID="Label6" runat="server" ForeColor="White" Style="margin-left: 70px;" Font-Bold="True" Text="請輸入您的帳號"></asp:Label>
                                    <asp:TextBox ID="tb_acc" runat="server" required="required"></asp:TextBox>
                                    <asp:Label ID="Label1" runat="server" ForeColor="White" Style="margin-left: 70px;" Font-Bold="True" Text="請輸入您的E-mail"></asp:Label>
                                    <asp:TextBox ID="tb_email" runat="server" required="required"></asp:TextBox>
                                    <asp:Label ID="Label2" runat="server" ForeColor="White" Style="margin-left: 70px;" Font-Bold="True" Text="請輸入驗證碼"></asp:Label>
                                    <asp:Label ID="Label5" runat="server" ForeColor="Red" Style="margin-left: 10px;" Font-Bold="True"></asp:Label>
                                    <asp:TextBox ID="tb_pwd1" runat="server"></asp:TextBox>
                                    <div id="remainingTimeLabel" style="margin-left: 70px; color: red; font-weight: bold;"></div>
                                    <asp:Button ID="Button1" runat="server" Text="獲取驗證碼"  Height="35px" OnClick="Button1_Click"  OnClientClick="startTimer()" />
                                    <asp:Button ID="Button4" runat="server" Text="確認"  CssClass="btn" OnClick="btn_ok_Click"  />
                                    <asp:HyperLink ID="HyperLink1" runat="server" Style="margin-left: 150px;" NavigateUrl="~/User/User_login.aspx" ForeColor="White">返回登入頁面</asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:View>
            <asp:View ID="View2" runat="server">
                <div class="main">
                    <div class="signup">
                        <div class="custom-scrollbar">
                            <label for="chk" aria-hidden="true">重設密碼</label>
                            <asp:Label ID="Label3" runat="server" ForeColor="White" Style="margin-left: 70px;" Font-Bold="True" Text="請輸入新密碼"></asp:Label>
                            <asp:TextBox ID="tb_new_pwd" runat="server" TextMode="Password"></asp:TextBox>
                            &emsp;&emsp;&emsp;&emsp;&ensp;
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="*密碼不一致~" ControlToCompare="tb_new_pwd" ControlToValidate="tb_new_pwd2" SetFocusOnError="True" ForeColor="#FFFFCC"></asp:CompareValidator><br />
                            <asp:Label ID="Label4" runat="server" ForeColor="White" Style="margin-left: 70px;" Font-Bold="True" Text="請再次輸入新密碼"></asp:Label>
                            <asp:TextBox ID="tb_new_pwd2" runat="server" TextMode="Password"></asp:TextBox>
                            <asp:Button ID="Button3" runat="server" Text="送出"  CssClass="btn" OnClick="Button3_Click" />
                        </div>
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>
    </form>
    <script type="text/javascript">
        var remainingTime;

        function startTimer() {
            var totalTime = 60; // 以秒為單位的總時間
            var timer = setInterval(function () {
                remainingTime = Math.ceil(totalTime-- / 600);
                document.getElementById('<%= Button1.ClientID %>').disabled = true;
                document.getElementById('<%= Button1.ClientID %>').innerText = remainingTime + "分鐘後可重新獲取驗證碼";
                document.getElementById('Label5').innerText = "*驗證碼已經傳送至您的信箱"; // 隱藏倒數時間

                // 新增顯示倒數時間的元素
                document.getElementById('remainingTimeLabel').innerText = "*"+remainingTime + "分鐘後才可重新獲取驗證碼";

                if (totalTime < 0) {
                    clearInterval(timer);
                    document.getElementById('<%= Button1.ClientID %>').disabled = false;
                    document.getElementById('<%= Button1.ClientID %>').innerText = "獲取驗證碼";
                    document.getElementById('remainingTimeLabel').innerText = ""; // 隱藏倒數時間
                    document.getElementById('Label5').innerText = ""; // 隱藏倒數時間
                }
            }, 1000);
        }
    </script>
</body>
</html>

