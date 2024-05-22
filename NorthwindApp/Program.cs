using AutoMapper;
using BL.Events;
using BL.Events.IEvent;
using BL.IServices;
using BL.Models.Mapper;
using BL.Services;
using Core.Interfaces.IRepository;
using DAL;
using DAL.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NorthwindApp.Hubs;
using NorthwindApp.Middleware;
using System;
using System.Net;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using BL.Utils.Interfaces;
using BL.Utils;
using BL.Models.VM;
using System.Configuration;
using StackExchange.Redis.Configuration;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis;




var builder = WebApplication.CreateBuilder(args);
#pragma warning disable CS8604 // Possible null reference argument.
builder.Services.AddDbContext<AppDataBaseContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
#pragma warning restore CS8604 // Possible null reference argument.
/*builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConfig");
    options.InstanceName = "NorthwindApp_";
});

var redisConfig = builder.Configuration.GetSection("RedisConfig").Get<RedisConfiguration>();*/

#region DistrbutedCacheConnection
/*builder.Services.Configure<RedisConfiguration>(builder.Configuration.GetSection("RedisConfig"));
*/
/*builder.Services.AddStackExchangeRedisCache(options =>
{
    var redisConfig = builder.Configuration.GetSection("RedisConfig").Get<RedisConfiguration>();
    options.Configuration = redisConfig.ConfigurationOptions.ToString();
    options.InstanceName = redisConfig.KeyPrefix;
});*/
#endregion
/*builder.Services.Configure<RedisConfiguration>(builder.Configuration.GetSection("RedisConfigLocal"));*/
builder.Services.AddStackExchangeRedisCache(options =>
{
    var redisConfig = builder.Configuration.GetSection("RedisConfigLocal").Get<RedisConfiguration>();
    options.Configuration = redisConfig.ConfigurationOptions.ToString();
   /* options.Configuration = builder.Configuration.GetConnectionString("RedisConfigLocal");*/
});

builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
{
    var redisConfig = builder.Configuration.GetSection("RedisConfigLocal").Get<RedisConfiguration>();
    var configuration = ConfigurationOptions.Parse(redisConfig.ConfigurationOptions.ToString());
    return ConnectionMultiplexer.Connect(configuration);
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

HelperMiddleware.AddRepos(builder.Services);
var mappingConfig = new MapperConfiguration(

                mc =>
                {
                    mc.AddProfile(new EmployeesProfile());
                    mc.AddProfile(new ProductCategoriesProfile());
                    mc.AddProfile(new ProductCompanyProfile());
                    mc.AddProfile(new ProductProfile());
                    mc.AddProfile(new ShippingCompanyProfile());
                    mc.AddProfile(new EditProductProfile());
                }
            );

IMapper mapper = mappingConfig.CreateMapper();

builder.Services.AddSingleton(mapper);
builder.Services.AddTransient<IProductService, ProductService>();
HelperMiddleware.AddServices(builder.Services);
/*builder.Services.AddTransient<IDistributedCacheing<T>, DistributedCacheing<T>>();*/
builder.Services.AddTransient<IDistributedCacheing<ProductVM>, DistributedCacheing<ProductVM>>();



builder.Services.AddSignalR();
/*.AddHubOptions<NotificationHub>(options =>
{
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
    options.KeepAliveInterval = TimeSpan.FromSeconds(15)
}); ;
*/
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCustomExceptionHandler();
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePages(); // modify
    /*app.UseMvcWithDefaultRoute();*/
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    });

    app.UseHsts();

}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

        endpoints.MapControllerRoute(
            name: "deleteProduct",
            pattern: "Product/Delete/{id}",
            defaults: new { controller = "Product", action = "Delete" });


    endpoints.MapControllers();
    endpoints.MapHub<NotificationHub>("/notificationHub").RequireCors("AllowAll");
    endpoints.MapControllerRoute(
             name: "client",
             pattern: "{controller=Client}/{action=Index}/{id?}");
});


app.Run();
