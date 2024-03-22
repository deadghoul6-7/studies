using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectAspEShop2024.DataAccessLayer;
using ProjectAspEShop2024.Domains;
using ProjectAspEShop2024.Filters;
using System.ComponentModel.DataAnnotations;

namespace ProjectAspEShop2024.Models
{
	public class FilterProductViewModel
	{
		public string SearchText { get; set; }

        public List<long>? ListSelectedBrandsId { get; set; }

        public List<BrandCheck> ListBrandCheck { get; set; }

		#region свойства для формирования html "select" Бренда

		[Display(Name ="Фильтр по Бренду")]
		public long? BrandId { get; set; }

		public List<SelectListItem> BrandsList { get; set; }

		public FilterProductViewModel()
		{ }

		public void Initialize(ShopContext context, long? categoryId = null)
		{
            // список брендов может зависеть от выбранной категории
            // максимальная границы цены также зависит от категории
            var activeCategory = context.GetActiveCategory(categoryId);
            var brandQuery = context.GetBrandsOfCategory(categoryId);

            BrandsList = brandQuery
				.Select(b => new SelectListItem
				{
					Text = b.Name,
					Value = b.BrandId.ToString(),
				})
				.ToList();

			ListBrandCheck = brandQuery
				.Select(b => new BrandCheck
				{
					BrandId = b.BrandId,
					BrandName = b.Name, 
					IsChecked = 
					this.ListSelectedBrandsId == null || this.ListSelectedBrandsId.Count == 0
						? false 
						: this.ListSelectedBrandsId.Contains(b.BrandId)
				})
				.ToList();

			if (activeCategory != null)
			{
				var products = activeCategory
					.ProductsOfCategory;

				if (products.Count == 0)
				{
					this.MaxPriceEdge = 10000;
				}
				else
				{
                    this.MaxPriceEdge = activeCategory
                    .ProductsOfCategory
                    .Max(p => p.Price);
                }
			}
			else
			{
				MaxPriceEdge = context.Products.Max(p => p.Price);
			}

			// пункт - ничего не выбрано! (все)
			BrandsList.Insert(0, new SelectListItem
			{
				Value = null,
				Text = "ВСЕ"
			});

			if (MaxPrice > MaxPriceEdge)
				MaxPrice = MaxPriceEdge;

			if (MinPrice < 0)
				MinPrice = 0;
		}

		#endregion

		public int MaxPriceEdge { get; set; }

		public int MinPrice { get; set; }

		public int MaxPrice { get; set; }

	}
}
