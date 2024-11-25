using API.Helpers;
using AutoMapper;
using Data.EntityFramework;
using Data.Model.General;
using Data.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Services.Backend.Price;
using Services.Backend.UserManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utility.API;
using Utility.Enum;
using Utility.Models.Admin.DTO;
using Utility.Models.Base;
using Utility.ResponseMapper;

namespace API.Areas.Backend.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class ProductPriceController : BaseController
    {
        private readonly IProductPriceService<ProductPrice> _productPriceService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public ProductPriceController(
            AppSettingsModel options,
            IAPIHelper apiHelper,
            IUserService<User> systemUserService,
            IProductPriceService<ProductPrice> productPriceService,
            IMapper mapper,
            IMemoryCache memoryCache) :
            base(options, apiHelper, systemUserService, PermissionTypes.ProductPrices)
        {
            base.ControllerName = typeof(ProductPriceController).Name;
            _productPriceService = productPriceService;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        #region ProductPrice Management

        [HttpGet, Route("api/ProductPrice/GetByGuid")]
        public async Task<IActionResult> GetByGuid(Guid? guid)
        {
            ResponseMapper<ProductPrice> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.ProductPrices, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                if (guid.HasValue)
                {
                    var item = await _productPriceService.GetByGuid(guid.Value);
                    response.GetById(item);
                }
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in GetByGuid");
            }

            return Ok(response);
        }

        [HttpPost, Route("api/ProductPrice/AddEdit")]
        public async Task<IActionResult> AddEdit([FromForm] ProductPrice item)
        {
            ResponseMapper<ProductPrice> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.ProductPrices, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                if (item.Guid.HasValue)
                {
                    item.ModifiedBy = this.UserId;
                    await _productPriceService.Update(item);
                    response.Update(item);
                    response.Message = "Product price edited successfully";
                }
                else
                {
                    item.Guid = Guid.NewGuid();
                    item.CreatedBy = this.UserId;
                    await _productPriceService.Create(item);
                    response.Create(item);
                    response.Message = "Product price created successfully";
                }
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in AddEdit");
            }

            return Ok(response);
        }

        [HttpPost, Route("api/ProductPrice/ToggleActive")]
        public async Task<IActionResult> ToggleActiveAsync(Guid guid)
        {
            ResponseMapper<ProductPrice> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.ProductPrices, AllowPermission.Active))
                {
                    return Ok(accessResponse);
                }

                var item = await _productPriceService.ToggleActive(guid);
                response.ToggleActive(item);
                ClearCache();
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in ToggleActiveAsync");
            }

            return Ok(response);
        }

        [HttpPost, Route("api/ProductPrice/UpdateDisplayOrders")]
        public async Task<IActionResult> UpdateDisplayOrders([FromForm] List<BaseRowOrder> items)
        {
            ResponseMapper<ProductPrice> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.ProductPrices, AllowPermission.DisplayOrder))
                {
                    return Ok(accessResponse);
                }

                if (items.Count > 0)
                {
                    items.ForEach(i => i.UserId = this.UserId);
                    await _productPriceService.UpdateDisplayOrders(items);
                    response.DisplayOrder(true);
                }

                ClearCache();
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in UpdateDisplayOrders");
            }

            return Ok(response);
        }

        #region ProductPrice Management

        [HttpGet, Route("api/ProductPrice/GetPriceTypeCategoryByChildCategoryId")]
        public async Task<IActionResult> GetPriceTypeCategoryByChildCategoryId(long childCategoryId)
        {
            ResponseMapper<List<PriceTypeCategory>> response = new();
            try
            {
                var items = await _productPriceService.GetPriceTypeCategoryByChildCategoryId(childCategoryId);
                response.GetAll(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in GetPriceTypeCategoryByChildCategoryId");
            }

            return Ok(response);
        }

        [HttpGet, Route("api/ProductPrice/GetPriceTypeByPriceTypeCategoryId")]
        public async Task<IActionResult> GetPriceTypeByPriceTypeCategoryId(long priceTypeCategoryId)
        {
            ResponseMapper<List<PriceType>> response = new();
            try
            {
                var items = await _productPriceService.GetPriceTypeByPriceTypeCategoryId(priceTypeCategoryId);
                response.GetAll(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in GetPriceTypeByPriceTypeCategoryId");
            }

            return Ok(response);
        }

        #endregion

        [HttpPost, Route("api/ProductPrice/GetProductPriceForDataTable")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetProductPriceForDataTable([FromForm] int? childCategoryId)
        {
            ResponseMapper<dynamic> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.ProductPrices, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var parameters = base.GetDataTableParameters; 
                var items = await _productPriceService.GetProductPriceForDataTable(parameters, childCategoryId);

                return Ok(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in GetProductPriceForDataTable");
            }

            return Ok(response);
        }

        [HttpDelete, Route("api/ProductPrice/Delete")]
        public async Task<IActionResult> Delete(Guid guid)
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.ProductPrices, AllowPermission.Delete))
                {
                    return Ok(accessResponse);
                }

                var item = await _productPriceService.Delete(guid);
                response.Delete(item);
                ClearCache();
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in Delete");
            }

            return Ok(response);
        }

        [HttpGet, Route("api/ProductPrice/GetAllForDropDownList")]
        public async Task<IActionResult> GetAllForDropDownList()
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                var items = await _productPriceService.GetAllForDropDownList(IsEnglish);
                response.GetAll(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in GetAllForDropDownList");
            }

            return Ok(response);
        }

        [HttpPost, Route("api/ProductPrice/ImportFromExcel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportFromExcel([FromForm] IFormFile file, [FromForm] int childCategoryId, [FromForm] int priceTypeCategoryId, [FromForm] int priceTypeId)
        {
            ResponseMapper<bool> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.ProductPrices, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                if (file == null || file.Length == 0)
                {
                    response.Message = "No file uploaded.";
                    response.Success = false;
                    return Ok(response);
                }

                var result = await _productPriceService.ImportProductPriceFromExcel(file, childCategoryId, priceTypeCategoryId, priceTypeId);
                if (result)
                {
                    response.Success = true;
                    response.Message = "Product prices imported successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to import product prices.";
                }
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in ImportFromExcel");
            }

            return Ok(response);
        }

        private void ClearCache()
        {
            _memoryCache.Remove("ProductPrices");
        }

       
        #endregion
    }
}
