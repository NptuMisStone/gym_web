using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_User_login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["user_id"] = null;
        Session["coach_id"] = null;
    }
    protected void btn_user_login_Click(object sender, EventArgs e)
    {
        if (user_acc.Text == null || user_pwd.Text == null)
        {
            string script = @"<script>
                Swal.fire({
                icon: ""error"",
                title: ""登入失敗"",
                text: '帳號或密碼錯誤',
                showConfirmButton: false,
                timer: 1500
                });
                </script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
        }
        else
        {
            string conectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString; ;
            SqlConnection sqlcn = new SqlConnection(conectionString);
            sqlcn.Open();
            string sql = "select * from 使用者資料 where 使用者帳號=@User_account and 使用者密碼=@User_password";
            SqlCommand command = new SqlCommand(sql, sqlcn);
            command.Parameters.AddWithValue("@User_account", user_acc.Text);
            command.Parameters.AddWithValue("@User_password", user_pwd.Text);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Session["coach_id"] = null;
                Session["user_id"] = reader["使用者編號"].ToString();
                int user_id = (int)reader["使用者編號"];
                Debug.WriteLine("User_id" + user_id);
                sqlcn.Close();

                string script = @"<script>
                Swal.fire({
                icon: ""success"",
                title: ""登入成功！"",
                showConfirmButton: false,
                timer: 1500
                });

                setTimeout(function () {
                window.location.href = '../page/Home.aspx';
                }, 1500);
                </script>";

                Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
            }
            else
            {
                string script = @"<script>
                Swal.fire({
                icon: ""error"",
                title: ""登入失敗"",
                text: '帳號或密碼錯誤',
                showConfirmButton: false,
                timer: 1500
                });
                </script>";

                Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
            }
        }
    }
}