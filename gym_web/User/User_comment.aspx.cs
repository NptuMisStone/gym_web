using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class User_User_comment : System.Web.UI.Page
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    public static int ap_id;
    public static int stars = 0;
    public static int User_id;
    protected void Page_Load(object sender, EventArgs e)
    {
        ap_id = Convert.ToInt32(Session["ap_id"]);
        User_id = Convert.ToInt32(Session["User_id"]);
        if (!IsPostBack)
        {
            show_record_data();
            show_commend();
        }
    }

    private void show_commend()
    {
        // 判斷使用者是否評論過
        bool exists = false;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "IF EXISTS (SELECT 1 FROM 完成預約評論表 WHERE 預約編號 = @預約編號) SELECT 'TRUE' AS Result ELSE SELECT 'FALSE' AS Result";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@預約編號", ap_id);
                connection.Open();
                exists = Convert.ToBoolean(command.ExecuteScalar()); // 執行查詢並轉換結果為布林值
            }
        }

        if (exists)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "select 評分,評論內容 from 完成預約評論表 where 預約編號=@ap_id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ap_id", ap_id);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        tb_comment.Text = reader["評論內容"].ToString();
                        stars = int.Parse(reader["評分"].ToString());
                    }
                    reader.Close();
                }
            }
            show_stars(stars);
        }
    }

    private void show_stars(int stars)
    {
        for (int i = 1; i <= stars; i++)
        {
            ImageButton starButton = (ImageButton)Panel1.FindControl($"img_star{i}");
            if (starButton != null)
            {
                starButton.ImageUrl = "images/star_click.png"; // 設定新的圖案
            }
        }
    }

    private void show_record_data()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "select 課程名稱,健身教練姓名,日期 from [使用者預約-評論用] where 使用者編號=@u_id and 預約編號=@ap_id";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@u_id", User_id);
                command.Parameters.AddWithValue("@ap_id", ap_id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    lb_de_name.Text += reader["健身教練姓名"].ToString();
                    lb_service.Text += reader["課程名稱"].ToString();

                    // 檢查預約日期是否存在且為有效的日期時間格式
                    if (!reader.IsDBNull(reader.GetOrdinal("日期")))
                    {
                        DateTime date;
                        if (DateTime.TryParse(reader["日期"].ToString(), out date))
                        {
                            // 使用 DayOfWeek 枚舉類型的值時，使用明確轉換
                            int dayOfWeek = (int)date.DayOfWeek;

                            // 使用 CultureInfo.CurrentCulture 作為格式提供者
                            CultureInfo culture = new CultureInfo("zh-TW");

                            // 使用 ToString() 方法將預約日期轉換為字串，並指定格式
                            lb_date.Text += date.ToString("yyyy年M月d日 " + DateTimeFormatInfo.GetInstance(culture).DayNames[dayOfWeek]);
                        }
                    }
                }

                reader.Close();
                connection.Close();
            }
        }
    }

    protected void img_star_Click(object sender, ImageClickEventArgs e)
    {
        // 取得觸發事件的按鈕
        ImageButton clickedButton = (ImageButton)sender;

        // 取得按鈕的CommandArgument
        int starNumber = Convert.ToInt32(clickedButton.CommandArgument);
        Session["starNumber"] = starNumber;
        //Label1.Text = starNumber.ToString();

        // 清空所有星號按鈕的圖案
        for (int i = 1; i <= 5; i++) // 有5個星號按鈕
        {
            ImageButton starButton = (ImageButton)Panel1.FindControl($"img_star{i}");
            if (starButton != null)
            {
                starButton.ImageUrl = "images/star.png"; // 清空圖案
            }
        }

        // 根據 starNumber 設定新的星號按鈕圖案
        for (int i = 1; i <= starNumber; i++)
        {
            ImageButton starButton = (ImageButton)Panel1.FindControl($"img_star{i}");
            if (starButton != null)
            {
                starButton.ImageUrl = "images/star_click.png"; // 設定新的圖案
            }
        }
        // 在這裡您可以執行其他相關的處理，例如將選定的星號數儲存到資料庫中等等
    }

    protected void btn_summit_Click(object sender, EventArgs e)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // Check if a review with the same ap_id exists
            string checkSql = "SELECT COUNT(*) FROM 完成預約評論表 WHERE 預約編號 = @app_num";
            using (SqlCommand checkCommand = new SqlCommand(checkSql, connection))
            {
                checkCommand.Parameters.AddWithValue("@app_num", ap_id);

                int existingReviewCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (existingReviewCount > 0)
                {
                    // A review with the same ap_id exists, so update it
                    string updateSql = "UPDATE 完成預約評論表 SET 評分 = @starNumber, 評論內容 = @comment, 評論日期 = @date, 評論時間 = @now WHERE 預約編號 = @app_num";
                    using (SqlCommand updateCommand = new SqlCommand(updateSql, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@starNumber", Convert.ToInt32(Session["starNumber"]));
                        updateCommand.Parameters.AddWithValue("@comment", tb_comment.Text);
                        updateCommand.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy/MM/dd"));
                        updateCommand.Parameters.AddWithValue("@now", DateTime.Now.ToString("HH:mm"));
                        updateCommand.Parameters.AddWithValue("@app_num", ap_id);

                        updateCommand.ExecuteNonQuery();
                    }
                    Response.Redirect("~/page/coach_detail.aspx");
                }
                else
                {
                    // No review with the same ap_id exists, so insert a new review
                    string insertSql = "INSERT INTO 完成預約評論表 (預約編號, 評分, 評論內容, 評論日期, 評論時間) VALUES (@app_num, @starNumber, @comment, @date, @now)";
                    using (SqlCommand insertCommand = new SqlCommand(insertSql, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@starNumber", Convert.ToInt32(Session["starNumber"]));
                        insertCommand.Parameters.AddWithValue("@comment", tb_comment.Text);
                        insertCommand.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy/MM/dd"));
                        insertCommand.Parameters.AddWithValue("@now", DateTime.Now.ToString("HH:mm"));
                        insertCommand.Parameters.AddWithValue("@app_num", ap_id);

                        insertCommand.ExecuteNonQuery();
                    }
                    Response.Redirect("User_appointment_record.aspx");
                }
            }

            connection.Close();
        }


    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("User_appointment_record.aspx");
    }
}