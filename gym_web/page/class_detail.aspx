<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage.master" AutoEventWireup="true" CodeFile="class_detail.aspx.cs" Inherits="page_class_detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.3/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>

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
            <img src="images/icon-schedule.png" height="25px" width="25px" />&nbsp;課程時段：
            <asp:Label ID="Course_Schedule1" runat="server"  Text='<%#  Eval("日期 ","{0:yyyy/MM/dd}")  %>' Font-Size="Medium"></asp:Label>
            <asp:Label ID="Course_Schedule4" runat="server"  Text='<%#  Eval("星期幾")  %>' Font-Size="Medium"></asp:Label>
            <span>(</span>
            <asp:Label ID="Course_Schedule2" runat="server"  Text='<%#  Eval("開始時間")  %>' Font-Size="Medium"></asp:Label>
            <span>~</span>
            <asp:Label ID="Course_Schedule3" runat="server"  Text='<%#  Eval("結束時間") %>' Font-Size="Medium"></asp:Label>
            <span>)</span>
            <br><br>
        </div>
        <div style="text-align: left; width: 100%; max-width: 600px;">
            <img src="images/icon-location.png" height="25px" width="25px" />&nbsp;課程地點：
            <asp:Label ID="Course_Address" runat="server"  Text='<%#  Eval("上課地點") %>' Font-Size="Medium"></asp:Label><br><br>
        </div>
    </div>
    <hr style="border: 1px solid black; width: 60%; margin: 20px auto;"/>
    <div class="container mt-3">
        <ul class="nav nav-tabs" id="myTab" >
            <li>
                <a class="nav-link active" id="tab1-tab" data-toggle="tab" href="#tab1"  aria-controls="tab1" aria-selected="true">課程簡介</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" id="tab2-tab" data-toggle="tab" href="#tab2"  aria-controls="tab2" aria-selected="false">課程地點</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" id="tab3-tab" data-toggle="tab" href="#tab3"  aria-controls="tab3" aria-selected="false">授課師資</a>
            </li>
        </ul>
        <div class="tab-content" id="myTabContent">
            <div class="tab-pane fade show active" id="tab1"  aria-labelledby="tab1-tab">
                <!-- 課程簡介的內容 -->
                <br>
                <span>課程類型：</span>
                <asp:Label ID="Course_all_Type" runat="server"  Text='<%#  Eval("課程類型") %>' Font-Size="Medium"></asp:Label>
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
            <div class="tab-pane fade" id="tab2"  aria-labelledby="tab2-tab">
                <!-- 課程地點的內容 -->
                <br>
                <%--可能到時候放個google-map，但需申請api--%>
                <asp:Label ID="Course_all_Address" runat="server"  Text='<%#  Eval("上課地點") %>' Font-Size="Medium"></asp:Label><br><br>
                
            </div>
            <div class="tab-pane fade" id="tab3"  aria-labelledby="tab3-tab">
                <!-- 授課師資的內容 -->
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
    </div>
    

</asp:Content>

