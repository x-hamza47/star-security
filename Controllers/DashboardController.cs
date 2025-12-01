using Microsoft.AspNetCore.Mvc;

namespace Star_Security.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
