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
            Session["SelectedDate"] = DateTime.Today;
            BindCourseData();
            BindCourseListview();
        }
        
    }
    private void BindCourseData()
    {// 使用 Session 中存儲的日期
        DateTime selectedDate = (DateTime)Session["SelectedDate"];
        DateTime today = DateTime.Today;

        DayOfWeek dayOfWeek = selectedDate.DayOfWeek;
        int newday = dayOfWeek == DayOfWeek.Sunday ? -6 : -(int)dayOfWeek + 1;
        DateTime startOfWeek = selectedDate.AddDays(newday);  // 星期一開始
        DateTime endOfWeek = startOfWeek.AddDays(6);  // 星期日結束

        // 更新 Label 控件中的日期
        lblMondayDate.Text = startOfWeek.ToString("MM/dd");
        lblTuesdayDate.Text = startOfWeek.AddDays(1).ToString("MM/dd");
        lblWednesdayDate.Text = startOfWeek.AddDays(2).ToString("MM/dd");
        lblThursdayDate.Text = startOfWeek.AddDays(3).ToString("MM/dd");
        lblFridayDate.Text = startOfWeek.AddDays(4).ToString("MM/dd");
        lblSaturdayDate.Text = startOfWeek.AddDays(5).ToString("MM/dd");
        lblSundayDate.Text = startOfWeek.AddDays(6).ToString("MM/dd");

        // 查詢該週課表
        string query = @"SELECT [課程名稱], [開始時間], [結束時間], [地點類型], [課表編號] 
                 FROM [健身教練課表課程合併]
                 WHERE [日期] BETWEEN @StartOfWeek AND @EndOfWeek 
                   AND [健身教練編號] = @Coach_id AND [星期幾] = @week
                 ORDER BY [開始時間]";


        List<string> weeklist = new List<string> { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日" };
        Dictionary<string, List<(string courseInfo, int locationType, string scheduleId)>> weekCoursesMap = new Dictionary<string, List<(string courseInfo, int locationType, string scheduleId)>>();


        // 初始化每星期對應的課程列表，新增 scheduleId
        foreach (string week in weeklist)
        {
            weekCoursesMap[week] = new List<(string courseInfo, int locationType, string scheduleId)>();
        }


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

                    while (reader.Read())
                    {
                        string courseName = reader["課程名稱"].ToString();
                        string startTime = reader["開始時間"].ToString();
                        string endTime = reader["結束時間"].ToString();
                        int locationType = Convert.ToInt32(reader["地點類型"]);
                        string scheduleId = reader["課表編號"].ToString(); // 讀取課表編號

                        weekCoursesMap[week].Add(($"{courseName} ({startTime} - {endTime})", locationType, scheduleId));

                        hasData = true;
                    }

                }
            }
        }

        if (!hasData)
        {
            lblMessage.Text = "無課表";
        }

        // 在這裡將 weekCoursesMap 的數據綁定到前端 HTML 表格
        BindCoursesToTable(weekCoursesMap, startOfWeek, today);
    }



    private void BindCoursesToTable(Dictionary<string, List<(string courseInfo, int locationType, string scheduleId)>> weekCoursesMap, DateTime startOfWeek, DateTime today)
    {
        string GenerateCourseHtml(string courseInfo, int locationType, string scheduleId)
        {
            var parts = courseInfo.Split(new[] { " (" }, StringSplitOptions.None);
            string courseName = parts[0];
            string time = parts.Length > 1 ? parts[1].TrimEnd(')') : "";

            string courseCardClass = locationType == 2 ? "course-card-blue" : "course-card-red";
            string courseTimeClass = locationType == 2 ? "course-time-blue" : "course-time-red";

            // 將課程包裹在一個可點擊的按鈕內，並附加點擊事件，傳遞課表編號
            return $"<button class='{courseCardClass}' onclick='handleCourseClick(\"{scheduleId}\")'><div class='{courseTimeClass}'>{time}</div><div class='course-name'>{courseName}</div></button>";
        }

        // 設置每個星期的課程，並根據 locationType 動態生成樣式
        MondayCell.InnerHtml = string.Join("<br />", weekCoursesMap["星期一"].Select(course =>
            GenerateCourseHtml(course.courseInfo, course.locationType, course.scheduleId)));
        TuesdayCell.InnerHtml = string.Join("<br />", weekCoursesMap["星期二"].Select(course =>
            GenerateCourseHtml(course.courseInfo, course.locationType, course.scheduleId)));
        WednesdayCell.InnerHtml = string.Join("<br />", weekCoursesMap["星期三"].Select(course =>
            GenerateCourseHtml(course.courseInfo, course.locationType, course.scheduleId)));
        ThursdayCell.InnerHtml = string.Join("<br />", weekCoursesMap["星期四"].Select(course =>
            GenerateCourseHtml(course.courseInfo, course.locationType, course.scheduleId)));
        FridayCell.InnerHtml = string.Join("<br />", weekCoursesMap["星期五"].Select(course =>
            GenerateCourseHtml(course.courseInfo, course.locationType, course.scheduleId)));
        SaturdayCell.InnerHtml = string.Join("<br />", weekCoursesMap["星期六"].Select(course =>
            GenerateCourseHtml(course.courseInfo, course.locationType, course.scheduleId)));
        SundayCell.InnerHtml = string.Join("<br />", weekCoursesMap["星期日"].Select(course =>
            GenerateCourseHtml(course.courseInfo, course.locationType, course.scheduleId)));

        // 檢查今天的日期，若是今天則加上底色
        DateTime mondayDate = startOfWeek;
        DateTime tuesdayDate = startOfWeek.AddDays(1);
        DateTime wednesdayDate = startOfWeek.AddDays(2);
        DateTime thursdayDate = startOfWeek.AddDays(3);
        DateTime fridayDate = startOfWeek.AddDays(4);
        DateTime saturdayDate = startOfWeek.AddDays(5);
        DateTime sundayDate = startOfWeek.AddDays(6);

        // 清除所有標題的背景顏色
        MondayHeader.Style["background-color"] = "";
        TuesdayHeader.Style["background-color"] = "";
        WednesdayHeader.Style["background-color"] = "";
        ThursdayHeader.Style["background-color"] = "";
        FridayHeader.Style["background-color"] = "";
        SaturdayHeader.Style["background-color"] = "";
        SundayHeader.Style["background-color"] = "";

        if (mondayDate.Date == today)
        {
            MondayHeader.Style["background-color"] = "#e31c25"; // 設置星期一標題底色
        }
        if (tuesdayDate.Date == today)
        {
            TuesdayHeader.Style["background-color"] = "#e31c25"; // 設置星期二標題底色
        }
        if (wednesdayDate.Date == today)
        {
            WednesdayHeader.Style["background-color"] = "#e31c25"; // 設置星期三標題底色
        }
        if (thursdayDate.Date == today)
        {
            ThursdayHeader.Style["background-color"] = "#e31c25"; // 設置星期四標題底色
        }
        if (fridayDate.Date == today)
        {
            FridayHeader.Style["background-color"] = "#e31c25"; // 設置星期五標題底色
        }
        if (saturdayDate.Date == today)
        {
            SaturdayHeader.Style["background-color"] = "#e31c25"; // 設置星期六標題底色
        }
        if (sundayDate.Date == today)
        {
            SundayHeader.Style["background-color"] = "#e31c25"; // 設置星期日標題底色
        }
    }
    [System.Web.Services.WebMethod]
    public static object GetScheduleDetails(string scheduleId)
    {
        Debug.WriteLine("Schedule ID: " + scheduleId);
        //存到全域變數給刪除函數使用
        Schedule_id = scheduleId;
        string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"SELECT [課程名稱], [日期], [課程時間長度], [開始時間], [結束時間]
                         FROM [健身教練課表課程合併]
                         WHERE [課表編號] = @ScheduleId";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ScheduleId", scheduleId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new
                    {
                        courseName = reader["課程名稱"].ToString(),
                        date = ((DateTime)reader["日期"]).ToString("MM/dd"), // 格式化日期
                        duration = reader["課程時間長度"].ToString(),
                        startTime = reader["開始時間"].ToString(),
                        endTime = reader["結束時間"].ToString()
                    };
                }
                else
                {
                    return new { error = "找不到相關課程信息。" };
                }
            }
        }
    }
    protected void btnDeleteSchedule_Click(object sender, EventArgs e)
    {
        // 檢查 Schedule_id 是否為 null
        if (Schedule_id != null)
        {
            // Schedule_id 不為 null，執行刪除操作
            string qry = @"DELETE FROM 健身教練課表 WHERE 課表編號 = @Schedule_id";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(qry, conn))
                {
                    command.Parameters.AddWithValue("@Schedule_id", Schedule_id);
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();
                    BindCourseData(); // 更新課程資料
                    string successScript = @"<script>
                            Swal.fire({
                            icon: 'success',
                            title: '刪除成功',
                            text: '班表已刪除',
                            showConfirmButton: false,
                            timer: 1500
                            });
                          </script>";

                    // 顯示刪除成功的提示
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertSuccess", successScript, false);
                }
            }
            RegisterScrollScript();
        }
        else
        {
            // 如果 Schedule_id 為 null，顯示錯誤訊息
            string errorScript = @"<script>
                                Swal.fire({
                                    icon: 'error',
                                    title: '刪除失敗',
                                    text: '課表編號無效',
                                    showConfirmButton: false,
                                    timer: 1500
                                });
                              </script>";

            // 顯示錯誤提示
            Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertError", errorScript, false);
        }
    }

    private void BindCourseListview()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // 查詢指定教練的所有課程
                string sql = "Select * From 健身教練課程 Where 健身教練編號=@CoachID";
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@CoachID", Coach_id);

                // 使用 SqlDataReader 來獲取資料
                SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.SequentialAccess);

                // 綁定到 ListView
                lv_class.DataSource = dataReader;
                lv_class.DataBind();

                connection.Close();
            }
        }
        catch (Exception ex)
        {
            // 可以在這裡記錄或處理錯誤
            Console.WriteLine(ex.Message);
        }
    }
    protected void lv_class_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "see_detail")
        {
            // 取得課程編號，存入 Session，並跳轉至詳細頁面
            Session["Class_id"] = Convert.ToInt32(e.CommandArgument);
            //Response.Redirect("class_detail.aspx");
        }
    }

    private void CheckAndSetTodayColor(Label lbl, DateTime date)
    {
        if (date.Date == DateTime.Today)
        {
            lbl.Style["background-color"] = "#e31c25"; // 設置底色為紅色
        }
    }
    protected void btnCurrentWeek_Click(object sender, EventArgs e)
    {
        // 設置為本週，即當前日期
        Session["SelectedDate"] = DateTime.Today;

        // 重新綁定課程資料
        BindCourseData();
    }
    protected void btnPreviousWeek_Click(object sender, EventArgs e)
    {
        // 取得當前選擇的日期
        DateTime selectedDate = (DateTime)Session["SelectedDate"];

        // 將日期向前調整一週
        selectedDate = selectedDate.AddDays(-7);

        // 更新 Session 中的日期
        Session["SelectedDate"] = selectedDate;

        // 重新綁定課程資料
        BindCourseData();
    }
    protected void btnNextWeek_Click(object sender, EventArgs e)
    {
        // 取得當前選擇的日期
        DateTime selectedDate = (DateTime)Session["SelectedDate"];

        // 將日期向後調整一週
        selectedDate = selectedDate.AddDays(7);

        // 更新 Session 中的日期
        Session["SelectedDate"] = selectedDate;

        // 重新綁定課程資料
        BindCourseData();
    }
    protected void btnQueryWeek_Click(object sender, EventArgs e)
    {
        // 獲取選擇的日期，從 TextBox 獲取文本
        DateTime selectedDate;
        if (DateTime.TryParse(txtSelectedDate.Text, out selectedDate)) // 使用 txtSelectedDate.Text
        {
            // 更新 Session 中的日期
            Session["SelectedDate"] = selectedDate;

            // 重新綁定課程資料
            BindCourseData();
        }
        else
        {
            lblMessage.Text = "請選擇有效的日期。";
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
            return "img/null.png"; // 替代圖片的路徑
        }
    }
    private void RegisterScrollScript()
    {
        // 註冊 JavaScript，PostBack 後自動捲動到 rblClassSize
        ClientScript.RegisterStartupScript(this.GetType(), "scrollToClassSize", "scrollToControl();", true);
    }

}