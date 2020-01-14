using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whiteboard.Isolated.DataAccess
{
    internal class CanvasDbInitializer : SqliteCreateDatabaseIfNotExists<CanvasDbContext>
    {
        public CanvasDbInitializer(DbModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        public CanvasDbInitializer(DbModelBuilder modelBuilder, bool nullByteFileMeansNotExisting) : base(modelBuilder, nullByteFileMeansNotExisting)
        {
        }

        public override void InitializeDatabase(CanvasDbContext context)
        {
            base.InitializeDatabase(context);
        }

        protected override void Seed(CanvasDbContext context)
        {
            //context.Set<Customer>().Add(new Customer { UserName = "user" + DateTime.Now.Ticks.ToString(), Roles = new List<Role> { new Role { RoleName = "user" } } });
            //context.Set<Post>().Add(new Post { Category = new Category() });

            base.Seed(context);
        }
    }
}
