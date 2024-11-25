using Data.EntityFramework;
using Data.Model.UrlManagement;
using Microsoft.EntityFrameworkCore;
using Utility.Models.Admin.DTO;

namespace API.Extensions
{
    public static class RouteProvider
    {
        public static void MapPostController(this WebApplication app)
        {
            //https://www.youtube.com/watch?v=WIWfNCoDiu0
            app.MapPost(pattern: "/shorturl", handler: async (UrlShortResponseDto url, ApplicationDbContext db, HttpContext ctx) =>
            {
                //validating the input url
                if (!Uri.TryCreate(url.Url, UriKind.Absolute, out var inputUrl))
                {
                    return Results.BadRequest(error: "Invalid url has been provided");
                }


                //mapping the short url with the long url
                var sUrl = new UrlManagement()
                {
                    Url = url.Url,
                    ShortUrl = Utility.Helpers.Common.GenerateRandomText()
                };

                //saving the mapping to the db
                db.ShortUrls.Add(sUrl);
                await db.SaveChangesAsync();

                var result = $"{ctx.Request.Scheme}://{ctx.Request.Host}/{sUrl.ShortUrl}";

                return Results.Ok(new UrlShortResponseDto()
                {
                    Url = result
                });


            });

            app.MapPost("/api/", HandleApiFallback);

            app.MapFallback(handler: async (ApplicationDbContext db, HttpContext ctx) =>
            {
                var path = ctx.Request.Path.ToUriComponent().Trim('/');
                //if(!path.Contains("/api") || !path.Contains("/web"))
                //{
                //    return Task.CompletedTask;
                //}

                UrlManagement urlMatch = await db.ShortUrls
                .FirstOrDefaultAsync(x => x.ShortUrl.Trim() == path.Trim());

                if (urlMatch == null)
                    return Results.BadRequest(error: "Invalid short url");

                return Results.Redirect(urlMatch.Url);


            });

            Task HandleApiFallback(HttpContext context)
            {
                context.Response.StatusCode = 200;// StatusCodes.Status404NotFound;
                return Task.CompletedTask;
            }
        }
    }
}
