using Ecommerce.SherdLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using System.Text.Json;

namespace Ecommerce.SherdLibrary.MiddleWares
{
    //RequestDelegate is a type in ASP.NET Core that represents a function which handles a single HTTP request using an HttpContext.​
    public class GlobalExceptionMiddleWare(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            //declare default some variables 

            string message = "Sorry , internal Server Error";
            int statusCode = (int)StatusCodes.Status500InternalServerError;
            string title = "Internal Server Error";

            try
            {
                await next(context);
                //check if too mmany requests 429
                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Warning : Too Many Requests";
                    message = "too many requests , Please Wait";
                    statusCode = (int)StatusCodes.Status429TooManyRequests;
                    await ModifyHeaderAsync(context, title, message, statusCode);
                }
                //if response is unAuthorized 401
                else if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Unauthorized Access";
                    message = "You are not authorized to access this resource.";
                    statusCode = (int)StatusCodes.Status401Unauthorized;
                    await ModifyHeaderAsync(context, title, message, statusCode);
                }
                //if response is forbidden 403 //its not that important because we gonna handle it in the controller level throw the roles 
                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Forbidden Access";
                    message = "You do not have permission to access this resource.";
                    statusCode = (int)StatusCodes.Status403Forbidden;
                    await ModifyHeaderAsync(context, title, message, statusCode);
                }
                //if response is not found 404
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    title = "Resource Not Found";
                    message = "The requested resource could not be found.";
                    statusCode = (int)StatusCodes.Status404NotFound;
                    await ModifyHeaderAsync(context, title, message, statusCode);
                }

            }
            catch (Exception ex)
            {
                //log the exception details here using your preferred logging framework
                //(serilog) we will use console for simplicity and in dev after prod we will use serilog

                LogsExceptions.LogToConsole(ex.Message);
                LogsExceptions.LogToFile(ex.Message);
                LogsExceptions.LogToDebugger(ex.Message);

                //check if exception is timeout exception

                if (ex is TimeoutException || ex is TaskCanceledException )
                {
                    title = "Request Timeout";
                    message = "The request has timed out. Please try again later.";
                    statusCode = (int)StatusCodes.Status408RequestTimeout;
                }
                //if none of these exception do the default 
                await ModifyHeaderAsync(context, title, message, statusCode);
            }

        }
        private async Task ModifyHeaderAsync(HttpContext context, string title, string message, int statusCode)
        {
            //display  the message in json format
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            //var response = new
            //{
            //    Title = title,
            //    Message = message,
            //    StatusCode = statusCode
            //};
            await context.Response.WriteAsJsonAsync(JsonSerializer.Serialize(new ProblemDetails 
            {
                    Title = title,
                    Detail = message,
                    Status = statusCode
            }),CancellationToken.None);
            return;
        }
    } 
}
