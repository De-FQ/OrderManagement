
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
                     EmailAddress = "fq@qureshi.com",
                     MobileNumber = "03192847346",
                     EncryptedPassword = new Utility.Helpers.EncryptionServices().EncryptString("Fq710"),
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
                Name = "ریسٹورنٹ مینو",
                Description = "Retaurant Menu",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "5f5fe9d8-f98a-44da-813f-31fc2aebc206.webp",

            };
            modelBuilder.Entity<Category>().HasData(category1);
            #endregion

            #region SubCategories
            var subCategory1 = new SubCategory
            {
                Id = 1,
                Guid = Guid.NewGuid(),
                CategoryId = category1.Id,
                Name = "بریانی",
                Description = "Biryani",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "3fc1eef7-2c10-4829-bae9-7988cc69e779.webp",

            };

            var subCategory2 = new SubCategory
            {
                Id = 2,
                Guid = Guid.NewGuid(),
                CategoryId = category1.Id,
                Name = "پلاو",
                Description = "Pulao",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "eeeb2d9a-79d8-49f3-99a2-28ebcc5f695f.webp",
            };

            var subCategory3 = new SubCategory
            {
                Id = 3,
                Guid = Guid.NewGuid(),
                CategoryId = category1.Id,
                Name = "سالن",
                Description = "Salan",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "bee49fe4-2215-4ee3-b2d8-24e784dfa930.webp",
            };

            var subCategory4 = new SubCategory
            {
                Id = 4,
                Guid = Guid.NewGuid(),
                CategoryId = category1.Id,
                Name = "کولڈ ڈرنک",
                Description = "Cold Drink",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "18affa30-6bb8-496d-9921-02336a9a3480.webp",
            };

            var subCategory5 = new SubCategory
            {
                Id = 5,
                Guid = Guid.NewGuid(),
                CategoryId = category1.Id,
                Name = "ایکسٹرا",
                Description = "Extra",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "2c064164-aecf-40a8-bea0-c0df28b0508a.webp",
            };

            modelBuilder.Entity<SubCategory>().HasData(subCategory1);
            modelBuilder.Entity<SubCategory>().HasData(subCategory2);
            modelBuilder.Entity<SubCategory>().HasData(subCategory3);
            modelBuilder.Entity<SubCategory>().HasData(subCategory4);
            modelBuilder.Entity<SubCategory>().HasData(subCategory5);
            #endregion

            #region ChildCategories
            var childCategory1 = new ChildCategory
            {
                Id = 1,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory1.Id,
                Name = "چکن بریانی سنگل 190",
                Description = "Chicken Biryani Single 190",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "f27056f2-969e-4760-af2d-3802962c88a7.webp",

            };

            var childCategory2 = new ChildCategory
            {
                Id = 2,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory1.Id,
                Name = "چکن بریانی 500 گرام سنگل بوٹی 230",
                Description = "Chiken Biryani 500 Gram Single Boti 230",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "7e8304fa-f496-4c69-ad39-f9db83a0f933.webp",
            };

            var childCategory3 = new ChildCategory
            {
                Id = 3,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory1.Id,
                Name = "چکن بریانی 500 گرام ڈبل بوٹی 280",
                Description = "Chickhen Biryani 500gm Double Boti 280",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "ba2f3ff2-5c4f-4f12-9b13-c95ad8fcf828.webp",
            };

            var childCategory4 = new ChildCategory
            {
                Id = 4,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory1.Id,
                Name = "چکن بریانی 1 کلو 560",
                Description = "Chicken Biryani 1kg 560",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "df1e0b2c-d0e0-4e37-bea0-429155128063.webp",
            };

            var childCategory5 = new ChildCategory
            {
                Id = 5,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory1.Id,
                Name = "سادہ بریانی سنگل 140",
                Description = "Sada Biryani Single 140",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "6f884135-fdfa-4a2f-b6ca-1f38d020713c.webp",
            };

            var childCategory6 = new ChildCategory
            {
                Id = 6,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory1.Id,
                Name = "سادہ بریانی 500 گرام 180",
                Description = "Sada Biryani 500gm 180",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "567a0a91-0b4f-4b1d-bfb8-db2df9e2ecfd.webp",
            };

            var childCategory7 = new ChildCategory
            {
                Id = 7,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory1.Id,
                Name = "سادہ بریانی 1 کلو 360",
                Description = "Sada Biryani 1kg 360",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "304fdda3-e2d2-4d5b-832f-e8a7d622c96a.webp",
            };

            var childCategory8 = new ChildCategory
            {
                Id = 8,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory1.Id,
                Name = "بریانی 100",
                Description = "Biryani 100",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "361c284b-50d9-4d79-80ff-02b9325afa1d.webp",
            };

            var childCategory9 = new ChildCategory
            {
                Id = 9,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory1.Id,
                Name = "بریانی 150",
                Description = "Biryani 150",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "6e6407b0-c4d9-4b8f-a9cf-28f4958ef69a.webp",
            };

            var childCategory10 = new ChildCategory
            {
                Id = 10,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory1.Id,
                Name = "بریانی 200",
                Description = "Biryani 200",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "ccb4d237-0bd7-4d03-94c1-707ef490ae47.webp",
            };

            var childCategory11 = new ChildCategory
            {
                Id = 11,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory1.Id,
                Name = "بریانی 50",
                Description = "Biryani 50",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "fb31e516-d59c-493d-9336-34674a47cb03.webp",
            };

            var childCategory12 = new ChildCategory
            {
                Id = 12,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory1.Id,
                Name = "باکس 10",
                Description = "Box 10",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "aa963eeb-db13-4fd3-a5a7-6b2c500f7a60.webp",
            };

            var childCategory13 = new ChildCategory
            {
                Id = 13,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory1.Id,
                Name = "بریانی 300",
                Description = "Biryani 300",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "1b0c1555-328b-4ec9-84cf-7e288f6c23b8.webp",
            };

            var childCategory14 = new ChildCategory
            {
                Id = 14,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory1.Id,
                Name = "بریانی 400",
                Description = "Biryani 400",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "e9462128-f475-4908-9838-3220902a0d45.webp",
            };

            var childCategory15 = new ChildCategory
            {
                Id = 15,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory1.Id,
                Name = "بریانی 500",
                Description = "Biryani 500",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "d896abd5-296d-4dcf-9fef-c0ffeeac562d.webp",
            };

            var childCategory16 = new ChildCategory
            {
                Id = 16,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory1.Id,
                Name = "بریانی 90",
                Description = "Biryani 90",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "139facfb-2539-43e8-937d-45a45b5f655f.webp",
            };

            var childCategory17 = new ChildCategory
            {
                Id = 17,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory1.Id,
                Name = "پاو سادہ پلاو 100",
                Description = "Pao Sada Pulao 100",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "bc42c777-2515-467a-bc43-172b85ff9b78.webp",
            };

            var childCategory18 = new ChildCategory
            {
                Id = 18,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory2.Id,
                Name = "چکن پلاو سنگل 190",
                Description = "Chicken Pulao Single 190",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "b949ace5-0900-4715-aaf9-8e93423b8f19.webp",
            };

            var childCategory19 = new ChildCategory
            {
                Id = 19,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory2.Id,
                Name = "چکن پلاو 500 گرام ڈبل بوٹی 280",
                Description = "Chickhen Pulao 500gm Double Boti 280",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "2ddb2b38-8def-497b-b4e0-ebd15e523715.webp",
            };

            var childCategory20 = new ChildCategory
            {
                Id = 20,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory2.Id,
                Name = "چکن پلاو 1 کلو 560",
                Description = "Chicken Pulao 1kg 560",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "6ff96321-81ba-4da0-b108-a107aef1f3ed.webp",
            };

            var childCategory21 = new ChildCategory
            {
                Id = 21,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory2.Id,
                Name = "سادہ پلاو سنگل 140",
                Description = "Sada Pulao Single 140",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "0bbff197-39da-4559-936e-dfb6f3d020c2.webp",
            };

            var childCategory22 = new ChildCategory
            {
                Id = 22,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory2.Id,
                Name = "سادہ پلاو 500 گرام 180",
                Description = "Sada Pulao 500gm 180",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "ce454ad2-513a-47bc-93d9-812267fa087e.webp",
            };

            var childCategory23 = new ChildCategory
            {
                Id = 23,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory2.Id,
                Name = "سادہ پلاو 1 کلو 360",
                Description = "Sada Pulao 1kg 360",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "cbcfaf75-72b2-4fc1-a445-93e837760ffd.webp",
            };

            var childCategory24 = new ChildCategory
            {
                Id = 24,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory2.Id,
                Name = "پلاو 100",
                Description = "Pulao 100",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "fc1e5a8e-5c0f-4dba-807f-fb01a52c1ff3.webp",
            };

            var childCategory25 = new ChildCategory
            {
                Id = 25,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory2.Id,
                Name = "پلاو 150",
                Description = "Pulao 150",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "6c17b2b1-7715-48de-a06b-9fe4c9033fd1.webp",
            };

            var childCategory26 = new ChildCategory
            {
                Id = 26,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory2.Id,
                Name = "پلاو 200",
                Description = "Pulao 200",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "0414aff1-3463-4c5f-9b93-de8ff44da150.webp",
            };

            var childCategory27 = new ChildCategory
            {
                Id = 27,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory2.Id,
                Name = "پلاو 50",
                Description = "Pulao 50",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "44b6cb0e-613e-490b-8223-d326a90d26d8.webp",
            };

            var childCategory28 = new ChildCategory
            {
                Id = 28,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory2.Id,
                Name = "باکس 10a",
                Description = "Box 10",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "d150861d-05ea-4849-b55c-6caa435a408a.webp",
            };

            var childCategory29 = new ChildCategory
            {
                Id = 29,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory2.Id,
                Name = "پلاو 300",
                Description = "Pulao 300",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "3e92bef7-28fb-4837-87cf-d5af923fc2f1.webp",
            };

            var childCategory30 = new ChildCategory
            {
                Id = 30,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory2.Id,
                Name = "پلاو 400",
                Description = "Pulao 400",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "81bcdea8-4565-478b-a674-c9dbef6fe3c0.webp",
            };

            var childCategory31 = new ChildCategory
            {
                Id = 31,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory2.Id,
                Name = "پلاو 500",
                Description = "Pulao 500",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "f7f9218d-10a0-4499-a3ad-aec6a83f372d.webp",
            };

            var childCategory32 = new ChildCategory
            {
                Id = 32,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory2.Id,
                Name = "پاو سادہ پلاو 100a",
                Description = "Pao Sada Pulao 100",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "4f6e8e97-cfc6-47bc-bbe3-0f1badaa206d.webp",
            };

            var childCategory33 = new ChildCategory
            {
                Id = 33,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory2.Id,
                Name = "پاو چکن 140a",
                Description = "Pao Chicken 140",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "4f6e8e97-cfc6-47bc-bbe3-0f1badaa206d.webp",
            };

            var childCategory34 = new ChildCategory
            {
                Id = 34,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = "چکن کڑاہی ہاف 150",
                Description = "Chicken Karhai Half 150",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "4f6e8e97-cfc6-47bc-bbe3-0f1badaa206d.webp",
            };

            var childCategory35 = new ChildCategory
            {
                Id = 35,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = "چکن کڑاہی فل 250",
                Description = "Chicken Karhai Full 250",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "cd9fe8ea-66d7-4a87-a4cd-c546580637c4.webp",
            };

            var childCategory36 = new ChildCategory
            {
                Id = 36,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = "مکس سبزی ہاف 70",
                Description = "Mix Sabzi Half 70",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "4387529e-6d23-4a19-a5c5-b62e93302d0c.webp",
            };

            var childCategory37 = new ChildCategory
            {
                Id = 37,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = "مکس سبزی فل 130",
                Description = "Mix Sabzi Full 130",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "35c364e2-c550-40b4-a304-67a2cf4bbd7d.webp",
            };

            var childCategory38 = new ChildCategory
            {
                Id = 38,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = "چکن کلیجی ہاف 120",
                Description = "Chicken Kaleji Half 120",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "35c364e2-c550-40b4-a304-67a2cf4bbd7d.webp",
            };

            var childCategory39 = new ChildCategory
            {
                Id = 39,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = "چکن کلیجی فل 200",
                Description = "Chicken Kaleji Full 200",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "c8d108f9-3346-4ef5-a9ae-80b3d8c700bf.webp",
            };

            var childCategory40 = new ChildCategory
            {
                Id = 40,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = "قیمہ ہاف 120",
                Description = "Qeema Half 120",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "b58faf38-d5a6-4003-b1fa-300b80066908.webp",
            };

            var childCategory41 = new ChildCategory
            {
                Id = 41,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = "قیمہ فل 200",
                Description = "Qeema Full 200",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "86b66cc6-0a40-4fb4-b590-a4acb0b93ad4.webp",
            };

            var childCategory42 = new ChildCategory
            {
                Id = 42,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = "دال ماش ہاف 90",
                Description = "Daal Maash Half 90",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "c76c0b95-f888-4981-ba59-63ee27b3a882.webp",
            };

            var childCategory43 = new ChildCategory
            {
                Id = 43,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = "دال ماش فل 160",
                Description = "Daal Maash Full 160",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "588f7db2-d641-4886-8ebc-fbcb5286a2a3.webp",
            };

            var childCategory44 = new ChildCategory
            {
                Id = 44,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = "مرغ چنا فل 180",
                Description = "Murgh Chana Full 180",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "f13008fb-1791-43b6-9671-a2028290eb51.webp",
            };

            var childCategory45 = new ChildCategory
            {
                Id = 45,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = "چنا ہاف 80",
                Description = "Chana Half 80",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "61438335-d77d-4d9f-a5e9-b370d7a6ffa1.webp",
            };

            var childCategory46 = new ChildCategory
            {
                Id = 46,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = "چنا فل 130",
                Description = "Chana Full 130",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "ab579cb1-5289-47a4-86d1-84743caa3e6a.webp",
            };

            var childCategory47 = new ChildCategory
            {
                Id = 47,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = "انڈہ ٹماٹر ہاف 90",
                Description = "Anda Tamatar Half 90",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "fdea841b-2d11-40ec-82c2-4204bdd5e381.webp",
            };

            var childCategory48 = new ChildCategory
            {
                Id = 48,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = "انڈہ ٹماٹر فل 150",
                Description = "Anda Tamatar Full 150",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "3727d6a8-4edc-4885-91f9-68b586167749.webp",
            };

            var childCategory49 = new ChildCategory
            {
                Id = 49,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = "سالان 100",
                Description = "Salan 100",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "08a2e41a-27c5-433f-ac1e-3369b316cb44.webp",
            };

            var childCategory50 = new ChildCategory
            {
                Id = 50,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = "فرائی 20",
                Description = "Fry 20",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "e1469f23-26d4-45a5-a48d-f4de2ea669d2.webp",
            };

            var childCategory51 = new ChildCategory
            {
                Id = 51,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = "نان 25",
                Description = "Naan 25",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "9b046c03-5534-41a3-aa2a-fead4206ee11.webp",
            };

            var childCategory52 = new ChildCategory
            {
                Id = 52,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = "لال روٹی 20",
                Description = "Lal Roti 20",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "cabb297e-16b1-469b-b34b-4efc893abb3a.webp",
            };

            var childCategory53 = new ChildCategory
            {
                Id = 53,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = "بھنڈی فل 200",
                Description = "Bhindi Full 200",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "1b5ccc91-ff1b-4442-9ac3-d3c5c5a78f79.webp",
            };

            var childCategory54 = new ChildCategory
            {
                Id = 54,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = "بھنڈی ہاف 110",
                Description = "Bhindi Half 110",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "d8ef1448-61fd-439c-9c1e-f55beaa5e8ce.webp",
            };

            var childCategory55 = new ChildCategory
            {
                Id = 55,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = "آلو پالک ہاف 80",
                Description = "Aalu Palak Half 80",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "d6410226-52fc-4e94-8195-e72d386127ca.webp",
            };

            var childCategory56 = new ChildCategory
            {
                Id = 56,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = " آلو پالک فل 130",
                Description = "Aalu Palak Full 130",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "924356b9-364d-4161-9208-06b87df17bce.webp",
            };

            var childCategory57 = new ChildCategory
            {
                Id = 57,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = " دال چنا ہاف 80",
                Description = "Daal Chana Half 80",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "0b8aea2c-be4a-4039-8b80-1ac70b1f4822.webp",
            };

            var childCategory58 = new ChildCategory
            {
                Id = 58,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory3.Id,
                Name = " دال چنا فل 130",
                Description = "Daal Chana Full 130",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "21a46f7a-caed-4487-8c5d-378aa576777a.webp",
            };

            var childCategory59 = new ChildCategory
            {
                Id = 59,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory4.Id,
                Name = " کولڈ ڈرنک 345 ملی لیٹر",
                Description = "Cold Drink 345 ML",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "53960d5f-9324-49ff-9b45-d07b3e0d2304.webp",
            };

            var childCategory60 = new ChildCategory
            {
                Id = 60,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory4.Id,
                Name = " کولڈ ڈرنک 500 ملی لیٹر",
                Description = "Cold Drink 500 ML",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "e707d3fc-89ea-4926-b14a-e5625edb76b0.webp",
            };

            var childCategory61 = new ChildCategory
            {
                Id = 61,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory4.Id,
                Name = " کولڈ ڈرنک 1 لیٹر",
                Description = "Cold Drink 1 LTR",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "32f185f8-23d7-4032-beee-ad1851ee5e2f.webp",
            };

            var childCategory62 = new ChildCategory
            {
                Id = 62,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory4.Id,
                Name = " کولڈ ڈرنک 1.25 لیٹر",
                Description = "Cold Drink 1.25 LTR",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "648fed29-8f67-41e7-998b-b9904778a427.webp",
            };

            var childCategory63 = new ChildCategory
            {
                Id = 63,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory4.Id,
                Name = " کولڈ ڈرنک 1.5 لیٹر",
                Description = "Cold Drink 1.5 LTR",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "c287e571-2640-40dd-bad7-ab9e1d8e54e2.webp",
            };

            var childCategory64 = new ChildCategory
            {
                Id = 64,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory4.Id,
                Name = "کولڈ ڈرنک جمبو 2.25 لیٹر",
                Description = "Cold Drink Jumbo 2.25 LTR",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "db5ecb62-7e24-46a6-818b-f0603a33604c.webp",
            };

            var childCategory65 = new ChildCategory
            {
                Id = 65,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory4.Id,
                Name = "اسٹنگ 345 ملی لیٹر",
                Description = "Sting 345 ML",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "44d8e28b-ad99-49ae-985c-a538d1dfc185.webp",
            };

            var childCategory66 = new ChildCategory
            {
                Id = 66,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory4.Id,
                Name = "اسٹنگ 500 ملی لیٹر",
                Description = "Sting 500 ML",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "8023d648-242e-4c43-b14d-427e4151033e.webp",
            };

            var childCategory67 = new ChildCategory
            {
                Id = 67,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory4.Id,
                Name = "پانی 345 ملی لیٹر",
                Description = "Water 345 ML",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "54870b30-867d-46f0-aba4-4b179ea64890.webp",
            };

            var childCategory68 = new ChildCategory
            {
                Id = 68,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory4.Id,
                Name = "پانی 500 ملی لیٹر",
                Description = "Water 500 ML",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "364838c6-5aa3-4264-8c1c-0561e4e173ff.webp",
            };

            var childCategory69 = new ChildCategory
            {
                Id = 69,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory4.Id,
                Name = "پانی 1.5 لیٹر",
                Description = "Water 1.5 LTR",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "1884fdde-67ea-4e87-ae15-9b9b06602b76.webp",
            };

            var childCategory70 = new ChildCategory
            {
                Id = 70,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory4.Id,
                Name = "سلائس جوس",
                Description = "Slice Juice",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "3d761859-2c4b-45b4-9313-a59a6b3bf25a.webp",
            };

            var childCategory71 = new ChildCategory
            {
                Id = 71,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory4.Id,
                Name = "کولڈ ڈرنک ریگولر سادہ",
                Description = "Cold Drink Regular Sada",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "54be627c-95bd-418f-9ec7-47506108f2f7.webp",
            };

            var childCategory72 = new ChildCategory
            {
                Id = 72,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory4.Id,
                Name = "کولڈ ڈرنک ریگولر اسٹنگ",
                Description = "Cold Drink Regular Sting",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "54be627c-95bd-418f-9ec7-47506108f2f7.webp",
            };

            var childCategory73 = new ChildCategory
            {
                Id = 73,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory5.Id,
                Name = "رائتہ",
                Description = "Raita",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "bde555f6-1b39-4e6d-9eff-d5860e8dc569.webp",
            };

            var childCategory74 = new ChildCategory
            {
                Id = 74,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory5.Id,
                Name = "سلاد",
                Description = "Salad",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "b81b227b-da5d-4bb4-86ff-42f2cdc019f2.webp",
            };

            var childCategory75 = new ChildCategory
            {
                Id = 75,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory5.Id,
                Name = "لال روٹی",
                Description = "Lal Roti",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "8f6825de-816c-4ceb-a29f-9d22634d819e.webp",
            };

            var childCategory76 = new ChildCategory
            {
                Id = 76,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory5.Id,
                Name = "چائے کٹ",
                Description = "Chae Cut",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "c5b362fd-4461-4dcf-8295-504d5feed2ba.webp",
            };

            var childCategory77 = new ChildCategory
            {
                Id = 77,
                Guid = Guid.NewGuid(),
                SubCategoryId = subCategory5.Id,
                Name = "چائے فل",
                Description = "Chae Full",
                ShowInMenu = true,
                Active = true,
                DiscountActive = false,
                DiscountPercentage = 0,
                ImageName = "fae017f5-a60d-4761-b188-a91280c22c02.webp",
            };

            modelBuilder.Entity<ChildCategory>().HasData(childCategory1);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory2);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory3);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory4);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory5);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory6);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory7);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory8);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory9);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory10);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory11);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory12);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory13);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory14);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory15);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory16);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory17);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory18);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory19);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory20);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory21);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory22);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory23);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory24);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory25);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory26);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory27);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory28);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory29);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory30);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory31);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory32);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory33);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory34);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory35);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory36);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory37);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory38);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory39);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory40);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory41);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory42);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory43);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory44);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory45);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory46);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory47);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory48);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory49);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory50);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory51);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory52);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory53);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory54);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory55);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory56);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory57);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory58);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory59);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory60);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory61);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory62);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory63);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory64);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory65);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory66);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory67);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory68);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory69);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory70);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory71);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory72);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory73);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory74);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory75);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory76);
            modelBuilder.Entity<ChildCategory>().HasData(childCategory77);
            #endregion

            #region PriceTypeCategories
            var priceTypeCategory1 = new PriceTypeCategory
            {
                Id = 1,
                Guid = Guid.NewGuid(),
                Name = "Chicken Biryani Single 190",
                Active = true,
                ChildCategoryId = 1
            };

            var priceTypeCategory2 = new PriceTypeCategory
            {
                Id = 2,
                Guid = Guid.NewGuid(),
                Name = "Chicken Biryani 500gm Single Boti 230",
                Active = true,
                ChildCategoryId = 2
            };

            var priceTypeCategory3 = new PriceTypeCategory
            {
                Id = 3,
                Guid = Guid.NewGuid(),
                Name = "Chicken Biryani 500gm Double Boti 280",
                Active = true,
                ChildCategoryId = 3
            };

            var priceTypeCategory4 = new PriceTypeCategory
            {
                Id = 4,
                Guid = Guid.NewGuid(),
                Name = "Chicken Biryani 1kg 560",
                Active = true,
                ChildCategoryId = 4
            };

            var priceTypeCategory5 = new PriceTypeCategory
            {
                Id = 5,
                Guid = Guid.NewGuid(),
                Name = "Sada Biryani Single 140",
                Active = true,
                ChildCategoryId = 5
            };

            var priceTypeCategory6 = new PriceTypeCategory
            {
                Id = 6,
                Guid = Guid.NewGuid(),
                Name = "Sada Biryani 500gm 180",
                Active = true,
                ChildCategoryId = 6
            };

            var priceTypeCategory7 = new PriceTypeCategory
            {
                Id = 7,
                Guid = Guid.NewGuid(),
                Name = "Sada Biryani 1kg 360",
                Active = true,
                ChildCategoryId = 7
            };

            var priceTypeCategory8 = new PriceTypeCategory
            {
                Id = 8,
                Guid = Guid.NewGuid(),
                Name = "Biryani 100",
                Active = true,
                ChildCategoryId = 8
            };

            var priceTypeCategory9 = new PriceTypeCategory
            {
                Id = 9,
                Guid = Guid.NewGuid(),
                Name = "Biryani 150",
                Active = true,
                ChildCategoryId = 9
            };

            var priceTypeCategory10 = new PriceTypeCategory
            {
                Id = 10,
                Guid = Guid.NewGuid(),
                Name = "Biryani 200",
                Active = true,
                ChildCategoryId = 10
            };

            var priceTypeCategory11 = new PriceTypeCategory
            {
                Id = 11,
                Guid = Guid.NewGuid(),
                Name = "Biryani 50",
                Active = true,
                ChildCategoryId = 11
            };

            var priceTypeCategory12 = new PriceTypeCategory
            {
                Id = 12,
                Guid = Guid.NewGuid(),
                Name = "Box 10",
                Active = true,
                ChildCategoryId = 12
            };

            var priceTypeCategory13 = new PriceTypeCategory
            {
                Id = 13,
                Guid = Guid.NewGuid(),
                Name = "Biryani 300",
                Active = true,
                ChildCategoryId = 13
            };

            var priceTypeCategory14 = new PriceTypeCategory
            {
                Id = 14,
                Guid = Guid.NewGuid(),
                Name = "Biryani 400",
                Active = true,
                ChildCategoryId = 14
            };

            var priceTypeCategory15 = new PriceTypeCategory
            {
                Id = 15,
                Guid = Guid.NewGuid(),
                Name = "Biryani 500",
                Active = true,
                ChildCategoryId = 15
            };

            var priceTypeCategory16 = new PriceTypeCategory
            {
                Id = 16,
                Guid = Guid.NewGuid(),
                Name = "Biryani 90",
                Active = true,
                ChildCategoryId = 16
            };

            var priceTypeCategory17 = new PriceTypeCategory
            {
                Id = 17,
                Guid = Guid.NewGuid(),
                Name = "Pao Sada Pulao 100",
                Active = true,
                ChildCategoryId = 17
            };

            var priceTypeCategory18 = new PriceTypeCategory
            {
                Id = 18,
                Guid = Guid.NewGuid(),
                Name = "Chicken Pulao Single 190",
                Active = true,
                ChildCategoryId = 18
            };

            var priceTypeCategory19 = new PriceTypeCategory
            {
                Id = 19,
                Guid = Guid.NewGuid(),
                Name = "Chickhen Pulao 500gm Single Boti 230",
                Active = true,
                ChildCategoryId = 19
            };

            var priceTypeCategory20 = new PriceTypeCategory
            {
                Id = 20,
                Guid = Guid.NewGuid(),
                Name = "Chickhen Pulao 500gm Double Boti 280",
                Active = true,
                ChildCategoryId = 20
            };

            var priceTypeCategory21 = new PriceTypeCategory
            {
                Id = 21,
                Guid = Guid.NewGuid(),
                Name = "Chicken Pulao 1kg 560",
                Active = true,
                ChildCategoryId = 21
            };

            var priceTypeCategory22 = new PriceTypeCategory
            {
                Id = 22,
                Guid = Guid.NewGuid(),
                Name = "Sada Pulao Single 140",
                Active = true,
                ChildCategoryId = 22
            };

            var priceTypeCategory23 = new PriceTypeCategory
            {
                Id = 23,
                Guid = Guid.NewGuid(),
                Name = "Sada Pulao 500gm 180",
                Active = true,
                ChildCategoryId = 23
            };

            var priceTypeCategory24 = new PriceTypeCategory
            {
                Id = 24,
                Guid = Guid.NewGuid(),
                Name = "Sada Pulao 1kg 360",
                Active = true,
                ChildCategoryId = 24
            };

            var priceTypeCategory25 = new PriceTypeCategory
            {
                Id = 25,
                Guid = Guid.NewGuid(),
                Name = "Pulao 100",
                Active = true,
                ChildCategoryId = 25
            };

            var priceTypeCategory26 = new PriceTypeCategory
            {
                Id = 26,
                Guid = Guid.NewGuid(),
                Name = "Pulao 150",
                Active = true,
                ChildCategoryId = 26
            };

            var priceTypeCategory27 = new PriceTypeCategory
            {
                Id = 27,
                Guid = Guid.NewGuid(),
                Name = "Pulao 200",
                Active = true,
                ChildCategoryId = 27
            };

            var priceTypeCategory28 = new PriceTypeCategory
            {
                Id = 28,
                Guid = Guid.NewGuid(),
                Name = "Pulao 50",
                Active = true,
                ChildCategoryId = 28
            };

            var priceTypeCategory29 = new PriceTypeCategory
            {
                Id = 29,
                Guid = Guid.NewGuid(),
                Name = "Box 10",
                Active = true,
                ChildCategoryId = 29
            };

            var priceTypeCategory30 = new PriceTypeCategory
            {
                Id = 30,
                Guid = Guid.NewGuid(),
                Name = "Pulao 300",
                Active = true,
                ChildCategoryId = 30
            };

            var priceTypeCategory31 = new PriceTypeCategory
            {
                Id = 31,
                Guid = Guid.NewGuid(),
                Name = "Pulao 400",
                Active = true,
                ChildCategoryId = 31
            };

            var priceTypeCategory32 = new PriceTypeCategory
            {
                Id = 32,
                Guid = Guid.NewGuid(),
                Name = "Pulao 500",
                Active = true,
                ChildCategoryId = 32
            };

            var priceTypeCategory33 = new PriceTypeCategory
            {
                Id = 33,
                Guid = Guid.NewGuid(),
                Name = "Pao Sada Pulao 100",
                Active = true,
                ChildCategoryId = 33
            };

            var priceTypeCategory34 = new PriceTypeCategory
            {
                Id = 34,
                Guid = Guid.NewGuid(),
                Name = "Pao Chicken 140",
                Active = true,
                ChildCategoryId = 34
            };

            var priceTypeCategory35 = new PriceTypeCategory
            {
                Id = 35,
                Guid = Guid.NewGuid(),
                Name = "Karhai Half",
                Active = true,
                ChildCategoryId = 35
            };

            var priceTypeCategory36 = new PriceTypeCategory
            {
                Id = 36,
                Guid = Guid.NewGuid(),
                Name = "Karhai Full",
                Active = true,
                ChildCategoryId = 36
            };

            var priceTypeCategory37 = new PriceTypeCategory
            {
                Id = 37,
                Guid = Guid.NewGuid(),
                Name = "Mix Sabzi Half",
                Active = true,
                ChildCategoryId = 37
            };

            var priceTypeCategory38 = new PriceTypeCategory
            {
                Id = 38,
                Guid = Guid.NewGuid(),
                Name = "Mix Sabzi Full",
                Active = true,
                ChildCategoryId = 38
            };

            var priceTypeCategory39 = new PriceTypeCategory
            {
                Id = 39,
                Guid = Guid.NewGuid(),
                Name = "Chicken Kaleji Half",
                Active = true,
                ChildCategoryId = 39
            };

            var priceTypeCategory40 = new PriceTypeCategory
            {
                Id = 40,
                Guid = Guid.NewGuid(),
                Name = "Chicken Kaleji Full",
                Active = true,
                ChildCategoryId = 40
            };

            var priceTypeCategory41 = new PriceTypeCategory
            {
                Id = 41,
                Guid = Guid.NewGuid(),
                Name = "Qeema Half",
                Active = true,
                ChildCategoryId = 41
            };

            var priceTypeCategory42 = new PriceTypeCategory
            {
                Id = 42,
                Guid = Guid.NewGuid(),
                Name = "Qeema Full",
                Active = true,
                ChildCategoryId = 42
            };

            var priceTypeCategory43 = new PriceTypeCategory
            {
                Id = 43,
                Guid = Guid.NewGuid(),
                Name = "Daal Maash Half",
                Active = true,
                ChildCategoryId = 43
            };

            var priceTypeCategory44 = new PriceTypeCategory
            {
                Id = 44,
                Guid = Guid.NewGuid(),
                Name = "Daal Maash Full",
                Active = true,
                ChildCategoryId = 44
            };

            var priceTypeCategory45 = new PriceTypeCategory
            {
                Id = 45,
                Guid = Guid.NewGuid(),
                Name = "Murgh Chana Full",
                Active = true,
                ChildCategoryId = 45
            };

            var priceTypeCategory46 = new PriceTypeCategory
            {
                Id = 46,
                Guid = Guid.NewGuid(),
                Name = "Chana Half",
                Active = true,
                ChildCategoryId = 46
            };

            var priceTypeCategory47 = new PriceTypeCategory
            {
                Id = 47,
                Guid = Guid.NewGuid(),
                Name = "Chana Full",
                Active = true,
                ChildCategoryId = 47
            };

            var priceTypeCategory48 = new PriceTypeCategory
            {
                Id = 48,
                Guid = Guid.NewGuid(),
                Name = "Anda Timatar Half",
                Active = true,
                ChildCategoryId = 48
            };

            var priceTypeCategory49 = new PriceTypeCategory
            {
                Id = 49,
                Guid = Guid.NewGuid(),
                Name = "Anda Timatar Full",
                Active = true,
                ChildCategoryId = 49
            };

            var priceTypeCategory50 = new PriceTypeCategory
            {
                Id = 50,
                Guid = Guid.NewGuid(),
                Name = "Salan",
                Active = true,
                ChildCategoryId = 50
            };

            var priceTypeCategory51 = new PriceTypeCategory
            {
                Id = 51,
                Guid = Guid.NewGuid(),
                Name = "Fry",
                Active = true,
                ChildCategoryId = 51
            };

            var priceTypeCategory52 = new PriceTypeCategory
            {
                Id = 52,
                Guid = Guid.NewGuid(),
                Name = "Naan",
                Active = true,
                ChildCategoryId = 52
            };

            var priceTypeCategory53 = new PriceTypeCategory
            {
                Id = 53,
                Guid = Guid.NewGuid(),
                Name = "Laal Roti",
                Active = true,
                ChildCategoryId = 53
            };

            var priceTypeCategory54 = new PriceTypeCategory
            {
                Id = 54,
                Guid = Guid.NewGuid(),
                Name = "Bhindi Full",
                Active = true,
                ChildCategoryId = 54
            };

            var priceTypeCategory55 = new PriceTypeCategory
            {
                Id = 55,
                Guid = Guid.NewGuid(),
                Name = "Bhindi Half",
                Active = true,
                ChildCategoryId = 55
            };

            var priceTypeCategory56 = new PriceTypeCategory
            {
                Id = 56,
                Guid = Guid.NewGuid(),
                Name = "Aalu Palak Half",
                Active = true,
                ChildCategoryId = 56
            };

            var priceTypeCategory57 = new PriceTypeCategory
            {
                Id = 57,
                Guid = Guid.NewGuid(),
                Name = "Aalu Palak Full",
                Active = true,
                ChildCategoryId = 57
            };

            var priceTypeCategory58 = new PriceTypeCategory
            {
                Id = 58,
                Guid = Guid.NewGuid(),
                Name = "Daal Chana Half",
                Active = true,
                ChildCategoryId = 58
            };

            var priceTypeCategory59 = new PriceTypeCategory
            {
                Id = 59,
                Guid = Guid.NewGuid(),
                Name = "Daal Chana Full",
                Active = true,
                ChildCategoryId = 59
            };

            var priceTypeCategory60 = new PriceTypeCategory
            {
                Id = 60,
                Guid = Guid.NewGuid(),
                Name = "Cold Drink 345 ML",
                Active = true,
                ChildCategoryId = 60
            };

            var priceTypeCategory61 = new PriceTypeCategory
            {
                Id = 61,
                Guid = Guid.NewGuid(),
                Name = "Cold Drink 500 ML",
                Active = true,
                ChildCategoryId = 61
            };

            var priceTypeCategory62 = new PriceTypeCategory
            {
                Id = 62,
                Guid = Guid.NewGuid(),
                Name = "Cold Drink 1 LTR",
                Active = true,
                ChildCategoryId = 62
            };

            var priceTypeCategory63 = new PriceTypeCategory
            {
                Id = 63,
                Guid = Guid.NewGuid(),
                Name = "Cold Drink 1.25 LTR",
                Active = true,
                ChildCategoryId = 63
            };

            var priceTypeCategory64 = new PriceTypeCategory
            {
                Id = 64,
                Guid = Guid.NewGuid(),
                Name = "Cold Drink 1.5 LTR",
                Active = true,
                ChildCategoryId = 64
            };

            var priceTypeCategory65 = new PriceTypeCategory
            {
                Id = 65,
                Guid = Guid.NewGuid(),
                Name = "Cold Drink Jumbo 2.25 LTR",
                Active = true,
                ChildCategoryId = 65
            };

            var priceTypeCategory66 = new PriceTypeCategory
            {
                Id = 66,
                Guid = Guid.NewGuid(),
                Name = "Sting 345 ML",
                Active = true,
                ChildCategoryId = 66
            };

            var priceTypeCategory67 = new PriceTypeCategory
            {
                Id = 67,
                Guid = Guid.NewGuid(),
                Name = "Sting 500 ML",
                Active = true,
                ChildCategoryId = 67
            };

            var priceTypeCategory68 = new PriceTypeCategory
            {
                Id = 68,
                Guid = Guid.NewGuid(),
                Name = "Water 345 ML",
                Active = true,
                ChildCategoryId = 68
            };

            var priceTypeCategory69 = new PriceTypeCategory
            {
                Id = 69,
                Guid = Guid.NewGuid(),
                Name = "Water 500 ML",
                Active = true,
                ChildCategoryId = 69
            };

            var priceTypeCategory70 = new PriceTypeCategory
            {
                Id = 70,
                Guid = Guid.NewGuid(),
                Name = "Water 1.5 LTR",
                Active = true,
                ChildCategoryId = 70
            };

            var priceTypeCategory71 = new PriceTypeCategory
            {
                Id = 71,
                Guid = Guid.NewGuid(),
                Name = "Slice Juice",
                Active = true,
                ChildCategoryId = 71
            };

            var priceTypeCategory72 = new PriceTypeCategory
            {
                Id = 72,
                Guid = Guid.NewGuid(),
                Name = "Cold Drink Regular Sada",
                Active = true,
                ChildCategoryId = 72
            };

            var priceTypeCategory73 = new PriceTypeCategory
            {
                Id = 73,
                Guid = Guid.NewGuid(),
                Name = "Cold Drink Regular Sting",
                Active = true,
                ChildCategoryId = 73
            };

            var priceTypeCategory74 = new PriceTypeCategory
            {
                Id = 74,
                Guid = Guid.NewGuid(),
                Name = "Raita",
                Active = true,
                ChildCategoryId = 74
            };

            var priceTypeCategory75 = new PriceTypeCategory
            {
                Id = 75,
                Guid = Guid.NewGuid(),
                Name = "Salad",
                Active = true,
                ChildCategoryId = 75
            };

            var priceTypeCategory76 = new PriceTypeCategory
            {
                Id = 76,
                Guid = Guid.NewGuid(),
                Name = "Lal Roti",
                Active = true,
                ChildCategoryId = 76
            };

            var priceTypeCategory77 = new PriceTypeCategory
            {
                Id = 77,
                Guid = Guid.NewGuid(),
                Name = "Chae Cut",
                Active = true,
                ChildCategoryId = 77
            };


            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory1);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory2);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory3);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory4);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory5);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory6);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory7);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory8);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory9);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory10);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory11);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory12);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory13);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory14);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory15);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory16);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory17);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory18);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory19);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory20);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory21);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory22);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory23);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory24);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory25);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory26);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory27);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory28);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory29);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory30);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory31);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory32);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory33);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory34);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory35);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory36);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory37);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory38);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory39);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory40);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory41);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory42);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory43);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory44);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory45);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory46);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory47);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory48);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory49);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory50);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory51);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory52);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory53);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory54);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory55);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory56);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory57);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory58);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory59);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory60);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory61);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory62);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory63);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory64);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory65);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory66);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory67);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory68);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory69);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory70);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory71);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory72);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory73);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory74);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory75);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory76);
            modelBuilder.Entity<PriceTypeCategory>().HasData(priceTypeCategory77);

            #endregion

            #region PriceTypes
            var priceType1 = new PriceType
            {
                Id = 1,
                Guid = Guid.NewGuid(),
                Name = "Biryani Rs.190",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory1.Id
            };

            var priceType2 = new PriceType
            {
                Id = 2,
                Guid = Guid.NewGuid(),
                Name = "Biryani Rs.230",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory2.Id
            };

            var priceType3 = new PriceType
            {
                Id = 3,
                Guid = Guid.NewGuid(),
                Name = "Biryani Rs.280",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory3.Id
            };

            var priceType4 = new PriceType
            {
                Id = 4,
                Guid = Guid.NewGuid(),
                Name = "Biryani Rs.560",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory4.Id
            };

            var priceType5 = new PriceType
            {
                Id = 5,
                Guid = Guid.NewGuid(),
                Name = "Biryani Rs.140",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory5.Id
            };

            var priceType6 = new PriceType
            {
                Id = 6,
                Guid = Guid.NewGuid(),
                Name = "Biryani Rs.180",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory6.Id
            };

            var priceType7 = new PriceType
            {
                Id = 7,
                Guid = Guid.NewGuid(),
                Name = "Biryani Rs.360",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory7.Id
            };

            var priceType8 = new PriceType
            {
                Id = 8,
                Guid = Guid.NewGuid(),
                Name = "Biryani Rs.100",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory8.Id
            };

            var priceType9 = new PriceType
            {
                Id = 9,
                Guid = Guid.NewGuid(),
                Name = "Biryani Rs.150",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory9.Id
            };

            var priceType10 = new PriceType
            {
                Id = 10,
                Guid = Guid.NewGuid(),
                Name = "Biryani Rs.200",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory10.Id
            };

            var priceType11 = new PriceType
            {
                Id = 11,
                Guid = Guid.NewGuid(),
                Name = "Biryani Rs.50",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory11.Id
            };

            var priceType12 = new PriceType
            {
                Id = 12,
                Guid = Guid.NewGuid(),
                Name = "Box Rs.10",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory12.Id
            };

            var priceType13 = new PriceType
            {
                Id = 13,
                Guid = Guid.NewGuid(),
                Name = "Biryani Rs.300",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory13.Id
            };

            var priceType14 = new PriceType
            {
                Id = 14,
                Guid = Guid.NewGuid(),
                Name = "Biryani Rs.400",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory14.Id
            };

            var priceType15 = new PriceType
            {
                Id = 15,
                Guid = Guid.NewGuid(),
                Name = "Biryani Rs.500",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory15.Id
            };

            var priceType16 = new PriceType
            {
                Id = 16,
                Guid = Guid.NewGuid(),
                Name = "Biryani Rs.90",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory16.Id
            };

            var priceType17 = new PriceType
            {
                Id = 17,
                Guid = Guid.NewGuid(),
                Name = "Pulao Rs.100",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory17.Id
            };

            var priceType18 = new PriceType
            {
                Id = 18,
                Guid = Guid.NewGuid(),
                Name = "Pulao Rs.190",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory18.Id
            };

            var priceType19 = new PriceType
            {
                Id = 19,
                Guid = Guid.NewGuid(),
                Name = "Pulao Rs.230",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory19.Id
            };

            var priceType20 = new PriceType
            {
                Id = 20,
                Guid = Guid.NewGuid(),
                Name = "Pulao Rs.280",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory20.Id
            };

            var priceType21 = new PriceType
            {
                Id = 21,
                Guid = Guid.NewGuid(),
                Name = "Pulao Rs.560",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory21.Id
            };

            var priceType22 = new PriceType
            {
                Id = 22,
                Guid = Guid.NewGuid(),
                Name = "Pulao Rs.140",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory22.Id
            };

            var priceType23 = new PriceType
            {
                Id = 23,
                Guid = Guid.NewGuid(),
                Name = "Pulao Rs.180",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory23.Id
            };

            var priceType24 = new PriceType
            {
                Id = 24,
                Guid = Guid.NewGuid(),
                Name = "Pulao Rs.360",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory24.Id
            };

            var priceType25 = new PriceType
            {
                Id = 25,
                Guid = Guid.NewGuid(),
                Name = "Pulao Rs.100",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory25.Id
            };

            var priceType26 = new PriceType
            {
                Id = 26,
                Guid = Guid.NewGuid(),
                Name = "Pulao Rs.150",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory26.Id
            };

            var priceType27 = new PriceType
            {
                Id = 27,
                Guid = Guid.NewGuid(),
                Name = "Pulao Rs.200",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory27.Id
            };

            var priceType28 = new PriceType
            {
                Id = 28,
                Guid = Guid.NewGuid(),
                Name = "Pulao Rs.50",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory28.Id
            };

            var priceType29 = new PriceType
            {
                Id = 29,
                Guid = Guid.NewGuid(),
                Name = "Box Rs.10",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory29.Id
            };

            var priceType30 = new PriceType
            {
                Id = 30,
                Guid = Guid.NewGuid(),
                Name = "Pulao Rs.300",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory30.Id
            };

            var priceType31 = new PriceType
            {
                Id = 31,
                Guid = Guid.NewGuid(),
                Name = "Pulao Rs.400",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory31.Id
            };

            var priceType32 = new PriceType
            {
                Id = 32,
                Guid = Guid.NewGuid(),
                Name = "Pulao Rs.500",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory32.Id
            };

            var priceType33 = new PriceType
            {
                Id = 33,
                Guid = Guid.NewGuid(),
                Name = "Pulao Rs.100",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory33.Id
            };

            var priceType34 = new PriceType
            {
                Id = 34,
                Guid = Guid.NewGuid(),
                Name = "Pao Chicken Rs.140",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory34.Id
            };

            var priceType35 = new PriceType
            {
                Id = 35,
                Guid = Guid.NewGuid(),
                Name = "Chicken Karhai Half Rs.150",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory35.Id
            };

            var priceType36 = new PriceType
            {
                Id = 36,
                Guid = Guid.NewGuid(),
                Name = "Chicken Karhai Full Rs.250",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory36.Id
            };

            var priceType37 = new PriceType
            {
                Id = 37,
                Guid = Guid.NewGuid(),
                Name = "Mix Sabzi Half Rs.70",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory37.Id
            };

            var priceType38 = new PriceType
            {
                Id = 38,
                Guid = Guid.NewGuid(),
                Name = "Sabzi Full Rs.130",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory38.Id
            };

            var priceType39 = new PriceType
            {
                Id = 39,
                Guid = Guid.NewGuid(),
                Name = "Kaleji Half Rs.120",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory39.Id
            };

            var priceType40 = new PriceType
            {
                Id = 40,
                Guid = Guid.NewGuid(),
                Name = "Kaleji Full Rs.200",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory40.Id
            };

            var priceType41 = new PriceType
            {
                Id = 41,
                Guid = Guid.NewGuid(),
                Name = "Qeema Half Rs.120",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory41.Id
            };

            var priceType42 = new PriceType
            {
                Id = 42,
                Guid = Guid.NewGuid(),
                Name = "Qeema Full Rs.200",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory42.Id
            };

            var priceType43 = new PriceType
            {
                Id = 43,
                Guid = Guid.NewGuid(),
                Name = "Daal Maash Half Rs.90",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory43.Id
            };

            var priceType44 = new PriceType
            {
                Id = 44,
                Guid = Guid.NewGuid(),
                Name = "Daal Maash Full Rs.160",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory44.Id
            };

            var priceType45 = new PriceType
            {
                Id = 45,
                Guid = Guid.NewGuid(),
                Name = "Murgh Chana Full Rs.180",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory45.Id
            };

            var priceType46 = new PriceType
            {
                Id = 46,
                Guid = Guid.NewGuid(),
                Name = "Chana Half Rs.80",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory46.Id
            };

            var priceType47 = new PriceType
            {
                Id = 47,
                Guid = Guid.NewGuid(),
                Name = "Chana Full Rs.130",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory47.Id
            };

            var priceType48 = new PriceType
            {
                Id = 48,
                Guid = Guid.NewGuid(),
                Name = "Anda Timatar Half Rs.90",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory48.Id
            };

            var priceType49 = new PriceType
            {
                Id = 49,
                Guid = Guid.NewGuid(),
                Name = "Anda Timatar Full Rs.150",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory49.Id
            };

            var priceType50 = new PriceType
            {
                Id = 50,
                Guid = Guid.NewGuid(),
                Name = "Salan Rs.100",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory50.Id
            };

            var priceType51 = new PriceType
            {
                Id = 51,
                Guid = Guid.NewGuid(),
                Name = "Fry Rs.20",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory51.Id
            };

            var priceType52 = new PriceType
            {
                Id = 52,
                Guid = Guid.NewGuid(),
                Name = "Naan Rs.25",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory52.Id
            };

            var priceType53 = new PriceType
            {
                Id = 53,
                Guid = Guid.NewGuid(),
                Name = "Laal Roti Rs.20",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory53.Id
            };

            var priceType54 = new PriceType
            {
                Id = 54,
                Guid = Guid.NewGuid(),
                Name = "Bhindi Full Rs.200",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory54.Id
            };

            var priceType55 = new PriceType
            {
                Id = 55,
                Guid = Guid.NewGuid(),
                Name = "Bhindi Half Rs.110",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory55.Id
            };

            var priceType56 = new PriceType
            {
                Id = 56,
                Guid = Guid.NewGuid(),
                Name = "Aalu Palak Half Rs.80",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory56.Id
            };

            var priceType57 = new PriceType
            {
                Id = 57,
                Guid = Guid.NewGuid(),
                Name = "Daal Chana Half Rs.80",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory57.Id
            };

            var priceType58 = new PriceType
            {
                Id = 58,
                Guid = Guid.NewGuid(),
                Name = "Daal Chana Full Rs.130",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory58.Id
            };

            var priceType59 = new PriceType
            {
                Id = 59,
                Guid = Guid.NewGuid(),
                Name = "Aalu Palak Full Rs.130",
                Active = true,
                PriceTypeCategoryId = priceTypeCategory59.Id
            };
            modelBuilder.Entity<PriceType>().HasData(priceType1);
            modelBuilder.Entity<PriceType>().HasData(priceType2);
            modelBuilder.Entity<PriceType>().HasData(priceType3);
            modelBuilder.Entity<PriceType>().HasData(priceType4);
            modelBuilder.Entity<PriceType>().HasData(priceType5);
            modelBuilder.Entity<PriceType>().HasData(priceType6);
            modelBuilder.Entity<PriceType>().HasData(priceType7);
            modelBuilder.Entity<PriceType>().HasData(priceType8);
            modelBuilder.Entity<PriceType>().HasData(priceType9);
            modelBuilder.Entity<PriceType>().HasData(priceType10);
            modelBuilder.Entity<PriceType>().HasData(priceType11);
            modelBuilder.Entity<PriceType>().HasData(priceType12);
            modelBuilder.Entity<PriceType>().HasData(priceType13);
            modelBuilder.Entity<PriceType>().HasData(priceType14);
            modelBuilder.Entity<PriceType>().HasData(priceType15);
            modelBuilder.Entity<PriceType>().HasData(priceType16);
            modelBuilder.Entity<PriceType>().HasData(priceType17);
            modelBuilder.Entity<PriceType>().HasData(priceType18);
            modelBuilder.Entity<PriceType>().HasData(priceType19);
            modelBuilder.Entity<PriceType>().HasData(priceType20);
            modelBuilder.Entity<PriceType>().HasData(priceType21);
            modelBuilder.Entity<PriceType>().HasData(priceType22);
            modelBuilder.Entity<PriceType>().HasData(priceType23);
            modelBuilder.Entity<PriceType>().HasData(priceType24);
            modelBuilder.Entity<PriceType>().HasData(priceType25);
            modelBuilder.Entity<PriceType>().HasData(priceType26);
            modelBuilder.Entity<PriceType>().HasData(priceType27);
            modelBuilder.Entity<PriceType>().HasData(priceType28);
            modelBuilder.Entity<PriceType>().HasData(priceType29);
            modelBuilder.Entity<PriceType>().HasData(priceType30);
            modelBuilder.Entity<PriceType>().HasData(priceType31);
            modelBuilder.Entity<PriceType>().HasData(priceType32);
            modelBuilder.Entity<PriceType>().HasData(priceType33);
            modelBuilder.Entity<PriceType>().HasData(priceType34);
            modelBuilder.Entity<PriceType>().HasData(priceType35);
            modelBuilder.Entity<PriceType>().HasData(priceType36);
            modelBuilder.Entity<PriceType>().HasData(priceType37);
            modelBuilder.Entity<PriceType>().HasData(priceType38);
            modelBuilder.Entity<PriceType>().HasData(priceType39);
            modelBuilder.Entity<PriceType>().HasData(priceType40);
            modelBuilder.Entity<PriceType>().HasData(priceType41);
            modelBuilder.Entity<PriceType>().HasData(priceType42);
            modelBuilder.Entity<PriceType>().HasData(priceType43);
            modelBuilder.Entity<PriceType>().HasData(priceType44);
            modelBuilder.Entity<PriceType>().HasData(priceType45);
            modelBuilder.Entity<PriceType>().HasData(priceType46);
            modelBuilder.Entity<PriceType>().HasData(priceType47);
            modelBuilder.Entity<PriceType>().HasData(priceType48);
            modelBuilder.Entity<PriceType>().HasData(priceType49);
            modelBuilder.Entity<PriceType>().HasData(priceType50);
            modelBuilder.Entity<PriceType>().HasData(priceType51);
            modelBuilder.Entity<PriceType>().HasData(priceType52);
            modelBuilder.Entity<PriceType>().HasData(priceType53);
            modelBuilder.Entity<PriceType>().HasData(priceType54);
            modelBuilder.Entity<PriceType>().HasData(priceType55);
            modelBuilder.Entity<PriceType>().HasData(priceType56);
            modelBuilder.Entity<PriceType>().HasData(priceType57);
            modelBuilder.Entity<PriceType>().HasData(priceType58);
            modelBuilder.Entity<PriceType>().HasData(priceType59);

            #endregion

            #region ProductPrices
            var productPrice1 = new ProductPrice
            {
                Id = 1,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 1,
                PriceTypeId = 1,
                PriceTypeCategoryId = 1,
                Price = 190,
                Active = true,
            };

            var productPrice2 = new ProductPrice
            {
                Id = 2,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 2,
                PriceTypeId = 2,
                PriceTypeCategoryId = 2,
                Price = 230,
                Active = true,
            };

            var productPrice3 = new ProductPrice
            {
                Id = 3,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 3,
                PriceTypeId = 3,
                PriceTypeCategoryId = 3,
                Price = 280,
                Active = true,
            };

            var productPrice4 = new ProductPrice
            {
                Id = 4,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 4,
                PriceTypeId = 4,
                PriceTypeCategoryId = 4,
                Price = 560,
                Active = true,
            };

            var productPrice5 = new ProductPrice
            {
                Id = 5,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 5,
                PriceTypeId = 5,
                PriceTypeCategoryId = 5,
                Price = 140,
                Active = true,
            };

            var productPrice6 = new ProductPrice
            {
                Id = 6,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 6,
                PriceTypeId = 6,
                PriceTypeCategoryId = 6,
                Price = 180,
                Active = true,
            };

            var productPrice7 = new ProductPrice
            {
                Id = 7,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 7,
                PriceTypeId = 7,
                PriceTypeCategoryId = 7,
                Price = 360,
                Active = true,
            };

            var productPrice8 = new ProductPrice
            {
                Id = 8,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 8,
                PriceTypeId = 8,
                PriceTypeCategoryId = 8,
                Price = 100,
                Active = true,
            };

            var productPrice9 = new ProductPrice
            {
                Id = 9,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 9,
                PriceTypeId = 9,
                PriceTypeCategoryId = 9,
                Price = 150,
                Active = true,
            };

            var productPrice10 = new ProductPrice
            {
                Id = 10,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 10,
                PriceTypeId = 10,
                PriceTypeCategoryId = 10,
                Price = 200,
                Active = true,
            };

            var productPrice11 = new ProductPrice
            {
                Id = 11,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 11,
                PriceTypeId = 11,
                PriceTypeCategoryId = 11,
                Price = 50,
                Active = true,
            };

            var productPrice12 = new ProductPrice
            {
                Id = 12,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 12,
                PriceTypeId = 12,
                PriceTypeCategoryId = 12,
                Price = 10,
                Active = true,
            };

            var productPrice13 = new ProductPrice
            {
                Id = 13,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 13,
                PriceTypeId = 13,
                PriceTypeCategoryId = 13,
                Price = 300,
                Active = true,
            };

            var productPrice14 = new ProductPrice
            {
                Id = 14,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 14,
                PriceTypeId = 14,
                PriceTypeCategoryId = 14,
                Price = 400,
                Active = true,
            };

            var productPrice15 = new ProductPrice
            {
                Id = 15,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 15,
                PriceTypeId = 15,
                PriceTypeCategoryId = 15,
                Price = 500,
                Active = true,
            };

            var productPrice16 = new ProductPrice
            {
                Id = 16,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 16,
                PriceTypeId = 16,
                PriceTypeCategoryId = 16,
                Price = 90,
                Active = true,
            };

            var productPrice17 = new ProductPrice
            {
                Id = 17,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 17,
                PriceTypeId = 17,
                PriceTypeCategoryId = 17,
                Price = 100,
                Active = true,
            };

            var productPrice18 = new ProductPrice
            {
                Id = 18,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 18,
                PriceTypeId = 18,
                PriceTypeCategoryId = 18,
                Price = 190,
                Active = true,
            };

            var productPrice19 = new ProductPrice
            {
                Id = 19,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 19,
                PriceTypeId = 19,
                PriceTypeCategoryId = 19,
                Price = 230,
                Active = true,
            };

            var productPrice20 = new ProductPrice
            {
                Id = 20,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 20,
                PriceTypeId = 20,
                PriceTypeCategoryId = 20,
                Price = 280,
                Active = true,
            };

            var productPrice21 = new ProductPrice
            {
                Id = 21,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 21,
                PriceTypeId = 21,
                PriceTypeCategoryId = 21,
                Price = 560,
                Active = true,
            };

            var productPrice22 = new ProductPrice
            {
                Id = 22,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 22,
                PriceTypeId = 22,
                PriceTypeCategoryId = 22,
                Price = 140,
                Active = true,
            };

            var productPrice23 = new ProductPrice
            {
                Id = 23,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 23,
                PriceTypeId = 23,
                PriceTypeCategoryId = 23,
                Price = 180,
                Active = true,
            };

            var productPrice24 = new ProductPrice
            {
                Id = 24,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 24,
                PriceTypeId = 24,
                PriceTypeCategoryId = 24,
                Price = 360,
                Active = true,
            };

            var productPrice25 = new ProductPrice
            {
                Id = 25,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 25,
                PriceTypeId = 25,
                PriceTypeCategoryId = 25,
                Price = 100,
                Active = true,
            };

            var productPrice26 = new ProductPrice
            {
                Id = 26,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 26,
                PriceTypeId = 26,
                PriceTypeCategoryId = 26,
                Price = 150,
                Active = true,
            };

            var productPrice27 = new ProductPrice
            {
                Id = 27,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 27,
                PriceTypeId = 27,
                PriceTypeCategoryId = 27,
                Price = 200,
                Active = true,
            };

            var productPrice28 = new ProductPrice
            {
                Id = 28,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 28,
                PriceTypeId = 28,
                PriceTypeCategoryId = 28,
                Price = 50,
                Active = true,
            };

            var productPrice29 = new ProductPrice
            {
                Id = 29,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 29,
                PriceTypeId = 29,
                PriceTypeCategoryId = 29,
                Price = 10,
                Active = true,
            };

            var productPrice30 = new ProductPrice
            {
                Id = 30,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 30,
                PriceTypeId = 30,
                PriceTypeCategoryId = 30,
                Price = 300,
                Active = true,
            };

            var productPrice31 = new ProductPrice
            {
                Id = 31,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 31,
                PriceTypeId = 31,
                PriceTypeCategoryId = 31,
                Price = 400,
                Active = true,
            };

            var productPrice32 = new ProductPrice
            {
                Id = 32,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 32,
                PriceTypeId = 32,
                PriceTypeCategoryId = 32,
                Price = 500,
                Active = true,
            };

            var productPrice33 = new ProductPrice
            {
                Id = 33,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 33,
                PriceTypeId = 33,
                PriceTypeCategoryId = 33,
                Price = 100,
                Active = true,
            };

            var productPrice34 = new ProductPrice
            {
                Id = 34,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 34,
                PriceTypeId = 34,
                PriceTypeCategoryId = 34,
                Price = 140,
                Active = true,
            };

            var productPrice35 = new ProductPrice
            {
                Id = 35,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 35,
                PriceTypeId = 35,
                PriceTypeCategoryId = 35,
                Price = 150,
                Active = true,
            };

            var productPrice36 = new ProductPrice
            {
                Id = 36,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 36,
                PriceTypeId = 36,
                PriceTypeCategoryId = 36,
                Price = 250,
                Active = true,
            };

            var productPrice37 = new ProductPrice
            {
                Id = 37,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 37,
                PriceTypeId = 37,
                PriceTypeCategoryId = 37,
                Price = 70,
                Active = true,
            };

            var productPrice38 = new ProductPrice
            {
                Id = 38,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 38,
                PriceTypeId = 38,
                PriceTypeCategoryId = 38,
                Price = 130,
                Active = true,
            };

            var productPrice39 = new ProductPrice
            {
                Id = 39,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 39,
                PriceTypeId = 39,
                PriceTypeCategoryId = 39,
                Price = 120,
                Active = true,
            };

            var productPrice40 = new ProductPrice
            {
                Id = 40,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 40,
                PriceTypeId = 40,
                PriceTypeCategoryId = 40,
                Price = 200,
                Active = true,
            };

            var productPrice41 = new ProductPrice
            {
                Id = 41,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 41,
                PriceTypeId = 41,
                PriceTypeCategoryId = 41,
                Price = 120,
                Active = true,
            };

            var productPrice42 = new ProductPrice
            {
                Id = 42,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 42,
                PriceTypeId = 42,
                PriceTypeCategoryId = 42,
                Price = 200,
                Active = true,
            };

            var productPrice43 = new ProductPrice
            {
                Id = 43,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 43,
                PriceTypeId = 43,
                PriceTypeCategoryId = 43,
                Price = 90,
                Active = true,
            };

            var productPrice44 = new ProductPrice
            {
                Id = 44,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 44,
                PriceTypeId = 44,
                PriceTypeCategoryId = 44,
                Price = 160,
                Active = true,
            };

            var productPrice45 = new ProductPrice
            {
                Id = 45,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 45,
                PriceTypeId = 45,
                PriceTypeCategoryId = 45,
                Price = 180,
                Active = true,
            };

            var productPrice46 = new ProductPrice
            {
                Id = 46,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 46,
                PriceTypeId = 46,
                PriceTypeCategoryId = 46,
                Price = 80,
                Active = true,
            };

            var productPrice47 = new ProductPrice
            {
                Id = 47,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 47,
                PriceTypeId = 47,
                PriceTypeCategoryId = 47,
                Price = 130,
                Active = true,
            };

            var productPrice48 = new ProductPrice
            {
                Id = 48,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 48,
                PriceTypeId = 48,
                PriceTypeCategoryId = 48,
                Price = 90,
                Active = true,
            };

            var productPrice49 = new ProductPrice
            {
                Id = 49,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 49,
                PriceTypeId = 49,
                PriceTypeCategoryId = 49,
                Price = 150,
                Active = true,
            };

            var productPrice50 = new ProductPrice
            {
                Id = 50,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 50,
                PriceTypeId = 50,
                PriceTypeCategoryId = 50,
                Price = 100,
                Active = true,
            };

            var productPrice51 = new ProductPrice
            {
                Id = 51,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 51,
                PriceTypeId = 51,
                PriceTypeCategoryId = 51,
                Price = 20,
                Active = true,
            };

            var productPrice52 = new ProductPrice
            {
                Id = 52,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 52,
                PriceTypeId = 52,
                PriceTypeCategoryId = 52,
                Price = 25,
                Active = true,
            };

            var productPrice53 = new ProductPrice
            {
                Id = 53,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 53,
                PriceTypeId = 53,
                PriceTypeCategoryId = 53,
                Price = 20,
                Active = true,
            };

            var productPrice54 = new ProductPrice
            {
                Id = 54,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 54,
                PriceTypeId = 54,
                PriceTypeCategoryId = 54,
                Price = 200,
                Active = true,
            };

            var productPrice55 = new ProductPrice
            {
                Id = 55,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 55,
                PriceTypeId = 55,
                PriceTypeCategoryId = 55,
                Price = 110,
                Active = true,
            };

            var productPrice56 = new ProductPrice
            {
                Id = 56,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 56,
                PriceTypeId = 56,
                PriceTypeCategoryId = 56,
                Price = 80,
                Active = true,
            };

            var productPrice57 = new ProductPrice
            {
                Id = 57,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 57,
                PriceTypeId = 57,
                PriceTypeCategoryId = 57,
                Price = 80,
                Active = true,
            };

            var productPrice58 = new ProductPrice
            {
                Id = 58,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 58,
                PriceTypeId = 58,
                PriceTypeCategoryId = 58,
                Price = 130,
                Active = true,
            };

            var productPrice59 = new ProductPrice
            {
                Id = 59,
                Guid = Guid.NewGuid(),
                ChildCategoryId = 59,
                PriceTypeId = 59,
                PriceTypeCategoryId = 59,
                Price = 130,
                Active = true,
            };

            modelBuilder.Entity<ProductPrice>().HasData(productPrice1);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice2);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice3);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice4);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice5);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice6);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice7);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice8);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice9);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice10);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice11);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice12);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice13);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice14);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice15);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice16);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice17);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice18);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice19);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice20);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice21);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice22);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice23);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice24);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice25);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice26);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice27);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice28);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice29);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice30);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice31);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice32);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice33);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice34);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice35);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice36);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice37);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice38);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice39);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice40);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice41);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice42);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice43);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice44);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice45);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice46);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice47);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice48);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice49);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice50);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice51);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice52);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice53);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice54);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice55);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice56);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice57);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice58);
            modelBuilder.Entity<ProductPrice>().HasData(productPrice59);
            #endregion
        }
    }
    }
