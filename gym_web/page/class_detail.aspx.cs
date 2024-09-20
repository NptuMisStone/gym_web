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

public partial class page_class_detail : System.Web.UI.Page
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    public static string Schedule_id;
    protected void Page_Load(object sender, EventArgs e)
    {
        Schedule_id = Convert.ToString(Session["Schedule_id"]);
        if (!IsPostBack)
        {
            Bind_Course_Img();
            Bind_Course_Info_simple();
            Bind_Course_Info();
            Bind_Coach_Info();
        }
    }
    private void Bind_Course_Img()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT 課程圖片,課程名稱 FROM 健身教練課表課程合併 WHERE 課表編號 = @ScheduleID";
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ScheduleID", Schedule_id);
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
    private void Bind_Course_Info_simple()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query2 = "SELECT 上課人數,課程時間長度,日期,開始時間,結束時間,星期幾,上課地點 FROM 健身教練課表課程合併 WHERE 課表編號 = @ScheduleID";
            connection.Open();
            SqlCommand command = new SqlCommand(query2, connection);
            command.Parameters.AddWithValue("@ScheduleID", Schedule_id);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Course_People.Text = reader["上課人數"].ToString();
                Course_Time.Text = reader["課程時間長度"].ToString();
                Course_Schedule1.Text = Convert.ToDateTime(reader["日期"]).ToString("yyyy/MM/dd");
                Course_Schedule2.Text = reader["開始時間"].ToString();
                Course_Schedule3.Text = reader["結束時間"].ToString();
                Course_Schedule4.Text = reader["星期幾"].ToString();
                Course_Address.Text = reader["上課地點"].ToString();
            }
            connection.Close();
        }
    }
    private void Bind_Course_Info()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query2 = "SELECT * FROM 健身教練課表課程合併 WHERE 課表編號 = @ScheduleID";
            connection.Open();
            SqlCommand command = new SqlCommand(query2, connection);
            command.Parameters.AddWithValue("@ScheduleID", Schedule_id);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Course_all_Type.Text = reader["課程類型"].ToString();
                Course_all_People.Text = reader["上課人數"].ToString();
                Course_all_Cost.Text = Convert.ToDouble(reader["課程費用"]).ToString("F0");
                Course_all_Item.Text = reader["所需設備"].ToString();
                Course_all_Intro.Text = reader["課程內容介紹"].ToString();
                Course_all_Address.Text= reader["上課地點"].ToString() ;


            }
            connection.Close();
        }
    }
    private void Bind_Coach_Info()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query2 = "SELECT * FROM 健身教練課表課程合併 WHERE 課表編號 = @ScheduleID";
            connection.Open();
            SqlCommand command = new SqlCommand(query2, connection);
            command.Parameters.AddWithValue("@ScheduleID", Schedule_id);
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
                switch (gender){
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
}