using System;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(GrInfra.Startup))]

namespace GrInfra
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration
                .UseSqlServerStorage("Hangfire_Blog");
            
          RecurringJob.AddOrUpdate("Employee", () => GrInfra.Controllers.SFTPDataController.EmployeeData(), "30 19 * * *");
            
            RecurringJob.AddOrUpdate("Hrnotification", () => GrInfra.Controllers.SFTPDataController.SendHrannoucement(),Cron.Daily);

                RecurringJob.AddOrUpdate("Roaster", () => GrInfra.Controllers.SFTPDataController.RoasterData(), "30 20 * * *");

            //   RecurringJob.AddOrUpdate("Harvir", () => GrInfra.Controllers.SFTPDataController.Main(), "30 19 * * *");
           //  RecurringJob.AddOrUpdate("Harvir", () => GrInfra.Controllers.SFTPDataController.Push(), "30 20 * * *");



            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new MyAuthorizationFilter() }
            });


            app.UseHangfireServer();
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        }
    }

    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {

            var role = "Admin";


            if (role == "Admin")
            {
                return true;
            }
            else
            {
                return false;
            }
            // Allow all authenticated users to see the Dashboard (potentially dangerous).
        }
    }
}
