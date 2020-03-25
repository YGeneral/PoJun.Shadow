using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PoJun.Shadow.Tools;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Common.Hosting;

namespace PoJun.Shadow.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .UseCloudHosting(APIConfig.GetInstance().Port)//部署在linux上才生效，接口的端口配置
               .UseStartup<Startup>()
               .UseUrls($"http://0.0.0.0:{APIConfig.GetInstance().Port}")//部署在linux上才生效，接口的端口配置
               .Build();
        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        //.UseCloudHosting(APIConfig.GetInstance().Port)//部署在linux上才生效，接口的端口配置
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });
    }
}
