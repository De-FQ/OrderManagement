using Microsoft.AspNetCore.Mvc;
using Utility.API;
using Utility.LoggerService;
using Utility.Models.Frontend.GeneralDto;
using Utility.ResponseMapper;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Web.Controllers
{
    public class OrderController : BaseController
    {
        public OrderController(IRazorViewEngine razorViewEngine, IAPIHelper apiHelper, AppSettingsModel options, IHttpContextAccessor httpContextAccessor) :
            base(razorViewEngine, apiHelper, options, httpContextAccessor)
        {
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<JsonResult> AddOrder([FromForm] OrderDto orderDto)
        {
            try
            {
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

        // Additional actions (e.g., order history, order details) can be added here
    }
}
