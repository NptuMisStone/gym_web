<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="User_appointment_record.aspx.cs" Inherits="User_User_appointment_record" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style>
        .appointment-container {
            display: flex;
            justify-content: center;
            margin:auto;
            width:90%;
        }

        .column {
            margin-right: 1%;
            margin-left: 1%;
            width: 19%;
            box-sizing: border-box;
            background-color: #f8f9fa;
            border-radius: 8px;
        }

        .card.record {
            box-sizing: border-box; /* 确保 padding 和边框不会影响元素宽度 */
            padding: 10px;
            margin: 0; /* 消除内边距 */
            width: 100%; /* 确保内容占满整个列宽 */
            border:none;
        }

        h3 {
            text-align: center;
            font-size: 18px;
            margin-top: 10px;
            margin-bottom: 10px;
        }
    </style>
        <!-- Page Header Start -->
    <div class="container-fluid page-header mb-5">
        <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 400px">
            <h4 class="display-4 mb-3 mt-0 mt-lg-5 text-white text-uppercase font-weight-bold">使用者預約紀錄</h4>
        </div>
    </div>

    <!-- GYM Class Start -->
    <div class="container feature pt-5">
        <div class="d-flex flex-column text-center mb-5">
            <h4 class="text-primary font-weight-bold">NPTU GYM</h4>
            <h4 class="display-4 font-weight-bold">預約紀錄</h4>
        </div>
    </div>
    <div class="appointment-container">
        <div class="column" style="border: 2px solid #475766;">
            <h3>預約中</h3>
            <asp:DataList ID="dl_inProgress" runat="server" style="padding:0; margin:0; width:100%;" OnItemCommand="dl_inProgress_ItemCommand" >
                <ItemTemplate>
                    <div id="scrollContainer" class="card record" style=" border-top: 2px solid #475766;border-bottom: 2px solid #475766;"  >
                        <asp:Label ID="lb_c_name" runat="server" Text='<%# "健身教練："+ Eval("健身教練姓名") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_course" runat="server" Text='<%# "課程名稱："+ Eval("課程名稱") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_date" runat="server" Text='<%# "預約日期：" + Eval("日期","{0:yyyy/MM/dd }") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_time" runat="server" Text='<%# "預約時間：" + Eval("開始時間") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_status" runat="server" Text="預約狀態：預約中" Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_fee" runat="server" Text='<%# "費用："+ Eval("課程費用","{0:f0}") +"元"%>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_text" runat="server" Text='<%# "備註："+ Eval("備註") %>' Font-Size="16px"></asp:Label><br />
                        <asp:Button ID="btm_cancel_appointment" CommandName="cancel" runat="server" Text="取消預約"  CommandArgument='<%# Eval("預約編號") %>' Visible='<%# Eval("預約狀態").ToString() == "1" ? true : false %>' Width="100px" Style="border-color:#475766; Font-Size:16px; background-color:#475766;border-radius:8px;color:white;" />
                    </div>            
                </ItemTemplate>
            </asp:DataList>
        </div>
        <div class="column" style="border: 2px solid #86B817;">
            <h3>已完成</h3>
            <asp:DataList ID="dl_completed" runat="server" style="padding:0; margin:0; width:100%;" OnItemCommand="dl_completed_ItemCommand" >
                <ItemTemplate>
                    <div id="scrollContainer" class="card record" style="border-top: 2px solid #86B817;border-bottom: 2px solid #86B817;" >
                        <asp:Label ID="lb_c_name" runat="server" Text='<%# "健身教練："+ Eval("健身教練姓名") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_course" runat="server" Text='<%# "課程名稱："+ Eval("課程名稱") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_date" runat="server" Text='<%# "預約日期：" + Eval("日期","{0:yyyy/MM/dd }") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_time" runat="server" Text='<%# "預約時間：" + Eval("開始時間") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_status" runat="server" Text="預約狀態：已完成" Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_fee" runat="server" Text='<%# "費用："+ Eval("課程費用","{0:f0}") +"元"%>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_text" runat="server" Text='<%# "備註："+ Eval("備註") %>' Font-Size="16px"></asp:Label><br />
                        <asp:Button ID="btn_comment" runat="server" CommandName="comment" Text="對健身教練留下評論"  Visible='<%# !HasCommented(Eval("預約編號")) && Eval("預約狀態").ToString() == "2" %>' CommandArgument='<%# Eval("預約編號") %>' Width="180px" Style="border-color:#86B817; Font-Size:16px; background-color:#86B817;border-radius:8px;color:white;" />
                        <asp:Label ID="Label1" runat="server" Text="你已經評論過了" Visible='<%# HasCommented(Eval("預約編號")) && Eval("預約狀態").ToString() == "2"%>' Font-Size="16px"></asp:Label>
                        <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Height="50px" Width="250px" Visible="false"></asp:TextBox>
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </div>
        <div class="column" style="border: 2px solid #B6B6B6;">
            <h3>已取消</h3>
            <asp:DataList ID="dl_cancelled" runat="server"  style="padding:0; margin:0; width:100%;">
                <ItemTemplate>
                   <div id="scrollContainer" class="card record" style="border-top: 2px solid #B6B6B6;border-bottom: 2px solid #B6B6B6;" >
                        <asp:Label ID="lb_c_name" runat="server" Text='<%# "健身教練："+ Eval("健身教練姓名") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_course" runat="server" Text='<%# "課程名稱："+ Eval("課程名稱") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_date" runat="server" Text='<%# "預約日期：" + Eval("日期","{0:yyyy/MM/dd }") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_time" runat="server" Text='<%# "預約時間：" + Eval("開始時間") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_status" runat="server" Text="預約狀態：已取消" Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_fee" runat="server" Text='<%# "費用："+ Eval("課程費用","{0:f0}") +"元"%>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_text" runat="server" Text='<%# "備註："+ Eval("備註") %>' Font-Size="16px"></asp:Label><br />
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </div>
        <div class="column" style="border: 2px solid #CF808B;">
            <h3>逾時</h3>
            <asp:DataList ID="dl_overtime" runat="server" style="padding:0; margin:0; width:100%;" >
                <ItemTemplate>
                    <div id="scrollContainer" class="card record" style="border-top: 2px solid #CF808B;border-bottom: 2px solid #CF808B;" >
                        <asp:Label ID="lb_c_name" runat="server" Text='<%# "健身教練："+ Eval("健身教練姓名") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_course" runat="server" Text='<%# "課程名稱："+ Eval("課程名稱") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_date" runat="server" Text='<%# "預約日期：" + Eval("日期","{0:yyyy/MM/dd }") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_time" runat="server" Text='<%# "預約時間：" + Eval("開始時間") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_status" runat="server" Text="預約狀態：逾時" Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_fee" runat="server" Text='<%# "費用："+ Eval("課程費用","{0:f0}") +"元"%>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_text" runat="server" Text='<%# "備註："+ Eval("備註") %>' Font-Size="16px"></asp:Label><br />
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </div>
        <div class="column" style="border: 2px solid #E5B76A;">
            <h3>教練取消</h3>
            <asp:DataList ID="dl_coachCancelled" runat="server" style="padding:0; margin:0; width:100%;" >
                <ItemTemplate>
                    <div id="scrollContainer" class="card record" style="border-top: 2px solid #E5B76A;border-bottom: 2px solid #E5B76A;" >
                        <asp:Label ID="lb_c_name" runat="server" Text='<%# "健身教練："+ Eval("健身教練姓名") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_course" runat="server" Text='<%# "課程名稱："+ Eval("課程名稱") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_date" runat="server" Text='<%# "預約日期：" + Eval("日期","{0:yyyy/MM/dd }") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_time" runat="server" Text='<%# "預約時間：" + Eval("開始時間") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_status" runat="server" Text="預約狀態：教練取消" Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_fee" runat="server" Text='<%# "費用："+ Eval("課程費用","{0:f0}") +"元"%>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_text" runat="server" Text='<%# "備註："+ Eval("備註") %>' Font-Size="16px"></asp:Label><br />
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </div>
    </div>
</asp:Content>

