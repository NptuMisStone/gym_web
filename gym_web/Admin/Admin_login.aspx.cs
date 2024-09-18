using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Admin_login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {

        }
    }
    protected void login(object sender, EventArgs e)
    {
        string account = Admin_Account.Text;
        string password = Admin_Password.Text;
        // 認證使用者
        if (ValidateUser(account, password))
        {
            // 登入成功，將使用者資訊存入 Session 中
            Session["User"] = account;
            ScriptManager.RegisterStartupScript(Page, GetType(), "showAlert1", "showAlert1()", true);
            // 轉到其他頁面(其他方法)
            // Response.Redirect("xx.aspx"); 
        }
        else
        {
            // 登入失敗，顯示錯誤訊息
            ScriptManager.RegisterStartupScript(Page, GetType(), "showAlert2", "showAlert2()", true);
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
            //        "swal('登入失敗!', '請重新輸入帳號密碼!', 'error')", true);

        }
    }

    private bool ValidateUser(string account, string password)
    {
        if (account == "123" && password == "123")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}