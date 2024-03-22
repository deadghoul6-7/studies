using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectAspEShop2024.BusinesLogic;
using ProjectAspEShop2024.DataAccessLayer;
using ProjectAspEShop2024.Dto;
using ProjectAspEShop2024.Helpers;

namespace ProjectAspEShop2024.Controllers
{
    public class CartController : Controller
    {
        private readonly ShopContext _context;
        private readonly IMapper _mapper;

        public CartController(ShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // RemoveFromCart
        public IActionResult RemoveFromCart(long productId, int count = 1)
        {
            var product = _context.Products
                .FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                ViewBag.ErrorMessage = $"не найден продукт с id '{productId}'";
                return View("Error");
            }

            var productDto = _mapper.Map<ProductDto>(product);
            // нашли - удаляем из корзины
            var cart = HttpContext.GetSubject<Cart>();
            cart.RemoveProduct(productDto, count);
            // корзину сохранить в сессию
            HttpContext.SaveSubject<Cart>(cart);

            return View("CartView", cart);
        }

        public IActionResult AddToCart(long productId, string returnUrl)
        {
            var product = _context.Products
                .FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                ViewBag.ErrorMessage = $"не найден продукт с id '{productId}'";
                return View("Error");
            }

            var productDto = _mapper.Map<ProductDto>(product);
            // нашли - добавляем в корзину
            var cart = HttpContext.GetSubject<Cart>();
            cart?.AddProduct(productDto);
            // корзину сохранить в сессию
            HttpContext.SaveSubject(cart);

            if (String.IsNullOrEmpty(returnUrl))
            {
                return View("CartView", cart);
            }
            else
            {
                return new RedirectResult(returnUrl);
            }
        }

        public IActionResult CartView(string returnUrl)
        {
            var cart = HttpContext.GetSubject<Cart>();
            if (!String.IsNullOrEmpty(returnUrl))
            {
                cart.ContinueShoppingUrl = returnUrl;
                HttpContext.SaveSubject(cart);
            }
            return View(cart);
        }

        // ContinueShopping
        public IActionResult ContinueShopping()
        {
            var cart = HttpContext.GetSubject<Cart>();

            if (String.IsNullOrEmpty(cart.ContinueShoppingUrl))
            {
                return RedirectToAction("Index", "Product");
            }
            else
            {
                return new RedirectResult(cart.ContinueShoppingUrl);
            }
        }
    }
}
