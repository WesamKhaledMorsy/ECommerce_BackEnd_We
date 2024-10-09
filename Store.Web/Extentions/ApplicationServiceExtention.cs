using Fast.Extensions.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data.Contexts;
using Store.Repository.Basket;
using Store.Repository.Interfaces;
using Store.Repository.Repositories;
using Store.Service.HandleResponses;
using Store.Service.Services.BasketServices;
using Store.Service.Services.CacheServices;
using Store.Service.Services.ProductBrandServices;
using Store.Service.Services.ProductServices;
using Store.Service.Services.ProductServices.DTOs;
using Store.Service.Services.TokenServices;
using Store.Service.Services.UserServices;

namespace Store.Web.Extentions
{
    public static class ApplicationServiceExtention
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductBrandService, ProductBrandService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<Service.Services.TokenServices.ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBasketRepository, BasketRepository>();    
            services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(typeof(BasketProfile));
            // add the configurations of ValidationErrorResponse class that we made it to help in our custom Middleware
            services.Configure<ApiBehaviorOptions>(options =>
            {
                // if use ModelState.IsValid >> fill the errors of that here
                options.InvalidModelStateResponseFactory= actionContext =>
                {
                    // to select the list that in list (as dictionary) 
                    var errors = actionContext.ModelState
                                .Where(model => model.Value?.Errors.Count > 0)
                                .SelectMany(model => model.Value?.Errors)
                                .Select(error => error.ErrorMessage)
                                .ToList();

                    var errorResponse = new ValidationErrorResponse
                    {
                        // fill the error list here
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });
            return services;
        }
    }
}
