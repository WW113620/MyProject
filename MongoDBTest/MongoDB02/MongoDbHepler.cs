using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB02
{
    public class MongoDbHepler
    {

        #region 获取数据库实例对象

        private static readonly string dbServerString = System.Configuration.ConfigurationManager.AppSettings["MongoDBServer"];
        private static readonly string dbNameString = System.Configuration.ConfigurationManager.AppSettings["MongoDBDatabase"];
        /// <summary>
        /// Version 1.9.2.235
        /// </summary>
        /// <returns></returns>
        private static MongoDatabase GetDatabase()
        {
            return GetDatabase(dbServerString, dbNameString);
        }
        /// <summary>
        /// 获取数据库实例对象
        /// </summary>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="dbName">数据库名称</param>
        /// <returns>数据库实例对象</returns>
        private static MongoDatabase GetDatabase(string connectionString, string dbName)
        {
            MongoClient _client = new MongoClient(connectionString);//创建mongoClient
            return _client.GetServer().GetDatabase(dbName);
        }

        public static MongoCollection<T> Collection<T>()
        {
            string collectionName = typeof(T).Name;
            MongoCollection<T> mc = GetDatabase().GetCollection<T>(collectionName);
            return mc;
        }
        public static MongoCollection<T> Collection<T>(string collectionName)
        {
            MongoCollection<T> mc = GetDatabase().GetCollection<T>(collectionName);
            return mc;
        }
        #endregion

        #region 插入数据
        public static void Insert<T>(T model)
        {
            if (model == null)
                return;
            var collection = Collection<T>();
            collection.Insert(model);
        }

        public static void InsertMore<T>(List<T> list)
        {
            if (list == null && list.Count < 0)
                return;
            var collection = Collection<T>();
            collection.InsertBatch(list);
        }

        #endregion

        #region 查询数据

        #region 简单查询
        public static List<T> FindAll<T>()
        {
            var collection = Collection<T>();
            return collection.FindAll().ToList();
        }

        public static T FindOne<T>(IMongoQuery query)
        {
            var collection = Collection<T>();
            query = InitQuery(query);
            T t = collection.FindOne(query);
            return t;
        }

        public static List<T> Find<T>(IMongoQuery query, string collectionName)
        {
            MongoCollection<T> mc = Collection<T>(collectionName);
            query = InitQuery(query);
            MongoCursor<T> mongoCursor = mc.Find(query);
            return mongoCursor.ToList<T>();
        }

        /// <summary>
        /// 根据指定条件查询集合中的数据
        /// </summary>
        /// <typeparam name="T">该集合数据的所属类型</typeparam>
        /// <param name="query">指定的查询条件 比如Query.And(Query.EQ("username","admin"),Query.EQ("password":"admin"))</param>
        /// <returns>返回一个List列表</returns>
        public static List<T> Find<T>(IMongoQuery query)
        {
            string collectionName = typeof(T).Name;
            return Find<T>(query, collectionName);
        }
        #endregion

        #region 分页查询 (当PageIndex很大时 效率明显变低)
        /// <summary>
        /// 分页查询 PageIndex和PageSize模式
        /// </summary>
        /// <param name="query">查询的条件</param>
        /// <param name="pageIndex">当前的页数</param>
        /// <param name="pageSize">当前的尺寸</param>
        /// <param name="sortBy">排序类型 1升序 -1降序(ortByDocument sortBy = new SortByDocument();sortBy.Add("ID", -1);)</param>
        /// <param name="collectionName">集合名称</param>
        /// <returns>返回List列表</returns>
        public static List<T> Find<T>(IMongoQuery query, int pageIndex, int pageSize, SortByDocument sortBy, string collectionName)
        {
            MongoCollection<T> mc = GetDatabase().GetCollection<T>(collectionName);
            MongoCursor<T> mongoCursor = null;
            query = InitQuery(query);
            sortBy = InitSortBy(sortBy);
            //如页序号为0时初始化为1
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            //按条件查询 排序 跳数 取数
            mongoCursor = mc.Find(query).SetSortOrder(sortBy).SetSkip((pageIndex - 1) * pageSize).SetLimit(pageSize);

            return mongoCursor.ToList<T>();
        }

        public static List<T> Find<T>(IMongoQuery query, int pageIndex, int pageSize, SortByDocument sortBy)
        {
            string collectionName = typeof(T).Name;
            return Find<T>(query, pageIndex, pageSize, sortBy, collectionName);
        }

        #endregion

        #region 分页查询 指定索引最后项-PageSize模式

        /// <summary>
        /// 分页查询 指定索引最后项-PageSize模式 
        /// </summary>
        /// <param name="query">查询的条件 没有可以为null</param>
        /// <param name="indexName">索引名称</param>
        /// <param name="lastKeyValue">最后索引的值</param>
        /// <param name="pageSize">分页的尺寸</param>
        /// <param name="sortType">排序类型 1升序 -1降序 仅仅针对该索引</param>
        /// <param name="collectionName">指定的集合名称</param>
        /// <returns>返回一个List列表数据</returns>
        public List<T> Find<T>(IMongoQuery query, string indexName, object lastKeyValue, int pageSize, int sortType, string collectionName)
        {
            MongoCollection<T> mc = GetDatabase().GetCollection<T>(collectionName);
            MongoCursor<T> mongoCursor = null;
            query = InitQuery(query);
            //判断升降序后进行查询
            if (sortType > 0)
            {
                //升序
                if (lastKeyValue != null)
                {
                    //有上一个主键的值传进来时才添加上一个主键的值的条件
                    query = Query.And(query, Query.GT(indexName, BsonValue.Create(lastKeyValue)));
                }
                //先按条件查询 再排序 再取数
                mongoCursor = mc.Find(query).SetSortOrder(new SortByDocument(indexName, 1)).SetLimit(pageSize);
            }
            else
            {
                //降序
                if (lastKeyValue != null)
                {
                    query = Query.And(query, Query.LT(indexName, BsonValue.Create(lastKeyValue)));
                }
                mongoCursor = mc.Find(query).SetSortOrder(new SortByDocument(indexName, -1)).SetLimit(pageSize);
            }
            return mongoCursor.ToList<T>();
        }

        public List<T> Find<T>(IMongoQuery query, string indexName, object lastKeyValue, int pageSize, int sortType)
        {
            string collectionName = typeof(T).Name;
            return this.Find<T>(query, indexName, lastKeyValue, pageSize, sortType, collectionName);
        }

        public List<T> Find<T>(IMongoQuery query, string lastObjectId, int pageSize, int sortType, string collectionName)
        {
            return this.Find<T>(query, OBJECTID_KEY, new ObjectId(lastObjectId), pageSize, sortType, collectionName);
        }

        public List<T> Find<T>(IMongoQuery query, string lastObjectId, int pageSize, int sortType)
        {
            string collectionName = typeof(T).Name;
            return Find<T>(query, lastObjectId, pageSize, sortType, collectionName);
        }

        #endregion

        #endregion

        #region 更新数据

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="update"> var up=Update<UserInfo>.Set<String>(t => t.Name, "北京");</param>
        public static bool Update<T>(IMongoQuery query, IMongoUpdate update, string collectionName)
        {
            MongoCollection<T> mc = Collection<T>(collectionName);
            query = InitQuery(query);
            WriteConcernResult result = mc.Update(query, update, UpdateFlags.Multi);
            return result.Ok;
        }

        public static bool Update<T>(IMongoQuery query, IMongoUpdate update)
        {
            string collectionName = typeof(T).Name;
            return Update<T>(query, update, collectionName);
        }
        #endregion

        #region 删除数据
        /// <summary>
        /// 移除指定的数据
        /// </summary>
        /// <param name="query">移除的数据条件</param>
        /// <param name="collectionName">指定的集合名词</param>
        public static bool Remove<T>(IMongoQuery query, string collectionName)
        {
            MongoCollection<T> mc = Collection<T>(collectionName);
            query = InitQuery(query);
            WriteConcernResult wc = mc.Remove(query);
            return wc.Ok;
        }

        public static bool Remove<T>(IMongoQuery query)
        {
            string collectionName = typeof(T).Name;
            return Remove<T>(query, collectionName);
        }

        public static bool RemoveAll<T>(string collectionName)
        {
            return Remove<T>(null, collectionName);
        }

        public static bool ReomveAll<T>()
        {
            string collectionName = typeof(T).Name;
            return Remove<T>(null, collectionName);
        }
        #endregion

        #region 创建索引
        /// <summary>
        /// 创建索引 
        /// </summary>
        /// <typeparam name="T">需要创建索引的实体类型</typeparam>
        public void CreateIndex<T>(MongoDbAttribute mongoField)
        {
            string collectionName = typeof(T).Name;
            MongoCollection<BsonDocument> mc = GetDatabase().GetCollection<BsonDocument>(collectionName);

            PropertyInfo[] propertys = typeof(T).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
            //得到该实体类型的属性
            foreach (PropertyInfo property in propertys)
            {
                //在各个属性中得到其特性
                foreach (object obj in property.GetCustomAttributes(true))
                {
                    if (mongoField != null)
                    {// 此特性为mongodb的字段属性

                        IndexKeysBuilder indexKey;
                        if (mongoField.Ascending)
                        {
                            //升序 索引
                            indexKey = IndexKeys.Ascending(property.Name);
                        }
                        else
                        {
                            //降序索引
                            indexKey = IndexKeys.Ascending(property.Name);
                        }
                        //创建该属性
                        mc.CreateIndex(indexKey, IndexOptions.SetUnique(mongoField.Unique));
                    }
                }
            }
        }
        #endregion

        #region 获取条目数
        public static long GetDataCount<T>()
        {
            string collectionName = typeof(T).Name;
            return GetDataCount(collectionName);
        }
        public static long GetDataCount(string collectionName)
        {

            MongoCollection<BsonDocument> mc = GetDatabase().GetCollection<BsonDocument>(collectionName);
            return mc.Count();
        }
        #endregion

        #region 获取集合的存储大小
        /// <summary>
        /// 获取集合的存储大小
        /// </summary>
        /// <typeparam name="T">该集合对应的实体类</typeparam>
        /// <returns>返回一个long型</returns>
        public static long GetDataSize<T>()
        {
            string collectionName = typeof(T).Name;
            return GetDataSize(collectionName);
        }

        /// <summary>
        /// 获取集合的存储大小
        /// </summary>
        /// <param name="collectionName">该集合对应的名称</param>
        /// <returns>返回一个long型</returns>
        public static long GetDataSize(string collectionName)
        {
            MongoCollection<BsonDocument> mc = GetDatabase().GetCollection<BsonDocument>(collectionName);
            return mc.GetTotalStorageSize();
        }
        #endregion

        #region 私有的一些辅助方法
        /// <summary>
        /// 初始化查询记录 主要当该查询条件为空时 会附加一个恒真的查询条件，防止空查询报错
        /// </summary>
        private static readonly string OBJECTID_KEY = "_id";
        private static IMongoQuery InitQuery(IMongoQuery query)
        {
            if (query == null)
            {
                //当查询为空时 附加恒真的条件 类似SQL：1=1的语法
                query = Query.Exists(OBJECTID_KEY);
            }
            return query;
        }

        /// <summary>
        /// 初始化排序条件  主要当条件为空时 会默认以ObjectId递增的一个排序
        /// </summary>
        private static SortByDocument InitSortBy(SortByDocument sortBy)
        {
            if (sortBy == null)
            {
                //默认ObjectId 递增
                sortBy = new SortByDocument(OBJECTID_KEY, 1);
            }
            return sortBy;
        }
        #endregion
    }
}
