using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectAspEShop2024.AppIdentity;
using ProjectAspEShop2024.Domains;
using ProjectAspEShop2024.Models;
using System.Reflection.Metadata;

namespace ProjectAspEShop2024.DataAccessLayer
{
    public class ShopContext : IdentityDbContext<AppUser, IdentityRole, string>
    {
		public virtual DbSet<FileDocument> FileDocuments { get; set; }

		public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Brand> Brands { get; set; }

        public ShopContext(DbContextOptions options) : base(options)
        { }
    }
}
