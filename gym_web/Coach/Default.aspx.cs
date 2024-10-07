using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Coach_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CaptchaHelper.CreateImageText(Session);
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string randomString = Session["ImgText"] as string;
        if (randomString != txtAnswer.Text.Trim())
        {
            Response.Write("驗證錯誤");
            CaptchaHelper.CreateImageText(Session);
        }
        else
        {
            Response.Write("驗證成功");
        }
    }
}