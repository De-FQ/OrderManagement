//using API.Helpers;
//using AutoMapper;
//using Microsoft.Extensions.Options;
//using Services.Frontend.WebCategory;
//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Utility.API;
//using Utility.LoggerService;
//using Utility.Models.Frontend.Category;
//using Utility.ResponseMapper;

//namespace API.Areas.Frontend.Factories
//{
//    public class CategoryModelFactory : ICategoryModelFactory
//    {
//        private readonly IWebCategoryService _categoryService;
//        private readonly AppSettingsModel AppSettings;
//        private readonly IMapper _mapper;
//        private readonly string _factoryName;
//        private readonly string _categoryImageUrl;

//        public CategoryModelFactory(IWebCategoryService categoryService, IMapper mapper, AppSettingsModel options)
//        {
//            _categoryService = categoryService;
//            _mapper = mapper;
//            AppSettings = options;
//            _categoryImageUrl = AppSettings.AppSettings.APIBaseUrl + AppSettings.ImageSettings.Categories;
//            _factoryName = typeof(CategoryModelFactory).Name;
//        }

//        public async Task<CategoryLandingListDto> GetLandingPageList(bool isEnglish, string seoName = "")
//        {
//            CategoryLandingListDto model = new();

//            try
//            {
//                var categories = await _categoryService.GetAllCategories(isEnglish);
//                model.Categories = categories.Select(category => new CategoryItemDto
//                {
//                    Guid = category.Guid,
//                    Name = category.Name,
//                    Description = category.Description,
//                    ImageUrl = _categoryImageUrl + category.ImageUrl,
//                    DisplayOrder = category.DisplayOrder,
//                    ShowInMenu = category.ShowInMenu
//                }).ToList();
//            }
//            catch (Exception ex)
//            {
//                AppSeriLog.LogInfo(ex.Message, _factoryName, Serilog.Events.LogEventLevel.Error);
//            }

//            return model;
//        }

//        public async Task<CategoryDetailDto> GetDetailBy(bool isEnglish, Guid guid)
//        {
//            CategoryDetailDto model = new();
//            try
//            {
//                var category = await _categoryService.GetCategoryByGuid(guid, isEnglish);

//                if (category != null)
//                {
//                    model = _mapper.Map<CategoryDetailDto>(category);
//                    model.Name = category.Name;
//                    model.LongDescription = category.Description;
//                    model.BannerURL = _categoryImageUrl + category.ImageUrl;

//                    var nextCategory = await _categoryService.GetNextCategory(guid);
//                    if (nextCategory != null)
//                    {
//                        model.NextRedirectionURL = "categories/" + nextCategory.Guid;
//                    }
//                    else
//                    {
//                        model.NextRedirectionURL = string.Empty;
//                    }

//                    var previousCategory = await _categoryService.GetPreviousCategory(guid);
//                    if (previousCategory != null)
//                    {
//                        model.PreviousRedirectionURL = "categories/" + previousCategory.Guid;
//                    }
//                    else
//                    {
//                        model.PreviousRedirectionURL = string.Empty;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                AppSeriLog.LogInfo(ex.Message, _factoryName, Serilog.Events.LogEventLevel.Error);
//            }

//            return model;
//        }
//    }
//}