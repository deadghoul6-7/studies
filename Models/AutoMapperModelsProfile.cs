using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ProjectAspEShop2024.AppIdentity;
using ProjectAspEShop2024.Domains;
using ProjectAspEShop2024.Dto;
using ProjectAspEShop2024.Filters;

namespace ProjectAspEShop2024.Models
{
	public class AutoMapperModelsProfile
		:Profile
	{
		public AutoMapperModelsProfile()
		{
			CreateMap<FilterProduct, FilterProductViewModel>();
			CreateMap<FilterProductViewModel, FilterProduct>();

            CreateMap<Product, ProductEditViewModel>();
            CreateMap<ProductEditViewModel, Product>();

            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryViewModel, Category>();

            CreateMap<AppUser, UserSimpleViewModel>();

            CreateMap<AppUser, UserEditViewModel>();
            CreateMap<UserEditViewModel, AppUser>();

            CreateMap<IdentityRole, RoleViewModel>();
        }
	}
}
