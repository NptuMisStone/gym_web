﻿using System;
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
    protected void Page_Load(object sender, EventArgs e)
    {
        Coach_id = Convert.ToString(Session["Coach_id"]);

        //驗證教練是否登入的類別函數
        CoachHelper.CheckLogin(this);

        if (!IsPostBack)
        {
            Session["uploadedImage"] = null;
            SetValidators();
            BindCourseTypes();
            BindRadioButtonList();
        }
        showTempimg();
    }
    private void BindCourseTypes()
    {
        string query = "SELECT * FROM [運動分類清單]";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                ddlCourseType.DataSource = reader;
                ddlCourseType.DataValueField = "分類編號";
                ddlCourseType.DataTextField = "分類名稱";
                ddlCourseType.DataBind();
            }
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
        tbClassLocation.Text = "";
        tbClassAddress.Text = "";
        tbClassLocation.Visible = false;
        tbClassAddress.Visible = false;
        tbClassSize.Text = "";
        tbClassSize.Visible = false;
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
            string courseName = tbCourseName.Text.Trim(); ;
            int courseType = Convert.ToInt32(ddlCourseType.SelectedValue);
            string courseDescription = tbCourseDescription.Text.Trim(); ;
            int courseDuration = Convert.ToInt32(ddlCourseTime.SelectedValue);
            decimal courseFee = Convert.ToDecimal(tbCourseFee.Text);
            int classSize = rblClassSize.SelectedValue == "1" ? 1 : int.Parse(tbClassSize.Text);
            string requiredEquipment = tbRequiredEquipment.Text.Trim();
            int LocationType = int.Parse(rblLocation.SelectedValue);
            string LocationName = tbClassLocation.Text.Trim();
            string LocationAddress = tbClassAddress.Text.Trim();
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

            string query = "INSERT INTO [健身教練課程] ([課程名稱], [分類編號], [課程內容介紹], [課程時間長度], [上課人數], [地點類型], [地點名稱], [地點地址], [課程費用], [所需設備], [課程圖片], [健身教練編號]) VALUES (@CourseName, @CourseType, @CourseDescription, @CourseDuration, @ClassSize, @LocationType, @LocationName, @LocationAddress, @CourseFee, @RequiredEquipment, @CourseImage, @CoachID)";
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
                    command.Parameters.AddWithValue("@LocationName", LocationType == 3 ? (object)LocationName : DBNull.Value);
                    command.Parameters.AddWithValue("@LocationAddress", LocationType == 3 ? (object)LocationAddress : DBNull.Value);
                    command.Parameters.AddWithValue("@CourseFee", courseFee);
                    command.Parameters.AddWithValue("@RequiredEquipment", requiredEquipment);
                    command.Parameters.AddWithValue("@CourseImage", courseImage);
                    command.Parameters.AddWithValue("@CoachID", Coach_id);

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

    protected void rdlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblLocation.SelectedItem.Value == "3") // 選擇其他選項
        {
            tbClassLocation.Visible = true; // 顯示指定地點輸入框
            tbClassAddress.Visible = true;
            rfvClassLocation.Enabled = true; // 啟用指定地點的必填驗證
            rfvClassAddress.Enabled = true;
        }
        else // 選擇到府選項
        {
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
}