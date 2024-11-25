using System.Threading.Tasks;
using Utility.Models.Frontend.GeneralDto;

namespace Services.Frontend.GeneralWeb.OrderSaving
{
    public interface IOrderService
    {
        Task<OrderDto?> GetOrder(int orderId);
        Task<bool> AddOrder(OrderDto order);
    }
}
