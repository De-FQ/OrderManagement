using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace API.Extensions
{
    public class AutoSkipAntiforgeryFilterProvider : IFilterProvider
    {
        private const string BEARER_STRING = "Bearer";
        public int Order => 999;
        public void OnProvidersExecuted(FilterProviderContext context) { }
        public void OnProvidersExecuting(FilterProviderContext context)
        {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            if (context.ActionContext.ActionDescriptor.FilterDescriptors != null)
            {
                var headers = context.ActionContext.HttpContext.Request.Headers;
                if (headers.ContainsKey("Authorization"))
                {
                    var header = headers["Authorization"].FirstOrDefault();
                    if (header.StartsWith(BEARER_STRING, StringComparison.OrdinalIgnoreCase))
                    {
                        var FilterDescriptor = new FilterDescriptor(SkipAntiforgeryPolicy.Instance, FilterScope.Last);
                        var filterItem = new FilterItem(FilterDescriptor, SkipAntiforgeryPolicy.Instance);
                        context.Results.Add(filterItem);
                    }
                }
            }
        }

        // a dummy IAntiforgeryPolicy
        class SkipAntiforgeryPolicy : IAntiforgeryPolicy, IAsyncAuthorizationFilter
        {
            // a singleton 
            public static SkipAntiforgeryPolicy Instance = new SkipAntiforgeryPolicy();
            public Task OnAuthorizationAsync(AuthorizationFilterContext context) => Task.CompletedTask;
        }
    }
}
