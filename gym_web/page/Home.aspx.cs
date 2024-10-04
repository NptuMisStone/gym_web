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

public partial class Home : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCarouselImages();
            Coachdata();
        }
    }
    private void LoadCarouselImages()
    {
        List<Tuple<string, string>> imagesAndLinks = new List<Tuple<string, string>>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = @"
                SELECT [圖片], [連結] 
                FROM [廣告]
                WHERE GETDATE() BETWEEN [上架日] AND [下架日]";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        byte[] imageData = (byte[])reader["圖片"];
                        string base64String = Convert.ToBase64String(imageData);
                        string imageUrl = "data:image/jpeg;base64," + base64String;
                        string link = reader["連結"].ToString();
                        imagesAndLinks.Add(new Tuple<string, string>(imageUrl, link));
                    }
                }
            }
            connection.Close();
        }

        Repeater1.DataSource = imagesAndLinks;
        Repeater1.DataBind();
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
            return "img/user.png"; // 替代圖片的路徑
        }
    }
    private void Coachdata()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "select TOP(3) 健身教練圖片,健身教練姓名,註冊類型,健身教練編號,人氣指數 from [首頁-人氣指數] where 審核狀態 = 1 ORDER BY [健身教練次數] DESC";
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.SequentialAccess);
                lv_coachdata.DataSource = dataReader;
                lv_coachdata.DataBind();
                string[] images = { "first.png", "second.png", "third.png" };

                for (int i = 0; i < lv_coachdata.Items.Count; i++)
                {
                    ListViewDataItem item = lv_coachdata.Items[i];
                    Image coachImage = (Image)item.FindControl("Image2");

                    if (i < images.Length)
                    {
                        coachImage.ImageUrl = "img/" + images[i];
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log or print the exception details
            Debug.WriteLine("Error retrieving data: " + ex.Message);
        }
    }

    protected void lv_coachdata_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "coach_detail")
        {
            // 取得教練編號，存入 Session，並跳轉至詳細頁面
            Session["coach_num"] = Convert.ToInt32(e.CommandArgument);
            Response.Redirect("coach_detail.aspx");
        }
    }

    protected void MoreCoachBtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("coach.aspx");
    }

    protected void RegisterUserBtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/User/User_register.aspx");
    }

    protected void RegisterCoachBtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Coach/Coach_register.aspx");
    }
}