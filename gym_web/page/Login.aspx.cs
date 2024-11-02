using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class page_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["user_id"] = null;
        Session["coach_id"] = null;
        acc.Focus();
    }
    protected void btn_login_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(acc.Text) || string.IsNullOrWhiteSpace(pwd.Text))
        {
            ShowAlert("error", "登入失敗", "請輸入帳號密碼", 1500);
            return;
        }

        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;

        // 檢查教練帳號
        using (SqlConnection sqlcn = new SqlConnection(connectionString))
        {
            sqlcn.Open();
            string sql = "select * from 健身教練資料 where 健身教練帳號=@Coach_account and 健身教練密碼=@Coach_password";
            SqlCommand command = new SqlCommand(sql, sqlcn);
            command.Parameters.AddWithValue("@Coach_account", acc.Text);
            command.Parameters.AddWithValue("@Coach_password", pwd.Text);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Session["coach_id"] = reader["健身教練編號"].ToString();

                // 檢查返回的路徑
                string returnUrl_Coach = Convert.ToString(Session["ReturnUrl_Coach"]);
                if (string.IsNullOrEmpty(returnUrl_Coach))
                {
                    ShowAlert("success", "登入成功！", null, 1500, true, "../Coach/Coach_index.aspx");
                }
                else
                {
                    Session["ReturnUrl_Coach"] = null; // 清空返回路徑
                    ShowAlert("success", "登入成功！", null, 1500, true, returnUrl_Coach);
                }

                return;
            }
        }

        // 若教練不存在，檢查使用者帳號
        using (SqlConnection sqlcn = new SqlConnection(connectionString))
        {
            sqlcn.Open();
            string sql = "select * from 使用者資料 where 使用者帳號=@User_account and 使用者密碼=@User_password";
            SqlCommand command = new SqlCommand(sql, sqlcn);
            command.Parameters.AddWithValue("@User_account", acc.Text);
            command.Parameters.AddWithValue("@User_password", pwd.Text);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Session["user_id"] = reader["使用者編號"].ToString();

                // 檢查返回的路徑
                string returnUrl_User = Convert.ToString(Session["ReturnUrl_User"]);
                if (string.IsNullOrEmpty(returnUrl_User))
                {
                    ShowAlert("success", "登入成功！", null, 1500, true, "../page/Home.aspx");
                }
                else
                {
                    Session["ReturnUrl_User"] = null; // 清空返回路徑
                    ShowAlert("success", "登入成功！", null, 1500, true, returnUrl_User);
                }

                return;
            }
        }

        // 若兩者均不存在
        ShowAlert("error", "登入失敗", "帳號或密碼錯誤", 1500);
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
}