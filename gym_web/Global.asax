﻿<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // 應用程式啟動時執行的程式碼
        System.Web.Http.GlobalConfiguration.Configure(gym_web.WebAPIConfig.Register);
        gym_web.RouteConfig.RegisterRoutes(System.Web.Routing.RouteTable.Routes);

    }
    void Application_BeginRequest(object sender, EventArgs e)
    {
        // 檢查是否是根路徑
        if (HttpContext.Current.Request.Url.AbsolutePath == "/")
        {
            // 重定向到 /page/home
            HttpContext.Current.Response.Redirect("/page/Home");
        }
    }
    void Application_End(object sender, EventArgs e) 
    {
        //  應用程式啟動時執行的程式碼

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // 應用程式啟動時執行的程式碼

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // 在新的工作階段啟動時執行的程式碼

    }

    void Session_End(object sender, EventArgs e) 
    {
        // 在工作階段結束時執行的程式碼
        // 注意: 只有在  Web.config 檔案中將 sessionstate 模式設定為 InProc 時，
        // 才會引起 Session_End 事件。如果將 session 模式設定為 StateServer 
        // 或 SQLServer，則不會引起該事件。

    }
       
</script>
