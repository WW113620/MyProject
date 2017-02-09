using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDBTest
{
    /// <summary>
    /// MongoDB的简单配置
    /// </summary>
    public class MongoDB
    {
        /// <summary>
        /// 数据库所在主机
        /// </summary>
        private readonly string MONGO_CONN_HOST = "127.0.0.1";

        /// <summary>
        /// 数据库所在主机的端口
        /// </summary>
        private readonly int MONGO_CONN_PORT = 27017;

        /// <summary>
        /// 连接超时设置 秒
        /// </summary>
        private readonly int CONNECT_TIME_OUT = 30;

        /// <summary>
        /// 数据库的名称
        /// </summary>
        private readonly string DB_NAME = "test";

        /// <summary>
        /// 得到数据库实例
        /// </summary>
        /// <returns></returns>
        public MongoDatabase GetDataBase()
        {
            MongoClientSettings mongoSetting = new MongoClientSettings();
            //设置连接超时时间
            mongoSetting.ConnectTimeout =new TimeSpan(CONNECT_TIME_OUT*TimeSpan.TicksPerSecond);
            //设置数据库服务器
            mongoSetting.Server = new MongoServerAddress(MONGO_CONN_HOST, MONGO_CONN_PORT);
            //创建Mongo的客户端
            MongoClient client = new MongoClient(mongoSetting);
            //得到服务器端并且生成数据库实例
            return client.GetServer().GetDatabase(DB_NAME);
        }
    }
}
