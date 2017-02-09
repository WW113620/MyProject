using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDBTest
{
    /// <summary>
    /// 测试表格的一个实体类
    /// </summary>
    public class LogInfo
    {
        public ObjectId _id;

        /// <summary>
        /// 日志Id  这里的特性 可以在帮助类中识别出
        /// </summary>
        [MongoDBFieldAttribute(true, Unique = true)]
        public int LogId { get; set; }

        /// <summary>
        /// 日志的内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 日志的创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
