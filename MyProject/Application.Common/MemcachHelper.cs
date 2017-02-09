using Memcached.ClientLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Application.Common
{
    public class MemcachHelper
    {
        /// <summary>
        /// Install-Package Memcached.ClientLibrary
        /// </summary>
        private static MemcachedClient mclient;
        static MemcachHelper()
        {          
            ArrayList serverlist=new ArrayList();
            serverlist.Add("127.0.0.1:11211");
            mclient=CreateServer(serverlist, "First");
        }

        #region 创建Memcache服务
        /// <summary>
        /// 创建Memcache服务
        /// </summary>
        /// <param name="serverlist">IP端口列表</param>
        /// <param name="poolName">Socket连接池名称</param>
        /// <returns>Memcache客户端代理类</returns>
        private static MemcachedClient CreateServer(ArrayList serverlist, string poolName)
        {
            //初始化memcache服务器池
            SockIOPool pool = SockIOPool.GetInstance(poolName);
            //设置Memcache池连接点服务器端。
            pool.SetServers(serverlist);
            pool.Initialize();
            mclient = new MemcachedClient();
            mclient.PoolName = poolName;
            mclient.EnableCompression = false;//是否压缩 
            return mclient;
        } 
        #endregion

        #region Memcache操作
        /// <summary>
        /// 缓存是否存在
        /// </summary>
        /// <returns></returns>
        public static bool CacheIsExists(string key)
        {
            if (mclient.KeyExists(key))
                return true;
            else
                return false;
        }
        /// <summary>
        /// 添加缓存(键不存在则添加,键存在则不能添加)
        /// </summary>
        /// <param name="expiry">DateTime.Now.AddMinutes(10)</param>
        /// <returns></returns>
        public static bool Add(string key, string value, DateTime expiry)
        {
            return mclient.Add(key, value, expiry);
        }
        /// <summary>
        /// 替换缓存(键存在的才能替换,不存在则不替换)
        /// </summary>
        /// <param name="minutes">6</param>
        /// <returns></returns>
        public static bool ReplaceCache(string key, string value, int minutes)
        {
            return mclient.Replace(key, value, DateTime.Now.AddMinutes(minutes));
        }
        /// <summary>
        /// 添加缓存(键不存在则添加,键存在则覆盖)
        /// </summary>
        /// <param name="expiry">DateTime.Now.AddDays(2)</param>
        /// <returns></returns>
        public static bool Set(string key, object value, DateTime expiry)
        {
            return mclient.Set(key, value, expiry);
        }
        /// <summary>
        ///  获取单个键对应的缓存
        /// </summary>
        /// <returns></returns>
        public static object Get(string key)
        {
            return mclient.Get(key);
        }
        /// <summary>
        /// 获取键数组对应的值
        /// </summary>
        /// <returns></returns>
        public static Hashtable GetCacheHt(string[] keys)
        {
            return mclient.GetMultiple(keys);
        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <returns></returns>
        public static bool Delete(string key)
        {
            return mclient.Delete(key);
        }
        /// <summary>
        /// 清空缓存
        /// </summary>
        /// <returns></returns>
        public static bool FlushAll()
        {
            return mclient.FlushAll();
        } 
        #endregion

    }
}