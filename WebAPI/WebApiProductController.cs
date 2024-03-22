using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAspEShop2024.BusinesLogic;
using ProjectAspEShop2024.DataAccessLayer;
using ProjectAspEShop2024.Dto;

namespace ProjectAspEShop2024.WebAPI
{
    [ApiController]
    [Route("api/")]
    public class WebApiProductController
        : ControllerBase
    {
        private readonly ShopContext _context;
        private readonly IMapper _mapper;

        public WebApiProductController(ShopContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        [HttpGet("GetProduct")]
        public ProductDetailsDto GetProduct()
        {
            var product = _context.Products
                    .Include(p => p.CategoryOfProduct)
                    .Include(p => p.BrandOfProduct)
                    .First();

            var productDto = _mapper
                .Map<ProductDetailsDto>(product);

            return productDto;
        }
    }
}
