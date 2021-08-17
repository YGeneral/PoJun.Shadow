using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PoJun.Shadow.WebApi
{
    /// <summary>
    /// 请求本地化配置
    /// 创建人：杨江军
    /// 创建时间：2021/3/30/星期二 14:45:20
    /// </summary>
    public static class RequestLocalizationMilddleware
    {
		/// <summary>
		/// 请求本地化配置
		/// </summary>
		/// <param name="app"></param>
		public static void UseRequestLocalizationMilddleware(this IApplicationBuilder app)
		{
			//解决docker中时间类型ToString后变为3/12/2020 10:16:00格式的问题
			var zh = new CultureInfo("zh-CN");
			zh.DateTimeFormat.FullDateTimePattern = "yyyy-MM-dd HH:mm:ss";
			zh.DateTimeFormat.LongDatePattern = "yyyy-MM-dd";
			zh.DateTimeFormat.LongTimePattern = "HH:mm:ss";
			zh.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
			zh.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
			var supportedCultures = new List<CultureInfo>
			{
			   zh,
			};
			app.UseRequestLocalization(new RequestLocalizationOptions
			{
				//这里指定默认语言包
				DefaultRequestCulture = new RequestCulture("zh-CN"),
				SupportedCultures = supportedCultures,
				SupportedUICultures = supportedCultures
			});
		}
	}
}
