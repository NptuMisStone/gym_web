using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Coach_Coach_delete : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    public static string Coach_id;
    protected void Page_Load(object sender, EventArgs e)
    {
        Coach_id = Convert.ToString(Session["Coach_id"]);

        //驗證教練是否登入的類別函數
        CoachHelper.CheckLogin(this);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string dbPassword;
        string password = TextBox1.Text;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT 健身教練密碼 FROM 健身教練資料 WHERE 健身教練編號 = @Coach_id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Coach_id", Coach_id);
                connection.Open();
                dbPassword = command.ExecuteScalar() as string;
            }
        }
        if(dbPassword == password)
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
            string query = "DELETE FROM 健身教練資料 WHERE 健身教練編號 = @Coach_id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Coach_id", Coach_id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        Session["Coach_id"] = null;
    }
}