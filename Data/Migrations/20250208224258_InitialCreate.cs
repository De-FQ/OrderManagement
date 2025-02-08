using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowInMenu = table.Column<bool>(type: "bit", nullable: false),
                    DiscountActive = table.Column<bool>(type: "bit", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayOrder = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerContact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmountReceived = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChangeToReturn = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayOrder = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShortUrls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayOrder = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NavigationUrl = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AccessList = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserPermissionId = table.Column<int>(type: "int", nullable: true),
                    ShowInMenu = table.Column<bool>(type: "bit", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayOrder = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPermissions_UserPermissions_UserPermissionId",
                        column: x => x.UserPermissionId,
                        principalTable: "UserPermissions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRefreshTokens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayOrder = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowInMenu = table.Column<bool>(type: "bit", nullable: false),
                    DiscountActive = table.Column<bool>(type: "bit", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayOrder = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    PriceTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayOrder = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayOrder = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItems_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SupplierItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayOrder = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierItems_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserRoleTypeId = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayOrder = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_UserRoleTypes_UserRoleTypeId",
                        column: x => x.UserRoleTypeId,
                        principalTable: "UserRoleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ChildCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubCategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowInMenu = table.Column<bool>(type: "bit", nullable: false),
                    DiscountActive = table.Column<bool>(type: "bit", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayOrder = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChildCategories_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Transaction = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    InventoryItemId = table.Column<int>(type: "int", nullable: true),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayOrder = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InventoryItemId = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayOrder = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipes_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryItemId = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetUnitCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CompanyCostMargin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OldQuantity = table.Column<int>(type: "int", nullable: false),
                    NewQuantity = table.Column<int>(type: "int", nullable: false),
                    TotalQuantity = table.Column<int>(type: "int", nullable: false),
                    TotalUnitNetCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalUnitCompanyPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayOrder = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stocks_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UserRolePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserPermissionId = table.Column<long>(type: "bigint", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: true),
                    UserRoleId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    Allowed = table.Column<bool>(type: "bit", nullable: false),
                    AllowList = table.Column<bool>(type: "bit", nullable: false),
                    AllowDisplayOrder = table.Column<bool>(type: "bit", nullable: false),
                    AllowAdd = table.Column<bool>(type: "bit", nullable: false),
                    AllowEdit = table.Column<bool>(type: "bit", nullable: false),
                    AllowDelete = table.Column<bool>(type: "bit", nullable: false),
                    AllowView = table.Column<bool>(type: "bit", nullable: false),
                    AllowActive = table.Column<bool>(type: "bit", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRolePermissions_UserPermissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "UserPermissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRolePermissions_UserRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "UserRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EncryptedPassword = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserRoleId = table.Column<int>(type: "int", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayOrder = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_UserRoles_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InventoryItemId = table.Column<int>(type: "int", nullable: false),
                    ChildCategoryId = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayOrder = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingredients_ChildCategories_ChildCategoryId",
                        column: x => x.ChildCategoryId,
                        principalTable: "ChildCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Ingredients_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PriceTypeCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChildCategoryId = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayOrder = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceTypeCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceTypeCategories_ChildCategories_ChildCategoryId",
                        column: x => x.ChildCategoryId,
                        principalTable: "ChildCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UserHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogoutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Duration = table.Column<double>(type: "float", nullable: false),
                    PublicIP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Browser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperatingSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Device = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "RecipeIngredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    IngredientId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayOrder = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PriceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceTypeCategoryId = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayOrder = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceTypes_PriceTypeCategories_PriceTypeCategoryId",
                        column: x => x.PriceTypeCategoryId,
                        principalTable: "PriceTypeCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProductPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChildCategoryId = table.Column<int>(type: "int", nullable: false),
                    PriceTypeId = table.Column<int>(type: "int", nullable: false),
                    PriceTypeCategoryId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayOrder = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPrices_ChildCategories_ChildCategoryId",
                        column: x => x.ChildCategoryId,
                        principalTable: "ChildCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProductPrices_PriceTypeCategories_PriceTypeCategoryId",
                        column: x => x.PriceTypeCategoryId,
                        principalTable: "PriceTypeCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProductPrices_PriceTypes_PriceTypeId",
                        column: x => x.PriceTypeId,
                        principalTable: "PriceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Active", "CreatedBy", "CreatedOn", "Deleted", "DeletedAt", "Description", "DiscountActive", "DiscountPercentage", "DisplayOrder", "Guid", "ImageName", "ModifiedBy", "ModifiedOn", "Name", "ShowInMenu" },
                values: new object[] { 1, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9313), false, null, "Retaurant Menu", false, 0m, 0L, new Guid("3a51191a-e122-4613-8334-e3270e332f5d"), "5f5fe9d8-f98a-44da-813f-31fc2aebc206.webp", null, null, "ریسٹورنٹ مینو", true });

            migrationBuilder.InsertData(
                table: "UserPermissions",
                columns: new[] { "Id", "AccessList", "Active", "CreatedBy", "CreatedOn", "Deleted", "DeletedAt", "DisplayOrder", "Guid", "Icon", "ModifiedBy", "ModifiedOn", "NavigationUrl", "ShowInMenu", "Title", "UserPermissionId" },
                values: new object[,]
                {
                    { 1, "Allowed", true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5533), false, null, 1L, new Guid("f7015c1d-6b5f-4350-a2e7-1767378d9b24"), "fa-solid fa-table-cells-large", null, null, "/Home/Index", true, "Dashboard", null },
                    { 2, "Allowed", true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5537), false, null, 2L, new Guid("454b5667-028a-4bab-84a9-ff395589b2f9"), "fas fa-user-cog", null, null, "#", true, "User Management", null },
                    { 6, "Allowed", true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5562), false, null, 6L, new Guid("b0903921-67b1-41dd-a98a-144d4feb8597"), "fa-solid fa-table-cells-large", null, null, "#", true, "Categories Menu", null },
                    { 10, "Allowed", true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5637), false, null, 10L, new Guid("084cdd58-975d-41e9-9c4f-fac118fe0d51"), null, null, null, "#", true, "General Setting", null },
                    { 14, "Allowed", true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5659), false, null, 14L, new Guid("10ea2efe-afc5-4c9e-af5b-21f9e2919140"), null, null, null, "/SalesReport/SalesReportList", true, "Sales Report", null },
                    { 15, "Allowed", true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5663), false, null, 15L, new Guid("21c69ea2-17db-4fb2-b54c-e21d89ecd7c1"), null, null, null, "#", true, "Inventory Management", null }
                });

            migrationBuilder.InsertData(
                table: "UserRolePermissions",
                columns: new[] { "Id", "AllowActive", "AllowAdd", "AllowDelete", "AllowDisplayOrder", "AllowEdit", "AllowList", "AllowView", "Allowed", "CreatedBy", "CreatedOn", "Deleted", "DeletedAt", "Guid", "ModifiedBy", "ModifiedOn", "PermissionId", "RoleId", "UserPermissionId", "UserRoleId" },
                values: new object[,]
                {
                    { 1, true, true, true, true, true, true, true, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5930), false, null, new Guid("abf3200c-e1e1-48ca-8d40-4db35d9523c4"), null, null, null, null, 1L, 1L },
                    { 2, true, true, true, true, true, true, true, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5956), false, null, new Guid("5be3258e-448e-4f2a-be11-7be2e402d12d"), null, null, null, null, 2L, 1L },
                    { 3, true, true, true, true, true, true, true, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5972), false, null, new Guid("6bf24a6f-87e3-4e82-b209-0bccc3ed2cb6"), null, null, null, null, 3L, 1L },
                    { 4, true, true, true, true, true, true, true, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5986), false, null, new Guid("5d4c1c55-ab5a-46ba-aace-ee7847983d3d"), null, null, null, null, 4L, 1L },
                    { 5, true, true, true, true, true, true, true, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5999), false, null, new Guid("5afc366c-1e1f-465a-a464-3b49bf884a54"), null, null, null, null, 5L, 1L },
                    { 6, true, true, true, true, true, true, true, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(6014), false, null, new Guid("a6bfa549-4577-4673-be20-79cd1cd05853"), null, null, null, null, 6L, 1L },
                    { 7, true, true, true, true, true, true, true, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(6028), false, null, new Guid("67e8aa7b-6060-4e43-a88d-a3ecbab08908"), null, null, null, null, 7L, 1L },
                    { 8, true, true, true, true, true, true, true, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(6042), false, null, new Guid("d167e02f-d4e5-40e4-9cda-f14be8387de8"), null, null, null, null, 8L, 1L },
                    { 9, true, true, true, true, true, true, true, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(6055), false, null, new Guid("a9d029ef-c98e-45ff-89f7-a8a01cd372c9"), null, null, null, null, 9L, 1L },
                    { 10, true, true, true, true, true, true, true, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(6073), false, null, new Guid("b41a8f7a-4131-41c5-953d-2fa44bb9b39e"), null, null, null, null, 10L, 1L },
                    { 11, true, true, true, true, true, true, true, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(6086), false, null, new Guid("23235b74-69e4-4371-a3ec-07d058e6b486"), null, null, null, null, 11L, 1L },
                    { 12, true, true, true, true, true, true, true, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(6099), false, null, new Guid("5d2be266-f4e6-4092-b8de-9827fbd3a692"), null, null, null, null, 12L, 1L },
                    { 13, true, true, true, true, true, true, true, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(6113), false, null, new Guid("e5b21022-2ccf-4bdf-be78-af361446a3c9"), null, null, null, null, 13L, 1L },
                    { 14, true, true, true, true, true, true, true, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(6129), false, null, new Guid("142ee59c-19e3-4cee-84fc-3e5b992a6141"), null, null, null, null, 14L, 1L },
                    { 15, true, true, true, true, true, true, true, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(6145), false, null, new Guid("265b0cf6-202b-4373-93ea-886a3115d246"), null, null, null, null, 15L, 1L },
                    { 16, true, true, true, true, true, true, true, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(6159), false, null, new Guid("f12ea3a5-0b0d-46e7-9172-5357438ff424"), null, null, null, null, 16L, 1L },
                    { 17, true, true, true, true, true, true, true, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(6173), false, null, new Guid("bd32f1e0-ef8f-4917-92c6-315f616ab21e"), null, null, null, null, 17L, 1L },
                    { 18, true, true, true, true, true, true, true, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(6191), false, null, new Guid("766b030b-19c0-4963-8973-6819c10f81c2"), null, null, null, null, 18L, 1L }
                });

            migrationBuilder.InsertData(
                table: "UserRoleTypes",
                columns: new[] { "Id", "Active", "CreatedBy", "CreatedOn", "Deleted", "DeletedAt", "DisplayOrder", "Guid", "ModifiedBy", "ModifiedOn", "Name" },
                values: new object[,]
                {
                    { 1, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5320), false, null, 0L, new Guid("d2d8e16a-3c11-4f5f-a239-1ddb9b94b4e2"), null, null, "Root" },
                    { 2, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5357), false, null, 0L, new Guid("df671305-27db-4dd9-8dd5-11acaaf58843"), null, null, "Administrator" },
                    { 3, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5372), false, null, 0L, new Guid("96e789a1-48ad-4dee-bccb-8fb56cb3c510"), null, null, "Content Manager" },
                    { 4, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5374), false, null, 0L, new Guid("f79aef44-3f91-4302-9dc9-f0993a22893f"), null, null, "Account Department" }
                });

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "Id", "Active", "CategoryId", "CreatedBy", "CreatedOn", "Deleted", "DeletedAt", "Description", "DiscountActive", "DiscountPercentage", "DisplayOrder", "Guid", "ImageName", "ModifiedBy", "ModifiedOn", "Name", "ShowInMenu" },
                values: new object[,]
                {
                    { 1, true, 1, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9384), false, null, "Biryani", false, 0m, 0L, new Guid("07871f26-0e04-4496-aee0-e7c6ede96b63"), "3fc1eef7-2c10-4829-bae9-7988cc69e779.webp", null, null, "بریانی", true },
                    { 2, true, 1, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9392), false, null, "Pulao", false, 0m, 0L, new Guid("f34397ca-b0d1-43ba-be98-013d891b388c"), "eeeb2d9a-79d8-49f3-99a2-28ebcc5f695f.webp", null, null, "پلاو", true },
                    { 3, true, 1, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9396), false, null, "Salan", false, 0m, 0L, new Guid("6b3005e1-5826-4ea1-bfe5-aa8e6dafb8c8"), "bee49fe4-2215-4ee3-b2d8-24e784dfa930.webp", null, null, "سالن", true },
                    { 4, true, 1, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9400), false, null, "Cold Drink", false, 0m, 0L, new Guid("e7d904df-be7a-45b4-b044-a3a7dcdb0d00"), "18affa30-6bb8-496d-9921-02336a9a3480.webp", null, null, "کولڈ ڈرنک", true },
                    { 5, true, 1, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9406), false, null, "Extra", false, 0m, 0L, new Guid("4b33cc88-2e7b-42d0-bb47-afef712ce097"), "2c064164-aecf-40a8-bea0-c0df28b0508a.webp", null, null, "ایکسٹرا", true }
                });

            migrationBuilder.InsertData(
                table: "UserPermissions",
                columns: new[] { "Id", "AccessList", "Active", "CreatedBy", "CreatedOn", "Deleted", "DeletedAt", "DisplayOrder", "Guid", "Icon", "ModifiedBy", "ModifiedOn", "NavigationUrl", "ShowInMenu", "Title", "UserPermissionId" },
                values: new object[,]
                {
                    { 3, "Allowed,Active,List,Add,Edit,Delete", true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5546), false, null, 3L, new Guid("1ee2dee1-a4c2-41a4-993e-0a3c898a58c0"), null, null, null, "/UserMgmt/User/UserList", true, "System Users", 2 },
                    { 4, "Allowed,Active,List,DisplayOrder,Add,Edit,Delete", true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5551), false, null, 4L, new Guid("bf17e5f4-6b72-4254-a9d4-b7a0ba233018"), null, null, null, "/UserMgmt/Role/RoleList", true, "System Roles", 2 },
                    { 5, "Allowed,Active,List,DisplayOrder,Add,Edit,Delete", true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5555), false, null, 5L, new Guid("27093ffd-9725-445a-84b3-5d4a4ead4146"), null, null, null, "/UserMgmt/Permission/PermissionList", true, "System Permissions", 2 },
                    { 7, "Allowed,Active,List,Add,Edit,Delete", true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5623), false, null, 7L, new Guid("6bcb2f97-e2ab-44e5-8def-c173fbb51ff5"), null, null, null, "/Category/CategoryList", true, "Category", 6 },
                    { 8, "Allowed,Active,List,Add,Edit,Delete", true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5627), false, null, 8L, new Guid("74f84d47-912a-4a62-b750-59b406e322f0"), null, null, null, "/SubCategory/SubCategoryList", true, "Sub Category", 6 },
                    { 9, "Allowed,Active,List,Add,Edit,Delete", true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5633), false, null, 9L, new Guid("28d0233f-2ab2-4ea9-a3b3-766b46b14181"), null, null, null, "/ChildCategory/ChildCategoryList", true, "Child Category", 6 },
                    { 11, "Allowed,Active,List,Add,Edit,Delete", true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5645), false, null, 11L, new Guid("1713b03b-0b05-4f06-bda8-a849db6e7d08"), null, null, null, "/PriceTypeCategory/PriceTypeCategoryList", true, "Price Type Category", 10 },
                    { 12, "Allowed,Active,List,Add,Edit,Delete", true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5649), false, null, 12L, new Guid("b70bae9d-0a86-49a7-b023-2909c149a235"), null, null, null, "/PriceType/PriceTypeList", true, "Price Type", 10 },
                    { 13, "Allowed,Active,List,Add,Edit,Delete", true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5654), false, null, 13L, new Guid("4e374cb6-4e55-45ee-b9ff-a15a33d39954"), null, null, null, "/ProductPrice/ProductPriceList", true, "Product Price", 10 },
                    { 16, "Allowed,Active,List,Add,Edit,Delete", true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5668), false, null, 16L, new Guid("1b588de4-62c5-4648-9463-64ae5b4c91b2"), null, null, null, "/Supplier/SupplierList", true, "Suppliers", 15 },
                    { 17, "Allowed,Active,List,Add,Edit,Delete", true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5673), false, null, 17L, new Guid("096f553c-4cea-40b2-ada5-f4eac6434f1c"), null, null, null, "/InventoryItem/InventoryItemList", true, "Inventory Item", 15 },
                    { 18, "Allowed,Active,List,Add,Edit,Delete", true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5678), false, null, 18L, new Guid("3d8462cb-1876-420e-ab12-2c3b2e73cc95"), null, null, null, "/Stock/StockList", true, "Stock", 15 }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "Active", "CreatedBy", "CreatedOn", "Deleted", "DeletedAt", "DisplayOrder", "Guid", "ModifiedBy", "ModifiedOn", "Name", "UserRoleTypeId" },
                values: new object[,]
                {
                    { 1, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5480), false, null, 1L, new Guid("e8f07d65-62ea-4619-be35-8f161865196c"), null, null, "Root", 1 },
                    { 2, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5485), false, null, 2L, new Guid("a2718f58-81de-4439-945e-4429719f3d4a"), null, null, "Administrator", 2 },
                    { 3, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(5489), false, null, 3L, new Guid("d1f04562-1d6e-4df7-866f-bb3b865e7cfe"), null, null, "Supervisor", 3 }
                });

            migrationBuilder.InsertData(
                table: "ChildCategories",
                columns: new[] { "Id", "Active", "CreatedBy", "CreatedOn", "Deleted", "DeletedAt", "Description", "DiscountActive", "DiscountPercentage", "DisplayOrder", "Guid", "ImageName", "ModifiedBy", "ModifiedOn", "Name", "ShowInMenu", "SubCategoryId" },
                values: new object[,]
                {
                    { 1, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9465), false, null, "Chicken Biryani Single 190", false, 0m, 0L, new Guid("b0199ee2-0ba7-4e2c-a361-a4915d53f68a"), "f27056f2-969e-4760-af2d-3802962c88a7.webp", null, null, "چکن بریانی سنگل 190", true, 1 },
                    { 2, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9535), false, null, "Chiken Biryani 500 Gram Single Boti 230", false, 0m, 0L, new Guid("613202dc-2660-48b6-9ea2-8b569ac1c7a0"), "7e8304fa-f496-4c69-ad39-f9db83a0f933.webp", null, null, "چکن بریانی 500 گرام سنگل بوٹی 230", true, 1 },
                    { 3, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9540), false, null, "Chickhen Biryani 500gm Double Boti 280", false, 0m, 0L, new Guid("d7ef7cfd-b18f-4f19-80b9-ab23ffe0749e"), "ba2f3ff2-5c4f-4f12-9b13-c95ad8fcf828.webp", null, null, "چکن بریانی 500 گرام ڈبل بوٹی 280", true, 1 },
                    { 4, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9552), false, null, "Chicken Biryani 1kg 560", false, 0m, 0L, new Guid("4bf32f89-11c7-449d-9482-9d9600ee1752"), "df1e0b2c-d0e0-4e37-bea0-429155128063.webp", null, null, "چکن بریانی 1 کلو 560", true, 1 },
                    { 5, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9556), false, null, "Sada Biryani Single 140", false, 0m, 0L, new Guid("40990db7-42b9-4da0-9b48-91112b63b29b"), "6f884135-fdfa-4a2f-b6ca-1f38d020713c.webp", null, null, "سادہ بریانی سنگل 140", true, 1 },
                    { 6, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9560), false, null, "Sada Biryani 500gm 180", false, 0m, 0L, new Guid("43fcc734-7048-49d2-86df-dd6425943833"), "567a0a91-0b4f-4b1d-bfb8-db2df9e2ecfd.webp", null, null, "سادہ بریانی 500 گرام 180", true, 1 },
                    { 7, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9564), false, null, "Sada Biryani 1kg 360", false, 0m, 0L, new Guid("280ea488-039d-4c52-a014-843ae4d7744a"), "304fdda3-e2d2-4d5b-832f-e8a7d622c96a.webp", null, null, "سادہ بریانی 1 کلو 360", true, 1 },
                    { 8, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9567), false, null, "Biryani 100", false, 0m, 0L, new Guid("851a5b9c-0038-48e5-beed-3e486ad141e4"), "361c284b-50d9-4d79-80ff-02b9325afa1d.webp", null, null, "بریانی 100", true, 1 },
                    { 9, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9570), false, null, "Biryani 150", false, 0m, 0L, new Guid("efec718d-c879-4678-995d-15803245b43c"), "6e6407b0-c4d9-4b8f-a9cf-28f4958ef69a.webp", null, null, "بریانی 150", true, 1 },
                    { 10, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9573), false, null, "Biryani 200", false, 0m, 0L, new Guid("f2391bdc-e44a-46ed-bb34-cd1a793ef11a"), "ccb4d237-0bd7-4d03-94c1-707ef490ae47.webp", null, null, "بریانی 200", true, 1 },
                    { 11, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9577), false, null, "Biryani 50", false, 0m, 0L, new Guid("9d3aa162-c6b9-43ea-a6ab-fe56789971d2"), "fb31e516-d59c-493d-9336-34674a47cb03.webp", null, null, "بریانی 50", true, 1 },
                    { 12, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9586), false, null, "Box 10", false, 0m, 0L, new Guid("edf3f426-8fce-4790-a9d8-ba4f0a3a5f82"), "aa963eeb-db13-4fd3-a5a7-6b2c500f7a60.webp", null, null, "باکس 10", true, 1 },
                    { 13, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9589), false, null, "Biryani 300", false, 0m, 0L, new Guid("7dfeacd4-1a71-4345-8b0d-568767404cac"), "1b0c1555-328b-4ec9-84cf-7e288f6c23b8.webp", null, null, "بریانی 300", true, 1 },
                    { 14, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9592), false, null, "Biryani 400", false, 0m, 0L, new Guid("c4f44d1d-35f0-479c-a21f-30347bc0b4c4"), "e9462128-f475-4908-9838-3220902a0d45.webp", null, null, "بریانی 400", true, 1 },
                    { 15, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9596), false, null, "Biryani 500", false, 0m, 0L, new Guid("2ee681ae-38a6-43b3-9b47-9b88252b0838"), "d896abd5-296d-4dcf-9fef-c0ffeeac562d.webp", null, null, "بریانی 500", true, 1 },
                    { 16, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9600), false, null, "Biryani 90", false, 0m, 0L, new Guid("8e9c0850-3025-4a22-9199-900ebbfc763c"), "139facfb-2539-43e8-937d-45a45b5f655f.webp", null, null, "بریانی 90", true, 1 },
                    { 17, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9604), false, null, "Pao Sada Pulao 100", false, 0m, 0L, new Guid("e7e1d65c-0d21-44a6-bd5e-95fdb99ed7bc"), "bc42c777-2515-467a-bc43-172b85ff9b78.webp", null, null, "پاو سادہ پلاو 100", true, 1 },
                    { 18, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9607), false, null, "Chicken Pulao Single 190", false, 0m, 0L, new Guid("8fe3c61a-c916-433e-9a99-af06650f6229"), "b949ace5-0900-4715-aaf9-8e93423b8f19.webp", null, null, "چکن پلاو سنگل 190", true, 2 },
                    { 19, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9610), false, null, "Chickhen Pulao 500gm Double Boti 280", false, 0m, 0L, new Guid("e8317d40-8338-47dc-9938-e45ac81f3549"), "2ddb2b38-8def-497b-b4e0-ebd15e523715.webp", null, null, "چکن پلاو 500 گرام ڈبل بوٹی 280", true, 2 },
                    { 20, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9618), false, null, "Chicken Pulao 1kg 560", false, 0m, 0L, new Guid("e4e95523-6f5f-4016-a413-a24d9b943c8d"), "6ff96321-81ba-4da0-b108-a107aef1f3ed.webp", null, null, "چکن پلاو 1 کلو 560", true, 2 },
                    { 21, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9621), false, null, "Sada Pulao Single 140", false, 0m, 0L, new Guid("dd267aee-d4cf-459b-a34b-379ad2c6448d"), "0bbff197-39da-4559-936e-dfb6f3d020c2.webp", null, null, "سادہ پلاو سنگل 140", true, 2 },
                    { 22, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9624), false, null, "Sada Pulao 500gm 180", false, 0m, 0L, new Guid("571bfb0a-c2c4-4a9b-b4dd-cf958fabdad9"), "ce454ad2-513a-47bc-93d9-812267fa087e.webp", null, null, "سادہ پلاو 500 گرام 180", true, 2 },
                    { 23, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9628), false, null, "Sada Pulao 1kg 360", false, 0m, 0L, new Guid("b882fcd2-9527-4389-a85c-4d9464e44190"), "cbcfaf75-72b2-4fc1-a445-93e837760ffd.webp", null, null, "سادہ پلاو 1 کلو 360", true, 2 },
                    { 24, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9631), false, null, "Pulao 100", false, 0m, 0L, new Guid("7f51c56e-7770-4243-bbdc-910af9e3c327"), "fc1e5a8e-5c0f-4dba-807f-fb01a52c1ff3.webp", null, null, "پلاو 100", true, 2 },
                    { 25, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9634), false, null, "Pulao 150", false, 0m, 0L, new Guid("303fae20-b610-4e79-ba4f-5e0c934aaeca"), "6c17b2b1-7715-48de-a06b-9fe4c9033fd1.webp", null, null, "پلاو 150", true, 2 },
                    { 26, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9637), false, null, "Pulao 200", false, 0m, 0L, new Guid("c66fdfa2-c0aa-435a-be9c-8935729b3038"), "0414aff1-3463-4c5f-9b93-de8ff44da150.webp", null, null, "پلاو 200", true, 2 },
                    { 27, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9641), false, null, "Pulao 50", false, 0m, 0L, new Guid("5fea8990-9c5a-42ba-98ed-7bbf10aaa4b1"), "44b6cb0e-613e-490b-8223-d326a90d26d8.webp", null, null, "پلاو 50", true, 2 },
                    { 28, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9646), false, null, "Box 10", false, 0m, 0L, new Guid("64f3a4df-21ac-4d89-93b6-ecfee35aa5d3"), "d150861d-05ea-4849-b55c-6caa435a408a.webp", null, null, "باکس 10a", true, 2 },
                    { 29, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9650), false, null, "Pulao 300", false, 0m, 0L, new Guid("287a3733-30ff-46de-9b66-dc50423a8695"), "3e92bef7-28fb-4837-87cf-d5af923fc2f1.webp", null, null, "پلاو 300", true, 2 },
                    { 30, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9653), false, null, "Pulao 400", false, 0m, 0L, new Guid("1665b4f5-5541-4f00-9d8c-a7b2b54c9f41"), "81bcdea8-4565-478b-a674-c9dbef6fe3c0.webp", null, null, "پلاو 400", true, 2 },
                    { 31, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9656), false, null, "Pulao 500", false, 0m, 0L, new Guid("5694b2f4-9a6a-4a12-aece-e0c2ed1de28e"), "f7f9218d-10a0-4499-a3ad-aec6a83f372d.webp", null, null, "پلاو 500", true, 2 },
                    { 32, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9660), false, null, "Pao Sada Pulao 100", false, 0m, 0L, new Guid("8759f738-6dc8-4154-93a5-60dc6da54ac5"), "4f6e8e97-cfc6-47bc-bbe3-0f1badaa206d.webp", null, null, "پاو سادہ پلاو 100a", true, 2 },
                    { 33, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9717), false, null, "Pao Chicken 140", false, 0m, 0L, new Guid("033b0c8b-a1ee-4e14-ace1-b1c40896f915"), "4f6e8e97-cfc6-47bc-bbe3-0f1badaa206d.webp", null, null, "پاو چکن 140a", true, 2 },
                    { 34, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9721), false, null, "Chicken Karhai Half 150", false, 0m, 0L, new Guid("82673a92-d8a2-4015-9021-52d43db16e0b"), "4f6e8e97-cfc6-47bc-bbe3-0f1badaa206d.webp", null, null, "چکن کڑاہی ہاف 150", true, 3 },
                    { 35, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9724), false, null, "Chicken Karhai Full 250", false, 0m, 0L, new Guid("b7c7d0f8-c9e4-423c-9b61-52dfb166fb2e"), "cd9fe8ea-66d7-4a87-a4cd-c546580637c4.webp", null, null, "چکن کڑاہی فل 250", true, 3 },
                    { 36, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9729), false, null, "Mix Sabzi Half 70", false, 0m, 0L, new Guid("ef276d36-6b79-4023-ad0f-928b6a5a4c3c"), "4387529e-6d23-4a19-a5c5-b62e93302d0c.webp", null, null, "مکس سبزی ہاف 70", true, 3 },
                    { 37, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9733), false, null, "Mix Sabzi Full 130", false, 0m, 0L, new Guid("7a7b2428-7d5c-4310-bc7f-75f33adb6c9d"), "35c364e2-c550-40b4-a304-67a2cf4bbd7d.webp", null, null, "مکس سبزی فل 130", true, 3 },
                    { 38, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9736), false, null, "Chicken Kaleji Half 120", false, 0m, 0L, new Guid("fe97d378-58fd-4dba-9bf7-d998211a8057"), "35c364e2-c550-40b4-a304-67a2cf4bbd7d.webp", null, null, "چکن کلیجی ہاف 120", true, 3 },
                    { 39, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9739), false, null, "Chicken Kaleji Full 200", false, 0m, 0L, new Guid("95f76762-681a-4889-9745-d3bd748ce3c2"), "c8d108f9-3346-4ef5-a9ae-80b3d8c700bf.webp", null, null, "چکن کلیجی فل 200", true, 3 },
                    { 40, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9743), false, null, "Qeema Half 120", false, 0m, 0L, new Guid("1965f5a6-f995-4607-a794-1506e9cce092"), "b58faf38-d5a6-4003-b1fa-300b80066908.webp", null, null, "قیمہ ہاف 120", true, 3 },
                    { 41, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9746), false, null, "Qeema Full 200", false, 0m, 0L, new Guid("bfb0b82a-1b65-420e-919e-d34bfe25d918"), "86b66cc6-0a40-4fb4-b590-a4acb0b93ad4.webp", null, null, "قیمہ فل 200", true, 3 },
                    { 42, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9749), false, null, "Daal Maash Half 90", false, 0m, 0L, new Guid("68960344-6fdf-4e7d-a63a-769abdfa4fef"), "c76c0b95-f888-4981-ba59-63ee27b3a882.webp", null, null, "دال ماش ہاف 90", true, 3 },
                    { 43, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9752), false, null, "Daal Maash Full 160", false, 0m, 0L, new Guid("2435747a-fb41-4d06-8ce5-3dc7c958f4c2"), "588f7db2-d641-4886-8ebc-fbcb5286a2a3.webp", null, null, "دال ماش فل 160", true, 3 },
                    { 44, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9758), false, null, "Murgh Chana Full 180", false, 0m, 0L, new Guid("6aa01a81-3c04-45f8-984a-a38081d0b377"), "f13008fb-1791-43b6-9671-a2028290eb51.webp", null, null, "مرغ چنا فل 180", true, 3 },
                    { 45, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9761), false, null, "Chana Half 80", false, 0m, 0L, new Guid("1b2c1cd3-2c52-440d-a34e-de311ef6e9bc"), "61438335-d77d-4d9f-a5e9-b370d7a6ffa1.webp", null, null, "چنا ہاف 80", true, 3 },
                    { 46, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9764), false, null, "Chana Full 130", false, 0m, 0L, new Guid("a3d7cd34-be43-4bbe-b0f5-ddfd2fd0cad7"), "ab579cb1-5289-47a4-86d1-84743caa3e6a.webp", null, null, "چنا فل 130", true, 3 },
                    { 47, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9767), false, null, "Anda Tamatar Half 90", false, 0m, 0L, new Guid("2f629168-3681-4a62-973f-b526db500522"), "fdea841b-2d11-40ec-82c2-4204bdd5e381.webp", null, null, "انڈہ ٹماٹر ہاف 90", true, 3 },
                    { 48, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9771), false, null, "Anda Tamatar Full 150", false, 0m, 0L, new Guid("7f2dbbaa-5e2e-424b-b5bb-05e5ed1ae344"), "3727d6a8-4edc-4885-91f9-68b586167749.webp", null, null, "انڈہ ٹماٹر فل 150", true, 3 },
                    { 49, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9775), false, null, "Salan 100", false, 0m, 0L, new Guid("333210ad-7f63-4fe9-9b17-cc9f2a58d909"), "08a2e41a-27c5-433f-ac1e-3369b316cb44.webp", null, null, "سالان 100", true, 3 },
                    { 50, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9778), false, null, "Fry 20", false, 0m, 0L, new Guid("e4ec7d69-787e-4c97-bde3-b4ef43eebbbe"), "e1469f23-26d4-45a5-a48d-f4de2ea669d2.webp", null, null, "فرائی 20", true, 3 },
                    { 51, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9781), false, null, "Naan 25", false, 0m, 0L, new Guid("de026db4-9ad5-4103-bdc4-fec04229ea9b"), "9b046c03-5534-41a3-aa2a-fead4206ee11.webp", null, null, "نان 25", true, 3 },
                    { 52, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9786), false, null, "Lal Roti 20", false, 0m, 0L, new Guid("5f9eb768-810b-44f2-9358-ef7978a61e72"), "cabb297e-16b1-469b-b34b-4efc893abb3a.webp", null, null, "لال روٹی 20", true, 3 },
                    { 53, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9789), false, null, "Bhindi Full 200", false, 0m, 0L, new Guid("c9349d7f-54ad-44e8-a7ee-2c26c8a9e71a"), "1b5ccc91-ff1b-4442-9ac3-d3c5c5a78f79.webp", null, null, "بھنڈی فل 200", true, 3 },
                    { 54, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9792), false, null, "Bhindi Half 110", false, 0m, 0L, new Guid("6e24a998-30fe-4d78-a01c-202fe15c68d8"), "d8ef1448-61fd-439c-9c1e-f55beaa5e8ce.webp", null, null, "بھنڈی ہاف 110", true, 3 },
                    { 55, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9796), false, null, "Aalu Palak Half 80", false, 0m, 0L, new Guid("badf92f8-1474-472d-815d-330fe9a48139"), "d6410226-52fc-4e94-8195-e72d386127ca.webp", null, null, "آلو پالک ہاف 80", true, 3 },
                    { 56, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9799), false, null, "Aalu Palak Full 130", false, 0m, 0L, new Guid("22008739-3422-4b56-bd76-e86553e0e703"), "924356b9-364d-4161-9208-06b87df17bce.webp", null, null, " آلو پالک فل 130", true, 3 },
                    { 57, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9802), false, null, "Daal Chana Half 80", false, 0m, 0L, new Guid("a886d8a8-e387-47e7-9593-622ec47e9d2d"), "0b8aea2c-be4a-4039-8b80-1ac70b1f4822.webp", null, null, " دال چنا ہاف 80", true, 3 },
                    { 58, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9805), false, null, "Daal Chana Full 130", false, 0m, 0L, new Guid("e8217746-d408-4b10-b9eb-2fca302a733e"), "21a46f7a-caed-4487-8c5d-378aa576777a.webp", null, null, " دال چنا فل 130", true, 3 },
                    { 59, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9809), false, null, "Cold Drink 345 ML", false, 0m, 0L, new Guid("86174444-d791-4163-8af2-71a3cfe0390e"), "53960d5f-9324-49ff-9b45-d07b3e0d2304.webp", null, null, " کولڈ ڈرنک 345 ملی لیٹر", true, 4 },
                    { 60, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9815), false, null, "Cold Drink 500 ML", false, 0m, 0L, new Guid("ce3a0822-0d82-4434-994e-60a3cb977a8a"), "e707d3fc-89ea-4926-b14a-e5625edb76b0.webp", null, null, " کولڈ ڈرنک 500 ملی لیٹر", true, 4 },
                    { 61, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9818), false, null, "Cold Drink 1 LTR", false, 0m, 0L, new Guid("c19e8e8e-d748-4e97-9fa3-2b848dd95d3b"), "32f185f8-23d7-4032-beee-ad1851ee5e2f.webp", null, null, " کولڈ ڈرنک 1 لیٹر", true, 4 },
                    { 62, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9821), false, null, "Cold Drink 1.25 LTR", false, 0m, 0L, new Guid("15380e0c-8be0-4318-b454-4cea390106a5"), "648fed29-8f67-41e7-998b-b9904778a427.webp", null, null, " کولڈ ڈرنک 1.25 لیٹر", true, 4 },
                    { 63, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9824), false, null, "Cold Drink 1.5 LTR", false, 0m, 0L, new Guid("de6e31bc-eb62-47c2-8659-0b17c1b7e9ff"), "c287e571-2640-40dd-bad7-ab9e1d8e54e2.webp", null, null, " کولڈ ڈرنک 1.5 لیٹر", true, 4 },
                    { 64, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9876), false, null, "Cold Drink Jumbo 2.25 LTR", false, 0m, 0L, new Guid("1b9be434-5877-4698-82be-7771382553d8"), "db5ecb62-7e24-46a6-818b-f0603a33604c.webp", null, null, "کولڈ ڈرنک جمبو 2.25 لیٹر", true, 4 },
                    { 65, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9880), false, null, "Sting 345 ML", false, 0m, 0L, new Guid("ac486df9-d2ea-49f7-8daf-3b2c4a1fe8de"), "44d8e28b-ad99-49ae-985c-a538d1dfc185.webp", null, null, "اسٹنگ 345 ملی لیٹر", true, 4 },
                    { 66, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9883), false, null, "Sting 500 ML", false, 0m, 0L, new Guid("42e8f699-3d6e-408d-a3c1-fcb12fc7b17b"), "8023d648-242e-4c43-b14d-427e4151033e.webp", null, null, "اسٹنگ 500 ملی لیٹر", true, 4 },
                    { 67, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9886), false, null, "Water 345 ML", false, 0m, 0L, new Guid("0758c9f2-422a-42d0-b115-15175a01167b"), "54870b30-867d-46f0-aba4-4b179ea64890.webp", null, null, "پانی 345 ملی لیٹر", true, 4 },
                    { 68, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9893), false, null, "Water 500 ML", false, 0m, 0L, new Guid("dd29745d-c43d-4c07-8331-7f6c06ddb650"), "364838c6-5aa3-4264-8c1c-0561e4e173ff.webp", null, null, "پانی 500 ملی لیٹر", true, 4 },
                    { 69, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9896), false, null, "Water 1.5 LTR", false, 0m, 0L, new Guid("426f1ab5-f3f3-4dd8-bec3-669252523821"), "1884fdde-67ea-4e87-ae15-9b9b06602b76.webp", null, null, "پانی 1.5 لیٹر", true, 4 },
                    { 70, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9899), false, null, "Slice Juice", false, 0m, 0L, new Guid("886b400b-a362-4c4a-8966-92bfd306c757"), "3d761859-2c4b-45b4-9313-a59a6b3bf25a.webp", null, null, "سلائس جوس", true, 4 },
                    { 71, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9903), false, null, "Cold Drink Regular Sada", false, 0m, 0L, new Guid("8c911944-343b-4288-9834-d9715e892a5c"), "54be627c-95bd-418f-9ec7-47506108f2f7.webp", null, null, "کولڈ ڈرنک ریگولر سادہ", true, 4 },
                    { 72, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9906), false, null, "Cold Drink Regular Sting", false, 0m, 0L, new Guid("cc2e6497-7d5d-490e-ba44-62e2bb1ed87f"), "54be627c-95bd-418f-9ec7-47506108f2f7.webp", null, null, "کولڈ ڈرنک ریگولر اسٹنگ", true, 4 },
                    { 73, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9910), false, null, "Raita", false, 0m, 0L, new Guid("56efbbd0-e659-4274-8e11-417e1fc7e19e"), "bde555f6-1b39-4e6d-9eff-d5860e8dc569.webp", null, null, "رائتہ", true, 5 },
                    { 74, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9913), false, null, "Salad", false, 0m, 0L, new Guid("ea4fcf2e-4349-4c53-8901-15211d5de674"), "b81b227b-da5d-4bb4-86ff-42f2cdc019f2.webp", null, null, "سلاد", true, 5 },
                    { 75, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9917), false, null, "Lal Roti", false, 0m, 0L, new Guid("913506dd-c87f-4fdb-9aa5-8463f1a7d174"), "8f6825de-816c-4ceb-a29f-9d22634d819e.webp", null, null, "لال روٹی", true, 5 },
                    { 76, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9922), false, null, "Chae Cut", false, 0m, 0L, new Guid("035c4eac-de6e-4a32-88e1-f2e8b1b9e025"), "c5b362fd-4461-4dcf-8295-504d5feed2ba.webp", null, null, "چائے کٹ", true, 5 },
                    { 77, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9926), false, null, "Chae Full", false, 0m, 0L, new Guid("c2ee2654-eb2f-4b67-8c54-8c3d607cdf8e"), "fae017f5-a60d-4761-b188-a91280c22c02.webp", null, null, "چائے فل", true, 5 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Active", "CreatedBy", "CreatedOn", "Deleted", "DeletedAt", "DisplayOrder", "EmailAddress", "EncryptedPassword", "FullName", "Guid", "ImageName", "LastLogin", "MobileNumber", "ModifiedBy", "ModifiedOn", "RefreshToken", "RefreshTokenExpiryTime", "UserRoleId" },
                values: new object[] { 1, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 202, DateTimeKind.Local).AddTicks(9284), false, null, 1L, "fq@qureshi.com", "AMSKey:0L8DKG89f58TmRx9xvwrXg==;tW9Bqr8dgm4AhCIUo004fw==", "system", new Guid("5f291e6f-6aa2-4b79-a9a2-9b98fe9af5f1"), null, new DateTime(2025, 2, 8, 22, 42, 58, 202, DateTimeKind.Utc).AddTicks(9085), "03192847346", null, null, "kXx9/1afnLCHg3M9kJJfbv7dtiySg51xB5Nx4hevQtF2TTRuCODwLVENgzimVYIVbLlye1RsJBH788o0KXIDgQ==", new DateTime(2025, 2, 9, 22, 42, 58, 202, DateTimeKind.Utc).AddTicks(9267), 1 });

            migrationBuilder.InsertData(
                table: "PriceTypeCategories",
                columns: new[] { "Id", "Active", "ChildCategoryId", "CreatedBy", "CreatedOn", "Deleted", "DeletedAt", "DisplayOrder", "Guid", "ModifiedBy", "ModifiedOn", "Name" },
                values: new object[,]
                {
                    { 1, true, 1, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(762), false, null, 0L, new Guid("536707df-71fa-43b2-90a3-b83cff82abad"), null, null, "Chicken Biryani Single 190" },
                    { 2, true, 2, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(768), false, null, 0L, new Guid("1cd1ce67-e985-4f54-84cb-f2ce1cfb1646"), null, null, "Chicken Biryani 500gm Single Boti 230" },
                    { 3, true, 3, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(771), false, null, 0L, new Guid("cce200b2-f47c-4487-8a61-8be4a6d00e04"), null, null, "Chicken Biryani 500gm Double Boti 280" },
                    { 4, true, 4, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(774), false, null, 0L, new Guid("772e0d85-7f91-4634-b969-1dad0c0eec0c"), null, null, "Chicken Biryani 1kg 560" },
                    { 5, true, 5, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(776), false, null, 0L, new Guid("8e65ca9d-795e-4511-98ad-003d06f452bc"), null, null, "Sada Biryani Single 140" },
                    { 6, true, 6, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(778), false, null, 0L, new Guid("874a9130-3619-4633-8378-2e6f0ad280e9"), null, null, "Sada Biryani 500gm 180" },
                    { 7, true, 7, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(833), false, null, 0L, new Guid("f3614dce-171d-4624-ab0d-19171e0f0219"), null, null, "Sada Biryani 1kg 360" },
                    { 8, true, 8, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(836), false, null, 0L, new Guid("54c55b7b-2717-4626-8dfd-27ea3f0dc3e7"), null, null, "Biryani 100" },
                    { 9, true, 9, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(838), false, null, 0L, new Guid("184c3623-3908-494c-a1eb-756cc1870249"), null, null, "Biryani 150" },
                    { 10, true, 10, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(841), false, null, 0L, new Guid("34001429-211a-4aaa-91bf-3c8b4b2614a3"), null, null, "Biryani 200" },
                    { 11, true, 11, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(843), false, null, 0L, new Guid("8583a1e7-c02f-4063-aa9f-d92846b8c8c8"), null, null, "Biryani 50" },
                    { 12, true, 12, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(845), false, null, 0L, new Guid("449d5c90-5006-4679-a9a8-9e5067a5726d"), null, null, "Box 10" },
                    { 13, true, 13, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(848), false, null, 0L, new Guid("731d6550-78de-4722-9e42-4967ad0247a6"), null, null, "Biryani 300" },
                    { 14, true, 14, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(850), false, null, 0L, new Guid("dda246f0-afff-4d60-bb2b-03025119ad09"), null, null, "Biryani 400" },
                    { 15, true, 15, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(855), false, null, 0L, new Guid("72504a33-f6ca-481b-99f1-aa572eae7363"), null, null, "Biryani 500" },
                    { 16, true, 16, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(857), false, null, 0L, new Guid("ba265e7c-8652-4a21-8c83-49ac887f5926"), null, null, "Biryani 90" },
                    { 17, true, 17, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(859), false, null, 0L, new Guid("13b3954c-9043-4b98-ae8b-39352ac97c80"), null, null, "Pao Sada Pulao 100" },
                    { 18, true, 18, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(862), false, null, 0L, new Guid("25eb4717-42d4-4f83-a115-10bf4ecc8578"), null, null, "Chicken Pulao Single 190" },
                    { 19, true, 19, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(865), false, null, 0L, new Guid("b2413113-6d43-4dd1-996a-41cee048a02d"), null, null, "Chickhen Pulao 500gm Single Boti 230" },
                    { 20, true, 20, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(867), false, null, 0L, new Guid("d8895b67-f55f-41a5-b5d9-6f19de082076"), null, null, "Chickhen Pulao 500gm Double Boti 280" },
                    { 21, true, 21, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(870), false, null, 0L, new Guid("408cdefa-6149-44f4-93cf-eab8541a1997"), null, null, "Chicken Pulao 1kg 560" },
                    { 22, true, 22, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(872), false, null, 0L, new Guid("0ad4d36f-cdbd-4291-84c8-052385e5b20f"), null, null, "Sada Pulao Single 140" },
                    { 23, true, 23, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(877), false, null, 0L, new Guid("a46dbb94-4b73-465c-b5aa-25796e373f2e"), null, null, "Sada Pulao 500gm 180" },
                    { 24, true, 24, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(879), false, null, 0L, new Guid("f6dd1d97-9603-474e-97b3-a4f328cac31f"), null, null, "Sada Pulao 1kg 360" },
                    { 25, true, 25, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(882), false, null, 0L, new Guid("1266b39f-90be-4c83-960d-06f1b50686db"), null, null, "Pulao 100" },
                    { 26, true, 26, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(884), false, null, 0L, new Guid("cba3ea66-107e-4447-9c2f-bf11b369af30"), null, null, "Pulao 150" },
                    { 27, true, 27, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(886), false, null, 0L, new Guid("faf122df-be82-468c-8bb8-b5f6d6f13535"), null, null, "Pulao 200" },
                    { 28, true, 28, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(889), false, null, 0L, new Guid("6775ef3b-67bb-4624-becf-0fe9c89fdead"), null, null, "Pulao 50" },
                    { 29, true, 29, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(891), false, null, 0L, new Guid("d38fe17c-d051-447f-8edc-c0a78c3daf7b"), null, null, "Box 10" },
                    { 30, true, 30, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(894), false, null, 0L, new Guid("f5c9310e-9427-41ba-a040-b5554581c9e0"), null, null, "Pulao 300" },
                    { 31, true, 31, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(899), false, null, 0L, new Guid("86c93151-5c4c-491f-a4a9-f8f47fc7c4ee"), null, null, "Pulao 400" },
                    { 32, true, 32, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(901), false, null, 0L, new Guid("78bb3132-69ba-4ab9-aa1e-2f9abc45bc3c"), null, null, "Pulao 500" },
                    { 33, true, 33, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(904), false, null, 0L, new Guid("f10de090-bb7b-4e17-83bf-42aae2dd5824"), null, null, "Pao Sada Pulao 100" },
                    { 34, true, 34, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(907), false, null, 0L, new Guid("dda42dbc-1698-4c73-b464-b5e4a9c0ff34"), null, null, "Pao Chicken 140" },
                    { 35, true, 35, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(909), false, null, 0L, new Guid("6ccdaecb-caf5-45af-9917-a43eedfc0769"), null, null, "Karhai Half" },
                    { 36, true, 36, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(912), false, null, 0L, new Guid("f3af5062-4d3f-406d-9679-5c53266f754e"), null, null, "Karhai Full" },
                    { 37, true, 37, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(915), false, null, 0L, new Guid("a240f3a3-b377-4861-8142-d4c1cf96406d"), null, null, "Mix Sabzi Half" },
                    { 38, true, 38, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(917), false, null, 0L, new Guid("f48cad35-b856-4661-8a63-daea32ebc31a"), null, null, "Mix Sabzi Full" },
                    { 39, true, 39, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(922), false, null, 0L, new Guid("8f25f164-9d5e-4ae5-ba1d-57f7ab0dbee5"), null, null, "Chicken Kaleji Half" },
                    { 40, true, 40, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(924), false, null, 0L, new Guid("b3b685d7-4075-4d4c-af09-a65bdcd5907e"), null, null, "Chicken Kaleji Full" },
                    { 41, true, 41, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(926), false, null, 0L, new Guid("878f4e6e-4a07-4758-94ed-2db28959515a"), null, null, "Qeema Half" },
                    { 42, true, 42, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(929), false, null, 0L, new Guid("dff40ec1-fcc7-4e7d-89fd-d4bef8e15a97"), null, null, "Qeema Full" },
                    { 43, true, 43, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(983), false, null, 0L, new Guid("7db6b7d3-f917-43db-9fcc-29f199b7e9d2"), null, null, "Daal Maash Half" },
                    { 44, true, 44, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(986), false, null, 0L, new Guid("2b4febd7-889b-4a23-a10c-664995df957f"), null, null, "Daal Maash Full" },
                    { 45, true, 45, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(988), false, null, 0L, new Guid("af5a4dd4-f1f5-4bb6-b80e-2d61e9a1cdde"), null, null, "Murgh Chana Full" },
                    { 46, true, 46, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(990), false, null, 0L, new Guid("57c583af-0cde-468f-b3f2-3c481a4e2455"), null, null, "Chana Half" },
                    { 47, true, 47, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(995), false, null, 0L, new Guid("563f8973-3c91-4183-800b-02f4ca865462"), null, null, "Chana Full" },
                    { 48, true, 48, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(998), false, null, 0L, new Guid("60c91dc4-a5b3-49c9-a54f-dc88d9fdc0a3"), null, null, "Anda Timatar Half" },
                    { 49, true, 49, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1000), false, null, 0L, new Guid("d23cbd96-ac36-457a-9e20-b3a9b97d41cf"), null, null, "Anda Timatar Full" },
                    { 50, true, 50, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1003), false, null, 0L, new Guid("893a34b4-a2c5-4cf0-8bd7-49d331538198"), null, null, "Salan" },
                    { 51, true, 51, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1005), false, null, 0L, new Guid("96969000-e6dc-4463-a93d-10a54fd9722c"), null, null, "Fry" },
                    { 52, true, 52, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1007), false, null, 0L, new Guid("f7dca535-36a1-48e3-85d6-67e82664a8ad"), null, null, "Naan" },
                    { 53, true, 53, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1010), false, null, 0L, new Guid("a0b60d4e-554d-4f20-b840-6451d6d184f1"), null, null, "Laal Roti" },
                    { 54, true, 54, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1012), false, null, 0L, new Guid("f6a257b7-bbdf-4a5e-83da-15fd466ea2b3"), null, null, "Bhindi Full" },
                    { 55, true, 55, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1017), false, null, 0L, new Guid("f04e2ce3-67c7-4cb4-aeef-d06e42f584eb"), null, null, "Bhindi Half" },
                    { 56, true, 56, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1020), false, null, 0L, new Guid("43cae52b-ce68-4bf9-8126-67deead42b02"), null, null, "Aalu Palak Half" },
                    { 57, true, 57, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1022), false, null, 0L, new Guid("83f09cc3-ef7b-4a9b-ba28-bbda9684c529"), null, null, "Aalu Palak Full" },
                    { 58, true, 58, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1024), false, null, 0L, new Guid("dc6dad4c-0b89-48e4-979a-8de50237f1a6"), null, null, "Daal Chana Half" },
                    { 59, true, 59, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1027), false, null, 0L, new Guid("b5684c2e-a2f8-446c-9645-67ec9f2393d4"), null, null, "Daal Chana Full" },
                    { 60, true, 60, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1029), false, null, 0L, new Guid("7d24fed1-1aed-49c1-b7c9-fbb5afcc4749"), null, null, "Cold Drink 345 ML" },
                    { 61, true, 61, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1031), false, null, 0L, new Guid("779fde35-9ea8-4096-8612-7169bd04878e"), null, null, "Cold Drink 500 ML" },
                    { 62, true, 62, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1035), false, null, 0L, new Guid("36450609-e625-4449-8396-eeb90d33afae"), null, null, "Cold Drink 1 LTR" },
                    { 63, true, 63, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1039), false, null, 0L, new Guid("c0306dbe-8915-4d86-95b2-9294e296c642"), null, null, "Cold Drink 1.25 LTR" },
                    { 64, true, 64, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1042), false, null, 0L, new Guid("6f31b247-7811-492a-8b1b-f6f0eed64200"), null, null, "Cold Drink 1.5 LTR" },
                    { 65, true, 65, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1044), false, null, 0L, new Guid("beae84db-fd50-4d17-adfc-122869efb3c0"), null, null, "Cold Drink Jumbo 2.25 LTR" },
                    { 66, true, 66, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1046), false, null, 0L, new Guid("7eff5936-992f-46bb-8772-a7dc062b9b6b"), null, null, "Sting 345 ML" },
                    { 67, true, 67, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1048), false, null, 0L, new Guid("d875d2be-7434-474e-80a0-35c935ec3462"), null, null, "Sting 500 ML" },
                    { 68, true, 68, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1051), false, null, 0L, new Guid("938d3fc3-5164-40cb-9cb7-9256cba042ce"), null, null, "Water 345 ML" },
                    { 69, true, 69, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1053), false, null, 0L, new Guid("529c3d21-52fe-4155-9545-62a6db8bfc84"), null, null, "Water 500 ML" },
                    { 70, true, 70, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1056), false, null, 0L, new Guid("ff955d7d-58f0-46a1-9d6d-ecec59b91f0d"), null, null, "Water 1.5 LTR" },
                    { 71, true, 71, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1061), false, null, 0L, new Guid("1debc7a0-a06c-4ce6-886b-bbfc29908dd4"), null, null, "Slice Juice" },
                    { 72, true, 72, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1063), false, null, 0L, new Guid("6e970dd7-4637-463e-b2c5-0cc70e300219"), null, null, "Cold Drink Regular Sada" },
                    { 73, true, 73, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1065), false, null, 0L, new Guid("6875684c-5800-43b6-a17b-18fe1dfc3e7b"), null, null, "Cold Drink Regular Sting" },
                    { 74, true, 74, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1068), false, null, 0L, new Guid("f0724d4f-e492-49a7-abf6-be9bc3c2a1e1"), null, null, "Raita" },
                    { 75, true, 75, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1070), false, null, 0L, new Guid("d4075f8e-f162-4135-9e2e-331c27876c06"), null, null, "Salad" },
                    { 76, true, 76, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1073), false, null, 0L, new Guid("16cf9c4d-7fd2-4f28-baf7-fb96d7d71e45"), null, null, "Lal Roti" },
                    { 77, true, 77, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1075), false, null, 0L, new Guid("242bf23b-3b31-40ad-ab00-fab43b9a190f"), null, null, "Chae Cut" }
                });

            migrationBuilder.InsertData(
                table: "PriceTypes",
                columns: new[] { "Id", "Active", "CreatedBy", "CreatedOn", "Deleted", "DeletedAt", "DisplayOrder", "Guid", "ModifiedBy", "ModifiedOn", "Name", "PriceTypeCategoryId" },
                values: new object[,]
                {
                    { 1, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1926), false, null, 0L, new Guid("d767f373-0494-4859-9b71-45f4f3eb58e3"), null, null, "Biryani Rs.190", 1 },
                    { 2, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1934), false, null, 0L, new Guid("c13804fd-6f65-4862-893d-ea69e473c698"), null, null, "Biryani Rs.230", 2 },
                    { 3, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1937), false, null, 0L, new Guid("f5050abb-f360-4624-9788-a24e56e3af84"), null, null, "Biryani Rs.280", 3 },
                    { 4, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1940), false, null, 0L, new Guid("148a5c48-5fa7-48b8-8c45-7c41a6d67597"), null, null, "Biryani Rs.560", 4 },
                    { 5, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1943), false, null, 0L, new Guid("209900e1-564e-4685-ae65-f8074899b6c2"), null, null, "Biryani Rs.140", 5 },
                    { 6, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1945), false, null, 0L, new Guid("319c0dd2-06d9-4088-bf01-bf7895e3b0f1"), null, null, "Biryani Rs.180", 6 },
                    { 7, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1948), false, null, 0L, new Guid("24371524-e25d-4a55-ada2-f9aae303bd4b"), null, null, "Biryani Rs.360", 7 },
                    { 8, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1950), false, null, 0L, new Guid("468565f5-c2b9-4bfe-8bdf-684e5b2eedd7"), null, null, "Biryani Rs.100", 8 },
                    { 9, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1953), false, null, 0L, new Guid("cc46b9a8-0ce1-447a-8690-d2d62549e9e8"), null, null, "Biryani Rs.150", 9 },
                    { 10, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1958), false, null, 0L, new Guid("b1feead5-1c9d-427e-823d-d1f0647e5bb0"), null, null, "Biryani Rs.200", 10 },
                    { 11, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1961), false, null, 0L, new Guid("6544c86e-1c60-4596-bb27-a286752f1d61"), null, null, "Biryani Rs.50", 11 },
                    { 12, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1963), false, null, 0L, new Guid("e345b1d5-5c22-40e0-845d-ba7b8d0bb4cc"), null, null, "Box Rs.10", 12 },
                    { 13, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1966), false, null, 0L, new Guid("6b0f69df-0dc6-446b-b3e6-39d7ec11a541"), null, null, "Biryani Rs.300", 13 },
                    { 14, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1969), false, null, 0L, new Guid("93192243-6876-43ca-a9c1-82b902b7fa19"), null, null, "Biryani Rs.400", 14 },
                    { 15, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1972), false, null, 0L, new Guid("52c051ca-9eca-48f3-8b6d-e7bfce07cfc1"), null, null, "Biryani Rs.500", 15 },
                    { 16, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1974), false, null, 0L, new Guid("d6a89295-e19a-46ca-ba2f-9fee4d0670dc"), null, null, "Biryani Rs.90", 16 },
                    { 17, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1977), false, null, 0L, new Guid("2f797a94-5d6b-4f12-be63-f93e07ce48af"), null, null, "Pulao Rs.100", 17 },
                    { 18, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1982), false, null, 0L, new Guid("e69bc9b8-48e7-407c-b99f-df23f042fd47"), null, null, "Pulao Rs.190", 18 },
                    { 19, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1985), false, null, 0L, new Guid("f2c832a8-0f81-4d4c-b678-aa5642c560bf"), null, null, "Pulao Rs.230", 19 },
                    { 20, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1988), false, null, 0L, new Guid("75240e4c-e1fb-4d03-867b-65bd8182a0df"), null, null, "Pulao Rs.280", 20 },
                    { 21, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1990), false, null, 0L, new Guid("1a2c59e8-7374-41a7-82ca-c73c5a3a26cb"), null, null, "Pulao Rs.560", 21 },
                    { 22, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1993), false, null, 0L, new Guid("ef47993e-d09f-4442-8a1a-f9e055738fc4"), null, null, "Pulao Rs.140", 22 },
                    { 23, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1995), false, null, 0L, new Guid("52ba83ea-22cb-4710-bed0-d3c202b1c4c2"), null, null, "Pulao Rs.180", 23 },
                    { 24, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(1998), false, null, 0L, new Guid("277ce4d8-5d5d-4016-9b85-b5c5fd3b42c1"), null, null, "Pulao Rs.360", 24 },
                    { 25, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2000), false, null, 0L, new Guid("5a3d0939-8be3-4169-b0d9-27c3856272a8"), null, null, "Pulao Rs.100", 25 },
                    { 26, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2005), false, null, 0L, new Guid("f3dc5907-d40f-420a-ac26-b903d7f1d629"), null, null, "Pulao Rs.150", 26 },
                    { 27, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2057), false, null, 0L, new Guid("2acca46f-17cb-470e-8904-f5cd51f5c6ff"), null, null, "Pulao Rs.200", 27 },
                    { 28, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2061), false, null, 0L, new Guid("1da0e358-05e0-4312-8f83-f1a1647eed7b"), null, null, "Pulao Rs.50", 28 },
                    { 29, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2063), false, null, 0L, new Guid("e8805359-d335-4faf-b8af-a3f8f4dbdea6"), null, null, "Box Rs.10", 29 },
                    { 30, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2066), false, null, 0L, new Guid("d00a806a-dd30-4699-a506-2f1a10303980"), null, null, "Pulao Rs.300", 30 },
                    { 31, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2068), false, null, 0L, new Guid("786d5b20-5204-48cc-81c0-e3578108da5f"), null, null, "Pulao Rs.400", 31 },
                    { 32, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2071), false, null, 0L, new Guid("cdb6cbd5-7a3f-480f-ac15-73488657b93a"), null, null, "Pulao Rs.500", 32 },
                    { 33, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2074), false, null, 0L, new Guid("e74b9c1c-578b-455b-b1d5-ad89d23d3970"), null, null, "Pulao Rs.100", 33 },
                    { 34, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2078), false, null, 0L, new Guid("b9b69d59-d018-43d8-910c-0eef35f4913b"), null, null, "Pao Chicken Rs.140", 34 },
                    { 35, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2081), false, null, 0L, new Guid("c6dacd3c-2421-4524-93e6-57c3fe4ca4e1"), null, null, "Chicken Karhai Half Rs.150", 35 },
                    { 36, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2083), false, null, 0L, new Guid("ac126a85-112a-428f-b175-8d8403652fee"), null, null, "Chicken Karhai Full Rs.250", 36 },
                    { 37, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2086), false, null, 0L, new Guid("9244108d-c7cc-44b3-b6bc-e0c909c42a59"), null, null, "Mix Sabzi Half Rs.70", 37 },
                    { 38, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2089), false, null, 0L, new Guid("0f1599c7-7366-45fb-86e5-29f73dc297d1"), null, null, "Sabzi Full Rs.130", 38 },
                    { 39, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2091), false, null, 0L, new Guid("8084dcf3-7a38-429d-b9fd-9566157c8212"), null, null, "Kaleji Half Rs.120", 39 },
                    { 40, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2094), false, null, 0L, new Guid("8c539c74-409e-4b69-8124-52d4534bae30"), null, null, "Kaleji Full Rs.200", 40 },
                    { 41, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2096), false, null, 0L, new Guid("e7721b80-96d5-46e6-ad15-5f7b48931dae"), null, null, "Qeema Half Rs.120", 41 },
                    { 42, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2101), false, null, 0L, new Guid("628ea423-115c-4562-9183-5f2c05ac368b"), null, null, "Qeema Full Rs.200", 42 },
                    { 43, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2104), false, null, 0L, new Guid("1ce844fe-211b-4da8-a31c-bef7f64b4086"), null, null, "Daal Maash Half Rs.90", 43 },
                    { 44, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2107), false, null, 0L, new Guid("b05bc6c7-addc-4721-89d7-b0508256efc4"), null, null, "Daal Maash Full Rs.160", 44 },
                    { 45, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2109), false, null, 0L, new Guid("f7c9f687-b7cf-4da7-a67d-9c2515513b2a"), null, null, "Murgh Chana Full Rs.180", 45 },
                    { 46, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2112), false, null, 0L, new Guid("0d3331bf-e0c0-4d3b-aae2-3876b3b8719b"), null, null, "Chana Half Rs.80", 46 },
                    { 47, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2114), false, null, 0L, new Guid("69dede67-e953-4b38-b833-8d43c81e71e1"), null, null, "Chana Full Rs.130", 47 },
                    { 48, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2117), false, null, 0L, new Guid("c20c68ab-5fee-4a54-bee3-ff3696a3f301"), null, null, "Anda Timatar Half Rs.90", 48 },
                    { 49, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2119), false, null, 0L, new Guid("9391f97f-1ed7-492f-ac16-f369ed608f4d"), null, null, "Anda Timatar Full Rs.150", 49 },
                    { 50, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2124), false, null, 0L, new Guid("d40a0767-502a-41ac-bcbc-3ad16a88c6b2"), null, null, "Salan Rs.100", 50 },
                    { 51, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2126), false, null, 0L, new Guid("5bd927c5-e074-4681-a4a6-ac16219f6e51"), null, null, "Fry Rs.20", 51 },
                    { 52, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2129), false, null, 0L, new Guid("f724c051-9fd6-40eb-b20e-88866355e9ee"), null, null, "Naan Rs.25", 52 },
                    { 53, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2131), false, null, 0L, new Guid("fe1abebf-b110-41b0-87f9-53e539954275"), null, null, "Laal Roti Rs.20", 53 },
                    { 54, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2134), false, null, 0L, new Guid("373667bf-2aeb-484b-922f-d9d92d30626c"), null, null, "Bhindi Full Rs.200", 54 },
                    { 55, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2137), false, null, 0L, new Guid("8c3317c6-2465-430d-a43c-4742b855bd71"), null, null, "Bhindi Half Rs.110", 55 },
                    { 56, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2139), false, null, 0L, new Guid("0238a1b1-9d2b-4a7f-ae54-4a8c45b864a8"), null, null, "Aalu Palak Half Rs.80", 56 },
                    { 57, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2142), false, null, 0L, new Guid("c66eff54-32a0-4f77-9eed-ded644cf21ea"), null, null, "Daal Chana Half Rs.80", 57 },
                    { 58, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2146), false, null, 0L, new Guid("f606f215-462f-4745-abc9-0646afd49f43"), null, null, "Daal Chana Full Rs.130", 58 },
                    { 59, true, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(2149), false, null, 0L, new Guid("449d703f-d1ea-435a-8ab4-8e71f7a33ae6"), null, null, "Aalu Palak Full Rs.130", 59 }
                });

            migrationBuilder.InsertData(
                table: "ProductPrices",
                columns: new[] { "Id", "Active", "ChildCategoryId", "CreatedBy", "CreatedOn", "Deleted", "DeletedAt", "DisplayOrder", "Guid", "ModifiedBy", "ModifiedOn", "Price", "PriceTypeCategoryId", "PriceTypeId" },
                values: new object[,]
                {
                    { 1, true, 1, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3222), false, null, 0L, new Guid("754d0ce5-2d00-4167-99ca-2d66b44f9dc7"), null, null, 190m, 1, 1 },
                    { 2, true, 2, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3231), false, null, 0L, new Guid("69db22ae-f7f4-4152-b6cf-f9685c42d6fe"), null, null, 230m, 2, 2 },
                    { 3, true, 3, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3234), false, null, 0L, new Guid("b92a12d1-87ec-4fe5-99f2-51e201f20ab8"), null, null, 280m, 3, 3 },
                    { 4, true, 4, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3237), false, null, 0L, new Guid("7947aa76-303a-4ebc-b507-272c01591f0a"), null, null, 560m, 4, 4 },
                    { 5, true, 5, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3240), false, null, 0L, new Guid("0f485cdd-0517-45ba-9fd1-39508fed874a"), null, null, 140m, 5, 5 },
                    { 6, true, 6, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3243), false, null, 0L, new Guid("84ee1228-a07c-473d-8913-5992ba75f0af"), null, null, 180m, 6, 6 },
                    { 7, true, 7, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3250), false, null, 0L, new Guid("17d9ea67-45c1-4d29-8f27-9f36d6bea181"), null, null, 360m, 7, 7 },
                    { 8, true, 8, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3253), false, null, 0L, new Guid("e0f79565-4ccb-4b87-9edb-b070d70b93fe"), null, null, 100m, 8, 8 },
                    { 9, true, 9, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3255), false, null, 0L, new Guid("34f50f18-9bef-47fa-a610-04c4d65c89f8"), null, null, 150m, 9, 9 },
                    { 10, true, 10, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3258), false, null, 0L, new Guid("1ee85e13-0117-4cdb-b5dd-d8a23d7a65a9"), null, null, 200m, 10, 10 },
                    { 11, true, 11, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3261), false, null, 0L, new Guid("3b169b00-caa3-4822-9a54-c5ae95645bbe"), null, null, 50m, 11, 11 },
                    { 12, true, 12, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3264), false, null, 0L, new Guid("e933350c-cc53-47b4-918d-10c88f60a8b8"), null, null, 10m, 12, 12 },
                    { 13, true, 13, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3267), false, null, 0L, new Guid("425e6d26-5e2c-4533-9651-fa2487166356"), null, null, 300m, 13, 13 },
                    { 14, true, 14, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3270), false, null, 0L, new Guid("431ff4c1-9adf-4ddb-a187-aed1ed9a9c8a"), null, null, 400m, 14, 14 },
                    { 15, true, 15, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3325), false, null, 0L, new Guid("4feaf713-be2a-4266-a981-496c619fd68d"), null, null, 500m, 15, 15 },
                    { 16, true, 16, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3328), false, null, 0L, new Guid("500501fb-ba50-45f9-bbe9-a988aa790c27"), null, null, 90m, 16, 16 },
                    { 17, true, 17, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3331), false, null, 0L, new Guid("eb1ded49-263e-406e-93c8-85181c3b5c61"), null, null, 100m, 17, 17 },
                    { 18, true, 18, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3334), false, null, 0L, new Guid("58a18c77-0bcc-4098-a41b-b6085098297c"), null, null, 190m, 18, 18 },
                    { 19, true, 19, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3337), false, null, 0L, new Guid("821efe8a-32db-4488-ac18-9a679721e63d"), null, null, 230m, 19, 19 },
                    { 20, true, 20, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3340), false, null, 0L, new Guid("ec9193dd-19be-48e9-b1ce-32d6615e4775"), null, null, 280m, 20, 20 },
                    { 21, true, 21, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3343), false, null, 0L, new Guid("e563810e-253e-46c9-9ff9-e87146b6a071"), null, null, 560m, 21, 21 },
                    { 22, true, 22, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3345), false, null, 0L, new Guid("7b3d2f4a-290c-4506-a503-09265dc72c31"), null, null, 140m, 22, 22 },
                    { 23, true, 23, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3351), false, null, 0L, new Guid("abd50b35-1ded-4f39-9c0b-e2ec43af6b10"), null, null, 180m, 23, 23 },
                    { 24, true, 24, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3353), false, null, 0L, new Guid("ad23c608-901a-4a5e-a599-7fc531353da2"), null, null, 360m, 24, 24 },
                    { 25, true, 25, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3356), false, null, 0L, new Guid("5ee9e569-4ab3-413f-ad0d-46c1d7efdb89"), null, null, 100m, 25, 25 },
                    { 26, true, 26, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3359), false, null, 0L, new Guid("4105c439-3897-453e-82c0-e33c09f5f062"), null, null, 150m, 26, 26 },
                    { 27, true, 27, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3361), false, null, 0L, new Guid("6f466b3a-2864-44ff-aead-85f69d673318"), null, null, 200m, 27, 27 },
                    { 28, true, 28, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3365), false, null, 0L, new Guid("bfcbb704-c469-4665-a6bf-5e4ff7b28b56"), null, null, 50m, 28, 28 },
                    { 29, true, 29, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3367), false, null, 0L, new Guid("ce2abd6a-bfcf-4f9e-b459-01fbcec3e7db"), null, null, 10m, 29, 29 },
                    { 30, true, 30, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3371), false, null, 0L, new Guid("d9f46e54-2ba4-461c-879b-ee5fdfc4f3ce"), null, null, 300m, 30, 30 },
                    { 31, true, 31, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3376), false, null, 0L, new Guid("c6071176-cec7-47f4-a886-0a10de11627f"), null, null, 400m, 31, 31 },
                    { 32, true, 32, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3378), false, null, 0L, new Guid("b9d7df10-58a5-4e35-8f54-367998faab64"), null, null, 500m, 32, 32 },
                    { 33, true, 33, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3381), false, null, 0L, new Guid("12d6c574-64bc-4d3b-b76d-8017f3cfb55a"), null, null, 100m, 33, 33 },
                    { 34, true, 34, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3384), false, null, 0L, new Guid("d9e0a1fc-b83e-44a5-b714-c350313b4513"), null, null, 140m, 34, 34 },
                    { 35, true, 35, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3387), false, null, 0L, new Guid("a8acf41f-c0df-452a-bbda-b8c1fd326762"), null, null, 150m, 35, 35 },
                    { 36, true, 36, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3390), false, null, 0L, new Guid("00de672a-7f70-432e-be06-bbd25f280307"), null, null, 250m, 36, 36 },
                    { 37, true, 37, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3392), false, null, 0L, new Guid("4126a8a9-1119-4e95-aac9-74e9569da8d4"), null, null, 70m, 37, 37 },
                    { 38, true, 38, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3396), false, null, 0L, new Guid("e582d6d6-2db4-4f27-8425-6d702ede6f43"), null, null, 130m, 38, 38 },
                    { 39, true, 39, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3402), false, null, 0L, new Guid("b355831f-3b48-4817-9cdd-88a7316bc149"), null, null, 120m, 39, 39 },
                    { 40, true, 40, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3404), false, null, 0L, new Guid("92d3b649-9042-4e97-acd3-095dbddc58e1"), null, null, 200m, 40, 40 },
                    { 41, true, 41, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3407), false, null, 0L, new Guid("7cbf7dd1-a5c0-4336-b47d-80b5edda4f77"), null, null, 120m, 41, 41 },
                    { 42, true, 42, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3410), false, null, 0L, new Guid("20dbe06f-0fa2-4148-aa80-7462be52e5d1"), null, null, 200m, 42, 42 },
                    { 43, true, 43, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3413), false, null, 0L, new Guid("56656334-9063-47ab-a535-b9d38bba6092"), null, null, 90m, 43, 43 },
                    { 44, true, 44, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3416), false, null, 0L, new Guid("13012b9c-0ae6-4413-b274-f491ff83c801"), null, null, 160m, 44, 44 },
                    { 45, true, 45, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3419), false, null, 0L, new Guid("e54893ff-2b41-49c2-91c2-2bca328abf5a"), null, null, 180m, 45, 45 },
                    { 46, true, 46, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3422), false, null, 0L, new Guid("17ae4ea0-41a9-4024-ac7b-17377556998c"), null, null, 80m, 46, 46 },
                    { 47, true, 47, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3428), false, null, 0L, new Guid("50eacd00-a825-4047-834a-b09f83ee6ab4"), null, null, 130m, 47, 47 },
                    { 48, true, 48, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3431), false, null, 0L, new Guid("3c0ff687-47b8-4bf4-b77b-fb6825d09b48"), null, null, 90m, 48, 48 },
                    { 49, true, 49, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3484), false, null, 0L, new Guid("138b143d-a0b0-4954-8602-10abbd35c6f5"), null, null, 150m, 49, 49 },
                    { 50, true, 50, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3488), false, null, 0L, new Guid("62090ae9-c7e4-46ac-ac4d-5c3313169fa8"), null, null, 100m, 50, 50 },
                    { 51, true, 51, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3490), false, null, 0L, new Guid("c956b32c-2554-4ac6-86fe-7c6967e4ad92"), null, null, 20m, 51, 51 },
                    { 52, true, 52, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3493), false, null, 0L, new Guid("e8bcd556-dc6b-4176-ab27-8aeb4fd99b4e"), null, null, 25m, 52, 52 },
                    { 53, true, 53, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3496), false, null, 0L, new Guid("487e57c9-0aab-4b4c-88b0-7c115ad28217"), null, null, 20m, 53, 53 },
                    { 54, true, 54, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3499), false, null, 0L, new Guid("9cdaf6cd-b642-44b0-a91c-458de91a702f"), null, null, 200m, 54, 54 },
                    { 55, true, 55, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3504), false, null, 0L, new Guid("eab8e615-280a-4e19-93df-cb4f4aeb2246"), null, null, 110m, 55, 55 },
                    { 56, true, 56, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3507), false, null, 0L, new Guid("469a449e-1fd2-41a4-b4e3-1c50ea5cc48f"), null, null, 80m, 56, 56 },
                    { 57, true, 57, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3510), false, null, 0L, new Guid("167cc4d8-2523-44b5-8f27-9cb0f001764f"), null, null, 80m, 57, 57 },
                    { 58, true, 58, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3512), false, null, 0L, new Guid("99559764-23b5-4951-aa67-405dc79fdb37"), null, null, 130m, 58, 58 },
                    { 59, true, 59, null, new DateTime(2025, 2, 9, 3, 42, 58, 203, DateTimeKind.Local).AddTicks(3516), false, null, 0L, new Guid("d52fc7fe-be44-462b-a2e7-19f2539340db"), null, null, 130m, 59, 59 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChildCategories_SubCategoryId",
                table: "ChildCategories",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_ChildCategoryId",
                table: "Ingredients",
                column: "ChildCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_InventoryItemId",
                table: "Ingredients",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_SupplierId",
                table: "InventoryItems",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_InventoryItemId",
                table: "InventoryTransactions",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceTypeCategories_ChildCategoryId",
                table: "PriceTypeCategories",
                column: "ChildCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceTypes_PriceTypeCategoryId",
                table: "PriceTypes",
                column: "PriceTypeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_ChildCategoryId",
                table: "ProductPrices",
                column: "ChildCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_PriceTypeCategoryId",
                table: "ProductPrices",
                column: "PriceTypeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_PriceTypeId",
                table: "ProductPrices",
                column: "PriceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_IngredientId",
                table: "RecipeIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_RecipeId",
                table: "RecipeIngredients",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_InventoryItemId",
                table: "Recipes",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_InventoryItemId",
                table: "Stocks",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierItems_SupplierId",
                table: "SupplierItems",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHistories_UserId",
                table: "UserHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_UserPermissionId",
                table: "UserPermissions",
                column: "UserPermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRolePermissions_PermissionId",
                table: "UserRolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRolePermissions_RoleId",
                table: "UserRolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserRoleTypeId",
                table: "UserRoles",
                column: "UserRoleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserRoleId",
                table: "Users",
                column: "UserRoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryTransactions");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "ProductPrices");

            migrationBuilder.DropTable(
                name: "RecipeIngredients");

            migrationBuilder.DropTable(
                name: "ShortUrls");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "SupplierItems");

            migrationBuilder.DropTable(
                name: "UserHistories");

            migrationBuilder.DropTable(
                name: "UserRefreshTokens");

            migrationBuilder.DropTable(
                name: "UserRolePermissions");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "PriceTypes");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserPermissions");

            migrationBuilder.DropTable(
                name: "PriceTypeCategories");

            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "ChildCategories");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "UserRoleTypes");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
