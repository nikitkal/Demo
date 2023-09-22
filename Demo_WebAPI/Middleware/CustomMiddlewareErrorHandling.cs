using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Demo_WebAPI.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CustomMiddlewareErrorHandling
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomMiddlewareErrorHandling> _logger;
     
        public CustomMiddlewareErrorHandling(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            try
            {
                return _next(httpContext);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;

                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Type = "Server Error",
                    Title = "Server Error",
                    Detail = "An internal server has occoured"
                };
                string Json = JsonSerializer.Serialize(problem);
                httpContext.Response.ContentType = "application/json";
                return httpContext.Response.WriteAsync(Json);
               
            }
        
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CustomMiddlewareErrorHandlingExtensions
    {
        public static IApplicationBuilder UseCustomMiddlewareErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomMiddlewareErrorHandling>();
        }
    }
}
