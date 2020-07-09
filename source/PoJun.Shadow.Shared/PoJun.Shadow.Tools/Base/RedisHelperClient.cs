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
    public sealed class RedisHelperClient
    {
        #region 初始化

        private static ConnectionMultiplexer Redis;
        private static IDatabase db;

        /// <summary>
        /// 定义一个静态变量来保存类的实例
        /// </summary>
        private static RedisHelperClient uniqueInstance;

        /// <summary>
        /// 定义一个标识确保线程同步
        /// </summary>
        private static readonly object locker = new object();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public static RedisHelperClient GetInstance()
        {
            if (uniqueInstance == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (uniqueInstance == null)
                    {
                        uniqueInstance = new RedisHelperClient();
                        Redis = ConnectionMultiplexer.Connect(RedisConfig.GetInstance().RedisExchangeHosts);
                        db = Redis.GetDatabase(int.Parse(RedisConfig.GetInstance().RedisDB));
                    }
                }
            }
            return uniqueInstance;
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        private RedisHelperClient()
        {

        }

        #endregion

        #region 判断key是否存储

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

        #endregion

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static async Task<bool> SetAsync<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?))
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
        public static async Task<string> GetAsync(string key)
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
        public static async Task<T> GetAsync<T>(string key)
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
            return $"{RedisConfig.GetInstance().RedisKey}-{oldKey}";
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
