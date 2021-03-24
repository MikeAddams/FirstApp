using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.DataContext
{
    public class RepoDbContext : DbContext
    {
        public static OptionsBuild options = new OptionsBuild();

        public class OptionsBuild
        {
            public DbContextOptionsBuilder<RepoDbContext> optionsBuilder { get; set; }
            public DbContextOptions<RepoDbContext> dbOptions { get; set; }
            private AppConfiguration settings { get; set; }

            public OptionsBuild()
            {
                settings = new AppConfiguration();
                optionsBuilder = new DbContextOptionsBuilder<RepoDbContext>();

                optionsBuilder.UseSqlServer(settings.sqlConnection);
                dbOptions = optionsBuilder.Options;
            }
        }

        public RepoDbContext(DbContextOptions<RepoDbContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}
