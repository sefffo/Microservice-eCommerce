using Ecommerce.SherdLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;

namespace ProductApi.Infrastructure.DpendencyInjections
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            //add infrastructure related services here
            services = SharedServiceContainer.AddSharedServices<ProductDbContext>(services, config, config["MySerilog:FileName"]);
            //register repositories
            services.AddScoped<IProduct, ProductRepo>();
            return services;
        }
        public static IApplicationBuilder UseInfrastructurePolicies(this IApplicationBuilder app)
        {
            //middleware related to infrastructure can be added here

            //listen to only api gateway calls


            //use infrastructure related policies here
            app = SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }

    }






}

