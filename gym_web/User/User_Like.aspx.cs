using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_User_Like : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    public static int User_id;
    protected void Page_Load(object sender, EventArgs e)
    {
        User_id = Convert.ToInt32(Session["User_id"]);
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
            if (!IsPostBack)
            {
                Coach_Count();
                BindCoach_Like();
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
    protected void Coach_Count()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "select count(*) from 教練被收藏 where 使用者編號=@使用者編號";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@使用者編號", User_id);
            connection.Open();
            int count = (int)command.ExecuteScalar();
            lb_count.Text = "您已收藏" + count + "個教練";
        }
    }
    private void BindCoach_Like()
    {
        using(SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "SELECT * FROM [使用者收藏-教練] WHERE [使用者編號]=@user_id ";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@user_id",User_id );
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            Like_CoachRP.DataSource = dt;
            Like_CoachRP.DataBind();
        }
        
    }
    protected void Like_CoachRP_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Edit_LikeBtn")
        {
            int coach_id = Convert.ToInt32(e.CommandArgument);
            ImageButton LikeBtn = e.Item.FindControl("LikeBtn") as ImageButton;
            if (LikeBtn.ImageUrl == "~/page/img/like.png")
            {
                LikeBtn.ImageUrl = "~/page/img/dislike.png";
                cancel_favorite(coach_id);//取消收藏
            }
            else
            {
                LikeBtn.ImageUrl = "~/page/img/like.png";
                add_to_favorite(coach_id);//收藏
            }
        }
    }
    private void cancel_favorite(int coach_id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "delete from 教練被收藏 where 使用者編號=@user_id and 健身教練編號=@coach_id";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@user_id", User_id);
            command.Parameters.AddWithValue("@coach_id", coach_id);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
    private void add_to_favorite(int coach_id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "insert into 教練被收藏(使用者編號,健身教練編號) values(@user_id,@coach_id)";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@user_id", User_id);
            command.Parameters.AddWithValue("@coach_id", coach_id);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}