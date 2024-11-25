
using Data.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Utility.Enum;
using static Utility.Helpers.Constants;
using System.Numerics;
using Data.Model.SiteCategory;
using Data.Model.General;

namespace Data.EntityFramework
{
    public static class Seeding
    {
        public static void Data(this ModelBuilder modelBuilder)
        {
            #region UserType
            var roleType1 = new UserRoleType
            {
                Id = 1,
                Guid = Guid.NewGuid(),
                Name = "Root",
                Active = true
            };
            var roleType2 = new UserRoleType
            {
                Id = 2,
                Guid = Guid.NewGuid(),
                Name = "Administrator",
                Active = true
            };
            var roleType3 = new UserRoleType
            {
                Id = 3,
                Guid = Guid.NewGuid(),
                Name = "Content Manager",
                Active = true
            };
            var roleType4 = new UserRoleType
            {
                Id = 4,
                Guid = Guid.NewGuid(),
                Name = "Account Department",
                Active = true
            };
            modelBuilder.Entity<UserRoleType>().HasData(roleType1);
            modelBuilder.Entity<UserRoleType>().HasData(roleType2);
            modelBuilder.Entity<UserRoleType>().HasData(roleType3);
            modelBuilder.Entity<UserRoleType>().HasData(roleType4);
            #endregion

            #region system default Roles
            var role1 = new UserRole
            {
                Id = 1,
                Guid = Guid.NewGuid(),
                Name = "Root",
                UserRoleTypeId = roleType1.Id,
                DisplayOrder = 1,
                Active = true,
                Deleted = false,
                CreatedBy = null,
                CreatedOn = DateTime.Now
            };

            var role2 = new UserRole
            {
                Id = 2,
                Guid = Guid.NewGuid(),
                Name = "Administrator",
                UserRoleTypeId = roleType2.Id,
                DisplayOrder = 2,
                Active = true,
                Deleted = false,
                CreatedBy = null,
                CreatedOn = DateTime.Now
            };

            var role3 = new UserRole
            {
                Id = 3,
                Guid = Guid.NewGuid(),
                Name = "Supervisor",
                UserRoleTypeId = roleType3.Id,
                DisplayOrder = 3,
                Active = true,
                Deleted = false,
                CreatedBy = null,
                CreatedOn = DateTime.Now
            };



            modelBuilder.Entity<UserRole>().HasData(role1);
            modelBuilder.Entity<UserRole>().HasData(role2);
            modelBuilder.Entity<UserRole>().HasData(role3);

            #endregion

            #region  System Default Permissions 
            var per1 = new UserPermission
            {
                Id = 1,
                Guid = Guid.NewGuid(),
                Title = "Dashboard",
                NavigationUrl = "/Home/Index",
                Icon = "fa-solid fa-table-cells-large",
                AccessList = "Allowed",
                UserPermissionId = null,
                ShowInMenu = true,
                CreatedBy = null,
                CreatedOn = DateTime.Now,
                DisplayOrder = 1,
                Active = true,
                Deleted = false
            };

            var per2 = new UserPermission
            {
                Id = 2,
                Guid = Guid.NewGuid(),
                Title = "User Management",
                NavigationUrl = "#",
                Icon = "fas fa-user-cog",
                AccessList = "Allowed",
                UserPermissionId = null,
                ShowInMenu = true,
                CreatedBy = null,
                CreatedOn = DateTime.Now,
                DisplayOrder = 2,
                Active = true,
                Deleted = false
            };
            var per3 = new UserPermission
            {
                Id = 3,
                Guid = Guid.NewGuid(),
                Title = "System Users",
                NavigationUrl = "/UserMgmt/User/UserList",
                Icon = null,
                AccessList = "Allowed,Active,List,Add,Edit,Delete",
                UserPermissionId = per2.Id,
                ShowInMenu = true,
                CreatedBy = null,
                CreatedOn = DateTime.Now,
                DisplayOrder = 3,
                Active = true,
                Deleted = false
            };
            var per4 = new UserPermission
            {
                Id = 4,
                Guid = Guid.NewGuid(),
                Title = "System Roles",
                NavigationUrl = "/UserMgmt/Role/RoleList",
                Icon = null,
                AccessList = "Allowed,Active,List,DisplayOrder,Add,Edit,Delete",
                UserPermissionId = per2.Id,
                ShowInMenu = true,
                CreatedBy = null,
                CreatedOn = DateTime.Now,
                DisplayOrder = 4,
                Active = true,
                Deleted = false
            };

            var per5 = new UserPermission
            {
                Id = 5,
                Guid = Guid.NewGuid(),
                Title = "System Permissions",
                NavigationUrl = "/UserMgmt/Permission/PermissionList",
                Icon = null,
                AccessList = "Allowed,Active,List,DisplayOrder,Add,Edit,Delete",
                UserPermissionId = per2.Id,
                ShowInMenu = true,
                CreatedBy = null,
                CreatedOn = DateTime.Now,
                DisplayOrder = 5,
                Active = true,
                Deleted = false
            };
            var per6 = new UserPermission
            {
                Id = 6,
                Guid = Guid.NewGuid(),
                Title = "Categories Menu",
                NavigationUrl = "#",
                Icon = "fa-solid fa-table-cells-large",
                AccessList = "Allowed",
                UserPermissionId = null,
                ShowInMenu = true,
                CreatedBy = null,
                CreatedOn = DateTime.Now,
                DisplayOrder = 6,
                Active = true,
                Deleted = false
            };
            var per7 = new UserPermission
            {
                Id = 7,
                Guid = Guid.NewGuid(),
                Title = "Category",
                NavigationUrl = "/Category/CategoryList",
                Icon = null,
                AccessList = "Allowed,Active,List,Add,Edit,Delete",
                UserPermissionId = per6.Id,
                ShowInMenu = true,
                CreatedBy = null,
                CreatedOn = DateTime.Now,
                DisplayOrder = 7,
                Active = true,
                Deleted = false
            };
            var per8 = new UserPermission
            {
                Id = 8,
                Guid = Guid.NewGuid(),
                Title = "Sub Category",
                NavigationUrl = "/SubCategory/SubCategoryList",
                Icon = null,
                AccessList = "Allowed,Active,List,Add,Edit,Delete",
                UserPermissionId = per6.Id,
                ShowInMenu = true,
                CreatedBy = null,
                CreatedOn = DateTime.Now,
                DisplayOrder = 8,
                Active = true,
                Deleted = false
            };
            var per9 = new UserPermission
            {
                Id = 9,
                Guid = Guid.NewGuid(),
                Title = "Child Category",
                NavigationUrl = "/ChildCategory/ChildCategoryList",
                Icon = null,
                AccessList = "Allowed,Active,List,Add,Edit,Delete",
                UserPermissionId = per6.Id,
                ShowInMenu = true,
                CreatedBy = null,
                CreatedOn = DateTime.Now,
                DisplayOrder = 9,
                Active = true,
                Deleted = false
            };
            var per10 = new UserPermission
            {
                Id = 10,
                Guid = Guid.NewGuid(),
                Title = "General Setting",
                NavigationUrl = "#",
                Icon = null,
                AccessList = "Allowed",
                ShowInMenu = true,
                CreatedBy = null,
                CreatedOn = DateTime.Now,
                DisplayOrder = 10,
                Active = true,
                Deleted = false
            };
            var per11 = new UserPermission
            {
                Id = 11,
                Guid = Guid.NewGuid(),
                Title = "Price Type Category",
                NavigationUrl = "/PriceTypeCategory/PriceTypeCategoryList",
                Icon = null,
                AccessList = "Allowed,Active,List,Add,Edit,Delete",
                UserPermissionId = per10.Id,
                ShowInMenu = true,
                CreatedBy = null,
                CreatedOn = DateTime.Now,
                DisplayOrder = 11,
                Active = true,
                Deleted = false
            };
            var per12 = new UserPermission
            {
                Id = 12,
                Guid = Guid.NewGuid(),
                Title = "Price Type",
                NavigationUrl = "/PriceType/PriceTypeList",
                Icon = null,
                AccessList = "Allowed,Active,List,Add,Edit,Delete",
                UserPermissionId = per10.Id,
                ShowInMenu = true,
                CreatedBy = null,
                CreatedOn = DateTime.Now,
                DisplayOrder = 12,
                Active = true,
                Deleted = false
            };
            var per13 = new UserPermission
            {
                Id = 13,
                Guid = Guid.NewGuid(),
                Title = "Product Price",
                NavigationUrl = "/ProductPrice/ProductPriceList",
                Icon = null,
                AccessList = "Allowed,Active,List,Add,Edit,Delete",
                UserPermissionId = per10.Id,
                ShowInMenu = true,
                CreatedBy = null,
                CreatedOn = DateTime.Now,
                DisplayOrder = 13,
                Active = true,
                Deleted = false
            };
            var per14 = new UserPermission
            {
                Id = 14,
                Guid = Guid.NewGuid(),
                Title = "Sales Report",
                NavigationUrl = "/SalesReport/SalesReportList",
                Icon = null,
                AccessList = "Allowed",
                ShowInMenu = true,
                CreatedBy = null,
                CreatedOn = DateTime.Now,
                DisplayOrder = 14,
                Active = true,
                Deleted = false
            };
            var per15 = new UserPermission
            {
                Id = 15,
                Guid = Guid.NewGuid(),
                Title = "Inventory Management",
                NavigationUrl = "#",
                Icon = null,
                AccessList = "Allowed",
                ShowInMenu = true,
                CreatedBy = null,
                CreatedOn = DateTime.Now,
                DisplayOrder = 15,
                Active = true,
                Deleted = false
            };
            var per16 = new UserPermission
            {
                Id = 16,
                Guid = Guid.NewGuid(),
                Title = "Suppliers",
                NavigationUrl = "/Supplier/SupplierList",
                Icon = null,
                AccessList = "Allowed,Active,List,Add,Edit,Delete",
                UserPermissionId = per15.Id,
                ShowInMenu = true,
                CreatedBy = null,
                CreatedOn = DateTime.Now,
                DisplayOrder = 16,
                Active = true,
                Deleted = false
            };
            var per17 = new UserPermission
            {
                Id = 17,
                Guid = Guid.NewGuid(),
                Title = "Inventory Item",
                NavigationUrl = "/InventoryItem/InventoryItemList",
                Icon = null,
                AccessList = "Allowed,Active,List,Add,Edit,Delete",
                UserPermissionId = per15.Id,
                ShowInMenu = true,
                CreatedBy = null,
                CreatedOn = DateTime.Now,
                DisplayOrder = 17,
                Active = true,
                Deleted = false
            }; 
            var per18 = new UserPermission
            {
                Id = 18,
                Guid = Guid.NewGuid(),
                Title = "Stock",
                NavigationUrl = "/Stock/StockList",
                Icon = null,
                AccessList = "Allowed,Active,List,Add,Edit,Delete",
                UserPermissionId = per15.Id,
                ShowInMenu = true,
                CreatedBy = null,
                CreatedOn = DateTime.Now,
                DisplayOrder = 18,
                Active = true,
                Deleted = false
            };

            modelBuilder.Entity<UserPermission>().HasData(per1);
            modelBuilder.Entity<UserPermission>().HasData(per2);
            modelBuilder.Entity<UserPermission>().HasData(per3);
            modelBuilder.Entity<UserPermission>().HasData(per4);
            modelBuilder.Entity<UserPermission>().HasData(per5);
            modelBuilder.Entity<UserPermission>().HasData(per6);
            modelBuilder.Entity<UserPermission>().HasData(per7);
            modelBuilder.Entity<UserPermission>().HasData(per8);
            modelBuilder.Entity<UserPermission>().HasData(per9);
            modelBuilder.Entity<UserPermission>().HasData(per10);
            modelBuilder.Entity<UserPermission>().HasData(per11);
            modelBuilder.Entity<UserPermission>().HasData(per12);
            modelBuilder.Entity<UserPermission>().HasData(per13);
            modelBuilder.Entity<UserPermission>().HasData(per14);
            modelBuilder.Entity<UserPermission>().HasData(per15);
            modelBuilder.Entity<UserPermission>().HasData(per16);
            modelBuilder.Entity<UserPermission>().HasData(per17);
            modelBuilder.Entity<UserPermission>().HasData(per18);

            modelBuilder.Entity<UserRolePermission>().HasData(
                new UserRolePermission
                {
                    Id = 1,
                    Guid = Guid.NewGuid(),
                    UserPermissionId = per1.Id,
                    UserRoleId = role1.Id,
                    Allowed = true,
                    AllowList = true,
                    AllowActive = true,
                    AllowDisplayOrder = true,
                    AllowAdd = true,
                    AllowDelete = true,
                    AllowEdit = true,
                    AllowView = true
                });

            modelBuilder.Entity<UserRolePermission>().HasData(new UserRolePermission
            {
                Id = 2,
                Guid = Guid.NewGuid(),
                UserPermissionId = per2.Id,
                UserRoleId = role1.Id,
                Allowed = true,
                AllowList = true,
                AllowActive = true,
                AllowDisplayOrder = true,
                AllowAdd = true,
                AllowDelete = true,
                AllowEdit = true,
                AllowView = true
            });

            modelBuilder.Entity<UserRolePermission>().HasData(new UserRolePermission
            {
                Id = 3,
                Guid = Guid.NewGuid(),
                UserPermissionId = per3.Id,
                UserRoleId = role1.Id,
                Allowed = true,
                AllowList = true,
                AllowActive = true,
                AllowDisplayOrder = true,
                AllowAdd = true,
                AllowDelete = true,
                AllowEdit = true,
                AllowView = true
            });

            modelBuilder.Entity<UserRolePermission>().HasData(new UserRolePermission
            {
                Id = 4,
                Guid = Guid.NewGuid(),
                UserPermissionId = per4.Id,
                UserRoleId = role1.Id,
                Allowed = true,
                AllowList = true,
                AllowActive = true,
                AllowDisplayOrder = true,
                AllowAdd = true,
                AllowDelete = true,
                AllowEdit = true,
                AllowView = true
            });

            modelBuilder.Entity<UserRolePermission>().HasData(new UserRolePermission
            {
                Id = 5,
                Guid = Guid.NewGuid(),
                UserPermissionId = per5.Id,
                UserRoleId = role1.Id,
                Allowed = true,
                AllowList = true,
                AllowActive = true,
                AllowDisplayOrder = true,
                AllowAdd = true,
                AllowDelete = true,
                AllowEdit = true,
                AllowView = true
            });
            modelBuilder.Entity<UserRolePermission>().HasData(
                new UserRolePermission
                {
                    Id = 6,
                    Guid = Guid.NewGuid(),
                    UserPermissionId = per6.Id,
                    UserRoleId = role1.Id,
                    Allowed = true,
                    AllowList = true,
                    AllowActive = true,
                    AllowDisplayOrder = true,
                    AllowAdd = true,
                    AllowDelete = true,
                    AllowEdit = true,
                    AllowView = true
                });
            modelBuilder.Entity<UserRolePermission>().HasData(
                new UserRolePermission
                {
                    Id = 7,
                    Guid = Guid.NewGuid(),
                    UserPermissionId = per7.Id,
                    UserRoleId = role1.Id,
                    Allowed = true,
                    AllowList = true,
                    AllowActive = true,
                    AllowDisplayOrder = true,
                    AllowAdd = true,
                    AllowDelete = true,
                    AllowEdit = true,
                    AllowView = true
                });
            modelBuilder.Entity<UserRolePermission>().HasData(
                new UserRolePermission
                {
                    Id = 8,
                    Guid = Guid.NewGuid(),
                    UserPermissionId = per8.Id,
                    UserRoleId = role1.Id,
                    Allowed = true,
                    AllowList = true,
                    AllowActive = true,
                    AllowDisplayOrder = true,
                    AllowAdd = true,
                    AllowDelete = true,
                    AllowEdit = true,
                    AllowView = true
                });
            modelBuilder.Entity<UserRolePermission>().HasData(
                new UserRolePermission
                {
                    Id = 9,
                    Guid = Guid.NewGuid(),
                    UserPermissionId = per9.Id,
                    UserRoleId = role1.Id,
                    Allowed = true,
                    AllowList = true,
                    AllowActive = true,
                    AllowDisplayOrder = true,
                    AllowAdd = true,
                    AllowDelete = true,
                    AllowEdit = true,
                    AllowView = true
                });
            modelBuilder.Entity<UserRolePermission>().HasData(
                new UserRolePermission
                {
                    Id = 10,
                    Guid = Guid.NewGuid(),
                    UserPermissionId = per10.Id,
                    UserRoleId = role1.Id,
                    Allowed = true,
                    AllowList = true,
                    AllowActive = true,
                    AllowDisplayOrder = true,
                    AllowAdd = true,
                    AllowDelete = true,
                    AllowEdit = true,
                    AllowView = true
                });
            modelBuilder.Entity<UserRolePermission>().HasData(
                new UserRolePermission
                {
                    Id = 11,
                    Guid = Guid.NewGuid(),
                    UserPermissionId = per11.Id,
                    UserRoleId = role1.Id,
                    Allowed = true,
                    AllowList = true,
                    AllowActive = true,
                    AllowDisplayOrder = true,
                    AllowAdd = true,
                    AllowDelete = true,
                    AllowEdit = true,
                    AllowView = true
                });
            modelBuilder.Entity<UserRolePermission>().HasData(
                new UserRolePermission
                {
                    Id = 12,
                    Guid = Guid.NewGuid(),
                    UserPermissionId = per12.Id,
                    UserRoleId = role1.Id,
                    Allowed = true,
                    AllowList = true,
                    AllowActive = true,
                    AllowDisplayOrder = true,
                    AllowAdd = true,
                    AllowDelete = true,
                    AllowEdit = true,
                    AllowView = true
                });
            modelBuilder.Entity<UserRolePermission>().HasData(
                new UserRolePermission
                {
                    Id = 13,
                    Guid = Guid.NewGuid(),
                    UserPermissionId = per13.Id,
                    UserRoleId = role1.Id,
                    Allowed = true,
                    AllowList = true,
                    AllowActive = true,
                    AllowDisplayOrder = true,
                    AllowAdd = true,
                    AllowDelete = true,
                    AllowEdit = true,
                    AllowView = true
                });
            modelBuilder.Entity<UserRolePermission>().HasData(
                new UserRolePermission
                {
                    Id = 14,
                    Guid = Guid.NewGuid(),
                    UserPermissionId = per14.Id,
                    UserRoleId = role1.Id,
                    Allowed = true,
                    AllowList = true,
                    AllowActive = true,
                    AllowDisplayOrder = true,
                    AllowAdd = true,
                    AllowDelete = true,
                    AllowEdit = true,
                    AllowView = true
                });
            modelBuilder.Entity<UserRolePermission>().HasData(
                new UserRolePermission
                {
                    Id = 15,
                    Guid = Guid.NewGuid(),
                    UserPermissionId = per15.Id,
                    UserRoleId = role1.Id,
                    Allowed = true,
                    AllowList = true,
                    AllowActive = true,
                    AllowDisplayOrder = true,
                    AllowAdd = true,
                    AllowDelete = true,
                    AllowEdit = true,
                    AllowView = true
                });
            modelBuilder.Entity<UserRolePermission>().HasData(
                new UserRolePermission
                {
                    Id = 16,
                    Guid = Guid.NewGuid(),
                    UserPermissionId = per16.Id,
                    UserRoleId = role1.Id,
                    Allowed = true,
                    AllowList = true,
                    AllowActive = true,
                    AllowDisplayOrder = true,
                    AllowAdd = true,
                    AllowDelete = true,
                    AllowEdit = true,
                    AllowView = true
                });
            modelBuilder.Entity<UserRolePermission>().HasData(
                new UserRolePermission
                {
                    Id = 17,
                    Guid = Guid.NewGuid(),
                    UserPermissionId = per17.Id,
                    UserRoleId = role1.Id,
                    Allowed = true,
                    AllowList = true,
                    AllowActive = true,
                    AllowDisplayOrder = true,
                    AllowAdd = true,
                    AllowDelete = true,
                    AllowEdit = true,
                    AllowView = true
                });
            modelBuilder.Entity<UserRolePermission>().HasData(
                new UserRolePermission
                {
                    Id = 18,
                    Guid = Guid.NewGuid(),
                    UserPermissionId = per18.Id,
                    UserRoleId = role1.Id,
                    Allowed = true,
                    AllowList = true,
                    AllowActive = true,
                    AllowDisplayOrder = true,
                    AllowAdd = true,
                    AllowDelete = true,
                    AllowEdit = true,
                    AllowView = true
                });

            #endregion

            modelBuilder.Entity<User>().HasData(
                 new User
                 {
                     Id = 1,
                     Guid = Guid.NewGuid(),
                     FullName = "system",
                     EmailAddress = "gts@gtechnosoft.com",
                     MobileNumber = "03063888546",
                     EncryptedPassword = new Utility.Helpers.EncryptionServices().EncryptString("md999"),
                     DisplayOrder = 1,
                     UserRoleId = role1.Id,
                     LastLogin = DateTime.UtcNow,
                     RefreshToken = Utility.Helpers.Common.GenerateRefreshToken(),
                     RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1),
                     Active = true,
                     Deleted = false,
                     CreatedBy = null,
                     CreatedOn = DateTime.Now
                 }
             );

