using Models.Common;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB02
{
    class Program
    {
        static void Main(string[] args)
        {

            Remove();
            long counts = MongoDbHepler.GetDataCount("UserInfo");

           
            Console.ReadKey();
        }

        static void Remove()
        {
            IMongoQuery query = null;
            query = Query.And(Query.EQ("Name", "wang"));
            var bo = MongoDbHepler.Remove<UserInfo>(query);
        }

        //查询
        static void Search()
        {
            List<UserInfo> list = MongoDbHepler.FindAll<UserInfo>();
            var one = MongoDbHepler.FindOne<UserInfo>(null);

            IMongoQuery query = null;
            query = Query.And(
                Query.EQ("Name", "wang"),
                Query.EQ("ID", 1)
                );
            var one1 = MongoDbHepler.FindOne<UserInfo>(query);

            SortByDocument sortBy = new SortByDocument();
            sortBy.Add("ID", 1);//-1=DESC 1=ASC
            var list2 = MongoDbHepler.Find<UserInfo>(null, 0, 10, sortBy);
        
        }
        //更新
        static void UpdateDB()
        {
            IMongoQuery query = null;
            query = Query.And( Query.EQ("ID", 1));
            var u1 = Update<UserInfo>.Set<String>(t => t.Name, "北京");//更新一个字段
            var bo = MongoDbHepler.Update<UserInfo>(query, u1);


            var updateValues = new List<UpdateBuilder>();//更新多个字段
            updateValues.Add(Update.Set("Name", "test"));
            updateValues.Add(Update.Set("Password", "123456"));
            IMongoUpdate update = Update.Combine(updateValues);
            var bo2 = MongoDbHepler.Update<UserInfo>(query, update);
        }
        //插入数据
       static void Insert()
        {
            var user1 = new UserInfo { ID = 1, UserGuid = Guid.NewGuid().ToString("N"), Name = "wang123", DataTime=DateTime.Now };
            var user2 = new UserInfo { ID = 2, UserGuid = Guid.NewGuid().ToString("N"), Name = "test123", DataTime = DateTime.Now };
            var user3 = new UserInfo { ID = 3, UserGuid = Guid.NewGuid().ToString("N"), Name = "admin123", DataTime = DateTime.Now };
            var list = new List<UserInfo> { user1,user2,user3};
            MongoDbHepler.Insert<UserInfo>(user1);
            MongoDbHepler.Insert<UserInfo>(user2);
            MongoDbHepler.Insert<UserInfo>(user3);
            MongoDbHepler.InsertMore<UserInfo>(list);
          
        }


    }  
}
