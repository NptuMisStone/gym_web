<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage.master" AutoEventWireup="true" CodeFile="User_appointment.aspx.cs" Inherits="User_User_appointment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
        <h3>請確認您的預約項目</h3>
        <span>名稱：</span>
        <asp:Label ID="ap_course_name" runat="server"  Text='<%#  Eval("課程名稱")  %>' Font-Size="Large"></asp:Label><br>
        <span>時長：</span>
        <asp:Label ID="ap_course_time" runat="server"  Text='<%#  Eval("課程時間長度")  %>' Font-Size="Large"></asp:Label>
        <span>分鐘</span>
        <br/>
        <span>地點：</span>
        <asp:Label ID="ap_course_add" runat="server"  Text='<%#  Eval("服務地點名稱")  %>' Font-Size="Large"></asp:Label><br>
        <asp:Label ID="Label1" runat="server"  Text="預約時段：" Font-Size="Large"></asp:Label>
        <asp:Label ID="ap_course_date" runat="server"  Text='<%#  Eval("日期")  %>' Font-Size="Large"></asp:Label>
        <span>(</span>
        <asp:Label ID="ap_course_stTime" runat="server"  Text='<%#  Eval("開始時間")  %>' Font-Size="Large"></asp:Label>
        <span>~</span>
        <asp:Label ID="ap_course_edTime" runat="server"  Text='<%#  Eval("結束時間")  %>' Font-Size="Large"></asp:Label>
        <span>)</span>
        <br />
        <asp:Label ID="Label3" runat="server"  Text="給教練的留言：" Font-Size="Large"></asp:Label><br>
        <asp:TextBox ID="ap_text" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="Label4" runat="server"  Text="到府服務地址：" Font-Size="Large"></asp:Label><br>
        <asp:TextBox ID="ap_location" runat="server" Enabled="false"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvlocation" runat="server" ControlToValidate="ap_location" ErrorMessage="到府地址不得為空" ForeColor="Red" Display="Dynamic" Enabled="false"  />
        <br />
        <br />
        <asp:Button ID="ap_btn" runat="server" Text="確認預約" OnClick="ap_btn_Click" CssClass="btn-info" />

    </div>
</asp:Content>

