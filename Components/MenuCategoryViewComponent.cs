using Microsoft.AspNetCore.Mvc;
using ProjectAspEShop2024.DataAccessLayer;

namespace ProjectAspEShop2024.Components
{
    [ViewComponent]
    public class MenuCategoryViewComponent
        :ViewComponent
    {
        private readonly ShopContext _context;

        public MenuCategoryViewComponent(ShopContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _context
                .Categories
                .Select(c => c.Name)
                .ToList();

            return View(categories);
        }
    }
}
