using Microsoft.AspNetCore.Http;

namespace Ecommerce.SherdLibrary.MiddleWares
{
    public class ListenToOnlyAPIGatway(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            //extract specific header from the request to verify it came from API Gateway
            //so wee nedd an interceprtor that add this header in the API Gateway level
            // Check for a specific header that API Gateway would include
            var sigendHeader = context.Request.Headers["Api-Gateway"];
            if(sigendHeader.FirstOrDefault() is null)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;   
                await context.Response.WriteAsync("Service Unavailable. Missing required header.");
                return;
            }
            if (!context.Request.Headers.ContainsKey("Api-Gateway"))
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Access denied. Requests must come through the API Gateway.");
                return;
            }
            else
            {
                // Call the next middleware in the pipeline
                await next(context);
            }
        
        }


    }
}
