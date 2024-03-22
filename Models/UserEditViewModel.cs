using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ProjectAspEShop2024.Models
{
    public class UserEditViewModel
    {
        [Display(Name = "ID")]
        public string Id { get; set; }

        [Display(Name = "Имя пользователя")]
        public string? Firstname { get; set; }

        [Display(Name = "Фамилия пользователя")]
        public string? Lastname { get; set; }

        [Display(Name = "Логин пользователя")]
        public string? UserName { get; set; }

        [Display(Name = "Отчество пользователя")]
        public string? Middlename { get; set; }

        [Display(Name = "Фото профиля")]
        public string? PhotoUrl { get; set; }

        [Display(Name = "Почтовый адрес")]
        [Required]
        public string? PostAddress { get; set; }


        public List<IdentityRole> AllRoles { get; set; }


        public List<string> UserRoles { get; set; }

        public List<string> EditRoles { get; set; }

    }
}
