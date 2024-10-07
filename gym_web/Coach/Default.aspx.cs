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
            UpdateCaptchaImage();
        }
    }
    private void UpdateCaptchaImage()
    {
        // 加上時間戳以避免瀏覽器快取
        imgCaptcha.ImageUrl = "~/Captcha.ashx?t=" + DateTime.Now.Ticks;
    }
    protected void imgCaptcha_Click(object sender, EventArgs e)
    {
        CaptchaHelper.CreateImageText(Session);
        UpdateCaptchaImage();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string randomString = Session["ImgText"] as string;
        if (randomString != txtAnswer.Text.Trim())
        {
            ShowAlert("error", "驗證錯誤", "請重新輸入", 1500);
            CaptchaHelper.CreateImageText(Session);
        }
        else
        {
            ShowAlert("success", "驗證成功", "繼續進行", 1500);
        }
    }
    private void ShowAlert(string icon, string title, string text, int timer, string redirectUrl = null)
    {
        string script = $@"<script>
            Swal.fire({{
                icon: '{icon}',
                title: '{title}',
                text: '{text}',
                showConfirmButton: false,
                timer: {timer}
            }});
            {(redirectUrl != null ? $"setTimeout(function() {{ window.location.href = '{redirectUrl}'; }}, {timer});" : "")}
        </script>";

        Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlertScript", script, false);
    }
}