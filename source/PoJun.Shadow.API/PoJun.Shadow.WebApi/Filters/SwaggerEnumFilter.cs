using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PoJun.Shadow.WebApi.Filters
{
    /// <summary>
    /// Add enum value descriptions to Swagger
    /// 创建人：杨江军
    /// 创建时间：2020/3/27 19:35:23
    /// </summary>
    public class SwaggerEnumFilter : IDocumentFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(Microsoft.OpenApi.Models.OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            Dictionary<string, Type> dict = GetAllEnum();

            foreach (var item in swaggerDoc.Components.Schemas)
            {
                var property = item.Value;
                var typeName = item.Key;
                Type itemType = null;
                if (property.Enum != null && property.Enum.Count > 0)
                {
                    if (dict.ContainsKey(typeName))
                    {
                        itemType = dict[typeName];
                    }
                    else
                    {
                        itemType = null;
                    }
                    List<OpenApiInteger> list = new List<OpenApiInteger>();
                    foreach (var val in property.Enum)
                    {
                        list.Add((OpenApiInteger)val);
                    }
                    property.Description += DescribeEnum(itemType, list);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, Type> GetAllEnum()
        {
            Dictionary<string, Type> dict = new Dictionary<string, Type>();

            GetAllEnum(dict, Assembly.Load("PoJun.Shadow.Enum"));
            GetAllEnum(dict, Assembly.Load("PoJun.Shadow.Code"));

            return dict;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="ass"></param>
        private static void GetAllEnum(Dictionary<string, Type> dict, Assembly ass)
        {
            Type[] types = ass.GetTypes();
            foreach (Type item in types)
            {
                if (item.IsEnum)
                {
                    dict.Add(item.Name, item);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="enums"></param>
        /// <returns></returns>
        private static string DescribeEnum(Type type, List<OpenApiInteger> enums)
        {
            var enumDescriptions = new List<string>();
            foreach (var item in enums)
            {
                if (type == null) continue;
                var value = System.Enum.Parse(type, item.Value.ToString());
                var desc = GetDescription(type, value);

                if (string.IsNullOrEmpty(desc))
                    enumDescriptions.Add($"{item.Value.ToString()}:{System.Enum.GetName(type, value)}; ");
                else
                    enumDescriptions.Add($"{item.Value.ToString()}:{desc}; ");

            }
            return $"<br/>{Environment.NewLine}{string.Join("<br/>" + Environment.NewLine, enumDescriptions)}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string GetDescription(Type t, object value)
        {
            foreach (MemberInfo mInfo in t.GetMembers())
            {
                if (mInfo.Name == t.GetEnumName(value))
                {
                    foreach (Attribute attr in Attribute.GetCustomAttributes(mInfo))
                    {
                        if (attr.GetType() == typeof(DescriptionAttribute))
                        {
                            return ((DescriptionAttribute)attr).Description;
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
