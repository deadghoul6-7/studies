using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ProjectAspEShop2024.Models
{
    public class CategoryViewModel
    {
        [HiddenInput]
        public long CategoryId { get; set; }

        [Required]
        [Display(Name="Наименование категории")]
        public string Name { get; set; }
    }
}
