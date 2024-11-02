using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// UserManager用來查詢name並展示在頁首
/// </summary>
public class UserManager
{
    public int UserId { get; set; }
    public int CoachId { get; set; }
    public bool UserLoginSuccess { get; set; }
    public bool CoachLoginSuccess { get; set; }

    private string ConnectionString => System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;

    public UserManager()
    {
        UserId = Convert.ToInt32(HttpContext.Current.Session["user_id"]);
        CoachId = Convert.ToInt32(HttpContext.Current.Session["coach_id"]);
        UserLoginSuccess = Convert.ToBoolean(HttpContext.Current.Session["user_loginsuccess"]);
        CoachLoginSuccess = Convert.ToBoolean(HttpContext.Current.Session["coach_loginsuccess"]);
    }

    public string GetUserName()
    {
        string u_name = "";
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            string query = "SELECT 使用者姓名 FROM 使用者資料 WHERE 使用者編號 = @使用者編號";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@使用者編號", UserId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    u_name = reader["使用者姓名"].ToString();
                }
                connection.Close();
            }
        }
        return "Hi, " + u_name;
    }

    public string GetCoachName()
    {
        string c_name = "";
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            string query = "SELECT 健身教練姓名 FROM 健身教練資料 WHERE 健身教練編號 = @健身教練編號";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@健身教練編號", CoachId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    c_name = reader["健身教練姓名"].ToString();
                }
                connection.Close();
            }
        }
        return "Hi, " + c_name + " 教練";
    }
}
