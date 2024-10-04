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
using System.Net.NetworkInformation;
using static System.Net.Mime.MediaTypeNames;

public partial class page_class : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    static string SearchTxT;
    private List<string> selectedCity = new List<string>();
    private List<string> selectedArea = new List<string>();
    private string[] selectCity;
    private string[] selectArea;
    protected void Page_Load(object sender, EventArgs e)
    {
        SearchTxT = "";
        if (!IsPostBack)
        {
            BindClass();
            BindFilter();
        }
        else {
            string eventTarget = Request["__EVENTTARGET"];
            if (eventTarget == "overlayClicked")
            {
                CoachGenderRB.SelectedIndex= 0;
                MinMoney.Text=string.Empty;
                MaxMoney.Text=string.Empty;
                ClassPeopleRBL.SelectedIndex= 0;
                CityTreeView.Nodes.Clear();
                BindFilter();
                BindClass();
            }
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
        FinishSearch();
    }
    private void BindFilter()
    {
        BindTypeDDL();
        BindTreeView();// 縣市、行政區
        CollapseAllNodes(CityTreeView.Nodes); // 讓他預設是摺疊起來的
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

    protected void FilterBtn_Click(object sender, EventArgs e)
    {
    }

    protected void SearchFilterBtn_Click(object sender, EventArgs e)
    {
        FinishSearch();
    }
    private void FinishSearch() 
    {
        selectCity = Session["selectCity"] as string[];
        selectArea= Session["selectArea"] as string[];
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(
                "SELECT * FROM [健身教練課程-有排課的] " +
                "WHERE [課程名稱] LIKE '%' + @SearchTxT+ '%'  " +
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
            if (selectCity == null || selectCity.Length == 0)
            {
                selectCity = new string[0];
                sqlBuilder.Append("  (( [縣市] LIKE @City )");
            }
            else
            {
                sqlBuilder.Append("(( ");
                for (int i = 0; i < selectCity.Length; i++)
                {
                    if (i > 0)
                    {
                        sqlBuilder.Append(" OR ");
                    }
                    sqlBuilder.Append("[縣市] LIKE @City" + i);
                }
                sqlBuilder.Append(" ) ");
            }
            if (selectArea == null || selectArea.Length == 0)
            {
                sqlBuilder.Append(" AND ( ");
                selectArea = new string[0];
                sqlBuilder.Append(" [行政區] LIKE @Area )) ");
            }
            else
            {
                sqlBuilder.Append(" AND ( ");
                for (int i = 0; i < selectArea.Length; i++)
                {
                    if (i > 0)
                    {
                        sqlBuilder.Append(" OR ");
                    }
                    sqlBuilder.Append("[行政區] LIKE @Area" + i);
                }
                sqlBuilder.Append(" )) ");
            }
            string sql = sqlBuilder.ToString();

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@SearchTxT", SearchText.Text);
                command.Parameters.AddWithValue("@Type", ClassTypeDDL.SelectedValue);
                command.Parameters.AddWithValue("@Gender", CoachGenderRB.SelectedValue);
                string min, max;
                if (MinMoney.Text.ToString().Trim() == string.Empty) { min = "0"; } else { min = MinMoney.Text.ToString().Trim(); }
                if (MaxMoney.Text.ToString().Trim() == string.Empty) { max = "9999"; } else { max = MaxMoney.Text.ToString().Trim(); }
                command.Parameters.AddWithValue("@Min", min);
                command.Parameters.AddWithValue("@Max", max);
                switch (ClassPeopleRBL.SelectedValue)
                {
                    case "0":
                        command.Parameters.AddWithValue("@People", "");
                        break;
                }

                if (selectCity == null || selectCity.Length == 0)
                {
                    command.Parameters.AddWithValue("@City", "%" + "" + "%");
                }
                else
                {
                    for (int i = 0; i < selectCity.Length; i++)
                    {
                        command.Parameters.AddWithValue("@City" + i, "%" + selectCity[i] + "%");
                    }
                }
                if (selectArea == null || selectArea.Length == 0)
                {
                    command.Parameters.AddWithValue("@Area", "%" + "" + "%");
                }
                else
                {
                    for (int i = 0; i < selectArea.Length; i++)
                    {
                        command.Parameters.AddWithValue("@Area" + i, "%" + selectArea[i] + "%");
                    }
                }
                connection.Open();
                SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.SequentialAccess);
                lv_class.DataSource = dataReader;
                lv_class.DataBind();
            }
        }
    }
    protected void CityTreeView_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
    {
        if (e.Node.Checked)
        {
            CheckAllChildNodes(e.Node, true);
        }
        else
        {
            CheckAllChildNodes(e.Node, false);
        }

        // 更新
        UpdateParentNodes(e.Node);
        //更新選重的值
        UpdateSelectedValues();
    }
    private void CheckAllChildNodes(TreeNode parentNode, bool isChecked)
    {
        foreach (TreeNode childNode in parentNode.ChildNodes)
        {
            childNode.Checked = isChecked;
        }
    }

    private void UpdateParentNodes(TreeNode node)
    {
        TreeNode parentNode = node.Parent;
        if (parentNode == null) return;

        bool hasCheckedChild = false;
        foreach (TreeNode siblingNode in parentNode.ChildNodes)
        {
            if (siblingNode.Checked)
            {
                hasCheckedChild = true;
                break;
            }
        }

        parentNode.Checked = hasCheckedChild;

        UpdateParentNodes(parentNode);
    }
    private void UpdateSelectedValues()
    {
        selectedCity.Clear();
        selectedArea.Clear();

        foreach (TreeNode parentNode in CityTreeView.Nodes)
        {
            if (parentNode.Checked)
            {
                selectedCity.Add(parentNode.Value);
            }

            foreach (TreeNode childNode in parentNode.ChildNodes)
            {
                if (childNode.Checked)
                {
                    selectedArea.Add(childNode.Value);
                }
            }
        }
        Session["selectCity"]  = selectedCity.ToArray();
        Session["selectArea"] = selectedArea.ToArray();
    }
    private DataTable GetParentData()
    {
        DataTable cityTable = new DataTable();
        using (SqlConnection conn= new SqlConnection(connectionString))
        {
            string sql = "SELECT 縣市id,縣市 FROM 縣市";
            conn.Open();
            SqlCommand cmd =new SqlCommand(sql,conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(cityTable);
        }
        return cityTable;
    }

    private DataTable GetChildData()
    {
        DataTable areaTable = new DataTable();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string sql = "SELECT 縣市id,行政區id,行政區 FROM 行政區";
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(areaTable);
        }
        return areaTable;
    }
    private void BindTreeView()
    {
        DataTable parentTable = GetParentData();
        DataTable childTable = GetChildData();
        //parent是父結點(縣市) child是子結點(行政區)
        foreach (DataRow parentRow in parentTable.Rows)
        {
            TreeNode parentNode = new TreeNode(parentRow["縣市"].ToString(), parentRow["縣市"].ToString());
            parentNode.ShowCheckBox = true; // 顯示checkbox
            parentNode.SelectAction = TreeNodeSelectAction.None; // 禁用文字觸發
            DataRow[] childRows = childTable.Select("縣市id = " + parentRow["縣市id"].ToString());

            foreach (DataRow childRow in childRows)
            {
                TreeNode childNode = new TreeNode(childRow["行政區"].ToString(), childRow["行政區"].ToString());
                childNode.ShowCheckBox = true; // 顯示checkbox
                childNode.SelectAction = TreeNodeSelectAction.None; // 禁用文字觸發
                parentNode.ChildNodes.Add(childNode);
            }

            CityTreeView.Nodes.Add(parentNode);
        }
    }
    private void CollapseAllNodes(TreeNodeCollection nodes)
    {
        foreach (TreeNode node in nodes)
        {
            node.Collapse(); // 摺疊
            CollapseAllNodes(node.ChildNodes); // 行政區折疊
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
    }
}