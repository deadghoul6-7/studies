using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAspEShop2024.DataAccessLayer;
using ProjectAspEShop2024.Domains;
using ProjectAspEShop2024.Dto;
using ProjectAspEShop2024.Filters;
using ProjectAspEShop2024.Helpers;
using ProjectAspEShop2024.Models;
using System.Linq;

namespace ProjectAspEShop2024.Controllers
{
    public class ProductController : Controller
    {
        // 1) в контроллере
        // не подходит - контроллер сбрасывается
        // private List<ProductDto> _cart;

        // 2) статический список (корзина)
        // не подходит! - у каждого юзера д/б своя

        // 3) корзина - хранить в базе! под каждого юзера
        // - нормальный.

        // 4) корзину хранить в локальной СЕССИИ
        // сессия - область памяти под каждого юзера
        // огранич время хранения

        private readonly ShopContext _context;
        private readonly IMapper _mapper;
        private int _pageSize = 10;

        // паттерн - Инъекция Зависимости
        public ProductController(ShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult FilterForListBrand(List<BrandCheck> brandChecks, string returnUrl)
        {
            var filter = HttpContext.GetSubject<FilterProduct>();
            
            filter.ListSelectedBrandsId = brandChecks
                .Where(b => b.IsChecked)
                .Select(b => b.BrandId)
                .ToList();

            HttpContext.SaveSubject(filter);

            return new RedirectResult(returnUrl);
        }

        public IActionResult ResetBrandsFilter(string returnUrl)
        {
            var filter = HttpContext.GetSubject<FilterProduct>();
            filter.ListSelectedBrandsId = null;
            HttpContext.SaveSubject(filter);

            return new RedirectResult(returnUrl);
        }

        public IActionResult ResetSearchText(string returnUrl)
        {
            var filter = HttpContext.GetSubject<FilterProduct>();
            filter.SearchText = null;
            HttpContext.SaveSubject(filter);

            return new RedirectResult(returnUrl);
        }

        public IActionResult FilterProductByName(string searchText, string returnUrl)
        {
            var filter = HttpContext.GetSubject<FilterProduct>();
            filter.SearchText = searchText;
            HttpContext.SaveSubject(filter);

            return new RedirectResult(returnUrl);
        }

        [HttpPost]
        public IActionResult ChangeFilter(FilterProductViewModel model, string returnUrl)
        {
            var filter = HttpContext.GetSubject<FilterProduct>();
            filter = _mapper.Map<FilterProduct>(model);

            HttpContext.SaveSubject(filter);

            return new RedirectResult(returnUrl);
        }

        public IActionResult SwitchOnSortByName(int direction, string returnUrl)
        {
            var filter = HttpContext.GetSubject<FilterProduct>();

            filter.SortByName = (SortType)direction;

            HttpContext.SaveSubject(filter);

            return new RedirectResult(returnUrl);
        }

        public IActionResult SwitchOnSortByPrice(int direction, string returnUrl)
        {
            var filter = HttpContext.GetSubject<FilterProduct>();

            filter.SortByPrice = (SortType)direction;

            HttpContext.SaveSubject(filter);

            return new RedirectResult(returnUrl);
        }

        public IActionResult Index(string categoryName = null, int page = 1)
        {
            var queryAll = _context
                .Products
                .OrderBy(p => p.ProductId);

            var queryProducts = String.IsNullOrEmpty(categoryName) ?
                queryAll : queryAll
                .Include(p => p.CategoryOfProduct)
				.Include(p => p.BrandOfProduct)
				.Where(p => p.CategoryOfProduct.Name == categoryName)
                .OrderBy(p => p.ProductId);

			var filter = HttpContext.GetSubject<FilterProduct>();

            filter.CategoryId = _context.Categories
                .FirstOrDefault(c => c.Name == categoryName)?
                .CategoryId;
			queryProducts = ApplyFilters(filter, queryProducts);

			HttpContext.SaveSubject(filter);

			var productsPage = queryProducts
                .Skip((page - 1) * _pageSize)
                .Take(_pageSize);

            int pageQuantity = (int) Math.Ceiling(
                queryProducts
                .Count()/ (float)_pageSize);
            // 10/3 = 3.3333 = 4 страницы

            var filterViewModel = _mapper
                .Map<FilterProductViewModel>(filter);
            filterViewModel.Initialize(_context, filter.CategoryId);

            var model = new ProductsPageViewModel
            {
                Filters = filterViewModel,
                Products = productsPage
                    .Select(p => _mapper.Map<ProductDto>(p))
                    .ToList(),
                ActivePage = page,
                PageQuantity = pageQuantity,
                ActiveCategory = categoryName
            };

            return View(model);
        }

        private IOrderedQueryable<Product> ApplyFilters(
            FilterProduct filter, IOrderedQueryable<Product> queryAll)
        {
			var query = queryAll.Where(p => 
                p.Price >= filter.MinPrice &&
                p.Price <= filter.MaxPrice);

            if (filter.BrandId != null)
            {
                query = query
                    .Where(p => p.BrandOfProduct.BrandId == filter.BrandId);
            }
            if (filter.ListSelectedBrandsId != null)
            {
                query = query.
                    Where(p => filter.ListSelectedBrandsId.Contains(p.BrandOfProduct.BrandId));
            }

            // фильтр по тексту
            if (!String.IsNullOrEmpty(filter.SearchText))
            {
                query = query
                    .Where(p => p.Name.ToLower()
                    .Contains(filter.SearchText.ToLower()));
            }

            queryAll = query.OrderBy(p => p.ProductId);

            switch (filter.SortByName)
            {
                case SortType.Ascending:
                    queryAll = query.OrderBy(p => p.Name);
                    break;
                case SortType.Descending:
                    queryAll = query.OrderByDescending(p => p.Name);
                    break;
            }
            switch (filter.SortByPrice)
            {
                case SortType.Ascending:
                    queryAll = query.OrderBy(p => p.Price);
                    break;
                case SortType.Descending:
                    queryAll = query.OrderByDescending(p => p.Price);
                    break;
            }

           

            return queryAll;
        }

        public IActionResult DetailsView(long productId, string returnUrl)
        {
            var product = _context.Products
                    .Include(p => p.CategoryOfProduct)
                    .Include(p => p.BrandOfProduct)
                    .FirstOrDefault(p => p.ProductId == productId);

            var productDto = _mapper
                .Map<ProductDetailsDto> (product);

            if (product == null)
            {
                ViewBag.ErrorMessage = $"Не найден продукт с Id={productId}";
                return View("Error");
            }

            ViewBag.ReturnUrl = returnUrl;
            
            return View(productDto);
        }
    }
}
