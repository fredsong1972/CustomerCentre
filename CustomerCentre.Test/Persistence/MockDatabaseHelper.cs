using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CustomerCentre.Persistence.Models;

namespace CustomerCentre.Test.Persistence
{
    public static class MockDatabaseHelper
    {
        public static DbContextOptions<CustomerCentreDbContext> CreateNewContextOptions(string databaseName)
        {
            //Create a fresh service provider, and therefore a fresh
            // InMemory database instance
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider
            var builder = new DbContextOptionsBuilder<CustomerCentreDbContext>();
            builder.UseInMemoryDatabase(databaseName)
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
    }
}
