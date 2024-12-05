using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Collections;
using System.Xml.Linq;

public partial class Coach_Coach_info1 : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    public static Image img_coachimg;
    public static string Coach_id;
    protected void Page_Load(object sender, EventArgs e)
    {
        Coach_id = Convert.ToString(Session["Coach_id"]);
        //驗證教練是否登入的類別函數
        CheckLogin.CheckUserOrCoachLogin(this.Page, "Coach");

        if (!IsPostBack)
        {
            BindCoachData();
            DisplayCoachImage();
        }
        // 判斷是否為刪除帳號的回傳
        if (Request["__EVENTTARGET"] == "DeleteAccount")
        {
            string password = Request["__EVENTARGUMENT"];
            DeleteAccount(password);
        }
    }

    protected void BtnEdit_Click(object sender, EventArgs e)
    {
        Panel_edit.Visible = false;
        Panel_save.Visible = true;
        lb_name.Visible = false;
        lb_phone.Visible = false;
        lb_email.Visible = false;
        lb_about.Visible = false;
        lb_gender.Visible = false;
        tb_name.Visible = true;
        tb_phone.Visible = true;
        tb_email.Visible = true;
        tb_about.Visible = true;
        tb_gender.Visible =true;
        FileUpload1.Visible = true;
        
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        Panel_edit.Visible = true;
        Panel_save.Visible = false;
        lb_name.Visible = true;
        lb_phone.Visible = true;
        lb_email.Visible = true;
        lb_about.Visible = true;
        lb_gender.Visible = true;
        tb_name.Visible = false;
        tb_phone.Visible = false;
        tb_email.Visible = false;
        tb_about.Visible = false;
        tb_gender.Visible =false;
        FileUpload1.Visible = false;

        int coachId = int.Parse(Session["Coach_id"].ToString());
        string editedName = tb_name.Text;
        string editedPhone = tb_phone.Text;
        string editedEmail = tb_email.Text;
        string editedAbout = tb_about.Text;
        string editedGender = tb_gender.SelectedValue;

        if (FileUpload1.HasFile)
        {
            updateCoachImage();
        }

        // 更新資料庫
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string updateQuery = "UPDATE 健身教練資料 SET " +
                                 "[健身教練姓名] = @Name, [健身教練電話] = @Phone, " +
                                 "[健身教練郵件] = @Email, [健身教練介紹] = @Introduction, " +
                                 "[健身教練性別] = @Gender " +
                                 "WHERE [健身教練編號] = @CoachId";

            using (SqlCommand command = new SqlCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@Name", editedName);
                command.Parameters.AddWithValue("@Phone", editedPhone);
                command.Parameters.AddWithValue("@Email", editedEmail);
                command.Parameters.AddWithValue("@Gender", editedGender);
                command.Parameters.AddWithValue("@Introduction", editedAbout);
                command.Parameters.AddWithValue("@CoachId", coachId);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowsAffected > 0)
                {
                    ShowAlert("success", "更新成功", "資料已更新", 1500);
                    BindCoachData();
                }
            }
        }
    }

    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        Panel_edit.Visible = true;
        Panel_save.Visible = false;
        lb_name.Visible = true;
        lb_phone.Visible = true;
        lb_email.Visible = true;
        lb_about.Visible = true;
        lb_gender.Visible = true;
        tb_name.Visible = false;
        tb_phone.Visible = false;
        tb_email.Visible = false;
        tb_about.Visible = false;
        tb_gender.Visible = false;
        FileUpload1.Visible = false;
        tb_name.Text = lb_name.Text;
        tb_phone.Text = lb_phone.Text;
        tb_email.Text = lb_email.Text;
        tb_about.Text = lb_about.Text;
        if (lb_gender.Text == "男生") { tb_gender.SelectedValue = "1"; }
        else if (lb_gender.Text == "女生") { tb_gender.SelectedValue = "2"; }
        else if (lb_gender.Text == "無性別") { tb_gender.SelectedValue = "3"; }
        DisplayCoachImage();
    }

    private void BindCoachData()
    {
        string query = "SELECT * FROM [健身教練資料] where [健身教練編號] = @Coach_id";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Coach_id", Coach_id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    lb_account.Text = reader["健身教練帳號"].ToString();
                    lb_name.Text = reader["健身教練姓名"].ToString();
                    tb_name.Text = reader["健身教練姓名"].ToString();
                    lb_phone.Text = reader["健身教練電話"].ToString();
                    tb_phone.Text = reader["健身教練電話"].ToString();
                    lb_email.Text = reader["健身教練郵件"].ToString();
                    tb_email.Text = reader["健身教練郵件"].ToString();
                    lb_about.Text = reader["健身教練介紹"].ToString();
                    tb_about.Text = reader["健身教練介紹"].ToString();
                    string gender = reader["健身教練性別"].ToString();
                    if (gender == "1")
                    {
                        lb_gender.Text = "男生";
                        tb_gender.SelectedValue = gender;
                    }
                    if (gender == "2")
                    {
                        lb_gender.Text = "女生";
                        tb_gender.SelectedValue = gender;
                    }
                    if (gender == "3")
                    {
                        lb_gender.Text = "無性別";
                        tb_gender.SelectedValue = gender;
                    }
                }
                reader.Close();
            }
        }
    }

    protected void PWDBtnSave_Click(object sender, EventArgs e)
    {
        int coachId = int.Parse(Session["Coach_id"].ToString());
        string editedPassword = Txtpassword.Text;
        string editednewPassword = Txtnewpassword.Text;
        string editednewPassword2 = Txtnewpassword2.Text;

        if (string.IsNullOrEmpty(editedPassword) || string.IsNullOrEmpty(editednewPassword) || string.IsNullOrEmpty(editednewPassword2))
        {
            ShowAlert("error", "更新失敗", "請完整輸入", 1500);
        }
        else
        {
            if (IsValid)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string updateQuery = "UPDATE 健身教練資料 SET " +
                                         "[健身教練密碼]=@password " +
                                         "WHERE [健身教練編號] = @CoachId";
                    if (!CheckOldPwd(editedPassword))
                    {
                        ShowAlert("error", "更新失敗", "原密碼錯誤", 1500);
                    }
                    else
                    {
                        using (SqlCommand command = new SqlCommand(updateQuery, connection))
                        {
                            command.Parameters.AddWithValue("@CoachId", coachId);
                            command.Parameters.AddWithValue("@password", editednewPassword);

                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();
                            connection.Close();

                            if (rowsAffected > 0)
                            {
                                ShowAlert("success", "更新成功", "密碼已更新", 1500);
                            }
                        }
                    }
                }
            }
            else
            {
                ShowAlert("error", "錯誤", "密碼重設失敗", 1500);
            }
        }
    }

    private bool CheckOldPwd(string oldPwd)
    {
        bool isValid = false;
        int coachId = int.Parse(Session["Coach_id"].ToString());
        using (SqlConnection sqlcn = new SqlConnection(connectionString))
        {
            sqlcn.Open();
            string checkQuery = "SELECT [健身教練密碼] FROM 健身教練資料 WHERE [健身教練編號] = @CoachId";
            using (SqlCommand command = new SqlCommand(checkQuery, sqlcn))
            {
                command.Parameters.AddWithValue("@CoachId", coachId);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isValid = reader["健身教練密碼"].ToString() == oldPwd;
                }
            }
        }
        return isValid;
    }
    protected void btn_verify_Click(object sender, EventArgs e)
    {
        Response.Redirect("Coach_verify.aspx");
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
                    coach_img.ImageUrl = "img/user.png";
                }
            }
            connection.Close();
        }
    }
    private void updateCoachImage()
    {
        // 獲取上傳的文件名
        string fileName = Path.GetFileName(FileUpload1.FileName);

        // 構建服務器上的文件路徑
        string filePath = Server.MapPath("~/Uploads/" + fileName);

        // 讀取上傳的文件字節數組
        byte[] imageData = FileUpload1.FileBytes;

        // 將圖片數據插入到數據庫
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "UPDATE 健身教練資料 SET 健身教練圖片 = @ProfileImage WHERE 健身教練編號 = @Coach_id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ProfileImage", imageData);
                command.Parameters.AddWithValue("@Coach_id", Coach_id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        DisplayCoachImage();
    }

    protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string password = Txtnewpassword.Text;
        string account = lb_account.Text;

        args.IsValid = password.Length >= 6 && password != account;
    }
    private void ShowAlert(string icon, string title, string text, int timer = 1500, bool redirect = false, string redirectUrl = null)
    {
        string script = $@"<script>
    Swal.fire({{
        icon: '{icon}',
        title: '{title}',
        text: '{text}',
        showConfirmButton: false,
        timer: {timer}
    }});
    ";

        if (redirect && !string.IsNullOrEmpty(redirectUrl))
        {
            script += $"setTimeout(function () {{ window.location.href = '{redirectUrl}'; }}, {timer});";
        }

        script += "</script>";

        Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
    }

    protected void Btn_delete_Click(object sender, EventArgs e)
    {
        string script = @"<script>
    Swal.fire({
        title: '確定刪除帳號嗎？',
        text: '刪除後將無法復原！',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: '確定刪除',
        cancelButtonText: '取消'
    }).then((result) => {
        if (result.isConfirmed) {
            // 顯示密碼輸入框
            Swal.fire({
                title: '請輸入密碼以確認刪除',
                input: 'password',
                inputPlaceholder: '請輸入密碼',
                showCancelButton: true,
                confirmButtonText: '確認刪除',
                cancelButtonText: '取消'
            }).then((result) => {
                if (result.isConfirmed) {
                    // 使用 __doPostBack 觸發後端邏輯
                    var password = result.value;
                    __doPostBack('DeleteAccount', password);
                }
            });
        }
    });
    </script>";

        Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertDelete", script, false);
    }
    private void DeleteAccount(string password)
    {
        string dbPassword;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT  健身教練密碼 FROM  健身教練資料 WHERE  健身教練編號 = @Coach_id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Coach_id", Coach_id);
                connection.Open();
                dbPassword = command.ExecuteScalar() as string;
            }
        }

        if (dbPassword == password)
        {
            // 刪除帳號，關聯未做
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM 健身教練審核 WHERE 健身教練編號 = @Coach_id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Coach_id", Coach_id);
                    command.ExecuteNonQuery();
                }
                string query2 = "DELETE FROM 健身教練資料 WHERE 健身教練編號 = @Coach_id";
                using (SqlCommand command = new SqlCommand(query2, connection))
                {
                    command.Parameters.AddWithValue("@Coach_id", Coach_id);
                    command.ExecuteNonQuery();
                }
            }
            Session["Coach_id"] = null;

            // 刪除成功的 SweetAlert
            string script = @"<script>
            Swal.fire({
                icon: 'success',
                title: '刪除成功',
                text: '帳號已刪除',
                showConfirmButton: false,
                timer: 1500
            }).then(() => {
                window.location.href = '../page/Home.aspx';
            });
            </script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertSuccess", script, false);
        }
        else
        {
            // 密碼錯誤的 SweetAlert
            string script = @"<script>
            Swal.fire({
                icon: 'error',
                title: '刪除失敗',
                text: '密碼錯誤，請重新確認',
                showConfirmButton: false,
                timer: 1500
            });
            </script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertError", script, false);
        }
    }
}
