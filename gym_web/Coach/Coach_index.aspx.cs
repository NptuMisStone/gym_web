using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Coach_Coach_home1 : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    public static string Coach_id;
    protected void Page_Load(object sender, EventArgs e)
    {
        Coach_id = Convert.ToString(Session["Coach_id"]);

        //驗證教練是否登入的類別函數
        CheckLogin.CheckUserOrCoachLogin(this.Page, "Coach");

        if (!IsPostBack)
        {
            //每一次登入都檢查是否超過合約到期日,如果是的話就更新審核狀態
            CheckCoachDate();
            DisplayCoachImage();
            UserManager userManager = new UserManager();

            if (Session["coach_id"] != null)
            {
                // 設置頁面上的教練名稱
                CoachName.Text = userManager.GetCoachName();
            }
            SetReviewStatus();
            LoadCoachDetails();
        }
    }
    protected void Btn_logout_Click(object sender, EventArgs e)
    {
        Session["user_id"] = null;
        Session["coach_id"] = null;
        Response.Redirect("/page/Login.aspx");
    }
    private void LoadCoachDetails()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM [健身教練審核合併] WHERE [健身教練編號] = @coach_id";
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@coach_id", Coach_id);
            SqlDataReader reader = command.ExecuteReader();

            // 使用 DataTable 來綁定 Repeater，方便在 ItemDataBound 中存取欄位
            DataTable dt = new DataTable();
            dt.Load(reader);
            rp_coach.DataSource = dt;
            rp_coach.DataBind();

            reader.Close();
        }
    }
    protected void rp_coach_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        // 確保只處理資料項目（排除 Header、Footer 等）
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            // 根據資料來源類型調整轉型
            var dataItem = e.Item.DataItem as DataRowView; // 如果使用 DataTable

            if (dataItem != null)
            {
                // 先檢查審核狀態
                int reviewStatus = Convert.ToInt32(dataItem["審核狀態"]);

                // 找到 Panel_store 控制項
                Panel panelStore = e.Item.FindControl("Panel_store") as Panel;

                if (panelStore != null)
                {
                    if (reviewStatus == 1) // 如果審核狀態為 1
                    {
                        string registrationType = dataItem["註冊類型"].ToString();

                        if (registrationType == "私人健身教練")
                        {
                            panelStore.Visible = false; // 隱藏 Panel_store
                        }
                        else
                        {
                            panelStore.Visible = true; // 顯示 Panel_store
                        }
                    }
                    else
                    {
                        // 審核狀態不是 1，隱藏 Panel_store
                        panelStore.Visible = false;
                    }
                }
            }
        }
    }

    protected void btn_verify_Click(object sender, EventArgs e)
    {
        Response.Redirect("Coach_verify.aspx");
    }
    private void SetReviewStatus()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT 註冊類型, 審核狀態, 合約到期日 FROM 健身教練審核 WHERE 健身教練編號 = @CoachId";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CoachId", Coach_id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    DateTime endDate;

                    if (reader["合約到期日"] != DBNull.Value && !string.IsNullOrWhiteSpace(reader["合約到期日"].ToString()))
                    {
                        if (DateTime.TryParse(reader["合約到期日"].ToString(), out endDate))
                        {
                            Console.WriteLine("成功解析日期: " + endDate.ToString("yyyy-MM-dd"));
                        }
                        else
                        {
                            Console.WriteLine("日期格式無效，使用預設值");
                            endDate = DateTime.MinValue.Date; // 或其他適當的預設值
                        }
                    }
                    else
                    {
                        Console.WriteLine("合約到期日為空，使用預設值");
                        endDate = DateTime.MinValue.Date; // 或其他適當的預設值
                    }

                    string registrationType = reader["註冊類型"].ToString();
                    string reviewStatus = reader["審核狀態"].ToString();

                    switch (reviewStatus)
                    {
                        case "0": // 審核中
                            Coach_Btn.Visible = false;
                            lblReviewStatus.Text = "審核中";
                            lblReviewStatus.ForeColor = System.Drawing.Color.Yellow;
                            lblReviewStatusText.Text = "我們正在處理您的教練身分審核，這可能需要一些時間，審核通過將會發送電子郵件至您的電子郵件信箱！感謝您的耐心等候！如有任何問題，請聯繫我們。";
                            btn_verify.Visible = false; // 審核中不顯示按鈕
                            EndDate.Visible = false;
                            img_Status.ImageUrl = "img/review.png";

                            break;

                        case "1": // 審核通過
                            Coach_Btn.Visible = true;
                            lblReviewStatus.Text = "審核通過";
                            lblReviewStatus.ForeColor = System.Drawing.Color.Green;
                            lblReviewStatusText.Text = registrationType; // 顯示註冊類型
                            lblContractEndDate.Text = endDate.ToString("yyyy/MM/dd");
                            btn_verify.Visible = false; // 審核通過不顯示按鈕
                            EndDate.Visible = true;
                            img_Status.ImageUrl = "img/check.png";
                            break;

                        case "2": // 審核未通過
                            Coach_Btn.Visible = false;
                            lblReviewStatus.Text = "審核未通過，請重新提出申請";
                            lblReviewStatus.ForeColor = System.Drawing.Color.Red;
                            lblReviewStatus.Text = "審核未通過，請重新提出申請";
                            lblReviewStatusText.Text = "經過審查，您所提交的資料不符合我們的審核標準。請檢查並修正相關內容後再次提交申請！如有任何問題，請聯繫我們。";
                            btn_verify.Visible = true; // 顯示 "重新驗證" 按鈕
                            btn_verify.Text = "重新驗證";
                            EndDate.Visible = false;
                            img_Status.ImageUrl = "img/reject.png";

                            break;

                        case "3": // 教練過期了
                            Coach_Btn.Visible = false;
                            lblReviewStatus.Text = "合約已到期，請重新提出申請";
                            lblReviewStatus.ForeColor = System.Drawing.Color.Red;
                            btn_verify.Visible = true; // 顯示 "重新驗證" 按鈕
                            btn_verify.Text = "重新驗證";
                            EndDate.Visible = false;
                            img_Status.ImageUrl = "img/warning.png";

                            break;

                        default: // 未知狀態
                            lblReviewStatus.Text = "未知狀態";
                            lblReviewStatus.ForeColor = System.Drawing.Color.Black;
                            btn_verify.Visible = false; // 不顯示按鈕
                            EndDate.Visible = false;

                            break;
                    }
                }
                else
                {
                    lblReviewStatus.Text = "立即驗證健身教練身分！";
                    lblReviewStatus.ForeColor = System.Drawing.Color.Gray;
                    lblReviewStatusText.Text = "提醒您，為了保障平台使用者的安全與信任，教練必須先完成身分驗證才能在平台上被用戶搜尋及預約。" +
                        "如未驗證，您的資訊將不會被上架顯示。";
                    btn_verify.Visible = true; // 顯示 "立即驗證" 按鈕
                    btn_verify.Text = "立即驗證";
                    img_Status.ImageUrl = "img/information.png"; // 設定圖示為尚未審核
                }
            }
        }
    }

    private void CheckCoachDate()
    {
        DateTime today = DateTime.Now; // 今天的日期
        DateTime contractEndDate;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            // 查詢教練的合約到期日和審核狀態
            string query1 = "SELECT 合約到期日, 審核狀態 FROM 健身教練審核 WHERE 健身教練編號 = @coach_id";
            using (SqlCommand command1 = new SqlCommand(query1, connection))
            {
                command1.Parameters.AddWithValue("@coach_id", Coach_id);

                connection.Open(); // 打開資料庫連接
                SqlDataReader reader = command1.ExecuteReader();
                if (reader.Read())
                {
                    // 檢查合約到期日是否為 DBNull
                    if (reader["合約到期日"] != DBNull.Value)
                    {
                        contractEndDate = Convert.ToDateTime(reader["合約到期日"]);
                    }
                    else
                    {
                        Debug.WriteLine("合約到期日為空，無需更新");
                        reader.Close();
                        return;
                    }

                    // 獲取當前審核狀態
                    int currentStatus = Convert.ToInt32(reader["審核狀態"]);

                    // 關閉 SqlDataReader
                    reader.Close();

                    // 檢查合約到期日和審核狀態
                    if (contractEndDate <= today)
                    {
                        if (currentStatus == 1) // 如果審核狀態為 1
                        {
                            // 更新審核狀態為 3
                            string query2 = "UPDATE 健身教練審核 SET 審核狀態 = @NewStatus WHERE 健身教練編號 = @Coach_Id";
                            using (SqlCommand command2 = new SqlCommand(query2, connection))
                            {
                                command2.Parameters.AddWithValue("@Coach_Id", Coach_id);
                                command2.Parameters.AddWithValue("@NewStatus", 3); // 假設 3 是更新的審核狀態

                                // 執行更新操作
                                int rowsAffected = command2.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    Debug.WriteLine("合約過期，審核狀態更新成功");
                                }
                                else
                                {
                                    Debug.WriteLine("合約過期，審核狀態更新失敗");
                                }
                            }
                        }
                        else
                        {
                            Debug.WriteLine("合約已過期，審核狀態不變，當前狀態為 " + currentStatus);
                        }
                    }
                    else
                    {
                        Debug.WriteLine("合約未過期，無需更新");
                    }
                }
                else
                {
                    Debug.WriteLine("查無資料");
                    reader.Close();
                }
            }
        }
    }
    private void DisplayCoachImage()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT 健身教練圖片 FROM 健身教練資料 WHERE 健身教練編號 = @Coach_id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Coach_id", Coach_id);
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    byte[] imageData = (byte[])result;

                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        System.Drawing.Image userImage = System.Drawing.Image.FromStream(ms);
                        coach_img.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(imageData);
                    }
                }
                else
                {
                    coach_img.ImageUrl = "~/page/img/coach_main_ic_default.jpg";
                }
            }
            connection.Close();
        }
    }
}