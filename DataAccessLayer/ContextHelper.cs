using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectAspEShop2024.AppIdentity;
using ProjectAspEShop2024.Domains;
using ProjectAspEShop2024.Models;
using System.Security.Claims;

namespace ProjectAspEShop2024.DataAccessLayer
{
    public static class ContextHelper
    {
        public async static Task CreateFileDocumentAsync(this ShopContext context,
            UserManager<AppUser> userManager,
            ClaimsPrincipal principal,
            FileDocument model)
        {
            model.ModifiedDate = DateTime.Now;
            model.ModifiedUser = await userManager.GetUserAsync(principal);

            context.FileDocuments.Add(model);

            context.SaveChanges();
        }

        public async static Task UpdateFileDocumentAsync(this ShopContext context,
            UserManager<AppUser> userManager,
            ClaimsPrincipal principal,
            FileDocument model)
        {
            var entity = context.FileDocuments
                .FirstOrDefault(x => x.FileDocumentId == model.FileDocumentId);

            entity.Name = model.Name;
            entity.StorageId = model.StorageId;
            entity.ModifiedDate = DateTime.Now;
            entity.ModifiedUser = await userManager.GetUserAsync(principal);

            context.SaveChanges();
        }

        public static void CreateProduct(this ShopContext context, IMapper _mapper, ProductEditViewModel model)
        {
            var product = _mapper
                .Map<Product>(model);

            var category = context
                    .Categories
                    .First(x => x.CategoryId == model.CategoryId);
            product.CategoryOfProduct = category;

            var brand = context
                    .Brands
                    .First(x => x.BrandId == model.BrandId);
            product.BrandOfProduct = brand;

            context.Products.Add(product);
            
            context.SaveChanges();
        }


        public static void UpdateCategory(this ShopContext context, CategoryViewModel model)
        {
            var entity = context.Categories
                .FirstOrDefault(x => x.CategoryId == model.CategoryId);

            // context.Entry(entity).CurrentValues.SetValues(model);
            entity.Name = model.Name;

            context.SaveChanges();
        }

        public static void UpdateUser(this ShopContext context, UserEditViewModel model)
        {
            var entity = context.Users
                .FirstOrDefault(x => x.Id == model.Id);

            context.Entry(entity).CurrentValues.SetValues(model);

            context.SaveChanges();
        }

        public static void CreateCategory(this ShopContext context, CategoryViewModel model)
        {
            var entity = new Category();
            
            entity.Name = model.Name;

            context.Categories.Add(entity);

            context.SaveChanges();
        }

        public static void DeleteCategory(this ShopContext context, long categoryId)
        {
            var entity = context.Categories
                .FirstOrDefault(x => x.CategoryId == categoryId);

            context.Categories.Remove(entity);

            context.SaveChanges();
        }

        public static void DeleteProduct(this ShopContext context, long productId)
        {
            var entity = context.Products
                .FirstOrDefault(x => x.ProductId == productId);

            context.Products.Remove(entity);

            context.SaveChanges();
        }

        public async static Task UpdateProductAsync(this ShopContext context, 
            UserManager<AppUser> userManager, 
            ClaimsPrincipal principal, 
            ProductEditViewModel model)
        {
            var entity = context.Products
                .Include(x => x.BrandOfProduct)
                .Include(x => x.CategoryOfProduct)
                .FirstOrDefault(x => x.ProductId == model.ProductId);

            int originalPrice = entity.Price;
            
            // меняем простые проперти (цена, количество, название)
            context.Entry(entity).CurrentValues.SetValues(model);

            if (!principal.CanEditPrice())
            {
                entity.Price = originalPrice;
            }

            entity.ModifiedDate = DateTime.Now;
            entity.ModifiedUser = await userManager.GetUserAsync(principal);

            // связи  - отдельно
            if (entity.CategoryOfProduct.CategoryId != model.CategoryId)
            {
                var category = context
                    .Categories
                    .First(x => x.CategoryId == model.CategoryId);

                entity.CategoryOfProduct = category;
            }
            if (entity.BrandOfProduct.BrandId != model.BrandId)
            {
                var brand = context
                    .Brands
                    .First(x => x.BrandId == model.BrandId);

                entity.BrandOfProduct = brand;
            }
            context.SaveChanges();
        }

        public static Category? GetActiveCategory(this ShopContext context, long? categoryId)
        {
            Category? activeCategory = categoryId == null ?
                null :
                context.Categories
                    .Include(x => x.ProductsOfCategory)
                    .ThenInclude(p => p.BrandOfProduct)
                    .FirstOrDefault(c => c.CategoryId == categoryId);

            return activeCategory;
        }

        public static IQueryable<Brand> GetBrandsOfCategory(this ShopContext context, long? categoryId)
        {
            var brandQuery = context.Brands.AsQueryable();
            var activeCategory = GetActiveCategory(context, categoryId);

            if (activeCategory != null)
            {
                brandQuery = activeCategory
                    .ProductsOfCategory
                    .Select(p => p.BrandOfProduct)
                    .Distinct()
                    .AsQueryable();
            }

            return brandQuery;
        }

        public static IQueryable<Category> GetAllCategories(this ShopContext context)
        {
            return context
                .Categories
                .AsQueryable();
        }

        public static IQueryable<Brand> GetAllBrands(this ShopContext context)
        {
            return context
                .Brands
                .AsQueryable();
        }
    }
}
