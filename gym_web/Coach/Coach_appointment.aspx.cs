using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

public partial class Coach_Coach_appointment : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;

    public static string Coach_id, TD,Ap_People,Coach_Time;
    public static int Schedule_id;
    protected void Page_Load(object sender, EventArgs e)
    {
        Coach_id = Convert.ToString(Session["Coach_id"]);
        //驗證教練是否登入的類別函數
        CoachHelper.CheckLogin(this);
        TD = DateTime.Now.ToString("yyyy-MM-dd");
        if (!IsPostBack)
        {
            BindTdData();
            BindFTData();
            BindPSData();
        }
    }
    private void BindTdData() 
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "Select * From 健身教練課表課程合併 Where 健身教練編號=@CoachID AND 日期 = @TD ORDER BY 開始時間";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CoachID", Coach_id);
            command.Parameters.AddWithValue("@TD", TD);
            SqlDataReader dataReader = command.ExecuteReader();
            TodayRP.DataSource = dataReader;
            TodayRP.DataBind();
            connection.Close();
        }
    }
    private void BindFTData()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "Select * From 健身教練課表課程合併 Where 健身教練編號=@CoachID AND 日期 > @TD ORDER BY 日期 , 開始時間 ";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CoachID", Coach_id);
            command.Parameters.AddWithValue("@TD", TD);
            SqlDataReader dataReader = command.ExecuteReader();
            FutureRP.DataSource = dataReader;
            FutureRP.DataBind();
            connection.Close();
        }
    }
    private void BindPSData()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "Select * From 健身教練課表課程合併 Where 健身教練編號=@CoachID AND 日期 < @TD ORDER BY 日期 , 開始時間 ";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CoachID", Coach_id);
            command.Parameters.AddWithValue("@TD", TD);
            SqlDataReader dataReader = command.ExecuteReader();
            PastRP.DataSource = dataReader;
            PastRP.DataBind();
            connection.Close();
        }
    }

    protected void TodayRP_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "TD_AP")
        {
            Schedule_id =Convert.ToInt32(e.CommandArgument);
            ShowAP(Schedule_id);
            Panel1.Visible= true;
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#" + Panel1.ClientID + "').modal('show');", true);
        }
    }

    protected void FutureRP_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "FT_AP")
        {
            Schedule_id = Convert.ToInt32(e.CommandArgument);
            ShowAP(Schedule_id);
            Panel1.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#" + Panel1.ClientID + "').modal('show');", true);
        }
    }

    protected void PastRP_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "PS_AP")
        {
            Schedule_id = Convert.ToInt32(e.CommandArgument);
            ShowAP(Schedule_id);
            Panel1.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#" + Panel1.ClientID + "').modal('show');", true);
        }
    }
    private void ShowAP(int schedule_id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString)) 
        {
            string sql = "Select * From [使用者預約-評論用] Where 課表編號=@ScheduleID AND (預約狀態=1 OR  預約狀態=2)";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ScheduleID", schedule_id);
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.HasRows)
            {
                AP_Detail.DataSource = dataReader;
                AP_Detail.DataBind();
                lblNoData.Visible = false;
            }
            else
            {
                lblNoData.Text = "無資料";
                lblNoData.Visible = true;
                AP_Detail.DataSource = null;
                AP_Detail.DataBind();
            }
            
            connection.Close();
        }
    }

    protected void AP_Detail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancel") 
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = AP_Detail.Rows[rowIndex];
            string ap_id = row.Cells[0].Text.Trim();
            string s_id=row.Cells[1].Text.Trim();
            CancelAP(ap_id);/*預約狀態更改*/
            CancelApPeople(s_id);/*預約人數更改*/
            BindTdData();
            BindFTData();
            BindPSData();
            ShowAP(Schedule_id);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#" + Panel1.ClientID + "').modal('show');", true);
        }
        else if(e.CommandName == "Finish")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = AP_Detail.Rows[rowIndex];
            string ap_id = row.Cells[0].Text.Trim();
            string c_id = row.Cells[2].Text.Trim();
            FinishAP(ap_id);
            FinishCoachTime(c_id);
            ShowAP(Schedule_id);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#" + Panel1.ClientID + "').modal('show');", true);
        }
    }
    private void CancelAP(string ap_id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "UPDATE 使用者預約 SET 預約狀態 = 5 WHERE 預約編號 = @AP_id";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@AP_id", ap_id);
            command.ExecuteReader();
            connection.Close();
        }
    }
    private void SearchApPeople(string s_id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "Select 預約人數 From 健身教練課表 Where 課表編號=@ScheduleID";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ScheduleID",s_id );
            SqlDataReader reader= command.ExecuteReader();
            if (reader.Read()) 
            {
                Ap_People = reader["預約人數"].ToString().Trim();
            }
            connection.Close();
        }
    }
    private void CancelApPeople(string s_id)
    {
        SearchApPeople(s_id);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "UPDATE 健身教練課表 SET 預約人數 = @AP_People WHERE 課表編號 = @ScheduleID";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@AP_People",int.Parse(Ap_People)-1);
            command.Parameters.AddWithValue("@ScheduleID", s_id);
            command.ExecuteReader();
            connection.Close();
        }
    }
    protected void AP_Detail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        e.Row.Cells[1].Visible = false;
        e.Row.Cells[2].Visible = false;
        e.Row.Cells[3].Visible = false;
    }

    protected void AP_Detail_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        //取消預約用，勿刪
    }
    private void FinishAP(string ap_id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "UPDATE 使用者預約 SET 預約狀態 = 2 WHERE 預約編號 = @AP_id";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@AP_id", ap_id);
            command.ExecuteReader();
            connection.Close();
        }
    }
    private void SearchCoachTime(string c_id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "Select 健身教練次數 From 健身教練資料 Where 健身教練編號=@CoachID";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CoachID", c_id);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Coach_Time = reader["健身教練次數"].ToString().Trim();
            }
            connection.Close();
        }
    }
    private void FinishCoachTime(string c_id) 
    {
        SearchCoachTime(c_id);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "UPDATE 健身教練資料 SET 健身教練次數 = @c_time WHERE 健身教練編號 = @CoachID";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@c_time", int.Parse(Coach_Time) + 1);
            command.Parameters.AddWithValue("@CoachID", c_id);
            command.ExecuteReader();
            connection.Close();
        }
    }
    protected void AP_Detail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // 假设根据某个字段的值判断是否隐藏按钮
            int status = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "預約狀態"));

            // 当状态为 1 时，隐藏 "取消預約" 按钮
            if (status == 2)
            {
                // 隐藏 ButtonField，指定对应列的索引（假设它是第3列，索引为2）
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
            }
        }
    }
}