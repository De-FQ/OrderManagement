using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Utility.Models.Frontend.GeneralDto;
using Data.EntityFramework;
using Data.Model.General;

namespace Services.Frontend.GeneralWeb.OrderSaving
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderDto?> GetOrder(int orderId)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.Id == orderId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (order == null)
                return null;

            return new OrderDto
            {
                Id = order.Id,
                Guid = order.Guid,
                CustomerName = order.CustomerName,
                CustomerContact = order.CustomerContact,
                PaymentMethod = order.PaymentMethod,
                AmountReceived = order.AmountReceived,
                TotalAmount = order.TotalAmount,
                ChangeToReturn = order.ChangeToReturn,
                OrderDate = order.OrderDate,
                SerialNumber = order.SerialNumber,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    Guid = oi.Guid,
                    PriceTypeName = oi.PriceTypeName,
                    Quantity = oi.Quantity,
                    Total = oi.Total,
                    CreatedOn = oi.CreatedOn,
                    ModifiedOn = oi.ModifiedOn
                }).ToList()
            };
        }

        public async Task<bool> AddOrder(OrderDto orderDto)
        {
            if (orderDto == null || orderDto.OrderItems == null)
                throw new ArgumentNullException(nameof(orderDto));

            var order = new Order
            {
                Guid = orderDto.Guid,
                CustomerName = orderDto.CustomerName,
                CustomerContact = orderDto.CustomerContact,
                PaymentMethod = orderDto.PaymentMethod,
                AmountReceived = orderDto.AmountReceived,
                TotalAmount = orderDto.TotalAmount,
                ChangeToReturn = orderDto.ChangeToReturn,
                OrderDate = orderDto.OrderDate,
                SerialNumber = orderDto.SerialNumber,
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in orderDto.OrderItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    Guid = item.Guid,
                    PriceTypeName = item.PriceTypeName,
                    Quantity = item.Quantity,
                    Total = item.Total
                });
            }

            _dbContext.Orders.Add(order);
            return await _dbContext.SaveChangesAsync() > 0;
        }

    }
}
