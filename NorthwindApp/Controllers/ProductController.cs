using BL.IServices;
using BL.Models.VM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Core.Entities;
using BL.Services;
using Core.Interfaces.IObservice;
using Microsoft.AspNetCore.SignalR;
using NorthwindApp.Hubs;
using Microsoft.Extensions.Caching.Distributed;


namespace NorthwindApp.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductCategoriesService _productCategoriesService;
        private readonly IHubContext<NotificationHub, IObserver<ProductVM>> _hubContext;
        public ProductController (
            IProductService productService , 
            IProductCategoriesService productCategoriesService , 
            IHubContext<NotificationHub, IObserver<ProductVM>> hubContext)
        {
            _productService = productService;
            _productCategoriesService = productCategoriesService;
            _hubContext = hubContext;
            _productService.Subscribe(OnProductAdded);
        }

        #region private Methods
        private void OnProductAdded(object? sender, ProductVM product)
        {
            _hubContext.Clients.Group("ProductSubscribers").SendWithMessageAsync(product);
        }
        #endregion

        public async Task<IActionResult> Index()
        {
            var productCategories = await _productCategoriesService.GetAllProductCategoriesAsync();
            var createProductVM = new CreateProductVM
            {
                Product = new ProductVM(),
                ProductCategories = productCategories
            };
            return View("Create", createProductVM);
        }


        [HttpGet("Details/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id)
        {
            var productdetail =  await _productService.GetProductDetail(id);
            return View(productdetail);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] ProductVM product)
        {
            if (ModelState.IsValid)
            {
                TempData["SuccessMessage"] = "Product added successfully!";
                _productService.AddProduct(product);
                _productService.NotifyProductAdded(product);
                /*_hubContext.Clients.Group("ProductSubscribers").SendWithMessageAsync(product);*/
                return View("Success");
            }
            return View(product);
        }
       
        // All Products
        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(int id)
        
        {
            /*var products = await _productService.GetAllAsync();*/
            
            
            var products = await _productService.GetLastProductsFromDatabaseAsync(10);
            
            if (id != 0)
            {
                var productUpdate = new EditProductVM { Id = id };
                return View("Update", productUpdate);
            }
            return View("Edit" , products);
        }
        [HttpPost("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm] EditProductVM product)
        {
            if (ModelState.IsValid)
            {
                TempData["SuccessMessage"] = "Product updated successfully!";
                await _productService.UpdateProductByIdAsync(product);
                
                return View("Success");
            }
            return View();
        }

        [HttpGet("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            _productService.DeleteProductById(id);
            TempData["SuccessMessage"] = "Product deleted successfully!";
            return View("Delete");
        }
    }
}
