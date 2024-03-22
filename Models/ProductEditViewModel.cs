using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectAspEShop2024.DataAccessLayer;
using ProjectAspEShop2024.Domains;
using System.ComponentModel.DataAnnotations;

namespace ProjectAspEShop2024.Models
{
    public class ProductEditViewModel
    {
        /// <summary>
        /// url-адрес для возврата на предыдущий маршрут
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// для загрузки картинки на сервер
        /// </summary>
        [ValidateNever]
        public IFormFile ImageFile { get; set; }


        [Required]
        [Display(Name = "Наименование")]
        public string Name { get; set; }

        [Display(Name ="Изменить бренд")]
        public long BrandId { get; set; }

        [Display(Name = "Изменить категорию")]
        public long CategoryId { get; set; }


        [Display(Name = "Штрихкод")]
        public long ProductId { get; set; }

        [Display(Name = "Цена (руб)")]
        [DataType(DataType.Currency)]
        public int Price { get; set; }

        [Display(Name = "Количество на складе (шт)")]
        public int Quantity { get; set; }


        [ValidateNever]
        public string? PhotoUrl { get; set; }

        [Display(Name = "Описание")]
        [ValidateNever]
        public string? Description { get; set; }

        [Display(Name = "Категория товара")]
        [ValidateNever]
        public string CategoryName { get; set; }

        [Display(Name = "Бренд")]
        [ValidateNever]
        public string BrandName { get; set; }

        [ValidateNever]
        public List<SelectListItem> BrandsList { get; set; }

        [ValidateNever]
        public List<SelectListItem> CategoriesList { get; set; }

        public void Initialize(ShopContext context, Product product = null)
        {
            if (product != null)
            {
                BrandId = product.BrandOfProduct.BrandId;
                CategoryId = product.CategoryOfProduct.CategoryId;
                BrandName = product.BrandOfProduct.Name;
                CategoryName = product.CategoryOfProduct.Name;
            }

            var categoriesQuery = context.GetAllCategories();
            var brandQuery = context.GetAllBrands();

            BrandsList = brandQuery
                .Select(b => new SelectListItem
                {
                    Text = b.Name,
                    Value = b.BrandId.ToString(),
                })
                .ToList();

            CategoriesList = categoriesQuery
                .Select(b => new SelectListItem
                {
                    Text = b.Name,
                    Value = b.CategoryId.ToString(),
                })
                .ToList();
        }
    }
}
