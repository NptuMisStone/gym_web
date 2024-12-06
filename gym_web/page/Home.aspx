<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .MoreCoachbtn {
            float: right;
            display: flex;
            font-size: 25px;
            font-weight: 550;
            background-color: transparent;
            border: none;
            color: red;
            transition: transform 0.3s ease;
        }

            .MoreCoachbtn:hover {
                transform: translateX(20px);
            }

        ul {
            list-style-type: none;
            padding: 0;
        }

            ul li {
                margin: 10px 0;
            }

        .RegisterBtn {
            border: none;
            border-radius: 5px;
            color: white;
            padding: 10px 20px;
            font-size: 1.2em;
            float: right;
            transition: transform 0.3s ease;
        }

            .RegisterBtn:hover {
                transform: scale(1.2);
            }

        .floating-obj { /*漂浮進入*/
            position: relative;
            opacity: 0;
            transform: translateY(150px);
            transition: opacity 0.6s ease-out, transform 0.6s ease-out;
        }

            .floating-obj.visible {
                opacity: 1;
                transform: translateY(0);
            }
    </style>
    <script>//重複漂浮
        document.addEventListener('scroll', function () {
            var objects = document.querySelectorAll('.floating-obj');
            var windowHeight = window.innerHeight || document.documentElement.clientHeight;

            objects.forEach(function (obj) {
                var rect = obj.getBoundingClientRect();

                // 檢查每個物件是否進入可視範圍
                if (rect.top <= windowHeight && rect.bottom >= 0) {
                    if (!obj.classList.contains('visible')) {
                        obj.classList.add('visible');
                    }
                } else {
                    // 物件離開可視範圍時移除 class，以便重新觸發動畫
                    if (obj.classList.contains('visible')) {
                        obj.classList.remove('visible');
                    }
                }
            });
        });
    </script>
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
                            &nbsp;</div>
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
            <div class="container pt-5 team">
                <div class="d-flex flex-column text-center mb-5">
                    <h4 class="text-primary font-weight-bold">NPTU GYM</h4>
                    <h4 class="display-4 font-weight-bold">熱門健身教練</h4>
                </div>
                <div class="floating-obj">
                    <!--漂浮進入-->
                    <asp:ListView ID="lv_coachdata" runat="server" OnItemCommand="lv_coachdata_ItemCommand">
                        <LayoutTemplate>
                            <div style="display: flex; justify-content: center;">
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div class="col-lg-3 col-md-6 mb-5">
                                <div class="card border-0 bg-secondary text-center text-white">
                                    <!-- 顯示圖片 -->
                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# GetImageUrl(Eval("健身教練圖片"),60) %>' CssClass="card-img-top" Style="object-fit: cover; height: 250px; width: 100%;" />

                                    <!-- 社交媒體圖示 -->
                                    <div class="card-social d-flex align-items-center justify-content-center">
                                        <%--                                        <a class="btn btn-outline-light rounded-circle text-center mr-2 px-0" style="width: 40px; height: 40px;"><i class="fab fa-twitter"></i></a>--%>
                                    </div>

                                    <!-- 點擊區域，透過 LinkButton 觸發事件 -->
                                    <asp:LinkButton ID="lb_coach" runat="server" CommandName="coach_detail" CommandArgument='<%# Eval("健身教練編號") %>' CssClass="card-body bg-secondary" Style="display: block; text-align: center; text-decoration: none; cursor: pointer;">
                                        <h4 class="card-title text-primary font-weight-bold"><%# Eval("健身教練姓名") %></h4>
                                        <p class="card-text" style="color: white"><%# Eval("註冊類型") %></p>
                                        <span style="font-size: 20px;">人氣 <%# Eval("人氣指數") %>%</span>
                                        <asp:Image ID="Image2" runat="server" Width="50px" Height="50px" ImageUrl="~/page/img/first.png" Style="float: right;" />
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
                <div>
                    <asp:Button ID="MoreCoachBtn" runat="server" Text="探索更多教練⇀" CssClass="MoreCoachbtn" OnClick="MoreCoachBtn_Click" />
                </div>
            </div>
        </div>

        <div class="container-fluid position-relative bmi my-5">
    <div class="container">
        <div class="row justify-content-center align-items-center">
            <!-- 左邊文字區塊 -->
            <div class="col-md-5 text-center">
                <h4 class="text-primary">Body Mass Index</h4>
                <h4 class="display-4 text-white font-weight-bold mb-4">什麼是BMI?</h4>
                <p class="m-0 text-white">以身體質量指數（Body Mass Index, BMI）來衡量肥胖程度，體重過重或是肥胖（BMI≧24）為糖尿病、心血管疾病、惡性腫瘤等慢性疾病的主要風險因素；而過瘦的健康問題，則會有營養不良、骨質疏鬆、猝死等健康問題。</p>
            </div>
            <!-- 右邊表單區塊 -->
            <div class="col-md-5 bg-secondary py-5">
                <div class="py-5 px-3 text-center">
                    <h1 class="mb-4 text-white">計算你的 BMI</h1>
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
                            <br /><br /><br /><br />
                        </div>
                    </div>
                    <asp:Panel ID="Panel1" runat="server" CssClass="centered-panel">
                        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</div>


        <div class="container pt-5 team">
            <div class="d-flex flex-column text-center mb-5">
                <h4 class="text-primary font-weight-bold">NPTU GYM</h4>
                <h4 class="display-4 font-weight-bold">讓你運動更有保障</h4>
            </div>
            <br />
            <div style="display: flex; justify-content: center; background-color: #f9f9f9;" class="floating-obj">
                <asp:Image ID="Image5" runat="server" ImageUrl="~/page/img/home-money.png" Style="width: 30%; height: auto;" />
                <div style="width: 30%; align-content: center; margin-left: 10%;">
                    <h2 style="text-align: center; font-weight: 600;">資訊及價格透明</h2>
                    <br />
                    <ul style="text-align: center; font-size: 18px;">
                        <li>課程資訊一目了然</li>
                        <li>不綁約、價格透明</li>
                        <li>讓您自由的選擇</li>
                        <li>並安排您的課程</li>
                    </ul>
                    <%--                <p style="text-align:center;font-size:18px;">課程資訊一目了然，不綁約、價格透明，讓您自由的選擇並安排您的課程</p>--%>
                </div>
            </div>
            <br />
            <br />
            <br />
            <br />
            <div style="display: flex; justify-content: center; background-color: #f9f9f9;" class="floating-obj">
                <div style="width: 30%; align-content: center; margin-right: 10%;">
                    <h2 style="text-align: center; font-weight: 600;">經審核過的健身教練</h2>
                    <br />
                    <ul style="text-align: center; font-size: 18px;">
                        <li>為平台使用者</li>
                        <li>提供良好的師資</li>
                        <li>讓您不用擔心</li>
                        <li>被有心人士詐騙</li>
                    </ul>
                    <%--                <p style="text-align:center;font-size:18px;">為平台使用者提供良好的師資，讓您不用擔心被有心人士詐騙</p>--%>
                </div>
                <asp:Image ID="Image6" runat="server" ImageUrl="~/page/img/home-coach.png" Style="width: 30%; height: auto;" />
            </div>
            <br />
            <br />
            <br />
            <br />
            <div style="display: flex; justify-content: center; background-color: #f9f9f9;" class="floating-obj">
                <asp:Image ID="Image7" runat="server" ImageUrl="~/page/img/home-comment.png" Style="width: 30%; height: auto;" />
                <div style="width: 30%; align-content: center; margin-left: 10%;">
                    <h2 style="text-align: center; font-weight: 600;">良好的使用環境</h2>
                    <br />
                    <ul style="text-align: center; font-size: 18px;">
                        <li>提供評論檢舉功能</li>
                        <li>避免您在使用平台時</li>
                        <li>因不適當的言論</li>
                        <li>而壞了好心情</li>
                    </ul>
                    <%--                <p style="text-align:center;font-size:18px;">提供評論檢舉功能，避免您在使用平台時，因不適當的言論，而壞了好心情</p>--%>
                </div>
            </div>
        </div>
        <br />
        <br />
        <br />
        <br />
        <br />
        <!-- About Start -->
        <div class="container py-5">
            <div class="row align-items-center">
                <div class="col-lg-6">
                    <img class="img-fluid mb-4 mb-lg-0" src="img/NPTU_GYM.png" alt="Image" >
                </div>
                <div class="col-lg-6">
                    <h2 class="display-4 font-weight-bold mb-4">我們團隊</h2>
                    <p>致力於為所有使用者提供良好且資訊透明的環境來預約、提高教練被發現的機會。</p>
                    <div class="row py-2">
                        <div class="col-sm-6">
                            <i class="flaticon-barbell display-2 text-primary"></i>
                            <h4 class="font-weight-bold">關於我們</h4>
                            <p>我們是由一群熱愛健身及運動的大學生組成</p>
                        </div>
                        <div class="col-sm-6" style="align-content: center;">
                            <a href="contact.aspx" class="btn btn-lg px-4 btn-outline-primary">了解更多</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- About End -->
    </section>
</asp:Content>



