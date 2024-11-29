<%@ Page Title="" Language="C#" MasterPageFile="~/page/MasterPage2.master" AutoEventWireup="true" CodeFile="class.aspx.cs" Inherits="page_class" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        /* 灰色覆蓋層 */
        .overlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.5);
            z-index: 999;
        }
        .close-button {
            float: right;
            font-size: 50px;
            background: none;
            border: none;
            cursor: pointer;
        }
        /* 側邊彈窗 */
        .side-panel {
            position: fixed;
            top: 0;
            left: -300px; /* 初始位置在頁面外 */
            width: 300px;
            height: 100%;
            background-color: white;
            box-shadow: -2px 0 5px rgba(0,0,0,0.5);
            z-index: 1000;
            transition: left 0.5s; /* 動畫效果 */
        }

        /* 弹窗内容 */
        .panel-content {
            padding: 20px;
            max-height: 100%; /* 內容的最大高度 */
            overflow-y: auto; /* 內容超出時，啟用滾動 */
        }
        .btn-hide {
            display:none;
        }
    </style>
    <script>
        // 顯示弹窗
        function showPopup() {
            document.getElementById('overlay').style.display = 'block';
            var panel = document.getElementById('<%= sidePanel.ClientID %>');
            panel.style.display = 'block';
            document.body.style.overflow = 'hidden'; // 禁止頁面滾動
            setTimeout(function () {
                panel.style.left = '0'; // 右邊滑出
            }, 10);
        }

        // 隐藏弹窗
        function hidePopup() {
            document.getElementById('overlay').style.display = 'none';
            var panel = document.getElementById('<%= sidePanel.ClientID %>');
            panel.style.left = '-300px'; // 滑回去XD
            setTimeout(function () {
                panel.style.display = 'none';
                document.body.style.overflow = ''; // 恢復頁面滾動
            }, 500); // 延遲
        }
        function triggerPostBack() {
            __doPostBack('overlayClicked', '');
        }
    </script>
    <script>
        function validateAndHidePopup() {
            var minMoney = document.getElementById('<%= MinMoney.ClientID %>').value;
            var maxMoney = document.getElementById('<%= MaxMoney.ClientID %>').value;
            if (minMoney !== "" && maxMoney !== "") {
                var min = parseInt(minMoney, 10);
                var max = parseInt(maxMoney, 10);

                if (min > max) {
                    alert('最小值不能大於最大值');
                    return false;
                }
            }

            // 如果验证通过，隐藏弹窗
            hidePopup();
            return true;
        }
    </script>
    <script type="text/javascript">
        function validateInput(textbox) {
            var value = parseInt(textbox.value, 10);
            
            // 如果輸入不是數字或為空，直接返回，不修改值
            if (isNaN(value) || textbox.value === "") {
                return;
            }
            // 檢查是否超出範圍
            if (value > 9999) {
                textbox.value = 9999;  // 將值限制為9999
            } else if (value < 0) {
                textbox.value = 0;     // 將值限制為0
            }
        }
    </script>

    <!-- Page Header Start -->
    <div class="container-fluid page-header mb-5">
        <div class="d-flex flex-column align-items-center justify-content-center pt-0 pt-lg-5" style="min-height: 400px">
            <h4 class="display-4 mb-3 mt-0 mt-lg-5 text-white text-uppercase font-weight-bold">尋找課程</h4>
        </div>
    </div>
    <!-- Page Header End -->

    <!-- GYM Class Start -->
    <div class="container feature pt-5 w-100" style="position: relative;">
        <div style="display: flex; justify-content: center; align-items: center; border: 2px solid #000000; border-radius: 10px; padding: 10px; width: 70%; margin: 0 auto;">
            <!-- 調整 TextBox 大小 -->
            <asp:TextBox ID="SearchText" runat="server" placeholder="搜尋..." 
                         style="border: none; outline: none; width: 90%; font-size: 18px; padding: 10px;" />
            <!-- 調整 ImageButton 大小 -->
            <asp:ImageButton ID="SearchBtn" runat="server" ImageUrl="~/page/img/search.png" 
                             style="width: 30px; height: 30px;" OnClick="SearchBtn_Click" />
        </div>
            <asp:ImageButton ID="FilterBtn" runat="server" ImageUrl="~/page/img/filter.png" 
                AlternateText="篩選" style="position: absolute; top: 67.5%; right: 10%; transform: translateY(-50%); width: 50px; height: 50px;" 
                OnClientClick="showPopup(); return false;" OnClick="FilterBtn_Click" />
    </div>
    
    <!-- 灰色覆蓋層 -->
    <div id="overlay" class="overlay" style="display:none;" onclick="hidePopup(); triggerPostBack();"><!--點灰色部分也可以關閉，但會清空篩選-->
        
    </div>
    <!-- 弹窗内容 -->
    <asp:Panel ID="sidePanel" runat="server" CssClass="side-panel" style="display:none;">
        <div class="panel-content">
            <asp:Button ID="Button2" runat="server" Text="×" OnClientClick="hidePopup();return false; " CssClass="close-button"/>
            <h2>篩選</h2>
            <asp:Label ID="Label1" runat="server" Text="課程類型"></asp:Label>
            <br />
            <asp:DropDownList ID="ClassTypeDDL" runat="server"></asp:DropDownList>
            <br />
            <asp:Label ID="Label2" runat="server" Text="教練性別"></asp:Label>
            <asp:RadioButtonList ID="CoachGenderRB" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="" Selected="True" >全部</asp:ListItem>
                <asp:ListItem Value="1">男</asp:ListItem>
                <asp:ListItem Value="2">女</asp:ListItem>
                <asp:ListItem Value="3">其他</asp:ListItem>
            </asp:RadioButtonList>
            <asp:Label ID="Label3" runat="server" Text="課程費用"></asp:Label>
            <br />
            <asp:TextBox ID="MinMoney" runat="server" placeholder="最小值" Width="75px" TextMode="Number" min="0" max="9999" oninput="validateInput(this)" ></asp:TextBox>
            <span>~</span>
            <asp:TextBox ID="MaxMoney" runat="server" placeholder="最大值" Width="75px" TextMode="Number" min="0" max="9999" oninput="validateInput(this)" ></asp:TextBox>
            <br />
            <asp:Label ID="Label4" runat="server" Text="課程人數"></asp:Label>
            <asp:RadioButtonList ID="ClassPeopleRBL" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="0" Selected="True" >全部</asp:ListItem>
                <asp:ListItem Value="1"> 一對一</asp:ListItem>
                <asp:ListItem Value="2">團體</asp:ListItem>
            </asp:RadioButtonList>
            <asp:Label ID="Label5" runat="server" Text="課程地點"></asp:Label>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <script type="text/javascript">
                // 為所有 TreeNode 的勾選加入事件
                function setupTreeViewCheckboxes() {
                    var treeView = document.getElementById('<%= CityTreeView.ClientID %>');
                    var checkboxes = treeView.getElementsByTagName('input');
                    for (var i = 0; i < checkboxes.length; i++) {
                        if (checkboxes[i].type === 'checkbox') {
                            checkboxes[i].onclick = function() {
                                __doPostBack('<%= Button1.UniqueID %>', '');
                            };
                        }
                    }
                }
                window.onload = setupTreeViewCheckboxes;

                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_endRequest(function () {
                    setupTreeViewCheckboxes(); // 每次 UpdatePanel 更新後重新绑定事件
                });
            </script>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:TreeView ID="CityTreeView" runat="server" OnTreeNodeCheckChanged="CityTreeView_TreeNodeCheckChanged" ShowCheckBoxes="All" EnableViewState="true">
                    </asp:TreeView>
                    <asp:Button ID="Button1" runat="server" Text="" CssClass="btn-hide" OnClick="Button1_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Button ID="SearchFilterBtn" runat="server" Text="查詢" OnClientClick="return validateAndHidePopup(); " OnClick="SearchFilterBtn_Click" ValidationGroup="FilterValidation" />
        </div>
    </asp:Panel>

    <div class="container feature pt-5 w-100">
        <div class="row">
            <asp:ListView ID="lv_class" runat="server" OnItemCommand="lv_class_ItemCommand">
                <layouttemplate>
                    <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                </layouttemplate>
                <itemtemplate>
                    <!-- 使用 col-md-6 控制每個課程寬度為6格 (半行寬) -->
                    <div class="col-md-6 mb-4" style="display: flex; justify-content: center;">
                        <!-- 調整 linkbtn 的 style 確保不會影響內部元素 -->
                        <!-- 將這裡的框線改為黑色且加粗 -->
                        <div style="width: 100%; transition: background-color 0.3s ease; border: 2px solid black; border-radius: 8px; overflow: hidden;"
                            onmouseover="this.style.backgroundColor='#f0f0f0'"
                            onmouseout="this.style.backgroundColor=''">
                            <!-- 將背景色過渡效果與懸停效果直接加在 style 和事件屬性中 -->
                            <asp:LinkButton ID="lb_class" runat="server" CommandName="see_detail" CommandArgument='<%# Eval("課程編號") %>'
                                CssClass="unstyled-link"
                                Style="position:relative; display: block; text-align: left; text-decoration: none; cursor: pointer;">
                                <div class="row align-items-center" style="padding: 20px;">
                                    <div class="col-sm-6" style="padding: 10px 15px;">
                                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# GetImageUrl(Eval("課程圖片"),60) %>' CssClass="img-fluid mb-3 mb-sm-0" Style="object-fit: cover; height: 130px; width: 100%;" />
                                        <%# Convert.ToInt32(Eval("上課人數")) == 1 ? 
                                            "<i style='font-size:20px; font-weight: bold;'>一對一</i>" : 
                                            "<i style='font-size:20px; font-weight: bold;'>團體</i>" %>
                                    </div>
                                    <div class="col-sm-6" style="padding: 10px 15px;">
                                        <h4 class="font-weight-bold"><%# Eval("課程名稱") %></h4>
                                        
                                        <h4 class="font-weight-bold mb-4" style="color: #e31c25"><%# "$ " + Convert.ToDouble(Eval("課程費用")).ToString("F0") + " /堂"%></h4>
                                        <p><%# Eval("健身教練姓名") + " 教練" %></p>
                                        <p><%# Eval("課程內容介紹") %></p>
                                        <p><%# "人數：" + Eval("上課人數") + "人"%></p>
                                    </div>
                                </div>
                            </asp:LinkButton>
                            <!-- 愛心圖示 -->
                            <div style="position: absolute; top: 5%; left: 5%;">
                                <asp:ImageButton 
                                    ID="LikeBtn"
                                    runat="server"
                                    CommandArgument='<%# Eval("課程編號") %>'
                                    OnClick="LikeBtn_Click"
                                    ImageUrl='<%# GetLikeImageUrl(Eval("課程編號")) %>'
                                    Style="width: 40px; height: 40px; object-fit: contain;" />
                            </div>
                        </div>
                    </div>
                </itemtemplate>
            </asp:ListView>
        </div>
    </div>
    <!-- GYM Class End -->
</asp:Content>

