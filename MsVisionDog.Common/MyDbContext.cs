using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsVisionDog.Common
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<MyEntity> MyEntities { get; set; }
        public DbSet<MyImage> MyImages { get; set; }
        public DbSet<MyEntityImage> MyEntityImages { get; set; }
    }
}
