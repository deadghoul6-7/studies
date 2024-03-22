using ProjectAspEShop2024.Models;
using System.Reflection;
using System.Security.Claims;
using static ProjectAspEShop2024.AppIdentity.Permissions;

namespace ProjectAspEShop2024.AppIdentity
{
    public static class ClaimsHelper
    {
        public static bool CanEditPrice(this ClaimsPrincipal principal)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity;

            bool userCanEditPrice = claimsIdentity
                .HasClaim(nameof(Permissions), Permissions.Products.EditPrice);

            return userCanEditPrice;
        }

        public static List<string> GetPermissions(Type type)
        {
            FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);

            return fields.Select(x => x.Name).ToList();
        }

        public static List<RoleClaimsViewModel> GetAllPermissions()
        {
            string prefix = "Permissions.Products.";
            var products = GetPermissions(typeof(Permissions.Products))
                .Select(x => new RoleClaimsViewModel
                {
                    Type = nameof(Permissions),
                    Value = prefix+x
                }).ToList();

            prefix = "Permissions.Categories.";
            var categories = GetPermissions(typeof(Permissions.Categories))
                .Select(x => new RoleClaimsViewModel
                {
                    Type = nameof(Permissions),
                    Value = prefix+x
                }).ToList();

            prefix = "Permissions.Reviews.";
            var reviews = GetPermissions(typeof(Permissions.Reviews))
                .Select(x => new RoleClaimsViewModel
                {
                    Type = nameof(Permissions),
                    Value = prefix + x
                }).ToList();

            return products.Union(categories).Union(reviews).ToList();
        }
    }
}
