using System;
using System.Collections.Generic;
using System.Configuration;
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
    public static string User_id;
    protected void Page_Load(object sender, EventArgs e)
    {
        user_loginsuccess = Convert.ToInt32(Session["user_loginsuccess"]);
        if (!IsPostBack)
        {
            string coachId = Request.QueryString["no"];
            if (!string.IsNullOrEmpty(coachId))
            {
                LoadCoachDetails(coachId);
            }
        }
    }
    private void LoadCoachDetails(string coachId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM [健身教練合併] where [健身教練編號] = @CoachId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CoachId", coachId);

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
                        img_de.ImageUrl = "img/team-1.jpg";  // 替換為你的預設圖片路徑
                    }
                }
            }
            reader.Close();
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

    protected void btn_ap_Click(object sender, EventArgs e)
    {
        if (user_loginsuccess == 1)
        {
            Session["ap_Coach_id"] = Request.QueryString["no"];
            Session["User_id"] = User_id;
            Debug.WriteLine(User_id);
            Debug.WriteLine(Request.QueryString["no"]);
            Response.Redirect("../User/User_appointment.aspx");
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('請登入後再預約!');", true);
        }
    }
}