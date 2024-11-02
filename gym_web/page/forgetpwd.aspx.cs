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

public partial class page_forgetpwd : System.Web.UI.Page
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    public static string verificationCode;
    public static DateTime verificationCodeTime; // 新增紀錄生成驗證碼的時間

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
        string account = tb_acc.Text.Trim(); // 確保刪除輸入的空格
        string email = tb_email.Text.Trim(); // 確保刪除輸入的空格

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

            // 首先檢查健身教練資料
            string queryCoach = "SELECT COUNT(*) FROM 健身教練資料 WHERE 健身教練帳號 = @Account AND 健身教練郵件 = @Email";
            using (SqlCommand command = new SqlCommand(queryCoach, connection))
            {
                command.Parameters.AddWithValue("@Account", account);
                command.Parameters.AddWithValue("@Email", email);

                int count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    // 如果找到健身教練資料，切換到第二步驟（驗證碼輸入畫面）
                    Panel1.Visible = false;
                    Panel2.Visible = true;
                    Panel3.Visible = false;

                    GenerateAndSendVerificationCode(email);
                    return; // 已找到，返回方法，不需要繼續執行後面的邏輯
                }
            }

            // 如果沒找到健身教練，接著檢查使用者資料
            string queryUser = "SELECT COUNT(*) FROM 使用者資料 WHERE 使用者帳號 = @Account AND 使用者郵件 = @Email";
            using (SqlCommand command = new SqlCommand(queryUser, connection))
            {
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@Account", account);
                command.Parameters.AddWithValue("@Email", email);

                int count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    // 如果找到使用者資料，切換到第二步驟（驗證碼輸入畫面）
                    Panel1.Visible = false;
                    Panel2.Visible = true;
                    Panel3.Visible = false;

                    GenerateAndSendVerificationCode(email);
                }
                else
                {
                    // 如果健身教練和使用者資料都查無，顯示帳號或信箱錯誤訊息
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

        string GoogleID = "NptuMisStone@gmail.com"; // Google 發信帳號
        string TempPwd = "lgtb rhoq irjc flyi"; // 應用程式密碼

        string SmtpServer = "smtp.gmail.com";
        int SmtpPort = 587;
        MailMessage mms = new MailMessage();
        mms.From = new MailAddress(GoogleID);
        mms.Subject = "【NPTU GYM】密碼重設通知信";
        mms.Body = "<p>我們收到您重設密碼的請求。" +
                   $"<p>您的驗證碼為：{verificationCode}</p>" +
                   "<p>有效期限為3分鐘，請妥善保管驗證碼，勿將其告知他人。</p>" +
                   "<p>如果這不是您的操作，請忽略這封郵件。</p>" +
                   "<p>（本郵件是由系統自動寄發，請勿直接回覆，謝謝。）</p>" +
                   "<p>NPTU GYM 團隊<br>NptuMisStone@gmail.com</p>";
        mms.IsBodyHtml = true; // 確保內容使用 HTML 格式
        mms.SubjectEncoding = System.Text.Encoding.UTF8;
        mms.To.Add(new MailAddress(email));
        using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
        {
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(GoogleID, TempPwd); // 寄信帳密 
            client.Send(mms); // 寄出信件
        }
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
                ShowAlert("error", "驗證碼已過期！", "請重新操作", 1500, true, "../page/forgetpwd.aspx");
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
        if (tb_new_pwd != null && tb_new_pwd2 != null)
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
    }

    // 更新使用者密碼
    private void UpdateUserPassword(string account, string newPassword)
    {
        using (SqlConnection sqlcn = new SqlConnection(connectionString))
        {
            sqlcn.Open();

            // 先檢查帳號是否存在於健身教練資料
            string sqlCoach = "SELECT COUNT(*) FROM 健身教練資料 WHERE 健身教練帳號 = @Account";
            using (SqlCommand commandCoach = new SqlCommand(sqlCoach, sqlcn))
            {
                commandCoach.Parameters.AddWithValue("@Account", account);
                int coachCount = (int)commandCoach.ExecuteScalar();

                if (coachCount > 0)
                {
                    // 如果帳號存在於健身教練資料，更新健身教練密碼
                    string updateCoachSql = "UPDATE 健身教練資料 SET 健身教練密碼 = @NewPassword WHERE 健身教練帳號 = @Account";
                    using (SqlCommand updateCoachCommand = new SqlCommand(updateCoachSql, sqlcn))
                    {
                        updateCoachCommand.Parameters.AddWithValue("@NewPassword", newPassword);
                        updateCoachCommand.Parameters.AddWithValue("@Account", account);
                        int rowsAffected = updateCoachCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            ShowAlert("success", "密碼重設成功！", "即將回到登入頁面...", 1500, true, "../page/Login.aspx");
                        }
                        else
                        {
                            ShowAlert("error", "錯誤", "密碼重設失敗");
                        }
                    }
                }
                else
                {
                    // 如果帳號不在健身教練資料，檢查是否存在於使用者資料
                    string sqlUser = "SELECT COUNT(*) FROM 使用者資料 WHERE 使用者帳號 = @Account";
                    using (SqlCommand commandUser = new SqlCommand(sqlUser, sqlcn))
                    {
                        commandUser.Parameters.AddWithValue("@Account", account);
                        int userCount = (int)commandUser.ExecuteScalar();

                        if (userCount > 0)
                        {
                            // 更新使用者密碼
                            string updateUserSql = "UPDATE 使用者資料 SET 使用者密碼 = @NewPassword WHERE 使用者帳號 = @Account";
                            using (SqlCommand updateUserCommand = new SqlCommand(updateUserSql, sqlcn))
                            {
                                updateUserCommand.Parameters.AddWithValue("@NewPassword", newPassword);
                                updateUserCommand.Parameters.AddWithValue("@Account", account);
                                int rowsAffected = updateUserCommand.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    ShowAlert("success", "密碼重設成功！", "即將回到登入頁面...", 1500, true, "../page/Login.aspx");
                                }
                                else
                                {
                                    ShowAlert("error", "錯誤", "密碼重設失敗");
                                }
                            }
                        }
                        else
                        {
                            // 如果教練和使用者都沒找到，顯示錯誤訊息
                            ShowAlert("error", "錯誤", "帳號不存在");
                        }
                    }
                }
            }
        }
    }


    // 顯示提示訊息
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

    protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string password = tb_new_pwd.Text;
        string account = tb_acc.Text;

        args.IsValid = password.Length >= 6 && password != account;
    }
}