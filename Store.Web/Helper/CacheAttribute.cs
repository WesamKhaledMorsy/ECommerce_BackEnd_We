using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Service.Services.CacheServices;
using System.Text;

namespace Store.Web.Helper
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeTOLiveInSeconds;

        public CacheAttribute(int timeTOLiveInSeconds)
        {
            _timeTOLiveInSeconds=timeTOLiveInSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // we can not inject the cacheService 
             var _cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var cachedResponse = await _cacheService.GetCacheResponseAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var contentResult =new ContentResult 
                { 
                    Content = cachedResponse ,
                    ContentType= "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
            
            //await next(); // but I want to store the response
            var executedContext = await next();
            if (executedContext.Result is OkObjectResult response) 
            {
                await _cacheService.SetCacheResponseAsync(cacheKey, response.Value, TimeSpan.FromSeconds(_timeTOLiveInSeconds));
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest httpRequest)
        {
            StringBuilder cacheKey = new StringBuilder();
            cacheKey.Append($"{httpRequest.Path}");

            foreach (var (key,value) in httpRequest.Query.OrderBy(x=>x.Key))
            {
                cacheKey.Append($"|{key}-{value}");
            }
            return cacheKey.ToString(); 
        }
    }
}
