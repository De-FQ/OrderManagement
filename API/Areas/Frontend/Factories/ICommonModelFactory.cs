using System.Threading.Tasks;
using Utility.Models.Admin.DTO;
using Utility.Models.Frontend.Attraction;
using Utility.Models.Frontend.GeneralDto;
using Utility.Models.Frontend.HomePage;
using Utility.ResponseMapper;

namespace API.Areas.Frontend.Factories
{
    public interface ICommonModelFactory
    {
        Task<bool> AddOrderForm(OrderDto order);
    }


}
   
