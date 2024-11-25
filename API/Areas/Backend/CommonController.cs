using API.Areas.Backend.Controllers;
using API.Helpers;
using AutoMapper;
using Libwebp.Net;
using Libwebp.Net.utility;
using Libwebp.Standard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Backend.UserManagement;
using Utility.API;
using Utility.Enum;
using Utility.Helpers;
using Utility.ResponseMapper;
using Data.UserManagement;
using Data.Model.SiteCategory;
using Services.Backend.Categorys;
using Serilog;
using Services.Backend.SubCategories;
using Services.Backend.ChildCategories;
using Services.Backend.Price;
using Data.Model.General;
using Data.Model.InventoryManagement;
using Services.Backend.Inventory;
using Services.Backend.InventoryManagement;
namespace API.Areas.Backend
{
    [Authorize]
    public class CommonController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IUserService<User> _systemUserService;
        private readonly IUserRoleService<UserRole> _userRoleService;
        private readonly IUserPermissionService<UserPermission> _userPermissionService;
        private readonly ICommonHelper _commonHelper;
        private readonly ICategoryService<Category> _categoryService;
        private readonly ISubCategoryService<SubCategory> _subCategoryService;
        private readonly IChildCategoryService<ChildCategory> _childCategoryService;
        private readonly IPriceTypeCategoryService<PriceTypeCategory> _priceTypeCategoryService;
        private readonly IPriceTypeService<PriceType> _priceTypeService;
        private readonly ISupplierService<Supplier> _supplierService;
        private readonly IInventoryItemService<InventoryItem> _inventoryItemService;

        public CommonController(
            AppSettingsModel options,
            IAPIHelper apiHelper,
            IUserService<User> systemUserService,
            IUserRoleService<UserRole> userRoleService,
            IUserPermissionService<UserPermission> userPermissionService,
            IMapper mapper,
            ICommonHelper commonHelper,
            ICategoryService<Category> categoryService, 
            ISubCategoryService<SubCategory> subCategoryService,
            IChildCategoryService<ChildCategory> childCategoryService,
            IPriceTypeCategoryService<PriceTypeCategory> priceTypeCategoryService,
            IPriceTypeService<PriceType> priceTypeService,
            ISupplierService<Supplier> supplierService,
            IInventoryItemService<InventoryItem> inventoryItemService) : 


            base(options, apiHelper, systemUserService, PermissionTypes.None)
        {
            base.ControllerName = typeof(CommonController).Name;
            _mapper = mapper;
            _systemUserService = systemUserService;
            _userRoleService = userRoleService;
            _userPermissionService = userPermissionService;
            _commonHelper = commonHelper;
            _categoryService = categoryService;
            _subCategoryService = subCategoryService;
            _childCategoryService = childCategoryService;
            _priceTypeCategoryService = priceTypeCategoryService;
            _priceTypeService = priceTypeService;
            _supplierService = supplierService;
            _inventoryItemService = inventoryItemService;
            
        }

        [HttpPost, Route("api/Common/Upload")]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {

            var filePath = MediaHelper.ConvertImageToWebp("01.png", file, "Images");

            if (file == null)
                throw new FileNotFoundException();

            //you can handle file checks ie. extensions, file size etc..

            //creating output file name
            // your name can be a unique Guid or any name of your choice with .webp extension..eg output.webp
            //in my case i am removing the extension from the uploaded file and appending
            // the .webp extension
            var oFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}.webp";

            // create your webp configuration
            var config = new WebpConfigurationBuilder()
               .Preset(Preset.PHOTO)
               .Output(oFileName)
               .Build();

            //create an encoder and pass in your configuration
            var encoder = new WebpEncoder(config);

            //copy file to memory stream
            var ms = new MemoryStream();
            file.CopyTo(ms);

            //call the encoder and pass in the Memorystream and input FileName
            //the encoder after encoding will return a FileStream output

            //Optional cast to Stream to return file for download
            Stream fs = await encoder.EncodeAsync(ms, file.FileName);

            /*Do whatever you want with the file....download, copy to disk or 
              save to cloud*/

