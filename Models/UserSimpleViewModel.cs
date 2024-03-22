using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ProjectAspEShop2024.Models
{
    public class UserSimpleViewModel
    {
        [Display(Name = "ID")]
        [HiddenInput]
        public string Id { get; set; }

        [Display(Name ="Имя пользователя")]
        public string? Firstname { get; set; }

        [Display(Name = "Фамилия пользователя")]
        public string? Lastname { get; set; }

        [Display(Name = "Логин пользователя")]
        public string? UserName { get; set; }

    }
}
