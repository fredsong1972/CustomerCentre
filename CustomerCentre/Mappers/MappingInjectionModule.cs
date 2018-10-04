using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerCentre.Mappers
{
    public static class MappingInjectionModule
    {
        /// <summary>
        /// Inject auto mapper profiles
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection InjectMappers(this IServiceCollection services)
        {
            var config = new MapperConfiguration(c =>
            {
                c.AddProfile<CustomerMappingProfile>();
            });

            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }
    }
}
