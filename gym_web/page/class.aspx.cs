using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Activities.Statements;
using System.Text;
using System.Windows.Input;
using System.Activities.Expressions;

public partial class page_class : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    static bool HomeId;
    protected void Page_Load(object sender, EventArgs e)
    {
        HomeId = true;
        if (!IsPostBack)
        {
            BindClass();
            BindFilter();
        }
    }
    private void BindClass()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "Select * From [健身教練課程-有排課的]";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.SequentialAccess);
            lv_class.DataSource = dataReader;
            lv_class.DataBind();
        }
    }

    protected void lv_class_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "see_detail")
        {
            // 取得課程編號，存入 Session，並跳轉至詳細頁面
            Session["Class_id"] = Convert.ToInt32(e.CommandArgument);
            Response.Redirect("class_detail.aspx");
        }
    }

    protected string GetImageUrl(object imageData, int quality)
    {
        if (imageData != null && imageData != DBNull.Value)
        {
            byte[] bytes = (byte[])imageData;

            using (MemoryStream originalStream = new MemoryStream(bytes))
            using (MemoryStream compressedStream = new MemoryStream())
            {
                // Decode the original image
                System.Drawing.Image originalImage = System.Drawing.Image.FromStream(originalStream);

                // Create an EncoderParameters object to set the image quality
                EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

                // Get the JPG codec info
                ImageCodecInfo jpgCodec = ImageCodecInfo.GetImageEncoders().First(codec => codec.MimeType == "image/jpeg");

                // Save the compressed image to the compressedStream
                originalImage.Save(compressedStream, jpgCodec, encoderParameters);

                // Convert the compressed image to a base64 string
                byte[] compressedBytes = compressedStream.ToArray();
                string base64String = Convert.ToBase64String(compressedBytes);

                // Generate the data URI for the compressed image
                return "data:image/jpeg;base64," + base64String;
            }
        }
        else
        {
            return "img/null.png"; // 替代圖片的路徑
        }
    }

    protected void SearchBtn_Click(object sender, ImageClickEventArgs e)
    {
        using (SqlConnection connection = new SqlConnection(connectionString)) 
        {
            string sql = "SELECT * FROM [健身教練課程-有排課的] WHERE [課程名稱] LIKE '%' + @SearchTxT + '%' OR [分類名稱] LIKE '%' + @SearchTxT + '%'";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@SearchTxT", SearchText.Text);
            SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.SequentialAccess);
            lv_class.DataSource = dataReader;
            lv_class.DataBind();
        }
    }
    private void BindFilter()
    {
        BindTypeDDL();
        BindClassPlaceCBL();
    }
    private void BindTypeDDL()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "SELECT * FROM [健身教練課程-有排課的]";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();
            ClassTypeDDL.DataSource = reader;
            ClassTypeDDL.DataTextField = "分類名稱";  // 要顯示文字
            ClassTypeDDL.DataValueField = "分類編號";  // 值
            ClassTypeDDL.DataBind();
            connection.Close();
            ClassTypeDDL.Items.Insert(0, new ListItem("全部", string.Empty));
           
        }
    }
    private void BindClassPlaceCBL()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "SELECT * FROM 縣市";
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter(sql, connection);
            DataTable dataTable = new DataTable();
            da.Fill(dataTable);
            ClassPlaceCBL.DataSource = dataTable;
            ClassPlaceCBL.DataTextField = "縣市";  // 要顯示文字
            ClassPlaceCBL.DataValueField = "縣市";  // 值
            ClassPlaceCBL.DataBind();
            connection.Close();
        }
    }

    protected void FilterBtn_Click(object sender, EventArgs e)
    {
    }

    protected void SearchFilterBtn_Click(object sender, EventArgs e)
    {
        List<string> selectedItems = new List<string>();
        foreach (ListItem item in ClassPlaceCBL.Items)
        {
            if (item.Selected)
            {
                selectedItems.Add(item.Value);
            }
        }
        string[] selectedPlace = selectedItems.ToArray();
        string[] selectedPlaceHome = CheckHomeServicePlace(selectedPlace).ToArray();//客戶到府
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(
                "SELECT * FROM [健身教練課程-有排課的] " +
                "WHERE ([課程名稱] LIKE '%' + @SearchTxT + '%' OR [分類名稱] LIKE '%' + @SearchTxT + '%') " +
                "AND [分類編號] LIKE '%' + @Type + '%'  " +
                "AND [健身教練性別] LIKE '%' + @Gender + '%'  " +
                "AND ([課程費用] >= @Min AND [課程費用] <= @Max) " +
                "AND ");
            switch (ClassPeopleRBL.SelectedValue)
            {
                case "0":
                    sqlBuilder.Append("[上課人數] LIKE '%' + @People + '%' AND ");
                    break;
                case "1":
                    sqlBuilder.Append("[上課人數] = 1 AND ");
                    break;
                case "2":
                    sqlBuilder.Append("[上課人數] > 1 AND ");
                    break;
            }
            if (HomeId == false) { sqlBuilder.Append("[課程編號] =0 "); }
            else {
                if (selectedPlaceHome == null || selectedPlaceHome.Length == 0) //客戶到府
                {
                    selectedPlaceHome = new string[0];
                    sqlBuilder.Append("[課程編號] LIKE @HomePlace");
                }
                else
                {
                    for (int i = 0; i < selectedPlaceHome.Length; i++)
                    {
                        if (i > 0)
                        {
                            sqlBuilder.Append(" OR ");
                        }
                        sqlBuilder.Append("[課程編號] LIKE @HomePlace" + i);
                    }
                }
            }
            if (selectedPlace == null || selectedPlace.Length == 0)
            {   // 課程地點沒選
                sqlBuilder.Append(" OR ");
                selectedPlace = new string[0];
                sqlBuilder.Append("[顯示地點地址] LIKE @Place");
            }
            else
            {   // 課程地點有選
                sqlBuilder.Append(" OR ");
                for (int i = 0; i < selectedPlace.Length; i++)
                {
                    if (i > 0)
                    {
                        sqlBuilder.Append(" OR ");
                    }
                    sqlBuilder.Append("[顯示地點地址] LIKE @Place" + i);
                }
            }
            string sql = sqlBuilder.ToString();

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@SearchTxT", SearchText.Text);
                command.Parameters.AddWithValue("@Type",ClassTypeDDL.SelectedValue);
                command.Parameters.AddWithValue("@Gender",CoachGenderRB.SelectedValue);
                string min,max;
                if (MinMoney.Text.ToString().Trim() == string.Empty) { min = "0"; } else { min = MinMoney.Text.ToString().Trim(); }
                if (MaxMoney.Text.ToString().Trim() == string.Empty) { max = "9999"; } else { max = MaxMoney.Text.ToString().Trim(); }
                command.Parameters.AddWithValue("@Min",min);
                command.Parameters.AddWithValue("@Max",max);
                switch (ClassPeopleRBL.SelectedValue)
                {
                    case "0":
                        command.Parameters.AddWithValue("@People", "");
                        break;
                }
                
                if (selectedPlaceHome == null || selectedPlaceHome.Length == 0)//客戶到府
                {
                    command.Parameters.AddWithValue("@HomePlace", "%" + "" + "%");
                }
                else 
                {
                    for (int i = 0; i < selectedPlaceHome.Length; i++)
                    {
                        command.Parameters.AddWithValue("@HomePlace" + i, "%" + selectedPlaceHome[i] + "%");
                    }
                }
                // 課程地點沒選
                if (selectedPlace == null || selectedPlace.Length == 0)
                {
                    command.Parameters.AddWithValue("@Place", "%"+""+"%");
                }
                else
                {
                    // 課程地點有選
                    for (int i = 0; i < selectedPlace.Length; i++)
                    {
                        command.Parameters.AddWithValue("@Place" + i, "%" + selectedPlace[i] + "%");
                    }
                }
                connection.Open();

                SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.SequentialAccess);
                lv_class.DataSource = dataReader;
                lv_class.DataBind();
            }
        }
    }
    private List<string> CheckHomeServicePlace(string[] selectedPlace)
    {
        List<string> coachIds = new List<string>();
        StringBuilder sqlBuilder = new StringBuilder();
        sqlBuilder.Append(
            "SELECT DISTINCT [課程編號] FROM [教練到府偏好] " +
            "WHERE ");
        if (selectedPlace == null || selectedPlace.Length == 0)
        {   // 課程地點沒選
            selectedPlace = new string[0];
            sqlBuilder.Append("[縣市] LIKE @Place ");
        }
        else
        {   // 課程地點有選
            for (int i = 0; i < selectedPlace.Length; i++)
            {
                if (i > 0)
                {
                    sqlBuilder.Append(" OR ");
                }
                sqlBuilder.Append("[縣市] LIKE @Place" + i);
            }
        }
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = sqlBuilder.ToString();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                if (selectedPlace == null || selectedPlace.Length == 0)
                {
                    command.Parameters.AddWithValue("@Place", "%" + "" + "%");
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string coachId = reader["課程編號"].ToString();
                            if (!string.IsNullOrEmpty(coachId))
                            {
                                coachIds.Add(coachId); // 添加課程編號到結果列表
                            }
                        }
                    }
                }
                else
                {
                    // 課程地點有選
                    for (int i = 0; i < selectedPlace.Length; i++)
                    {
                        command.Parameters.AddWithValue("@Place" + i, "%" + selectedPlace[i] + "%");
                    }
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string coachId = reader["課程編號"].ToString();
                            if (!string.IsNullOrEmpty(coachId))
                            {
                                coachIds.Add(coachId); // 添加課程編號到結果列表
                            }
                        }
                    }
                    if (coachIds.Count == 0) { HomeId = false; }
                }

            }
        }
        return coachIds;
    }
}