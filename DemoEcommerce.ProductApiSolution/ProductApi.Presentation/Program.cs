
using ProductApi.Infrastructure.DpendencyInjections;
using Scalar.AspNetCore;
//using ProductApi.Infrastructure.DpendencyInjections.ServiceContainer;
namespace ProductApi.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddOpenApi();
            builder.Services.AddInfrastructureServices(builder.Configuration);//wht does this mean?


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(); // Add Scalar UI
            }

            app.UseInfrastructurePolicies(); //      After mapping endpoints
            //app.UseInfrastructurePolicies();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
