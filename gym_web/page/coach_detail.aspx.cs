using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class page_coach_detail : System.Web.UI.Page
{
    public static int user_loginsuccess;
    public static string User_id, Coach_id;
    protected void Page_Load(object sender, EventArgs e)
    {
        user_loginsuccess = Convert.ToInt32(Session["user_loginsuccess"]);
        if (!IsPostBack)
        {
            Coach_id = Convert.ToString(Session["Coach_id"]);
            LoadCoachDetails();
            BindClass();
        }
    }
    private void LoadCoachDetails()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM [健身教練審核合併] where [健身教練編號] = @Coach_id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Coach_id", Coach_id);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    lb_dename.Text = reader["健身教練姓名"].ToString();
                    lb_de_intr.Text = reader["健身教練介紹"].ToString();
                    lb_shop.Text = reader["服務地點名稱"].ToString();
                    lb_phone.Text = reader["健身教練電話"].ToString();
                    if (!reader.IsDBNull(reader.GetOrdinal("健身教練圖片")))
                    {
                        byte[] imageData = (byte[])reader["健身教練圖片"];
                        string base64Image = Convert.ToBase64String(imageData);
                        img_de.ImageUrl = "data:image;base64," + base64Image;
                    }
                    else
                    {
                        img_de.ImageUrl = "images/user.png";  // 替換為你的預設圖片路徑
                    }
                }
            }
            reader.Close();
        }
    }
    private void BindClass()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM [健身教練課程-有排課的] WHERE 健身教練編號 = @Coach_id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Coach_id", Coach_id);

            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                lv_classes.DataSource = dt;
                lv_classes.DataBind();
                lb_noClasses.Visible = false;  // 隱藏 "尚未安排課程" 的訊息
            }
            else
            {
                lv_classes.DataSource = null;
                lv_classes.DataBind();
                lb_noClasses.Visible = true;  // 顯示 "尚未安排課程" 的訊息
            }
        }
    }

    protected void lv_classes_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "ViewDetails")
        {
            // 獲取點擊的課程編號
            string classId = e.CommandArgument.ToString();

            // 將課程編號儲存到 Session 中
            Session["Class_id"] = classId;

            // 重定向到 class_detail.aspx
            Response.Redirect("class_detail.aspx");
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
                System.Drawing.Imaging.EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                // Get the JPG codec info
                ImageCodecInfo jpgCodec = ImageCodecInfo.GetImageEncoders()
                    .First(codec => codec.MimeType == "image/jpeg");

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
            return string.Empty; // 替代圖片的路徑
        }
    }

    protected string GetPeopleType(int num)
    {
        if (num > 1)
        {
            return "團體課程";
            
        }
        else if(num == 1)
        {
            return "一對一課程";
        }
        else
        {
            return "無法辨識";
        }
    }
}