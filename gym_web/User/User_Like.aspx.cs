using System;
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

public partial class User_User_Like : System.Web.UI.Page
{
    string conectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        // 驗證用戶是否登入的類別函數
        CheckLogin.CheckUserOrCoachLogin(this.Page, "User");

        if (!IsPostBack)
        {
            coachdata();
            classdata();
        }
    }
    private void coachdata()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                // 只顯示用戶已收藏的教練
                string sql = @"
                SELECT 
                    健身教練審核合併.健身教練圖片,
                    健身教練審核合併.健身教練姓名,
                    健身教練審核合併.註冊類型,
                    健身教練審核合併.健身教練編號 
                FROM 
                    教練被收藏 
                JOIN 
                    健身教練審核合併 ON 教練被收藏.健身教練編號 = 健身教練審核合併.健身教練編號
                WHERE 
                    教練被收藏.使用者編號 = @userId";

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@userId", Convert.ToInt32(Session["User_id"]));

                connection.Open();
                SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.SequentialAccess);
                lv_coachdata.DataSource = dataReader;
                lv_coachdata.DataBind();
            }
        }
        catch (Exception ex)
        {
            // Log or print the exception details
            Debug.WriteLine("Error retrieving data: " + ex.Message);
        }
    }
    private void classdata()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(conectionString))
            {
                // 只顯示用戶已收藏的有排課的課程
                string sql = @"SELECT * FROM 收藏課程 WHERE 使用者編號=@userId";

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@userId", Convert.ToInt32(Session["User_id"]));

                connection.Open();
                SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.SequentialAccess);
                lv_classdata.DataSource = dataReader;
                lv_classdata.DataBind();
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
            Response.Redirect("~/page/coach_detail.aspx");
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
    protected string GetLikeImageUrl(object coachId)
    {
        if (Session["User_id"] == null)
        {
            return "img/dislike3.png";
        }

        int userId = Convert.ToInt32(Session["User_id"]);
        int coachNum = Convert.ToInt32(coachId);

        using (SqlConnection connection = new SqlConnection(conectionString))
        {
            connection.Open();
            string sql = "SELECT COUNT(*) FROM 教練被收藏 WHERE 健身教練編號 = @likecoach_id AND 使用者編號 = @likeuser_id";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@likecoach_id", coachNum);
            command.Parameters.AddWithValue("@likeuser_id", userId);

            int count = (int)command.ExecuteScalar();
            return count > 0 ? "img/like1.png" : "img/dislike3.png";
        }
    }
    protected string GetClassLikeImageUrl(object classID)
    {
        if (Session["User_id"] == null)
        {
            return "img/dislike2.png";
        }

        int userId = Convert.ToInt32(Session["User_id"]);
        int classId = Convert.ToInt32(classID);

        using (SqlConnection connection = new SqlConnection(conectionString))
        {
            connection.Open();
            string sql = "SELECT COUNT(*) FROM 課程被收藏 WHERE 課程編號 = @likeclass_id AND 使用者編號 = @likeuser_id";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@likeclass_id", classId);
            command.Parameters.AddWithValue("@likeuser_id", userId);

            int count = (int)command.ExecuteScalar();
            return count > 0 ? "img/like1.png" : "img/dislike2.png";
        }
    }
    protected void LikeBtn_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btn = (ImageButton)sender;
        int coachNum = Convert.ToInt32(btn.CommandArgument);
        int userId = Convert.ToInt32(Session["User_id"]);

        using (SqlConnection connection = new SqlConnection(conectionString))
        {
            connection.Open();

            if (btn.ImageUrl == "img/dislike3.png")
            {
                // 喜歡教練，插入收藏記錄
                string sql = "INSERT INTO 教練被收藏 (使用者編號, 健身教練編號) VALUES (@likeuser_id, @likecoach_id)";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@likeuser_id", userId);
                command.Parameters.AddWithValue("@likecoach_id", coachNum);
                command.ExecuteNonQuery();

                btn.ImageUrl = "img/like1.png";
            }
            else
            {
                // 取消喜歡，刪除收藏記錄
                string sql = "DELETE FROM 教練被收藏 WHERE 健身教練編號 = @dislikecoach_id AND 使用者編號 = @dislikeuser_id";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@dislikecoach_id", coachNum);
                command.Parameters.AddWithValue("@dislikeuser_id", userId);
                command.ExecuteNonQuery();

                btn.ImageUrl = "img/dislike3.png";
            }
        }
        coachdata();
        classdata();
    }

    protected void ClassLikeBtn_Click(object sender, ImageClickEventArgs e)
    {
        // 驗證用戶是否登入的類別函數
        CheckLogin.CheckUserOrCoachLogin(this.Page, "User");

        ImageButton btn = (ImageButton)sender;
        int classId = Convert.ToInt32(btn.CommandArgument);
        int userId = Convert.ToInt32(Session["User_id"]);

        using (SqlConnection connection = new SqlConnection(conectionString))
        {
            connection.Open();

            if (btn.ImageUrl == "img/dislike2.png")
            {
                // 喜歡課程，插入收藏記錄
                string sql = "INSERT INTO 課程被收藏 (使用者編號, 課程編號) VALUES (@likeuser_id, @likeclass_id)";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@likeuser_id", userId);
                command.Parameters.AddWithValue("@likeclass_id", classId);
                command.ExecuteNonQuery();

                btn.ImageUrl = "img/like1.png";
            }
            else
            {
                // 取消喜歡，刪除收藏記錄
                string sql = "DELETE FROM 課程被收藏 WHERE 課程編號 = @dislikeclass_id AND 使用者編號 = @dislikeuser_id";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@dislikeclass_id", classId);
                command.Parameters.AddWithValue("@dislikeuser_id", userId);
                command.ExecuteNonQuery();

                btn.ImageUrl = "img/dislike2.png";
            }
        }
        coachdata();
        classdata();
    }

    protected void lv_classdata_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "see_detail")
        {
            // 取得課程編號，存入 Session，並跳轉至詳細頁面
            Session["Class_id"] = Convert.ToInt32(e.CommandArgument);
            Response.Redirect("~/page/class_detail.aspx");
        }
    }
}
