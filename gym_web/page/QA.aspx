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
            font-weight:bold;
            color:#FF0000;
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
        <summary>如何知道健身教練的詳細資訊及安排的課程？</summary><!--問題-->
        <div class="answer">
            <p>在<a href="coach.aspx">健身教練</a>頁面，點擊任何一個教練即可查看教練詳細資訊及他所有安排的課程。</p><!--答案-->
        </div>
        </details>
        <details>
        <summary>想知道如何預約課程？</summary>
        <div class="answer">
            <p>在<a href="class.aspx">尋找課程</a>中會有各個教練安排的課程供你們選擇，課程點擊後，即可依照課程日期、時間進行預約。</p>
        </div>
    </details>
        <details>
            <summary>如何查找特定課程？</summary>
            <div class="answer">
                <p>在<a href="class.aspx">尋找課程</a>中，搜尋列表旁，有個"篩選按鈕<span><img src="img/filter.png" style="width:25px;height:25px;" /></span>"，可進行選取想查詢的課程。</p>
            </div>
        </details>
        <details>
        <summary>想知道如何取消預約？</summary>
        <div class="answer">
            <p>在<a href="../User/User_appointment_record.aspx">預約紀錄</a>中，可以查看所有預約紀錄，並進行取消。</p>
        </div>
    </details>
        <details>
        <summary>如何查看目前所收藏的教練及課程？</summary>
        <div class="answer">
            <p>在<a href="../User/User_Like.aspx">我的收藏</a>中，即可看到所收藏的教練及課程。</p>
        </div>
    </details>
</div>
<!-- Team End -->
</asp:Content>

