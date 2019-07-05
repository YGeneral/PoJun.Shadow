using Autofac;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace PoJun.Shadow.WebApi
{
    public class AutofacModuleRegister : Autofac.Module
    {
        //重写Autofac管道Load方法，在这里注册注入
        protected override void Load(ContainerBuilder builder)
        {
            //注册Service中的对象,Service中的类要以Service结尾，否则注册失败
            builder.RegisterAssemblyTypes(GetAssemblyByName("PoJun.Shadow.Api.Service")).Where(a => a.Name.EndsWith("Service")).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(GetAssemblyByName("PoJun.Shadow.LogFramework")).Where(a => a.Name.EndsWith("Service")).AsImplementedInterfaces();
            //注册Repository中的对象,Repository中的类要以Repository结尾，否则注册失败
            builder.RegisterAssemblyTypes(GetAssemblyByName("PoJun.Shadow.Api.MongoDBRepository")).Where(a => a.Name.EndsWith("Repository")).AsImplementedInterfaces();
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
