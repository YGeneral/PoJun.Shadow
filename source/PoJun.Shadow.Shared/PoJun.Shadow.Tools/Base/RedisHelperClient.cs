using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoJun.Shadow.Tools
{
    /// <summary>
    /// Redis客户端工具类
    /// </summary>
    public static class RedisHelperClient
    {
        private static ConnectionMultiplexer Redis;
        private static IDatabase db;

        static RedisHelperClient()
        {
            if (RedisConfig.GetInstance().RedisSwitch)
            {
                Redis = ConnectionMultiplexer.Connect(RedisConfig.GetInstance().RedisConnStr);
                db = Redis.GetDatabase(int.Parse(RedisConfig.GetInstance().RedisDB));
            }
        }

        /// <summary>
        /// 判断key是否存储
        /// </summary>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public static async Task<bool> KeyExistsAsync(string key)
        {
            key = AddSysCustomKey(key);
            return await db.KeyExistsAsync(key);
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static async Task<bool> StringSetAsync<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?))
        {
            key = AddSysCustomKey(key);
            string json = ConvertJson(obj);
            return await db.StringSetAsync(key, json, expiry);
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<string> StringGetAsync(string key)
        {
            key = AddSysCustomKey(key);
            var result = await db.StringGetAsync(key);
            return result;
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> StringGetAsync<T>(string key)
        {
            key = AddSysCustomKey(key);
            var result = await db.StringGetAsync(key);
            if (result.IsNull)
                return default(T);

            return ConvertObj<T>(result);
        }

        /// <summary>
        /// 构造缓存key
        /// </summary>
        /// <param name="oldKey"></param>
        /// <returns></returns>
        private static string AddSysCustomKey(string oldKey)
        {
            return RedisConfig.GetInstance().SysCustomKey + oldKey;
        }

        /// <summary>
        /// 转json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string ConvertJson<T>(T value)
        {
            string result = value is string ? value.ToString() : JsonConvert.SerializeObject(value);
            return result;
        }

        /// <summary>
        /// 转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private static T ConvertObj<T>(RedisValue value)
        {
            if (value.IsNull)
                return default(T);

            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
