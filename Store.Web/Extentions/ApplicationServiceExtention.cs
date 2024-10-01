using Microsoft.AspNetCore.Mvc;
using Store.Repository.Interfaces;
using Store.Repository.Repositories;
using Store.Service.HandleResponses;
using Store.Service.Services.ProductBrandServices;
using Store.Service.Services.ProductServices;
using Store.Service.Services.ProductServices.DTOs;

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
            services.AddAutoMapper(typeof(ProductProfile));
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
