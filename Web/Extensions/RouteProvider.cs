namespace Web.Extensions
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
			endpointRouteBuilder.MapControllerRoute(
				 name: "default",
				 pattern: "{controller=Home}/{action=Index}/{id?}" );

            //endpointRouteBuilder.MapControllerRoute(
            //      name: "PlaceOrder",
            //      pattern: "PlaceOrder",
            //      defaults: new { controller = "Categories", action = "PlaceOrder" });

        }


    }
}
