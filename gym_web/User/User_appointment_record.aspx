<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage.master" AutoEventWireup="true" CodeFile="User_appointment_record.aspx.cs" Inherits="User_User_appointment_record" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-xxl py-5">
    <div class="container">
        <div class="text-center wow fadeInUp" data-wow-delay="0.1s">
            <h1 class="mb-5">預約紀錄</h1>
        </div>
         <center>
                    <table style="width: 300px;">
                        <tr>
                            <td style="text-align:right;" >
                                <div style="width: 15px; height: 15px; background-color: #475766"></div>
                            </td>
                            <td style="text-align:left;" >
                                <asp:Label ID="Label2" runat="server" Text="預約中"></asp:Label>
                            </td>
                            <td style="text-align:right;">
                                <div style="width: 15px; height: 15px; background-color: #86B817"></div>
                            </td>
                            <td style="text-align:left;">
                                <asp:Label ID="Label7" runat="server" Text="已完成"></asp:Label>
                            </td>
                            <td style="text-align:right;">
                                <div style="width: 15px; height: 15px; background-color: #B6B6B6"></div>
                            </td>
                            <td style="text-align:left;">
                                <asp:Label ID="Label9" runat="server" Text="已取消"></asp:Label>
                            </td>
                            <td style="text-align:right;">
                                <div style="width: 15px; height:15px; background-color: #CF808B"></div>
                            </td>
                            <td style="text-align:left;">
                                <asp:Label ID="Label10" runat="server" Text="逾時"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </center>
        <div class="row g-4 justify-content-center">
            <div class="text-center p-4">
                <asp:Label ID="Label16" runat="server" Text="查詢預約狀態" ForeColor="Black"></asp:Label>&nbsp;&nbsp;
                <asp:DropDownList ID="ddl_status" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_status_SelectedIndexChanged" >
                    <asp:ListItem Value="0">全部</asp:ListItem>
                    <asp:ListItem Value="1">預約中</asp:ListItem>
                    <asp:ListItem Value="2">已完成</asp:ListItem>
                    <asp:ListItem Value="3">已取消</asp:ListItem>
                    <asp:ListItem Value="4">逾時</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="lb_count" runat="server" Text="Label"></asp:Label>
                <asp:Label ID="lb_norecord" runat="server" Text="*查無預約紀錄" ForeColor="#CC0000" Visible="False"></asp:Label>
            </div>
            
        </div>
        <center>
            <asp:DataList ID="dl_record" runat="server" OnItemCommand="dl_record_ItemCommand">
                <ItemTemplate>
                    <div id="scrollContainer" class="card record" style='<%# " width: 300px; border-left-style: solid; border-left-color: " + GetBorderColor(Eval("預約狀態")) + "; border-left-width: 10px;" %>' >
                        <asp:Label ID="lb_c_name" runat="server" Text='<%# "健身教練："+ Eval("健身教練姓名") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_course" runat="server" Text='<%# "課程名稱："+ Eval("課程名稱") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_date" runat="server" Text='<%# "預約日期：" + Eval("日期","{0:yyyy/MM/dd }") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_time" runat="server" Text='<%# "預約時間：" + Eval("開始時間") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_status" runat="server" Text='<%# "預約狀態："+ GetStatusText(Eval("預約狀態")) %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_fee" runat="server" Text='<%# "費用："+ Eval("課程費用","{0:f0}") +"元"%>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_text" runat="server" Text='<%# "備註："+ Eval("備註") %>' Font-Size="16px"></asp:Label><br />
                        <asp:Button ID="btm_cancel_appointment" CommandName="cancel" runat="server" Text="取消預約" class="btn btn-sm btn-primary px-3 border-end" CommandArgument='<%# Eval("預約編號") %>' Visible='<%# Eval("預約狀態").ToString() == "1" ? true : false %>' Width="100px" Style=" Font-Size:16px; background-color:#475766" />
                        <asp:Button ID="btn_comment" runat="server" CommandName="comment" Text="對健身教練留下評論" class="btn btn-sm btn-primary px-3 border-end" Visible='<%# !HasCommented(Eval("預約編號")) && Eval("預約狀態").ToString() == "2" %>' CommandArgument='<%# Eval("預約編號") %>' Width="180px" Style=" Font-Size:16px; background-color:#86B817" />
                        <asp:Label ID="Label1" runat="server" Text="你已經評論過了" Visible='<%# HasCommented(Eval("預約編號")) && Eval("預約狀態").ToString() == "2"%>' Font-Size="16px"></asp:Label>
                        <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Height="50px" Width="250px" Visible="false"></asp:TextBox>
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </center>
    </div>
</div>
</asp:Content>

