using AutoMapper;
using Microsoft.Extensions.Logging;
using Store.Repository.Basket;
using Store.Repository.Basket.Models;
using Store.Service.Services.BasketServices.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.BasketServices
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
       // private readonly ILogger<CustomerBasketDto> _logger;
        public BasketService(IBasketRepository basketRepository,
                                IMapper mapper
                                //,ILogger<CustomerBasketDto> logger
            )
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
            //_logger = logger;
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            try
            {
                await _basketRepository.DeleteBasketAsync(basketId);
                return true;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex,ex.Message); 
                return false;
            }
        }

        public async Task<CustomerBasketDto> GetBasketAsync(string basketId)
        {
            try
            {
                var  basket = await _basketRepository.GetBasketAsync(basketId);
                if (basket == null)
                    return new CustomerBasketDto();

                var mappedBasket = _mapper.Map<CustomerBasketDto>(basket);
                return mappedBasket;
            }
            catch (Exception ex)
            {
               // _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<CustomerBasketDto> UpdateBasketAsync(CustomerBasketDto input)
        {
            try
            {
                if (input.Id is null) // there is no basket                   
                    input.Id = GenerateRandomBasketId();  // Generate Basket Id

                var customerBasket = _mapper.Map<CustomerBasket>(input);
                var updatedBasket = await _basketRepository.UpdateBasketAsync(customerBasket);
                var mappedUpdatedBasket = _mapper.Map<CustomerBasketDto>(updatedBasket);    
                return mappedUpdatedBasket; 

            }
            catch (Exception ex)
            {
               // _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        private string GenerateRandomBasketId()
        {
            Random random = new Random();
            int randomDigits = random.Next(1000, 10000); // from 1000 to 9999
            return $"BS-{randomDigits}";
        }
    }
}
