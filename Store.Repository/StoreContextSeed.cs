using Microsoft.Extensions.Logging;
using Store.Data.Contexts;
using Store.Data.Entities;
using System.Text.Json;

namespace Store.Repository
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreDbContext context,ILoggerFactory loggerFactory)
        {
			try
			{
				if(context.ProductBrands != null && !context.ProductBrands.Any())
				{
					//Presist Data to Database
					var brandsData = File.ReadAllText("../Store.Repository/SeedData/brands.json");
					var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
					if (brandsData is not null)
						await context.ProductBrands.AddRangeAsync(brands);
				}
                if (context.ProductTypes != null && !context.ProductTypes.Any())
                {
                    //Presist Data to Database
                    var typesData = File.ReadAllText("../Store.Repository/SeedData/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    if (types is not null)
                        await context.ProductTypes.AddRangeAsync(types);
                }
                if (context.Products != null && !context.Products.Any())
                {
                    //Presist Data to Database
                    var productsData = File.ReadAllText("../Store.Repository/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    if (products is not null)
                        await context.Products.AddRangeAsync(products);
                }
                await context.SaveChangesAsync();
            }
			catch (Exception ex)
			{
                var logger = loggerFactory.CreateLogger<StoreDbContext>();
                logger.LogError(ex.Message);
			}
        }
    }
}
