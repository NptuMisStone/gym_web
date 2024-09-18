<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage.master" AutoEventWireup="true" CodeFile="class_detail.aspx.cs" Inherits="page_class_detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style>
        .repeater-container {
            overflow-y: auto;  /* 橫向 */
        }

        .item {
            
            margin: auto;
            width: 300px;  
            height: 300px;
            line-height:60px;
            background-color: #f0f0f0;
            text-align: center;
            border: 1px solid #ddd;
            
        }
        .btn {
            width:75px;
            height:37.5px;
            border: 2px solid black;
        }
        .imgbtn{
            width:30px;
            height:30px;
        }
    </style>
    <div style="display: flex; flex-direction: column; align-items: center; margin-top: 25px;">
        <asp:Image ID="img_Course" runat="server" Height="400px" Width="600px"  ImageUrl='<%# GetImageUrl(Eval("課程圖片"),15) %>' />
        <br/>
        <asp:Label ID="Course_Name" runat="server"  Text='<%# Eval("課程名稱") %>' Font-Size="25px" Font-Bold="True" ></asp:Label>
    </div>
    <div style="display: flex; flex-direction: column; align-items: center; margin-top: 25px;">
        <div style="text-align: left; width: 100%; max-width: 600px;">
            <img src="images/icon-people.png" height="25px" width="25px" />&nbsp;課程人數：
            <asp:Label ID="Course_People" runat="server"  Text='<%#  Eval("上課人數") %>' Font-Size="Medium"></asp:Label>
            <span>人</span>
            <br><br>
        </div>
        <div style="text-align: left; width: 100%; max-width: 600px;">
            <img src="images/icon-time.png" height="25px" width="25px" />&nbsp;課程時間：
            <asp:Label ID="Course_Time" runat="server"  Text='<%#  Eval("課程時間長度") %>' Font-Size="Medium"></asp:Label>
            <span>分鐘</span>
            <br><br>
        </div>
        <div style="text-align: left; width: 100%; max-width: 600px;">
            <img src="images/icon-location.png" height="25px" width="25px" />&nbsp;課程地點：
            <asp:Label ID="Course_Address" runat="server"  Text='<%#  Eval("上課地點") %>' Font-Size="Medium"></asp:Label><br><br>
        </div>
    </div>
    <hr style="border: 1px solid black; width: 60%; margin: 20px auto;"/>
    
    <div style="display: flex; flex-direction: column; align-items: center; margin-top: 25px;">
        <div style="text-align: left; width: 100%; max-width: 800px;">
                <h3>課程簡介</h3>
                <br>
                <span>課程類型：</span>
                <asp:Label ID="Course_all_Type" runat="server"  Text='<%#  Eval("分類名稱") %>' Font-Size="Medium"></asp:Label>
                <br><br>
                <span>課程人數：</span>
                <asp:Label ID="Course_all_People" runat="server"  Text='<%#  Eval("上課人數") %>' Font-Size="Medium"></asp:Label>
                <span>人</span>
                <br><br>
                <span>課程費用：</span>
                <asp:Label ID="Course_all_Cost" runat="server"  Text='<%#  Eval("課程費用") %>' Font-Size="Medium"></asp:Label>
                <br><br>
                <span>所需設備：</span>
                <asp:Label ID="Course_all_Item" runat="server"  Text='<%#  Eval("所需設備") %>' Font-Size="Medium"></asp:Label>
                <br><br>
                <span>課程介紹：</span>
                <asp:Label ID="Course_all_Intro" runat="server"  Text='<%#  Eval("課程內容介紹") %>' Font-Size="Medium"></asp:Label>
                <br><br>
        </div>
    </div>
    <div style="display: flex; flex-direction: column; align-items: center; margin-top: 25px;" >
        <div style="text-align: left; width: 100%; max-width: 800px;">
            <h3>課程時段</h3>
            <div class="repeater-container">
                <asp:ScriptManager ID="ScriptManager1" runat="server" />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Calendar ID="Course_date_choose" runat="server" OnSelectionChanged="Course_date_choose_SelectionChanged" OnDayRender="Course_date_choose_DayRender"></asp:Calendar>
                        <br />
                        <asp:Repeater ID="TimeRepeater" runat="server" OnItemCommand="TimeRepeater_ItemCommand">
                            <ItemTemplate>
                                <div class="item">
                                    <asp:Label ID="Course_all_date" runat="server"  Text='<%#  Eval("日期 ","{0:yyyy/MM/dd}")  %>' Font-Size="Medium"></asp:Label>
                                    <br />
                                    <asp:Label ID="Course_all_week" runat="server"  Text='<%#  Eval("星期幾") %>' Font-Size="Medium"></asp:Label>
                                    <br />
                                    <asp:Label ID="Course_all_stTime" runat="server"  Text='<%#  Eval("開始時間") %>' Font-Size="Medium"></asp:Label>
                                    <span>~</span>
                                    <asp:Label ID="Course_all_edTime" runat="server"  Text='<%#  Eval("結束時間") %>' Font-Size="Medium"></asp:Label>
                                    <br />
                                    <span>目前人數：</span>
                                    <asp:Label ID="ap_all_people" runat="server"  Text='<%# Eval("預約人數") %>' Font-Size="Medium"></asp:Label>
                                    <span>/</span>
                                    <asp:Label ID="course_all_people" runat="server"  Text='<%#  Eval("上課人數") %>' Font-Size="Medium"></asp:Label>
                                    <br />
                                    <asp:Button ID="Appointment_btn" runat="server" Text="預約" CssClass="btn" CommandName="ap" CommandArgument='<%# Eval("課表編號") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:Label ID="noshow" runat="server"  Text="無時段" Font-Size="100px" Visible="false"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="TimeRepeater" />
                    </Triggers>
                </asp:UpdatePanel>
                
            </div>
        </div>
    </div>
    <div style="display: flex; flex-direction: column; align-items: center; margin-top: 25px;" >
        <div style="text-align: left; width: 100%; max-width: 800px;">
            <h3>課程地點</h3>
            <asp:Label ID="Course_all_Address" runat="server"  Text='<%#  Eval("上課地點") %>' Font-Size="Medium"></asp:Label><br>
            <asp:Label ID="Course_all_mapADD" runat="server"  Text='<%#  Eval("服務地點地址") %>' Font-Size="Medium"></asp:Label>
            <asp:HyperLink runat="server" ID="map" Target="_blank" ><img src="images/icon-location.png" height="25px" width="25px" /></asp:HyperLink><br>
        </div>
    </div>
    <div style="display: flex; flex-direction: column; align-items: center; margin-top: 25px;">
        <div style="text-align: left; width: 100%; max-width: 800px;">
            <h3>授課師資</h3>
            <br>
            <asp:Image ID="Coach_Img" runat="server" Height="200px" Width="200px" CssClass="circular-image"  ImageUrl='<%# GetImageUrl(Eval("健身教練圖片"),15) %>' />
            <br><br>
            <span>姓名：</span>
            <asp:Label ID="Coach_Name" runat="server"  Text='<%#  Eval("健身教練姓名") %>' Font-Size="Medium"></asp:Label>
            <br><br>
            <span>性別：</span>
            <asp:Label ID="Coach_Gender" runat="server"  Text='<%#  Eval("健身教練性別") %>' Font-Size="Medium"></asp:Label>
            <br><br>
            <span>電話：</span>
            <asp:Label ID="Coach_Phone" runat="server"  Text='<%#  Eval("健身教練電話") %>' Font-Size="Medium"></asp:Label>
            <br><br>
            <span>郵件：</span>
            <asp:Label ID="Coach_Mail" runat="server"  Text='<%#  Eval("健身教練郵件") %>' Font-Size="Medium"></asp:Label>
            <br><br>
            <span>個人介紹：</span>
            <asp:Label ID="Coach_Intro" runat="server"  Text='<%#  Eval("健身教練介紹") %>' Font-Size="Medium"></asp:Label>
            <br><br>
        </div>
    </div>
    
</asp:Content>

