using ProjectAspEShop2024.Dto;
using ProjectAspEShop2024.Filters;

namespace ProjectAspEShop2024.Models
{
    public class ProductsPageViewModel
    {
        public FilterProductViewModel Filters { get; set; }

        public List<ProductDto> Products { get; set; }

        public string ActiveCategory { get; set; }

        public int ActivePage { get; set; }

        public int PageQuantity { get; set; }
    }
}
