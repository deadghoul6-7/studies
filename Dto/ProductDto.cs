using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Mvc;
using ProjectAspEShop2024.Domains;
using System.ComponentModel.DataAnnotations;

namespace ProjectAspEShop2024.Dto
{
    public class ProductDto
    {
        [Required]
        [Display(Name = "Наименование")]
        public string Name { get; set; }

        [HiddenInput(DisplayValue = false)]
        public long ProductId { get; set; }

        [Required]
        [Display(Name = "Цена (руб)")]
        [DataType(DataType.Currency)]
        public int Price { get; set; }
    }
}
