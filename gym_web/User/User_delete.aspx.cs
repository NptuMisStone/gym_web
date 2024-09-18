using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_User_delete : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    public static string User_id;
    protected void Page_Load(object sender, EventArgs e)
    {
        User_id = Convert.ToString(Session["User_id"]);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string dbPassword;
        string password = TextBox1.Text;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT 使用者密碼 FROM 使用者資料 WHERE 使用者編號 = @User_id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@User_id", User_id);
                connection.Open();
                dbPassword = command.ExecuteScalar() as string;
            }
        }
        if (dbPassword == password)
        {
            DeleteAccount();
            string script = @"<script>
                            Swal.fire({
                            icon: ""success"",
                            title: ""刪除成功"",
                            text: '帳號已刪除',
                            showConfirmButton: false,
                            timer: 1500
                            }).then(() => {
                                window.location.href = '../page/Home.aspx';
                            });
                            </script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
        }
        else
        {
            string script = @"<script>
                Swal.fire({
                icon: ""error"",
                title: ""刪除失敗"",
                text: '密碼錯誤',
                showConfirmButton: false,
                timer: 1500
                });
                </script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
        }
    }

    private void DeleteAccount()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "DELETE FROM 使用者資料 WHERE 使用者編號 = @User_id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@User_id", User_id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        Session["User_id"] = null;
    }

}