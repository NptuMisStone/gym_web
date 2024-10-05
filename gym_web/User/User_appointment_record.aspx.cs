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
                Update_Pass();
                User_id = Convert.ToString(Session["User_id"]);
                show_record();
            }
        }
    }
    private void Update_Pass()//逾時
    {
        using (SqlConnection connection = new SqlConnection(connectionString)) {
            string sql = "SELECT 預約編號 FROM [使用者預約-有預約的] WHERE [日期] < @today AND [預約狀態] = 1 ";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@today", DateTime.Now.Date);
            connection.Open();
            List<int> reservationIds = new List<int>();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                reservationIds.Add(reader.GetInt32(0)); // 假設 預約編號 是整數型別
            }
            reader.Close();
            string updateSql = "UPDATE [使用者預約] SET 預約狀態 = @status WHERE 預約編號 = @reservationId";
            SqlCommand updateCommand = new SqlCommand(updateSql, connection);
            updateCommand.Parameters.AddWithValue("@status", 4); // 假設更新狀態為逾時

            // 逐一更新預約狀態
            foreach (int reservationId in reservationIds)
            {
                updateCommand.Parameters.AddWithValue("@reservationId", reservationId);
                updateCommand.ExecuteNonQuery();
                updateCommand.Parameters.Clear(); // 清空參數以便下次使用
                updateCommand.Parameters.AddWithValue("@status", 4); // 再次設置預約狀態
            }
            connection.Close();
        }
    }
    public void show_record()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "SELECT * FROM [使用者預約-有預約的] WHERE [使用者編號] = @User_id ORDER BY [日期], [開始時間]";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@User_id", User_id);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            // 分别绑定
            var inProgress = new List<AppointmentRecord>();
            var completed = new List<AppointmentRecord>();
            var cancelled = new List<AppointmentRecord>();
            var overtime = new List<AppointmentRecord>();
            var coachCancelled = new List<AppointmentRecord>();

            while (reader.Read())
            {
                var record = new AppointmentRecord
                {
                    預約編號 = Convert.ToInt32(reader["預約編號"]),
                    健身教練姓名 = reader["健身教練姓名"].ToString(),
                    課程名稱 = reader["課程名稱"].ToString(),
                    日期 = Convert.ToDateTime(reader["日期"]),
                    開始時間 = reader["開始時間"].ToString(),
                    預約狀態 = Convert.ToInt32(reader["預約狀態"]),
                    課程費用 = Convert.ToInt32(reader["課程費用"]),
                    備註 = reader["備註"].ToString()
                };

                int status = Convert.ToInt32(reader["預約狀態"]);
                switch (status)
                {
                    case 1:
                        inProgress.Add(record);
                        break;
                    case 2:
                        completed.Add(record);
                        break;
                    case 3:
                        cancelled.Add(record);
                        break;
                    case 4:
                        overtime.Add(record);
                        break;
                    case 5:
                        coachCancelled.Add(record);
                        break;
                }
            }
            // 分别绑定 DataList
            dl_inProgress.DataSource = inProgress;
            dl_inProgress.DataBind();

            dl_completed.DataSource = completed;
            dl_completed.DataBind();

            dl_cancelled.DataSource = cancelled;
            dl_cancelled.DataBind();

            dl_overtime.DataSource = overtime;
            dl_overtime.DataBind();

            dl_coachCancelled.DataSource = coachCancelled;
            dl_coachCancelled.DataBind();

            connection.Close();
        }
    }
    public class AppointmentRecord
    {
        public int 預約編號 { get; set; }
        public string 健身教練姓名 { get; set; }
        public string 課程名稱 { get; set; }
        public DateTime 日期 { get; set; }
        public string 開始時間 { get; set; }
        public int 預約狀態 { get; set; }
        public int 課程費用 { get; set; }
        public string 備註 { get; set; }
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
            connection.Close();
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
            connection.Close();
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

    protected void dl_inProgress_ItemCommand(object source, DataListCommandEventArgs e)
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
    }

    protected void dl_completed_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "comment")
        {
            Session["User_id"] = User_id;
            Session["ap_id"] = e.CommandArgument;
            Response.Redirect("User_comment.aspx");
        }
    }
}