using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Coach_Coach_addclass : System.Web.UI.Page
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    public static string Coach_id;
    string city;
    protected void Page_Load(object sender, EventArgs e)
    {
        Coach_id = Convert.ToString(Session["Coach_id"]);

        //驗證教練是否登入的類別函數
        CheckLogin.CheckUserOrCoachLogin(this.Page, "Coach");

        if (!IsPostBack)
        {
            Session["uploadedImage"] = null;
            BindDropDownList();
            SetValidators();
            BindRadioButtonList();
        }
        showTempimg();
    }
    private void BindDropDownList()
    {
        // 1. 綁定運動分類清單
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"SELECT * FROM [運動分類清單]";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            ddlCourseType.DataSource = reader;
            ddlCourseType.DataTextField = "分類名稱";
            ddlCourseType.DataValueField = "分類編號";
            ddlCourseType.DataBind();
        }

        // 2. 綁定縣市
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"SELECT * FROM [縣市]";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            ddl_city.DataSource = reader;
            ddl_city.DataTextField = "縣市";
            ddl_city.DataValueField = "縣市id";
            ddl_city.DataBind();
        }
        // 添加一個空的初始項目
        ddlCourseType.Items.Insert(0, new ListItem("選擇課程類型", ""));
        ddlCourseTime.Items.Insert(0, new ListItem("選擇課程時長", ""));
    }

    private void BindRadioButtonList()
    {
        string query = "SELECT [註冊類型], [服務地點名稱] FROM [健身教練審核合併] WHERE [健身教練編號] = @CoachID";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CoachID", Coach_id);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string registrationType = reader["註冊類型"].ToString();
                        string serviceLocationName = reader["服務地點名稱"].ToString();

                        if (registrationType == "店家健身教練")
                        {
                            // 動態增加一個新的 ListItem 作為第一個項目
                            rblLocation.Items.Insert(0, new ListItem(serviceLocationName, "1"));
                        }
                    }
                }
            }
        }
        rblLocation.SelectedIndex = 0;
        rbReset();
    }

    protected void btnHiddenUpload_Click(object sender, EventArgs e)
    {
        showTempimg();
    }
    private void showTempimg()
    {
        if (fuCourseImage.HasFile) // 確認是否有上傳檔案
        {
            byte[] imgBytes = fuCourseImage.FileBytes;
            if (imgBytes != null && imgBytes.Length > 0)
            {
                // 儲存圖片到 Session
                Session["uploadedImage"] = imgBytes;

                // 轉換為 Base64 字串並設置 Image1 的 ImageUrl
                string imageBase64 = Convert.ToBase64String(imgBytes);
                string imgSrc = $"data:image/jpeg;base64,{imageBase64}";
                Image1.ImageUrl = imgSrc;
            }
        }
        else if (Session["uploadedImage"] != null)
        {
            byte[] imgBytes = (byte[])Session["uploadedImage"];
            if (imgBytes.Length > 0)
            {
                string imageBase64 = Convert.ToBase64String(imgBytes);
                string imgSrc = $"data:image/jpeg;base64,{imageBase64}";
                Image1.ImageUrl = imgSrc;
            }
        }
        else // 沒有上傳檔案時，顯示預設圖片
        {
            Image1.ImageUrl = ResolveUrl("~/Coach/img/upload.png");
        }
    }
    protected void btnAddCourse_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            // 執行表單提交邏輯
            string courseName = tbCourseName.Text.Trim();
            int courseType = Convert.ToInt32(ddlCourseType.SelectedValue);
            string courseDescription = tbCourseDescription.Text.Trim();
            int courseDuration = Convert.ToInt32(ddlCourseTime.SelectedValue);
            decimal courseFee = Convert.ToDecimal(tbCourseFee.Text);
            int classSize = rblClassSize.SelectedValue == "1" ? 1 : int.Parse(tbClassSize.Text);
            string requiredEquipment = tbRequiredEquipment.Text.Trim();
            int LocationType = int.Parse(rblLocation.SelectedValue);
            string LocationName = null;
            int city_id = 0;
            int area_id = 0;
            string LocationAddress = null;
            byte[] courseImage;

            // 如果 Session["uploadedImage"] 不為 null，使用 Session 中的圖片
            if (Session["uploadedImage"] != null)
            {
                courseImage = (byte[])Session["uploadedImage"];
            }
            else
            {
                courseImage = null;
            }

            // 處理 LocationType == 1 的情況
            if (LocationType == 1)
            {
                // 從資料庫查詢服務地點資訊
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query1 = "SELECT [服務地點名稱], [縣市id], [行政區id], [服務地點地址] FROM [健身教練審核合併] WHERE [健身教練編號] = @CoachID";
                    SqlCommand command = new SqlCommand(query1, connection);
                    command.Parameters.AddWithValue("@CoachID", Coach_id);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        LocationName = reader["服務地點名稱"].ToString();
                        city_id = Convert.ToInt32(reader["縣市id"]);
                        area_id = Convert.ToInt32(reader["行政區id"]);
                        LocationAddress = reader["服務地點地址"].ToString();
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            else if (LocationType == 2)
            {
                // LocationType == 2 時，只存縣市和行政區ID
                city_id = Convert.ToInt32(ddl_city.SelectedValue);
                area_id = Convert.ToInt32(ddl_area.SelectedValue);
            }
            else if (LocationType == 3)
            {
                // LocationType == 3 時，存所有資訊
                LocationName = tbClassLocation.Text.Trim();
                city_id = Convert.ToInt32(ddl_city.SelectedValue);
                area_id = Convert.ToInt32(ddl_area.SelectedValue);
                LocationAddress = tbClassAddress.Text.Trim();
            }

            // 建立插入課程的 SQL 語句
            string query = @"
        INSERT INTO [健身教練課程] 
        ([課程名稱], [分類編號], [課程內容介紹], [課程時間長度], [上課人數], [地點類型], [地點名稱], [縣市id], [行政區id], [地點地址], [課程費用], [所需設備], [健身教練編號]
        " + (courseImage != null ? ", [課程圖片]" : "") + @")
        VALUES 
        (@CourseName, @CourseType, @CourseDescription, @CourseDuration, @ClassSize, @LocationType, @LocationName, @City_id, @Area_id, @LocationAddress, @CourseFee, @RequiredEquipment, @CoachID
        " + (courseImage != null ? ", @CourseImage" : "") + ")";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CourseName", courseName);
                    command.Parameters.AddWithValue("@CourseType", courseType);
                    command.Parameters.AddWithValue("@CourseDescription", courseDescription);
                    command.Parameters.AddWithValue("@CourseDuration", courseDuration);
                    command.Parameters.AddWithValue("@ClassSize", classSize);
                    command.Parameters.AddWithValue("@LocationType", LocationType);

                    // 判斷 LocationType 來決定存哪些參數
                    if (LocationType == 1 || LocationType == 3)
                    {
                        command.Parameters.AddWithValue("@LocationName", LocationName);
                        command.Parameters.AddWithValue("@City_id", city_id);
                        command.Parameters.AddWithValue("@Area_id", area_id);
                        command.Parameters.AddWithValue("@LocationAddress", LocationAddress);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@LocationName", DBNull.Value);
                        command.Parameters.AddWithValue("@City_id", city_id);
                        command.Parameters.AddWithValue("@Area_id", area_id);
                        command.Parameters.AddWithValue("@LocationAddress", DBNull.Value);
                    }
                    command.Parameters.AddWithValue("@CourseFee", courseFee);
                    command.Parameters.AddWithValue("@RequiredEquipment", requiredEquipment);
                    command.Parameters.AddWithValue("@CoachID", Coach_id);

                    if (courseImage != null)
                    {
                        command.Parameters.AddWithValue("@CourseImage", courseImage);
                    }

                    connection.Open();
                    command.ExecuteNonQuery();

                    // 新增成功後，彈出提示並跳轉到 Coach_class.aspx
                    string script = @"
                    Swal.fire({
                    icon: 'success',
                    title: '新增成功',
                    text: '課程已更新',
                    showConfirmButton: false,
                    timer: 1500
                    }).then(function() {
                    window.location = 'Coach_class.aspx'; // 導向到 Coach_class.aspx
                    });";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlertScript", script, true);
                }
            }
        }
    }

    protected void ddl_city_SelectedIndexChanged(object sender, EventArgs e)
    {
        // 檢查是否有選擇縣市，如果選擇的值為空，則不繼續執行
        if (string.IsNullOrEmpty(ddl_city.SelectedValue) || ddl_city.SelectedValue == "0")
        {
            ddl_area.Items.Clear();
            return;
        }

        city = ddl_city.SelectedItem.Text;
        int city_id = Convert.ToInt32(ddl_city.SelectedValue);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string sql = "SELECT 行政區,行政區id FROM 行政區 WHERE 縣市id=@縣市id";
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@縣市id", city_id);

            SqlDataReader dataReader = cmd.ExecuteReader();

            // 先清空 ddl_area 裡現有的項目，防止重複顯示
            ddl_area.Items.Clear();
            ddl_area.DataSource = dataReader;
            ddl_area.DataTextField = "行政區";
            ddl_area.DataValueField = "行政區id";
            ddl_area.DataBind();

            // 在第一項插入提示 "請選擇鄉鎮區"
            ddl_area.Items.Insert(0, new ListItem("請選擇鄉鎮區", ""));

            dataReader.Close();
            connection.Close();
            RegisterScrollScript(tbCourseFee.ClientID);
        }
    }

    protected void ddl_city_DataBound(object sender, EventArgs e)
    {
        ddl_city.Items.Insert(0, new ListItem("請選擇縣市", ""));
    }

    protected void rdlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        rbReset();
        RegisterScrollScript(tbCourseFee.ClientID);
    }
    private void rbReset()
    {
        if (rblLocation.SelectedItem.Value == "3") // 選擇其他選項
        {
            tbClassLocation.Visible = true; // 顯示指定地點輸入框
            ddl_city.Visible = true;
            ddl_area.Visible = true;
            tbClassAddress.Visible = true;
            rfvClassLocation.Enabled = true; // 啟用指定地點的必填驗證
            rfvClassAddress.Enabled = true;
        }
        else if (rblLocation.SelectedItem.Value == "2") // 選擇到府選項
        {
            ddl_city.Visible = true;
            ddl_area.Visible = true;
            tbClassLocation.Text = ""; // 清空指定地點輸入框
            tbClassAddress.Text = "";
            tbClassLocation.Visible = false; // 隱藏指定地點輸入框
            tbClassAddress.Visible = false;
            rfvClassLocation.Enabled = false; // 停用指定地點的必填驗證
            rfvClassAddress.Enabled = false;
        }
        else // 選擇預設店家選項
        {
            ddl_city.Visible = false;
            ddl_area.Visible = false;
            tbClassLocation.Text = ""; // 清空指定地點輸入框
            tbClassAddress.Text = "";
            tbClassLocation.Visible = false; // 隱藏指定地點輸入框
            tbClassAddress.Visible = false;
            rfvClassLocation.Enabled = false; // 停用指定地點的必填驗證
            rfvClassAddress.Enabled = false;
        }
    }
    protected void rblClassSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblClassSize.SelectedItem.Value == "2") // 選擇團體課程
        {
            tbClassSize.Visible = true; // 顯示團體課程人數輸入框
            rfvClassSize.Enabled = true; // 啟用團體課程人數的必填驗證
            cvClassSize.Enabled = true; // 啟用團體課程人數的自定義驗證

            rblLocation.Items.FindByValue("2").Enabled = false; // 停用到府選項

            // 如果目前選擇的是到府，切換到其他選項
            if (rblLocation.SelectedValue == "2")
            {
                rblLocation.ClearSelection();
                rblLocation.Items.FindByValue("3").Selected = true;
                tbClassLocation.Visible = true;
                tbClassAddress.Visible = true;
                rfvClassLocation.Enabled = true;
            }
        }
        else // 選擇一對一課程
        {
            tbClassSize.Text = ""; // 清空團體課程人數輸入框
            tbClassSize.Visible = false; // 隱藏團體課程人數輸入框
            rfvClassSize.Enabled = false; // 停用團體課程人數的必填驗證
            cvClassSize.Enabled = false; // 停用團體課程人數的自定義驗證

            rblLocation.Items.FindByValue("2").Enabled = true; // 啟用到府選項
        }
        RegisterScrollScript(tbCourseFee.ClientID);
    }
    protected void cvClassSize_ServerValidate(object source, ServerValidateEventArgs args)
    {
        int classSize;
        // 嘗試將輸入值轉換為整數
        if (int.TryParse(tbClassSize.Text, out classSize))
        {
            // 驗證是否大於1
            if (classSize > 1)
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }
        }
        else
        {
            // 如果無法轉換為整數，表示輸入無效
            args.IsValid = false;
        }
    }
    private void SetValidators()
    {
        // 設定團體課程人數的驗證條件
        if (rblClassSize.SelectedValue == "2") // 團體課程
        {
            tbClassSize.Visible = true;
            rfvClassSize.Enabled = true;
            cvClassSize.Enabled = true;
        }
        else // 一對一課程
        {
            tbClassSize.Visible = false;
            rfvClassSize.Enabled = false;
            cvClassSize.Enabled = false;
        }

        // 設定上課地點的驗證條件
        if (rblLocation.SelectedValue == "3") // 教練指定地點
        {
            tbClassLocation.Visible = true;
            tbClassAddress.Visible = true;
            rfvClassLocation.Enabled = true;
            rfvClassAddress.Enabled = true;
        }
        else // 到府上課
        {
            tbClassLocation.Visible = false;
            tbClassAddress.Visible = false;
            rfvClassLocation.Enabled = false;
            rfvClassAddress.Enabled = false;
        }
    }
    private void RegisterScrollScript(string controlId)
    {
        // 使用 controlId 傳遞 ClientID 而不是靜態 ID
        ClientScript.RegisterStartupScript(this.GetType(), "scrollToControl", $"scrollToControl('{controlId}');", true);
    }
}