using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class page_register : System.Web.UI.Page
{
    string conectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            string pwd1 = tb_user_pwd.Text;
            tb_user_pwd.Attributes.Add("value", pwd1);
            string pwd2 = tb_user_pwd.Text;
            tb_user_pwd2.Attributes.Add("value", pwd2);
        }
    }
    protected void btn_user_login_Click(object sender, EventArgs e)
    {
        if (txtAnswer.Text.Trim() == HttpContext.Current.Session["ImgText"] as string)
        {
            if (IsValid)
            {
                string phone = tb_phone.Text;
                string mail = tb_email.Text;
                string acc = tb_user_acc.Text;
                if (IsAccountRegistered(acc))
                {
                    ShowAlert("error", "帳號已存在", "請重新輸入", 1500);
                    tb_user_acc.Text = "";
                }
                else if (IsPhoneRegistered(phone))
                {
                    ShowAlert("error", "電話號碼已存在", "請重新輸入", 1500);
                    tb_phone.Text = "";
                }
                else if (IsEmailRegistered(mail))
                {
                    ShowAlert("error", "電子郵件已存在", "請重新輸入", 1500);
                    tb_email.Text = "";
                }
                else
                {
                    using (SqlConnection sqlcn = new SqlConnection(conectionString))
                    {
                        sqlcn.Open();
                        string sql = "insert into 使用者資料 (使用者姓名,使用者帳號,使用者密碼,使用者電話,使用者郵件,使用者性別)" +
                            "values(@user_name,@user_acc,@userpwd,@userphone,@user_email,@user_gender)";
                        SqlCommand command = new SqlCommand(sql, sqlcn);
                        command.Parameters.AddWithValue("@user_name", tb_user_name.Text);
                        command.Parameters.AddWithValue("@user_acc", tb_user_acc.Text);
                        command.Parameters.AddWithValue("@userpwd", tb_user_pwd.Text);
                        command.Parameters.AddWithValue("@userphone", tb_phone.Text);
                        command.Parameters.AddWithValue("@user_email", tb_email.Text);
                        command.Parameters.AddWithValue("@user_gender", tb_gender.SelectedValue);
                        command.ExecuteNonQuery();
                    }
                    ShowAlert("success", "註冊成功！", "即將回到登入頁面...", 1500, true, "Login.aspx");
                }
            }
        }
        else
        {
            ShowAlert("error", "驗證錯誤", "請重新輸入正確的驗證碼", 1500);
        }
        txtAnswer.Text = "";
    }

    private bool IsAccountRegistered(string acc)
    {
        bool isRegistered = false;

        using (SqlConnection sqlcn = new SqlConnection(conectionString))
        {
            sqlcn.Open();

            // 先檢查健身教練資料
            string sqlCoach = "select count(*) from 健身教練資料 where 健身教練帳號=@acc";
            using (SqlCommand command = new SqlCommand(sqlCoach, sqlcn))
            {
                command.Parameters.AddWithValue("@acc", acc);
                int count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    isRegistered = true;
                    return isRegistered; // 如果教練已存在，直接返回
                }
            }

            // 再檢查使用者資料
            string sqlUser = "select count(*) from 使用者資料 where 使用者帳號=@acc";
            using (SqlCommand command = new SqlCommand(sqlUser, sqlcn))
            {
                command.Parameters.AddWithValue("@acc", acc);
                int count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    isRegistered = true;
                }
            }
        }

        return isRegistered;
    }
    private bool IsEmailRegistered(string email)
    {
        bool isRegistered = false;

        using (SqlConnection sqlcn = new SqlConnection(conectionString))
        {
            sqlcn.Open();

            // 先檢查健身教練資料
            string sqlCoach = "select count(*) from 健身教練資料 where 健身教練郵件=@Email";
            using (SqlCommand command = new SqlCommand(sqlCoach, sqlcn))
            {
                command.Parameters.AddWithValue("@Email", email);
                int count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    isRegistered = true;
                    return isRegistered; // 如果教練已存在，直接返回
                }
            }

            // 再檢查使用者資料
            string sqlUser = "select count(*) from 使用者資料 where 使用者郵件=@Email";
            using (SqlCommand command = new SqlCommand(sqlUser, sqlcn))
            {
                command.Parameters.AddWithValue("@Email", email);
                int count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    isRegistered = true;
                }
            }
        }

        return isRegistered;
    }
    private bool IsPhoneRegistered(string phone)
    {
        bool isRegistered = false;

        using (SqlConnection sqlcn = new SqlConnection(conectionString))
        {
            sqlcn.Open();

            // 先檢查健身教練資料
            string sqlCoach = "select count(*) from 健身教練資料 where 健身教練電話=@Phone";
            using (SqlCommand command = new SqlCommand(sqlCoach, sqlcn))
            {
                command.Parameters.AddWithValue("@Phone", phone);
                int count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    isRegistered = true;
                    return isRegistered; // 如果教練已存在，直接返回
                }
            }

            // 再檢查使用者資料
            string sqlUser = "select count(*) from 使用者資料 where 使用者電話=@Phone";
            using (SqlCommand command = new SqlCommand(sqlUser, sqlcn))
            {
                command.Parameters.AddWithValue("@Phone", phone);
                int count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    isRegistered = true;
                }
            }
        }

        return isRegistered;
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

    protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string password = tb_user_pwd.Text;
        string account = tb_user_acc.Text;

        args.IsValid = password.Length >= 6 && password != account;
    }
    [WebMethod]
    public static void GetCaptchaText()
    {
        string randomString = CaptchaHelper.GetRandomString(4);
        HttpContext.Current.Session["ImgText"] = randomString;
    }
}