using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

public partial class Admin_AD_review : System.Web.UI.Page
{
    string conectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ShowPanel("新增廣告");
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        DateTime startDate = DateTime.Parse(tb_date_start.Text);
        DateTime endDate = DateTime.Parse(tb_date_end.Text);

        if (startDate > endDate)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('下架日不能早於上架日');", true);
            return;
        }

        using (SqlConnection sqlcn = new SqlConnection(conectionString))
        {
            sqlcn.Open();
            byte[] imageData = FileUpload1.FileBytes;

            string sql = "INSERT INTO 廣告 (名稱, 圖片, 上架日, 下架日, 連結) VALUES (@ad_name, @ad_picture, @ad_date_start, @ad_date_end, @ad_url)";
            SqlCommand command = new SqlCommand(sql, sqlcn);
            command.Parameters.AddWithValue("@ad_name", tb_name.Text);
            command.Parameters.AddWithValue("@ad_picture", imageData);
            command.Parameters.AddWithValue("@ad_date_start", tb_date_start.Text);
            command.Parameters.AddWithValue("@ad_date_end", tb_date_end.Text);
            command.Parameters.AddWithValue("@ad_url", tb_url.Text);
            command.ExecuteNonQuery();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('廣告新增成功');", true);
        }
    }


    protected void Button2_Click(object sender, EventArgs e)
    {
        Panel_Edit.Visible = false;
        ShowPanel("新增廣告");
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        Panel_Edit.Visible=false;
        ShowPanel("有效廣告");
        LoadAds();
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        Panel_Edit.Visible = false;
        ShowPanel("過期廣告");
        LoadExpiredAds();
    }

    protected void Button5_Click(object sender, EventArgs e)
    {
        Panel_Edit.Visible = false;
        ShowPanel("未來廣告");
        LoadFutureAds();
    }

    private void ShowPanel(string panelName)
    {
        Panel1.Visible = panelName == "新增廣告";
        Panel2.Visible = panelName == "有效廣告";
        Panel3.Visible = panelName == "過期廣告";
        Panel4.Visible = panelName == "未來廣告";
        Panel_Edit.Visible = panelName == "有效廣告" && Panel_Edit.Visible;
    }

    private void LoadAds()
    {
        using (SqlConnection connection = new SqlConnection(conectionString))
        {
            string query = "SELECT [編號], [名稱], [圖片], [上架日], [下架日], [連結] FROM [廣告] WHERE GETDATE() BETWEEN [上架日] AND [下架日]";
            SqlCommand command = new SqlCommand(query, connection);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            rp_ads.DataSource = dt;
            rp_ads.DataBind();
        }
    }

    protected void rp_ads_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "edit_ad")
        {
            string adId = e.CommandArgument.ToString();
            LoadAdForEditing(adId);
            Panel_Edit.Visible = true;
            Panel2.Visible = false;
            Panel3.Visible = false;
            Panel4.Visible = false;
        }
    }

    private void LoadAdForEditing(string adId)
    {
        using (SqlConnection connection = new SqlConnection(conectionString))
        {
            string query = "SELECT [名稱], [圖片], [上架日], [下架日], [連結] FROM [廣告] WHERE [編號] = @ad_id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ad_id", adId);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                hf_ad_id.Value = adId;
                tb_edit_name.Text = reader["名稱"].ToString();
                tb_edit_date_start.Text = Convert.ToDateTime(reader["上架日"]).ToString("yyyy-MM-dd");
                tb_edit_date_end.Text = Convert.ToDateTime(reader["下架日"]).ToString("yyyy-MM-dd");
                tb_edit_url.Text = reader["連結"].ToString();

                // Handling image upload
                byte[] imageBytes = reader["圖片"] as byte[];
                if (imageBytes != null)
                {
                    string base64Image = Convert.ToBase64String(imageBytes);
                    string imageUrl = "data:image/png;base64," + base64Image;
                    // Set image URL to a hidden image control for preview
                    // e.g., img_preview.ImageUrl = imageUrl;
                }
            }
        }
    }

    protected void btn_update_Click(object sender, EventArgs e)
    {
        using (SqlConnection connection = new SqlConnection(conectionString))
        {
            connection.Open();

            if (FileUpload2.HasFile)
            {
                updateADImage();
            }

            string query = "UPDATE [廣告] SET [名稱] = @ad_name, [上架日] = @ad_date_start, [下架日] = @ad_date_end, [連結] = @ad_url WHERE [編號] = @ad_id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ad_name", tb_edit_name.Text);
            command.Parameters.AddWithValue("@ad_date_start", tb_edit_date_start.Text);
            command.Parameters.AddWithValue("@ad_date_end", tb_edit_date_end.Text);
            command.Parameters.AddWithValue("@ad_url", tb_edit_url.Text);
            command.Parameters.AddWithValue("@ad_id", hf_ad_id.Value);
            command.ExecuteNonQuery();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('廣告更新成功');", true);

            Panel_Edit.Visible = false;
            LoadAds(); // 刷新廣告列表
            ShowPanel("有效廣告");
        }
    }


    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        Panel_Edit.Visible = false;
        Panel2.Visible = true;
    }

    public string GetImageUrl(object imageData, int size)
    {
        if (imageData != DBNull.Value)
        {
            byte[] imageBytes = (byte[])imageData;
            string base64Image = Convert.ToBase64String(imageBytes);
            return "data:image/png;base64," + base64Image;
        }
        return "img/default.jpg"; // Replace with your default image path
    }

    private void updateADImage()
    {
        // 獲取上傳的文件名
        string fileName = Path.GetFileName(FileUpload2.FileName);

        // 構建服務器上的文件路徑
        string filePath = Server.MapPath("~/Uploads/" + fileName);

        // 讀取上傳的文件字節數組
        byte[] imageData = FileUpload2.FileBytes;

        // 將圖片數據插入到數據庫
        using (SqlConnection connection = new SqlConnection(conectionString))
        {
            string query = "UPDATE 廣告 SET 圖片 = @ProfileImage WHERE 編號 = @ad_id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProfileImage", imageData);
            command.Parameters.AddWithValue("@ad_id", hf_ad_id.Value);
            connection.Open();
            command.ExecuteNonQuery();
        }
        LoadAds();
    }
    private void LoadExpiredAds()
    {
        using (SqlConnection connection = new SqlConnection(conectionString))
        {
            string query = "SELECT [編號], [名稱], [圖片], [上架日], [下架日], [連結] FROM [廣告] WHERE [下架日] < GETDATE()";
            SqlCommand command = new SqlCommand(query, connection);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            rp_expired_ads.DataSource = dt;
            rp_expired_ads.DataBind();
        }
    }

    private void LoadFutureAds()
    {
        using (SqlConnection connection = new SqlConnection(conectionString))
        {
            string query = "SELECT [編號], [名稱], [圖片], [上架日], [下架日], [連結] FROM [廣告] WHERE [上架日] > GETDATE()";
            SqlCommand command = new SqlCommand(query, connection);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            rp_future_ads.DataSource = dt;
            rp_future_ads.DataBind();
        }
    }
}
