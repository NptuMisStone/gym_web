using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_User_register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    
    protected void btn_user_login_Click(object sender, EventArgs e)
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
            string conectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
            SqlConnection sqlcn = new SqlConnection(conectionString);
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
                window.location.href = '../User/User_login.aspx';
                }, 1500);
                </script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
        }
    }

    private bool IsAccountRegistered(string acc)
    {
        bool isRegistered = false;
        string conectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
        SqlConnection sqlcn = new SqlConnection(conectionString);
        sqlcn.Open();
        string sql = "select count(*) from 使用者資料 where 使用者帳號=@acc";
        SqlCommand command = new SqlCommand(sql, sqlcn);
        command.Parameters.AddWithValue("@acc", acc);
        int count = (int)command.ExecuteScalar();

        if (count > 0)
        {
            isRegistered = true;
        }

        return isRegistered;
    }
    private bool IsEmailRegistered(string user_email)
    {
        bool isRegistered = false;
        string conectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
        SqlConnection sqlcn = new SqlConnection(conectionString);
        sqlcn.Open();
        string sql = "select count(*) from 使用者資料 where 使用者郵件=@User_email";
        SqlCommand command = new SqlCommand(sql, sqlcn);
        command.Parameters.AddWithValue("@User_email", user_email);
        int count = (int)command.ExecuteScalar();

        if (count > 0)
        {
            isRegistered = true;
        }

        return isRegistered;
    }
    private bool IsPhoneRegistered(string user_phone)
    {
        bool isRegistered = false;
        string conectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
        SqlConnection sqlcn = new SqlConnection(conectionString);
        sqlcn.Open();
        string sql = "select count(*) from 使用者資料 where 使用者電話=@User_phone";
        SqlCommand command = new SqlCommand(sql, sqlcn);
        command.Parameters.AddWithValue("@User_phone", user_phone);
        int count = (int)command.ExecuteScalar();

        if (count > 0)
        {
            isRegistered = true;
        }

        return isRegistered;
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

    protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string password = tb_user_pwd.Text;
        string account = tb_user_acc.Text;

        args.IsValid = password.Length >= 6 && password != account;
    }
}