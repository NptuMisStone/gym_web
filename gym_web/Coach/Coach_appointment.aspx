<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="Coach_appointment.aspx.cs" Inherits="Coach_Coach_appointment" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Page Header Start -->
<div class="container-fluid page-header mb-5">
    <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 130px">
    </div>
</div>
<!-- Page Header End -->
    <style>
        .course-container {
            background-color: #f0f8ff;
            border: 1px solid #ccc;
            padding: 10px;
            margin-bottom: 10px;
            border-radius: 5px;
            box-shadow: 1px 1px 5px rgba(0, 0, 0, 0.1);
            display: flex;
            align-items: center; /* 垂直居中 */
            border-bottom: 1px solid #ddd;
            padding: 10px;
        }
        .enroll-button {
            padding: 10px 10px;
            font-size: medium;
            margin-left: auto; /* 将按钮移到最右侧 */
        }
        .modal-dialog {
            max-width: 80%; /* 自定义宽度 */
        }
    </style>
    <div class="container mt-3">
    <ul class="nav nav-tabs" id="myTab" >
        <li>
            <a class="nav-link active" id="tab1-tab" data-toggle="tab" href="#tab1"  aria-controls="tab1" aria-selected="true">本日課程</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" id="tab2-tab" data-toggle="tab" href="#tab2"  aria-controls="tab2" aria-selected="false">未來課程</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" id="tab3-tab" data-toggle="tab" href="#tab3"  aria-controls="tab3" aria-selected="false">過去課程</a>
        </li>
    </ul>
    <div class="tab-content" id="myTabContent">
        <div class="tab-pane fade show active" id="tab1"  aria-labelledby="tab1-tab">
                <asp:Repeater ID="TodayRP" runat="server" OnItemCommand="TodayRP_ItemCommand">
                    <ItemTemplate>
                        <div class="course-container">
                            <div class="course-box">
                                <span>日期：</span>
                                <asp:Label ID="TD_date" runat="server" Text='<%#Eval("日期","{0:yyyy/MM/dd}") %>' Font-Size="Medium"></asp:Label><br />
                                <span>時段</span>
                                <asp:Label ID="TD_ST_time" runat="server" Text='<%#  Eval("開始時間") %>' Font-Size="Medium"></asp:Label>
                                <span>~</span>
                                <asp:Label ID="TD_ED_time" runat="server" Text='<%#  Eval("結束時間") %>' Font-Size="Medium"></asp:Label><br />
                                <span>課程名稱：</span>
                                <asp:Label ID="TD_Course_Name" runat="server" Text='<%#  Eval("課程名稱") %>' Font-Size="Medium"></asp:Label><br />
                                <span>課程類型：</span>
                                <asp:Label ID="TD_Course_Type" runat="server" Text='<%#  Eval("分類名稱") %>' Font-Size="Medium"></asp:Label><br />
                                <span>地點：</span>
                                <asp:Label ID="TD_Place" runat="server" Text='<%#  Eval("地點名稱") %>' Font-Size="Medium"></asp:Label><br />
                                <span>預約人數：</span>
                                <asp:Label ID="TD_Last_People" runat="server" Text='<%#  Eval("預約人數") %>' Font-Size="Medium"></asp:Label>
                                <span>/</span>
                                <asp:Label ID="TD_Max_People" runat="server" Text='<%#  Eval("上課人數") %>' Font-Size="Medium"></asp:Label>
                                <span>人</span><br />
                            </div>
                            <asp:Button ID="TD_AP_Chcek" runat="server" Text="查看預約名單" CssClass="enroll-button" CommandName="TD_AP" CommandArgument='<%# Eval("課表編號") %>'  />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        <div class="tab-pane fade" id="tab2"  aria-labelledby="tab2-tab">
                <asp:Repeater ID="FutureRP" runat="server" OnItemCommand="FutureRP_ItemCommand" >
                    <ItemTemplate>
                        <div class="course-container">
                            <div class="course-box" >
                                <span>日期：</span>
                                <asp:Label ID="FT_date" runat="server"  Text='<%#  Eval("日期","{0:yyyy/MM/dd}") %>' Font-Size="Medium"></asp:Label><br />
                                <span>時段</span>
                                <asp:Label ID="FT_ST_time" runat="server"  Text='<%#  Eval("開始時間") %>' Font-Size="Medium"></asp:Label>
                                <span>~</span>
                                <asp:Label ID="FT_ED_time" runat="server"  Text='<%#  Eval("結束時間") %>' Font-Size="Medium"></asp:Label><br />
                                <span>課程名稱：</span>
                                <asp:Label ID="FT_Course_Name" runat="server"  Text='<%#  Eval("課程名稱") %>' Font-Size="Medium"></asp:Label><br />
                                <span>課程類型：</span>
                                <asp:Label ID="FT_Course_Type" runat="server"  Text='<%#  Eval("分類名稱") %>' Font-Size="Medium"></asp:Label><br />
                                <span>地點：</span>
                                <asp:Label ID="FT_Place" runat="server"  Text='<%#  Eval("地點名稱") %>' Font-Size="Medium"></asp:Label><br />
                                <span>預約人數：</span>
                                <asp:Label ID="FT_Last_People" runat="server"  Text='<%#  Eval("預約人數") %>' Font-Size="Medium"></asp:Label>
                                <span>/</span>
                                <asp:Label ID="FT_Max_People" runat="server"  Text='<%#  Eval("上課人數") %>' Font-Size="Medium"></asp:Label>
                                <span>人</span><br />
                            </div>
                        <asp:Button ID="FT_AP_Chcek" runat="server" Text="查看預約名單" CssClass="enroll-button" CommandName="FT_AP" CommandArgument='<%# Eval("課表編號") %>'  />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
        </div>
        <div class="tab-pane fade" id="tab3"  aria-labelledby="tab3-tab">
            <asp:Repeater ID="PastRP" runat="server" OnItemCommand="PastRP_ItemCommand" >
                <ItemTemplate>
                    <div class="course-container">
                        <div class="course-box" >
                            <span>日期：</span>
                            <asp:Label ID="PS_date" runat="server"  Text='<%#  Eval("日期","{0:yyyy/MM/dd}") %>' Font-Size="Medium"></asp:Label><br />
                            <span>時段</span>
                            <asp:Label ID="PS_ST_time" runat="server"  Text='<%#  Eval("開始時間") %>' Font-Size="Medium"></asp:Label>
                            <span>~</span>
                            <asp:Label ID="PS_ED_time" runat="server"  Text='<%#  Eval("結束時間") %>' Font-Size="Medium"></asp:Label><br />
                            <span>課程名稱：</span>
                            <asp:Label ID="PS_Course_Name" runat="server"  Text='<%#  Eval("課程名稱") %>' Font-Size="Medium"></asp:Label><br />
                            <span>課程類型：</span>
                            <asp:Label ID="PS_Course_Type" runat="server"  Text='<%#  Eval("分類名稱") %>' Font-Size="Medium"></asp:Label><br />
                            <span>地點：</span>
                            <asp:Label ID="PS_Place" runat="server"  Text='<%#  Eval("地點名稱") %>' Font-Size="Medium"></asp:Label><br />
                            <span>預約人數：</span>
                            <asp:Label ID="PS_Last_People" runat="server"  Text='<%#  Eval("預約人數") %>' Font-Size="Medium"></asp:Label>
                            <span>/</span>
                            <asp:Label ID="PS_Max_People" runat="server"  Text='<%#  Eval("上課人數") %>' Font-Size="Medium"></asp:Label>
                            <span>人</span><br />
                        </div>
                    <asp:Button ID="PS_AP_Chcek" runat="server" Text="查看預約名單" CssClass="enroll-button" CommandName="PS_AP" CommandArgument='<%# Eval("課表編號") %>'  />
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
    
    <asp:Panel ID="Panel1" runat="server" Visible="false" CssClass="modal" tabindex="-1" role="dialog" aria-labelledby="ModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="ModalLabel">預約名單</h5>
                </div>
                <div class="modal-body">
                    <asp:Label ID="lblNoData" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Gridview ID="AP_Detail" runat="server" AutoGenerateColumns="False" OnRowCommand="AP_Detail_RowCommand"  OnRowCreated="AP_Detail_RowCreated" OnRowCancelingEdit="AP_Detail_RowCancelingEdit" OnRowDataBound="AP_Detail_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="預約編號" HeaderText="預約編號"  /><%--改預約狀態用--%>
                            <asp:BoundField DataField="課表編號" HeaderText="課表編號"  /><%--改預約人數用--%>
                            <asp:BoundField DataField="健身教練編號" HeaderText="健身教練編號"  /><%--改教練次數用--%>
                            <asp:BoundField DataField="預約狀態" HeaderText="預約狀態"  /><%--判斷需不需按鈕用--%>
                            <asp:BoundField DataField="使用者姓名" HeaderText="客戶名稱"  />
                            <asp:BoundField DataField="使用者性別" HeaderText="性別" />
                            <asp:BoundField DataField="使用者電話" HeaderText="電話"/>
                            <asp:BoundField DataField="使用者郵件" HeaderText="信箱"/>
                            <asp:BoundField DataField="備註" HeaderText="備註" />

                            <asp:ButtonField ButtonType="Button" CommandName="Cancel" Text="取消預約"  />
                            <asp:ButtonField ButtonType="Button" CommandName="Finish" Text="完成預約"  />
                        </Columns>
                    </asp:GridView>
        
                <div class="modal-footer">
                </div>
            </div>
        </div>
    </div>
    </asp:Panel>
</asp:Content>

