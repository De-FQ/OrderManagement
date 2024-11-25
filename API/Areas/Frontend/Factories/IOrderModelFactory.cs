using Utility.Models.Frontend.GeneralDto;

namespace API.Areas.Frontend.Factories
{
    public interface IOrderModelFactory
    {
        //Task<OrderDto> GetDetailById(long id);
        Task<OrderDto?> GetOrder(int orderId);
    }
}
