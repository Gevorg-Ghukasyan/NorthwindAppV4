using BL.Models.VM;
using BL.Utils.Interfaces;
using Core.Abstraction;
using Core.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDatabase = StackExchange.Redis.IDatabase;

namespace BL.Utils
{
    public class DistributedCacheing<T> : IDistributedCacheing<T>  where T : class 
    {
        private readonly IDistributedCache _cache;
        private readonly IDatabase _redisDatabase;

        public DistributedCacheing(IDistributedCache cache , IConnectionMultiplexer redisConnection)
        {
            _cache = cache;
            _redisDatabase = redisConnection.GetDatabase();
        }
        public async Task<IEnumerable<T>?> GetAllAsyncCache()
        {
            var typeName = typeof(T).Name;
            var hashKey = "All" + typeName;
            var subkeys = await _redisDatabase.HashKeysAsync(hashKey);
            List<T> products = new List<T>();
            foreach (var subkey in subkeys)
            {
                var cachedProductValue = await _redisDatabase.HashGetAsync(hashKey, subkey);

                if (cachedProductValue.IsNull)
                {
                    return null;
                }
                else
                {
                    var cachedProduct = JsonConvert.DeserializeObject<T>(cachedProductValue.ToString());
                    products.Add(cachedProduct);
                }
               

            }
            if (products.Count > 0)
            {
                var result = products;
                return result;
            }
            return null;
        }

        public async Task SetAllAsyncCache(IEnumerable<T> allTableVMObjects)
        {
            var typeName = typeof(T).Name;
            var hashKey = "All" + typeName;

    

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                SlidingExpiration = TimeSpan.FromMinutes(1)
            };
            foreach (var item in allTableVMObjects)
            {
                var serializedProduct = JsonConvert.SerializeObject(item);
                var commondId = typeof(T).GetProperty("Id").GetValue(item);

                await _redisDatabase.HashSetAsync(hashKey, commondId.ToString(), serializedProduct, When.Always, CommandFlags.FireAndForget);
            }
        }

        /* public async Task AddToHashAsync(string hashKey, string subKey, byte[] value)
         {
             await _redisDatabase.HashSetAsync(hashKey, subKey, value, When.Always, CommandFlags.FireAndForget);
         }*/

