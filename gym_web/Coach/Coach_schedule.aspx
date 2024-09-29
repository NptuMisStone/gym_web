<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="Coach_schedule.aspx.cs" Inherits="Coach_Coach_schedule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/tempusdominus-bootstrap-4/5.39.0/css/tempusdominus-bootstrap-4.min.css" />
<script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <!-- Page Header Start -->
<div class="container-fluid page-header mb-5">
    <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 400px">
        <h4 class="display-4 mb-3 mt-0 mt-lg-5 text-white text-uppercase font-weight-bold">課程安排</h4>
    </div>
</div>

<script type="text/javascript">
    $(function () {
        $('#startTimePicker').datetimepicker({
            format: 'A hh:mm',
            stepping: 10
        });
        $('#startTimePicker').on('show.datetimepicker', function () {
            if ($('#datetimepicker-confirm-btn').length === 0) {
                $('.bootstrap-datetimepicker-widget').append('<button id="datetimepicker-confirm-btn" type="button" class="btn btn-primary">確認</button>');
            }
        });
        $(document).on('click', '#datetimepicker-confirm-btn', function () {
            __doPostBack('<%= tbCourseStartTime.ClientID %>', '');
        $('#startTimePicker').datetimepicker('hide');
    });
    });
    $(function () {
        $('#detailstarttimePicker').datetimepicker({
            format: 'A hh:mm',
            stepping: 10
        });
        $('#detailstarttimePicker').on('show.datetimepicker', function () {
            if ($('#datetimepicker-confirm-btn-detail').length === 0) {
                $('.bootstrap-datetimepicker-widget').append('<button id="datetimepicker-confirm-btn-detail" type="button" class="btn btn-primary">確認</button>');
            }
        });
        $(document).on('click', '#datetimepicker-confirm-btn-detail', function () {
            __doPostBack('<%= detailstarttime.ClientID %>', '');
        $('#detailstarttimePicker').datetimepicker('hide');
    });
    });

    window.onload = function () {
    var today = new Date();
    var yyyy = today.getFullYear();
    var mm = today.getMonth() + 1; // 月份是從0開始的，所以加1
    var dd = today.getDate();

    if (mm < 10) {
        mm = '0' + mm;
    }

    if (dd < 10) {
        dd = '0' + dd;
    }

    today = yyyy + '-' + mm + '-' + dd;
        document.getElementById('<%= txtDate.ClientID %>').setAttribute('min', today);
        document.getElementById('<%= detaildate.ClientID %>').setAttribute('min', today);
}
</script>
<style>
    .course-box {
    background-color: #f0f8ff;
    border: 1px solid #ccc;
    padding: 10px;
    margin-bottom: 10px;
    text-align:center;
    border-radius: 5px;
    box-shadow: 1px 1px 5px rgba(0, 0, 0, 0.1);
    }
    .course-box h4 {
        margin: 5px 0;
        font-size: 18px;
    }

    .course-box p {
        margin: 2px 0;
        font-size: 14px;
    }
    thead th {
        border: 1px solid #000;
        margin:auto;
        text-align: center;
        padding: 8px;
    }
    .course-table{
        width: 50%;
        table-layout: fixed;
        border-collapse: collapse;
    }
    tbody td {
        vertical-align: top; 
    }
    .course-link {
    display: block;
    text-decoration: none;
    color: inherit;
}

