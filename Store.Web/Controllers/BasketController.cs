using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Service.Services.BasketServices;
using Store.Service.Services.BasketServices.DTOs;

namespace Store.Web.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IBasketService _basketService;
        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerBasketDto>>GetBasketAsync(string id)
            =>Ok(await _basketService.GetBasketAsync(id));
        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> UpdateBasketAsync(CustomerBasketDto customerBasket)
              => Ok(await _basketService.UpdateBasketAsync(customerBasket));


        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBasketAsync(string id)
           => Ok(await _basketService.DeleteBasketAsync(id));
    }
}
