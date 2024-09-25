using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_User_appointment : System.Web.UI.Page
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    public static string User_id,Coach_id,Course_id, Schedule_id,Ap_date, Ap_starttime, Ap_endtime;
    public static int Ap_time,Ap_people, checkPlace;
    protected void Page_Load(object sender, EventArgs e)
    {
        Schedule_id = Convert.ToString(Session["Schedule_id"]);
        User_id = Convert.ToString(Session["User_id"]);
        ap_location.Enabled=false;
        rfvlocation.Enabled=false;
        if (!IsPostBack)
        {
            ShowSweetAlert();
            ShowDetail();
            GetId();
            CheckPlace();
        }
    }
    private void ShowSweetAlert()
    {
        // Define a function to show SweetAlert
        string script = @"<script>
                    Swal.fire({
                        icon: 'info',
                        title: '請確定以下規則',
                        html: '<strong>預約注意事項：</strong><br>' +
                            '<ul>' +
                            '<li>請確認您預約的日期及時間，</li>' +
                            '<li>確認後再送出您的預約請求。</li>' +
                            '</ul>',
                        confirmButtonText: '開始預約'
                    });
                </script>";

        // 注册 SweetAlert 脚本到页面
        Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", script, false);
    }
    private void ShowDetail()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM 健身教練課表課程合併 WHERE 課表編號 = @Schedule_id ";
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Schedule_id", Schedule_id);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read()) {
                ap_course_name.Text = reader["課程名稱"].ToString();
                ap_course_time.Text = reader["課程時間長度"].ToString();
                ap_course_add.Text = reader["地點名稱"].ToString();
                ap_course_date.Text = Convert.ToDateTime(reader["日期"]).ToString("yyyy/MM/dd");
                ap_course_stTime.Text = reader["開始時間"].ToString();
                ap_course_edTime.Text = reader["結束時間"].ToString();

                Ap_date= Convert.ToDateTime(reader["日期"]).ToString("yyyy/MM/dd");
                Ap_starttime= reader["開始時間"].ToString();
                Ap_endtime= reader["結束時間"].ToString();
            }
            connection.Close();
        }
    }
    private void GetId()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM 健身教練課表課程合併 WHERE 課表編號 = @Schedule_id ";
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Schedule_id", Schedule_id);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Coach_id = reader["健身教練編號"].ToString();
                Course_id = reader["課程編號"].ToString();
                if (reader["健身教練次數"].ToString() == string.Empty) { Ap_time = 0; }
                else
                {
                    Ap_time = int.Parse(reader["健身教練次數"].ToString());
                }
            }
            connection.Close();
        }
    }
    protected void ap_btn_Click(object sender, EventArgs e)
    {
        if (CheckAP())
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "Insert Into 使用者預約 (使用者編號,健身教練編號,課程編號,課表編號,預約狀態,備註,客戶到府地址)" +
                    "values(@u_id,@c_id,@course_id,@schedule_id,@status,@text,@location)";
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@u_id", User_id);
                command.Parameters.AddWithValue("@c_id", Coach_id);
                command.Parameters.AddWithValue("@course_id", Course_id);
                command.Parameters.AddWithValue("@schedule_id", Schedule_id);
                command.Parameters.AddWithValue("@status", 1);
                command.Parameters.AddWithValue("@text", ap_text.Text);
                command.Parameters.AddWithValue("@location", ap_location.Text);
                command.ExecuteReader();
                connection.Close();
                UpdatePeople();
                string script = @"<script>
                Swal.fire({
                icon: ""success"",
                title: ""預約成功！"",
                showConfirmButton: false,
                timer: 1500
                });

                setTimeout(function () {
                window.location.href = '../User/User_appointment_record.aspx';
                }, 1500);
                </script>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
            }
        }
    }
    private void CheckPlace()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT 地點類型 FROM 健身教練課表課程合併 WHERE 課表編號 = @Schedule_id ";
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Schedule_id", Schedule_id);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                checkPlace = int.Parse(reader["地點類型"].ToString().Trim());
                if (checkPlace == 2){
                    ap_location.Enabled = true;
                    rfvlocation.Enabled = true;
                }
            }
            connection.Close();
        }
    }
    private void UpdatePeople() 
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "UPDATE 健身教練課表 SET 預約人數 = @ap_people WHERE 課表編號 = @Schedule_id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                Search_ap_people();
                command.Parameters.AddWithValue("@ap_people", Ap_people+1);
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
    private bool CheckAP()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string queryCheck = @"
                SELECT 開始時間, 結束時間 
                FROM [使用者預約-有預約的] 
                WHERE 日期 = @Ap_date  
                AND 預約狀態 = 1 ";

            using (SqlCommand checkCommand = new SqlCommand(queryCheck, connection))
            {
                checkCommand.Parameters.AddWithValue("Ap_date", Ap_date);
                using (SqlDataReader reader = checkCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TimeSpan scheduledStartTime = TimeSpan.Parse(reader["開始時間"].ToString());
                        TimeSpan scheduledEndTime = TimeSpan.Parse(reader["結束時間"].ToString());

                        TimeSpan selectedStartTime = TimeSpan.Parse(Ap_starttime);
                        TimeSpan selectedEndTime = TimeSpan.Parse(Ap_endtime);

                        if ((selectedStartTime >= scheduledStartTime && selectedStartTime < scheduledEndTime) ||
                            (selectedEndTime > scheduledStartTime && selectedEndTime <= scheduledEndTime) ||
                            (selectedStartTime <= scheduledStartTime && selectedEndTime >= scheduledEndTime))
                        {
                            string script2 = @"<script>
                                            Swal.fire({
                                              icon: 'error',
                                              title: '預約時段衝突！',
                                              confirmButtonText: '確定',
                                            }).then((result) => {
                                              if (result.isConfirmed) {
                                                 window.location.href = '../User/User_appointment_record.aspx';
                                              }
                                            });
                                            </script>";

                            Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script2, false);
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}