
using Data.UserManagement;
using Microsoft.EntityFrameworkCore;
using Data.Base;
using Data.Model.UrlManagement;
using Data.Model.SiteCategory;
using Data.Model;
using Data.Model.General;
using Data.Model.InventoryManagement;

namespace Data.EntityFramework
{
    public class ApplicationDbContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new SoftDeleteInterceptor());
        }




        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Automatically adding query filter toall LINQ queries that use below tables
            #region Entity

            builder.Entity<UserHistory>().HasQueryFilter(x => x.Deleted == false);
            builder.Entity<UserPermission>().HasQueryFilter(x => x.Deleted == false);
            builder.Entity<UserRolePermission>().HasQueryFilter(x => x.Deleted == false);
            builder.Entity<UserRole>().HasQueryFilter(x => x.Deleted == false);
            builder.Entity<UserRoleType>().HasQueryFilter(x => x.Deleted == false);
            builder.Entity<User>().HasQueryFilter(x => x.Deleted == false);
            builder.Entity<Category>().HasQueryFilter(x => x.Deleted == false);
            builder.Entity<ChildCategory>().HasQueryFilter(x => x.Deleted == false);
            builder.Entity<SubCategory>().HasQueryFilter(x => x.Deleted == false);
            builder.Entity<Order>().HasQueryFilter(x => x.Deleted == false);
            builder.Entity<OrderItem>().HasQueryFilter(x => x.Deleted == false);
            builder.Entity<PriceType>().HasQueryFilter(x => x.Deleted == false);
            builder.Entity<PriceTypeCategory>().HasQueryFilter(x => x.Deleted == false);
            builder.Entity<ProductPrice>().HasQueryFilter(x => x.Deleted == false);
            builder.Entity<Ingredient>().HasQueryFilter(x => x.Deleted == false);
            builder.Entity<InventoryItem>().HasQueryFilter(x => x.Deleted == false);
            builder.Entity<InventoryTransaction>().HasQueryFilter(x => x.Deleted == false);
            builder.Entity<Recipe>().HasQueryFilter(x => x.Deleted == false);
            builder.Entity<RecipeIngredient>().HasQueryFilter(x => x.Deleted == false);
            builder.Entity<Stock>().HasQueryFilter(x => x.Deleted == false);
            builder.Entity<Supplier>().HasQueryFilter(x => x.Deleted == false); 
            builder.Entity<SupplierItem>().HasQueryFilter(x => x.Deleted == false); 

            Seeding.Data(builder);
            #endregion

            /// Creating Entity
        }

        #region Compact Url
        public virtual DbSet<UrlManagement> ShortUrls { get; set; }
        #endregion


        #region General

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<PriceType> PriceTypes { get; set; }
        public virtual DbSet<PriceTypeCategory> PriceTypeCategories { get; set; }
        public virtual DbSet<ProductPrice> ProductPrices { get; set; }

        #endregion

        #region Site Category
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ChildCategory> ChildCategories { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        #endregion

        #region System User Management
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserPermission> UserPermissions { get; set; }
        public virtual DbSet<UserRolePermission> UserRolePermissions { get; set; }
        public virtual DbSet<UserRoleType> UserRoleTypes { get; set; }
        public virtual DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public virtual DbSet<UserHistory> UserHistories { get; set; }
        #endregion

        #region Inventory Management

        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<InventoryItem> InventoryItems { get; set; }
        public virtual DbSet<InventoryTransaction> InventoryTransactions { get; set; }  
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierItem> SupplierItems { get; set; }

        #endregion


    }
}