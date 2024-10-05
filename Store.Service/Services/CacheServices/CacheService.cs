using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Service.Services.CacheServices
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _database;
        public CacheService(IConnectionMultiplexer redis)
        {
            // In Memory Database not belongs to the datasource as it mainly used to reduce the load on the datasource
            _database = redis.GetDatabase();
        }
        public async Task<string> GetCacheResponseAsync(string key)
        {
            var cachedResponse = await _database.StringGetAsync(key);
            if (cachedResponse.IsNullOrEmpty)
                return null;
            return cachedResponse.ToString();
        }

        public async Task SetCacheResponseAsync(string key, object response, TimeSpan timetoLive)
        {
            if (response is null)
                return;

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            // Serialize => convert the object to json 
            var serliaizedResponse = JsonSerializer.Serialize(response, options);

            await _database.StringSetAsync(key, serliaizedResponse, timetoLive);
        }
    }
}
