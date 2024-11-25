using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Rewrite;

namespace Web.Extensions
{
    public class RedirectToWwwRule : IRule
    {
        public virtual void ApplyRule(RewriteContext context)
        {
            var req = context.HttpContext.Request;
            //allowed for localhost
            if (req.Host.Value.StartsWith("localhost", StringComparison.OrdinalIgnoreCase))
            {
                context.Result = RuleResult.ContinueRules;
                return;
            }

            //allowed for website hosting domain url
            if (req.Host.Value.StartsWith("admin.gtechnosoft.com", StringComparison.OrdinalIgnoreCase))
            {
                context.Result = RuleResult.ContinueRules;
                return;
            }

            //otherwise not allowed, 301 bad request
            var wwwHost = new HostString($"www.{req.Host.Value}");
            var newUrl = UriHelper.BuildAbsolute(req.Scheme, wwwHost, req.PathBase, req.Path, req.QueryString);
            var response = context.HttpContext.Response;
            response.StatusCode = 301;
            response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Location] = newUrl;
            context.Result = RuleResult.EndResponse;
        }
    }
}
