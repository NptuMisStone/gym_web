using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Coach_Coach_register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    
    protected void btn_coach_login_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            string phone = tb_phone.Text;
            string mail = tb_email.Text;
            string acc = tb_coach_acc.Text;
            if (IsAccountRegistered(acc))
            {
                ShowAlert("error", "帳號已存在", "請重新輸入", 1500);
                tb_coach_acc.Text = "";
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
                string conectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
                SqlConnection sqlcn = new SqlConnection(conectionString);
                sqlcn.Open();
                string sql = "insert into 健身教練資料 (健身教練姓名,健身教練帳號,健身教練密碼,健身教練電話,健身教練郵件,健身教練性別)" +
                    "values(@coach_name,@coach_acc,@coachpwd,@coachphone,@coach_email,@coach_gender)";
                SqlCommand command = new SqlCommand(sql, sqlcn);
                command.Parameters.AddWithValue("@coach_name", tb_coach_name.Text);
                command.Parameters.AddWithValue("@coach_acc", tb_coach_acc.Text);
                command.Parameters.AddWithValue("@coachpwd", tb_coach_pwd.Text);
                command.Parameters.AddWithValue("@coachphone", tb_phone.Text);
                command.Parameters.AddWithValue("@coach_email", tb_email.Text);
                command.Parameters.AddWithValue("@coach_gender", tb_gender.SelectedValue);
                command.ExecuteNonQuery();
                Debug.WriteLine("成功");
                sqlcn.Close();

                string script = @"<script>
                Swal.fire({
                icon: ""success"",
                title: ""註冊成功！"",
                text: '即將回到登入頁面...',
                showConfirmButton: false,
                timer: 1500
                });

                setTimeout(function () {
                window.location.href = '../Coach/Coach_login.aspx';
                }, 1500);
                </script>";

                Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
            }
        }
    }

    private bool IsAccountRegistered(string acc)
    {
        bool isRegistered = false;
        string conectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
        SqlConnection sqlcn = new SqlConnection(conectionString);
        sqlcn.Open();
        string sql = "select count(*) from 健身教練資料 where 健身教練帳號=@acc";
        SqlCommand command = new SqlCommand(sql, sqlcn);
        command.Parameters.AddWithValue("@acc", acc);
        int count = (int)command.ExecuteScalar();

        if (count > 0)
        {
            isRegistered = true;
        }

        return isRegistered;
    }
    private bool IsEmailRegistered(string coach_email)
    {
        bool isRegistered = false;
        string conectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
        SqlConnection sqlcn = new SqlConnection(conectionString);
        sqlcn.Open();
        string sql = "select count(*) from 健身教練資料 where 健身教練郵件=@Coach_email";
        SqlCommand command = new SqlCommand(sql, sqlcn);
        command.Parameters.AddWithValue("@Coach_email", coach_email);
        int count = (int)command.ExecuteScalar();

        if (count > 0)
        {
            isRegistered = true;
        }

        return isRegistered;
    }

    private bool IsPhoneRegistered(string coach_phone)
    {
        bool isRegistered = false;
        string conectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
        SqlConnection sqlcn = new SqlConnection(conectionString);
        sqlcn.Open();
        string sql = "select count(*) from 健身教練資料 where 健身教練電話=@Coach_phone";
        SqlCommand command = new SqlCommand(sql, sqlcn);
        command.Parameters.AddWithValue("@Coach_phone", coach_phone);
        int count = (int)command.ExecuteScalar();

        if (count > 0)
        {
            isRegistered = true;
        }

        return isRegistered;
    }
    protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string password = tb_coach_pwd.Text;
        string account = tb_coach_acc.Text;

        args.IsValid = password.Length >= 6 && password != account;
    }
    private void ShowAlert(string icon, string title, string text, int timer, string redirectUrl = null)
    {
        string script = $@"<script>
            Swal.fire({{
                icon: '{icon}',
                title: '{title}',
                text: '{text}',
                showConfirmButton: false,
                timer: {timer}
            }});
            {(redirectUrl != null ? $"setTimeout(function() {{ window.location.href = '{redirectUrl}'; }}, {timer});" : "")}
        </script>";

        Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
    }
}