using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class page_MasterPage2 : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //頁首name-start
        UserManager userManager = new UserManager();

        if (Session["user_id"] != null)
        {
            panel_username.Visible = true;
            panel_logout.Visible = true;
            panel_login.Visible = false;
            panel_register.Visible = false;
            Label_username.Text = userManager.GetUserName();
            Main_LOGO.HRef = "/page/Home.aspx";
            navbarCollapse.Visible = true;
            URL.Visible = true;
            Join.Visible = true;
        }
        else if (Session["coach_id"] != null)
        {
            panel_coachname.Visible = true;
            panel_logout.Visible = true;
            panel_login.Visible = false;
            panel_register.Visible = false;
            Label_coachname.Text = userManager.GetCoachName();
            Main_LOGO.HRef = "/Coach/Coach_index.aspx";
            navbarCollapse.Visible = false;
            URL.Visible = false;
            Join.Visible = false;
        }
        else
        {
            Main_LOGO.HRef = "/page/Home.aspx";
            navbarCollapse.Visible = true;
            URL.Visible = true;
            Join.Visible = true;
        }
        //頁首name-end
    }
    protected void Btn_logout_Click(object sender, EventArgs e)
    {
        // 清空 Session
        Session["user_id"] = null;
        Session["coach_id"] = null;
        panel_username.Visible = false;
        panel_coachname.Visible = false;
        panel_logout.Visible = false;
        panel_login.Visible = true;
        panel_register.Visible = true;

        // 取得當前路徑
        string currentPath = Request.Url.AbsolutePath;

        // 如果當前路徑包含 "/User"，則導向首頁
        if (currentPath.Contains("/User"))
        {
            Response.Redirect("~/page/Home.aspx");  // 導向首頁
        }
        else
        {
            // 否則保持在當前頁面並重整
            Response.Redirect(Request.Url.ToString());
        }
    }

}
