using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyModel;
using PoJun.Shadow.BaseFramework;
using PoJun.Shadow.IFramework.Web;
using PoJun.Shadow.Tools;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace PoJun.Shadow.WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class AutofacModuleRegister : Autofac.Module
    {
        /// <summary>
        /// 重写Autofac管道Load方法，在这里注册注入
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            #region Quartz调度框架注册

            builder.RegisterType<QuartzStartup>().SingleInstance();
            builder.RegisterType<StdSchedulerFactory>().As<ISchedulerFactory>().SingleInstance();
            builder.RegisterType<IOCJobFactory>().As<IJobFactory>().SingleInstance();

            #endregion

            #region 自定义job注册

            //builder.RegisterType<TestJob>().SingleInstance();

            #endregion

            #region 注入HttpClientHelp

            builder.RegisterType<HttpClientHelp>().As<IHttpClientHelp>().SingleInstance();

            #endregion

            //注册Service中的对象,Service中的类要以Service结尾，否则注册失败
            builder.RegisterAssemblyTypes(GetAssemblyByName("PoJun.Shadow.Api.Service")).Where(a => a.Name.EndsWith("Service")).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(GetAssemblyByName("PoJun.Shadow.LogFramework")).Where(a => a.Name.EndsWith("Service")).AsImplementedInterfaces();
            //注册Repository中的对象,Repository中的类要以Repository结尾，否则注册失败
            builder.RegisterAssemblyTypes(GetAssemblyByName("PoJun.Shadow.Api.MongoDBRepository")).Where(a => a.Name.EndsWith("Repository")).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(GetAssemblyByName("PoJun.Shadow.Api.MySqlRepository")).Where(a => a.Name.EndsWith("Repository")).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(GetAssemblyByName("PoJun.Shadow.Api.SqlServerRepository")).Where(a => a.Name.EndsWith("Repository")).AsImplementedInterfaces();
        }

        #region 根据程序集名称获取程序集

        /// <summary>
        /// 根据程序集名称获取程序集
        /// </summary>
        /// <param name="AssemblyName">程序集名称</param>
        /// <returns></returns>
        public static Assembly GetAssemblyByName(String AssemblyName)
        {
            return Assembly.Load(AssemblyName);
        }

        #endregion

        #region 获取所有MongoDB项目程序集名称

        /// <summary>
        /// 获取所有MongoDB项目程序集名称
        /// </summary>
        /// <returns></returns>
        public static string[] GetAllAssembliesName()
        {
            var list = new List<string>();
            var deps = DependencyContext.Default;
            //排除所有的系统程序集(Microsoft.***、System.***等)、Nuget下载包
            var libs = deps.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package");
            foreach (var lib in libs)
            {
                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                    if (assembly.GetName().ToString().Contains("MongoDB"))
                        list.Add(assembly.GetName().ToString());
                }
                catch (System.Exception)
                {

                }
            }
            return list.ToArray();
        }

        #endregion
    }
}
