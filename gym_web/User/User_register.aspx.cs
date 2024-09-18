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

    protected void btn_user_login_Click(object sender, EventArgs e)
    {
        string phone = tb_phone.Text;
        string mail = tb_email.Text;
        string acc = tb_user_acc.Text;
        if (IsPhoneRegistered(phone))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('電話已註冊過~')", true);
            tb_phone.Text = " ";
        }
        else if (IsEmailRegistered(mail))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('E-mail已註冊過~')", true);
            tb_email.Text = " ";
        }
        else if (IsAccountRegistered(acc))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('帳號已註冊過~')", true);
            tb_user_acc.Text = " ";
        }
        else
        {
            string conectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
            SqlConnection sqlcn = new SqlConnection(conectionString);
            sqlcn.Open();
            string sql = "insert into 使用者資料 (使用者暱稱,使用者帳號,使用者密碼,使用者電話,使用者郵件,使用者性別)" +
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('註冊成功,請登入帳號喔！');window.location='User_login.aspx?A_Sno=1';", true);
            sqlcn.Close();
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

   
}