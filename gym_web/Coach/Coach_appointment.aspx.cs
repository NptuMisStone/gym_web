using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;

public partial class Coach_Coach_appointment : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;

    public static string Coach_id, TD, Ap_People, Coach_Time;
    public static int Schedule_id;
    protected void Page_Load(object sender, EventArgs e)
    {
        Coach_id = Convert.ToString(Session["Coach_id"]);
        //驗證教練是否登入的類別函數
        CheckLogin.CheckUserOrCoachLogin(this.Page, "Coach");
        TD = DateTime.Now.ToString("yyyy-MM-dd");
        if (!IsPostBack)
        {
            BindTdData();
        }
    }
    protected void btnToday_Click(object sender, EventArgs e)
    {
        BindTdData();
        ResetButtonClasses();
        btnToday.CssClass = "course-btn active";
        Session["whatdata"] = "Today";
    }

    protected void btnFuture_Click(object sender, EventArgs e)
    {
        BindFTData();
        ResetButtonClasses();
        btnFuture.CssClass = "course-btn active";
        Session["whatdata"] = "Future";
    }

    protected void btnPast_Click(object sender, EventArgs e)
    {
        BindPSData();
        ResetButtonClasses();
        btnPast.CssClass = "course-btn active";
        Session["whatdata"] = "Past";
    }
    // 用來重置按鈕的 CssClass
    private void ResetButtonClasses()
    {
        btnToday.CssClass = "course-btn";
        btnFuture.CssClass = "course-btn";
        btnPast.CssClass = "course-btn";
    }
    private void BindTdData()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "Select * From 健身教練課表課程合併 Where 健身教練編號=@CoachID AND 日期 = @TD ORDER BY 開始時間";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CoachID", Coach_id);
            command.Parameters.AddWithValue("@TD", TD);
            SqlDataReader dataReader = command.ExecuteReader();
            RP.DataSource = null;
            if (dataReader.HasRows)
            {
                RP.DataSource = dataReader;
                RP.DataBind();
                lblNoCourses.Visible = false;
            }
            else
            {
                lblNoCourses.Visible = true;
                lblNoCourses.Text = "今日無安排課程";
                RP.DataSource = null; // 清空 Repeater
                RP.DataBind(); // 重新綁定空的資料來源，防止顯示舊資料
            }
            connection.Close();
        }
    }
    private void BindFTData()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "Select * From 健身教練課表課程合併 Where 健身教練編號=@CoachID AND 日期 > @TD ORDER BY 日期 , 開始時間 ";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CoachID", Coach_id);
            command.Parameters.AddWithValue("@TD", TD);
            SqlDataReader dataReader = command.ExecuteReader();
            RP.DataSource = null;
            if (dataReader.HasRows)
            {
                RP.DataSource = dataReader;
                RP.DataBind();
                lblNoCourses.Visible = false;
            }
            else
            {
                lblNoCourses.Visible = true;
                lblNoCourses.Text = "未來無安排課程";
                RP.DataSource = null; // 清空 Repeater
                RP.DataBind(); // 重新綁定空的資料來源，防止顯示舊資料
            }
            connection.Close();
        }
    }
    private void BindPSData()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "Select * From 健身教練課表課程合併 Where 健身教練編號=@CoachID AND 日期 < @TD ORDER BY 日期 DESC, 開始時間 ASC";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CoachID", Coach_id);
            command.Parameters.AddWithValue("@TD", TD);
            SqlDataReader dataReader = command.ExecuteReader();
            RP.DataSource = null;
            if (dataReader.HasRows)
            {
                RP.DataSource = dataReader;
                RP.DataBind();
                lblNoCourses.Visible = false;
            }
            else
            {
                lblNoCourses.Visible = true;
                lblNoCourses.Text = "過去無安排課程";
                RP.DataSource = null; // 清空 Repeater
                RP.DataBind(); // 重新綁定空的資料來源，防止顯示舊資料
            }
            connection.Close();
        }
    }

    protected void RP_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "AP")
        {
            Label whatPlacetype = (Label)e.Item.FindControl("whatPlacetype");
            Session["whatPlacetype"] = whatPlacetype.Text;
            Schedule_id = Convert.ToInt32(e.CommandArgument);
            ShowAP(Schedule_id);
            Panel1.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#" + Panel1.ClientID + "').modal('show');", true);
        }
    }

    private void ShowAP(int schedule_id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "SELECT * FROM [使用者預約-評論用] WHERE 課表編號 = @ScheduleID AND (預約狀態 = 1 OR 預約狀態 = 2)";
            connection.Open();

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@ScheduleID", SqlDbType.Int).Value = schedule_id;

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dataReader);
                        AP_Detail.DataSource = dt;
                        AP_Detail.DataBind();
                        lblNoData.Visible = false;
                    }
                    else
                    {
                        lblNoData.Text = "無資料";
                        lblNoData.Visible = true;
                        AP_Detail.DataSource = null;
                        AP_Detail.DataBind();
                    }
                }
            }
        }
    }



    private void CancelAP(string ap_id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "UPDATE 使用者預約 SET 預約狀態 = 5 WHERE 預約編號 = @AP_id";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@AP_id", ap_id);
            command.ExecuteReader();
            connection.Close();
        }
    }
    private void SearchApPeople(string s_id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "Select 預約人數 From 健身教練課表 Where 課表編號=@ScheduleID";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ScheduleID", s_id);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Ap_People = reader["預約人數"].ToString().Trim();
            }
            connection.Close();
        }
    }
    private void CancelApPeople(string s_id)
    {
        SearchApPeople(s_id);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "UPDATE 健身教練課表 SET 預約人數 = @AP_People WHERE 課表編號 = @ScheduleID";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@AP_People", int.Parse(Ap_People) - 1);
            command.Parameters.AddWithValue("@ScheduleID", s_id);
            command.ExecuteReader();
            connection.Close();
        }
    }


    private void FinishAP(string ap_id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "UPDATE 使用者預約 SET 預約狀態 = 2 WHERE 預約編號 = @AP_id";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@AP_id", ap_id);
            command.ExecuteReader();
            connection.Close();
        }
    }
    private void SearchCoachTime(string c_id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "Select 健身教練次數 From 健身教練資料 Where 健身教練編號=@CoachID";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CoachID", c_id);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Coach_Time = reader["健身教練次數"].ToString().Trim();
            }
            connection.Close();
        }
    }
    private void FinishCoachTime(string c_id)
    {
        SearchCoachTime(c_id);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {   
            string sql = "UPDATE 健身教練資料 SET 健身教練次數 = @c_time WHERE 健身教練編號 = @CoachID";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@c_time", int.Parse(Coach_Time) + 1);
            command.Parameters.AddWithValue("@CoachID", c_id);
            command.ExecuteReader();
            connection.Close();
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
            return "~/page/img/user.png"; // 替代圖片的路徑
        }
    }

    protected void AP_Detail_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label status = e.Item.FindControl("status") as Label;
        Button cbtn = e.Item.FindControl("btnCancel") as Button;
        Button fbtn = e.Item.FindControl("btnFinish") as Button;
        Label showplacenm= e.Item.FindControl("ap_detail_placeName_label") as Label;
        Label ap_detail_placeName = e.Item.FindControl("ap_detail_placeName") as Label;
        Label showuserplace = e.Item.FindControl("ap_detail_Userplace_label") as Label;
        Label ap_detail_Userplace = e.Item.FindControl("ap_detail_Userplace") as Label;
        HyperLink ap_detail_Userplace_map = e.Item.FindControl("ap_detail_Userplace_map") as HyperLink;

        int st = Convert.ToInt32(status.Text);
        if (st == 2)
        {
            cbtn.Visible=false;
            fbtn.Visible = false;
        }
        string whatdata = Convert.ToString(Session["whatdata"]);
        if (whatdata == "Future")
        {
            fbtn.Visible = false;
        }
        else
        {
            cbtn.Visible = false;
        }
        string whatPlacetype = Convert.ToString(Session["whatPlacetype"]);
        if (whatPlacetype == "2")
        {
            ap_detail_placeName.Visible = false;
            showplacenm.Visible = false;
            ap_detail_Userplace.Visible = true;
            showuserplace.Visible = true;
            ap_detail_Userplace_map.Visible = true;
            ap_detail_Userplace_map.NavigateUrl = "https://www.google.com.tw/maps/place/" + ap_detail_Userplace.Text;
        }
        else
        {
            ap_detail_placeName.Visible = true;
            showplacenm.Visible = true;
            ap_detail_Userplace.Visible = false;
            showuserplace.Visible = false;
            ap_detail_Userplace_map.Visible=false;
        }
    }

    protected void AP_Detail_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
        Button cbtn = e.Item.FindControl("btnCancel") as Button;
        Button fbtn = e.Item.FindControl("btnFinish") as Button;
        Label schedule_id = e.Item.FindControl("schedule_id") as Label;
        Label coach_id = e.Item.FindControl("coach_id") as Label;
        Label status = e.Item.FindControl("status") as Label;
        Label showplacenm = e.Item.FindControl("ap_detail_placeName_label") as Label;
        Label ap_detail_placeName = e.Item.FindControl("ap_detail_placeName") as Label;
        Label showuserplace = e.Item.FindControl("ap_detail_Userplace_label") as Label;
        Label ap_detail_Userplace = e.Item.FindControl("ap_detail_Userplace") as Label;
        HyperLink ap_detail_Userplace_map = e.Item.FindControl("ap_detail_Userplace_map") as HyperLink;
        schedule_id.Visible = false;
        coach_id.Visible = false;
        status.Visible = false;
        string whatdata = Convert.ToString(Session["whatdata"]);
        if (whatdata== "Future")
        {
            fbtn.Visible = false;
        }
        else {
            cbtn.Visible = false;
        }
        string whatPlacetype = Convert.ToString(Session["whatPlacetype"]);
        if (whatPlacetype == "2")
        {
            ap_detail_placeName.Visible = false;
            showplacenm.Visible = false;
            ap_detail_Userplace.Visible = true;
            showuserplace.Visible = true;
            ap_detail_Userplace_map.Visible=true;
            ap_detail_Userplace_map.NavigateUrl = "https://www.google.com.tw/maps/place/" + ap_detail_Userplace.Text;
        }
        else
        {
            ap_detail_placeName.Visible = true;
            showplacenm.Visible = true;
            ap_detail_Userplace.Visible = false;
            showuserplace.Visible = false;
            ap_detail_Userplace_map.Visible=false;
        }
        
    }

    protected void AP_Detail_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Cancel")
        {
            string ap_id = e.CommandArgument.ToString();
            Label schedule_id = e.Item.FindControl("schedule_id") as Label;
            string s_id = schedule_id.Text;
            if (IsCancelable(s_id))
            {
                NotifyUsersAboutCancellation(ap_id);//寄信
                CancelAP(ap_id);/*預約狀態更改*/
                CancelApPeople(s_id);/*預約人數更改*/
                switch (Session["whatdata"].ToString())
                {
                    case "Today":
                        BindTdData();
                        break;
                    case "Future":
                        BindFTData();
                        break;
                    case "Past":
                        BindPSData();
                        break;
                }

                ShowAP(Schedule_id);
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#" + Panel1.ClientID + "').modal('show');", true);

            }
            else {
                string script = @"<script>
                            Swal.fire({
                            icon: 'error',
                            title: '取消失敗',
                            text: '需於課程開始 24 小時前取消',
                            showConfirmButton: false,
                            timer: 1500
                            });
                          </script>";

                Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
            }

        }
        else if (e.CommandName == "Finish")
        {
            string ap_id = e.CommandArgument.ToString();
            Label coach_id = e.Item.FindControl("coach_id") as Label;
            string c_id = coach_id.Text;
            FinishAP(ap_id);
            FinishCoachTime(c_id);
            ShowAP(Schedule_id);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#" + Panel1.ClientID + "').modal('show');", true);
        }
    }
    protected void NotifyUsersAboutCancellation(string APId)
    {
        string query = "SELECT 使用者郵件,課程名稱,日期,開始時間,結束時間,健身教練姓名 " +
                       "FROM [使用者預約-有預約的] WHERE 預約編號 = @APId ";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@APId", APId); // 設定課表編號參數
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string userEmail_mail = reader["使用者郵件"].ToString();
                        string courseName_mail = reader["課程名稱"].ToString();
                        DateTime courseDate = (DateTime)reader["日期"];
                        string courseDate_mail = courseDate.ToString("yyyy-MM-dd");
                        string startTime_mail = reader["開始時間"].ToString();
                        string endTime_mail = reader["結束時間"].ToString();
                        string coachName_mail = reader["健身教練姓名"].ToString();

                        // 發送通知給使用者
                        SendCancellationNotification(userEmail_mail, courseDate_mail, startTime_mail, endTime_mail, courseName_mail, coachName_mail);
                    }
                }
            }
        }
    }

    private void SendCancellationNotification(string userEmail, string courseDate, string startTime, string endTime, string courseName, string coachName)
    {
        string GoogleID = "NptuMisStone@gmail.com"; // Google 發信帳號
        string TempPwd = "lgtb rhoq irjc flyi"; // 應用程式密碼

        string SmtpServer = "smtp.gmail.com";
        int SmtpPort = 587;
        MailMessage mms = new MailMessage();
        mms.From = new MailAddress(GoogleID);
        mms.Subject = "【屏大Fit-健身預約系統】課程取消通知";
        mms.Body = "<p>您好，</p>" +
            "<p>我們遺憾地通知您，您所預約的課程已被教練取消。</p>" +
            "<p>課程詳細資訊如下：</p>" +
            $"<p>課程名稱：{courseName}</p>" +
            $"<p>課程日期：{courseDate}</p>" +
            $"<p>課程時間：{startTime} ~ {endTime}</p>" +
            $"<p>教練名稱：{coachName}</p>" +
            "<p>若有任何問題，請聯繫教練或客服人員。</p>" +
            "<p>屏大Fit 團隊</p>";
        mms.IsBodyHtml = true; // 確保內容使用 HTML 格式
        mms.SubjectEncoding = System.Text.Encoding.UTF8;
        mms.To.Add(new MailAddress(userEmail));
        using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
        {
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(GoogleID, TempPwd); // 寄信帳密 
            try
            {
                client.Send(mms);// 寄出信件
                Debug.WriteLine("郵件已成功發送！");
            }
            catch (SmtpException ex)
            {
                Debug.WriteLine($"郵件發送失敗：{ex.Message}");
            }
        }
        Debug.WriteLine("已寄出取消通知");

    }
    protected string GetGenderText(object genderValue)
    {
        if (genderValue != null)
        {
            switch (genderValue.ToString())
            {
                case "1":
                    return "男性";
                case "2":
                    return "女性";
                case "3":
                    return "不願透露";
                default:
                    return "未知";
            }
        }
        return "未知";
    }
    public bool IsCancelable(string scheduleId)
    {
        string query = "SELECT 開始時間, 日期 FROM 健身教練課表 WHERE 課表編號 = @ScheduleId";
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ScheduleId", scheduleId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // 確保正確解析日期和時間
                            DateTime classStart;
                            if (reader["日期"] != DBNull.Value && reader["開始時間"] != DBNull.Value)
                            {
                                DateTime classDate = Convert.ToDateTime(reader["日期"]);
                                TimeSpan classStartTime = TimeSpan.Parse(reader["開始時間"].ToString());
                                classStart = classDate.Add(classStartTime);
                            }
                            else
                            {
                                // 如果日期或時間為空，返回不可刪除
                                return false;
                            }

                            DateTime now = DateTime.Now;

                            // 計算時間差
                            TimeSpan duration = classStart - now;

                            // 返回是否超過 24 小時
                            return duration.TotalHours >= 24;
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error checking cancelability: " + e.Message);
        }

        return false; // 如果查詢失敗或其他錯誤，預設不可取消
    }
}