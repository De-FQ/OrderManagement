using Microsoft.AspNetCore.Mvc;
using Services.Frontend.GeneralWeb.OrderSaving;
using Utility.Models.Frontend.GeneralDto;
using System.Threading.Tasks;
using API.Helpers;
using Utility.LoggerService;
using Utility.ResponseMapper;
using System;

namespace WebApi.Controllers
{
    [Route("webapi/Order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost, Route("AddOrder")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> AddOrder([FromForm] OrderDto orderDto)
        {
            var response = new ResponseMapper<dynamic>();
            try
            {
                 orderDto.Guid = Guid.NewGuid();
                foreach (var item in orderDto.OrderItems)
                {
                    item.Guid = Guid.NewGuid();
                }
                bool result = await _orderService.AddOrder(orderDto);
                if (result)
                {
                    response.Success = true;
                    response.Message = "Order has been successfully added.";
                }
                else
                {
                    response.Message = "Failed to add the order.";
                }
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                AppSeriLog.LogInfo(ex.Message, typeof(OrderController).Name, Serilog.Events.LogEventLevel.Error);
                response.Message = ex.Message;
                return Ok(response);
            }
            catch (Exception ex)
            {
                AppSeriLog.LogInfo(ex.Message, typeof(OrderController).Name, Serilog.Events.LogEventLevel.Error);
                response.Message = "An error occurred while processing your request.";
                return Ok(response);
            }
        }



        [HttpGet, Route("GetOrder/{orderId}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            var response = new ResponseMapper<OrderDto>();
            try
            {
                var order = await _orderService.GetOrder(orderId);

                if (order != null)
                {
                    response.Success = true;
                    response.Data = order;
                    response.Message = "Order retrieved successfully.";
                }
                else
                {
                    response.Message = "Order not found.";
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                AppSeriLog.LogInfo(ex.Message, typeof(OrderController).Name, Serilog.Events.LogEventLevel.Error);
                response.Message = "An error occurred while processing your request.";
                return Ok(response);
            }
        }
    }
}
