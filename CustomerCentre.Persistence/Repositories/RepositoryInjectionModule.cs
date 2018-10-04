using Microsoft.Extensions.DependencyInjection;

namespace CustomerCentre.Persistence.Repositories
{
    public static class RepositoryInjectionModule
    {
        /// <summary>
        ///  Dependency inject DbContextFactory and CustomerRepository
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddScoped<IDbContextFactory, DbContextFactory>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            return services;
        }
    }
}
