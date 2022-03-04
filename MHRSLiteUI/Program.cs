using MHRSLiteUI.QuartzWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MHRSLiteUI
{
    public class Program
    {
        [Obsolete]
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        [Obsolete]
        public static IHostBuilder CreateHostBuilder(string[] args) =>
             //Host.CreateDefaultBuilder(args)
             //    .ConfigureWebHostDefaults(webBuilder =>
             //    {
             //        webBuilder.UseStartup<Startup>();
             //    });
             Host.CreateDefaultBuilder(args)
            //.ConfigureServices((hostContext, services) =>
            //{
            //    // Add the required Quartz.NET services
            //    services.AddQuartz(q =>
            //    {
            //        // Use a Scoped container to create jobs. I'll touch on this later
            //        q.UseMicrosoftDependencyInjectionScopedJobFactory();
            //    });

            //    // Add the Quartz.NET hosted service

            //    services.AddQuartzHostedService(
            //        q => q.WaitForJobsToComplete = true);

            //    // other config

            //})
            .ConfigureServices((hostContext, services) =>
            {
                services.AddQuartz(q =>
                {
                    q.UseMicrosoftDependencyInjectionScopedJobFactory();

                    // Register the job, loading the schedule from configuration
                    q.AddJobAndTrigger<AppointmentStatusJob>(hostContext.Configuration);
                    q.AddJobAndTrigger<RomatologyClaimJob>(hostContext.Configuration);
                    //q.AddJobAndTrigger<DenemeJob>(hostContext.Configuration);
                });

                services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
