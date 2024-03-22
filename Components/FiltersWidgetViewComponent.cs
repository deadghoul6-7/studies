using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectAspEShop2024.BusinesLogic;
using ProjectAspEShop2024.DataAccessLayer;
using ProjectAspEShop2024.Filters;
using ProjectAspEShop2024.Helpers;
using ProjectAspEShop2024.Models;

namespace ProjectAspEShop2024.Components
{
    [ViewComponent]
    public class FiltersWidgetViewComponent 
        : ViewComponent
    {
		private readonly ShopContext _context;
		private readonly IMapper _mapper;
		private FilterProduct _filter;

        public FiltersWidgetViewComponent(ShopContext context, IMapper mapper)
        {
			_context = context;
			_mapper = mapper;
		}

        protected FilterProductViewModel GetFilterProductViewModel()
        {
            // загрузить фильтры из Сессии
            _filter = HttpContext.GetSubject<FilterProduct>();

            var model = _mapper.Map<FilterProductViewModel>(_filter);
            model.Initialize(_context, _filter.CategoryId);

            return model;
        }

        public virtual IViewComponentResult Invoke()
        {
            var model = GetFilterProductViewModel();

            return View(model);
        }
    }
}
