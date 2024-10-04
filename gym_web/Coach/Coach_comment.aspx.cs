using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.DynamicData;

public partial class Coach_Coach_comment : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    public static string Coach_id;
    protected void Page_Load(object sender, EventArgs e)
    {
        Coach_id = Convert.ToString(Session["Coach_id"]);
        //驗證教練是否登入的類別函數
        CoachHelper.CheckLogin(this);
        if (!IsPostBack)
        {
            BindComment();
        }
    }
    private void BindComment() 
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = @"
                    SELECT * FROM [查看評論] WHERE [健身教練編號] = @Coach_id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Coach_id", Coach_id);
                SqlDataReader reader = command.ExecuteReader();
                UserReviewsRepeater.DataSource = reader;
                UserReviewsRepeater.DataBind();
                foreach (RepeaterItem item in UserReviewsRepeater.Items)
                {
                    ImageButton clickedButton = (ImageButton)item.FindControl("ImageButton1");
                    TextBox replyTextBox = (TextBox)item.FindControl("ReplyTextBox");
                    Button submitButton = (Button)item.FindControl("SubmitReplyButton");
                    if (replyTextBox.Visible == true && submitButton.Visible == true)
                    {
                        clickedButton.Visible = false;
                    }
                }
            }
        }
    }
    protected string GenerateStars(int rating)
    {
        string stars = "";
        for (int i = 0; i < rating; i++)
        {
            stars += "★";
        }
        return stars;
    }
    protected void SubmitReplyButton_Click(object sender, EventArgs e)
    {
        Button submitButton = (Button)sender;
        int reviewID = Convert.ToInt32(submitButton.CommandArgument);
        RepeaterItem item = (RepeaterItem)submitButton.NamingContainer;
        TextBox replyTextBox = (TextBox)item.FindControl("ReplyTextBox");
        string replyText = replyTextBox.Text;

        SaveReplyToDatabase(reviewID, replyText);

        Response.Redirect(Request.RawUrl);
        
    }

    private void SaveReplyToDatabase(int reviewID, string replyText)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string updateQuery = "UPDATE [完成預約評論表] SET [回覆] = @回覆 WHERE [評論編號] = @評論編號";

            using (SqlCommand command = new SqlCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@回覆", replyText);
                command.Parameters.AddWithValue("@評論編號", reviewID);
                command.ExecuteNonQuery();
            }

        }
        Response.Redirect(Request.RawUrl);
    }
    protected string GetAvatarUrl(object avatarData)
    {
        if (avatarData != DBNull.Value && avatarData != null)
        {
            byte[] avatarBytes = (byte[])avatarData;
            string base64String = Convert.ToBase64String(avatarBytes);
            return "data:image/png;base64," + base64String;
        }
        else
        {
            return "img/user.png";
        }
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        // 获取点击的 ImageButton1
        ImageButton clickedButton = (ImageButton)sender;

        // 查找 ImageButton1 所在的 Repeater Item
        RepeaterItem item = (RepeaterItem)clickedButton.Parent;

        // 查找 Repeater Item 中的 ReplyTextBox 和 SubmitReplyButton
        TextBox replyTextBox = (TextBox)item.FindControl("ReplyTextBox");
        Button submitButton = (Button)item.FindControl("SubmitReplyButton");
        Button cancelButton = (Button)item.FindControl("CancelReplyButton");
        // 设置 ReplyTextBox 和 SubmitReplyButton 可见
        replyTextBox.Visible = true;
        submitButton.Visible = true;
        cancelButton.Visible = true;
        if (replyTextBox.Visible == true && submitButton.Visible == true)
        {
            clickedButton.Visible = false;
        }
    }

    protected void CancelReplyButton_Click(object sender, EventArgs e)
    {
        Button cancelButton = (Button)sender;
        RepeaterItem item = (RepeaterItem)cancelButton.Parent;
        ImageButton clickedButton = (ImageButton)item.FindControl("ImageButton1");
        TextBox replyTextBox = (TextBox)item.FindControl("ReplyTextBox");
        Button submitButton = (Button)item.FindControl("SubmitReplyButton");
        replyTextBox.Visible = false;
        submitButton.Visible = false;
        cancelButton.Visible = false;
        clickedButton.Visible=true;
        BindComment();
    }
}