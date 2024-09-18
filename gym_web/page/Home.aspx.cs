using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCarouselImages();
        }
    }
    private void LoadCarouselImages()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
        List<Tuple<string, string>> imagesAndLinks = new List<Tuple<string, string>>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = @"
                SELECT [圖片], [連結] 
                FROM [廣告]
                WHERE GETDATE() BETWEEN [上架日] AND [下架日]";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        byte[] imageData = (byte[])reader["圖片"];
                        string base64String = Convert.ToBase64String(imageData);
                        string imageUrl = "data:image/jpeg;base64," + base64String;
                        string link = reader["連結"].ToString();
                        imagesAndLinks.Add(new Tuple<string, string>(imageUrl, link));
                    }
                }
            }
            connection.Close();
        }

        Repeater1.DataSource = imagesAndLinks;
        Repeater1.DataBind();
    }
}