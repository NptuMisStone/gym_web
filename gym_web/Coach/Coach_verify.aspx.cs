using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Expressions;
using System.Xml.Linq;

public partial class Coach_Coach_verify : System.Web.UI.Page
{
    private static string connectionString = ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    private string area;
    public static string city;
    public static string Coach_id;
    protected void Page_Load(object sender, EventArgs e)
    {
        Coach_id = Convert.ToString(Session["Coach_id"]);

        //驗證教練是否登入的類別函數
        CoachHelper.CheckLogin(this);

        if (!IsPostBack)
        {
            ShowSweetAlert();
        }
    }
    private void ShowSweetAlert()
    {
        // Define a function to show SweetAlert
        string script = @"<script>
                    Swal.fire({
                        icon: 'info',
                        title: '歡迎驗證健身教練身分',
                        html: '<strong>驗證注意事項：</strong><br>' +
                            '<ul>' +
                            '<li>請提供您的教練服務單位名稱、服務地點、聯絡電話、電子郵件地址以及詳細地址等相關資訊。這些資訊將用於驗證您的身分。</li>' +
                            '<li>您需要上傳相應的資格證明文件，以確保您具備相關的教練資格。請注意，這些文件應該符合我們的審核標準。提供清晰且完整的文件將有助於加快審核過程。</li>' +
                            '<li>請確保所提供的資訊是真實且準確的，包括個人資料和上傳的文件。提供虛假資訊可能會導致您的驗證申請被拒絕或賬戶被凍結。</li>' +
                            '<li>我們尊重您的隱私權，所有提供的個人資料將受到嚴格的保護和保密。請閱讀我們的隱私政策，以了解我們如何處理您的資訊。</li>' +
                            '<li>在提交驗證申請前，請仔細閱讀並同意我們的使用條款和條件。這些條款將規範您的權利和義務。</li>' +
                            '<li>如果您在驗證過程中遇到任何問題或有疑問，請隨時聯繫我們的客戶支援團隊。我們將樂意協助您完成驗證。</li>' +
                            '<li>驗證通過後，您將能夠在我們的平台上享受教練相關的服務和機會。請確保您已經準備好開始參與。</li>' +
                            '</ul>',
                        confirmButtonText: '開始驗證'
                    });
                </script>";

        // 注册 SweetAlert 脚本到页面
        Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", script, false);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string type = rdtype.SelectedValue;
        string name = txtName.Text;
        string phone = txtPhone.Text;
        string email = txtEmail.Text;
        string address = txtAddress.Text;
        byte[] data = null;
        if (fuVerificationData.HasFile)
        {
            using (BinaryReader reader = new BinaryReader(fuVerificationData.PostedFile.InputStream))
            {
                data = reader.ReadBytes(fuVerificationData.PostedFile.ContentLength);
            }
        }

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string checkQuery = "SELECT 審核狀態 FROM 健身教練審核 WHERE 健身教練編號 = @CoachId";

            using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
            {
                checkCommand.Parameters.AddWithValue("@CoachId", Coach_id);
                var status = checkCommand.ExecuteScalar();

                if (status != null)
                {
                    string reviewStatus = status.ToString();
                    if (reviewStatus == "2" || reviewStatus == "1")
                    {
                        string updateQuery = "UPDATE 健身教練審核 SET 註冊類型 = @Type, 服務地點名稱 = @Name, 服務地點電話 = @Phone, 服務地點郵件 = @Email, 服務地點地址 = @Address, 審核資料 = @Data, 審核狀態 = @Status WHERE 健身教練編號 = @CoachId";
                        using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@Type", type);
                            updateCommand.Parameters.AddWithValue("@Name", name);
                            updateCommand.Parameters.AddWithValue("@Phone", phone);
                            updateCommand.Parameters.AddWithValue("@Email", email);
                            updateCommand.Parameters.AddWithValue("@Address", address);
                            updateCommand.Parameters.AddWithValue("@Data", data);
                            updateCommand.Parameters.AddWithValue("@Status", "0");
                            updateCommand.Parameters.AddWithValue("@CoachId", Coach_id);

                            updateCommand.ExecuteNonQuery();
                        }
                    }
                    else if (reviewStatus == "0")
                    {
                        string successScript1 = @"<script>
                            Swal.fire({
                                icon: 'error',
                                title: '您已提出申請',
                                text: '請靜待審核，請勿重複申請。',
                                onClose: function() {
                                window.location.href = '../Coach/Coach_info.aspx'; // 跳轉到 index.aspx
                                }
                            });
                        </script>";

                        ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertSuccess", successScript1);
                    }
                }
                else
                {
                    string insertQuery = "INSERT INTO 健身教練審核 (健身教練編號, 註冊類型, 服務地點名稱, 服務地點電話, 服務地點郵件, 服務地點地址, 審核資料, 審核狀態) " +
                        "VALUES (@CoachId, @Type, @Name, @Phone, @Email, @Address, @Data, @Status)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@CoachId", Coach_id);
                        command.Parameters.AddWithValue("@Type", type);
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Phone", phone);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Address", address);
                        command.Parameters.AddWithValue("@Data", data);
                        command.Parameters.AddWithValue("@Status", "0");

                        command.ExecuteNonQuery();
                    }
                }
            }
        }


        // 成功提交的 SweetAlert 脚本
        string successScript = @"<script>
    Swal.fire({
        icon: 'success',
        title: '已成功送出您的申請',
        text: '請靜待審核，並留意您的信箱是否通過申請。',
        onClose: function() {
            window.location.href = '../Coach/Coach_info.aspx'; // 跳轉到 index.aspx
        }
    });