            #region Categories
            var category1 = new Category
            {
                Id = 1,
                Guid = Guid.NewGuid(),
                Name = "Electronics",
                Description = "All electronic items",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0
            };

            var category2 = new Category
            {
                Id = 2,
                Guid = Guid.NewGuid(),
                Name = "Clothing",
                Description = "Apparel and accessories",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0
            };

            modelBuilder.Entity<Category>().HasData(category1);
            modelBuilder.Entity<Category>().HasData(category2);
            #endregion

            #region SubCategories
            var subCategory1 = new SubCategory
            {
                Id = 1,
                Guid = Guid.NewGuid(),
                CategoryId = category1.Id,
                Name = "Mobile Phones",
                Description = "Smartphones and accessories",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0
            };

            var subCategory2 = new SubCategory
            {
                Id = 2,
                Guid = Guid.NewGuid(),
                CategoryId = category2.Id,
                Name = "Men's Wear",
                Description = "Clothing for men",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0
            };

            var subCategory3 = new SubCategory
            {
                Id = 3,
                Guid = Guid.NewGuid(),
                CategoryId = category1.Id,
                Name = "Laptops",
                Description = "All kinds of laptops",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0
            };

            var subCategory4 = new SubCategory
            {
                Id = 4,
                Guid = Guid.NewGuid(),
                CategoryId = category2.Id,
                Name = "Women's Wear",
                Description = "Clothing for women",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0
            };

