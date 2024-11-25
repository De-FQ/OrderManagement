using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CheckoutData()
        {
            return View();
        }
    }
}
