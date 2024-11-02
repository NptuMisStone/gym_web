<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="BMI.aspx.cs" Inherits="page_BMI" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .centered-panel {
            text-align: center;
            margin: 0 auto;
            width: fit-content;
            color: white;
            font-size: 25px;
        }
    </style>
    <div class="container-fluid page-header mb-5">
        <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 400px">
            <h4 class="display-4 mb-3 mt-0 mt-lg-5 text-white text-uppercase font-weight-bold">BMI計算</h4>
        </div>
    </div>
    <div class="container pt-5 team">
        <div class="d-flex flex-column text-center mb-5">
            <h4 class="text-primary font-weight-bold">NPTU GYM</h4>
            <h4 class="display-4 font-weight-bold">BMI計算</h4>
        </div>
        <div class="container-fluid position-relative bmi my-5">
            <div class="container">
                <div class="row px-3 align-items-center">
                    <div class="col-md-6">
                        <div class="pr-md-3 d-none d-md-block">
                            <h4 class="text-primary">Body Mass Index </h4>
                            <h4 class="display-4 text-white font-weight-bold mb-4">什麼是BMI?</h4>
                            <p class="m-0 text-white">以身體質量指數（Body Mass Index, BMI）來衡量肥胖程度，體重過重或是肥胖（BMI≧24）為糖尿病、心血管疾病、惡性腫瘤等慢性疾病的主要風險因素；而過瘦的健康問題，則會有營養不良、骨質疏鬆、猝死等健康問題。</p>
                        </div>
                    </div>
                    <div class="col-md-6 bg-secondary py-5">
                        <div class="py-5 px-3">
                            <h1 class="mb-4 text-white">計算你的 BMI</h1>
                            <form>
                                <div class="form-row">
                                    <div class="col form-group">
                                        <asp:TextBox ID="Weight" runat="server" CssClass="form-control form-control-lg bg-dark text-white" placeholder="公斤 (KG)"></asp:TextBox>
                                    </div>
                                    <div class="col form-group">
                                        <asp:TextBox ID="Height" runat="server" CssClass="form-control form-control-lg bg-dark text-white" placeholder="身高 (CM)"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-row">
                                    <div class="col form-group">
                                        <asp:TextBox ID="Age" runat="server" CssClass="form-control form-control-lg bg-dark text-white" placeholder="年齡(歲)"></asp:TextBox>
                                    </div>
                                    <div class="col form-group">
                                        <asp:DropDownList ID="Gender" runat="server" CssClass="custom-select custom-select-lg bg-dark text-muted">
                                            <asp:ListItem Selected="True">性別</asp:ListItem>
                                            <asp:ListItem>男性</asp:ListItem>
                                            <asp:ListItem>女性</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-row">
                                    <div class="col">
                                        <asp:Button ID="Button1" runat="server" Text="計算" CssClass="btn btn-lg btn-block btn-dark border-light" OnClick="Button1_Click" />
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                    </div>
                                </div>
                            </form>
                            <asp:Panel ID="Panel1" runat="server" CssClass="centered-panel">
                                <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

