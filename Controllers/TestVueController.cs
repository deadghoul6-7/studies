using Microsoft.AspNetCore.Mvc;

namespace ProjectAspEShop2024.Controllers
{
    public class TestVueController 
        : Controller
    {
        public IActionResult EditCartView()
        {
            return View();
        }
    }
}
