using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjectAspEShop2024.Controllers
{
    public class TestSignalRController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Общая комната (все со всеми)
        /// </summary>
        public ActionResult TestChatRoom()
        {
            return View();
        }

        /// <summary>
        /// Чат с менеджером по продажам
        /// </summary>
        public ActionResult TestChatManager()
        {
            return View();
        }
    }
}
