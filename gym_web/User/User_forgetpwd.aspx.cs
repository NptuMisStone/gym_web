using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_User_forgetpwd : System.Web.UI.Page
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    public static string verificationCode;
    public static DateTime verificationCodeTime; // 記錄生成驗證碼的時間

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Panel1.Visible = true;   // 顯示第一個步驟的面板
            Panel2.Visible = false;  // 隱藏第二個步驟的面板
            Panel3.Visible = false;  // 隱藏第三個步驟的面板
            Debug.WriteLine("Page Loaded (Not Postback)");
        }
    }

    // 第一步：檢查帳號和 Email 並寄送驗證碼
    protected void Btn_step1_Click(object sender, EventArgs e)
    {
        string account = tb_acc.Text.Trim();
        string email = tb_email.Text.Trim();

        if (!string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(email))
        {
            CheckCredentials(account, email);
        }
        else
        {
            // 顯示錯誤訊息
            ShowAlert("error", "輸入錯誤", "請輸入完整的帳號與Email");
        }
    }

    // 檢查資料庫中的帳號和信箱是否匹配
    public void CheckCredentials(string account, string email)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT COUNT(*) FROM 使用者資料 WHERE 使用者帳號 = @Account AND 使用者郵件 = @Email";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Account", account);
                command.Parameters.AddWithValue("@Email", email);

                int count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    // 切換到第二步驟（驗證碼輸入畫面）
                    Panel1.Visible = false;
                    Panel2.Visible = true;
                    Panel3.Visible = false;

                    GenerateAndSendVerificationCode(email);
                }
                else
                {
                    // 顯示帳號或信箱錯誤訊息
                    ShowAlert("error", "查無資料", "帳號或信箱錯誤");
                }
            }
        }
    }

    // 生成並寄送驗證碼
    private void GenerateAndSendVerificationCode(string email)
    {
        verificationCodeTime = DateTime.Now;
        tb_pwd1.Enabled = true;
        string allChars = "1234567890";
        char[] chars = new char[6];
        Random rd = new Random(Guid.NewGuid().GetHashCode());
        for (int i = 0; i < 6; i++)
        {
            chars[i] = allChars[rd.Next(0, allChars.Length)];
        }
        verificationCode = new string(chars);
        Debug.WriteLine("產生的驗證碼：" + verificationCode);

        string GoogleID = "cbf110003@nptu.edu.tw"; // Google 發信帳號
        string TempPwd = "vhwa vtxp hdbt grwk"; // 應用程式密碼

        string SmtpServer = "smtp.gmail.com";
        int SmtpPort = 587;
        MailMessage mms = new MailMessage();
        mms.From = new MailAddress(GoogleID);
        mms.Subject = "GYM - 密碼重設通知信";
        mms.Body = $"<p>您的驗證碼為：{verificationCode}，有效期限3分鐘。</p>" +
                   "<p>如果這不是您的操作，請忽略這封郵件。</p>" +
                   "<p>這是系統自動發出的郵件，請勿回覆，謝謝。</p>";
        mms.IsBodyHtml = true;
        mms.SubjectEncoding = System.Text.Encoding.UTF8;
        mms.To.Add(new MailAddress(email));
        using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
        {
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(GoogleID, TempPwd); // 寄信帳密 
            client.Send(mms); // 寄出信件
        }

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "StartTimer", "startTimer();", true); // 啟動倒計時
        Debug.WriteLine("已寄出驗證碼至信箱");
    }

    // 第二步：驗證使用者輸入的驗證碼
    protected void Btn_step2_Click(object sender, EventArgs e)
    {
        DateTime now = DateTime.Now;
        if (tb_pwd1.Text == verificationCode)
        {
            // 檢查驗證碼是否在有效期限內
            TimeSpan timeDifference = now - verificationCodeTime;
            if (timeDifference.TotalMinutes <= 3)
            {
                // 切換到第三步驟（密碼重設畫面）
                Panel1.Visible = false;
                Panel2.Visible = false;
                Panel3.Visible = true;
            }
            else
            {
                // 驗證碼過期的處理邏輯
                ShowAlert("error", "驗證碼已過期", "請重新操作", 1500, true);
            }
        }
        else
        {
            // 驗證碼錯誤的處理邏輯
            ShowAlert("error", "驗證碼錯誤", "請重新輸入");
        }
    }

    // 第三步：重設密碼
    protected void Btn_step3_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            UpdateUserPassword(tb_acc.Text, tb_new_pwd.Text);
        }
        else
        {
            // 密碼不一致或為空的處理邏輯
            ShowAlert("error", "錯誤", "密碼重設失敗");
        }
    }

    // 更新使用者密碼
    private void UpdateUserPassword(string account, string newPassword)
    {
        using (SqlConnection sqlcn = new SqlConnection(connectionString))
        {
            sqlcn.Open();

            string sql = "UPDATE 使用者資料 SET 使用者密碼 = @Coach_password WHERE 使用者帳號 = @Coach_account";
            using (SqlCommand command = new SqlCommand(sql, sqlcn))
            {
                command.Parameters.AddWithValue("@Coach_password", newPassword);
                command.Parameters.AddWithValue("@Coach_account", account);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    // 更新成功的處理邏輯
                    ShowAlert("success", "密碼重設成功！", "", 1500, true);
                }
                else
                {
                    // 更新失敗的處理邏輯
                    ShowAlert("error", "錯誤", "密碼重設失敗");
                }
            }
        }
    }

    // 顯示提示訊息
    private void ShowAlert(string icon, string title, string text, int timer = 1500, bool redirect = false)
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

        if (redirect)
        {
            script += "setTimeout(function () { window.location.href = 'User_login.aspx'; }, 1500);";
        }

        script += "</script>";

        Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
    }

    protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string password = tb_new_pwd.Text;
        string account = tb_acc.Text;

        args.IsValid = password.Length >= 6 && password != account;
    }
}