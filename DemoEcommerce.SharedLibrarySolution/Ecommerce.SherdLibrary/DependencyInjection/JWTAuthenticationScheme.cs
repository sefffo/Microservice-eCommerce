using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Ecommerce.SherdLibrary.DependencyInjection
{
    public static class JWTAuthenticationScheme
    {
        public static IServiceCollection AddJWTAuthenticationScheme(this IServiceCollection services ,  IConfiguration config )
        {
            //add JWT Service here
                                                                        // modified after auth service changes
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("Bearar",options =>
            {
                var key = Encoding.UTF8.GetBytes(config.GetSection("Authentication:Key").Value!);
                string issuer = config.GetSection("Authentication:Issuer").Value!;
                string audience = config.GetSection("Authentication:Audience").Value!;

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key)
                };
            });

            return services;



        }
    }
}
