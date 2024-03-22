using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAspEShop2024.BusinesLogic;
using ProjectAspEShop2024.Helpers;

namespace ProjectAspEShop2024.Components
{
    [ViewComponent]
    public class CartWidgetViewComponent
        :ViewComponent
    {
        private Cart? _cart;

        public CartWidgetViewComponent()
        {
            // загрузить корзину из Сессии
            _cart = HttpContext.GetSubject<Cart>();
        }

        public IViewComponentResult Invoke()
        {
            // загрузить корзину из Сессии
            _cart = HttpContext.GetSubject<Cart>();

            return View(_cart);
        }
    }
}
