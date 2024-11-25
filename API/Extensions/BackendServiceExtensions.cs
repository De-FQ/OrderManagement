
using Services.Backend.UserManagement;
using Data.UserManagement;
using Services.Backend.Categorys;
using Services.Backend.SubCategories;
using Data.Model.SiteCategory;
using Services.Backend.ChildCategories;
using Services.Backend.Price;
using Data.Model.General;
using Services.Backend.SalesReport;
using Services.Backend.Inventory;
using Data.Model.InventoryManagement;
using Services.Backend.InventoryManagement;
using Services.Backend.InventoryManagement.Stocks;
using Services.Backend.InventoryManagement.SupplierItems;


namespace API.Extensions
{
    public static class BackendServiceExtensions
    {
        /// <summary>
        ///  Custom - Added all backend services to AddScoped
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void RegisterBackendService(this WebApplicationBuilder builder)// IServiceCollection services)
        {
            
            builder.Services.AddScoped<IUserService<User>, UserService>();
            builder.Services.AddScoped<IUserRoleService<UserRole>, UserRoleService>();
            builder.Services.AddScoped<IUserPermissionService<UserPermission>, UserPermissionService>();
            builder.Services.AddScoped<ICategoryService<Category>, CategoryService>();
            builder.Services.AddScoped<ISubCategoryService<SubCategory>, SubCategoryService>();
            builder.Services.AddScoped<IChildCategoryService<ChildCategory>, ChildCategoryService>();
            builder.Services.AddScoped<IPriceTypeCategoryService<PriceTypeCategory>, PriceTypeCategoryService>();
            builder.Services.AddScoped<IPriceTypeService<PriceType>, PriceTypeService>();
            builder.Services.AddScoped<IProductPriceService<ProductPrice>, ProductPriceService>();
            builder.Services.AddScoped<ISalesReportService<OrderItem>, SalesReportService>();
            builder.Services.AddScoped<ISupplierService<Supplier>, SupplierService>();
            builder.Services.AddScoped<IInventoryItemService<InventoryItem>, InventoryItemService>();
            builder.Services.AddScoped<IStockService<Stock>, StockService>();
            builder.Services.AddScoped<ISupplierItemService<SupplierItem>, SupplierItemService>();

        }
    }
}
