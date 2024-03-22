using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectAspEShop2024.AppIdentity;
using ProjectAspEShop2024.DataAccessLayer;

namespace ProjectAspEShop2024.Controllers
{
    [Authorize(Roles = "sales_manager")]
    public class SalesManagerController : Controller
    {
        private readonly ShopContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger _logger;

        public SalesManagerController(
            ShopContext context,
            IMapper mapper,
            UserManager<AppUser> userManager,
            ILogger<ContentManagerController> logger)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ChatClientsView()
        {
            return View();
        }

		public ActionResult MaterialChatManagerView()
		{
			return View();
		}
	}
}
