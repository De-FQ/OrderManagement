using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Utility.API;
using Utility.LoggerService;
using Utility.Models.Frontend.GeneralDto;
using Utility.ResponseMapper;

namespace Web.Controllers
{
    public class CategoriesController : BaseController
    {

        public CategoriesController(IRazorViewEngine razorViewEngine, IAPIHelper apiHelper, AppSettingsModel options, IHttpContextAccessor httpContextAccessor) :
          base(razorViewEngine, apiHelper, options, httpContextAccessor)
        { }
        //public IActionResult Category()
        //{
        //    return View();
        //}

        public IActionResult SubCategory()
        {
            return View();
        }

        public IActionResult ChildCategory()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<JsonResult> AddOrder([FromForm] OrderDto orderDto)
        {
            try
            {
                // Call API to place the order
                var responseOrder = await _apiHelper.PostAsync<ResponseMapper<dynamic>>("Order/AddOrder", orderDto);
                if (responseOrder.Success)
                {
                    return Json(new { success = true, message = "Order placed successfully.", data = responseOrder.Data });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to place the order. Please try again." });
                }
            }
            catch (Exception ex)
            {
                AppSeriLog.LogInfo(ex.Message, typeof(OrderController).Name, Serilog.Events.LogEventLevel.Error);
                return Json(new { success = false, message = "An error occurred while placing the order." });
            }
        }
    }
}
