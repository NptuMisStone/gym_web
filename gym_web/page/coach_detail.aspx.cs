using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class page_coach_detail : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    public static int user_loginsuccess, User_id;
    public static string coach_num;

    protected void Page_Load(object sender, EventArgs e)
    {
        user_loginsuccess = Convert.ToInt32(Session["user_loginsuccess"]);
        if (!IsPostBack)
        {
            User_id = Convert.ToInt32(Session["User_id"]);
            coach_num = Convert.ToString(Session["coach_num"]);
            LoadCoachDetails();
            BindClass();
            update_ProgressBar();
            bind_commend_score();
            bind_rp_comment();
        }
        if (Request["__EVENTTARGET"] == "ReportComment")
        {
            string[] args = Request["__EVENTARGUMENT"].Split('|');
            int commentId = Convert.ToInt32(args[0]);
            string reason = args[1]; // 理由

            ReportComment(commentId, reason);
            string script = @"<script>
                Swal.fire({
                    icon: 'success',
                    title: '檢舉成功',
                    text: '檢舉內容已提交管理員',
                    showConfirmButton: false,
                    timer: 1500
                });
            </script>";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertSuccess", script, false);
            LoadCoachDetails();
            BindClass();
            update_ProgressBar();
            bind_commend_score();
            bind_rp_comment();
        }
    }
    private void LoadCoachDetails()
    {
        Debug.WriteLine("健身教練編號： " + coach_num);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM [健身教練審核合併] WHERE [健身教練編號] = @coach_num";
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@coach_num", coach_num);
            SqlDataReader reader = command.ExecuteReader();

            // 使用 DataTable 來綁定 Repeater，方便在 ItemDataBound 中存取欄位
            DataTable dt = new DataTable();
            dt.Load(reader);
            rp_coach.DataSource = dt;
            rp_coach.DataBind();

            reader.Close();
        }
    }
    protected void rp_coach_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        // 確保只處理資料項目（排除 Header、Footer 等）
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            // 根據資料來源類型調整轉型
            var dataItem = e.Item.DataItem as DataRowView; // 如果使用 DataTable

            if (dataItem != null)
            {
                string registrationType = dataItem["註冊類型"].ToString();

                // 找到 Panel_store 控制項
                Panel panelStore = e.Item.FindControl("Panel_store") as Panel;
                if (panelStore != null)
                {
                    if (registrationType == "私人健身教練")
                    {
                        panelStore.Visible = false; // 隱藏 Panel_store
                    }
                    else
                    {
                        panelStore.Visible = true; // 顯示 Panel_store
                    }
                }
            }

            // 獲取教練編號
            var coachId = DataBinder.Eval(e.Item.DataItem, "健身教練編號").ToString();

            // 獲取 LikeBtn 控制項
            ImageButton LikeBtn = (ImageButton)e.Item.FindControl("LikeBtn");

            // 獲取使用者編號
            var userId = Session["User_id"]?.ToString();

            // 綁定 LikeBtn 的狀態
            BindLikeBtn(LikeBtn, coachId, userId);
        }
    }

    private void BindClass()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM [健身教練課程-有排課的] WHERE 健身教練編號 = @coach_num";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@coach_num", coach_num);

            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                lv_classes.DataSource = dt;
                lv_classes.DataBind();
                lb_noClasses.Visible = false;  // 隱藏 "尚未安排課程" 的訊息
            }
            else
            {
                lv_classes.DataSource = null;
                lv_classes.DataBind();
                lb_noClasses.Visible = true;  // 顯示 "尚未安排課程" 的訊息
            }
        }
    }

    protected void lv_classes_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "ViewDetails")
        {
            // 獲取點擊的課程編號
            string classId = e.CommandArgument.ToString();

            // 將課程編號儲存到 Session 中
            Session["Class_id"] = classId;

            // 重定向到 class_detail.aspx
            Response.Redirect("class_detail.aspx");
        }
    }
    public string GetGenderDescription(object genderValue)
    {
        if (genderValue == null || string.IsNullOrEmpty(genderValue.ToString()))
        {
            return "未知";
        }

        switch (genderValue.ToString())
        {
            case "1":
                return "男性";
            case "2":
                return "女性";
            case "3":
                return "不願透露性別";
            default:
                return "未知";
        }
    }

    protected string GetImageUrl(object imageData, int quality)
    {
        if (imageData != null && imageData != DBNull.Value)
        {
            byte[] bytes = (byte[])imageData;

            using (MemoryStream originalStream = new MemoryStream(bytes))
            using (MemoryStream compressedStream = new MemoryStream())
            {
                // Decode the original image
                System.Drawing.Image originalImage = System.Drawing.Image.FromStream(originalStream);

                // Create an EncoderParameters object to set the image quality
                System.Drawing.Imaging.EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                // Get the JPG codec info
                ImageCodecInfo jpgCodec = ImageCodecInfo.GetImageEncoders()
                    .First(codec => codec.MimeType == "image/jpeg");

                // Save the compressed image to the compressedStream
                originalImage.Save(compressedStream, jpgCodec, encoderParameters);

                // Convert the compressed image to a base64 string
                byte[] compressedBytes = compressedStream.ToArray();
                string base64String = Convert.ToBase64String(compressedBytes);

                // Generate the data URI for the compressed image
                return "data:image/jpeg;base64," + base64String;
            }
        }
        else
        {
            return "~/page/img/coach_main_ic_default.jpg"; // 替代圖片的路徑
        }
    }
    protected string GetImageUrl2(object imageData, int quality)//課程
    {
        if (imageData != null && imageData != DBNull.Value)
        {
            byte[] bytes = (byte[])imageData;

            using (MemoryStream originalStream = new MemoryStream(bytes))
            using (MemoryStream compressedStream = new MemoryStream())
            {
                // Decode the original image
                System.Drawing.Image originalImage = System.Drawing.Image.FromStream(originalStream);

                // Create an EncoderParameters object to set the image quality
                System.Drawing.Imaging.EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                // Get the JPG codec info
                ImageCodecInfo jpgCodec = ImageCodecInfo.GetImageEncoders()
                    .First(codec => codec.MimeType == "image/jpeg");

                // Save the compressed image to the compressedStream
                originalImage.Save(compressedStream, jpgCodec, encoderParameters);

                // Convert the compressed image to a base64 string
                byte[] compressedBytes = compressedStream.ToArray();
                string base64String = Convert.ToBase64String(compressedBytes);

                // Generate the data URI for the compressed image
                return "data:image/jpeg;base64," + base64String;
            }
        }
        else
        {
            return "~/page/img/coach_class_main_ic_default.png"; // 替代圖片的路徑
        }
    }
    protected string GetPeopleType(int num)
    {
        if (num > 1)
        {
            return "團體課程";
            
        }
        else if(num == 1)
        {
            return "一對一課程";
        }
        else
        {
            return "無法辨識";
        }
    }
    private void update_ProgressBar()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "SELECT COUNT(*) AS comment_count, " +
                  "SUM(CASE WHEN 評分 = 1 THEN 1 ELSE 0 END) AS star1_count, " +
                  "SUM(CASE WHEN 評分 = 2 THEN 1 ELSE 0 END) AS star2_count, " +
                  "SUM(CASE WHEN 評分 = 3 THEN 1 ELSE 0 END) AS star3_count, " +
                  "SUM(CASE WHEN 評分 = 4 THEN 1 ELSE 0 END) AS star4_count, " +
                  "SUM(CASE WHEN 評分 = 5 THEN 1 ELSE 0 END) AS star5_count " +
                  "FROM 查看評論 " +
                  "WHERE 健身教練編號 = @coach_num";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@coach_num", coach_num);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                int commentCount = Convert.ToInt32(reader["comment_count"]);

                if (reader["star1_count"] != DBNull.Value)
                {
                    double star1Count = Convert.ToInt32(reader["star1_count"]);
                    
                    int progressValue1 = (int)((star1Count * 100) / commentCount);
                    litProgStar1.Text = $"<script>document.getElementById('prog_star1').style.width = '{progressValue1}%';</script>";
                }

                if (reader["star2_count"] != DBNull.Value)
                {
                    double star2Count = Convert.ToInt32(reader["star2_count"]);
                    int progressValue2 = (int)((star2Count * 100) / commentCount);
                    litProgStar2.Text = $"<script>document.getElementById('prog_star2').style.width = '{progressValue2}%';</script>";
                }

                if (reader["star3_count"] != DBNull.Value)
                {
                    double star3Count = Convert.ToInt32(reader["star3_count"]);
                    int progressValue3 = (int)((star3Count * 100) / commentCount);

                    litProgStar3.Text = $"<script>document.getElementById('prog_star3').style.width = '{progressValue3}%';</script>";
                }

                if (reader["star4_count"] != DBNull.Value)
                {
                    double star4Count = Convert.ToInt32(reader["star4_count"]);
                    int progressValue4 = (int)((star4Count * 100) / commentCount);

                    litProgStar4.Text = $"<script>document.getElementById('prog_star4').style.width = '{progressValue4}%';</script>";
                }

                if (reader["star5_count"] != DBNull.Value)
                {
                    double star5Count = Convert.ToInt32(reader["star5_count"]);
                    int progressValue5 = (int)((star5Count * 100) / commentCount);
                    litProgStar5.Text = $"<script>document.getElementById('prog_star5').style.width = '{progressValue5}%';</script>";
                }
            }

        }
    }
    private void bind_commend_score()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "select 平均評分,評論數量 from [健身教練-評分] where 健身教練編號=@coach_num";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@coach_num", coach_num);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                object averageScoreObj = reader["平均評分"];
                if (averageScoreObj != DBNull.Value)
                {
                    double averageScore = Convert.ToDouble(averageScoreObj);
                    lb_score.Text = averageScore.ToString("0.0") + "顆星"; // 顯示一位小數
                }
                else
                {
                    lb_score.Text = "尚未有評分";
                }
                lb_comment_count.Text = reader["評論數量"].ToString() + "則評論";
            }
        }
    }
    private void bind_rp_comment()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "SELECT * FROM [查看評論] WHERE 健身教練編號 =@coach_num ORDER BY 評論日期 DESC";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@coach_num", coach_num);

            // 使用 DataTable 來保存資料
            DataTable dataTable = new DataTable();
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
            {
                dataAdapter.Fill(dataTable);
            }

            // 將 DataTable 繫結到 rp_comment
            rp_comment.DataSource = dataTable;
            rp_comment.DataBind();

            connection.Close();

            if (dataTable.Rows.Count > 0)
            {
                // 有數據時顯示 pn_comment_btn 控件
                pn_comment_btn.Visible = true;
            }
            else
            {
                // 沒有數據，可以選擇隱藏 pn_comment_btn 控件
                pn_comment_btn.Visible = false;
            }
        }
    }
    protected string Getstar_img(object rating)
    {
        int ratingValue = (int)rating;

        string imageUrl = string.Empty;

        // 使用 switch 語句來處理不同的評分值
        switch (ratingValue)
        {
            case 1:
                imageUrl = "img/star1.png";
                break;
            case 2:
                imageUrl = "img/star2.png";
                break;
            case 3:
                imageUrl = "img/star3.png";
                break;
            case 4:
                imageUrl = "img/star4.png";
                break;
            case 5:
                imageUrl = "img/star5.png";
                break;
        }

        return imageUrl;
    }
    protected bool Has_reply(object commend_id)
    {
        bool exists = false;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "select 回覆 from 查看評論 where 評論編號=@commend_id";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@commend_id", commend_id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (reader["回覆"] != null)
                    {
                        exists = reader["回覆"] != DBNull.Value;
                    }
                }
            }
        }
        return exists;
    }
    protected bool has_comment(object commend_id)
    {
        bool exists = false;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "select 使用者編號 from 查看評論 where 評論編號=@commend_id";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@commend_id", commend_id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (Convert.ToInt32(reader["使用者編號"]) == User_id)
                    {
                        exists = true;
                    }
                }
            }
        }
        return exists;
    }
    
    

    protected void rp_comment_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "edit")
        {
            Session["ap_id"] = e.CommandArgument;
            Response.Redirect("~/User/User_comment.aspx");
        }
        else if(e.CommandName =="delete")
        {
            int ap_id = Convert.ToInt32(e.CommandArgument);
            DeleteComment(ap_id);
        }
        else if (e.CommandName == "report")
        {
            if (Session["User_id"] == null)
            {
                // 驗證用戶是否登入的類別函數
                CheckLogin.CheckUserOrCoachLogin(this.Page, "User");
            }
            else
            {
                int commentId = Convert.ToInt32(e.CommandArgument);
                string script = $@"
                <script>
                Swal.fire({{
                    title: '檢舉評論',
                    text: '請選擇檢舉的理由',
                    input: 'select',
                    inputOptions: {{
                        '發表仇恨、歧視、具有攻擊性言論': '發表仇恨、歧視、具有攻擊性言論',
                        '成人色情、性騷擾內容': '成人色情、性騷擾內容',
                        '血腥、暴力、有害或危險內容': '血腥、暴力、有害或危險內容',
                        '違反廣告、或商業內容等定義': '違反廣告、或商業內容等定義',
                        '賭博、或博弈內容': '賭博、或博弈內容',
                        'other': '其他'
                    }},
                    inputPlaceholder: '選擇理由...',
                    showCancelButton: true,
                    confirmButtonText: '確定',
                    cancelButtonText: '取消',
                    reverseButtons: true,
                    preConfirm: () => {{
                        const selectedReason = Swal.getPopup().querySelector('select').value;
                        if (!selectedReason) {{
                            Swal.showValidationMessage('請選擇檢舉理由');
                            return false;
                        }}
                        return selectedReason;
                    }}
                }}).then((result) => {{
                    if (result.isConfirmed) {{
                        let reportReason = result.value;
                        if (reportReason === 'other') {{
                            Swal.fire({{
                                title: '請輸入檢舉原因',
                                input: 'textarea',
                                inputPlaceholder: '請輸入理由...',
                                showCancelButton: true,
                                confirmButtonText: '提交',
                                cancelButtonText: '取消',
                                inputValidator: (value) => {{
                                    if (!value) {{
                                        return '檢舉原因不能為空!';
                                    }}
                                }}
                            }}).then((reasonResult) => {{
                                if (reasonResult.isConfirmed) {{
                                    let detailedReason = reasonResult.value;
                                    __doPostBack('ReportComment', '{commentId}|' + detailedReason);
                                }}
                            }});
                        }} else {{
                            __doPostBack('ReportComment', '{commentId}|' + reportReason);
                        }}
                    }}
                }});
                </script>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
                LoadCoachDetails();
                BindClass();
                update_ProgressBar();
                bind_commend_score();
                bind_rp_comment();
            }

        }

    }
    protected void rp_comment_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    {
        //千萬別刪(刪除評論用)
    }
    private void DeleteComment(int ap_id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "DELETE FROM 完成預約評論表 where 預約編號=@ap_id";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("ap_id", ap_id);
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                string script = @"<script>
                            Swal.fire({
                            icon: 'error',
                            title: '刪除成功',
                            text: '已刪除評論',
                            showConfirmButton: false,
                            timer: 1500
                            });
                          </script>";

                Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
                update_ProgressBar();//更新評分進度條
                bind_commend_score();//顯示評分                               
                bind_rp_comment();//顯示評論
            }

        }
    }


    protected void rp_comment_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
    {
        // set current page startindex,max rows and rebind to false  
        DataPager2.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
        // Rebind the ListView1  
        if (btn_my_comment.CssClass == "btn-editclick")
        {
            //DataPager2.SetPageProperties(0, DataPager2.MaximumRows, true);
            bind_my_comment();
        }
        else if (btn_new_comment.CssClass == "btn-editclick")
        {
            //DataPager2.SetPageProperties(0, DataPager2.MaximumRows, true);
            bind_new_comment();
        }
        else if (btn_low_comment.CssClass == "btn-editclick")
        {
            //DataPager2.SetPageProperties(0, DataPager2.MaximumRows, true);
            bind_low_comment();
        }
        else if (btn_higher_comment.CssClass == "btn-editclick")
        {
            //DataPager2.SetPageProperties(0, DataPager2.MaximumRows, true);
            bind_higher_comment();
        }
        else
        {
            //DataPager2.SetPageProperties(0, DataPager2.MaximumRows, true);
            bind_rp_comment();
        }
    }
    private void bind_my_comment()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "SELECT * FROM 查看評論 where 健身教練編號 =@coach_num AND 使用者編號 =@u_id ORDER BY 評論日期 DESC, 評論時間 DESC";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@coach_num", coach_num);
            command.Parameters.AddWithValue("@u_id", User_id);
            DataTable dataTable = new DataTable();
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
            {
                dataAdapter.Fill(dataTable);
            }
            rp_comment.DataSource = dataTable;
            rp_comment.DataBind();
            connection.Close();
        }
    }
    private void bind_new_comment()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "SELECT * FROM 查看評論 where 健身教練編號 =@coach_num ORDER BY 評論日期 DESC, 評論時間 DESC";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@coach_num", coach_num);
            DataTable dataTable = new DataTable();
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
            {
                dataAdapter.Fill(dataTable);
            }
            rp_comment.DataSource = dataTable;
            rp_comment.DataBind();
            connection.Close();
        }
    }
    private void bind_higher_comment()
    {
        string conectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(conectionString))
        {
            string sql = "SELECT * FROM 查看評論 where 健身教練編號 =@coach_num ORDER BY 評分 DESC, 評論時間 DESC";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@coach_num", coach_num);
            // 使用 DataTable 來保存資料
            DataTable dataTable = new DataTable();
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
            {
                dataAdapter.Fill(dataTable);
            }
            rp_comment.DataSource = dataTable;
            rp_comment.DataBind();
            connection.Close();
        }
    }
    private void bind_low_comment()
    {
        string conectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(conectionString))
        {
            string sql = "SELECT * FROM 查看評論 where 健身教練編號 =@coach_num ORDER BY 評分 , 評論時間 DESC";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@coach_num", coach_num);
            // 使用 DataTable 來保存資料
            DataTable dataTable = new DataTable();
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
            {
                dataAdapter.Fill(dataTable);
            }
            rp_comment.DataSource = dataTable;
            rp_comment.DataBind();
            connection.Close();
        }
    }

    protected void btn_my_comment_Click(object sender, EventArgs e)
    {
        DataPager2.SetPageProperties(0, DataPager2.PageSize, true);
        btn_my_comment.CssClass = "btn btn-outline-primary mt-2 px-3 active";
        btn_new_comment.CssClass = "btn btn-outline-primary mt-2 px-3";
        btn_higher_comment.CssClass = "btn btn-outline-primary mt-2 px-3";
        btn_low_comment.CssClass = "btn btn-outline-primary mt-2 px-3";
        bind_my_comment();
        string script = "scrollToReviewSection();";
        ClientScript.RegisterStartupScript(this.GetType(), "ScrollToReview", script, true);
    }

    protected void btn_new_comment_Click(object sender, EventArgs e)
    {
        DataPager2.SetPageProperties(0, DataPager2.PageSize, true);
        btn_my_comment.CssClass = "btn btn-outline-primary mt-2 px-3";
        btn_new_comment.CssClass = "btn btn-outline-primary mt-2 px-3 active";
        btn_higher_comment.CssClass = "btn btn-outline-primary mt-2 px-3";
        btn_low_comment.CssClass = "btn btn-outline-primary mt-2 px-3";
        bind_new_comment();
        string script = "scrollToReviewSection();";
        ClientScript.RegisterStartupScript(this.GetType(), "ScrollToReview", script, true);
    }

    protected void btn_higher_comment_Click(object sender, EventArgs e)
    {
        DataPager2.SetPageProperties(0, DataPager2.PageSize, true);
        btn_my_comment.CssClass = "btn btn-outline-primary mt-2 px-3";
        btn_new_comment.CssClass = "btn btn-outline-primary mt-2 px-3";
        btn_higher_comment.CssClass = "btn btn-outline-primary mt-2 px-3 active";
        btn_low_comment.CssClass = "btn btn-outline-primary mt-2 px-3";
        bind_higher_comment();
        string script = "scrollToReviewSection();";
        ClientScript.RegisterStartupScript(this.GetType(), "ScrollToReview", script, true);
    }

    protected void btn_low_comment_Click(object sender, EventArgs e)
    {
        DataPager2.SetPageProperties(0, DataPager2.PageSize, true);
        btn_my_comment.CssClass = "btn btn-outline-primary mt-2 px-3";
        btn_new_comment.CssClass = "btn btn-outline-primary mt-2 px-3";
        btn_higher_comment.CssClass = "btn btn-outline-primary mt-2 px-3";
        btn_low_comment.CssClass = "btn btn-outline-primary mt-2 px-3 active";
        bind_low_comment();
        string script = "scrollToReviewSection();";
        ClientScript.RegisterStartupScript(this.GetType(), "ScrollToReview", script, true);
    }

    private void BindLikeBtn(ImageButton LikeBtn, string coachId, string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            LikeBtn.ImageUrl = "img/dislike2.png";
        }
        else
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT COUNT(*) FROM 教練被收藏 WHERE 健身教練編號=@likecoach_id AND 使用者編號=@likeuser_id";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@likecoach_id", coachId);
                command.Parameters.AddWithValue("@likeuser_id", userId);
                int count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    LikeBtn.ImageUrl = "img/like1.png";
                }
                else
                {
                    LikeBtn.ImageUrl = "img/dislike2.png";
                }
            }
        }
    }

    protected void LikeBtn_Click(object sender, ImageClickEventArgs e)
    {
        if (Session["User_id"] != null)
        {
            ImageButton LikeBtn = (ImageButton)sender;

            // 獲取教練編號
            var coachId = GetCoachIdFromLikeBtn(LikeBtn);


            if (LikeBtn.ImageUrl == "img/dislike2.png")
            {
                LikeBtn.ImageUrl = "img/like1.png";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "insert into 教練被收藏 (使用者編號,健身教練編號) values(@likeuser_id,@likecoach_id)";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@likeuser_id", User_id);
                    command.Parameters.AddWithValue("@likecoach_id", coach_num);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                LoadCoachDetails();
                BindClass();
                update_ProgressBar();
                bind_commend_score();
                bind_rp_comment();
            }
            else
            {
                LikeBtn.ImageUrl = "img/dislike2.png";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "delete from 教練被收藏 where 健身教練編號=@dislikecoach_id and 使用者編號=@dislikeuser_id";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@dislikecoach_id", coach_num);
                    command.Parameters.AddWithValue("@dislikeuser_id", User_id);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                LoadCoachDetails();
                BindClass();
                update_ProgressBar();
                bind_commend_score();
                bind_rp_comment();
            }
        }
        else {
            CheckLogin.CheckUserOrCoachLogin(this.Page, "User");
            LoadCoachDetails();
            BindClass();
            update_ProgressBar();
            bind_commend_score();
            bind_rp_comment();
        }
        
    }
    private string GetCoachIdFromLikeBtn(ImageButton LikeBtn)
    {
        return LikeBtn.CommandArgument;
    }

    private void ReportComment(int comment_id,string reason)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "Insert Into [評論檢舉] (評論編號,檢舉原因) values(@comment_id,@reason)";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@comment_id", comment_id);
            command.Parameters.AddWithValue("@reason", reason);
            command.ExecuteNonQuery();
        }
    }
}