</script>";

        ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertSuccess", successScript);


    }

    protected void ddl_city_SelectedIndexChanged(object sender, EventArgs e)
    {
        // 根據選擇的類型進行過濾
        string type = rdtype.SelectedValue;

        if (type == "店家健身教練")
        {
            // 處理地區查詢的顯示
            area = ddl_city.SelectedItem.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT DISTINCT 服務地點名稱, 服務地點地址, 服務地點郵件, 服務地點電話 FROM 健身教練審核 WHERE 服務地點地址 LIKE @地址+'%' AND 審核狀態 = 1";
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@地址", area);
                command.Parameters.AddWithValue("@Type", type); // 根據選擇的類型過濾
                SqlDataReader dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    lb_no_result.Visible = false;
                    dl_shop.Visible = true;
                    dl_shop.DataSource = dataReader;
                    dl_shop.DataBind();
                }
                else
                {
                    lb_no_result.Visible = true;
                    dl_shop.Visible = false;
                }

                dataReader.Close();
                connection.Close();
            }

            city = ddl_city.SelectedItem.Text;
            int city_id = Convert.ToInt32(ddl_city.SelectedValue);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT 行政區 FROM 行政區 WHERE 縣市id=@縣市id";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@縣市id", city_id);

                SqlDataReader dataReader = cmd.ExecuteReader();

                ddl_area.DataSource = dataReader;
                ddl_area.DataTextField = "行政區";
                ddl_area.DataValueField = "行政區";
                ddl_area.DataBind();

                dataReader.Close();
                connection.Close();
            }
        }
        else if(type == "私人健身教練") 
        {
            // 如果選擇私人教練，隱藏查詢相關元素
            lb_no_result.Visible = false;
            dl_shop.Visible = false;
            ddl_area.Visible = false;
            ddl_city.Visible = false;
        }
    }

    protected void ddl_city_DataBound(object sender, EventArgs e)
    {
        ddl_city.Items.Insert(0, new ListItem("請選擇縣市", "0"));
    }

    protected void ddl_area_SelectedIndexChanged(object sender, EventArgs e)
    {
        string city = ddl_city.SelectedItem.Text;
        string area = ddl_area.SelectedItem.Text;
        string fullAddress = city + area;

        Debug.WriteLine("selected=" + fullAddress);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "SELECT DISTINCT 服務地點名稱, 服務地點地址, 服務地點郵件, 服務地點電話 FROM 健身教練審核 WHERE 服務地點地址 LIKE @地址+'%' AND 審核狀態 = 1";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@地址", fullAddress);
            SqlDataReader dataReader = command.ExecuteReader();

            if (dataReader.HasRows)
            {
                lb_no_result.Visible = false;
                dl_shop.Visible = true;
                dl_shop.DataSource = dataReader;
                dl_shop.DataBind();
            }
            else
            {
                lb_no_result.Visible = true;
                dl_shop.Visible = false;
            }

            dataReader.Close();
            connection.Close();
        }
    }


    protected void ddl_area_DataBound(object sender, EventArgs e)
    {
        ddl_area.Items.Insert(0, new ListItem("全部", "0"));
    }
    
    protected void rdVerifyMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        rd_set();
    }
   
    protected void dl_shop_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "select_shop")
        {
            string[] shopDetails = e.CommandArgument.ToString().Split(';');
            if (shopDetails.Length == 4)
            {
                txtName.Text = shopDetails[0];
                txtPhone.Text = shopDetails[1];
                txtEmail.Text = shopDetails[2];
                txtAddress.Text = shopDetails[3];
            }
        }
    }
    private void rd_set()
    {
        lb_no_result.Visible = false;
        ddl_city.SelectedIndex = 0;
        ddl_area.Items.Clear();
        ddl_area.Items.Add(new ListItem("全部", "0"));
        ddl_area.SelectedIndex = 0;
        string type = rdtype.SelectedValue;

        if (type == "店家健身教練")
        {
            Name.Visible = true;
            Phone.Visible = true;
            Email.Visible = true;
            Address.Visible = true;
            txtName.Enabled = false;
            txtPhone.Enabled = false;
            txtEmail.Enabled = false;
            txtAddress.Enabled = false;
            search_type.Visible = true;
            search_form.Visible = true;
            search_form_detail.Visible = true;
            ddl_city.Visible = true;
            ddl_area.Visible = true;

            if (rdVerifyMode.SelectedValue == "search")
            {
                // 顯示地區查詢形式
                search_form.Visible = true;
                search_form_detail.Visible = true;
            }
            else if (rdVerifyMode.SelectedValue == "manual")
            {
                // 顯示手動輸入形式
                txtName.Enabled = true;
                txtPhone.Enabled = true;
                txtEmail.Enabled = true;
                txtAddress.Enabled = true;
                search_form.Visible = false;
                search_form_detail.Visible = false;
            }
        }
        else if (type == "私人健身教練")
        {
            // 隱藏查詢相關元素
            Name.Visible = false;
            Phone.Visible = false;
            Email.Visible = false;
            Address.Visible = false;
            search_type.Visible = false;
            search_form.Visible = false;
            search_form_detail.Visible = false;
            ddl_city.Visible = false;
            ddl_area.Visible = false;
        }
    }
}