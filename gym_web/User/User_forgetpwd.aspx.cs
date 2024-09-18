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
    public static string verificationCode;
    public static DateTime verificationCodeTime; // 新增紀錄生成驗證碼的時間

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MultiView1.ActiveViewIndex = 0;
            Debug.WriteLine("Postback occurred");
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        verificationCodeTime = DateTime.Now;
        //Label5.Visible = true;
        tb_pwd1.Enabled = true;
        string allChars = "1234567890";
        char[] chars = new char[6];
        Random rd = new Random(Guid.NewGuid().GetHashCode());
        rd.Next();
        for (int i = 0; i < 6; i++)
        {
            chars[i] = allChars[rd.Next(0, allChars.Length)];
        }
        verificationCode = new string(chars);
        Debug.WriteLine("產生的驗證碼：" + verificationCode);

        string GoogleID = "cbf110003@nptu.edu.tw"; // Google 發信帳號
        string TempPwd = "vhwa vtxp hdbt grwk"; // 應用程式密碼

        string ReceiveMail = tb_email.Text; // 接收信箱
        string SmtpServer = "smtp.gmail.com";
        int SmtpPort = 587;
        MailMessage mms = new MailMessage();
        mms.From = new MailAddress(GoogleID);
        mms.Subject = "GYM - 通知信";
        mms.Body = "您的驗證碼為：" + verificationCode + "，" + "有效期限10分鐘";
        mms.IsBodyHtml = true;
        mms.SubjectEncoding = System.Text.Encoding.UTF8;
        mms.To.Add(new MailAddress(ReceiveMail));
        using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
        {
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(GoogleID, TempPwd); // 寄信帳密 
            client.Send(mms); // 寄出信件
        }
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "StartTimer", "startTimer();", true);

        Debug.WriteLine("禁用按鈕");
    }

    protected void btn_ok_Click(object sender, EventArgs e)
    {
        DateTime now = DateTime.Now;
        if (tb_pwd1.Text == verificationCode)
        {
            // 檢查時間差是否超過10分鐘
            TimeSpan timeDifference = now - verificationCodeTime;
            if (timeDifference.TotalMinutes <= 10)
            {
                MultiView1.ActiveViewIndex = 1;
            }
            else
            {
                // 驗證碼過期的處理逻辑
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "ALERT", "alert('驗證碼已過期，請重新獲取');window.location='User_forgetpwd.aspx?A_Sno=1';", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "ALERT", "alert('驗證碼錯誤');window.location='User_forgetpwd.aspx?A_Sno=1';", true);
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;

        using (SqlConnection sqlcn = new SqlConnection(connectionString))
        {
            sqlcn.Open();

            string sql = "update 使用者資料 set 使用者密碼=@User_password where 使用者帳號=@User_account";

            using (SqlCommand command = new SqlCommand(sql, sqlcn))
            {
                command.Parameters.AddWithValue("@User_password", tb_new_pwd.Text);
                command.Parameters.AddWithValue("@User_account", tb_acc.Text);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // 更新成功的处理逻辑
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('更改成功，請重新登入！');window.location='User_login.aspx?A_Sno=1';", true);
                }
                else
                {
                    // 更新失败的处理逻辑
                }
            }
        }
    }
}