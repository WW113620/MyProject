using Models.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MyTest.Models
{
    /// <summary>
    /// Install-Package EntityFramework -Version 5.0.0.0
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["strConn"].ToString();
        public ApplicationDbContext()
            : base(connectionString)
        {
            //获取导航熟悉延时加载数据
            //this.Configuration.ProxyCreationEnabled = false; 
            //this.Configuration.LazyLoadingEnabled = true; //默认就是true
        }
        public DbSet<UserInfo> User { get; set; }
        public DbSet<AppointMent> AppointMent { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //按model的名字生成表，而不是复数形式
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        //程序初始化模型改变重新创建数据库
        public class BlogContextInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
        {
            protected override void Seed(ApplicationDbContext context)
            {
                base.Seed(context);
            }
        }


    }
}