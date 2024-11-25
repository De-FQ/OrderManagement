using Data.Model.General;
using Microsoft.AspNetCore.Http;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.API;
using Utility.Models.Admin.DTO;
using Utility.Models.Base;

namespace Services.Backend.Price
{
    public interface IPriceTypeService<T> : IBaseService<T>
    {
        Task<DataTableResult<List<PriceTypeDto>>> GetPriceTypeForDataTable(DataTableParam param, int? priceTypeCategoryId);
        Task<List<PriceType>> GetPriceTypesByPriceTypeCategoryId(int priceTypeCategoryId);
        Task<PriceType> Create(PriceType model);
        Task<bool> Update(PriceType model);
        Task<bool> ImportPriceTypeFromExcel(IFormFile file, int priceTypeCategoryId);
    }
}
