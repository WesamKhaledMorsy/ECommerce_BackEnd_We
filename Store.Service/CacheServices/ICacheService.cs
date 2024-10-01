using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.CacheServices
{
    public interface ICacheService
    {
        // only two methods Set in Cache , get data from cache
        Task SetCacheResponseAsync(string key, object response, TimeSpan timetoLive); // key & value as it will be json object ,,, time to live in Cache
        Task<string> GetCacheResponseAsync(string key);
    }
}
