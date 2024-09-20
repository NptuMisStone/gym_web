using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Coach_Coach_class : System.Web.UI.Page
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    public static string Coach_id;

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
        string query = @"SELECT c.*, ct.分類名稱 FROM [健身教練課程] c JOIN [運動分類清單] ct ON c.課程類型 = ct.分類編號 WHERE c.[健身教練編號] = @CoachID";

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
        string query = "SELECT [註冊類型], [服務地點名稱] FROM [健身教練合併] WHERE [健身教練編號] = @CoachID";

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

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        BindCourses();
    }


    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        BindCourses();
    }

    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int courseID = (int)GridView1.DataKeys[e.RowIndex].Value;
        string courseName = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("tbCourseName")).Text;
        int courseType = Convert.ToInt32(((DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlCourseType")).SelectedValue);
        string courseDescription = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("tbCourseDescription")).Text;
        int courseDuration = Convert.ToInt32(((TextBox)GridView1.Rows[e.RowIndex].FindControl("tbCourseDuration")).Text);
        int classSize = Convert.ToInt32(((TextBox)GridView1.Rows[e.RowIndex].FindControl("tbClassSize")).Text);
        string classLocation = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("tbClassLocation")).Text;
        decimal courseFee = Convert.ToDecimal(((TextBox)GridView1.Rows[e.RowIndex].FindControl("tbCourseFee")).Text);
        string requiredEquipment = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("tbRequiredEquipment")).Text;

        FileUpload fuCourseImage = (FileUpload)GridView1.Rows[e.RowIndex].FindControl("fuCourseImage");
        byte[] courseImage = null;
        if (fuCourseImage.HasFile)
        {
            using (Stream fs = fuCourseImage.PostedFile.InputStream)
            using (BinaryReader br = new BinaryReader(fs))
            {
                courseImage = br.ReadBytes((Int32)fs.Length);
            }
        }

        string query = "UPDATE [健身教練課程] SET [課程名稱] = @CourseName, [課程類型] = @CourseType, [課程內容介紹] = @CourseDescription, [課程時間長度] = @CourseDuration, [上課人數] = @ClassSize, [上課地點] = @ClassLocation, [課程費用] = @CourseFee, [所需設備] = @RequiredEquipment, [課程圖片] = @CourseImage WHERE [課程編號] = @CourseID";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CourseID", courseID);
                command.Parameters.AddWithValue("@CourseName", courseName);
                command.Parameters.AddWithValue("@CourseType", courseType);
                command.Parameters.AddWithValue("@CourseDescription", courseDescription);
                command.Parameters.AddWithValue("@CourseDuration", courseDuration);
                command.Parameters.AddWithValue("@ClassSize", classSize);
                command.Parameters.AddWithValue("@ClassLocation", classLocation);
                command.Parameters.AddWithValue("@CourseFee", courseFee);
                command.Parameters.AddWithValue("@RequiredEquipment", requiredEquipment);
                command.Parameters.AddWithValue("@CourseImage", courseImage != null ? (object)courseImage : DBNull.Value);

                connection.Open();
                command.ExecuteNonQuery();
                GridView1.EditIndex = -1;
                BindCourses();
            }
        }
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int courseID = (int)GridView1.DataKeys[e.RowIndex].Value;
        string query = "DELETE FROM [健身教練課程] WHERE [課程編號] = @CourseID";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CourseID", courseID);
                connection.Open();
                command.ExecuteNonQuery();
                BindCourses();
            }
        }
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

            string query = "INSERT INTO [健身教練課程] ([課程名稱], [課程類型], [課程內容介紹], [課程時間長度], [上課人數], [上課地點], [課程費用], [所需設備], [課程圖片], [健身教練編號]) VALUES (@CourseName, @CourseType, @CourseDescription, @CourseDuration, @ClassSize, @ClassLocation, @CourseFee, @RequiredEquipment, @CourseImage, @CoachID)";
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
}
