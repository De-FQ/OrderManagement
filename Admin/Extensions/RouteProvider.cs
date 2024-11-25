using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Admin.Extensions
{
    /// <summary>
    /// Route provider
    /// </summary>
    public static class RouteProvider
    {
        /// <summary>
        /// Register routes          
        /// </summary>
        /// <param name="endpointRouteBuilder">Endpoint route builder</param>
        public static void MapControllerCustomRoutes(this IEndpointRouteBuilder endpointRouteBuilder)
        {
   //         app.MapControllerRoute(
   // name: "default",
   //pattern: "{controller=Home}/{action=Index}/{id?}");

            //home
            endpointRouteBuilder.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}",
                   defaults: new { controller = "Home", action = "Index" });

            //endpointRouteBuilder.MapControllerRoute(
            //name: "country",
            //pattern: "",
            //defaults: new { controller = "country", action = "AddEdit" });

            //
            endpointRouteBuilder.MapControllerRoute(
                    name: "account",
                    pattern: "{controller=Account}/{action=Login}/{id?}");

        }
    }
}
