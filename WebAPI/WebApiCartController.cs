using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Newtonsoft.Json;
using NuGet.Protocol;
using ProjectAspEShop2024.BusinesLogic;
using ProjectAspEShop2024.Helpers;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Serialization;

namespace ProjectAspEShop2024.WebAPI
{
    [ApiController]
    [Route("api/")]
    public class WebApiCartController
        : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        //private readonly ILogger _logger;

        public WebApiCartController(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }


        [HttpGet("GetCart")]
        public Cart GetFullCart()
        {
            var cart = httpContextAccessor
                .HttpContext
                .GetSubject<Cart>();

            return cart;
        }

        [HttpPost("PostCart")]
        public IResult PostFullCart(object obj)
        {
            try
            {
                string json = obj.ToString();

                Cart cart = JsonConvert
                    .DeserializeObject<Cart>(json);

                httpContextAccessor.HttpContext.SaveSubject<Cart>(cart);
            }
            catch (Exception ex)
            {
                //  _logger.LogError(ex, "PostFullCart");
                return Results.BadRequest();
            }

            return Results.Ok();
        }
    }
}
