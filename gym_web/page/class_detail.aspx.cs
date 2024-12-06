using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Security.Policy;
using System.Diagnostics;

public partial class page_class_detail : System.Web.UI.Page
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    public static string Class_id, Time_id;
    public static DateTime Date_change, date;
    public static string User_id, Coach_id, Course_id, Schedule_id, Ap_date, Ap_starttime, Ap_endtime;
    public static int Ap_people, checkPlace;
    protected void Page_Load(object sender, EventArgs e)
    {
        Class_id = Convert.ToString(Session["Class_id"]);
        if (!IsPostBack)
        {
            User_id = Convert.ToString(Session["User_id"]);
            Date_change = DateTime.Today;
            BindClass();
            Bind_Time_Info();
            GetAddress();
            GetDate();
        }
    }
    private void BindClass()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM [健身教練課程-有排課的] WHERE 課程編號 = @Class_id";
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Class_id", Class_id);
            SqlDataReader reader = command.ExecuteReader();
            rp_class.DataSource = reader;
            rp_class.DataBind();
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
    protected void rp_class_ItemCommand(object sender, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "select_coach")
        {
            // 取得教練編號並存入 Session
            Session["coach_num"] = Convert.ToInt32(e.CommandArgument);
            // 跳轉至教練詳細頁面
            Response.Redirect("coach_detail.aspx");
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
                EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

                // Get the JPG codec info
                ImageCodecInfo jpgCodec = ImageCodecInfo.GetImageEncoders().First(codec => codec.MimeType == "image/jpeg");

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
    public void GetAddress()
    {
        // 示例的資料庫操作代碼
        string address = "";
        string location_name = "";
        string query = "SELECT [顯示地點名稱],[顯示地點地址] FROM [健身教練課程-有排課的] WHERE [課程編號] = @Class_id";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Class_id", Class_id);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read()) 
            {
                location_name = reader["顯示地點名稱"].ToString();
                address = reader["顯示地點地址"].ToString();
                Course_all_Address.Text = location_name;
                Course_all_mapADD.Text = address;
                map.NavigateUrl = "https://www.google.com.tw/maps/place/" + address;
                //search是搜尋，place是直接顯示，但如果place沒找到地址，就沒有東西
            }
        }
    }
    private void Bind_Time_Info()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query2 = "SELECT * FROM 健身教練課表課程合併 WHERE 課程編號 = @Class_id AND 日期 = @Date ORDER BY [開始時間] ";
            connection.Open();
            SqlCommand command = new SqlCommand(query2, connection);
            command.Parameters.AddWithValue("@Class_id", Class_id);
            command.Parameters.AddWithValue("@Date", Date_change);
            SqlDataReader reader = command.ExecuteReader();
            TimeRepeater.DataSource = reader;
            TimeRepeater.DataBind();
            if (TimeRepeater.Items.Count > 0) { noshow.Visible = false; }
            else { noshow.Visible = true; }
            connection.Close();
        }
        foreach (RepeaterItem item in TimeRepeater.Items)
        {
            Label courseTimeLabel = (Label)item.FindControl("Course_all_stTime");
            Label courseDateLabel = (Label)item.FindControl("Course_all_date");
            Button appointmentButton = (Button)item.FindControl("Appointment_btn");
            Label ap_people = (Label)item.FindControl("ap_all_people");
            Label course_people = (Label)item.FindControl("course_all_people");
            DateTime courseDate = DateTime.Parse(courseDateLabel.Text);
            if (int.Parse(ap_people.Text.ToString().Trim()) >= int.Parse(course_people.Text.ToString().Trim()))
            {
                appointmentButton.Enabled = false;
                appointmentButton.Text = "額滿";
            }
            if (courseDate == DateTime.Today)
            {
                if (courseTimeLabel != null && appointmentButton != null)
                {
                    DateTime courseTime = DateTime.Parse(courseTimeLabel.Text);
                    if (courseTime <= DateTime.Now)
                    {
                        appointmentButton.Enabled = false;
                        appointmentButton.Text = "已逾時";
                    }
                }
            }
        }
    }
    protected void Course_date_choose_SelectionChanged(object sender, EventArgs e)
    {
        Date_change = Convert.ToDateTime(Course_date_choose.SelectedDate);
        Bind_Time_Info();
    }


    protected void Course_date_choose_DayRender(object sender, DayRenderEventArgs e)
    {
        DateTime today = DateTime.Today;
        List<DateTime> courseDates = GetDate();
        // 禁用今天之前的日期
        if (e.Day.Date < today)
        {
            e.Cell.ForeColor = System.Drawing.Color.Gray; // 文字顏色
            e.Cell.BackColor = System.Drawing.Color.LightGray; // 背景顏色
            e.Cell.Attributes.Add("onclick", "return false;"); // 禁用
        }
        else if (courseDates.Contains(e.Day.Date)) // 如果資料庫裡的日期有資料
        {
            e.Cell.ForeColor = System.Drawing.Color.Black; // 文字顏色
            e.Cell.BackColor = System.Drawing.Color.Green; // 背景顏色
            e.Cell.Attributes.Add("onclick", "ShowProgressBar(); ");//執行進度條
        }
        
    }
    private List<DateTime> GetDate()
    {
        List<DateTime> courseDates = new List<DateTime>();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT DISTINCT 日期 FROM 健身教練課表課程合併 WHERE 課程編號 = @Class_id ";
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Class_id", Class_id);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                date = reader.GetDateTime(0);
                courseDates.Add(date);
            }
            
        }
        return courseDates;
    }

    protected void TimeRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "ap")
        {
            if (Session["User_id"] == null)
            {
               CheckLogin.CheckUserOrCoachLogin(this.Page, "User");
            }
            else
            {
                Schedule_id = Convert.ToString(e.CommandArgument);
                User_id = Convert.ToString(Session["User_id"]);
                ap_location.Enabled = false;
                rfvlocation.Enabled = false;
                ShowDetail();
                GetId();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#" + AP_Panel.ClientID + "').modal('show');", true);
            }
        }
    }
    private void ShowDetail()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM 健身教練課表課程合併 WHERE 課表編號 = @Schedule_id ";
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Schedule_id", Schedule_id);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                ap_course_name.Text = reader["課程名稱"].ToString();
                ap_course_time.Text = reader["課程時間長度"].ToString();
                ap_add_name.Text = reader["地點名稱"].ToString();
                ap_add_city.Text = reader["縣市"].ToString();
                ap_add_area.Text = reader["行政區"].ToString();
                ap_course_add.Text = reader["地點地址"].ToString();
                ap_course_date.Text = Convert.ToDateTime(reader["日期"]).ToString("yyyy/MM/dd");
                ap_course_stTime.Text = reader["開始時間"].ToString();
                ap_course_edTime.Text = reader["結束時間"].ToString();
                home_city.Text = reader["縣市"].ToString();
                home_area.Text = reader["行政區"].ToString();
                checkPlace = int.Parse(reader["地點類型"].ToString().Trim());

                if (checkPlace == 2)
                {
                    ap_location.Enabled = true;
                    rfvlocation.Enabled = true;
                    ADD_Panel.Visible = false;
                    home_city.Visible = true;
                    home_area.Visible = true;
                }

                Ap_date = Convert.ToDateTime(reader["日期"]).ToString("yyyy/MM/dd");
                Ap_starttime = reader["開始時間"].ToString();
                Ap_endtime = reader["結束時間"].ToString();
            }
            connection.Close();
        }
    }
    private void GetId()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM 健身教練課表課程合併 WHERE 課表編號 = @Schedule_id ";
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Schedule_id", Schedule_id);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Coach_id = reader["健身教練編號"].ToString();
                Course_id = reader["課程編號"].ToString();
            }
            connection.Close();
        }
    }
    protected void ap_btn_Click(object sender, EventArgs e)
    {
        if (CheckAP())
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "Insert Into 使用者預約 (使用者編號,健身教練編號,課程編號,課表編號,預約狀態,備註,客戶到府地址)" +
                    "values(@u_id,@c_id,@course_id,@schedule_id,@status,@text,@location)";
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@u_id", User_id);
                command.Parameters.AddWithValue("@c_id", Coach_id);
                command.Parameters.AddWithValue("@course_id", Course_id);
                command.Parameters.AddWithValue("@schedule_id", Schedule_id);
                command.Parameters.AddWithValue("@status", 1);
                command.Parameters.AddWithValue("@text", ap_text.Text);
                command.Parameters.AddWithValue("@location", ap_location.Text);
                command.ExecuteReader();
                connection.Close();
                UpdatePeople();
                string script = @"<script>
                Swal.fire({
                icon: ""success"",
                title: ""預約成功！"",
                showConfirmButton: false,
                timer: 1500
                });

                setTimeout(function () {
                window.location.href = '../User/User_appointment_record.aspx';
                }, 1500);
                </script>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
            }
        }
    }
    private void UpdatePeople()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "UPDATE 健身教練課表 SET 預約人數 = @ap_people WHERE 課表編號 = @Schedule_id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                Search_ap_people();
                command.Parameters.AddWithValue("@ap_people", Ap_people + 1);
                command.Parameters.AddWithValue("@Schedule_id", Schedule_id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
    private void Search_ap_people()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT 預約人數 FROM 健身教練課表 WHERE 課表編號 = @Schedule_id ";
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Schedule_id", Schedule_id);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Ap_people = int.Parse(reader["預約人數"].ToString().Trim());
            }
            connection.Close();
        }
    }
    private bool CheckAP()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string queryCheck = @"
                SELECT 開始時間, 結束時間 
                FROM [使用者預約-有預約的] 
                WHERE 日期 = @Ap_date  
                AND 預約狀態 = 1 AND 使用者編號 = @User_id";

            using (SqlCommand checkCommand = new SqlCommand(queryCheck, connection))
            {
                checkCommand.Parameters.AddWithValue("Ap_date", Ap_date);
                checkCommand.Parameters.AddWithValue("@User_id", User_id);
                using (SqlDataReader reader = checkCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TimeSpan scheduledStartTime = TimeSpan.Parse(reader["開始時間"].ToString());
                        TimeSpan scheduledEndTime = TimeSpan.Parse(reader["結束時間"].ToString());

                        TimeSpan selectedStartTime = TimeSpan.Parse(Ap_starttime);
                        TimeSpan selectedEndTime = TimeSpan.Parse(Ap_endtime);

                        if ((selectedStartTime >= scheduledStartTime && selectedStartTime < scheduledEndTime) ||
                            (selectedEndTime > scheduledStartTime && selectedEndTime <= scheduledEndTime) ||
                            (selectedStartTime <= scheduledStartTime && selectedEndTime >= scheduledEndTime))
                        {
                            string script2 = @"<script>
                                            Swal.fire({
                                              icon: 'error',
                                              title: '預約時段衝突！',
                                              confirmButtonText: '確定',
                                            }).then((result) => {
                                              if (result.isConfirmed) {
                                                 window.location.href = '../User/User_appointment_record.aspx';
                                              }
                                            });
                                            </script>";

                            Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script2, false);
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
    private string GetClassIdFromLikeBtn(ImageButton LikeBtn)
    {
        return LikeBtn.CommandArgument;
    }
    private void BindLikeBtn(ImageButton LikeBtn, string classId, string userId)
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
                string sql = "SELECT COUNT(*) FROM 課程被收藏 WHERE 課程編號=@likeclass_id AND 使用者編號=@likeuser_id";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@likeclass_id", classId);
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
        ImageButton LikeBtn = (ImageButton)sender;

        // 獲取課程編號
        var classId = GetClassIdFromLikeBtn(LikeBtn);

        CheckLogin.CheckUserOrCoachLogin(this.Page, "User");

        if (LikeBtn.ImageUrl == "img/dislike2.png")
        {
            LikeBtn.ImageUrl = "img/like1.png";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "insert into 課程被收藏 (使用者編號,課程編號) values(@likeuser_id,@likeclass_id)";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@likeuser_id", User_id);
                command.Parameters.AddWithValue("@likeclass_id", classId);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        else
        {
            LikeBtn.ImageUrl = "img/dislike2.png";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "delete from 課程被收藏 where 課程編號=@dislikeclass_id and 使用者編號=@dislikeuser_id";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@dislikeclass_id", classId);
                command.Parameters.AddWithValue("@dislikeuser_id", User_id);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }

    protected void rp_class_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {

            // 獲取教練編號
            var classId = DataBinder.Eval(e.Item.DataItem, "課程編號").ToString();

            // 獲取 LikeBtn 控制項
            ImageButton LikeBtn = (ImageButton)e.Item.FindControl("LikeBtn");

            // 獲取使用者編號
            var userId = Session["User_id"]?.ToString();

            // 綁定 LikeBtn 的狀態
            BindLikeBtn(LikeBtn, classId, userId);
        }
    }
}