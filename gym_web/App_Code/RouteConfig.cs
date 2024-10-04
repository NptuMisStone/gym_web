using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace gym_web
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode = RedirectMode.Off;//宇哲RedirectMode.Permanent修改為RedirectMode.Off，有關ajax
            routes.EnableFriendlyUrls(settings);
        }
    }
}
