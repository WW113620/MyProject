using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MongoDBTest
{
    class Program
    {
        static MongoDBHelper db;

        static void Main(string[] args)
        {
            //创建Mongodb的数据库实例
            db = new MongoDBHelper();

            #region 1000W条数据的初始化
            InitData();
            #endregion

            Console.WriteLine("Mongodb 中自己的Skip-Limit分页与自定义的Where-Limit分页效率测试（毫秒）：");
            //各种分页 尺寸的测试 具体注释我也不写了 
            PagerTest(1, 100);//这个测试忽略，估计第一次查询之后会相应的缓存下数据  导致之后的查询很快
            PagerTest(3, 100);
            PagerTest(30, 100);
            PagerTest(300, 100);
            //PagerTest(300, 1000);
            //PagerTest(3000, 100);
            //PagerTest(30000, 100);
            //PagerTest(300000, 100);
            
            Console.ReadKey();


        }

        /// <summary>
        /// 分页的测试
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页尺寸</param>
        static void PagerTest(int pageIndex,int pageSize)
        {
            //分页查询条件空（封装中会转恒真条件） 排序条件空（转为ObjectId递增） 设定页码 也尺寸
            
            Console.WriteLine("页码{0},页尺寸{1}", pageIndex, pageSize);
            Stopwatch sw1 = new Stopwatch();
            sw1.Start();
            List<LogInfo> list1 = db.Find<LogInfo>(null, pageIndex, pageSize, null);
            sw1.Stop();
            Console.WriteLine("Skip-Limit方式分页耗时：{0}", sw1.ElapsedMilliseconds);
            Stopwatch sw2 = new Stopwatch();
            sw2.Start();
            //这里以Logid索引为标志 如果集合里面没有这些主键标志的话 完全可以使用自己的ObjectId来做 帮助类里面也是封装好的
            //根据页码计算的LogId也只是简单的模拟 实际中这些LogId不一定会连续 这种方式分页一般不是传页码 而是传最后一个标志的值
            List<LogInfo> list2 = db.Find<LogInfo>(null, "LogId", (pageIndex - 1) * pageSize, pageSize, 1);
            sw2.Stop();
            Console.WriteLine("Where-Limit方式分页耗时：{0}\r\n", sw2.ElapsedMilliseconds);
        }

        /// <summary>
        /// 初始化一下数据
        /// </summary>
        static void InitData()
        {
            //创建 测试日志类的索引 索引的配置在LogInfo类的特性中
            db.CreateIndex<LogInfo>();

            //初始化日志的集合
            List<LogInfo> list = new List<LogInfo>();
            int temp = 0;

            //插入1000W条 测试的数据
            for (int i = 1; i <= 1000; i++)
            {
                list.Add(new LogInfo
                {
                    LogId = i,
                    Content = "content" + i.ToString(),
                    CreateTime = DateTime.Now
                });

                //temp计数  并作大于100的判断
                if (++temp >= 100)
                {
                    //大于等于100就清零
                    temp = 0;
                    //用封装好的方法批量插入数据
                    db.Insert<LogInfo>(list);
                    //插入数据之后将当前数据清空掉
                    list.Clear();
                }
            }
        }
    }
}
