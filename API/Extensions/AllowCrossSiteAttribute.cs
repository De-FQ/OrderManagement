using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace API.Extensions
{
    /// <summary>
    /// To add a Custom Header for Antiforgery Token in each API call, 
    /// It can be added in Any Controller Method
    /// </summary>
    public class AllowCrossSiteAttribute : ResultFilterAttribute
    {
         
        public override void OnResultExecuting( ResultExecutingContext context)
        {
             var tokens = context.HttpContext.RequestServices.GetRequiredService<IAntiforgery>();
            var token = tokens.GetAndStoreTokens(context.HttpContext);
           // context.HttpContext.Response.Cookies.Append("CSRF-TOKEN", token.RequestToken, new CookieOptions { HttpOnly = false });
            //context.HttpContext.Response.Cookies.Append("Antiforgery_Token_new", token.RequestToken, new CookieOptions { HttpOnly = false });
            //context.HttpContext.Response.Cookies.Append("XSRF-TOKEN", token.RequestToken, new CookieOptions() { HttpOnly = false });

            context.HttpContext.Response.Headers.Append("XSRF-TOKEN", token.RequestToken.ToString());
            //.Headers.Add(new KeyValuePair<string, string>("XSRF-TOKEN", token.RequestToken));
            //context.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            context.HttpContext.Request.Headers.Add("RequestCustomerHeader", token.RequestToken.ToString());

            base.OnResultExecuting(context);
        }
    }
}
