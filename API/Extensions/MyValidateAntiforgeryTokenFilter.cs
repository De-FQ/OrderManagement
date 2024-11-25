using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;

namespace API.Extensions
{
    public class MyValidateAntiforgeryTokenFilter : IAsyncAuthorizationFilter, IAntiforgeryPolicy
    {
        private readonly IAntiforgery _antiforgery;

        public MyValidateAntiforgeryTokenFilter(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (HttpMethods.IsPost(context.HttpContext.Request.Method) && context.ActionDescriptor.DisplayName == "/Account/Login")
            {
                try
                {
                    await _antiforgery.ValidateRequestAsync(context.HttpContext);
                }
                catch (AntiforgeryValidationException)
                {
                    // redirect to index page
                    context.Result = new RedirectResult("/");
                }
            }
        }
    }
}
