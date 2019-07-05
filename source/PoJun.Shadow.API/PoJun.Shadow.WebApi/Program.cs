using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PoJun.Shadow.Tools;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace PoJun.Shadow.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateWebHostBuilder(args).Build().Run();
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .UseCloudFoundryHosting(APIConfig.GetInstance().Port)//部署在linux上才生效，接口的端口配置
               .UseStartup<Startup>()
               .UseUrls($"http://0.0.0.0:{APIConfig.GetInstance().Port}")//部署在linux上才生效，接口的端口配置
               .Build();
        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>();
    }
}
