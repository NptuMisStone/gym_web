using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using System.Collections;
using System.Web.UI.WebControls.WebParts;
public partial class Coach_Coach_schedule : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;

    public static string Coach_id;
    public static int Course_time,NewCourse_time;
    public static string Course_id, Course_starttime, Course_endtime, Course_week, Schedule_id, NewStarttime, NewEndtime, NewCourse_id;
    public static DateTime Course_date;
    private const string SelectedDatesKey = "SelectedDates";
    public static string tbCourseWeek;

    protected void Page_Load(object sender, EventArgs e)
    {
        Coach_id = Convert.ToString(Session["Coach_id"]);
        //驗證教練是否登入的類別函數
        CoachHelper.CheckLogin(this);

        if (!IsPostBack)
        {
            BindCourse();
            ClearCalendar();
            UpdateSelectedDatesLabel();
            txtDate.Text = DateTime.Today.ToString();
            BindCourseData();
        }
        
    }
    private void BindCourse()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "Select * From 健身教練課程 Where 健身教練編號=@CoachID ";
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@CoachID", Coach_id);

                SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.SequentialAccess);
                gv_course.DataSource = dataReader;
                gv_course.DataBind();
                connection.Close();
            }
        }
        catch { }

    }
    protected void GetCourseInfo(object sender, GridViewCommandEventArgs e)
    {
        arrange_date.Visible = true;

        if (e.CommandName == "Select")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gv_course.Rows[rowIndex];
            string courseName = row.Cells[1].Text;
            tbCourseName.Text = courseName;
            int coursetime = int.Parse(row.Cells[3].Text);
            Course_time = coursetime;
            Course_id = row.Cells[0].Text;
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
            return "img/team-1.jpg"; // 替代圖片的路徑
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        tbCourseName.Text = string.Empty;
        ClearCalendar();
        UpdateSelectedDatesLabel();
        tbCourseStartTime.Text = string.Empty;
        tbCourseEndTime.Text = string.Empty;
        tbCourseWeek = string.Empty;
        arrange_date.Visible = false;

    }
    protected void btnAddSchedule_Click(object sender, EventArgs e)
    {
        // 已選擇的日期
        List<DateTime> selectedDates = GetSelectedDates();

        // 檢查是否有選
        if (selectedDates.Count > 0)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach (DateTime date in selectedDates)
                {
                    // 查询同一天已安排的课程
                    string queryCheck = @"
                    SELECT 開始時間, 結束時間 
                    FROM [健身教練課表課程-判斷課程衝突用] 
                    WHERE 日期 = @course_date
                    AND 健身教練編號 = @coach_id ";

                    using (SqlCommand checkCommand = new SqlCommand(queryCheck, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@course_date", date);
                        checkCommand.Parameters.AddWithValue("coach_id", Coach_id);
                        using (SqlDataReader reader = checkCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TimeSpan scheduledStartTime = TimeSpan.Parse(reader["開始時間"].ToString());
                                TimeSpan scheduledEndTime = TimeSpan.Parse(reader["結束時間"].ToString());

                                TimeSpan selectedStartTime = TimeSpan.Parse(Course_starttime);
                                TimeSpan selectedEndTime = TimeSpan.Parse(Course_endtime);

                                if ((selectedStartTime >= scheduledStartTime && selectedStartTime < scheduledEndTime) ||
                                    (selectedEndTime > scheduledStartTime && selectedEndTime <= scheduledEndTime) ||
                                    (selectedStartTime <= scheduledStartTime && selectedEndTime >= scheduledEndTime))
                                {
                                    string script2 = @"<script>
                                                Swal.fire({
                                                icon: 'error',
                                                title: '時間衝突',
                                                text: '所選時間與現有時間重疊',
                                                confirmButtonText: '確定'
                                                });
                                              </script>";

                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script2, false);
                                    return;
                                }
                            }
                        }
                    }
                }
                    using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;

                    foreach (DateTime date in selectedDates)
                    {
                        Course_date = date;
                        CultureInfo cultureInfo = new CultureInfo("zh-TW");
                        string dayOfWeek = date.ToString("dddd", cultureInfo);
                        tbCourseWeek = dayOfWeek;
                        Course_week = tbCourseWeek;

                        string query = "insert into 健身教練課表 (課程編號, 日期, 開始時間, 結束時間, 星期幾, 預約人數 )" +
                                       "values (@course_id, @course_date, @course_starttime, @course_endtime, @course_week, @ap_people)";
                        command.CommandText = query;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@course_id", Course_id);
                        command.Parameters.AddWithValue("@course_date", Course_date);
                        command.Parameters.AddWithValue("@course_starttime", Course_starttime);
                        command.Parameters.AddWithValue("@course_endtime", Course_endtime);
                        command.Parameters.AddWithValue("@course_week", Course_week);
                        command.Parameters.AddWithValue("@ap_people", 0);
                        command.ExecuteNonQuery();
                    }
                }
            }

            tbCourseName.Text = string.Empty;
            tbCourseStartTime.Text = string.Empty;
            tbCourseEndTime.Text = string.Empty;
            tbCourseWeek = string.Empty;
            ClearCalendar();
            UpdateSelectedDatesLabel();
            arrange_date.Visible = false;
            txtDate.Text = DateTime.Today.ToString();
            BindCourseData();
            string script = @"<script>
                            Swal.fire({
                            icon: 'success',
                            title: '新增成功',
                            text: '班表已新增',
                            showConfirmButton: false,
                            timer: 1500
                            });
                          </script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
        }
        else
        {
            string script = @"<script>
                            Swal.fire({
                            icon: 'error',
                            title: '請選擇日期',
                            confirmButtonText: '確定'
                            });
                          </script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
        }
    }
    protected void gv_course_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        //隱藏課程編號，先bind完再隱藏才有資料
    }
    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
        List<DateTime> selectedDates = GetSelectedDates();

        // 選擇的日期
        DateTime[] newSelection = Calendar1.SelectedDates.Cast<DateTime>().ToArray();

        // 更新
        foreach (var date in newSelection)
        {
            if (selectedDates.Contains(date))
            {
                selectedDates.Remove(date);
            }
            else
            {
                selectedDates.Add(date);
            }
        }

        Session[SelectedDatesKey] = selectedDates;
        UpdateSelectedDatesLabel();

    }

    private void UpdateSelectedDatesLabel()
    {
        List<DateTime> selectedDates = GetSelectedDates();

        if (selectedDates.Any())
        {
            SelectedDatesLabel.Text = "選擇的日期：" + string.Join(", ", selectedDates.Select(d => d.ToShortDateString()));
        }
        else
        {
            SelectedDatesLabel.Text = "選擇的日期：未選擇";
        }

        // 清除所有已选择的日期样式
        Calendar1.SelectedDates.Clear();
    }

    private List<DateTime> GetSelectedDates()
    {
        if (Session[SelectedDatesKey] is List<DateTime> dates)
        {
            return dates;
        }
        else
        {
            return new List<DateTime>();
        }
    }
    private void ClearCalendar()
    {
        Calendar1.SelectedDates.Clear();
        Session[SelectedDatesKey] = new List<DateTime>();
    }
    protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
    {
        List<DateTime> selectedDates = GetSelectedDates();
        DateTime today = DateTime.Today;

        // 禁用今天之前的日期
        if (e.Day.Date < today)
        {
            e.Cell.ForeColor = System.Drawing.Color.Gray; // 文字顏色
            e.Cell.BackColor = System.Drawing.Color.LightGray; // 背景顏色
            e.Cell.Attributes.Add("onclick", "return false;"); // 禁用
        }
        else
        {
            if (selectedDates.Contains(e.Day.Date))
            {
                e.Cell.BackColor = System.Drawing.Color.Green; // 背景顏色
                e.Cell.ForeColor = System.Drawing.Color.White;  // 文字顏色
            }
        }
    }

    protected void tbCourseStartTime_TextChanged(object sender, EventArgs e)
    {
        if (DateTime.TryParse(tbCourseStartTime.Text, out DateTime startDateTime))
        {
            TimeSpan courseDuration = TimeSpan.FromMinutes(Course_time);

            DateTime endDateTime = startDateTime.Add(courseDuration);

            Course_starttime = startDateTime.ToString("HH:mm").Trim();
            Course_endtime = endDateTime.ToString("HH:mm").Trim();
            var cultureInfo = new System.Globalization.CultureInfo("en-US");
            tbCourseEndTime.Text = endDateTime.ToString("tt hh:mm", cultureInfo);
        }
    }
    protected void btnGetSchedule_Click(object sender, EventArgs e)
    {
        BindCourseData();
    }
    private void BindCourseData()
    {
        lblMessage.Text = string.Empty;
        DateTime selectedDate;
        if (DateTime.TryParse(txtDate.Text, out selectedDate))
        {
            DayOfWeek dayOfWeek = selectedDate.DayOfWeek;
            int newday = dayOfWeek == DayOfWeek.Sunday ? -6 : -(int)dayOfWeek + 1;//如果是星期日設-6，不是的話依照星期幾+1(向前移動幾天得到星期一)
            DateTime startOfWeek = selectedDate.AddDays(newday);  // 星期一開始
            DateTime endOfWeek = startOfWeek.AddDays(6);  // 星期日结束
            lb1date.Text = startOfWeek.ToString("yyyy/MM/dd") + "~" + endOfWeek.ToString("yyyy/MM/dd");
            string query = @"SELECT * FROM [健身教練課表課程合併]
                         WHERE [日期] BETWEEN @StartOfWeek AND @EndOfWeek 
                           AND [健身教練編號] = @Coach_id AND [星期幾] = @week
                         ORDER BY  [開始時間]";
            List<string> weeklist = new List<string> { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日" };
            bool hasData = false;
            foreach (string week in weeklist)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {

                        cmd.Parameters.AddWithValue("@Coach_id", Coach_id);
                        cmd.Parameters.AddWithValue("@StartOfWeek", startOfWeek);
                        cmd.Parameters.AddWithValue("@EndOfWeek", endOfWeek);
                        cmd.Parameters.AddWithValue("@week", week);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        switch (week)
                        {
                            case "星期一":
                                RepeaterMonday.DataSource = reader;
                                RepeaterMonday.DataBind();
                                if (RepeaterMonday.Items.Count > 0) hasData = true;
                                break;
                            case "星期二":
                                RepeaterTuesday.DataSource = reader;
                                RepeaterTuesday.DataBind();
                                if (RepeaterTuesday.Items.Count > 0) hasData = true;
                                break;
                            case "星期三":
                                RepeaterWednesday.DataSource = reader;
                                RepeaterWednesday.DataBind();
                                if (RepeaterWednesday.Items.Count > 0) hasData = true;
                                break;
                            case "星期四":
                                RepeaterThursday.DataSource = reader;
                                RepeaterThursday.DataBind();
                                if (RepeaterThursday.Items.Count > 0) hasData = true;
                                break;
                            case "星期五":
                                RepeaterFriday.DataSource = reader;
                                RepeaterFriday.DataBind();
                                if (RepeaterFriday.Items.Count > 0) hasData = true;
                                break;
                            case "星期六":
                                RepeaterSaturday.DataSource = reader;
                                RepeaterSaturday.DataBind();
                                if (RepeaterSaturday.Items.Count > 0) hasData = true;
                                break;
                            case "星期日":
                                RepeaterSunday.DataSource = reader;
                                RepeaterSunday.DataBind();
                                if (RepeaterSunday.Items.Count > 0) hasData = true;
                                break;
                        }
                    }
                }
            }
            if (!hasData)
            {
                lblMessage.Text = "無課表";
            }
        }
    }

    protected void RepeaterWeekInfo_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "ShowId")
        {
            // 取得課表編號
            Schedule_id = e.CommandArgument.ToString();
            showSchedule();
        }
    }
    private void showSchedule()
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#" + SchedulePanel.ClientID + "').modal('show');", true);
        SchedulePanel.Visible = true;
        string qry = @"SELECT * FROM [健身教練課表課程合併] WHERE [課表編號]=@Schedule_id ";
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(qry, conn))
            {
                command.Parameters.AddWithValue("@Schedule_id", Schedule_id);
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    NewCourse_id = reader["課程編號"].ToString();
                    var cultureInfo = new System.Globalization.CultureInfo("en-US");
                    NewCourse_time =int.Parse(reader["課程時間長度"].ToString());
                    detaildate.Text = Convert.ToDateTime(reader["日期"]).ToString("yyyy/MM/dd");
                    detailstarttime.Text = Convert.ToDateTime(reader["開始時間"]).ToString("HH:mm");
                    detailendtime.Text= Convert.ToDateTime(reader["結束時間"]).ToString("tt hh:mm", cultureInfo);
                    BindDropDownList();
                    BindDetailCourse();
                }

                conn.Close();
            }
        }
    }

    protected void Schedule_delete_Click(object sender, EventArgs e)
    {
        string qry = @"DELETE FROM 健身教練課表 WHERE 課表編號 =@Schedule_id ";
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(qry, conn))
            {
                command.Parameters.AddWithValue("@Schedule_id", Schedule_id);
                conn.Open();
                command.ExecuteReader();
                conn.Close();
                txtDate.Text = DateTime.Today.ToString();
                BindCourseData();
                SchedulePanel.Visible = false;
                string script = @"<script>
                            Swal.fire({
                            icon: 'error',
                            title: '刪除成功',
                            text: '班表已刪除',
                            showConfirmButton: false,
                            timer: 1500
                            });
                          </script>";

                Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);

            }
        }
        
    }

    protected void Schedule_cancel_Click(object sender, EventArgs e)
    {
        SchedulePanel.Visible = false;
    }

    protected void Schedule_save_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(detaildate.Text))
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string queryCheck = @"
                SELECT 開始時間, 結束時間 
                FROM [健身教練課表課程-判斷課程衝突用] 
                WHERE 日期 = @course_date
                AND 健身教練編號 = @coach_id 
                AND 課表編號 != @schedule_id";

                using (SqlCommand checkCommand = new SqlCommand(queryCheck, conn))
                {
                    checkCommand.Parameters.AddWithValue("@course_date", detaildate.Text);
                    checkCommand.Parameters.AddWithValue("@coach_id", Coach_id);
                    checkCommand.Parameters.AddWithValue("@schedule_id", Schedule_id);
                    using (SqlDataReader reader = checkCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TimeSpan scheduledStartTime = TimeSpan.Parse(reader["開始時間"].ToString());
                            TimeSpan scheduledEndTime = TimeSpan.Parse(reader["結束時間"].ToString());
                            
                            TimeSpan selectedStartTime = TimeSpan.Parse(NewStarttime);
                            TimeSpan selectedEndTime = TimeSpan.Parse(NewEndtime);
                            if ((selectedStartTime >= scheduledStartTime && selectedStartTime < scheduledEndTime) ||
                                (selectedEndTime > scheduledStartTime && selectedEndTime <= scheduledEndTime) ||
                                (selectedStartTime <= scheduledStartTime && selectedEndTime >= scheduledEndTime))
                            {
                                string script2 = @"<script>
                                        Swal.fire({
                                        icon: 'error',
                                        title: '時間衝突',
                                        text: '所選時間與現有時間重疊',
                                        confirmButtonText: '確定'
                                        });
                                      </script>";

                                Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script2, false);
                                return;
                            }
                            
                        }
                    }
                }
                string updateQuery = "UPDATE [健身教練課表] SET " +
                                     " [日期] = @SchDate, [星期幾] = @SchWeek, " +
                                     "[開始時間] = @SchSTime, [結束時間] = @SchETime, " +
                                     "[課程編號] = @Course_id " +
                                     "WHERE [課表編號] = @Schedule_id";
                
                using (SqlCommand command = new SqlCommand(updateQuery, conn))
                {
                    DateTime sdate;
                    if (DateTime.TryParse(detaildate.Text, out sdate))
                    {
                        CultureInfo cultureInfo = new CultureInfo("zh-TW");
                        string detailweek = sdate.ToString("dddd", cultureInfo);
                        command.Parameters.AddWithValue("@SchDate", detaildate.Text);
                        command.Parameters.AddWithValue("@SchWeek", detailweek);
                        command.Parameters.AddWithValue("@SchSTime", NewStarttime);
                        command.Parameters.AddWithValue("@SchETime", NewEndtime);
                        command.Parameters.AddWithValue("@Schedule_id", Schedule_id);
                        command.Parameters.AddWithValue("@Course_id", coursedata.SelectedValue);
                        command.ExecuteReader();
                        conn.Close();
                        txtDate.Text = DateTime.Today.ToString();
                        BindCourseData();
                        SchedulePanel.Visible = false;
                        string script = @"<script>
                        Swal.fire({
                        icon: 'success',
                        title: '更新成功',
                        text: '班表已更新',
                        showConfirmButton: false,
                        timer: 1500
                        });
                        </script>";

                        Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);

                    }

                }
            }
        }
        else 
        {
            string script = @"<script>
                            Swal.fire({
                            icon: 'error',
                            title: '請選擇日期',
                            confirmButtonText: '確定'
                            });
                          </script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
        }


    }

    protected void detailstarttime_TextChanged(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#" + SchedulePanel.ClientID + "').modal('show');", true);
        changetime();
    }
    private void changetime()
    {
        if (DateTime.TryParse(detailstarttime.Text, out DateTime startTime))
        {
            TimeSpan courseDuration = TimeSpan.FromMinutes(NewCourse_time);

            DateTime endTime = startTime.Add(courseDuration);

            NewStarttime = startTime.ToString("HH:mm").Trim();
            NewEndtime = endTime.ToString("HH:mm").Trim();
            var cultureInfo = new System.Globalization.CultureInfo("en-US");
            detailendtime.Text = endTime.ToString("tt hh:mm", cultureInfo);
        }
    }
    private void BindDropDownList()
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT 課程編號 , 課程名稱 FROM 健身教練課程 WHERE 健身教練編號=@Coach_id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Coach_id", Coach_id);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            
            coursedata.DataSource = reader;
            coursedata.DataTextField = "課程名稱";  // 要顯示文字
            coursedata.DataValueField = "課程編號";  // 值
            coursedata.DataBind();
        }
        coursedata.SelectedValue = NewCourse_id;
    }
    private void BindDetailCourse() 
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM 健身教練課程 WHERE 課程編號=@Course_id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Course_id", coursedata.SelectedValue);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                if (!reader.IsDBNull(reader.GetOrdinal("課程圖片")))
                {
                    // 將 VARBINARY 圖片資料轉換為 base64 編碼字串
                    byte[] imageData = (byte[])reader["課程圖片"];
                    string base64Image = Convert.ToBase64String(imageData);
                    // 將 base64 編碼的圖片字串設定為 <asp:Image> 控制項的來源
                    img_Course.ImageUrl = "data:image;base64," + base64Image;
                }
                else
                {
                    // 如果沒有圖片，使用預設圖片
                    /*img_de.ImageUrl = "img/team-1.jpg";*/  // 替換為你的預設圖片路徑
                }
                Gettype();
                detailcoursetime.Text = reader["課程時間長度"].ToString();
                detailcoursepeople.Text = reader["上課人數"].ToString();
                detailcoursemoney.Text = Convert.ToDouble(reader["課程費用"]).ToString("F0");
                detailcourseitem.Text = reader["所需設備"].ToString();
                detailcourseplace.Text = reader["上課地點"].ToString();
                detailcourseintro.Text = reader["課程內容介紹"].ToString();
                NewCourse_time =int.Parse(reader["課程時間長度"].ToString().Trim());
                changetime();
            }
            conn.Close();
        }
    }
    protected void coursedata_SelectedIndexChanged(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#" + SchedulePanel.ClientID + "').modal('show');", true);
        BindDetailCourse();
    }
    private void Gettype()
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM 健身教練課表課程合併 WHERE 課程編號=@Course_id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Course_id", coursedata.SelectedValue);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                detailcoursetype.Text = reader["分類名稱"].ToString();
            } 
            conn.Close ();
        }
    }
}

