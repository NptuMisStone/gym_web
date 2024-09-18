using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Coach_Coach_class : System.Web.UI.Page
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    public static string Coach_id, Class_id, classType_id;

    protected void Page_Load(object sender, EventArgs e)
    {
        Coach_id = Convert.ToString(Session["Coach_id"]);

        //驗證教練是否登入的類別函數
        CoachHelper.CheckLogin(this);

        if (!IsPostBack)
        {
            BindCourses();
            BindCourseTypes();
            BindRadioButtonList();
        }
    }

    private void BindCourses()
    {
        string query = @"SELECT c.*, ct.分類名稱 FROM [健身教練課程] c JOIN [運動分類清單] ct ON c.分類編號 = ct.分類編號 WHERE c.[健身教練編號] = @CoachID";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CoachID", Coach_id);
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }
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
        ddlCourseType.Items.Insert(0, new ListItem("--選擇課程類型--", ""));
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
        rblLocation.SelectedIndex = 0;
        tbClassLocation.Text = "";
        tbClassLocation.Visible = false;
        tbClassSize.Text = "";
        tbClassSize.Visible = false;
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "SelectClass")
        {
            // 取得課程編號
            Class_id = e.CommandArgument.ToString();
            LoadClassDetails();
        }
    }

    private void LoadClassDetails()
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#" + ClassPanel.ClientID + "').modal('show');", true);
        ClassPanel.Visible = true;

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
                        detailName.Text = reader["課程名稱"].ToString();
                        detailTime.Text = reader["課程時間長度"].ToString();
                        detailpeople.Text = reader["上課人數"].ToString();
                        detailmoney.Text = reader["課程費用"].ToString();
                        detailitem.Text = reader["所需設備"].ToString();
                        detailplace.Text = reader["上課地點"].ToString();
                        detailintro.Text = reader["課程內容介紹"].ToString();

                        // 顯示圖片
                        img_Course.ImageUrl = GetImageUrl(reader["課程圖片"], 50);
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

            detailType.DataSource = reader;
            detailType.DataTextField = "分類名稱";
            detailType.DataValueField = "分類編號";
            detailType.DataBind();
        }
        detailType.SelectedValue = classType_id;
    }


    protected void Class_delete_Click(object sender, EventArgs e)
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
                BindCourses();
                ClassPanel.Visible = false;
                string script = @"<script>
                            Swal.fire({
                            icon: 'success',
                            title: '刪除成功',
                            text: '課程已刪除',
                            showConfirmButton: false,
                            timer: 1500
                            });
                          </script>";

                Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);

            }
        }

    }

    protected void Class_cancel_Click(object sender, EventArgs e)
    {
        ClassPanel.Visible = false;
    }

    protected void Class_save_Click(object sender, EventArgs e)
    {
        // 從 TextBox 讀取新資料
        string courseName = detailName.Text;
        int courseDuration = Convert.ToInt32(detailTime.Text);
        int coursePeople = Convert.ToInt32(detailpeople.Text);
        decimal courseFee = Convert.ToDecimal(detailmoney.Text);
        string requiredEquipment = detailitem.Text;
        string courseLocation = detailplace.Text;
        string courseDescription = detailintro.Text;
        int courseType = Convert.ToInt32(detailType.SelectedValue);

        if (FileUpload1.HasFile)
        {
            updateclassImage();
        }

        // 構建更新的 SQL 語句
        string query = @"UPDATE [健身教練課程] 
                     SET [課程名稱] = @CourseName,
                         [課程時間長度] = @CourseDuration,
                         [上課人數] = @CoursePeople,
                         [課程費用] = @CourseFee,
                         [所需設備] = @RequiredEquipment,
                         [上課地點] = @CourseLocation,
                         [課程內容介紹] = @CourseDescription,
                         [分類編號] = @CourseType
                     WHERE [課程編號] = @ClassID";

        // 更新資料庫
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // 添加參數
                command.Parameters.AddWithValue("@CourseName", courseName);
                command.Parameters.AddWithValue("@CourseDuration", courseDuration);
                command.Parameters.AddWithValue("@CoursePeople", coursePeople);
                command.Parameters.AddWithValue("@CourseFee", courseFee);
                command.Parameters.AddWithValue("@RequiredEquipment", requiredEquipment);
                command.Parameters.AddWithValue("@CourseLocation", courseLocation);
                command.Parameters.AddWithValue("@CourseDescription", courseDescription);
                command.Parameters.AddWithValue("@CourseType", courseType);
                command.Parameters.AddWithValue("@ClassID", Class_id); // 課程編號

                // 開啟連接並執行命令
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // 更新成功提示
        BindCourses(); // 重新綁定課程清單
        ClassPanel.Visible = false;
        string script = @"<script>
                        Swal.fire({
                            icon: 'success',
                            title: '更新成功',
                            text: '課程已更新',
                            showConfirmButton: false,
                            timer: 1500
                        });
                      </script>";

        // 在頁面上顯示提示訊息
        Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
    }


    protected void btnAddCourse_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            // 執行表單提交邏輯
            string courseName = tbCourseName.Text;
            int courseType = Convert.ToInt32(ddlCourseType.SelectedValue);
            string courseDescription = tbCourseDescription.Text;
            int courseDuration = Convert.ToInt32(tbCourseDuration.Text);
            decimal courseFee = Convert.ToDecimal(tbCourseFee.Text);
            string requiredEquipment = tbRequiredEquipment.Text;

            byte[] courseImage = null;
            if (fuCourseImage.HasFile)
            {
                using (Stream fs = fuCourseImage.PostedFile.InputStream)
                using (BinaryReader br = new BinaryReader(fs))
                {
                    courseImage = br.ReadBytes((Int32)fs.Length);
                }
            }
            else
            {
                string imagePath = Server.MapPath("~/Coach/images/課程預設圖.jpg");
                courseImage = File.ReadAllBytes(imagePath);
            }

            int classSize = 0;
            if (rblClassSize.SelectedItem.Value == "1")
            {
                classSize = 1;
            }
            else if (rblClassSize.SelectedItem.Value == "2")
            {
                classSize = Convert.ToInt32(tbClassSize.Text);
            }

            string classLocation = "";
            if (rblLocation.SelectedValue == "3")
            {
                classLocation = tbClassLocation.Text;
            }
            else
            {
                classLocation = rblLocation.SelectedItem.Text;
            }

            string query = "INSERT INTO [健身教練課程] ([課程名稱], [分類編號], [課程內容介紹], [課程時間長度], [上課人數], [上課地點], [課程費用], [所需設備], [課程圖片], [健身教練編號]) VALUES (@CourseName, @CourseType, @CourseDescription, @CourseDuration, @ClassSize, @ClassLocation, @CourseFee, @RequiredEquipment, @CourseImage, @CoachID)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CourseName", courseName);
                    command.Parameters.AddWithValue("@CourseType", courseType);
                    command.Parameters.AddWithValue("@CourseDescription", courseDescription);
                    command.Parameters.AddWithValue("@CourseDuration", courseDuration);
                    command.Parameters.AddWithValue("@ClassSize", classSize);
                    command.Parameters.AddWithValue("@ClassLocation", classLocation);
                    command.Parameters.AddWithValue("@CourseFee", courseFee);
                    command.Parameters.AddWithValue("@RequiredEquipment", requiredEquipment);
                    command.Parameters.AddWithValue("@CourseImage", courseImage);
                    command.Parameters.AddWithValue("@CoachID", Coach_id);

                    connection.Open();
                    command.ExecuteNonQuery();

                    string script = @"
                Swal.fire({
                    icon: 'success',
                    title: '新增成功',
                    text: '課程已更新',
                    showConfirmButton: false,
                    timer: 1500
                }).then(function() {
                    window.location = '" + Request.RawUrl + @"';
                });";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlertScript", script, true);
                }
            }
        }
        
    }
    protected void rdLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblLocation.SelectedItem.Value == "3")
        {
            tbClassLocation.Visible = true;
        }
        else
        {
            tbClassLocation.Text = "";
            tbClassLocation.Visible = false;
        }
    }
    protected void rblClassSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblClassSize.SelectedItem.Value == "2")
        {
            tbClassSize.Visible = true;
            rblLocation.Items.FindByValue("2").Enabled=false;
            if (rblLocation.SelectedValue == "2")
            {
                rblLocation.ClearSelection();
                rblLocation.Items.FindByValue("3").Selected = true; // 選擇 "其他(教練指定地點)"
                tbClassLocation.Visible = true;
            }
        }
        else
        {
            tbClassSize.Text = "";
            tbClassSize.Visible = false;
            rblLocation.Items.FindByValue("2").Enabled = true;
        }
    }
    protected void cvClassSize_ServerValidate(object source, ServerValidateEventArgs args)
    {
        int classSize;
        if(rblClassSize.SelectedValue == "1") 
        {
            args.IsValid = true;
        }
        else
        {
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
    private void updateclassImage()
    {
        // 獲取上傳的文件名
        string fileName = Path.GetFileName(FileUpload1.FileName);

        // 構建服務器上的文件路徑
        string filePath = Server.MapPath("~/Uploads/" + fileName);

        // 讀取上傳的文件字節數組
        byte[] imageData = FileUpload1.FileBytes;

        // 將圖片數據插入到數據庫
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "UPDATE 健身教練課程 SET 課程圖片 = @ProfileImage WHERE 課程編號 = @ClassID";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ProfileImage", imageData);
                command.Parameters.AddWithValue("@ClassID", Class_id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

}
