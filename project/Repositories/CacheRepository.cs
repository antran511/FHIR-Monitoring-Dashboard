using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FHIR_FIT3077.IRepositories;
using FHIR_FIT3077.IRepository;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace FHIR_FIT3077.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDistributedCache _distributedCache;

        public CacheRepository(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public void SetObject<T>(string key, T value)
        {
            _distributedCache.SetString(key, JsonConvert.SerializeObject(value));
        }

        public T GetObject<T>(string key)
        {
            var value =  _distributedCache.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public bool ExistObject<T>(string key)
        {
            bool value;
            if (string.IsNullOrEmpty(_distributedCache.GetString(key)))
            {
                value = false;
            }
            else
            {
                value = true;
            } ;
            return value;
        }
    }
}