</style>       
    <asp:Gridview ID="gv_course" runat="server" AutoGenerateColumns="False" OnRowCreated="gv_course_RowCreated"  OnRowCommand="GetCourseInfo" >
        <Columns>
            <asp:BoundField DataField="課程編號" HeaderText="課程編號"  />
            <asp:BoundField DataField="課程名稱" HeaderText="課程名稱" />
            <asp:BoundField DataField="分類編號" HeaderText="分類編號"/>
            <asp:BoundField DataField="課程時間長度" HeaderText="課程時間長度"/>
            <asp:BoundField DataField="上課人數" HeaderText="上課人數" />
            <asp:BoundField DataField="地點名稱" HeaderText="地點名稱" />
            <asp:BoundField DataField="課程費用" HeaderText="課程費用" />
            <asp:TemplateField HeaderText="課程圖片">
                <ItemTemplate>
                    <asp:Image ID="imgCourse" runat="server" ImageUrl='<%# GetImageUrl(Eval("課程圖片"), 15) %>' Width="100px" />
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:ButtonField ButtonType="Button" CommandName="Select" Text="選擇" />
        </Columns>
    </asp:GridView>
    <br>
    <br>
    <asp:Panel ID="arrange_date" runat="server" Visible="false">
        <h3>安排課表</h3>
        <asp:Label ID="lb1CourseName" runat="server" Text="課程名稱："></asp:Label>
        <asp:TextBox ID="tbCourseName" runat="server" Text="" Enabled="false"></asp:TextBox>
        <br />
        <asp:Label ID="lblCourseDate" runat="server" Text="日期:"></asp:Label>
        <asp:Calendar ID="Calendar1" runat="server" OnSelectionChanged="Calendar1_SelectionChanged" OnDayRender="Calendar1_DayRender" >
        </asp:Calendar>
        <asp:Label ID="SelectedDatesLabel" runat="server" Text="選擇的日期：" />
        <br />
        <br />
        <div class="input-group date" id="startTimePicker" data-target-input="nearest">
            <asp:Label ID="lblCourseStartTime" runat="server" Text="開始時間:"></asp:Label>            
            <asp:TextBox ID="tbCourseStartTime" runat="server" CssClass="custom-time-input" AutoPostBack="true" OnTextChanged="tbCourseStartTime_TextChanged"  ></asp:TextBox>
            <div class="input-group-append" data-target="#startTimePicker" data-toggle="datetimepicker">
                <div class="input-group-text"><i class="fa fa-clock"></i></div>
            </div>
            <asp:RequiredFieldValidator ID="rfvCourseStartTime" runat="server" ControlToValidate="tbCourseStartTime" ErrorMessage="開始時間不得為空" ForeColor="Red" Display="Dynamic"  />
        </div>
        <br />
        <asp:Label ID="lblCourseEndTime" runat="server" Text="結束時間:"></asp:Label>
        <asp:TextBox ID="tbCourseEndTime" runat="server" Enabled="false"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="btnAddSchedule" runat="server" Text="新增課表" OnClick="btnAddSchedule_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" />
    </asp:Panel>
    <br />
    <asp:TextBox ID="txtDate" runat="server" TextMode="Date"></asp:TextBox>
    <asp:Button ID="btnGetSchedule" runat="server" Text="查找课表" OnClick="btnGetSchedule_Click" />
    <asp:Label ID="lb1date" runat="server"></asp:Label>
    <table class="course-table">
        <thead>
            <tr>
                <th>星期一</th>
                <th>星期二</th>
                <th>星期三</th>
                <th>星期四</th>
                <th>星期五</th>
                <th>星期六</th>
                <th>星期日</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>
                    <asp:Repeater ID="RepeaterMonday" runat="server" OnItemCommand="RepeaterWeekInfo_ItemCommand">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkCourse" runat="server" CommandName="ShowId" CommandArgument='<%# Eval("課表編號") %>' CssClass="course-link">
                                <div class="course-box" >
                                    <p style="display:none;"><%# Eval("課表編號") %></p>
                                    <p><%# Eval("開始時間") %> ~ <%# Eval("結束時間") %></p>
                                    <h4><%# Eval("課程名稱") %></h4>
                                    <p><%# Eval("日期","{0:yyyy/MM/dd}") %></p>
                                </div>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
                <td>
                    <asp:Repeater ID="RepeaterTuesday" runat="server" OnItemCommand="RepeaterWeekInfo_ItemCommand">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkCourse" runat="server" CommandName="ShowId" CommandArgument='<%# Eval("課表編號") %>' CssClass="course-link">
                                <div class="course-box" >
                                    <p style="display:none;"><%# Eval("課表編號") %></p>
                                    <p><%# Eval("開始時間") %> ~ <%# Eval("結束時間") %></p>
                                    <h4><%# Eval("課程名稱") %></h4>
                                    <p><%# Eval("日期","{0:yyyy/MM/dd}") %></p>
                                </div>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
                <td>
                    <asp:Repeater ID="RepeaterWednesday" runat="server" OnItemCommand="RepeaterWeekInfo_ItemCommand">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkCourse" runat="server" CommandName="ShowId" CommandArgument='<%# Eval("課表編號") %>' CssClass="course-link">
                                <div class="course-box" >
                                    <p style="display:none;"><%# Eval("課表編號") %></p>
                                    <p><%# Eval("開始時間") %> ~ <%# Eval("結束時間") %></p>
                                    <h4><%# Eval("課程名稱") %></h4>
                                    <p><%# Eval("日期","{0:yyyy/MM/dd}") %></p>
                                </div>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
                <td>
                    <asp:Repeater ID="RepeaterThursday" runat="server" OnItemCommand="RepeaterWeekInfo_ItemCommand">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkCourse" runat="server" CommandName="ShowId" CommandArgument='<%# Eval("課表編號") %>' CssClass="course-link">
                                <div class="course-box" >
                                    <p style="display:none;"><%# Eval("課表編號") %></p>
                                    <p><%# Eval("開始時間") %> ~ <%# Eval("結束時間") %></p>
                                    <h4><%# Eval("課程名稱") %></h4>
                                    <p><%# Eval("日期","{0:yyyy/MM/dd}") %></p>
                                </div>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
                <td>
                    <asp:Repeater ID="RepeaterFriday" runat="server" OnItemCommand="RepeaterWeekInfo_ItemCommand">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkCourse" runat="server" CommandName="ShowId" CommandArgument='<%# Eval("課表編號") %>' CssClass="course-link">
                                <div class="course-box" >
                                    <p style="display:none;"><%# Eval("課表編號") %></p>
                                    <p><%# Eval("開始時間") %> ~ <%# Eval("結束時間") %></p>
                                    <h4><%# Eval("課程名稱") %></h4>
                                    <p><%# Eval("日期","{0:yyyy/MM/dd}") %></p>
                                </div>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
                <td>
                    <asp:Repeater ID="RepeaterSaturday" runat="server" OnItemCommand="RepeaterWeekInfo_ItemCommand">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkCourse" runat="server" CommandName="ShowId" CommandArgument='<%# Eval("課表編號") %>' CssClass="course-link">
                                <div class="course-box" >
                                    <p style="display:none;"><%# Eval("課表編號") %></p>
                                    <p><%# Eval("開始時間") %> ~ <%# Eval("結束時間") %></p>
                                    <h4><%# Eval("課程名稱") %></h4>
                                    <p><%# Eval("日期","{0:yyyy/MM/dd}") %></p>
                                </div>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
                <td>
                    <asp:Repeater ID="RepeaterSunday" runat="server" OnItemCommand="RepeaterWeekInfo_ItemCommand">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkCourse" runat="server" CommandName="ShowId" CommandArgument='<%# Eval("課表編號") %>' CssClass="course-link">
                                <div class="course-box" >
                                    <p style="display:none;"><%# Eval("課表編號") %></p>
                                    <p><%# Eval("開始時間") %> ~ <%# Eval("結束時間") %></p>
                                    <h4><%# Eval("課程名稱") %></h4>
                                    <p><%# Eval("日期","{0:yyyy/MM/dd}") %></p>
                                </div>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
        </tbody>
    </table>
    <asp:Label ID="lblMessage" runat="server" Text="" Font-Bold="True" Font-Size="50px"></asp:Label>
    <asp:Panel ID="SchedulePanel" runat="server" Visible="false" CssClass="modal" tabindex="-1" role="dialog" aria-labelledby="scheduleModalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="scheduleModalLabel">詳細資訊</h5>
            </div>
            <div class="modal-body">
                <asp:Label runat="server">日期：</asp:Label>
                <asp:Textbox ID="detaildate" runat="server"  TextMode="Date"  Text='<%#  Eval("日期") %>'></asp:Textbox>
                <br/>
                <div class="input-group date" id="detailstarttimePicker" data-target-input="nearest">
                    <asp:Label runat="server">開始時間：</asp:Label>
                    <asp:Textbox ID="detailstarttime" runat="server" CssClass="custom-time-input" AutoPostBack="true" OnTextChanged="detailstarttime_TextChanged" Text='<%#  Eval("開始時間") %>'></asp:Textbox>
                    <div class="input-group-append" data-target="#detailstarttimePicker" data-toggle="datetimepicker">
                        <div class="input-group-text"><i class="fa fa-clock"></i></div>
                    </div>
                </div>
                <asp:Label runat="server">結束時間：</asp:Label>
                <asp:Textbox ID="detailendtime" runat="server"  Text='<%#  Eval("結束時間") %>'></asp:Textbox>
                <br/>
                <asp:Label runat="server">課程名稱：</asp:Label>
                <asp:DropDownList ID="coursedata" runat="server"  AutoPostBack="True" OnSelectedIndexChanged="coursedata_SelectedIndexChanged">
                </asp:DropDownList>
                <br/>
                <asp:Image ID="img_Course" runat="server" Height="250px" Width="300px"  ImageUrl='<%# GetImageUrl(Eval("課程圖片"),15) %>' />
                <br/>
                <asp:Label runat="server">課程類型：</asp:Label>
                <asp:Label ID="detailcoursetype" runat="server"></asp:Label>
                <br/>
                <asp:Label runat="server">課程時間：</asp:Label>
                <asp:Label ID="detailcoursetime" runat="server" Text='<%#  Eval("課程時間長度") %>'></asp:Label>
                <span>分鐘</span>
                <br/>
                <asp:Label runat="server">上課人數：</asp:Label>
                <asp:Label ID="detailcoursepeople" runat="server" Text='<%#  Eval("上課人數") %>'></asp:Label>
                <span>人</span>
                <br/>
                <asp:Label runat="server">課程費用：</asp:Label>
                <asp:Label ID="detailcoursemoney" runat="server" Text='<%#  Eval("課程費用") %>'></asp:Label>
                <br/>
                <asp:Label runat="server">所需設備：</asp:Label>
                <asp:Label ID="detailcourseitem" runat="server" Text='<%#  Eval("所需設備") %>'></asp:Label>
                <br/>
                <asp:Label runat="server">地點名稱：</asp:Label>
                <asp:Label ID="detailcourseplace" runat="server" Text='<%#  Eval("地點名稱") %>'></asp:Label>
                <br/>
                <asp:Label runat="server">課程介紹：</asp:Label>
                <asp:Label ID="detailcourseintro" runat="server" Text='<%#  Eval("課程內容介紹") %>'></asp:Label>
                <br/>
        
            <div class="modal-footer">
                <asp:Button ID="Schedule_save" runat="server" Text="更新" OnClick="Schedule_save_Click" />
                <asp:Button ID="Schedule_cancel" runat="server" Text="取消" OnClick="Schedule_cancel_Click" />
                <asp:Button ID="Schedule_delete" runat="server" Text="刪除" OnClick="Schedule_delete_Click" />
            </div>
        </div>
    </div>
</div>
</asp:Panel>
</asp:Content>

