using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
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
        }
        else if (Session["coach_id"] != null)
        {
            panel_coachname.Visible = true;
            panel_logout.Visible = true;
            panel_login.Visible = false;
            panel_register.Visible = false;
            Label_coachname.Text = userManager.GetCoachName();
        }
        //頁首name-end
    }
    protected void Btn_logout_Click(object sender, EventArgs e)
    {
        Session["user_id"] = null;
        Session["coach_id"] = null;
        panel_username.Visible = false;
        panel_coachname.Visible = false;
        panel_logout.Visible = false;
        panel_login.Visible = true;
        panel_register.Visible = true;
        Response.Redirect("/page/home.aspx");
    }
}
