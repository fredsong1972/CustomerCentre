using Microsoft.Extensions.DependencyInjection;

namespace CustomerCentre.Services
{
    public static class ServiceInjectionModule
    {
        /// <summary>
        /// Dependency inject services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection InjectServices(this IServiceCollection services)
        {
            services.AddTransient<ICustomerService, CustomerService>();
            return services;
        }
    }
}
