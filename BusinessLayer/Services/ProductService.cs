using AutoMapper;
using BL.IServices;
using BL.Models.VM;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.IRepository;
using DAL.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using Core.Interfaces;
using BL.Events;
using BL.Events.IEvent;
using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.Extensions.Caching.Distributed;
using BL.Utils.Interfaces;
using BL.Utils;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
namespace BL.Services
{
    public class ProductService : IProductService, IProductAddedEvent
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly List<EventHandler<ProductVM>> _subscribers = new List<EventHandler<ProductVM>>();
        private readonly IDistributedCacheing<ProductVM> _distributedCacheing;
        public event EventHandler<ProductVM>? ProductAdded;

        public ProductService(IRepositoryManager repositoryManager , IMapper mapper , IDistributedCacheing<ProductVM> distributedCache) 
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _distributedCacheing = distributedCache;
        }

        #region ProductService IO Methods
        public ProductVM GetProduct(int id)
        {
            var result = _repositoryManager.Products.Get(id);

            if (result == null)
            {
                var errorMessage = $"Product with ID {id} not found.";
                Console.WriteLine(errorMessage);

                throw new NotFoundException(errorMessage);
            }


            return _mapper.Map<ProductVM>(result);
        }

        public async Task<ProductDetailsVM> GetProductDetail(int id)
        {
            var product = await _repositoryManager.Products
                .GetAsync(id);

            var productCategory = await _repositoryManager.ProductCategories
                .GetAsync(product.ProductCategoryId);
            if (product == null)
            {
                throw new NotFoundException();
            }
            if (product.ProductDescription == null)
            {
                product.ProductDescription = string.Empty;
            }
            return new ProductDetailsVM
            {
                Id = product.Id,
                ProductCode = product.ProductCode,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                UnitPrice = product.UnitPrice,
                Discontinued = product.Discontinued,
                ProductCategoryImage = productCategory.ProductCategoryImage,
                ProductCategoryName = productCategory.ProductCategoryName,
                ProductCategoriesId = productCategory.Id


            };

        }



        public async Task<ProductVM> GetProductAsync(int id)
        {
            var result = await _repositoryManager.Products.GetAsync(id);
            if (result == null)
            {
                var errorMessage = $"Product with ID {id} not found.";
                Console.WriteLine(errorMessage);

                throw new NotFoundException(errorMessage);
            }
            
            return _mapper.Map<ProductVM>(result);
        }

        public IEnumerable<ProductVM> GetAll()
        {
            var result = _repositoryManager.Products.GetAll();
            if (result == null)
            {
                var errorMessage = $"Products not found.";
                Console.WriteLine(errorMessage);

                throw new NotFoundException(errorMessage);
            }
            return _mapper.Map<IEnumerable<ProductVM>>(result);
        }

        public async Task<IEnumerable<ProductVM>> GetAllAsync()
        {
            var resultCache = await _distributedCacheing.GetAllAsyncCache();
            if (resultCache == null)
            {
                 var result = await _repositoryManager.Products.GetAllAsync();
                if (result == null)
                {
                    var errorMessage = $"Products not found.";
                    Console.WriteLine(errorMessage);

                    throw new NotFoundException(errorMessage);
                }
                var allProductsFromDatabase = _mapper.Map<IEnumerable<ProductVM>>(result);
                await _distributedCacheing.SetAllAsyncCache(allProductsFromDatabase);
                return allProductsFromDatabase;
            }
            
           return resultCache;
        }

        public async Task<IEnumerable<ProductVM>?> GetLastProductsFromDatabaseAsync(int count)
        {
            IEnumerable<ProductVM>? resultCache = await _distributedCacheing.GetAllAsyncCache();
            if (resultCache == null)
            {
                var result = await _repositoryManager.Products.GetLastProductsFromDatabaseAsync(count);
                if (result == null)
                {
                    var errorMessage = $"Products not found.";
                    Console.WriteLine(errorMessage);

                    throw new NotFoundException(errorMessage);

                }

                var CountProductsFromDatabase = _mapper.Map<IEnumerable<ProductVM>>(result);
                await _distributedCacheing.SetAllAsyncCache(CountProductsFromDatabase);
                return CountProductsFromDatabase;
            }
           
            return resultCache;
        }

        public int GetTotalStockQuantityOnHand()
        {
            var result = _repositoryManager.Products.GetTotalStockQuantityOnHand();
            return result;
        }

        public async Task<int> GetTotalStockQuantityOnHandAsync()
        {
            var result = await _repositoryManager.Products.GetTotalStockQuantityOnHandAsync();
            return result;
        }

        public void AddProduct(ProductVM product)
        {
            var productDTO = _mapper.Map<ProductVM , Product>(product);
            _repositoryManager.Products.Add(productDTO);
        }

        public async Task AddProductAsync(ProductVM product)
        {
            var productDTO = _mapper.Map<Product>(product);
            await _repositoryManager.Products.AddAsync(productDTO);
        }

        public void AddRangeProduct(IEnumerable<ProductVM> products)
        {
            var productDTOs = _mapper.Map<IEnumerable<Product>>(products);
            _repositoryManager.Products.AddRange(productDTOs);
        }

        public async Task AddRangeProductAsync(IEnumerable<ProductVM> products)
        {
            var productDTOs = _mapper.Map<Product>(products);
            await _repositoryManager.Products.AddAsync(productDTOs);
        }

        public void DeleteProductById(int productId)
        {
            var product = _repositoryManager.Products.Get(productId);
            if (product != null)
            {
                _repositoryManager.Products.DeleteById(productId);
            }
            else
            {
                throw new NotFoundException();
            }
        }

        public async Task DeleteProductByIdAsync(int productId)
        {
            var product = await _repositoryManager.Products.GetAsync(productId);
            if (product != null) 
            { 
                await _repositoryManager.Products.DeleteByIdAsync(productId);
            }
            else
            {
                throw new NotFoundException();
            }
        }

        public ProductVM FindDefaultProduct(params object?[]? keyValues)
        {
            var result = _repositoryManager.Products.FindDefault(keyValues);
            return _mapper.Map<ProductVM>(result);
        }

        public async Task<ProductVM> FindDefaultProductAsync(params object?[]? keyValues)
        {
            var result = await _repositoryManager.Products.FindDefaultAsync(keyValues);
            return _mapper.Map<ProductVM>(result);
        }

        public IEnumerable<ProductVM> FindProduct(Expression<Func<ProductVM, bool>> predicate)
        {

            var result =_repositoryManager.Products.Find(_mapper.Map<Expression<Func<Product, bool>>>(predicate));
            return _mapper.Map<IEnumerable<ProductVM>>(result);
        }

        public async Task<IEnumerable<ProductVM>> FindProductAsync(Expression<Func<ProductVM, bool>> predicate)
        {
            var result = await _repositoryManager.Products.FindAsync(_mapper.Map<Expression<Func<Product, bool>>>(predicate));
            return _mapper.Map<IEnumerable<ProductVM>>(result);
        }

        public async Task<IEnumerable<ProductVM>> FindProductsByVendorAsync(int vendorId)
        {
            var result = await _repositoryManager.Products.FindProductsByVendorAsync(vendorId);
            return _mapper.Map<IEnumerable<ProductVM>>(result);

        }

        public async Task<IEnumerable<ProductVM>> FindProductsInCompletedOrdersAsync()
        {
            var result = await _repositoryManager.Products.FindProductsInCompletedOrdersAsync();
            return _mapper.Map<IEnumerable<ProductVM>>(result);
        }

        public async Task<IEnumerable<ProductVM>> FindProductsInOrdersAsync()
        {
            var result = await _repositoryManager.Products.FindProductsInOrdersAsync();
            return _mapper.Map<IEnumerable<ProductVM>>(result);
        }

        public ProductVM FirstOrDefaultProduct()
        {
            var result = _repositoryManager.Products.FirstOrDefault();
            return _mapper.Map<ProductVM>(result);
        }



        public void RemoveProduct(ProductVM product)
        {
            var productDTO = _mapper.Map<Product>(product);
            _repositoryManager.Products.Remove(productDTO);
        }

        public async Task RemoveProductAsync(ProductVM product)
        {
            var productDTO =  _mapper.Map<ProductVM ,Product>(product);
            await _repositoryManager.Products.RemoveAsync(productDTO);
        }

        public void RemoveRangeProduct(IEnumerable<ProductVM> products)
        {
            var productDTOs = _mapper.Map<IEnumerable<Product>>(products);
            _repositoryManager.Products.RemoveRange(productDTOs);
        }

        public async Task RemoveRangeProductAsync(IEnumerable<ProductVM> products)
        {
            var productDTOs = _mapper.Map<IEnumerable<Product>>(products);
            await _repositoryManager.Products.RemoveRangeAsync(productDTOs);
        }

        public ProductVM SingleOrDefaultProduct(Expression<Func<ProductVM, bool>> predicate)
        {
            var result = _repositoryManager.Products.SingleOrDefault(_mapper.Map<Expression<Func<Product, bool>>>(predicate));
            return _mapper.Map<ProductVM>(result);
        }

        public void UpdateProduct(ProductVM product)
        {
            _repositoryManager.Products.Update(_mapper.Map<ProductVM, Product>(product));
        }

        public void UpdateProductById(EditProductVM product) // modify
        {
           var productCurrent = _repositoryManager.Products.Get(product.Id);
            _distributedCacheing.AddToHashAsync(_mapper.Map<Product, ProductVM>(productCurrent)); // add to Cache
            var productDTO = _mapper.Map<EditProductVM, Product>(product , productCurrent);
            _repositoryManager.Products.Update(productDTO);
        }

        public async Task UpdateProductByIdAsync(EditProductVM product)
        {
            var productCurrent = await _repositoryManager.Products.GetAsync(product.Id);
            /*await _distributedCacheing.AddToHashAsync(_mapper.Map<Product, ProductVM>(productCurrent));*/ // add to Cache
            await _distributedCacheing.AddOrUpdateInHashAsync(_mapper.Map<Product, ProductVM>(productCurrent));
            /*await _distributedCacheing.UpdateProductAsync(_mapper.Map<Product, ProductVM>(productCurrent));*/
            /*await _distributedCacheing.UpdateFieldInHashAsync(_mapper.Map<Product, ProductVM>(productCurrent));*/
            var productDTO = _mapper.Map<EditProductVM, Product>(product, productCurrent);
            await _repositoryManager.Products.UpdateAsync(productDTO);
        }

        public async Task UpdateProductAsync(ProductVM product)
        {
           await _repositoryManager.Products.UpdateAsync(_mapper.Map<ProductVM , Product>(product));
        }

        public void UpdateProductRange(IEnumerable<ProductVM> products)
        {
            _repositoryManager.Products.UpdateRange(_mapper.Map<IEnumerable<Product>>(products));
        }

        public async Task UpdateProductRangeAsync(IEnumerable<ProductVM> products)
        {
           await _repositoryManager.Products.UpdateRangeAsync(_mapper.Map<IEnumerable<Product>>(products));
        }
        #endregion

        #region Observer
        public void OnProductAdded(ProductVM product)
        {

            ProductAdded?.Invoke(this, product);
        }

        public void NotifyProductAdded(ProductVM product)
        {
            OnProductAdded(product);
        }

   

        public void Subscribe(EventHandler<ProductVM> handler)
        {
            _subscribers.Add(handler);
            ProductAdded += handler;
        }

        public void Unsubscribe(EventHandler<ProductVM> handler)
        {
            _subscribers.Remove(handler);
            ProductAdded -= handler;
        }


        #endregion
    }
}
