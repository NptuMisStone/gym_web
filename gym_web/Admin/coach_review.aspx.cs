using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.Diagnostics;


public partial class system_administrator_Registration_Approval : System.Web.UI.Page
{
    private static string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["ShowSweetAlert"] != null && (bool)Session["ShowSweetAlert"])
        {
            string script = @"<script type='text/javascript'>
                        Swal.fire({
                            title: '審核成功！',
                            text: '您的審核已經完成。',
                            icon: 'success',
                            confirmButtonText: '確定'
                        });
                    </script>";

            ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", script);

            Session["ShowSweetAlert"] = false; // 重設 Session 變數為 false，以確保 SweetAlert 不會重複顯示
        }
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        // 獲取DetailsView中選定的行的RegistrationID
        int registrationID = Convert.ToInt32(dvDesignerApproval.SelectedValue);
        // 將審核狀態更改為1
        UpdateReviewStatus(registrationID, 1);
        UpdateReviewDate(registrationID);
        // 重新綁定DetailsView以更新數據
        dvDesignerApproval.DataBind();
        //寄信
        FindCoach(registrationID);
        string script = @"<script type='text/javascript'>
                        Swal.fire({
                            title: '審核成功！',
                            text: '您的審核已經完成。',
                            icon: 'success',
                            confirmButtonText: '確定'
                        });
                    </script>";
        ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", script);
        Session["ShowSweetAlert"] = true;
        Response.Redirect(Request.Url.AbsoluteUri); // 重新導向到當前頁面
    }

    protected void DownloadPDF(object sender, EventArgs e)
    {
        LinkButton lnkDownload = (LinkButton)sender;
        int registrationID = Convert.ToInt32(lnkDownload.CommandArgument);

        // 根据 registrationID 从数据库中获取 VerificationData 的二进制数据
        byte[] pdfData = GetPDFDataFromDatabase(registrationID);

        // 将二进制数据提供给用户作为 PDF 文件
        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/pdf";
        Response.BinaryWrite(pdfData);
        Response.Flush();
        Response.End();
    }

    public byte[] GetPDFDataFromDatabase(int registrationID)
    {
        byte[] pdfData = null;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT 審核資料 FROM 健身教練審核 WHERE 編號 = @RegistrationID";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RegistrationID", registrationID);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (!(reader["審核資料"] is DBNull))
                        {
                            pdfData = (byte[])reader["審核資料"];
                        }
                    }
                }
            }
        }

        return pdfData;
    }

    private void UpdateReviewStatus(int registrationID, int newStatus)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "UPDATE 健身教練審核 SET 審核狀態 = @NewStatus WHERE 編號 = @RegistrationID";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@NewStatus", newStatus);
                command.Parameters.AddWithValue("@RegistrationID", registrationID);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

    private void UpdateReviewDate(int registrationID)
    {
        DateTime contractEndDate = DateTime.Now.AddYears(1); // 設定合約到期日為一年後的今天

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // 直接更新該筆資料的合約到期日
            string query = "UPDATE 健身教練審核 SET 合約到期日 = @ContractEndDate WHERE 編號 = @RegistrationID";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ContractEndDate", contractEndDate);
                command.Parameters.AddWithValue("@RegistrationID", registrationID);

                // 執行更新操作
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Debug.WriteLine("更新日期成功");
                }
                else
                {
                    Debug.WriteLine("更新日期失敗");
                }
            }

            connection.Close(); // 確保連線被關閉
        }
    }
    private void FindCoach(int registrationID)
    {

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // 直接更新該筆資料的合約到期日
            string query = "SELECT 健身教練姓名,健身教練郵件,合約到期日 FROM [健身教練審核合併] WHERE 編號=@RegistrationID";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RegistrationID", registrationID);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        SendMail(reader["健身教練郵件"].ToString(), reader["健身教練姓名"].ToString(), reader["合約到期日"].ToString());
                    }
                }
            }

            connection.Close(); // 確保連線被關閉
        }
    }



    protected void btncancel_Click(object sender, EventArgs e)
    {
        // 獲取DetailsView中選定的行的RegistrationID
        int registrationID = Convert.ToInt32(dvDesignerApproval.SelectedValue);

        // 將審核狀態更改為1
        UpdateReviewStatus(registrationID, 2);
        // 重新綁定DetailsView以更新數據
        dvDesignerApproval.DataBind();
        string script = @"<script type='text/javascript'>
                        Swal.fire({
                            title: '未過關！',
                            text: '送回冷宮。',
                            icon: 'error',
                            confirmButtonText: '確定'
                        });
                    </script>";

        ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", script);
    }


    //需重新叫回使用
    public void SendMail(string emails, string coach_name, string EDdate)
    {
        string GoogleID = "NptuMisStone@gmail.com"; // Google 發信帳號
        string TempPwd = "lgtb rhoq irjc flyi"; // 應用程式密碼

        string SmtpServer = "smtp.gmail.com";
        int SmtpPort = 587;
        MailMessage mms = new MailMessage();
        mms.From = new MailAddress(GoogleID);
        mms.Subject = "【屏大Fit-健身預約系統】教練審核結果通知信";
        mms.Body = $"<p>親愛的用戶{coach_name}您好：</p>" +
                   "<p>我們收到您註冊成為健身教練的請求。" +
                   $"<p>您的審核已通過，合約到期日為{EDdate}。</p>" +
                   "<p>祝您使用愉快，謝謝！" +
                   "<p>如果這不是您的操作，請忽略這封郵件。</p>" +
                   "<p>（本郵件是由系統自動寄發，請勿直接回覆，謝謝。）</p>" +
                   "<p>屏大Fit 團隊<br>NptuMisStone@gmail.com</p>";
        mms.IsBodyHtml = true; // 確保內容使用 HTML 格式
        mms.SubjectEncoding = System.Text.Encoding.UTF8;
        mms.To.Add(new MailAddress(emails));
        using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
        {
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(GoogleID, TempPwd); // 寄信帳密 
            client.Send(mms); // 寄出信件
        }
        Debug.WriteLine("已寄出審核通知至信箱");
    }
}

