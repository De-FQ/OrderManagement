using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.IO;
using System.Reflection;
using Serilog;
using NuGet.Protocol;

namespace API.Extensions
{
    //using System.Threading.Tasks;
    //using Microsoft.AspNetCore.Antiforgery;
    //using Microsoft.AspNetCore.Http;

    public class ValidateAntiForgeryTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAntiforgery _antiforgery;

        public ValidateAntiForgeryTokenMiddleware(RequestDelegate next, IAntiforgery antiforgery)
        {
            _next = next;
            _antiforgery = antiforgery;
        }

        public async Task Invoke(HttpContext context)
        {

            if (HttpMethods.IsPost(context.Request.Method))
            {
                var path = context.Request.Path.ToString().ToLower();
                if (path.Contains("/addedit") || path.Contains("/getfordatatable"))
                {
                    var v = await _antiforgery.IsRequestValidAsync(context);
                    Log.Information(path + " IsValid:" + v.ToString());
                    await _antiforgery.ValidateRequestAsync(context);
                }
            }
             

            await _next(context);
        }
    }

    //public class AntiforgeryMiddleware : IMiddleware
    //{
    //    private readonly IAntiforgery _antiforgery;

    //    public AntiforgeryMiddleware(IAntiforgery antiforgery)
    //    {
    //        _antiforgery = antiforgery;
    //    }

    //    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    //    {
    //        var isGetRequest = string.Equals("GET", context.Request.Method, StringComparison.OrdinalIgnoreCase);

    //        if (!isGetRequest)
    //        {
    //            var headers = context.Request.Headers;
    //            //var tokenCookie = headers.Select(c => "x-csrf-token").FirstOrDefault();
    //            var tokenCookie = headers.Where(c => c.Key.ToString() == "x-csrf-token").Select(x => x.Value).FirstOrDefault();
    //            var tokenHeader = string.Empty;
    //            if (headers.ContainsKey("x-csrf-token"))
    //            {
    //                tokenHeader = tokenCookie;
    //            }
    //            var action = context.Request.RouteValues["action"] as string;
    //            var controller = context.Request.RouteValues["controller"] as string;

    //            if (action == "Login" &&
    //                controller == "MyController" &&
    //                context.Request.HttpContext.User != null &&
    //                context.Request.HttpContext.User.Identity.IsAuthenticated)
    //            {
    //                //context.ExceptionHandled = true;

    //                // redirect/show error/whatever?
    //                //context.Result = new RedirectResult("/homepage");
    //            }

    //            await _antiforgery.ValidateRequestAsync(context);

    //            // await _antiforgery.ValidateRequestAsync(tokenCookie != null ? tokenCookie.ToString() : null, tokenHeader);
    //        }

    //        await next(context);
    //    }
    //}


    //public static class AntiforgeryService
    //{
    //    /// <summary>
    //    /// it will generate golabl ___RequestAntiforgeryValidationToken in each html POST Form
    //    /// </summary>
    //    /// <param name="services"></param>
    //    public static void ConfigureService(this IServiceCollection services)
    //    {
    //        // Extension method comes from the `Microsoft.AspNetCore.Antiforgery` package
    //        services.AddAntiforgery(options =>
    //        {
    //            options.HeaderName = "X-CSRF-TOKEN";
    //        });

    //         services.AddScoped<AntiforgeryMiddleware>();
    //    }

    //}
}
