<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage.master" AutoEventWireup="true" CodeFile="class.aspx.cs" Inherits="page_class"   %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div style="display: flex; flex-direction: column; align-items: center; margin-top: 25px;">
        <h1>找課程</h1>
        <hr style="border: 0.5px solid black; width: 80%; margin-bottom: 15px;"/>
    </div>
    <div style="display: flex; justify-content: center; flex-wrap: wrap;" >
                <asp:Repeater ID="rp_class" runat="server" OnItemCommand="rp_class_ItemCommand">
            <ItemTemplate>
                <%# (Container.ItemIndex + 1) % 4 == 1 ? "<div class='row horizontal-row'>" : "" %>
                <div class="card repeater-de" style="display: inline-block; width: 200px; height: 300px; text-align: center; margin-bottom: 5px;">
                    <asp:Image ID="Image1" runat="server"  ImageUrl='<%# GetImageUrl(Eval("課程圖片"),15) %>' Width="150px"  /><br>
                    <asp:Label ID="Label1" runat="server" CssClass="mt-4" Text='<%# Eval("課程名稱") %>' Font-Size="Larger" Font-Bold="True"></asp:Label><br>
                    <asp:Label ID="Label4" runat="server" CssClass="mt-4" Text='<%# Eval("健身教練姓名") %>'></asp:Label><br>
                    <asp:Label ID="Label2" runat="server" CssClass="mb-0" Text='<%# "人數：" +  Eval("上課人數") %>' Font-Size="Small"></asp:Label><br />
                    <asp:Label ID="Label3" runat="server" CssClass="mb-0" Text='<%# "$" + Convert.ToDouble(Eval("課程費用")).ToString("F0") %>' Font-Size="Small"></asp:Label><br />
                                                                                        <%--↑去除小數點--%>
                    <asp:Label ID="Label5" runat="server"  Text='<%# "時段：" +  Eval("日期 ","{0:yyyy/MM/dd}")  %>' Font-Size="Small"></asp:Label><br>
                    <asp:Label ID="Label6" runat="server"  Text='<%# "(" + Eval("開始時間") %>' Font-Size="Small"></asp:Label>
                    <asp:Label ID="Label7" runat="server"  Text='<%#  "~" + Eval("結束時間") +")"  %>' Font-Size="Small"></asp:Label><br>                                                              
                    <asp:Button ID="Button2" runat="server" class="btn btn-primary rounded-pill py-2 px-4  top-0 end-0 me-2" Text="查看詳細資訊" CommandName="see_detail" CommandArgument='<%# Eval("課表編號") %>' />&nbsp;
                </div>
                <br>
                <%# (Container.ItemIndex + 1) % 4 == 0 || Container.ItemIndex == rp_class.Items.Count - 1 ? "</div>" : "" %>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    

    

</asp:Content>

