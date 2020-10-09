using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

namespace ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            //CreateWebHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(c=>
            {
                c.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
            }
            )
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    //webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                    //{
                    //    config
                    //        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                    //        .AddJsonFile("appsettings.json", true, true)
                    //        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)//引入与环境有关配置
                    //        .AddOcelot(hostingContext.HostingEnvironment)//调用Ocelot封装的文件合并
                    //        .AddEnvironmentVariables();
                    //});
                });


        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseUrls("http://*:9000")
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config
                .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                .AddJsonFile("ocelot.json")
                .AddEnvironmentVariables();
            })
            .ConfigureServices(services =>
            {
                services.AddOcelot().AddConsul();
            })
            .Configure(app =>
            {
                app.UseOcelot().Wait();
            });
          
    }
}
