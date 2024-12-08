using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Default : System.Web.UI.Page
{
    private string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCounts();
        }
    }

    private void LoadCounts()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string activeAdsQuery = "SELECT COUNT(*) FROM [廣告] WHERE GETDATE() BETWEEN [上架日] AND [下架日]";
            string futureAdsQuery = "SELECT COUNT(*) FROM [廣告] WHERE [上架日] > GETDATE()";
            string expiredAdsQuery = "SELECT COUNT(*) FROM [廣告] WHERE [下架日] < GETDATE()";

            string pendingCoachesQuery = "SELECT COUNT(*) FROM [健身教練審核] WHERE [審核狀態] = 0";
            string activeCoachesQuery = "SELECT COUNT(*) FROM [健身教練審核] WHERE [審核狀態] = 1";
            string expiredCoachesQuery = "SELECT COUNT(*) FROM [健身教練審核] WHERE [審核狀態] = 3";

            string pendingCommentsQuery = "SELECT COUNT(DISTINCT [評論編號]) FROM [評論檢舉]";



            // 綁定 Label
            lblActiveAds.Text = ExecuteCountQuery(connection, activeAdsQuery).ToString();
            lblFutureAds.Text = ExecuteCountQuery(connection, futureAdsQuery).ToString();
            lblExpiredAds.Text = ExecuteCountQuery(connection, expiredAdsQuery).ToString();
            lblPendingCoaches.Text = ExecuteCountQuery(connection, pendingCoachesQuery).ToString();
            lblActiveCoaches.Text = ExecuteCountQuery(connection, activeCoachesQuery).ToString();
            lblExpiredCoaches.Text = ExecuteCountQuery(connection, expiredCoachesQuery).ToString();
            lblPendingComments.Text = ExecuteCountQuery(connection, pendingCommentsQuery).ToString();
        }
    }

    private int ExecuteCountQuery(SqlConnection connection, string query)
    {
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            return (int)command.ExecuteScalar();
        }
    }
}