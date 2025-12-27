using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Ecommerce.SherdLibrary.MiddleWares;

namespace Ecommerce.SherdLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {                                                                                                                 //to use generoic logging 
        public static IServiceCollection AddSharedServices<TContext>(this IServiceCollection services, IConfiguration config,string fileName) where TContext : DbContext
        {
            //add generic db context
            services.AddDbContext<TContext>(options =>
                options.UseSqlServer(config.GetConnectionString("eCommerceConnection"),
                sqlserverOption=>sqlserverOption.EnableRetryOnFailure()
            ));

            //config serilog logging 
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"{fileName}-.txt", restrictedToMinimumLevel:Serilog.Events.LogEventLevel.Information,
                outputTemplate:"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {lj}{NewLine}{Exception}",rollingInterval:RollingInterval.Day)
                .CreateLogger();
            JWTAuthenticationScheme.AddJWTAuthenticationScheme(services, config);
            return services;
        }

        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
        {
            //use global exception handling middleware here
            app.UseMiddleware<GlobalExceptionMiddleWare>();
            //listen to only api gateway requests
            app.UseMiddleware<ListenToOnlyAPIGatway>();
            //use authentication and authorization middlewares here
            //app.UseAuthentication();
            //app.UseAuthorization();
            return app;
        }

    }
}
