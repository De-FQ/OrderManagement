using API.Helpers;
using AutoMapper;
using Services.Frontend.GeneralWeb.OrderSaving;
using Services.Web.Categorys;
using Utility.API;
using Utility.LoggerService;
using Utility.Models.Admin.DTO;
using Utility.Models.Frontend.GeneralDto;
using Utility.Models.Frontend.HomePage;

namespace API.Areas.Frontend.Factories
{
    public class CommonModelFactory : ICommonModelFactory
    {
        private readonly IWebCategoryService _categoryService;
        private readonly AppSettingsModel _appSettings;
        private readonly IMapper _mapper;
        private readonly string _factoryName;
        private readonly IOrderService _orderService;

        public CommonModelFactory(
            IWebCategoryService categoryService,
            IMapper mapper,
            IOrderService orderService,
            AppSettingsModel options)
        {

            _categoryService = categoryService;
            _mapper = mapper;
            _appSettings = options;
            _factoryName = typeof(CommonModelFactory).Name;
            _orderService = orderService;
        }

        //public async Task<List<CategoryDto>> GetCategories(bool isEnglish)
        //{
        //    List<CategoryDto> categories = new();
        //    try
        //    {
        //        var categoryList = await _categoryService.GetAllCategories(lang: isEnglish);
        //        if (categoryList != null)
        //        {
        //            categories = categoryList.Select(category => _mapper.Map<CategoryDto>(category)).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        AppSeriLog.LogInfo(ex.Message, _factoryName, Serilog.Events.LogEventLevel.Error);
        //    }

        //    return categories;
        //}

        public async Task<bool> AddOrderForm(OrderDto order)
        {

            return await _orderService.AddOrder(order);

        }
    }
}
