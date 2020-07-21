using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace GrInfra
{
    public class Global : HttpApplication
    {
        //protected void Application_PreSendRequestHeaders()
        //{
        //    Response.Headers.Remove("X-Frame-Options");
        //    Response.AddHeader("X-Frame-Options", "AllowAll");

        //}
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

        //protected void Application_BeginRequest()
        //{
        //    if (FormsAuthentication.RequireSSL && !Request.IsSecureConnection)
        //    {
        //        Response.Redirect(Request.Url.AbsoluteUri.Replace("https://", "http://"));
        //    }
        //}
        protected void Application_BeginRequest(object sender, EventArgs e)
        {

            CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss";
            newCulture.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = newCulture;
        }
        public class SessionExpireAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                HttpContext ctx = HttpContext.Current;
                // check  sessions here
                if (HttpContext.Current.Session["LoginId"] == null)
                {
                    filterContext.Result = new RedirectResult("~/LoginPortal/Login");
                    return;
                }
                base.OnActionExecuting(filterContext);
            }
        }
    }
}
