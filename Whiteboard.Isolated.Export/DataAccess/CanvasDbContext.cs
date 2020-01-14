using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite.EF6;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whiteboard.Isolated.Model;

namespace Whiteboard.Isolated.DataAccess
{
    /// <summary>
    /// 表示用户数据库的上下文
    /// </summary>
    public class CanvasDbContext : DbContext
    {
        /// <summary>
        /// 保存连接字符串
        /// </summary>
        private string _connectionString = string.Empty;


        /// <summary>
        /// 白板页夹
        /// </summary>
        public DbSet<DrawingBinder> DrawingBinders { get; set; }

        /// <summary>
        /// 白板页
        /// </summary>
        public DbSet<DrawingPage> DrawingPages { get; set; }

        /// <summary>
        /// 构造DbContext
        /// </summary>
        /// <param name="teacherBusinessId"></param>
        public CanvasDbContext(string teacherBusinessId) : base(CreateDbConnection(teacherBusinessId), false)
        {
            //_connectionString = CreateDbConnection(teacherBusinessId).ConnectionString;

            //Console.WriteLine("==================");
            //Console.WriteLine(_connectionString);
            //Console.WriteLine("==================");

        }

        /// <summary>
        /// 重写建模
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DrawingBinder>();
            modelBuilder.Entity<DrawingPage>();


            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.AddFromAssembly(typeof(CanvasDbContext).Assembly);

            Database.SetInitializer(new CanvasDbInitializer(modelBuilder));
                     
            //var init = new SqliteCreateDatabaseIfNotExists<GlobalDbContext>(modelBuilder);
            //Database.SetInitializer(init);
        }


        /// <summary>
        /// 动态创建连接
        /// </summary>
        /// <param name="teacherBusinessId"></param>
        /// <returns></returns>
        public static DbConnection CreateDbConnection(string teacherBusinessId)
        {
            string teacherDbFilePath = GetTeacherDbFilePathByBusinessId(teacherBusinessId);

            DbConnection connection = SQLiteProviderFactory.Instance.CreateConnection();
            connection.ConnectionString = $"Data Source={teacherDbFilePath}";

            return connection;
        }

        /// <summary>
        /// 根据指定的教师Id, 给出相应的数据文件夹路径
        /// </summary>
        /// <param name="teacherBusinessId"></param>
        /// <returns></returns>
        private static string GetTeacherDbFilePathByBusinessId(string teacherBusinessId)
        {
            string teacherDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserCache", teacherBusinessId);
            if (!Directory.Exists(teacherDir))
            {
                Directory.CreateDirectory(teacherDir);
            }

            string dbPath = Misc.GetUserDbName();
            return Path.Combine(
                teacherDir,
                dbPath);

        }
    }
    

}
