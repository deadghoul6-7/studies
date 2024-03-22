using Microsoft.AspNetCore.Mvc;

namespace ProjectAspEShop2024.Controllers
{
    public class TestAngularController 
        : Controller
    {
        public IActionResult EditCartView()
        {
            return View();
        }
    }
}
