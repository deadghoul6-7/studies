using AutoMapper;
using ProjectAspEShop2024.Domains;

namespace ProjectAspEShop2024.Dto
{
    public class AutoMapperDomainsProfile : Profile
    {
        public AutoMapperDomainsProfile()
        {
            CreateMap<Product, ProductDto>();

            var cfg = CreateMap<Product, ProductDetailsDto>()
                .ForMember(
                // название бренда берём по ссылке из Product в табл Brand
                dto => dto.BrandName,
                opt => opt.MapFrom(p => p.BrandOfProduct.Name))
               .ForMember(
                // название категории берём по ссылке из Product в табл Category
                dto => dto.CategoryName,
                opt => opt.MapFrom(p => p.CategoryOfProduct.Name));
 
        }
    }
}
