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
public partial class Coach_Coach_schedule : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;

    public static string Coach_id;
    public static int Course_time;
    public static string Course_id, Course_starttime, Course_endtime, Course_week;
    public static DateTime Course_date;
    private const string SelectedDatesKey = "SelectedDates";
    public static string tbCourseWeek;

    protected void Page_Load(object sender, EventArgs e)
    {
        Coach_id = Convert.ToString(Session["Coach_id"]);
        if (Session["Coach_id"] == null)
        {
            string script = @"<script>
                Swal.fire({
                  icon: 'error',
                  title: '請先登入！',
                  confirmButtonText: '確定',
                }).then((result) => {
                  if (result.isConfirmed) {
                     window.location.href = '../Coach/Coach_login.aspx';
                  }
                });
                </script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
        }
        else
        {
            if (!IsPostBack)
            {
                BindCourse();
                ClearCalendar();
                UpdateSelectedDatesLabel();
                txtDate.Text = DateTime.Today.ToString();
                BindCourseData();
            }
            Calendar1.DayRender += new DayRenderEventHandler(Calendar1_DayRender);


        }
    }
    private void BindCourse()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "Select 課程編號,課程名稱,課程類型,課程時間長度,上課人數,上課地點,課程費用,課程圖片 From 健身教練課程 Where 健身教練編號=@CoachID ";
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@CoachID", Coach_id);

                SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.SequentialAccess);
                gv_course.DataSource = dataReader;
                gv_course.DataBind();
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

                    // 遍历 selectedDates 列表
                    foreach (DateTime date in selectedDates)
                    {
                        Course_date = date;
                        CultureInfo cultureInfo = new CultureInfo("zh-TW");
                        string dayOfWeek = date.ToString("dddd", cultureInfo);
                        tbCourseWeek = dayOfWeek;
                        Course_week = tbCourseWeek;

                        string query = "insert into 健身教練課表 (課程編號, 日期, 開始時間, 結束時間, 星期幾)" +
                                       "values (@course_id, @course_date, @course_starttime, @course_endtime, @course_week)";
                        command.CommandText = query;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@course_id", Course_id);
                        command.Parameters.AddWithValue("@course_date", Course_date);
                        command.Parameters.AddWithValue("@course_starttime", Course_starttime);
                        command.Parameters.AddWithValue("@course_endtime", Course_endtime);
                        command.Parameters.AddWithValue("@course_week", Course_week);

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
        var dates = Session[SelectedDatesKey] as List<DateTime>;
        if (dates != null)
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
        DateTime startDateTime;
        if (DateTime.TryParse(tbCourseStartTime.Text, out startDateTime))
        {
            TimeSpan courseDuration = TimeSpan.FromMinutes(Course_time);

            DateTime endDateTime = startDateTime.Add(courseDuration);

            Course_starttime = startDateTime.ToString("HH:mm").Trim();
            Course_endtime = endDateTime.ToString("HH:mm").Trim();
            var cultureInfo = new CultureInfo("en-US");
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
            DateTime startOfWeek = selectedDate.AddDays(-(int)selectedDate.DayOfWeek + 1);
            DateTime endOfWeek = startOfWeek.AddDays(6);
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
                        cmd.Parameters.AddWithValue("week", week);
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
}

