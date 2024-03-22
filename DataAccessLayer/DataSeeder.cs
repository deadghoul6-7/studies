using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectAspEShop2024.AppIdentity;
using ProjectAspEShop2024.Domains;

namespace ProjectAspEShop2024.DataAccessLayer
{
    public class DataSeeder
    {
        public static void Seed(IServiceProvider provider)
        {
            SeedProducts(provider);
            SeedRolesAsync(provider);
        }

        private static async void SeedRolesAsync(IServiceProvider provider)
        {
            var roleManager = provider
                .GetService<RoleManager<IdentityRole>>();

            if (roleManager.Roles.Count() > 0)
                return;

            var userManager = provider
                .GetService<UserManager<AppUser>>();

            string[] mainRoles = 
            { 
                "admin", 
                "content_manager",
                "sales_manager",
                "user"
            };
            string[] mainUsers =
            {
                "adminVasya",
                "contentPetya",
                "salesKolya",
                "userTolya"
            };

            IdentityResult identityResult;

            foreach (var roleName in mainRoles)
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    identityResult = await roleManager
                        .CreateAsync(new IdentityRole(roleName));

                    if (identityResult != IdentityResult.Success)
                    {
                        throw new Exception("Error while create IdentityRole");
                    }
                }
            }

            for (int i = 0; i < mainUsers.Length; i++)
            {
                CreateUserWithRoleAsync(userManager, mainUsers[i], mainRoles[i]);
            }
        }

        private static async void CreateUserWithRoleAsync(
            UserManager<AppUser> userManager, string userName, string roleName)
        {
            string defaultPassword = "!Qwerty1";

            var user = new AppUser
            {
                UserName = $"{userName}@ya.ru",
                Email = $"{userName}@ya.ru"
            };
            
            /* var userExists = await userManager.FindByEmailAsync(user.Email);
            if (userExists != null)
                return;*/

            var identityResult = userManager
                .CreateAsync(user, defaultPassword).Result;

            if (identityResult == IdentityResult.Success)
            {
                var token = userManager
                    .GenerateEmailConfirmationTokenAsync(user).Result;

                identityResult = userManager.ConfirmEmailAsync(user, token)
                    .Result;

                identityResult = userManager.AddToRoleAsync(user, roleName)
                    .Result;
            }
        }

        private static void SeedProducts(IServiceProvider provider)
        {
            // контекст для БД
            var context = provider
                .GetRequiredService<ShopContext>();

            // проверям базу на наличие записей
            if (context.Products.Count() > 0)
            {
                return;
            }

            // заполняем базу начальными данными
            var di = new DirectoryInfo("ProductJson");
            var fileJson = di.GetFiles("*.json");

            foreach (var file in fileJson)
            {
                // название категории - имя файла
                string categoryString = Path
                    .GetFileNameWithoutExtension(file.Name);

                Category category = GetOrCreateCategory(context, categoryString);

                string jsonString = File
                    .ReadAllText(file.FullName);

                var jarray = JArray.Parse(jsonString);

                foreach (var j in jarray)
                {
                    string name = (string)j["name"];
                    string priceString = (string)j["price"];
                    string brandString = (string)j["brand"];
                    string pictureString = (string)j["picture"];

                    Brand brand = GetOrCreateBrand(context, brandString);

                    var product = new Product
                    {
                        Name = name,
                        PhotoUrl = pictureString,
                        Price = int.Parse(priceString),
                        Quantity = 100,
                        BrandOfProduct = brand,
                        CategoryOfProduct = category
                    };

                    context.Products.Add(product);
                }

                context.SaveChanges();
            }
        }

        public static Category GetOrCreateCategory(ShopContext context, string categoryString)
        {
            var category = context.Categories
                .FirstOrDefault(x => x.Name == categoryString);

            if (category == null) 
            {
                category = new Category
                {
                    Name = categoryString
                };
                context.Categories.Add(category);
                context.SaveChanges();
            }

            return category;
        }

        public static Brand GetOrCreateBrand(ShopContext context, string? brandString)
        {
            var brand = context.Brands
                .FirstOrDefault(x => x.Name == brandString);

            if (brand == null)
            {
                brand = new Brand
                {
                    Name = brandString
                };
                context.Brands.Add(brand);
                context.SaveChanges();
            }

            return brand;
        }
    }
}
