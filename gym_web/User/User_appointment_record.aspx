<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="User_appointment_record.aspx.cs" Inherits="User_User_appointment_record" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style>
        .column {
            display: flex;
            justify-content: center;
        }
        
        

        .record-box {
            width:300px;
            background-color: #fff;
            border-radius: 10px;
            border: 2px solid #ddd;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); /* 添加陰影效果 */
            padding: 20px;
            box-sizing: border-box;
            display: flex;
            flex-direction: column;
            align-items: flex-start;
        }

        .record-box label {
            font-size: 14px;
            margin-bottom: 5px;
        }

        .record-box button {
            align-self: center; /* 按鈕置中 */
            margin-top: 10px;
            padding: 8px 12px;
            font-size: 14px;
            border: none;
            border-radius: 8px;
            color: white;
            background-color: #475766;
            cursor: pointer;
            transition: background-color 0.3s;
        }

        .record-box button:hover {
            background-color: #333;
        }
        /* 外層容器，讓按鈕水平排列並保持間距 */
        .button-container123 {
            display: flex;
            justify-content: center;
            gap: 20px; /* 按鈕間距 */
            padding: 20px;
        }
        /* 通用按鈕樣式 */
        .btn123 {
            border: 2px solid transparent;
            border-radius: 8px;
            padding: 12px 20px; /* 按鈕內部間距 */
            font-size: 18px; /* 增加文字大小 */
            cursor: pointer;
            background-color: #f8f9fa;
            color: #000;
            font-weight: normal;
            transition: all 0.3s ease;
            width: 150px; /* 固定按鈕寬度 */
            height: 50px; /* 固定按鈕高度 */
            text-align: center; /* 文字置中 */
            display: flex; /* 使用 flex 排列 */
            align-items: center; /* 讓文字垂直置中 */
            justify-content: center; /* 讓文字水平置中 */
        }

        /* 預約中樣式 */
        .btn123-pending {
            border:2px solid;
            border-color: #475766;
            color: #5a5a5a;
        }

        /* 已完成樣式 */
        .btn123-completed {
            border:2px solid;
            border-color: #86B817;
            color: #4CAF50;
        }

        /* 已取消樣式 */
        .btn123-cancelled {
            border:2px solid;
            border-color: #B6B6B6;
            color: #cccccc;
        }

        /* 逾時樣式 */
        .btn123-overdue {
            border:2px solid;
            border-color: #CF808B;
            color: #f44336;
        }

        /* 滑鼠懸停效果 */
        .btn123:hover {
            color: #fff; /* 字體變白 */
            font-weight: bold; /* 字體加粗 */
        }

        .btn123-pending:hover {
            background-color: #5a5a5a;
        }

        .btn123-completed:hover {
            background-color: #4CAF50;
        }

        .btn123-cancelled:hover {
            background-color: #cccccc;
        }

        .btn123-overdue:hover {
            background-color: #f44336;
        }
        .btn123-active {
            background-color: #007bff; /* 激活時的背景顏色 */
            color: #fff; /* 激活時的文字顏色 */
            font-weight: bold; /* 激活時的文字加粗 */
            border: 2px solid #0056b3; /* 激活時的邊框顏色 */
        }

    </style>
        <!-- Page Header Start -->
    <div class="container-fluid page-header mb-5">
        <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 400px">
            <h4 class="display-4 mb-3 mt-0 mt-lg-5 text-white text-uppercase font-weight-bold">使用者預約紀錄</h4>
        </div>
    </div>

    <div class="button-container123">
        <asp:Button ID="btnPending" runat="server" Text="預約中" CssClass="btn123 btn123-pending" OnClick="btnPending_Click" />
        <asp:Button ID="btnCompleted" runat="server" Text="已完成" CssClass="btn123 btn123-completed" OnClick="btnCompleted_Click" />
        <asp:Button ID="btnCancelled" runat="server" Text="已取消" CssClass="btn123 btn123-cancelled" OnClick="btnCancelled_Click" />
        <asp:Button ID="btnOverdue" runat="server" Text="逾時" CssClass="btn123 btn123-overdue" OnClick="btnOverdue_Click" />
    </div>


        <div class="column">
            <asp:DataList ID="dl_inProgress" runat="server" OnItemCommand="dl_inProgress_ItemCommand" RepeatColumns="3">
                <ItemTemplate>
                    <div class="record-box">
                        <asp:Label ID="lb_c_name" runat="server" Text='<%# "健身教練：" + Eval("健身教練姓名") %>'></asp:Label>
                        <asp:Label ID="lb_course" runat="server" Text='<%# "課程名稱：" + Eval("課程名稱") %>'></asp:Label>
                        <asp:Label ID="lb_ap_date" runat="server" Text='<%# "預約日期：" + Eval("日期", "{0:yyyy/MM/dd}") %>'></asp:Label>
                        <asp:Label ID="lb_ap_time" runat="server" Text='<%# "預約時間：" + Eval("開始時間") %>'></asp:Label>
                        <asp:Label ID="lb_ap_status" runat="server" Text="預約狀態：預約中" Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_fee" runat="server" Text='<%# "費用：" + Eval("課程費用", "{0:f0}") + "元" %>'></asp:Label>
                        <asp:Label ID="lb_ap_text" runat="server" Text='<%# "備註：" + Eval("備註") %>'></asp:Label><br />
                        <asp:Button ID="btm_cancel_appointment" runat="server" CommandName="cancel" CommandArgument='<%# Eval("預約編號") %>' Text="取消預約"
                            CssClass="btn123 btn123-pending" Visible='<%# Eval("預約狀態").ToString() == "1" ? true : false %>' />
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </div>
        <div class="column" >
            <asp:DataList ID="dl_completed" runat="server" RepeatColumns="3" OnItemCommand="dl_completed_ItemCommand" >
                <ItemTemplate>
                    <div class="record-box">
                        <asp:Label ID="lb_c_name" runat="server" Text='<%# "健身教練："+ Eval("健身教練姓名") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_course" runat="server" Text='<%# "課程名稱："+ Eval("課程名稱") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_date" runat="server" Text='<%# "預約日期：" + Eval("日期","{0:yyyy/MM/dd }") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_time" runat="server" Text='<%# "預約時間：" + Eval("開始時間") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_status" runat="server" Text="預約狀態：已完成" Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_fee" runat="server" Text='<%# "費用："+ Eval("課程費用","{0:f0}") +"元"%>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_text" runat="server" Text='<%# "備註："+ Eval("備註") %>' Font-Size="16px"></asp:Label><br />
                        <asp:Button ID="btn_comment" runat="server" CommandName="comment" Text="評論"  Visible='<%# !HasCommented(Eval("預約編號")) && Eval("預約狀態").ToString() == "2" %>' CommandArgument='<%# Eval("預約編號") %>' Width="180px" CssClass="btn123 btn123-completed" />
                        <asp:Label ID="Label1" runat="server" Text="你已經評論過了" Visible='<%# HasCommented(Eval("預約編號")) && Eval("預約狀態").ToString() == "2"%>' Font-Size="16px"></asp:Label>
                        <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Height="50px" Width="250px" Visible="false"></asp:TextBox>
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </div>
        <div class="column">
            <asp:DataList ID="dl_cancelled" runat="server"  RepeatColumns="3">
                <ItemTemplate>
                   <div class="record-box">
                        <asp:Label ID="lb_c_name" runat="server" Text='<%# "健身教練："+ Eval("健身教練姓名") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_course" runat="server" Text='<%# "課程名稱："+ Eval("課程名稱") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_date" runat="server" Text='<%# "預約日期：" + Eval("日期","{0:yyyy/MM/dd }") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_time" runat="server" Text='<%# "預約時間：" + Eval("開始時間") %>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_status" runat="server" Text='<%# Eval("預約狀態").ToString() == "3" ? "預約狀態：用戶取消" : Eval("預約狀態").ToString() == "5" ? "預約狀態：教練取消" : "預約狀態：其他" %>'></asp:Label>
                        <asp:Label ID="lb_ap_fee" runat="server" Text='<%# "費用："+ Eval("課程費用","{0:f0}") +"元"%>' Font-Size="16px"></asp:Label>
                        <asp:Label ID="lb_ap_text" runat="server" Text='<%# "備註："+ Eval("備註") %>' Font-Size="16px"></asp:Label><br />
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </div>
        <div class="column" >
            <asp:DataList ID="dl_overtime" runat="server" RepeatColumns="3" >
                <ItemTemplate>
                    <div class="record-box">
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
</asp:Content>

