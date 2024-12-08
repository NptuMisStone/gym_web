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
using System.Net;
using System.Data.Common;
public partial class Coach_Coach_schedule : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;

    public static string Coach_id, Course_id, Course_starttime, Course_endtime, Course_week, tbCourseWeek, Schedule_id;
    public static DateTime Course_date;
    public static int Location_type;
    private const string SelectedDatesKey = "SelectedDates";

    protected void Page_Load(object sender, EventArgs e)
    {
        Coach_id = Convert.ToString(Session["Coach_id"]);
        //驗證教練是否登入的類別函數
        CheckLogin.CheckUserOrCoachLogin(this.Page, "Coach");

        if (!IsPostBack)
        {
            Session["SelectedDate"] = DateTime.Today;
            CourseContainer.InnerHtml = @"
                        <p class='text-center' style='border: 2px dashed black; border-radius: 8px; padding: 15px; height: 100px;'>
                            尚未選擇課程
                        </p>";
            Course_id = null;
            ClearCalendar();
            BindCourseData();
        }
        BindCourseListview();
        // 檢查是否是來自 __doPostBack 的回傳
        if (Request["__EVENTTARGET"] == "ConfirmDeleteScheduleHandler")
        {
            string scheduleId = Request["__EVENTARGUMENT"];
            if (!string.IsNullOrEmpty(scheduleId))
            {
                DeleteSchedule();
            }
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

                    }

                }
            }
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
        // 檢查 Schedule_id 是否有效
        if (!string.IsNullOrEmpty(Schedule_id))
        {
            if (IsDeleteable(Schedule_id))
            {
                // 彈出確認刪除的提示框
                ConfirmDeleteSchedule();
            }
            else {
                ShowAlert("error", "刪除失敗", "需於課程開始 24 小時前刪除", 1500);
            }
        }
        else
        {
            // 如果 Schedule_id 為 null，顯示錯誤訊息
            ShowAlert("error", "刪除失敗", "課表編號無效", 1500);
        }
    }
    protected void ConfirmDeleteSchedule()
    {
        if (!string.IsNullOrEmpty(Schedule_id))
        {
            // 檢查是否有預約人數
            int reservedCount = GetReservedCount();

            if (reservedCount > 0)
            {
                // 如果有預約人數，提示用戶是否確認刪除
                string script = $@"
                Swal.fire({{
                    title: '警告',
                    text: '該課程已有使用者預約，若要刪除課程將會發送通知給使用者，是否仍要刪除此班表？',
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#d33',
                    cancelButtonColor: '#3085d6',
                    confirmButtonText: '仍要刪除',
                    cancelButtonText: '取消'
                }}).then((result) => {{
                    if (result.isConfirmed) {{
                        __doPostBack('ConfirmDeleteScheduleHandler', '{Schedule_id}');
                    }}
                }});";
                ScriptManager.RegisterStartupScript(this, GetType(), "ConfirmDeleteSchedule", script, true);
            }
            else
            {
                // 無預約人數直接刪除課表
                DeleteSchedule();
            }
        }
        else
        {
            ShowAlert("error", "刪除失敗", "課表編號無效", 1500);
        }
    }
    private int GetReservedCount()
    {
        int reservedCount = 0;
        string query = "SELECT 預約人數 FROM 健身教練課表 WHERE 課表編號 = @ScheduleId";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ScheduleId", Schedule_id);
                conn.Open();
                reservedCount = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        return reservedCount;
    }
    private void DeleteSchedule()
    {
        NotifyUsersAboutCancellation(Schedule_id);
        string deleteReservationsQuery = "DELETE FROM 使用者預約 WHERE 課表編號 = @Schedule_id";
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(deleteReservationsQuery, conn))
            {
                command.Parameters.AddWithValue("@Schedule_id", Schedule_id);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
        string qry = @"DELETE FROM 健身教練課表 WHERE 課表編號 = @Schedule_id";
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(qry, conn))
            {
                command.Parameters.AddWithValue("@Schedule_id", Schedule_id);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
        BindCourseData();
        BindCourseListview();
        ShowAlert("success", "刪除成功", "班表已刪除", 1500);
        RegisterScrollScript(btnCurrentWeek.ClientID);
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
        if (e.CommandName == "show")
        {
            string[] args = e.CommandArgument.ToString().Split(',');
            Course_id = args[0];
            Location_type = Convert.ToInt32(args[1]);
            Bind_selectclass(Course_id);
            RegisterScrollScript(Addclass.ClientID);
        }
        BindCourseListview();
    }
    private void Bind_selectclass(string Course_id)
    {
        if (!string.IsNullOrEmpty(Course_id))
        {
            // 清空課程容器的內容，避免重複顯示
            CourseContainer.InnerHtml = "";

            string query = @"SELECT [課程名稱], [課程圖片], [課程時間長度], [上課人數]
             FROM [健身教練課程]
             WHERE [課程編號] = @CourseId AND [健身教練編號] = @CoachId";


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // 設定參數
                    cmd.Parameters.AddWithValue("@CourseId", Course_id);
                    cmd.Parameters.AddWithValue("@CoachId", Coach_id);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                // 取得查詢結果的各個欄位
                                string courseName = reader["課程名稱"].ToString();
                                byte[] courseImageData = reader["課程圖片"] as byte[];  // 課程圖片是二進位資料
                                string courseImage = GetImageUrl(courseImageData, 60);  // 使用 GetImageUrl 方法處理圖片
                                int courseDuration = Convert.ToInt32(reader["課程時間長度"]);
                                int classSize = Convert.ToInt32(reader["上課人數"]);
                                string classType = classSize == 1 ? "一對一" : "團體";

                                // 動態生成 HTML 內容
                                string courseHtml = $@"
    <div style='width: 100%; transition: background-color 0.3s ease; border: 2px solid black; border-radius: 8px; overflow: hidden;'
        onmouseover='this.style.backgroundColor=""#f0f0f0""'
        onmouseout='this.style.backgroundColor=""""'>
        <div class='row align-items-center' style='padding: 5px;'>
            <div class='col-sm-5 text-center' style='padding: 5px;'>
                <img src='{courseImage}' alt='課程圖片' style='object-fit: cover; height: 60px; width: 60px;' class='img-fluid rounded-circle' />
                <br />
                <i style='font-size:14px; font-weight: bold; color:#e31c25;'>{classType}</i>
            </div>
            <div class='col-sm-7'>
                <h4 class='font-weight-bold' style='font-size: 16px; margin: 0;'>{courseName}</h4>
                <p style='font-size: 14px; margin: 0;'>時長：{courseDuration}分鐘</p>
                <p style='font-size: 14px; margin: 0;'>人數：{classSize}人</p>
                <!-- 隱藏的 input 用來存儲課程時間長度，方便前端獲取 -->
                <input type='hidden' class='course-duration' value='{courseDuration}' />
            </div>
        </div>
    </div>
    <br />";


                                // 將生成的 HTML 內容添加到頁面的容器中
                                CourseContainer.InnerHtml += courseHtml; // 注意這裡使用 "+=" 來追加內容
                            }
                        }
                        else
                        {
                            // 處理沒有找到課程的情況
                            CourseContainer.InnerHtml = "<p>未找到相關課程</p>";
                        }
                    }
                }
            }
        }
    }
    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
        // 獲取已選擇的日期
        List<DateTime> selectedDates = GetSelectedDates();

        // 獲取新的選擇日期
        DateTime[] newSelection = Calendar1.SelectedDates.Cast<DateTime>().ToArray();

        // 更新選擇的日期
        foreach (var date in newSelection)
        {
            if (selectedDates.Contains(date))
            {
                // 如果日期已被選擇，則移除
                selectedDates.Remove(date);
            }
            else
            {
                // 如果日期未被選擇，則添加
                selectedDates.Add(date);
            }
        }

        // 更新 Session
        Session[SelectedDatesKey] = selectedDates;
        UpdateSelectedDatesLabel();
        RegisterScrollScript(Addclass.ClientID);
    }

    private void UpdateSelectedDatesLabel()
    {
        // 獲取當前已選擇的日期
        List<DateTime> selectedDates = GetSelectedDates();

        if (selectedDates.Any())
        {
            // 每個日期用一個 <span> 包裝
            string datesHtml = string.Join("", selectedDates.Select(d => $"<span class='date-badge'>{d.ToShortDateString()}</span>"));

            // 設置 Label 的 HTML 內容
            SelectedDatesLabel.Text = "選擇的日期：" + datesHtml;
        }
        else
        {
            SelectedDatesLabel.Text = "選擇的日期：未選擇";
        }

        // 清除所有已選擇的日期樣式
        Calendar1.SelectedDates.Clear();
    }


    private List<DateTime> GetSelectedDates()
    {
        // 獲取 Session 中的選擇日期
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
        // 清除日曆中的所有選擇日期
        Calendar1.SelectedDates.Clear();
        Session[SelectedDatesKey] = new List<DateTime>();
    }

    protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
    {
        // 獲取已選擇的日期
        List<DateTime> selectedDates = GetSelectedDates();
        DateTime today = DateTime.Today;

        // 禁用今天之前的日期
        if (e.Day.Date <= today)
        {
            e.Cell.ForeColor = System.Drawing.Color.Gray; // 文字顏色
            e.Cell.BackColor = System.Drawing.Color.LightGray; // 背景顏色
            e.Cell.Attributes.Add("onclick", "return false;"); // 禁用
        }
        else
        {
            // 如果該日期已選擇，則更改樣式
            if (selectedDates.Contains(e.Day.Date))
            {
                e.Cell.BackColor = System.Drawing.Color.Green; // 背景顏色
                e.Cell.ForeColor = System.Drawing.Color.White; // 文字顏色
            }
        }
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
            BindCourseListview();
            RegisterScrollScript(Week.ClientID);
        }
        else
        {
            ShowAlert("error", "請選擇有效的日期", "請選擇有效的日期", 1500);
        }
    }
    protected void btnAddSchedule_Click(object sender, EventArgs e)
    {
        // 已選擇的日期
        List<DateTime> selectedDates = GetSelectedDates();

        Course_starttime = tbCourseStartTime.Text;
        Course_endtime = hiddenCourseEndTime.Value;

        if (Course_id == null)
        {
            // 顯示未選擇課程訊息
            ShowAlert("error", "請選擇課程", "請先選擇課程再進行操作", 1500);
        }
        else
        {
            // 檢查是否有選擇日期
            if (selectedDates.Count > 0)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    foreach (DateTime date in selectedDates)
                    {
                        string queryCheck = @"
                        SELECT 課程編號, 開始時間, 結束時間
                        FROM [健身教練課表課程-判斷課程衝突用] 
                        WHERE 日期 = @course_date
                        AND 健身教練編號 = @coach_id ";

                        using (SqlCommand checkCommand = new SqlCommand(queryCheck, connection))
                        {
                            checkCommand.Parameters.AddWithValue("@course_date", date);
                            checkCommand.Parameters.AddWithValue("@coach_id", Coach_id);
                            bool hasConflict = false;

                            using (SqlDataReader reader = checkCommand.ExecuteReader())
                            {
                                TimeSpan selectedStartTime = TimeSpan.Parse(Course_starttime);
                                TimeSpan selectedEndTime = TimeSpan.Parse(Course_endtime);

                                TimeSpan? previousEndTime = null;
                                TimeSpan? nextStartTime = null;

                                string currentCourseId = Course_id;  // 當前選擇的課程ID
                                string previousCourseId = null;      // 前一堂課的課程ID
                                string nextCourseId = null;

                                while (reader.Read())
                                {
                                    TimeSpan scheduledStartTime = TimeSpan.Parse(reader["開始時間"].ToString());
                                    TimeSpan scheduledEndTime = TimeSpan.Parse(reader["結束時間"].ToString());
                                    string scheduledCourseId = reader["課程編號"].ToString();

                                    // 時間衝突判斷
                                    if ((selectedStartTime >= scheduledStartTime && selectedStartTime < scheduledEndTime) ||
                                        (selectedEndTime > scheduledStartTime && selectedEndTime <= scheduledEndTime) ||
                                        (selectedStartTime <= scheduledStartTime && selectedEndTime >= scheduledEndTime))
                                    {
                                        ShowAlert("error", "時間衝突", "所選時間與現有時間重疊", 0, true, "確定");
                                        return;
                                    }


                                    // 找到前一堂課的結束時間
                                    if (scheduledEndTime <= selectedStartTime && (previousEndTime == null || scheduledEndTime > previousEndTime))
                                    {
                                        previousEndTime = scheduledEndTime;
                                        previousCourseId = scheduledCourseId;
                                    }

                                    // 找到下一堂課的開始時間
                                    if (scheduledStartTime >= selectedEndTime && (nextStartTime == null || scheduledStartTime < nextStartTime))
                                    {
                                        nextStartTime = scheduledStartTime;
                                        nextCourseId = scheduledCourseId;
                                    }
                                }
                                // 檢查是否與前一堂課留有30分鐘的間隔，但若前一堂課ID相同則跳過
                                if (previousEndTime.HasValue && previousCourseId != currentCourseId && selectedStartTime < previousEndTime.Value.Add(TimeSpan.FromMinutes(30)))
                                {
                                    ShowAlert("error", "課程時間過度密集", "請與前課程預留至少30分鐘交通時間", 0, true, "確定");
                                    hasConflict = true;
                                }

                                // 檢查是否與下一堂課留有30分鐘的間隔，但若下一堂課ID相同則跳過
                                if (nextStartTime.HasValue && nextCourseId != currentCourseId && selectedEndTime > nextStartTime.Value.Subtract(TimeSpan.FromMinutes(30)))
                                {
                                    ShowAlert("error", "課程時間過度密集", "請與後課程預留至少30分鐘交通時間", 0, true, "確定");
                                    hasConflict = true;
                                }

                                if (hasConflict)
                                {
                                    return;  // 如果有衝突，停止執行
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

                // 恢復 CourseContainer 的內容為預設文字 "尚未選擇課程"
                CourseContainer.InnerHtml = @"
                    <p class='text-center' style='border: 2px dashed black; border-radius: 8px; padding: 15px; height: 100px;'>
                        尚未選擇課程
                    </p>";

                // 清空 Course_id
                Course_id = null;
                tbCourseWeek = string.Empty;
                ClearCalendar();
                UpdateSelectedDatesLabel();
                BindCourseData();
                BindCourseListview();

                // 顯示成功訊息
                ShowAlert("success", "新增成功", "班表已新增", 1500);

                RegisterScrollScript(Week.ClientID);
            }
            else
            {
                // 顯示未選擇日期錯誤訊息
                ShowAlert("error", "請選擇日期", "請先選擇日期再進行操作", 1500);
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        // 恢復 CourseContainer 的內容為預設文字 "尚未選擇課程"
        CourseContainer.InnerHtml = @"
        <p class='text-center' style='border: 2px dashed black; border-radius: 8px; padding: 15px; height: 100px;'>
            尚未選擇課程
        </p>";

        // 清空 Course_id
        Course_id = null;
        ClearCalendar();
        UpdateSelectedDatesLabel();
        tbCourseWeek = string.Empty;

    }
    private void ShowAlert(string icon, string title, string text, int timer = 1500, bool showConfirmButton = false, string confirmButtonText = "", bool redirect = false, string redirectUrl = null)
    {
        string script = $@"<script>
Swal.fire({{
    icon: '{icon}',
    title: '{title}',
    text: '{text}',
    showConfirmButton: {showConfirmButton.ToString().ToLower()},
    confirmButtonText: '{confirmButtonText}',
    timer: {timer}
}});";

        if (redirect && !string.IsNullOrEmpty(redirectUrl))
        {
            script += $"setTimeout(function () {{ window.location.href = '{redirectUrl}'; }}, {timer});";
        }

        script += "</script>";

        Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
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

        RegisterScrollScript(Week.ClientID);
        // 重新綁定課程資料
        BindCourseData();
        BindCourseListview();
    }
    protected void btnPreviousWeek_Click(object sender, EventArgs e)
    {
        // 取得當前選擇的日期
        DateTime selectedDate = (DateTime)Session["SelectedDate"];

        // 將日期向前調整一週
        selectedDate = selectedDate.AddDays(-7);

        // 更新 Session 中的日期
        Session["SelectedDate"] = selectedDate;

        RegisterScrollScript(Week.ClientID);
        // 重新綁定課程資料
        BindCourseData();
        BindCourseListview();
    }
    protected void btnNextWeek_Click(object sender, EventArgs e)
    {
        // 取得當前選擇的日期
        DateTime selectedDate = (DateTime)Session["SelectedDate"];

        // 將日期向後調整一週
        selectedDate = selectedDate.AddDays(7);

        // 更新 Session 中的日期
        Session["SelectedDate"] = selectedDate;

        RegisterScrollScript(Week.ClientID);
        // 重新綁定課程資料
        BindCourseData();
        BindCourseListview();
    }
    protected string GetImageUrl(object imageData, int quality)
    {
        try
        {
            if (imageData is byte[] bytes && bytes.Length > 0)
            {
                using (MemoryStream originalStream = new MemoryStream(bytes))
                using (MemoryStream compressedStream = new MemoryStream())
                {
                    System.Drawing.Image originalImage = System.Drawing.Image.FromStream(originalStream);
                    EncoderParameters encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

                    ImageCodecInfo jpgCodec = ImageCodecInfo.GetImageEncoders().First(codec => codec.MimeType == "image/jpeg");
                    originalImage.Save(compressedStream, jpgCodec, encoderParameters);

                    byte[] compressedBytes = compressedStream.ToArray();
                    return "data:image/jpeg;base64," + Convert.ToBase64String(compressedBytes);
                }
            }
            else
            {
                return "img/class_default.png";
            }
        }
        catch (Exception ex)
        {
            // 記錄例外資訊以便調試
            Console.WriteLine($"Error in GetImageUrl: {ex.Message}");
            return "img/class_default.png";
        }
    }
    protected string GetLocation(object Class_id)
    {
        string location = string.Empty;
        string query = @"
        SELECT 地點類型, 地點名稱, 縣市, 行政區
        FROM [健身教練課程-完全整合]
        WHERE 課程編號 = @class_id";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@class_id", Class_id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int courseType = Convert.ToInt32(reader["地點類型"]);
                        string locationName = reader["地點名稱"].ToString();
                        string city = reader["縣市"].ToString();
                        string district = reader["行政區"].ToString();

                        if (courseType == 2)
                        {
                            // 當課程類型為 2 時，顯示 "到府" + 縣市 + 行政區
                            location = city + district;
                        }
                        else
                        {
                            // 否則顯示地點名稱
                            location = locationName;
                        }
                    }
                }
            }
        }

        return location;
    }

    private void RegisterScrollScript(string controlId)
    {
        // 使用 controlId 傳遞 ClientID 而不是靜態 ID
        ClientScript.RegisterStartupScript(this.GetType(), "scrollToControl", $"scrollToControl('{controlId}');", true);
    }
    protected void NotifyUsersAboutCancellation( string scheduleid)
    {
        string query = "SELECT 使用者郵件,課程名稱,日期,開始時間,結束時間,健身教練姓名 " +
                       "FROM [使用者預約-有預約的] WHERE 課表編號 = @ScheduleId AND 預約狀態=2";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ScheduleId", scheduleid); // 設定課表編號參數
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
        mms.Subject = "【屏大Fit-健身預約系統】取消開課通知";
        mms.Body = "<p>您好，</p>" +
            "<p>我們遺憾地通知您，您所預約的課程已被教練取消開課。</p>" +
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
        Debug.WriteLine("已寄出取消開課通知");

    }
    public bool IsDeleteable(string scheduleId)
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