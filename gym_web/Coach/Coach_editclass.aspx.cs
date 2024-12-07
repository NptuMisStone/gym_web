using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Coach_Coach_editclass : System.Web.UI.Page
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    public static string Coach_id, Class_id, classType_id;
    string city;
    protected void Page_Load(object sender, EventArgs e)
    {
        Coach_id = Convert.ToString(Session["Coach_id"]);
        Class_id = Convert.ToString(Session["Class_id"]);

        //驗證教練是否登入的類別函數
        CheckLogin.CheckUserOrCoachLogin(this.Page, "Coach");

        if (!IsPostBack)
        {
            Session["uploadedImage"] = null;
            BindDropDownList();
            BindRadioButtonList();
            LoadClassDetails();
            SetValidators();
            
        }
        showTempimg();
        if (Request["__EVENTTARGET"] == "btnConfirmDelete" && !string.IsNullOrEmpty(Request["__EVENTARGUMENT"]))
        {
            string classId = Request["__EVENTARGUMENT"];
            confirmdelete(classId);
        }
    }
    private void LoadClassDetails()
    {
        string query = @"SELECT * FROM [健身教練課程] WHERE [課程編號] = @ClassID";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ClassID", Class_id);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        classType_id = reader["分類編號"].ToString();
                        // 將讀取到的課程資料綁定到控制項上
                        ddlCourseType.SelectedValue = classType_id;
                        tbCourseName.Text = reader["課程名稱"].ToString();
                        ddlCourseTime.SelectedValue = reader["課程時間長度"].ToString();
                        tbRequiredEquipment.Text = reader["所需設備"].ToString();
                        tbCourseDescription.Text = reader["課程內容介紹"].ToString();

                        decimal courseFee = Convert.ToDecimal(reader["課程費用"]);
                        tbCourseFee.Text = courseFee.ToString("0");

                        int people = (int)reader["上課人數"];
                        if (people == 1)
                        {
                            rblClassSize.SelectedValue = "1";
                        }
                        else if (people > 1)
                        {
                            rblClassSize.SelectedValue = "2";
                            tbClassSize.Visible = true;
                            tbClassSize.Text = people.ToString();
                        }

                        int locationType = (int)reader["地點類型"];
                        rblLocation.SelectedValue = locationType.ToString();

                        // 先檢查地點類型
                        if (locationType == 3 || locationType == 2)
                        {
                            // 設置地點名稱和地址
                            tbClassLocation.Text = reader["地點名稱"].ToString();
                            tbClassAddress.Text = reader["地點地址"].ToString();

                            // 獲取縣市ID並選擇
                            if (!reader.IsDBNull(reader.GetOrdinal("縣市id")))  // 檢查是否為 DBNull
                            {
                                int cityId = Convert.ToInt32(reader["縣市id"]);
                                ddl_city.SelectedValue = cityId.ToString();

                                // 加載行政區
                                BindAreaItem(cityId);

                                // 設置已選擇的行政區
                                if (!reader.IsDBNull(reader.GetOrdinal("行政區id")))  // 檢查是否為 DBNull
                                {
                                    ddl_area.SelectedValue = reader["行政區id"].ToString();
                                }
                            }
                        }

                        // 顯示圖片
                        if (!reader.IsDBNull(reader.GetOrdinal("課程圖片")))
                        {
                            Session["uploadedImage"] = (byte[])reader["課程圖片"];
                        }
                        else
                        {
                            Session["uploadedImage"] = null;
                        }
                    }
                }
            }
        }
    }

    private void BindAreaItem(int cityId)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"SELECT 行政區id, 行政區 FROM 行政區 WHERE 縣市id = @CityId";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CityId", cityId);
            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            ddl_area.DataSource = reader;
            ddl_area.DataTextField = "行政區";  // 顯示的文本
            ddl_area.DataValueField = "行政區id";  // 對應的值
            ddl_area.DataBind();

            // 插入一個 "請選擇" 選項
            ddl_area.Items.Insert(0, new ListItem("請選擇行政區", ""));
        }
    }

    private void BindDropDownList()
    {
        // 1. 綁定運動分類清單
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"SELECT * FROM [運動分類清單]";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            ddlCourseType.DataSource = reader;
            ddlCourseType.DataTextField = "分類名稱";
            ddlCourseType.DataValueField = "分類編號";
            ddlCourseType.DataBind();
        }

        // 2. 綁定縣市
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"SELECT * FROM [縣市]";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            ddl_city.DataSource = reader;
            ddl_city.DataTextField = "縣市";
            ddl_city.DataValueField = "縣市id";
            ddl_city.DataBind();
        }
        // 添加一個空的初始項目
        ddlCourseType.Items.Insert(0, new ListItem("選擇課程類型", ""));
        ddlCourseTime.Items.Insert(0, new ListItem("選擇課程時長", ""));
    }

    private void BindRadioButtonList()
    {
        string query = "SELECT [註冊類型], [服務地點名稱] FROM [健身教練審核合併] WHERE [健身教練編號] = @CoachID";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CoachID", Coach_id);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string registrationType = reader["註冊類型"].ToString();
                        string serviceLocationName = reader["服務地點名稱"].ToString();

                        if (registrationType == "店家健身教練")
                        {
                            // 動態增加一個新的 ListItem 作為第一個項目
                            rblLocation.Items.Insert(0, new ListItem(serviceLocationName, "1"));
                        }
                    }
                }
            }
        }
        rbReset();
    }
    protected void btnHiddenUpload_Click(object sender, EventArgs e)
    {
        showTempimg();
    }
    private void showTempimg()
    {
        if (fuCourseImage.HasFile) // 確認是否有上傳檔案
        {
            byte[] imgBytes = fuCourseImage.FileBytes;
            if (imgBytes != null && imgBytes.Length > 0)
            {
                // 儲存圖片到 Session
                Session["uploadedImage"] = imgBytes;

                // 轉換為 Base64 字串並設置 Image1 的 ImageUrl
                string imageBase64 = Convert.ToBase64String(imgBytes);
                string imgSrc = $"data:image/jpeg;base64,{imageBase64}";
                Image1.ImageUrl = imgSrc;
            }
        }
        else if (Session["uploadedImage"] != null)
        {
            byte[] imgBytes = (byte[])Session["uploadedImage"];
            if (imgBytes.Length > 0)
            {
                string imageBase64 = Convert.ToBase64String(imgBytes);
                string imgSrc = $"data:image/jpeg;base64,{imageBase64}";
                Image1.ImageUrl = imgSrc;
            }
        }
        else // 沒有上傳檔案時，顯示預設圖片
        {
            Image1.ImageUrl = ResolveUrl("~/Coach/img/upload.png");
        }
    }
    private int city_id;
    private int area_id;
    protected void btnUpdateCourse_Click(object sender, EventArgs e)
    {
        
        if (Page.IsValid)
        {
            string query = @"UPDATE [健身教練課程]
                 SET [課程名稱] = @CourseName,
                     [分類編號] = @CourseType,
                     [課程時間長度] = @CourseTime,
                     [課程內容介紹] = @CourseDescription,
                     [所需設備] = @RequiredEquipment,
                     [課程費用] = @CourseFee,
                     [上課人數] = @ClassSize,
                     [地點類型] = @LocationType,
                     [地點名稱] = @LocationName,
                     [地點地址] = @LocationAddress,
                     [縣市id] = @City_id,
                     [行政區id] = @Area_id";

            // 只有當 courseImage 有值時才更新圖片
            if (Session["uploadedImage"] != null)
            {
                query += ", [課程圖片] = @CourseImage";
            }

            query += " WHERE [課程編號] = @ClassID";

            string courseName = tbCourseName.Text.Trim();
            string courseType = ddlCourseType.SelectedValue;
            int courseTime = int.Parse(ddlCourseTime.SelectedValue);
            string courseDescription = tbCourseDescription.Text.Trim();
            string requiredEquipment = tbRequiredEquipment.Text.Trim();
            decimal courseFee = decimal.Parse(tbCourseFee.Text);
            int classSize = rblClassSize.SelectedValue == "1" ? 1 : int.Parse(tbClassSize.Text);
            int locationType = int.Parse(rblLocation.SelectedValue);
            string LocationName = tbClassLocation.Text.Trim();
            string LocationAddress = tbClassAddress.Text.Trim();
            byte[] courseImage = Session["uploadedImage"] != null ? (byte[])Session["uploadedImage"] : null;
            
            if (locationType == 1)
            {
                // 從資料庫查詢服務地點資訊
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query1 = "SELECT[縣市id], [行政區id]FROM [健身教練審核合併] WHERE [健身教練編號] = @CoachID";
                    SqlCommand command = new SqlCommand(query1, connection);
                    command.Parameters.AddWithValue("@CoachID", Coach_id);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        city_id = Convert.ToInt32(reader["縣市id"]);
                        area_id = Convert.ToInt32(reader["行政區id"]);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            else {
                city_id = Convert.ToInt32(ddl_city.SelectedValue);
                area_id = Convert.ToInt32(ddl_area.SelectedValue);
            }
            

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CourseName", courseName);
                    command.Parameters.AddWithValue("@CourseType", courseType);
                    command.Parameters.AddWithValue("@CourseTime", courseTime);
                    command.Parameters.AddWithValue("@CourseDescription", courseDescription);
                    command.Parameters.AddWithValue("@RequiredEquipment", requiredEquipment);
                    command.Parameters.AddWithValue("@CourseFee", courseFee);
                    command.Parameters.AddWithValue("@ClassSize", classSize);
                    command.Parameters.AddWithValue("@LocationType", locationType);
                    command.Parameters.AddWithValue("@LocationName", locationType == 3 ? (object)LocationName : DBNull.Value);
                    command.Parameters.AddWithValue("@LocationAddress", locationType == 3 ? (object)LocationAddress : DBNull.Value);

                    command.Parameters.AddWithValue("@City_id", city_id);
                    command.Parameters.AddWithValue("@Area_id", area_id);

                    // 只有當有圖片時才傳遞圖片參數
                    if (courseImage != null)
                    {
                        command.Parameters.AddWithValue("@CourseImage", courseImage);
                    }

                    command.Parameters.AddWithValue("@ClassID", Class_id);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        ShowAlert("success", "更新成功", "課程已更新", 1500, true, "Coach_class.aspx");
                    }
                    else
                    {
                        ShowAlert("error", "更新失敗", "資料填寫不完整或有誤，請檢查後再試", 1500, true, "Coach_class.aspx");
                    }
                }
            }
        }
        else
        {
            ShowAlert("error", "更新失敗", "資料填寫不完整或有誤，請檢查後再試", 1500, true, "Coach_class.aspx");
        }
    }


    protected void btnDeleteCourse_Click(object sender, EventArgs e)
    {
        DeleteCourse(Class_id);
    }
    protected void ddl_city_SelectedIndexChanged(object sender, EventArgs e)
    {
        // 檢查是否有選擇縣市，如果選擇的值為空，則不繼續執行
        if (string.IsNullOrEmpty(ddl_city.SelectedValue) || ddl_city.SelectedValue == "0")
        {
            ddl_area.Items.Clear();
            return;
        }

        city = ddl_city.SelectedItem.Text;
        int city_id = Convert.ToInt32(ddl_city.SelectedValue);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string sql = "SELECT 行政區,行政區id FROM 行政區 WHERE 縣市id=@縣市id";
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@縣市id", city_id);

            SqlDataReader dataReader = cmd.ExecuteReader();

            // 先清空 ddl_area 裡現有的項目，防止重複顯示
            ddl_area.Items.Clear();
            ddl_area.DataSource = dataReader;
            ddl_area.DataTextField = "行政區";
            ddl_area.DataValueField = "行政區id";
            ddl_area.DataBind();

            // 在第一項插入提示 "請選擇鄉鎮區"
            ddl_area.Items.Insert(0, new ListItem("請選擇鄉鎮區", ""));

            dataReader.Close();
            connection.Close();
            RegisterScrollScript(tbCourseFee.ClientID);
        }
    }
    protected void ddl_city_DataBound(object sender, EventArgs e)
    {
        ddl_city.Items.Insert(0, new ListItem("請選擇縣市", ""));
    }

    protected void rdlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        rbReset();
        RegisterScrollScript(tbCourseFee.ClientID);
    }
    private void rbReset()
    {
        if (rblLocation.SelectedItem.Value == "3") // 選擇其他選項
        {
            tbClassLocation.Visible = true; // 顯示指定地點輸入框
            ddl_city.Visible = true;
            ddl_area.Visible = true;
            tbClassAddress.Visible = true;
            rfvClassLocation.Enabled = true; // 啟用指定地點的必填驗證
            rfvClassAddress.Enabled = true;
        }
        else if (rblLocation.SelectedItem.Value == "2") // 選擇到府選項
        {
            ddl_city.Visible = true;
            ddl_area.Visible = true;
            tbClassLocation.Text = ""; // 清空指定地點輸入框
            tbClassAddress.Text = "";
            tbClassLocation.Visible = false; // 隱藏指定地點輸入框
            tbClassAddress.Visible = false;
            rfvClassLocation.Enabled = false; // 停用指定地點的必填驗證
            rfvClassAddress.Enabled = false;
        }
        else // 選擇預設店家選項
        {
            ddl_city.Visible = false;
            ddl_area.Visible = false;
            tbClassLocation.Text = ""; // 清空指定地點輸入框
            tbClassAddress.Text = "";
            tbClassLocation.Visible = false; // 隱藏指定地點輸入框
            tbClassAddress.Visible = false;
            rfvClassLocation.Enabled = false; // 停用指定地點的必填驗證
            rfvClassAddress.Enabled = false;
        }
    }
    protected void rblClassSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblClassSize.SelectedItem.Value == "2") // 選擇團體課程
        {
            tbClassSize.Visible = true; // 顯示團體課程人數輸入框
            rfvClassSize.Enabled = true; // 啟用團體課程人數的必填驗證
            cvClassSize.Enabled = true; // 啟用團體課程人數的自定義驗證

            rblLocation.Items.FindByValue("2").Enabled = false; // 停用到府選項

            // 如果目前選擇的是到府，切換到其他選項
            if (rblLocation.SelectedValue == "2")
            {
                rblLocation.ClearSelection();
                rblLocation.Items.FindByValue("3").Selected = true;
                tbClassLocation.Visible = true;
                tbClassAddress.Visible = true;
                rfvClassLocation.Enabled = true;
            }
        }
        else // 選擇一對一課程
        {
            tbClassSize.Text = ""; // 清空團體課程人數輸入框
            tbClassSize.Visible = false; // 隱藏團體課程人數輸入框
            rfvClassSize.Enabled = false; // 停用團體課程人數的必填驗證
            cvClassSize.Enabled = false; // 停用團體課程人數的自定義驗證

            rblLocation.Items.FindByValue("2").Enabled = true; // 啟用到府選項
        }
        RegisterScrollScript(tbCourseFee.ClientID);
    }
    protected void cvClassSize_ServerValidate(object source, ServerValidateEventArgs args)
    {
        int classSize;
        // 嘗試將輸入值轉換為整數
        if (int.TryParse(tbClassSize.Text, out classSize))
        {
            // 驗證是否大於1
            if (classSize > 1)
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }
        }
        else
        {
            // 如果無法轉換為整數，表示輸入無效
            args.IsValid = false;
        }
    }
    private void SetValidators()
    {
        // 設定團體課程人數的驗證條件
        if (rblClassSize.SelectedValue == "2") // 團體課程
        {
            tbClassSize.Visible = true;
            rfvClassSize.Enabled = true;
            cvClassSize.Enabled = true;
        }
        else // 一對一課程
        {
            tbClassSize.Visible = false;
            rfvClassSize.Enabled = false;
            cvClassSize.Enabled = false;
        }

        // 設定上課地點的驗證條件
        if (rblLocation.SelectedValue == "3") // 教練指定地點
        {
            tbClassLocation.Visible = true;
            ddl_city.Visible = true;
            ddl_area.Visible = true;
            tbClassAddress.Visible = true;
            rfvClassLocation.Enabled = true;
            rfvClassAddress.Enabled = true;
        }
        else if (rblLocation.SelectedValue == "2")// 到府上課
        {
            ddl_city.Visible = true;
            ddl_area.Visible = true;
            tbClassLocation.Visible = false;
            tbClassAddress.Visible = false;
            rfvClassLocation.Enabled = false;
            rfvClassAddress.Enabled = false;
        }
        else
        {
            ddl_city.Visible = false;
            ddl_area.Visible = false;
            tbClassLocation.Visible = false;
            tbClassAddress.Visible = false;
            rfvClassLocation.Enabled = false;
            rfvClassAddress.Enabled = false;
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
            return "課程預設圖.png"; // 替代圖片的路徑
        }
    }
    private void ShowAlert(string icon, string title, string text, int timer = 1500, bool redirect = false, string redirectUrl = null)
    {
        string script = $@"<script>
        Swal.fire({{
            icon: '{icon}',
            title: '{title}',
            text: '{text}',
            showConfirmButton: false,
            timer: {timer}
        }});
    ";

        if (redirect && !string.IsNullOrEmpty(redirectUrl))
        {
            script += $"setTimeout(function () {{ window.location.href = '{redirectUrl}'; }}, {timer});";
        }

        script += "</script>";

        Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
    }
    private void RegisterScrollScript(string controlId)
    {
        // 使用 controlId 傳遞 ClientID 而不是靜態 ID
        ClientScript.RegisterStartupScript(this.GetType(), "scrollToControl", $"scrollToControl('{controlId}');", true);
    }
    protected void DeleteCourse(string classId)
    {
        // 檢查是否有未結束的預約
        string queryCheck = @"
        SELECT COUNT(*) AS count 
        FROM [使用者預約-有預約的] 
        WHERE 課程編號 = @classId 
          AND (日期 > GETDATE() 
               OR (日期 = GETDATE() AND 結束時間 > CONVERT(VARCHAR, GETDATE(), 108)))";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(queryCheck, conn))
            {
                cmd.Parameters.AddWithValue("@classId", classId);
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count > 0)
                {
                    // 如果有未結束的預約，提示 SweetAlert
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                        "Swal.fire({ " +
                        "title: '無法刪除課程', " +
                        "text: '該課程仍有未結束的預約，請先於班表移除排班後再嘗試刪除。', " +
                        "icon: 'warning', " +
                        "confirmButtonText: '確定' });", true);
                    return;
                }
            }
        }

        // 顯示確認 SweetAlert
        ScriptManager.RegisterStartupScript(this, GetType(), "confirmDelete",
            "Swal.fire({ " +
            "title: '二次刪除課程確認', " +
            "text: '您真的確定要刪除課程？刪除後課程將無法復原！', " +
            "icon: 'warning', " +
            "showCancelButton: true, " +
            "confirmButtonText: '確定', " +
            "cancelButtonText: '取消' " +
            "}).then((result) => { " +
            "if (result.isConfirmed) { " +
            "__doPostBack('btnConfirmDelete', '" + classId + "'); " +
            "} });", true);
    }
    private void confirmdelete(string classId)
    {
        string queryDelete = @"
        DELETE FROM 課程被收藏 WHERE 課程編號 = @classId;
        DELETE FROM 健身教練課程 WHERE 課程編號 = @classId;
    ";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(queryDelete, conn))
            {
                cmd.Parameters.AddWithValue("@classId", classId);
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // 成功刪除
                    ScriptManager.RegisterStartupScript(this, GetType(), "success",
                        "Swal.fire({ " +
                        "title: '課程已成功刪除', " +
                        "icon: 'success', " +
                        "timer: 2000, " +
                        "showConfirmButton: false " +
                        "}).then(() => { window.location = 'Coach_class.aspx'; });", true);
                }
                else
                {
                    // 刪除失敗
                    ScriptManager.RegisterStartupScript(this, GetType(), "error",
                        "Swal.fire({ " +
                        "title: '刪除失敗', " +
                        "text: '課程不存在。', " +
                        "icon: 'error', " +
                        "confirmButtonText: '確定' });", true);
                }
            }
        }
    }



}