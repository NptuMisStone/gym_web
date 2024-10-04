using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Coach_Coach_login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["user_id"] = null;
        Session["coach_id"] = null;
    }
    protected void btn_coach_login_Click(object sender, EventArgs e)
    {
        if (coach_acc.Text == null || coach_pwd.Text == null)
        {
            string script = @"<script>
                Swal.fire({
                icon: ""error"",
                title: ""登入失敗"",
                text: '請輸入帳號密碼',
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
            string conectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString; ;//AppSetting中括號
            SqlConnection sqlcn = new SqlConnection(conectionString);
            sqlcn.Open();
            string sql = "select * from 健身教練資料 where 健身教練帳號=@Coach_account and 健身教練密碼=@Coach_password";
            SqlCommand command = new SqlCommand(sql, sqlcn);
            command.Parameters.AddWithValue("@Coach_account", coach_acc.Text);
            command.Parameters.AddWithValue("@Coach_password", coach_pwd.Text);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Session["user_id"] = null;
                Session["coach_name"] = reader["健身教練姓名"].ToString();
                Session["coach_id"] = reader["健身教練編號"].ToString();
                int coach_id = (int)reader["健身教練編號"];
                Debug.WriteLine("coach_id=" + coach_id);
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