using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectAspEShop2024.DataAccessLayer;
using ProjectAspEShop2024.Filters;
using ProjectAspEShop2024.Helpers;
using ProjectAspEShop2024.Models;

namespace ProjectAspEShop2024.Components
{
    [ViewComponent]
    public class BrandsCheckViewComponent
        :FiltersWidgetViewComponent
    {
        public BrandsCheckViewComponent(ShopContext context, IMapper mapper)
            :base(context, mapper) { }

        public override IViewComponentResult Invoke()
        {
            var model = base.GetFilterProductViewModel();

            return View(model.ListBrandCheck);
        }
    }
}
