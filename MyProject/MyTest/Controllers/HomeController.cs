using Application.Common;
using Application.Redis;
using log4net;
using Models.Common;
using MyTest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceStack.Redis;
using ServiceStack.Redis.Support;
using System.Drawing;

namespace MyTest.Controllers
{
    public class HomeController : Controller
    {
        private ILog logger = LogManager.GetLogger(typeof(HomeController));
        private static ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            try
            {

                Stopwatch sw = new Stopwatch();
                sw.Start();

                var UserId = LoginHelper.UserId;
                var list = db.User.ToList();

                sw.Stop();
                long secondes = sw.ElapsedMilliseconds;
                logger.InfoFormat("具体期刊页面当前期刊'目录'检索花费时间:{0}", secondes);
            }
            catch (Exception e)
            {
                logger.InfoFormat("日志记录错误内容:{0}", e.Message);
            }                       
            return View();
        }


        #region Redis
        public ActionResult Redis()
        {
            try
            {
                //获取Redis操作接口
                IRedisClient Redis = RedisManager.GetClient();
                //Hash表操作
                HashOperator operators = new HashOperator();

                //移除某个缓存数据
                bool isTrue = Redis.Remove("additemtolist");
                //将字符串列表添加到redis
                List<string> storeMembers = new List<string>() { "韩梅梅", "李雷", "露西" };
                storeMembers.ForEach(x => Redis.AddItemToList("additemtolist", x));

                //得到指定的key所对应的value集合
                var members = Redis.GetAllItemsFromList("additemtolist");
                List<string> strList = new List<string>();
                members.ForEach(s => strList.Add(s));

                // 获取指定索引位置数据
                var item = Redis.GetItemFromList("additemtolist", 1);

                //将数据存入Hash表中
                UserInfo userInfos = new UserInfo() { Name = "李雷", Password = "123" };
                var ser = new ObjectSerializer();    //位于namespace ServiceStack.Redis.Support;
                bool results = operators.Set<byte[]>("userInfosHash", "userInfos", ser.Serialize(userInfos));
                byte[] infos = operators.Get<byte[]>("userInfosHash", "userInfos");
                userInfos = ser.Deserialize(infos) as UserInfo;
                string strInfo = string.Format("name={0},Password={1}",userInfos.Name,userInfos.Password);

                //object序列化方式存储
                UserInfo uInfo = new UserInfo() { Name = "张三", Password = "12345" };
                bool result = Redis.Set<byte[]>("uInfo", ser.Serialize(uInfo));
                UserInfo userinfo2 = ser.Deserialize(Redis.Get<byte[]>("uInfo")) as UserInfo;
                string strInfo2 = string.Format("name={0},Password={1}", userinfo2.Name, userinfo2.Password);

                //存储值类型数据
                Redis.Set<int>("my_age", 12);//或Redis.Set("my_age", 12);
                int age = Redis.Get<int>("my_age");

                //序列化列表数据
                Console.WriteLine("列表数据:");
                List<UserInfo> userinfoList = new List<UserInfo> {
                new UserInfo{Name="露西",Password="123",ID=1},
                new UserInfo{Name="玛丽",Password="456",ID=2},
                };
                Redis.Set<byte[]>("userinfolist_serialize", ser.Serialize(userinfoList));
                List<UserInfo> userList = ser.Deserialize(Redis.Get<byte[]>("userinfolist_serialize")) as List<UserInfo>;
                var list3 = userList.Select(p => new UserInfo { ID = p.ID, Name = p.Name, Password = p.Password });
                //释放内存
                Redis.Dispose();
                operators.Dispose();

            }
            catch (Exception e)
            {

            }
            return View();
        }
        #endregion

        #region 生成条形码
        public ActionResult TiaoXing()
        {
            return View();
        }

        public ActionResult CreateBarCode(string code)
        {
            JXUtil.BarCode barcode = new JXUtil.BarCode();
            barcode.Text = code;
            barcode.Height = 80;
            barcode.Magnify = 1;
            barcode.ViewFont = new Font("宋体", 20);
            System.Drawing.Image codeImage = barcode.GetCodeImage(JXUtil.BarCode.Code39Model.Code39Normal, true);

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            codeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            Response.ContentType = "image/jpeg";
            Response.Clear();
            Response.BinaryWrite(ms.ToArray());

            return new EmptyResult();
        } 
        #endregion


    }
}
