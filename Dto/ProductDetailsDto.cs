using AutoMapper;
using ProjectAspEShop2024.Domains;
using System.ComponentModel.DataAnnotations;

namespace ProjectAspEShop2024.Dto
{
    public class ProductDetailsDto
    {
        [Display(Name ="Штрихкод")]
        public long ProductId { get; set; }

        [Required]
        [Display(Name = "Наименование")]
        public string Name { get; set; }

        [Display(Name = "Цена (руб)")]
        [DataType(DataType.Currency)]
        public int Price { get; set; }

        [Display(Name = "Количество на складе (шт)")]
        public int Quantity { get; set; }

        public string? PhotoUrl { get; set; }

        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [Display(Name = "Категория товара")]
        public string CategoryName { get; set; }

        [Display(Name = "Бренд")]
        public string BrandName { get; set; }
    }
}
