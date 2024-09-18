using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class User_User_appointment_record : System.Web.UI.Page
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    public static string User_id, cancel_coach_id, Schedule_id;
    public static int cancel_coach_time,Ap_people;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_id"] == null)
        {
            string script = @"<script>
                Swal.fire({
                  icon: 'error',
                  title: '請先登入！',
                  confirmButtonText: '確定',
                }).then((result) => {
                  if (result.isConfirmed) {
                     window.location.href = '../User/User_login.aspx';
                  }
                });
                </script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlertScript", script, false);
        }
        else 
        {
            if (!IsPostBack)
            {
                User_id = Convert.ToString(Session["User_id"]);
                ddl_status.SelectedIndex = 0;
                show_record();
            }
        }
    }
    public void show_record()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "SELECT * FROM [使用者預約-有預約的] WHERE [使用者編號] = @User_id ORDER BY [日期] , [開始時間]";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@User_id", User_id);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                lb_norecord.Visible = false;
                dl_record.Visible = true;
                lb_count.Visible = true;
                dl_record.DataSource = reader;
                dl_record.DataBind();
            }
            else
            {
                lb_norecord.Visible = true;
                dl_record.Visible = false;
                lb_count.Visible = false;
            }
            connection.Close();
        }
        record_count();//共有幾筆預約紀錄
    }

    private void record_count()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "select count(*) from [使用者預約-有預約的] where [使用者編號] = @User_id" ;
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@User_id", User_id);
            connection.Open();
            int count = (int)command.ExecuteScalar();
            lb_count.Text = "共有" + count + "筆記錄";
            connection.Close();
        }
    }

    protected string GetStatusText(object status)//顯示預約狀態文字
    {
        int statusValue = Convert.ToInt32(status);
        switch (statusValue)
        {
            case 1:
                return "預約中";
            case 2:
                return "已完成";
            case 3:
                return "已取消";
            case 4:
                return "逾時";
            default:
                return ""; // 或回傳預設的狀態文字
        }
    }
    protected string GetBorderColor(object status)
    {
        if (status != null)
        {
            string statusString = status.ToString();
            switch (statusString)
            {
                case "1":
                    return "#475766"; 
                case "2":
                    return "#86B817"; 
                case "3":
                    return "#B6B6B6"; 
                case "4":
                    return "#CF808B"; 
                default:
                    return "#000000"; 
            }
        }
        else
        {
            return "#000000"; 
        }
    }
    protected void ddl_status_SelectedIndexChanged(object sender, EventArgs e)
    {
        int se_status = int.Parse(ddl_status.SelectedValue);
        serch_status(se_status);
    }
    private void serch_status(int se_status)
    {
        if (se_status == 0)
        {
            lb_norecord.Visible = false;
            dl_record.Visible = true;
            lb_count.Visible = true;
            show_record();
        }
        else
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "select * from [使用者預約-有預約的] where [使用者編號] = @User_id AND [預約狀態] = @Status ORDER BY [日期] , [開始時間]";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@User_id", User_id);
                command.Parameters.AddWithValue("@Status", se_status);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    lb_norecord.Visible = false;
                    dl_record.Visible = true;
                    dl_record.DataSource = reader;
                    dl_record.DataBind();
                    lb_count.Visible = true;
                }
                else
                {
                    lb_norecord.Visible = true;
                    lb_count.Visible = false;
                    dl_record.Visible = false;
                }
                connection.Close();
            }
            search_status_count(se_status);
        }
    }

    private void search_status_count(int se_status)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "select count(*) from [使用者預約-有預約的] where [使用者編號] = @User_id AND [預約狀態] = @Status" ;
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@User_id", User_id);
            command.Parameters.AddWithValue("@Status", se_status);
            connection.Open();
            int count = (int)command.ExecuteScalar();
            lb_count.Text = "共有" + count + "筆記錄";
        }
    }

    protected void dl_record_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "cancel")
        {
            SqlConnection sqlconn = new SqlConnection(connectionString);
            string query = "SELECT 課表編號 FROM 使用者預約 WHERE 預約編號=@id";
            SqlCommand command = new SqlCommand(query, sqlconn);
            command.Parameters.AddWithValue("@id", e.CommandArgument);
            sqlconn.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read()) { Schedule_id = reader["課表編號"].ToString(); }
            sqlconn.Close();
            UpdatePeople();

            SqlConnection sqlcn = new SqlConnection(connectionString);
            string sql = "update 使用者預約 set 預約狀態=3 where 使用者編號=@User_id AND 預約編號=@id";
            SqlCommand cmd = new SqlCommand(sql, sqlcn);
            cmd.Parameters.AddWithValue("@user_id", User_id);
            cmd.Parameters.AddWithValue("@id", e.CommandArgument);
            sqlcn.Open();
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                string script = @"<script>
                            Swal.fire({
                            icon: 'success',
                            title: '取消成功',
                            text: '預約已取消',
                            showConfirmButton: false,
                            timer: 1500
                            });
                          </script>";

                Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
            }
            else
            {
                string script = @"<script>
                            Swal.fire({
                            icon: 'error',
                            title: '取消失敗',
                            text: '預約取消失敗',
                            showConfirmButton: false,
                            timer: 1500
                            });
                          </script>";

                Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
            }
            sqlcn.Close();
            show_record();
        }
        else if (e.CommandName == "comment")
        {
            Session["User_id"] = User_id;
            Session["ap_id"] = e.CommandArgument;
            Response.Redirect("User_comment.aspx");
        }
    }

    protected bool HasCommented(object ap_id)
    {
        bool exists = false;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "IF EXISTS (SELECT 1 FROM 完成預約評論表 WHERE 預約編號 = @預約編號) SELECT 'TRUE' AS Result ELSE SELECT 'FALSE' AS Result";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@預約編號", ap_id);
                connection.Open();
                exists = Convert.ToBoolean(command.ExecuteScalar()); // 執行查詢並轉換結果為布林值
            }
        }
        return exists;
    }
    private void UpdatePeople()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "UPDATE 健身教練課表 SET 預約人數 = @ap_people WHERE 課表編號 = @Schedule_id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                Search_ap_people();
                command.Parameters.AddWithValue("@ap_people", Ap_people - 1);
                command.Parameters.AddWithValue("@Schedule_id", Schedule_id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
    private void Search_ap_people()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT 預約人數 FROM 健身教練課表 WHERE 課表編號 = @Schedule_id ";
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Schedule_id", Schedule_id);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Ap_people = int.Parse(reader["預約人數"].ToString().Trim());
            }
            connection.Close();
        }
    }
}