using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; 
using System.Net; 
namespace Utility.Middleware
{

    public class GlobalExceptionHandlingMiddleware : IMiddleware
    { 
        private readonly ILogger _logger;
        public GlobalExceptionHandlingMiddleware ( ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }
         
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await  next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Server error",
                    Title = "Server error",
                    Detail = ex.Message
                };

                string jsonString = System.Text.Json.JsonSerializer.Serialize(problem);

                await context.Response.WriteAsync(jsonString);
                context.Response.ContentType="application/json";
            }
        }
    }
}
