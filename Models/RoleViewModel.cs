using System.ComponentModel.DataAnnotations;

namespace ProjectAspEShop2024.Models
{
    public class RoleViewModel
    {
        [Display(Name="Индентификатор роли")]
        public string Id { get; set; }

        [Display(Name = "Наименование роли")]
        public string Name { get; set; }

        public List<RoleClaimsViewModel> RoleClaims { get; set; }
    }

    public class RoleClaimsViewModel
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public bool Selected { get; set; }
    }
}
