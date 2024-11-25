using API.Areas.Frontend.Factories;
using Data.Model.General;
using Services.FrontEnd.Categories.ChildCategoryWeb;
using Services.FrontEnd.Categories.SubCategoryWeb;
using Services.FrontEnd.GeneralWeb.ChildCategoryDetails;
using Services.FrontEnd.GeneralWeb.PeiceTypeCategoryWeb;
using Services.FrontEnd.GeneralWeb.PriceTypeWeb;
using Services.FrontEnd.GeneralWeb.ProductPriceWeb;
using Services.Web.Categorys;
using Utility.API;
using Services.Frontend.GeneralWeb.OrderSaving;
using Services.FrontEnd.Auth;

namespace API.Extensions
{
    public static class FrontendServiceExtensions
    {
        /// <summary>
        ///  Custom - Its empty template
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        public static void RegisterFrontendService(this WebApplicationBuilder builder)// )IServiceCollection serviceCollection, IConfiguration configuration)
        {
            #region Services 
          
            builder.Services.AddScoped<IWebCategoryService, WebCategoryService>();
            builder.Services.AddScoped<IWebSubCategoryService, WebSubCategoryService>();
            builder.Services.AddScoped<IWebChildCategoryService, WebChildCategoryService>();
            builder.Services.AddScoped<IPriceTypeCategoryWebService, PriceTypeCategoryWebService>();
            builder.Services.AddScoped<IPriceTypeWebService, PriceTypeWebService>();
            builder.Services.AddScoped<IProductPriceWebService, ProductPriceWebService>();
            builder.Services.AddScoped<IWebChildCategoryDetailsService, WebChildCategoryDetailsService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IWebUserAuthService, WebUserAuthService>();

            #endregion

            #region Factories
            builder.Services.AddScoped<ICommonModelFactory, CommonModelFactory>();
            builder.Services.AddScoped<IOrderModelFactory, OrderModelFactory>();

            //builder.Services.AddScoped<ICategoryModelFactory, CategoryModelFactory>();
            builder.Services.AddScoped<IAPIHelper, APIHelper>();
            #endregion
        }
    }
}
