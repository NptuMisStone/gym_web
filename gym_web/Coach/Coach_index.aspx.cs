using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Coach_Coach_home1 : System.Web.UI.Page
{
    public static string Coach_id;
    protected void Page_Load(object sender, EventArgs e)
    {
        Coach_id = Convert.ToString(Session["Coach_id"]);

        //驗證教練是否登入的類別函數
        CoachHelper.CheckLogin(this);
    }
}