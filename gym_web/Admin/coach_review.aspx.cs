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
        // 重新綁定DetailsView以更新數據
        dvDesignerApproval.DataBind();
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

    public void SendMail(string emails, string title, string content)
    {
        var mail = new MailMessage();

        // 收件人 Email 地址
        foreach (var email in emails.Split(','))
        {
            mail.To.Add(email);
        }
        // 主旨
        mail.Subject = title;
        // 內文
        mail.Body = content;
        // 內文是否為 HTML
        mail.IsBodyHtml = true;
        // 優先權
        mail.Priority = MailPriority.Normal;

        // 發信來源,最好與你發送信箱相同,否則容易被其他的信箱判定為垃圾郵件.
        mail.From = new MailAddress("hairdressing.info.cutsy@gmail.com", "cutsy_123");


        // Gmail 的 SMTP 設定
        var smtp = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new System.Net.NetworkCredential("hairdressing.info.cutsy@gmail.com", "nycl qdvd olge hcyt"),
            EnableSsl = true
        };

        // 投遞出去
        smtp.Send(mail);

        mail.Dispose();
    }
}

