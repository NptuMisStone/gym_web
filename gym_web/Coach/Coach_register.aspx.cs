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

    protected void btn_coach_login_Click(object sender, EventArgs e)
    {
        string phone = tb_phone.Text;
        string mail = tb_email.Text;
        string acc = tb_coach_acc.Text;
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
            tb_coach_acc.Text = " ";
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('註冊成功,請登入帳號喔！');window.location='Coach_login.aspx?A_Sno=1';", true);
            sqlcn.Close();
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

}