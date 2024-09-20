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
        CoachHelper.CheckLogin(this);

        if (!IsPostBack)
        {
            BindCoachData();
            DisplayCoachImage();
            SetReviewStatus(Coach_id);
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
        tb_name.Visible = true;
        tb_phone.Visible = true;
        tb_email.Visible = true;
        tb_about.Visible = true;
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
        tb_name.Visible = false;
        tb_phone.Visible = false;
        tb_email.Visible = false;
        tb_about.Visible = false;
        FileUpload1.Visible = false;

        int coachId = int.Parse(Session["Coach_id"].ToString());
        string editedName = tb_name.Text;
        string editedPhone = tb_phone.Text;
        string editedEmail = tb_email.Text;
        string editedAbout = tb_about.Text;

        if (FileUpload1.HasFile)
        {
            updateCoachImage();
        }

        // 更新資料庫
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string updateQuery = "UPDATE 健身教練資料 SET " +
                                 "[健身教練姓名] = @Name, [健身教練電話] = @Phone, " +
                                 "[健身教練郵件] = @Email, [健身教練介紹] = @Introduction " +
                                 "WHERE [健身教練編號] = @CoachId";

            using (SqlCommand command = new SqlCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@Name", editedName);
                command.Parameters.AddWithValue("@Phone", editedPhone);
                command.Parameters.AddWithValue("@Email", editedEmail);
                command.Parameters.AddWithValue("@Introduction", editedAbout);
                command.Parameters.AddWithValue("@CoachId", coachId);

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
        tb_name.Visible = false;
        tb_phone.Visible = false;
        tb_email.Visible = false;
        tb_about.Visible = false;
        FileUpload1.Visible = false;
        tb_name.Text = lb_name.Text;
        tb_phone.Text = lb_phone.Text;
        tb_email.Text = lb_email.Text;
        tb_about.Text = lb_about.Text;

        DisplayCoachImage();
    }

    private void BindCoachData()
    {
        string query = "SELECT * FROM [健身教練資料] where [健身教練編號] = @Coach_id";
        string query2 = "SELECT * FROM [健身教練合併] where [健身教練編號] = @Coach_id AND [審核狀態] = 1";

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
                }
                reader.Close();
            }

            using (SqlCommand command = new SqlCommand(query2, connection))
            {
                command.Parameters.AddWithValue("@Coach_id", Coach_id);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    lb_store.Text = reader["服務地點名稱"].ToString();
                    lb_type.Text = reader["註冊類型"].ToString();

                    string registrationType = reader["註冊類型"].ToString();
                    if (registrationType == "私人教練")
                    {
                        store.Visible = false;
                    }
                    else
                    {
                        store.Visible = true;
                    }
                }
                else
                {
                    store.Visible = false;
                    type.Visible = false;
                }
            }
        }
    }


    protected void PWDBtnSave_Click(object sender, EventArgs e)
    {
        int coachId = int.Parse(Session["Coach_id"].ToString());
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string updateQuery = "UPDATE 健身教練資料 SET " +
                                     "[健身教練密碼]=@password " +
                                     "WHERE [健身教練編號] = @CoachId";
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
                        command.Parameters.AddWithValue("@CoachId", coachId);
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
                    coach_img.ImageUrl = "images/account_default.png";
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

    private void SetReviewStatus(string Coach_id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT 審核狀態 FROM 健身教練審核 WHERE 健身教練編號 = @CoachId";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CoachId", Coach_id);
                connection.Open();
                object result = command.ExecuteScalar();
                connection.Close();

                if (result == null)
                {
                    lblReviewStatus.Text = "尚未審核";
                    lblReviewStatus.ForeColor = System.Drawing.Color.Gray;
                }
                else
                {
                    string reviewStatus = result.ToString();

                    switch (reviewStatus)
                    {
                        case "0":
                            lblReviewStatus.Text = "審核中";
                            lblReviewStatus.ForeColor = System.Drawing.Color.Yellow;
                            hlVerify.Visible = false;
                            break;
                        case "1":
                            lblReviewStatus.Text = "審核通過";
                            lblReviewStatus.ForeColor = System.Drawing.Color.Green;
                            hlVerify.Text = "重新驗證健身教練身分";
                            break;
                        case "2":
                            lblReviewStatus.Text = "審核未通過，請重新提出申請";
                            lblReviewStatus.ForeColor = System.Drawing.Color.Red;
                            break;
                        default:
                            lblReviewStatus.Text = "未知狀態";
                            lblReviewStatus.ForeColor = System.Drawing.Color.Black;
                            break;
                    }
                }
            }
        }
    }

}