        public async Task AddToHashAsync(T product)
        {
            var typeName = typeof(T).Name;
            var hashKey = "All" + typeName;
            var serializedProduct = JsonConvert.SerializeObject(product);
            var productBytes = Encoding.UTF8.GetBytes(serializedProduct);

            var Key = typeof(T).GetProperty("Id");
            var subKey = Key.GetValue(product)?.ToString();
            await _redisDatabase.HashSetAsync(hashKey, subKey.ToString(), productBytes, When.Always, CommandFlags.FireAndForget);
         
        }
        public async Task AddOrUpdateInHashAsync(T product)
        {
            var typeName = typeof(T).Name;
            var hashKey = "All" + typeName;
            var serializedProduct = JsonConvert.SerializeObject(product);
            var productBytes = Encoding.UTF8.GetBytes(serializedProduct);

            var keyProperty = typeof(T).GetProperty("Id");
            var subKey = keyProperty.GetValue(product)?.ToString();

 
            if (await _redisDatabase.HashExistsAsync(hashKey, subKey))
            {
                try
                {
                    await _redisDatabase.HashDeleteAsync(hashKey, subKey.ToString());
                    Console.WriteLine("Success");
                }
                catch(Exception ex)
                {
                    await Console.Out.WriteLineAsync($"Erorr : {ex.Message}");
                }
            }
            try
            {
                await _redisDatabase.HashSetAsync(hashKey, subKey, productBytes, When.Always, CommandFlags.FireAndForget);
                Console.WriteLine("Success added");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"Erorr : {ex.Message}");
            }
        }

        public async Task UpdateFieldInHashAsync(T product)
        {
            var typeName = typeof(T).Name;
            var hashKey = "All" + typeName;
            var keyProperty = typeof(T).GetProperty("Id");
            var subKey = keyProperty.GetValue(product)?.ToString();

            var existingProductBytes = await _redisDatabase.HashGetAsync(hashKey, subKey);

            if (existingProductBytes.HasValue)
            {
                var existingProduct = JsonConvert.DeserializeObject<T>(existingProductBytes);

                existingProduct = product;

                var updatedProductBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(existingProduct));

                await _redisDatabase.HashSetAsync(hashKey, subKey, updatedProductBytes, When.Always, CommandFlags.FireAndForget);
                Console.WriteLine("Success updated field");
            }
            else
            {
                Console.WriteLine("Product not found");
            }
        }


        public async Task UpdateProductAsync(T updatedProduct)
        {
            var hashKey = "AllProductVM";
            var serializedProduct = JsonConvert.SerializeObject(updatedProduct);
            var productBytes = Encoding.UTF8.GetBytes(serializedProduct);
            var keyProperty = typeof(T).GetProperty("Id");
            var subKey = keyProperty.GetValue(updatedProduct)?.ToString();

            await _redisDatabase.HashSetAsync(hashKey, subKey, productBytes, When.Always, CommandFlags.FireAndForget);
        }

        public async Task<T> GetFromHashAsync<T>(string hashKey, string subKey)
        {
            var result = await _redisDatabase.HashGetAsync(hashKey, subKey);

         /*   if (result.IsNull)
            {
                return default;
            }*/

            var jsonString = Encoding.UTF8.GetString(result);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }


        #region CommMethodsDefault
        /*        public async Task<IEnumerable<T>?> GetAllAsyncCache() 
                {
                    var type = typeof(T);
                    var typeName = type.Name;
                    var cacheKey = "All" + typeName;
                    var cachedProducts = await _cache.GetAsync(cacheKey);
                    if (cachedProducts != null)
                    {

                        return JsonConvert.DeserializeObject<List<T>>(Encoding.UTF8.GetString(cachedProducts));
                    }
                    else { return null; }
                }

                public async Task SetAllAsyncCache(IEnumerable<T> allTableVMObjects)
                {
                    var type = typeof(T);
                    var typeName = type.Name;
                    var cacheKey = "All" + typeName;
                    var serializedProducts = JsonConvert.SerializeObject(allTableVMObjects);
                    var cacheOptions = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                        SlidingExpiration = TimeSpan.FromMinutes(10)
                    };
                    await _cache.SetAsync(cacheKey, Encoding.UTF8.GetBytes(serializedProducts), cacheOptions);
                }*/
        #endregion

        #region CommMethods
        /*        public async Task<IEnumerable<T>?> GetAllAsyncCache()
                {
                    var type = typeof(T);
                    var typeName = type.Name;
                    var cacheKey = "All" + typeName;
                    var cachedProducts = await _cache.GetAsync(cacheKey);

                    if (cachedProducts != null)
                    {
                        return JsonConvert.DeserializeObject<List<T>>(Encoding.UTF8.GetString(cachedProducts));
                    }
                    else
                    {
                        return null;
                    }
                }
                public async Task SetAllAsyncCache(IEnumerable<T> allTableVMObjects)
                {
                    var type = typeof(T);
                    var typeName = type.Name;
                    var cacheKey = "All" + typeName;

                    var cacheOptions = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                        SlidingExpiration = TimeSpan.FromMinutes(10)
                    };
                    foreach (var item in allTableVMObjects)
                    {
                        var OneProduct = item.GetType().GetProperty("Id");
                        var OneProductKey = OneProduct.GetValue(item);
                        var combinedKey = $"{cacheKey}:{OneProductKey}";
                        var serializedProducts = JsonConvert.SerializeObject(item);
                        await _cache.SetAsync(combinedKey, Encoding.UTF8.GetBytes(serializedProducts), cacheOptions);
                    }


                }*/
        #endregion
        public async Task AddToCacheAsync(T newProduct)
        {
            var type = typeof(T);
            var typeName = type.Name;
            var cacheKey = "All" + typeName;
            List<T> currentProducts;
            currentProducts = (await GetAllAsyncCache()).ToList<T>() ?? new List<T>();
            currentProducts.Add(newProduct);
       
            var serializedProducts = JsonConvert.SerializeObject(currentProducts);
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                SlidingExpiration = TimeSpan.FromMinutes(10)
            };

            await _cache.SetAsync(cacheKey, Encoding.UTF8.GetBytes(serializedProducts), cacheOptions);
        }
    }
}