            return File(fs, "application/octet-stream", oFileName);
        }

        [HttpPost, Route("api/Common/UploadFile")]
        public IActionResult UploadFileAsync(IFormFile file)
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                if (file != null && file.Length > 0)
                {
                    string fileName = MediaHelper.ConvertImageToWebp(string.Empty, file, AppSettings.ImageSettings.UploadForms);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        response.Message = "success";
                        response.Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                var innerException = ex.InnerException;
                if (innerException != null)
                {
                    do
                    {
                        errorMessage = errorMessage + ", " + innerException.Message;
                        innerException = innerException.InnerException;
                    }
                    while (innerException != null);
                }

                response.Message = errorMessage;
            }

            return Ok(response);
        }



        #region Category
        [HttpGet, Route("api/Common/ForCategoryDropDownList")]
        public async Task<IActionResult> ForCategoryDropDownList()
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                var items = await _categoryService.GetAllForDropDownList(IsEnglish);
                response.GetAll(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Logger.Error(ex, "Error in ForCategoryDropDownList");
            }

            return Ok(response);
        }

        /// <summary>
        /// Menu list based on logged-In user's role
        /// </summary>
        /// <returns></returns>
        //[HttpGet, Route("api/Common/GetCategoryMenuList")]
        //public async Task<IActionResult> GetCategoryMenuList()
        //{
        //    ResponseMapper<List<UserPermission>> response = new();
        //    try
        //    {
        //        // To get the logged-in user's RoleId,
        //        var auth = base.AuthenticateUser;
        //        if (TokenExpired)
        //        {
        //            return Ok(new ResponseMapper<User> { StatusCode = (int)ResponseStatus.TokenExpired, Message = "Token is Expired" });
        //        }
        //        // Fetch menu-list for the logged-in user
        //        var items = await _userPermissionService.GetMenuByRoleId(base.RoleId, IsEnglish, base.UserId);
        //        response.GetAll(items);
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.CacheException(ex);
        //        Log.Logger.Error(ex, "Error in GetMenuList");
        //    }

        //    return Ok(response);
        //}
        #endregion

        #region Sub Category

        [HttpGet, Route("api/Common/ForSubCategoryDropDownList")]
        public async Task<IActionResult> ForSubCategoryDropDownList()
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                var items = await _subCategoryService.GetAllForDropDownList(IsEnglish);
                response.GetAll(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Logger.Error(ex, "Error in ForSubCategoryDropDownList");
            }

            return Ok(response);
        }

        /// <summary>
        /// SubCategory menu list based on logged-In user's role
        /// </summary>
        /// <returns></returns>
        //[HttpGet, Route("api/Common/GetSubCategoryMenuList")]
        //public async Task<IActionResult> GetSubCategoryMenuList()
        //{
        //    ResponseMapper<List<UserPermission>> response = new();
        //    try
        //    {
        //        // To get the logged-in user's RoleId,
        //        var auth = base.AuthenticateUser;
        //        if (TokenExpired)
        //        {
        //            return Ok(new ResponseMapper<User> { StatusCode = (int)ResponseStatus.TokenExpired, Message = "Token is Expired" });
        //        }
        //        // Fetch menu-list for the logged-in user
        //        var items = await _userPermissionService.GetMenuByRoleId(base.RoleId, IsEnglish, base.UserId);

        //        response.GetAll(items);
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.CacheException(ex);
        //        Log.Logger.Error(ex, "Error in GetSubCategoryMenuList");
        //    }

        //    return Ok(response);
        //}


        #endregion

        #region Child Category

        [HttpGet, Route("api/Common/ForChildCategoryDropDownList")]
        public async Task<IActionResult> ForChildCategoryDropDownList()
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                var items = await _childCategoryService.GetAllForDropDownList(IsEnglish);
                response.GetAll(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Logger.Error(ex, "Error in ForChildCategoryDropDownList");
            }

            return Ok(response);
        }

        /// <summary>
        /// ChildCategory menu list based on logged-In user's role
        /// </summary>
        /// <returns></returns>
        //[HttpGet, Route("api/Common/GetChildCategoryMenuList")]
        //public async Task<IActionResult> GetChildCategoryMenuList()
        //{
        //    ResponseMapper<List<UserPermission>> response = new();
        //    try
        //    {
        //        // To get the logged-in user's RoleId,
        //        var auth = base.AuthenticateUser;
        //        if (TokenExpired)
        //        {
        //            return Ok(new ResponseMapper<User> { StatusCode = (int)ResponseStatus.TokenExpired, Message = "Token is Expired" });
        //        }
        //        // Fetch menu-list for the logged-in user
        //        var items = await _userPermissionService.GetMenuByRoleId(base.RoleId, IsEnglish, base.UserId);

        //        response.GetAll(items);
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.CacheException(ex);
        //        Log.Logger.Error(ex, "Error in GetChildCategoryMenuList");
        //    }

        //    return Ok(response);
        //}

        #endregion

        #region Price Type Category

        [HttpGet, Route("api/Common/ForPriceTypeCategoryDropDownList")]
        public async Task<IActionResult> ForPriceTypeCategoryDropDownList()
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                var items = await _priceTypeCategoryService.GetAllForDropDownList(IsEnglish);
                response.GetAll(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Logger.Error(ex, "Error in ForPriceTypeCategoryDropDownList");
            }

            return Ok(response);
        }

        /// <summary>
        /// PriceTypeCategory menu list based on logged-In user's role
        /// </summary>
        /// <returns></returns>
        //[HttpGet, Route("api/Common/GetPriceTypeCategoryMenuList")]
        //public async Task<IActionResult> GetPriceTypeCategoryMenuList()
        //{
        //    ResponseMapper<List<UserPermission>> response = new();
        //    try
        //    {
        //        // To get the logged-in user's RoleId,
        //        var auth = base.AuthenticateUser;
        //        if (TokenExpired)
        //        {
        //            return Ok(new ResponseMapper<User> { StatusCode = (int)ResponseStatus.TokenExpired, Message = "Token is Expired" });
        //        }
        //        // Fetch menu-list for the logged-in user
        //        var items = await _userPermissionService.GetMenuByRoleId(base.RoleId, IsEnglish, base.UserId);

        //        response.GetAll(items);
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.CacheException(ex);
        //        Log.Logger.Error(ex, "Error in GetPriceTypeCategoryMenuList");
        //    }

        //    return Ok(response);
        //}

        #endregion

        #region Price Type

        [HttpGet, Route("api/Common/ForPriceTypeDropDownList")]
        public async Task<IActionResult> ForPriceTypeDropDownList()
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                var items = await _priceTypeService.GetAllForDropDownList(IsEnglish);
                response.GetAll(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Logger.Error(ex, "Error in ForPriceTypeDropDownList");
            }

            return Ok(response);
        }

        /// <summary>
        /// PriceType menu list based on logged-In user's role
        /// </summary>
        /// <returns></returns>
        //[HttpGet, Route("api/Common/GetPriceTypeMenuList")]
        //public async Task<IActionResult> GetPriceTypeMenuList()
        //{
        //    ResponseMapper<List<UserPermission>> response = new();
        //    try
        //    {
        //        // To get the logged-in user's RoleId,
        //        var auth = base.AuthenticateUser;
        //        if (TokenExpired)
        //        {
        //            return Ok(new ResponseMapper<User> { StatusCode = (int)ResponseStatus.TokenExpired, Message = "Token is Expired" });
        //        }
        //        // Fetch menu-list for the logged-in user
        //        var items = await _userPermissionService.GetMenuByRoleId(base.RoleId, IsEnglish, base.UserId);

        //        response.GetAll(items);
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.CacheException(ex);
        //        Log.Logger.Error(ex, "Error in GetPriceTypeMenuList");
        //    }

        //    return Ok(response);
        //}

        #endregion

        #region Supplier

        [HttpGet, Route("api/Common/ForSupplierDropDownList")]
        public async Task<IActionResult> ForSupplierDropDownList()
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                var items = await _supplierService.GetAllForDropDownList(IsEnglish);
                response.GetAll(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Logger.Error(ex, "Error in ForSupplierDropDownList");
            }

            return Ok(response);
        }

        /// <summary>
        /// Supplier menu list based on logged-In user's role
        /// </summary>
        /// <returns></returns>
        //[HttpGet, Route("api/Common/GetSupplierMenuList")]
        //public async Task<IActionResult> GetSupplierMenuList()
        //{
        //    ResponseMapper<List<UserPermission>> response = new();
        //    try
        //    {
        //        // To get the logged-in user's RoleId,
        //        var auth = base.AuthenticateUser;
        //        if (TokenExpired)
        //        {
        //            return Ok(new ResponseMapper<User> { StatusCode = (int)ResponseStatus.TokenExpired, Message = "Token is Expired" });
        //        }
        //        // Fetch menu-list for the logged-in user
        //        var items = await _userPermissionService.GetMenuByRoleId(base.RoleId, IsEnglish, base.UserId);

        //        response.GetAll(items);
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.CacheException(ex);
        //        Log.Logger.Error(ex, "Error in GetSupplierMenuList");
        //    }

        //    return Ok(response);
        //}

        #endregion

        #region Inventory Item

        [HttpGet, Route("api/Common/ForInventoryItemDropDownList")]
        public async Task<IActionResult> ForInventoryItemDropDownList()
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                var items = await _inventoryItemService.GetAllForDropDownList(IsEnglish);
                response.GetAll(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Logger.Error(ex, "Error in ForInventoryItemDropDownList");
            }

            return Ok(response);
        }

        /// <summary>
        /// Supplier menu list based on logged-In user's role
        /// </summary>
        /// <returns></returns>
        //[HttpGet, Route("api/Common/GetInventoryMenuList")]
        //public async Task<IActionResult> GetInventoryMenuList()
        //{
        //    ResponseMapper<List<UserPermission>> response = new();
        //    try
        //    {
        //        // To get the logged-in user's RoleId,
        //        var auth = base.AuthenticateUser;
        //        if (TokenExpired)
        //        {
        //            return Ok(new ResponseMapper<User> { StatusCode = (int)ResponseStatus.TokenExpired, Message = "Token is Expired" });
        //        }
        //        // Fetch menu-list for the logged-in user
        //        var items = await _userPermissionService.GetMenuByRoleId(base.RoleId, IsEnglish, base.UserId);

        //        response.GetAll(items);
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.CacheException(ex);
        //        Log.Logger.Error(ex, "Error in GetInventoryItemMenuList");
        //    }

        //    return Ok(response);
        //}

        #endregion

        #region Role Types

        [HttpGet, Route("api/Common/ForRoleTypeDropDownList")]
        public async Task<IActionResult> ForRoleTypeDropDownList()
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                var items = await base.UserService.GetRoleTypesForDropDownList();
                response.GetAll(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }
        #endregion

        #region Roles

        [HttpGet, Route("api/Common/ForRoleDropDownList")]
        public async Task<IActionResult> ForRoleDropDownList()
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                var items = await _userRoleService.GetAllForDropDownList(IsEnglish);
                response.GetAll(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        /// <summary>
        /// Menu list based on logged-In user
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/Common/GetMenuList")]
        public async Task<IActionResult> GetMenuList()
        {
            ResponseMapper<List<UserPermission>> response = new();
            try
            {
                //to get the logged-in user RoleId, 
                var auth = base.AuthenticateUser;
                if (TokenExpired)
                {
                    return Ok(new ResponseMapper<User> { StatusCode = (int)ResponseStatus.TokenExpired, Message = "Token is Expired" });
                }
                //fetch menu-list for the logged-in user
                var items = await _userPermissionService.GetMenuByRoleId(base.RoleId, IsEnglish, base.UserId);

                response.GetAll(items);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        #endregion

        #region Test

        [HttpGet, Route("api/Common/TestApi")]
        public IActionResult TestApi()
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                string title = "hello";
                title = title.ToLower();
                var lstTitle = title.GroupBy(x => x).Select(x => new { Char = x.Key, Count = x.Count() }).OrderBy(x => x.Char).ToList();
                string output = "";
                foreach (var item in lstTitle)
                {
                    output = output + (item.Char + "" + item.Count);
                }
                response.GetById(output);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }
        #endregion
    }
}
