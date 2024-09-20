using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Imaging;
using System.IO;

public partial class page_coach : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            coachdata();
        }
    }
    private void coachdata()
    {
        try
        {
            string conectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                string sql = "select 健身教練圖片,健身教練姓名,服務地點名稱,健身教練編號 from 健身教練合併 where 審核狀態 = 1";
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.SequentialAccess);
                rp_coachdata.DataSource = dataReader;
                rp_coachdata.DataBind();
            }
        }
        catch (Exception ex)
        {
            // Log or print the exception details
            Debug.WriteLine("Error retrieving data: " + ex.Message);
        }
    }
    protected void rp_coachdata_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "coach_detail")
        {
            string coachId = e.CommandArgument.ToString();
            Response.Redirect("coach_detail.aspx?no=" + coachId);
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
            return "img/team-1.jpg"; // 替代圖片的路徑
        }
    }
}