            modelBuilder.Entity<SubCategory>().HasData(subCategory1);
            modelBuilder.Entity<SubCategory>().HasData(subCategory2);
            modelBuilder.Entity<SubCategory>().HasData(subCategory3);
            modelBuilder.Entity<SubCategory>().HasData(subCategory4);
            #endregion

            #region ChildCategories
            var childCategory1 = new ChildCategory
            {
                Id = 1,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory1.Id,
                Name = "Smartphones",
                Description = "Latest smartphones",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0
            };

            var childCategory2 = new ChildCategory
            {
                Id = 2,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory2.Id,
                Name = "Casual Shirts",
                Description = "Casual shirts for men",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0
            };

            var childCategory3 = new ChildCategory
            {
                Id = 3,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = "Gaming Laptops",
                Description = "High-performance laptops for gaming",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0
            };

            var childCategory4 = new ChildCategory
            {
                Id = 4,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory4.Id,
                Name = "Formal Dresses",
                Description = "Elegant formal wear for women",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0
            };

            modelBuilder.Entity<ChildCategory>().HasData(childCategory1);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory2);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory3);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory4);
            #endregion

            #region PriceTypeCategories
            var priceTypeCategory1 = new PriceTypeCategory
            {
                Id = 1,
                Guid = Guid.NewGuid(),
                Name = "Retail Price",
                Active = true,
                ChildCategoryId = 1 // Assuming ChildCategory with Id 1 exists
            };

            var priceTypeCategory2 = new PriceTypeCategory
            {
                Id = 2,
                Guid = Guid.NewGuid(),
                Name = "Wholesale Price",
                Active = true,
                ChildCategoryId = 2 // Assuming ChildCategory with Id 2 exists
            };

            var priceTypeCategory3 = new PriceTypeCategory
            {
                Id = 3,
                Guid = Guid.NewGuid(),
                Name = "Discounted Price",
                Active = true,
                ChildCategoryId = 3 // Assuming ChildCategory with Id 3 exists
            };

            var priceTypeCategory4 = new PriceTypeCategory
            {
                Id = 4,
                Guid = Guid.NewGuid(),
                Name = "Special Price",
                Active = true,
                ChildCategoryId = 4 // Assuming ChildCategory with Id 4 exists
            };

            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory1);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory2);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory3);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory4);
            #endregion

            #region PriceTypes
            var priceType1 = new PriceType
            {
                Id = 1,
                Guid = Guid.NewGuid(),
                Name = "Standard",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory1.Id
            };

            var priceType2 = new PriceType
            {
                Id = 2,
                Guid = Guid.NewGuid(),
                Name = "Bulk",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory2.Id
            };

            var priceType3 = new PriceType
            {
                Id = 3,
                Guid = Guid.NewGuid(),
                Name = "Holiday Special",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory3.Id
            };

            var priceType4 = new PriceType
            {
                Id = 4,
                Guid = Guid.NewGuid(),
                Name = "Clearance",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory4.Id
            };

            modelBuilder.Entity<PriceType>().HasData(priceType1);
            modelBuilder.Entity<PriceType>().HasData(priceType2);
            modelBuilder.Entity<PriceType>().HasData(priceType3);
            modelBuilder.Entity<PriceType>().HasData(priceType4);
            #endregion

            #region ProductPrices
            var productPrice1 = new ProductPrice
            {
                Id = 1,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 1, // Assuming ChildCategory with Id 1 exists
                PriceTypeId = priceType1.Id,
                PriceTypeCategoryId = priceTypeCategory1.Id,
                Price = 999,
                Active = true,
            };

            var productPrice2 = new ProductPrice
            {
                Id = 2,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 2, // Assuming ChildCategory with Id 2 exists
                PriceTypeId = priceType2.Id,
                PriceTypeCategoryId = priceTypeCategory2.Id,
                Price = 1999,
                Active = true,
            };

            var productPrice3 = new ProductPrice
            {
                Id = 3,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 3, // Assuming ChildCategory with Id 3 exists
                PriceTypeId = priceType3.Id,
                PriceTypeCategoryId = priceTypeCategory3.Id,
                Price = 499,
                Active = true,
            };

            var productPrice4 = new ProductPrice
            {
                Id = 4,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 4, // Assuming ChildCategory with Id 4 exists
                PriceTypeId = priceType4.Id,
                PriceTypeCategoryId = priceTypeCategory4.Id,
                Price = 299,
                Active = true,
            };

            modelBuilder.Entity<ProductPrice>().HasData(productPrice1);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice2);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice3);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice4);
            #endregion
        }
    }
    }
