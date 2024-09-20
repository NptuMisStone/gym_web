<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage.master" AutoEventWireup="true" CodeFile="coach_detail.aspx.cs" Inherits="page_coach_detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <center>
    <table style="width: 1000px; margin-top: 20px;">
        <tr>
            <td rowspan="5" style="width: 212px; text-align: center;">
                <asp:Image ID="img_de" runat="server" Height="150px" Width="150px" CssClass="circular-image" Style="object-fit:cover;" />
                <br />
                <asp:Image ID="Image3" runat="server" Height="30px" Width="30px" ImageUrl="img/star_click.png" />
                <asp:Label ID="lb_stars" runat="server" Text='<%# Eval("平均評分") %>' Font-Size="Large"></asp:Label>
                <br />
                <br />
                <asp:Button ID="btn_ap" runat="server" class="button2222" Text="立即預約" Height="40px"  Width="120px" Font-Size="Large" OnClick="btn_ap_Click" />
            </td>
            <td style="height: 32px; text-align: left;">
                <asp:Label ID="lb_dename" runat="server" Text='<%# Eval("姓名") %>' Font-Bold="True" Font-Size="X-Large"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 40px; text-align: left;">
                <asp:Label ID="lb_de_intr" runat="server" Text='<%# Eval("介紹") %>'></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 40px; text-align: left;">
                <asp:Image ID="Image4" runat="server" Height="27px" Width="27px" ImageUrl="img/shop.png" />
                <asp:Label ID="lb_shop" runat="server" Text='<%# Eval("服務店家") %>'></asp:Label>
                &nbsp;&nbsp;
            <asp:Label ID="lb_address" runat="server" Text='<%# Eval("服務地址") %>'></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 33px; text-align: left;">
                <asp:Image ID="Image1" runat="server" Height="27px" Width="27px" ImageUrl="img/call.png" />
                <asp:Label ID="lb_phone" runat="server" Text='<%# Eval("聯絡電話") %>'></asp:Label>&nbsp;<asp:Image ID="Image5" runat="server" Height="27px" Width="27px" ImageUrl="img/line.png" />
                <asp:Label ID="lb_line" runat="server" Text='<%# Eval("lineid") %>'></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 37px; text-align: left;">
                <asp:Label ID="lb_services" runat="server" Text='<%# Eval("服務項目") %>'></asp:Label>
            </td>
        </tr>
    </table>

    <table style="width: 1000px; margin-top: 50px; margin-left: 100px;">
        <tr>
            <td style="height: 62px; text-align: left;">
                <asp:Label ID="Label7" runat="server" Text="作品集" Font-Bold="True" Font-Size="XX-Large"></asp:Label></td>
        </tr>
        <tr>
            <td style="text-align: left">
                <asp:ListView ID="lv_works" runat="server">
                    <ItemTemplate>
                        <asp:ImageButton ID="ImageButton1" runat="server" CssClass="circular-commend" Width="130px" Height="170px"  CommandName="work_de" CommandArgument='<%# Eval("作品編號") + "|" + Eval("設計師編號") %>' Style="margin-right: 15px; object-fit:cover;"/>
                    </ItemTemplate>
                </asp:ListView>
                <br />

            </td>
        </tr>
        <tr>
            <td style="margin-left: 150px;">
                <div style="margin-left: 400px;">
                    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="lv_works" PageSize="5">
                        <Fields>
                            <asp:NumericPagerField NumericButtonCssClass="datapagerStyle" />
                        </Fields>
                    </asp:DataPager>
                </div>
            </td>
        </tr>
    </table>

    <table style="width: 1000px; height: 222px; margin-top: 50px; margin-left: 100px;">
        <tr>
            <td style="text-align: left; height: 60px;" colspan="3">
                <asp:Label ID="Label8" runat="server" Text="評分與評論" Font-Bold="True" Font-Size="XX-Large"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 282px; padding-right: 20px;" rowspan="5">
                <asp:Image ID="Image6" runat="server" Height="50px" Width="50px" ImageUrl="img/star_click.png" />
                &nbsp;&nbsp;&nbsp;
                <br />
                <asp:Label ID="lb_score" runat="server" Text='<%# Eval("平均評分") %>' Font-Size="X-Large" ForeColor="#FFC000" Font-Bold="True"></asp:Label><br />
                <asp:Label ID="lb_comment_count" runat="server" Font-Bold="True" Font-Size="Medium" Text='<%# Eval("評論數量") %>' ForeColor="#FFC000"></asp:Label>
            </td>
            <td style="text-align: left; width: 16px; height: 32px;">5</td>
            <td style="text-align: left; height: 32px;">
                
            </td>
        </tr>
        <tr>
            <td style="text-align: left; width: 16px; height: 32px;">4</td>
            <td style="text-align: left; height: 32px;">
                
            </td>
        </tr>
        <tr>
            <td style="text-align: left; width: 16px; height: 32px;">3</td>
            <td style="text-align: left; height: 32px;">
                
            </td>
        </tr>
        <tr>
            <td style="text-align: left; width: 16px; height: 32px;">2</td>
            <td style="text-align: left; height: 32px;">
                
            </td>
        </tr>
        <tr>
            <td style="text-align: left; width: 16px; height: 32px;">1</td>
            <td style="text-align: left; height: 32px;">
                
            </td>
        </tr>
    </table>

    <table style="width: 600px; margin-left: 480px; margin-right: 800px; margin-top: 10px;">
        <tr>

            <td style="text-align: right; vertical-align: bottom; margin-bottom: 30px; padding-bottom: 15px;">
                <asp:Panel ID="pn_comment_btn" runat="server">
                    <asp:Label ID="Label6" runat="server" Text="排序"></asp:Label><br />
                    <asp:Button ID="btn_my_comment" runat="server" Text="我的評論" class="btn-edit" Font-Size="Large" Height="30px" Width="100px"  />
                    &nbsp;&nbsp;
                <asp:Button ID="btn_new_comment" runat="server" Text="最新評分" class="btn-edit" Font-Size="Large" Height="30px" Width="100px" />
                    &nbsp;&nbsp;
                <asp:Button ID="btn_higher_comment" runat="server" Text="最高評分" class="btn-edit" Font-Size="Large" Height="30px" Width="100px" />
                    &nbsp;&nbsp;
                <asp:Button ID="btn_low_comment" runat="server" Text="最低評分" class="btn-edit" Font-Size="Large" Height="30px" Width="100px" />
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; vertical-align: bottom;">
                <asp:ListView ID="rp_comment" runat="server">
                    <ItemTemplate>
                        <div style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #C9C9C9;">
                            <div style="width: 600px;">
                                <div style="width: 400px; display: inline-block;">
                                    <asp:Image ID="Image2" runat="server" Height="40px" Width="40px" CssClass="circular-image"
                                          ImageAlign="Bottom" style="object-fit:cover;" />
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("User_name") %>'></asp:Label>
                                </div>
                                <asp:PlaceHolder ID="ph_user_edit" runat="server" >
                                    <div style="width: 200px; display: inline-block; float: right;">
                                        <asp:Button ID="Button1" runat="server" Text="修改" CssClass="button2222" Height="30px" Width="60px" CommandName="edit" CommandArgument='<%# Eval("預約編號") %>' />&nbsp;&nbsp;
                                        <asp:Button ID="Button2" runat="server" Text="刪除" CssClass="button2222" Height="30px" Width="60px" CommandName="delete" CommandArgument='<%# Eval("預約編號") %>' /><br />
                                    </div>
                                </asp:PlaceHolder>
                            </div>
                            <asp:Image ID="Image7" runat="server" Height="20" Width="120" />
                            <asp:Label ID="Label5" runat="server" Text='<%# Eval("評論日期", "{0:yyyy/MM/dd}") %>'></asp:Label>
                            <%--                            <asp:Label ID="Label6" runat="server" Text='<%# ((TimeSpan)Eval("評論時間")).ToString(@"hh\:mm") %>'></asp:Label>--%>
                            <br />
                            <asp:Label ID="Label13" runat="server" Text='<%# "預約項目："+Eval("服務項目名稱") %>' Font-Size="14px" ForeColor="#ACACAC"></asp:Label><br />
                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("評論內容") %>'></asp:Label><br />
                            <asp:PlaceHolder ID="phDesignerReply" runat="server" >
                                <div style="border: 0px hidden #FFCC00; background-color: #E6E3BB; border-radius: 10px; padding-left: 10px; margin-left: 5px; margin-top: 5px; padding-top: 5px; width: 560px; margin-right: 5px;">
                                    <asp:Label ID="Label3" runat="server" Text="設計師的回覆："></asp:Label><br />
                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("回復") %>' Width="560px" Style="display: block; word-wrap: break-word; max-width: 540px;"></asp:Label><br />
                                </div>
                                <br />
                            </asp:PlaceHolder>
                        </div>
                        <br />
                    </ItemTemplate>
                </asp:ListView>
                <tr>
                <td style="text-align:center;">
                <asp:DataPager ID="DataPager2" runat="server" PagedControlID="rp_comment" PageSize="3">
                    <Fields>
                        <asp:NumericPagerField NumericButtonCssClass="datapagerStyle" />
                    </Fields>
                </asp:DataPager>
            </td>
        </tr>
    </table>


    <table style="vertical-align: text-top; margin-top: 0px; width: 800px; text-align: left;">
        <tr>
            <td style="text-align: left; height: 62px;">&nbsp;</td>
            <td style="text-align: left; height: 62px;" colspan="6">
                <asp:Label ID="Label9" runat="server" Text="可預約時段" Font-Bold="True" Font-Size="XX-Large"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 50PX; height: 26px; padding-top: -5px;">
                <asp:ImageButton ID="img_btn_left" runat="server" Height="25px" ImageUrl="~/userpage/img/arrow-left.png" Width="25px" Visible="False"/>
            </td>
            <td style="width: 50PX; vertical-align: text-top;" rowspan="7">
                <asp:DataList ID="DataList1" runat="server">
                    <HeaderTemplate>
                        <asp:Label ID="Label10" runat="server" Text='<%# Eval("Date") %>'></asp:Label><br />
                        <asp:Label ID="Label11" runat="server" Text="1"></asp:Label><br />
                        <br />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label12" runat="server" Text='<%# ((TimeSpan)Container.DataItem).ToString("hh\\:mm") %>'></asp:Label>
                    </ItemTemplate>
                </asp:DataList>
            </td>
            <td style="width: 50PX; vertical-align: text-top;" rowspan="7">
                <asp:DataList ID="DataList2" runat="server">
                    <HeaderTemplate>
                        <asp:Label ID="Label10" runat="server" Text='<%# Eval("Date") %>'></asp:Label><br />
                        <asp:Label ID="Label11" runat="server" Text="2"></asp:Label><br />
                        <br />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label12" runat="server" Text='<%# ((TimeSpan)Container.DataItem).ToString("hh\\:mm") %>'></asp:Label>
                    </ItemTemplate>
                </asp:DataList></td>
            <td style="width: 50PX; vertical-align: text-top;" rowspan="7">
                <asp:DataList ID="DataList3" runat="server">
                    <HeaderTemplate>
                        <asp:Label ID="Label10" runat="server" Text='<%# Eval("Date") %>'></asp:Label><br />
                        <asp:Label ID="Label11" runat="server" Text="3"></asp:Label><br />
                        <br />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label12" runat="server" Text='<%# ((TimeSpan)Container.DataItem).ToString("hh\\:mm") %>'></asp:Label>
                    </ItemTemplate>
                </asp:DataList></td>
            <td style="width: 50PX; vertical-align: text-top;" rowspan="7">
                <asp:DataList ID="DataList4" runat="server">
                    <HeaderTemplate>
                        <asp:Label ID="Label10" runat="server" Text='<%# Eval("Date") %>'></asp:Label><br />
                        <asp:Label ID="Label11" runat="server" Text="4"></asp:Label><br />
                        <br />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label12" runat="server" Text='<%# ((TimeSpan)Container.DataItem).ToString("hh\\:mm") %>'></asp:Label>
                    </ItemTemplate>
                </asp:DataList></td>
            <td style="width: 50PX; vertical-align: text-top;" rowspan="7">
                <asp:DataList ID="DataList5" runat="server">
                    <HeaderTemplate>
                        <asp:Label ID="Label10" runat="server" Text='<%# Eval("Date") %>'></asp:Label><br />
                        <asp:Label ID="Label11" runat="server" Text="5"></asp:Label><br />
                        <br />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label12" runat="server" Text='<%# ((TimeSpan)Container.DataItem).ToString("hh\\:mm") %>'></asp:Label>
                    </ItemTemplate>
                </asp:DataList></td>
            <td style="width: 50PX; vertical-align: text-top;" rowspan="7">
                <asp:DataList ID="DataList6" runat="server">
                    <HeaderTemplate>
                        <asp:Label ID="Label10" runat="server" Text='<%# Eval("Date") %>'></asp:Label><br />
                        <asp:Label ID="Label11" runat="server" Text="6"></asp:Label><br />
                        <br />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label12" runat="server" Text='<%# ((TimeSpan)Container.DataItem).ToString("hh\\:mm") %>'></asp:Label>
                    </ItemTemplate>
                </asp:DataList></td>
            <td style="width: 50PX; vertical-align: text-top;" rowspan="7">
                <asp:DataList ID="DataList7" runat="server">
                    <HeaderTemplate>
                        <asp:Label ID="Label10" runat="server" Text='<%# Eval("Date") %>'></asp:Label><br />
                        <asp:Label ID="Label11" runat="server" Text="7"></asp:Label><br />
                        <br />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label12" runat="server" Text='<%# ((TimeSpan)Container.DataItem).ToString("hh\\:mm") %>'></asp:Label>
                    </ItemTemplate>
                </asp:DataList></td>
            <td style="width: 50PX; height: 26px; padding-top: -5px;">
                <asp:ImageButton ID="img_btn_right" runat="server" Height="25px" ImageUrl="~/userpage/img/right-arrow.png" Width="25px" />
            </td>
        </tr>
        <tr>
            <td style="width: 50PX; vertical-align: text-top; padding-top: 0px;">&nbsp;</td>
            <td style="width: 50PX; vertical-align: text-top;">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 50PX; vertical-align: text-top; padding-top: 0px;">&nbsp;</td>
            <td style="width: 50PX; vertical-align: text-top;">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 50PX; vertical-align: text-top; padding-top: 0px;">&nbsp;</td>
            <td style="width: 50PX; vertical-align: text-top;">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 50PX; vertical-align: text-top; padding-top: 0px;">&nbsp;</td>
            <td style="width: 50PX; vertical-align: text-top;">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 50PX; vertical-align: text-top; padding-top: 0px;">&nbsp;</td>
            <td style="width: 50PX; vertical-align: text-top;">&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 50PX; vertical-align: text-top; padding-top: 0px;">&nbsp;</td>
            <td style="width: 50PX; vertical-align: text-top;">&nbsp;</td>
        </tr>
    </table>
</center>
<br />
</asp:Content>

