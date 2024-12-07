using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Coach_Coach_class : System.Web.UI.Page
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ManagerConnectionString"].ConnectionString;
    public static string Coach_id, Class_id, classType_id;

    protected void Page_Load(object sender, EventArgs e)
    {
        Coach_id = Convert.ToString(Session["Coach_id"]);

        //驗證教練是否登入的類別函數
        CheckLogin.CheckUserOrCoachLogin(this.Page, "Coach");

        if (!IsPostBack)
        {
            BindCourses();
        }
    }

    private void BindCourses()
    {
        string query = @"SELECT c.*, ct.分類名稱 FROM [健身教練課程] c 
                         JOIN [運動分類清單] ct ON c.分類編號 = ct.分類編號 
                         WHERE c.[健身教練編號] = @CoachID";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CoachID", Coach_id);
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                // 綁定資料到 ListView 控制項
                lv_class.DataSource = dt;
                lv_class.DataBind();
            }
        }
    }
    protected void lv_class_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "see_detail")
        {
            // 取得課程編號
            Session["Class_id"] = Convert.ToInt32(e.CommandArgument);
            Response.Redirect("Coach_editclass.aspx");
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("Coach_addclass.aspx");
    }


    protected string GetImageUrl(object imageData, int quality)
    {
        try
        {
            if (imageData is byte[] bytes && bytes.Length > 0)
            {
                using (MemoryStream originalStream = new MemoryStream(bytes))
                using (MemoryStream compressedStream = new MemoryStream())
                {
                    System.Drawing.Image originalImage = System.Drawing.Image.FromStream(originalStream);
                    EncoderParameters encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

                    ImageCodecInfo jpgCodec = ImageCodecInfo.GetImageEncoders().First(codec => codec.MimeType == "image/jpeg");
                    originalImage.Save(compressedStream, jpgCodec, encoderParameters);

                    byte[] compressedBytes = compressedStream.ToArray();
                    return "data:image/jpeg;base64," + Convert.ToBase64String(compressedBytes);
                }
            }
            else
            {
                return "img/class_default.png";
            }
        }
        catch (Exception ex)
        {
            // 記錄例外資訊以便調試
            Console.WriteLine($"Error in GetImageUrl: {ex.Message}");
            return "img/class_default.png";
        }
    }
}
