<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="contact.aspx.cs" Inherits="contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <!-- Page Header Start -->
    <div class="container-fluid page-header mb-5">
        <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 400px">
            <h4 class="display-4 mb-3 mt-0 mt-lg-5 text-white text-uppercase font-weight-bold">聯絡我們</h4>
        </div>
    </div>
    <!-- Page Header End -->

    <!-- Contact Start -->
    <div class="container pt-5">
        <div class="d-flex flex-column text-center mb-5">
            <h4 class="text-primary font-weight-bold">如有任何問題</h4>
            <h4 class="display-4 font-weight-bold">歡迎與我們聯繫</h4>
        </div>
        <div class="row px-3 pb-2">
            <div class="col-sm-4 text-center mb-3">
                <i class="fa fa-2x fa-map-marker-alt mb-3 text-primary"></i>
                <h4 class="font-weight-bold">地點</h4>
                <p>屏東市民生路4-18號</p>
            </div>
            <div class="col-sm-4 text-center mb-3">
                <i class="fa fa-2x fa-phone-alt mb-3 text-primary"></i>
                <h4 class="font-weight-bold">電話</h4>
                <p>08-7663800</p>
            </div>
            <div class="col-sm-4 text-center mb-3">
                <i class="far fa-2x fa-envelope mb-3 text-primary"></i>
                <h4 class="font-weight-bold">Email</h4>
                <p>NptuGym@gmail.com</p>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 pb-5">
                <iframe style="width: 100%; height: 392px;" src="https://www.google.com/maps/embed?pb=!1m14!1m8!1m3!1d3681.8648490109836!2d120.510421!3d22.6588271!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x346e181dbba3004f%3A0x62d42664d6fd8a30!2z5ZyL56uL5bGP5p2x5aSn5a245bGP5ZWG5qCh5Y2A!5e0!3m2!1szh-TW!2stw!4v1697639903287!5m2!1szh-TW!2stw" frameborder="0" allowfullscreen="" aria-hidden="false" tabindex="0"></iframe>
            </div>
        </div>
    </div>
    <!-- Contact End -->

</asp:Content>

