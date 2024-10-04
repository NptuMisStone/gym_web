using System;
using System.Web;
using System.Web.UI;

/// <summary>
/// UserHelper 的摘要描述
/// </summary>
public class UserHelper
{
    public static void CheckLogin(Page page)
    {
        string userId = Convert.ToString(HttpContext.Current.Session["User_id"]);
        if (string.IsNullOrEmpty(userId))
        {
            string script = @"<script>
                Swal.fire({
                  icon: 'error',
                  title: '請先登入！',
                  confirmButtonText: '確定',
                }).then((result) => {
                  if (result.isConfirmed) {
                     window.location.href = '../User/User_login.aspx';
                  }
                });
                </script>";

            page.ClientScript.RegisterStartupScript(page.GetType(), "SweetAlertScript", script, false);
        }
    }
}
