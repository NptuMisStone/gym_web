<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="client py-lg-0 py-md-0 py-sm-0 py-0" id="client">
        <div class="container-fluid px-0">
            <div id="carouselExampleIndicators" class="carousel slide" data-ride="carousel" data-interval="2000">
                <ol class="carousel-indicators">
                    <asp:Repeater ID="Repeater2" runat="server">
                        <ItemTemplate>
                            <li data-target="#carouselExampleIndicators" data-slide-to="<%# Container.ItemIndex %>" class='<%# Container.ItemIndex == 0 ? "active" : "" %>'></li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ol>
                <div class="carousel-inner" style="max-height: 720px; margin: auto;">
                    <asp:Repeater ID="Repeater1" runat="server">
                        <ItemTemplate>
                            <div class="carousel-item <%# Container.ItemIndex == 0 ? "active" : "" %>">
                                <a href='<%# ((Tuple<string, string>)Container.DataItem).Item2 %>' target="_blank">
                                    <img src='<%# ((Tuple<string, string>)Container.DataItem).Item1 %>' class="d-block w-100" alt="Slide <%# Container.ItemIndex + 1 %>" style="height: 720px; object-fit: cover;">
                                </a>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <a class="carousel-control-prev" href="#carouselExampleIndicators" role="button" data-slide="prev">
                    <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                    <span class="sr-only">Previous</span>
                </a>
                <a class="carousel-control-next" href="#carouselExampleIndicators" role="button" data-slide="next">
                    <span class="carousel-control-next-icon" aria-hidden="true"></span>
                    <span class="sr-only">Next</span>
                </a>
            </div>

            <h3 class="title text-center mb-lg-5 mb-md-4 mb-sm-4 mb-3">首頁</h3>
        </div>
    </section>
</asp:Content>



