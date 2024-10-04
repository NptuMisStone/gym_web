<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="QA.aspx.cs" Inherits="page_QA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style>
        details {
            margin:0 auto 10px;
            width:580px;
        }
        summary {
            display:flex;
            justify-content:space-between;
            align-items:center;
            padding:20px 30px;font-size:18px;
            background-color:#d6d6d6;
            border-radius:10px;
            cursor:pointer;
        }
        summary:hover,details[open] summary{
            background-color:#bbb;
        }
        summary::after{
            content:'＞';
            margin-left:30px;
            color:#5b8f8f;
            font-size:21px;
            transition:transform .5s;
        }
        details[open] summary::after{
            transform:rotate(90deg);
        }
        .answer{
            padding:20px;
            line-height:1.6;
        }
    </style>
    <!-- Page Header Start -->
<div class="container-fluid page-header mb-5">
    <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 400px">
        <h4 class="display-4 mb-3 mt-0 mt-lg-5 text-white text-uppercase font-weight-bold">常見問題Q&A</h4>
    </div>
</div>
<!-- Page Header End -->

<!-- Team Start -->
<div class="container pt-5 team">
    <div class="d-flex flex-column text-center mb-5">
        <h4 class="text-primary font-weight-bold">NPTU GYM</h4>
        <h4 class="display-4 font-weight-bold">常見問題Q&A</h4>
    </div>
    <details>
        <summary>這是什麼平台？</summary><!--問題-->
        <div class="answer">
            <p>我也不知道</p><!--答案-->
        </div>
        </details>
        <details>
        <summary></summary>
        <div class="answer">
            <p></p>
        </div>
    </details>
        <details>
        <summary></summary>
        <div class="answer">
            <p></p>
        </div>
    </details>
        <details>
        <summary></summary>
        <div class="answer">
            <p></p>
        </div>
    </details>
        <details>
        <summary></summary>
        <div class="answer">
            <p></p>
        </div>
    </details>
</div>
<!-- Team End -->
</asp:Content>

