using API.Helpers;
using AutoMapper;
using Data.Model.General;
using Microsoft.Extensions.Options;
using Services.Frontend.GeneralWeb.OrderSaving;
using Utility.API;
using Utility.LoggerService;
using Utility.Models.Frontend.GeneralDto;
using Utility.ResponseMapper;

namespace API.Areas.Frontend.Factories
{
    public class OrderModelFactory : IOrderModelFactory
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly string _factoryName = nameof(OrderModelFactory);

        public OrderModelFactory(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        public async Task<OrderDto?> GetOrder(int orderId)
        {
            OrderDto? orderDto = null;
            try
            {
                var order = await _orderService.GetOrder(orderId);
                if (order != null)
                {
                    orderDto = _mapper.Map<OrderDto>(order);
                }
            }
            catch (Exception ex)
            {
                AppSeriLog.LogInfo(ex.Message, _factoryName, Serilog.Events.LogEventLevel.Error);
            }

            return orderDto;
        }
    }
}
