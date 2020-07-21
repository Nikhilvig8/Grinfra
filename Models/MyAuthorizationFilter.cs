using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrInfra.Models
{
    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            bool status = true;
            //string role = "";

            //if (HttpContext.Current.Session["Role"] != null)
            //{
            //    role = HttpContext.Current.Session["Role"].ToString();
            //}

            // In case you need an OWIN context, use the next line, `OwinContext` class
            // is the part of the `Microsoft.Owin` package.
            //var owinContext = new OwinContext(context.GetOwinEnvironment());

            // Allow all authenticated users to see the Dashboard (potentially dangerous).

            //if (role == "RL01" || role == "RL02")
            //{
            //    status = true;
            //}

            return status;
        }
    }
}