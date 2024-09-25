using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// CoachHelper 的摘要描述
/// </summary>
public class CoachHelper
{
    public static void CheckLogin(Page page)
    {
        string coachId = Convert.ToString(HttpContext.Current.Session["Coach_id"]);
        if (string.IsNullOrEmpty(coachId))
        {
            string script = @"<script>
                Swal.fire({
                  icon: 'error',
                  title: '請先登入！',
                  confirmButtonText: '確定',
                }).then((result) => {
                  if (result.isConfirmed) {
                     window.location.href = '../Coach/Coach_login.aspx';
                  }
                });
                </script>";

            page.ClientScript.RegisterStartupScript(page.GetType(), "SweetAlertScript", script, false);
        }
    }
}