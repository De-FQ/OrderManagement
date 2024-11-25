using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System.Net;
using System.Text.Json;
using Utility.LoggerService;

namespace Admin.Extensions
{ 
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app )
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                         AppSeriLog.LogInfo($"Something went wrong: {contextFeature.Error}", "ExceptionMiddlewareExtensions.ConfigureExceptionHandler.UseExceptionHandler", Serilog.Events.LogEventLevel.Error);
                         
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error."
                        }.ToString());
                    }
                });
            });
        }
    }

    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }


    //public class ExceptionMiddleware
    //{
    //    private readonly RequestDelegate _next;
    //    private readonly ILoggerManager _logger;
    //  //  private   LoggerConfiguration _loggerconf;

    //    public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
    //    {
    //        _logger = logger;
    //        //Log.Logger = new LoggerConfiguration()
    //        //    .MinimumLevel.Information()
    //        //    .WriteTo.Console()
    //        //    .WriteTo.File("logs/applog.txt", rollingInterval: RollingInterval.Day)
    //        //    .CreateLogger();
    //        //Log.Logger = new LoggerConfiguration()
    //        //.MinimumLevel.Information()
    //        //.WriteTo.Console()
    //        //.WriteTo.File("logs/applog.txt", rollingInterval: RollingInterval.Day)
    //        //.CreateLogger();

    //        _next = next;
    //    }
    //    public async Task InvokeAsync(HttpContext httpContext)
    //    {
    //        try
    //        {
    //            await _next(httpContext);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError($"Something went wrong: {ex}");
    //            Log.Information(ex.InnerException.ToString()); 
    //            await HandleExceptionAsync(httpContext, ex);
    //        }
    //    }
    //    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    //    {
    //        context.Response.ContentType = "application/json";
    //        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    //        await context.Response.WriteAsync(new ErrorDetails()
    //        {
    //            StatusCode = context.Response.StatusCode,
    //            Message = "Internal Server Error from the custom middleware."
    //        }.ToString());
    //    }
    //}
}
