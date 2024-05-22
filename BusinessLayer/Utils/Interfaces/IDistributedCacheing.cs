using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Utils.Interfaces
{
    public interface IDistributedCacheing<T> where T : class
    {

        public Task<IEnumerable<T>?> GetAllAsyncCache();
        public Task SetAllAsyncCache(IEnumerable<T> allTableVMObjects);
        public Task AddToHashAsync(T product);
        public Task AddOrUpdateInHashAsync(T product);
        public Task UpdateProductAsync(T updatedProduct);
        public Task UpdateFieldInHashAsync(T product);
    }
}
