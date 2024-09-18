<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage.master" AutoEventWireup="true" CodeFile="contact.aspx.cs" Inherits="contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <section class="client py-lg-0 py-md-0 py-sm-0 py-0" id="client">
        <div class="container-fluid px-0">
           <div class="container-xxl py-5">
        <div class="container">
            <div class="text-center wow fadeInUp" data-wow-delay="0.1s">
                <h1 class="mb-5" style="color: #B0171F; font-weight: bolder;" >聯絡我們</h1>
            </div>
            <br />
            <div class="row g-4">
                <div class="col-lg-4 col-md-6 wow fadeInUp" data-wow-delay="0.1s">
                    <h5>我們的聯絡資訊</h5>
                    <p class="mb-4">如有任何問題，歡迎與我們聯繫</p>
                    <div class="d-flex align-items-center mb-4">
                        <div class="d-flex align-items-center justify-content-center flex-shrink-0 " style="width: 50px; height: 50px; background-color: #990033;">
                            <i class="fa fa-map-marker-alt text-white"></i>
                        </div>
                        <div class="ms-3"style="margin-left: 10px;">
                            <h5 style="color: #B0171F">地點</h5>
                            <p class="mb-0">屏東市民生路4-18號</p>
                        </div>
                    </div>
                    <div class="d-flex align-items-center mb-4">
                        <div class="d-flex align-items-center justify-content-center flex-shrink-0 " style="width: 50px; height: 50px; background-color: #990033;">
                            <i class="fa fa-phone text-white"></i>
                        </div>
                        <div class="ms-3" style="margin-left: 10px;">
                            <h5 style="color: #B0171F">電話</h5>
                            <p class="mb-0">0000-000-000</p>
                        </div>
                    </div>
                    <div class="d-flex align-items-center">
                        <div class="d-flex align-items-center justify-content-center flex-shrink-0 " style="width: 50px; height: 50px; background-color: #990033;">
                            <i class="fa fa-envelope-open text-white"></i>
                        </div>
                        <div class="ms-3" style="margin-left: 10px;">
                            <h5  style="color: #B0171F">Email</h5>
                            <p class="mb-0">NptuGym@gmail.com</p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4 col-md-6 wow fadeInUp" data-wow-delay="0.3s">
                    <iframe src="https://www.google.com/maps/embed?pb=!1m14!1m8!1m3!1d3681.8648490109836!2d120.510421!3d22.6588271!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x346e181dbba3004f%3A0x62d42664d6fd8a30!2z5ZyL56uL5bGP5p2x5aSn5a245bGP5ZWG5qCh5Y2A!5e0!3m2!1szh-TW!2stw!4v1697639903287!5m2!1szh-TW!2stw" width="750" height="350" style="border: 0;" allowfullscreen="" loading="lazy" referrerpolicy="no-referrer-when-downgrade"></iframe>
                </div>
            </div>
        </div>
    </div>
        </div>
</section>
</asp:Content>

