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

public partial class User_User_info : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    public static Image img_userimg;
    public static string User_id;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        User_id = Convert.ToString(Session["User_id"]);

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

            Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
        }
        else
        {
            if (!IsPostBack)
            {
                BindUserData();
                DisplayUserImage();
            }
        }
    }

    protected void BtnEdit_Click(object sender, EventArgs e)
    {
        Panel_edit.Visible = false;
        Panel_save.Visible = true;
        lb_name.Visible = false;
        lb_phone.Visible = false;
        lb_email.Visible = false;
        lb_gender.Visible = false;
        tb_name.Visible = true;
        tb_phone.Visible = true;
        tb_email.Visible = true;
        tb_gender.Visible = true;
        FileUpload1.Visible = true;
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        Panel_edit.Visible = true;
        Panel_save.Visible = false;
        lb_name.Visible = true;
        lb_phone.Visible = true;
        lb_email.Visible = true;
        lb_gender.Visible = true;
        tb_name.Visible = false;
        tb_phone.Visible = false;
        tb_email.Visible = false;
        tb_gender.Visible = false;
        FileUpload1.Visible = false;

        int userId = int.Parse(Session["User_id"].ToString());
        string editedName = tb_name.Text;
        string editedPhone = tb_phone.Text;
        string editedEmail = tb_email.Text;
        string editedGender = tb_gender.SelectedValue;

        if (FileUpload1.HasFile)
        {
            updateUserImage();
        }

        // 更新資料庫
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string updateQuery = "UPDATE 使用者資料 SET " +
                                 "[使用者姓名] = @Name, [使用者電話] = @Phone, " +
                                 "[使用者郵件] = @Email, [使用者性別] = @Gender " +
                                 "WHERE [使用者編號] = @UserId";

            using (SqlCommand command = new SqlCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@Name", editedName);
                command.Parameters.AddWithValue("@Phone", editedPhone);
                command.Parameters.AddWithValue("@Email", editedEmail);
                command.Parameters.AddWithValue("@Gender", editedGender);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowsAffected > 0)
                {
                    string script = @"<script>
                            Swal.fire({
                            icon: ""success"",
                            title: ""更新成功"",
                            text: '資料已更新',
                            showConfirmButton: false,
                            timer: 1500
                            });
                            </script>";

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);

                    BindUserData();
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
        lb_gender.Visible = true;
        tb_name.Visible = false;
        tb_phone.Visible = false;
        tb_email.Visible = false;
        tb_gender.Visible = false;
        FileUpload1.Visible = false;
        tb_name.Text = lb_name.Text;
        tb_phone.Text = lb_phone.Text;
        tb_email.Text = lb_email.Text;
        if (lb_gender.Text == "男生") { tb_gender.SelectedValue = "1"; }
        else if(lb_gender.Text == "女生") { tb_gender.SelectedValue = "2"; }
        else if (lb_gender.Text == "無性別") { tb_gender.SelectedValue = "3"; }
        

        DisplayUserImage();
    }

    private void BindUserData()
    {
        string query = "SELECT * FROM [使用者資料] where [使用者編號] = " + Session["User_id"];
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@編號", User_id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    lb_account.Text = reader["使用者帳號"].ToString();
                    lb_name.Text = reader["使用者姓名"].ToString();
                    tb_name.Text = reader["使用者姓名"].ToString();
                    lb_phone.Text = reader["使用者電話"].ToString();
                    tb_phone.Text = reader["使用者電話"].ToString();
                    lb_email.Text = reader["使用者郵件"].ToString();
                    tb_email.Text = reader["使用者郵件"].ToString();
                    string gender = reader["使用者性別"].ToString();
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
                connection.Close();
            }
        }
    }

    protected void PWDBtnSave_Click(object sender, EventArgs e)
    {
        int userId = int.Parse(Session["User_id"].ToString());
        string editedPassword = Txtpassword.Text;
        string editednewPassword = Txtnewpassword.Text;
        string editednewPassword2 = Txtnewpassword2.Text;
        if (editedPassword == "" || editednewPassword == "" || editednewPassword2 == "")
        {
            string script = @"<script>
                Swal.fire({
                icon: ""error"",
                title: ""更新失敗"",
                text: '請完整輸入',
                showConfirmButton: false,
                timer: 1500
                });
                </script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
        }
        else
        {
            if (IsValid)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string updateQuery = "UPDATE 使用者資料 SET " +
                                         "[使用者密碼]=@password " +
                                         "WHERE [使用者編號] = @UserId";
                    if (!CheckOldPwd(editedPassword))
                    {
                        string script = @"<script>
                    Swal.fire({
                    icon: ""error"",
                    title: ""更新失敗"",
                    text: '原密碼錯誤',
                    showConfirmButton: false,
                    timer: 1500
                    });
                    </script>";

                        Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
                    }
                    else
                    {
                        using (SqlCommand command = new SqlCommand(updateQuery, connection))
                        {
                            command.Parameters.AddWithValue("@UserId", userId);
                            command.Parameters.AddWithValue("@password", editednewPassword);

                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();
                            connection.Close();

                            if (rowsAffected > 0)
                            {
                                string script = @"<script>
                            Swal.fire({
                            icon: ""success"",
                            title: ""更新成功"",
                            text: '密碼已更新',
                            showConfirmButton: false,
                            timer: 1500
                            });
                            </script>";

                                Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
                            }
                        }
                    }
                }
            }
            else
            {
                string script = @"<script>
                            Swal.fire({
                            icon: ""error"",
                            title: ""錯誤"",
                            text: '密碼重設失敗',
                            showConfirmButton: false,
                            timer: 1500
                            });
                            </script>";

                Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
            }
        }
    }

    private bool CheckOldPwd(string oldPwd)
    {
        bool isValid = false;
        int userId = int.Parse(Session["User_id"].ToString());
        using (SqlConnection sqlcn = new SqlConnection(connectionString))
        {
            sqlcn.Open();
            string checkQuery = "SELECT [使用者密碼] FROM 使用者資料 WHERE [使用者編號] = @UserId";
            using (SqlCommand command = new SqlCommand(checkQuery, sqlcn))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isValid = reader["使用者密碼"].ToString() == oldPwd;
                }
            }
        }
        return isValid;
    }

    private void DisplayUserImage()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT 使用者圖片 FROM 使用者資料 WHERE 使用者編號 = @User_id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@User_id", User_id);
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    byte[] imageData = (byte[])result;

                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        System.Drawing.Image userImage = System.Drawing.Image.FromStream(ms);
                        user_img.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(imageData);
                    }
                }
                else
                {
                    user_img.ImageUrl = "images/account_default.png";
                }
            }
            connection.Close();
        }
    }

    private void updateUserImage()
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
            string query = "UPDATE 使用者資料 SET 使用者圖片 = @ProfileImage WHERE 使用者編號 = @User_id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ProfileImage", imageData);
                command.Parameters.AddWithValue("@User_id", User_id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        DisplayUserImage();
    }
    protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string password = Txtnewpassword.Text;
        string account = lb_account.Text;

        args.IsValid = password.Length >= 6 && password != account;
    }
}