<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage.master" AutoEventWireup="true" CodeFile="Coach_class.aspx.cs" Inherits="Coach_Coach_class" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand">
        <columns>
            <asp:BoundField DataField="課程編號" HeaderText="課程編號" ReadOnly="True" InsertVisible="False" />
            <asp:BoundField DataField="課程名稱" HeaderText="課程名稱" />

            <asp:TemplateField HeaderText="分類名稱">
                <itemtemplate>
                    <asp:Label ID="lblCourseType" runat="server" Text='<%# Eval("分類名稱") %>'></asp:Label>
                </itemtemplate>
                <edititemtemplate>
                    <asp:DropDownList ID="ddlCourseTypeEdit" runat="server" SelectedValue='<%# Bind("分類編號") %>'>
                    </asp:DropDownList>
                </edititemtemplate>
            </asp:TemplateField>


            <asp:BoundField DataField="課程內容介紹" HeaderText="課程內容介紹" />
            <asp:BoundField DataField="課程時間長度" HeaderText="課程時間長度" />
            <asp:BoundField DataField="上課人數" HeaderText="上課人數" />
            <asp:BoundField DataField="上課地點" HeaderText="上課地點" />
            <asp:BoundField DataField="課程費用" HeaderText="課程費用" />
            <asp:BoundField DataField="所需設備" HeaderText="所需設備" />
            <asp:TemplateField HeaderText="課程圖片">
                <itemtemplate>
                    <asp:Image ID="imgCourse" runat="server" ImageUrl='<%# GetImageUrl(Eval("課程圖片"), 15) %>' Width="100px" />
                </itemtemplate>
                <edititemtemplate>
                    <asp:FileUpload ID="fuCourseImage" runat="server" />
                </edititemtemplate>
            </asp:TemplateField>
           <asp:TemplateField>
            <ItemTemplate>
                <asp:Button ID="btnSelect" runat="server" CommandName="SelectClass" CommandArgument='<%# Eval("課程編號") %>' Text="選擇" CssClass="btn btn-primary" />
            </ItemTemplate>
        </asp:TemplateField>
        </columns>
    </asp:GridView>
    <br />
    <asp:Panel ID="AddCoursePanel" runat="server">
        <h3>新增課程</h3>
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
        <br />
        <asp:Label ID="lblCourseName" runat="server" Text="課程名稱:"></asp:Label>
        <asp:TextBox ID="tbCourseName" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvCourseName" runat="server" ControlToValidate="tbCourseName" ErrorMessage="課程名稱不得為空" ForeColor="Red" Display="Dynamic" ValidationGroup="vg1" />
        <br />
        <br />
        <asp:Label ID="lblCourseType" runat="server" Text="課程類型:"></asp:Label>
        <asp:DropDownList ID="ddlCourseType" runat="server">
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="rfvCourseType" runat="server" ControlToValidate="ddlCourseType" ErrorMessage="課程類型不得為空" ForeColor="Red" Display="Dynamic" InitialValue="" ValidationGroup="vg1" />
        <br />
        <br />
        <asp:Label ID="lblCourseDescription" runat="server" Text="課程內容介紹:"></asp:Label>
        <asp:TextBox ID="tbCourseDescription" runat="server" TextMode="MultiLine"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvCourseDescription" runat="server" ControlToValidate="tbCourseDescription" ErrorMessage="課程內容介紹不得為空" ForeColor="Red" Display="Dynamic" ValidationGroup="vg1" />
        <br />
        <br />
        <asp:Label ID="lblCourseDuration" runat="server" Text="課程時間長度:"></asp:Label>
        <asp:TextBox ID="tbCourseDuration" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvCourseDuration" runat="server" ControlToValidate="tbCourseDuration" ErrorMessage="課程時間長度不得為空" ForeColor="Red" Display="Dynamic" ValidationGroup="vg1" />
        <br />
        <br />
        <asp:Label ID="lblClassSize" runat="server" Text="上課人數:"></asp:Label>

        <asp:RadioButtonList ID="rblClassSize" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblClassSize_SelectedIndexChanged" AutoPostBack="True">
            <asp:ListItem Value="1" Selected="True">一對一課程</asp:ListItem>
            <asp:ListItem Value="2">團體課程：</asp:ListItem>
        </asp:RadioButtonList>

        <asp:TextBox ID="tbClassSize" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvClassSize" runat="server" ControlToValidate="tbClassSize" ErrorMessage="上課人數不得為空" ForeColor="Red" Display="Dynamic" />
        <asp:CustomValidator ID="cvClassSize" runat="server" ControlToValidate="tbClassSize" ErrorMessage="團體課程人數必須大於 1" OnServerValidate="cvClassSize_ServerValidate" ForeColor="Red" Display="Dynamic" ValidationGroup="vg1" />

        <br />
        <br />
        <asp:Label ID="lblClassLocation" runat="server" Text="上課地點:"></asp:Label>

        <asp:RadioButtonList ID="rblLocation" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdLocation_SelectedIndexChanged" AutoPostBack="True">
            <asp:ListItem Value="2" Selected="True">到府(客戶指定地點)</asp:ListItem>
            <asp:ListItem Value="3">其他(教練指定地點)：</asp:ListItem>
        </asp:RadioButtonList>
        <asp:TextBox ID="tbClassLocation" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvClassLocation" runat="server" ControlToValidate="tbClassLocation" ErrorMessage="上課地點不得為空" ForeColor="Red" Display="Dynamic" />

        <br />
        <br />
        <asp:Label ID="lblCourseFee" runat="server" Text="課程費用:"></asp:Label>
        <asp:TextBox ID="tbCourseFee" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvCourseFee" runat="server" ControlToValidate="tbCourseFee" ErrorMessage="課程費用不得為空" ForeColor="Red" Display="Dynamic" ValidationGroup="vg1" />
        <br />
        <br />
        <asp:Label ID="lblRequiredEquipment" runat="server" Text="所需設備:"></asp:Label>
        <asp:TextBox ID="tbRequiredEquipment" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Label ID="lblCourseImage" runat="server" Text="課程圖片:"></asp:Label>
        <asp:FileUpload ID="fuCourseImage" runat="server" />
        <br />
        <br />
        <asp:Button ID="btnAddCourse" runat="server" Text="新增課程" OnClick="btnAddCourse_Click" ValidationGroup="vg1" />
    </asp:Panel>

        <asp:Panel ID="ClassPanel" runat="server" Visible="false" CssClass="modal" tabindex="-1" role="dialog" aria-labelledby="ClassModalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="ClassModalLabel">詳細資訊</h5>
            </div>
            <div class="modal-body">
                <asp:Label runat="server">課程名稱：</asp:Label>
                <asp:TextBox ID="detailName" runat="server"></asp:TextBox>
                <br/>
                <asp:Image ID="img_Course" runat="server" Height="250px" Width="300px" ImageUrl='<%# GetImageUrl(Eval("課程圖片"),15) %>' />

                <asp:FileUpload ID="FileUpload1" runat="server" />

                <br/>
                <asp:Label runat="server">課程類型：</asp:Label>
                <asp:DropDownList ID="detailType" runat="server">
                </asp:DropDownList>
                <br/>
                <asp:Label runat="server">課程時間：</asp:Label>
                <asp:TextBox ID="detailTime" runat="server"></asp:TextBox>
                <span>分鐘</span>
                <br/>
                <asp:Label runat="server">上課人數：</asp:Label>
                <asp:TextBox ID="detailpeople" runat="server"></asp:TextBox>
                <span>人</span>
                <br/>
                <asp:Label runat="server">課程費用：</asp:Label>
                <asp:TextBox ID="detailmoney" runat="server"></asp:TextBox>
                <br/>
                <asp:Label runat="server">所需設備：</asp:Label>
                <asp:TextBox ID="detailitem" runat="server"></asp:TextBox>
                <br/>
                <asp:Label runat="server">上課地點：</asp:Label>
                <asp:TextBox ID="detailplace" runat="server"></asp:TextBox>
                <br/>
                <asp:Label runat="server">課程介紹：</asp:Label>
                <asp:TextBox ID="detailintro" runat="server"></asp:TextBox>
                <br/>
        
            <div class="modal-footer">
                <asp:Button ID="Class_save" runat="server" Text="更新" OnClick="Class_save_Click" />
                <asp:Button ID="Class_cancel" runat="server" Text="取消" OnClick="Class_cancel_Click" />
                <asp:Button ID="Class_delete" runat="server" Text="刪除" OnClick="Class_delete_Click" />
            </div>
        </div>
    </div>
</div>
</asp:Panel>
</asp:Content>
