using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Repositories.DataContext;

namespace Repositories.DataContext
{
    public class RepoDbContextFactory : IDesignTimeDbContextFactory<RepoDbContext>
    {
        public RepoDbContext CreateDbContext(string[] args)
        {
            var appConfig = new AppConfiguration();
            var optionsBuilder = new DbContextOptionsBuilder<RepoDbContext>();

            optionsBuilder.UseSqlServer(appConfig.sqlConnection);

            return new RepoDbContext(optionsBuilder.Options);
        }
    }
}
