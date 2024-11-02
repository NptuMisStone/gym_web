using System;
using System.Web;
using System.Web.UI;

/// <summary>
/// CheckLogin 的摘要描述
/// </summary>
public class CheckLogin
{
    public static void CheckUserOrCoachLogin(Page page, string userType)
    {
        HttpContext.Current.Session["ReturnUrl_User"] = null;
        HttpContext.Current.Session["ReturnUrl_Coach"] = null;

        // 根據用戶類型選擇要檢查的 Session ID 和 ReturnUrl
        string sessionKey = userType == "User" ? "User_id" : "Coach_id";
        string returnUrlKey = userType == "User" ? "ReturnUrl_User" : "ReturnUrl_Coach";

        // 取得對應的 Session 值
        string sessionValue = Convert.ToString(HttpContext.Current.Session[sessionKey]);

        if (string.IsNullOrEmpty(sessionValue))
        {
            // 將當前頁面路徑存入對應的 ReturnUrl Session
            HttpContext.Current.Session[returnUrlKey] = page.Request.Url.PathAndQuery;

            string script = @"<script>
            Swal.fire({
            icon: 'error',
            title: '請先登入！',
            confirmButtonText: '確定',
            allowOutsideClick: false, // 禁用外部點擊關閉彈窗
            allowEscapeKey: false, // 禁用 Esc 鍵關閉彈窗
            }).then((result) => {
            if (result.isConfirmed) {
                window.location.href = '../page/Login.aspx';
                }
            });
            </script>";


            page.ClientScript.RegisterStartupScript(page.GetType(), "SweetAlertScript", script, false);
        }
    }

}
