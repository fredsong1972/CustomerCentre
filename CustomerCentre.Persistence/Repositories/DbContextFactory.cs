using CustomerCentre.Persistence.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using CustomerCentre.Persistence.Models;
using System;

namespace CustomerCentre.Persistence.Repositories
{
    public class DbContextFactory : IDbContextFactory, IDisposable
    {
        /// <summary>
        /// Create Db context with connection string
        /// </summary>
        /// <param name="settings"></param>
        public DbContextFactory(IOptions<DbContextSettings> settings) 
        {
            var options = new DbContextOptionsBuilder<CustomerCentreDbContext>().UseSqlServer(settings.Value.CustomerCentreConnectionString).Options;
            DbContext = new CustomerCentreDbContext(options);
        }

        ~DbContextFactory()
        {
            Dispose();
        }

        public CustomerCentreDbContext DbContext { get; private set; }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}
