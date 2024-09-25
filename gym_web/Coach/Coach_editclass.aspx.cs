using System;
using System.Collections.Generic;
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
    protected void Page_Load(object sender, EventArgs e)
    {
        Coach_id = Convert.ToString(Session["Coach_id"]);
        Class_id = Convert.ToString(Session["Class_id"]);

        //驗證教練是否登入的類別函數
        CoachHelper.CheckLogin(this);

        if (!IsPostBack)
        {
            Session["uploadedImage"] = null;
            LoadClassDetails();
            SetValidators();
            BindCourseTypes();
            BindRadioButtonList();
        }
        showTempimg();
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

                        if (locationType == 3)
                        {
                            tbClassLocation.Visible = true;
                            tbClassAddress.Visible = true;
                            tbClassLocation.Text = reader["地點名稱"].ToString();
                            tbClassAddress.Text = reader["地點地址"].ToString();
                        }

                        // 顯示圖片
                        if (!reader.IsDBNull(reader.GetOrdinal("課程圖片")))
                        {
                            // 如果圖片欄位不為 DBNull，則讀取圖片
                            Session["uploadedImage"] = (byte[])reader["課程圖片"];
                        }
                        else
                        {
                            // 如果圖片欄位為 DBNull，則設置預設圖片或清空 Session 中的圖片資料
                            Session["uploadedImage"] = null;
                        }
                        BindDropDownList();
                    }
                }
            }
        }
    }
    private void BindDropDownList()
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"SELECT * FROM [運動分類清單]";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Class_id", Class_id);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            ddlCourseType.DataSource = reader;
            ddlCourseType.DataTextField = "分類名稱";
            ddlCourseType.DataValueField = "分類編號";
            ddlCourseType.DataBind();
        }
        ddlCourseType.SelectedValue = classType_id;
    }
    private void BindCourseTypes()
    {
        string query = "SELECT * FROM [運動分類清單]";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                ddlCourseType.DataSource = reader;
                ddlCourseType.DataValueField = "分類編號";
                ddlCourseType.DataTextField = "分類名稱";
                ddlCourseType.DataBind();
            }
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

    protected void btnUpdateCourse_Click(object sender, EventArgs e)
    {
        // 確認頁面上的所有驗證是否通過
        if (Page.IsValid)
        {
            // 準備更新的 SQL 語句
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
                             [課程圖片] = @CourseImage
                         WHERE [課程編號] = @ClassID";

            // 獲取表單中的資料
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
            byte[] courseImage;

            if (Session["uploadedImage"] != null)
            {
                courseImage = (byte[])Session["uploadedImage"];
            }
            else
            {
                // 否則使用預設圖片
                string imagePath = Server.MapPath("~/Coach/img/課程預設圖.png");
                courseImage = File.ReadAllBytes(imagePath);
            }

            // 連接資料庫，並執行更新
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // 設定參數值
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
                    command.Parameters.AddWithValue("@ClassID", Class_id);

                    // 判斷是否有圖片上傳，並設置圖片參數
                    if (courseImage != null && courseImage.Length > 0)
                    {
                        command.Parameters.AddWithValue("@CourseImage", courseImage);
                    }
                    else
                    {
                        // 若無圖片，則設定為資料庫原本圖片
                        command.Parameters.AddWithValue("@CourseImage", Session["uploadedImage"] ?? DBNull.Value);
                    }

                    // 開啟連接並執行命令
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // 新增成功後，彈出提示並跳轉到 Coach_class.aspx
                        string script = @"
                Swal.fire({
                    icon: 'success',
                    title: '更新成功',
                    text: '課程已更新',
                    showConfirmButton: false,
                    timer: 1500
                }).then(function() {
                    window.location = 'Coach_class.aspx'; // 導向到 Coach_class.aspx
                });";

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlertScript", script, true);
                    }
                    else
                    {
                        string script = @"
                Swal.fire({
                    icon: 'error',
                    title: '更新失敗',
                    text: '資料填寫不完整或有誤，請檢查後再試',
                    showConfirmButton: false,
                    timer: 1500
                }).then(function() {
                    window.location = 'Coach_class.aspx'; // 導向到 Coach_class.aspx
                });";

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlertScript", script, true);
                    }
                }
            }
        }
        else
        {
            string script = @"
                Swal.fire({
                    icon: 'error',
                    title: '更新失敗',
                    text: '資料填寫不完整或有誤，請檢查後再試',
                    showConfirmButton: false,
                    timer: 1500
                }).then(function() {
                    window.location = 'Coach_class.aspx'; // 導向到 Coach_class.aspx
                });";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlertScript", script, true);
        }
    }
    protected void btnDeleteCourse_Click(object sender, EventArgs e)
    {
        string qry = @"DELETE FROM 健身教練課程 WHERE 課程編號 =@Class_id ";
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(qry, conn))
            {
                command.Parameters.AddWithValue("@Class_id", Class_id);
                conn.Open();
                command.ExecuteReader();
                conn.Close();

                string script = @"
                Swal.fire({
                    icon: 'success',
                    title: '刪除成功',
                    text: '課程已刪除',
                    showConfirmButton: false,
                    timer: 1500
                }).then(function() {
                    window.location = 'Coach_class.aspx'; // 導向到 Coach_class.aspx
                });";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlertScript", script, true);
            }
        }
    }
    protected void rdlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblLocation.SelectedItem.Value == "3") // 選擇其他選項
        {
            tbClassLocation.Visible = true; // 顯示指定地點輸入框
            tbClassAddress.Visible = true;
            rfvClassLocation.Enabled = true; // 啟用指定地點的必填驗證
            rfvClassAddress.Enabled = true;
        }
        else // 選擇到府選項
        {
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
            tbClassAddress.Visible = true;
            rfvClassLocation.Enabled = true;
            rfvClassAddress.Enabled = true;
        }
        else // 到府上課
        {
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
}