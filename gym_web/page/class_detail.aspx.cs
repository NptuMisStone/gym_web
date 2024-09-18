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

public partial class page_class_detail : System.Web.UI.Page
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    public static string Class_id, Time_id;
    public static DateTime Date_change, date;
    protected void Page_Load(object sender, EventArgs e)
    {
        Class_id = Convert.ToString(Session["Class_id"]);
        if (!IsPostBack)
        {
            Date_change=DateTime.Today;
            Bind_Class_Img();
            Bind_Class_Info_simple();
            Bind_Class_Info();
            Bind_Coach_Info();
            Bind_Time_Info();
            GetAddress();
            GetDate();
        }
    }
    private void Bind_Class_Img()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT 課程圖片,課程名稱 FROM [健身教練課程-有排課的] WHERE 課程編號 = @Class_id";
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Class_id", Class_id);
            SqlDataReader reader = command.ExecuteReader();
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
                Course_Name.Text = reader["課程名稱"].ToString();
            }
            connection.Close();
        }
    }
    private void Bind_Class_Info_simple()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query2 = "SELECT * FROM 健身教練課程 WHERE 課程編號 = @Class_id";
            connection.Open();
            SqlCommand command = new SqlCommand(query2, connection);
            command.Parameters.AddWithValue("@Class_id", Class_id);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Course_People.Text = reader["上課人數"].ToString();
                Course_Time.Text = reader["課程時間長度"].ToString();
                Course_Address.Text = reader["上課地點"].ToString();
            }
            connection.Close();
        }
    }
    private void Bind_Class_Info()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query2 = "SELECT * FROM [健身教練課程-有排課的] WHERE 課程編號 = @Class_id";
            connection.Open();
            SqlCommand command = new SqlCommand(query2, connection);
            command.Parameters.AddWithValue("@Class_id", Class_id);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Course_all_Type.Text = reader["分類名稱"].ToString();
                Course_all_People.Text = reader["上課人數"].ToString();
                Course_all_Cost.Text = Convert.ToDouble(reader["課程費用"]).ToString("F0");
                Course_all_Item.Text = reader["所需設備"].ToString();
                Course_all_Intro.Text = reader["課程內容介紹"].ToString();
                Course_all_Address.Text = reader["上課地點"].ToString();
            }
            connection.Close();
        }
    }
    private void Bind_Coach_Info()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query2 = "SELECT * FROM 健身教練課表課程合併 WHERE 課程編號 = @Class_id";
            connection.Open();
            SqlCommand command = new SqlCommand(query2, connection);
            command.Parameters.AddWithValue("@Class_id", Class_id);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                if (!reader.IsDBNull(reader.GetOrdinal("健身教練圖片")))
                {
                    // 將 VARBINARY 圖片資料轉換為 base64 編碼字串
                    byte[] imageData = (byte[])reader["健身教練圖片"];
                    string base64Image = Convert.ToBase64String(imageData);
                    // 將 base64 編碼的圖片字串設定為 <asp:Image> 控制項的來源
                    Coach_Img.ImageUrl = "data:image;base64," + base64Image;
                }
                else
                {
                    // 如果沒有圖片，使用預設圖片
                    /*img_de.ImageUrl = "img/team-1.jpg";*/  // 替換為你的預設圖片路徑
                }
                Coach_Name.Text = reader["健身教練姓名"].ToString();
                Coach_Phone.Text = reader["健身教練電話"].ToString();
                Coach_Mail.Text = reader["健身教練郵件"].ToString();
                Coach_Intro.Text = reader["健身教練介紹"].ToString();
                string gender = reader["健身教練性別"].ToString();
                switch (gender)
                {
                    case "1":
                        Coach_Gender.Text = "男生";
                        break;
                    case "2":
                        Coach_Gender.Text = "女生";
                        break;
                    case "3":
                        Coach_Gender.Text = "無性別";
                        break;
                }
            }
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
    public string GetAddress()
    {
        // 示例的資料庫操作代碼
        string address = "";
        string query = "SELECT [服務地點地址] FROM [健身教練課表課程合併] WHERE [課表編號] = @Class_id";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Class_id", Class_id);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read()) 
            {
                address = reader["服務地點地址"].ToString();
                Course_all_mapADD.Text = address;
                map.NavigateUrl = "https://www.google.com.tw/maps/place/" + address;
                //search是搜尋，place是直接顯示，但如果place沒找到地址，就沒有東西
            }
        }
        return address;
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
            if(TimeRepeater.Items.Count > 0) { noshow.Visible = false; }
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
                string script = @"<script>
                Swal.fire({
                  icon: 'error',
                  title: '請先登入！',
                  confirmButtonText: '確定',
                }).then((result) => {
                  if (result.isConfirmed) {
                     window.location.href = '../User/User_login.aspx';
                  }
                });
                </script>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlertScript", script, false);
            }
            else 
            {
                Session["Schedule_id"] = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("~/User/User_appointment.aspx");

            }
        }
    }
}