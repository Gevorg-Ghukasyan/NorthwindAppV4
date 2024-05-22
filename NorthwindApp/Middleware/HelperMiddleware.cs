using BL.IServices;
using BL.Services;
using Core.Interfaces.IRepository;
using DAL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace NorthwindApp.Middleware
{
    public static class HelperMiddleware
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IProductCompanyService, ProductCompanyService>();
            services.AddScoped<IProductCategoriesService, ProductCategoriesService>();
            services.AddScoped<IShippingCompanyService, ShippingCompanyService>();

            /*builder.Services.AddScoped<ICompanyTypesService, CompanyTypesService>();
            builder.Services.AddScoped<IContactsService, ContactsService>();
            builder.Services.AddScoped<IEmployeePrivilegesService, EmployeePrivilegesService>();
            builder.Services.AddScoped<IEmployeesService, EmployeesService>();
            builder.Services.AddScoped<IMRUService, MRUService>();
            builder.Services.AddScoped<IOrderDetailsService, OrderDetailsService>();
            builder.Services.AddScoped<IOrderDetailStatusService, OrderDetailStatusService>();
            builder.Services.AddScoped<IOrdersService, OrdersService>();
            builder.Services.AddScoped<IOrderStatusService, OrderStatusService>();*/

            /*builder.Services.AddScoped<IProductVendorService, ProductVendorService>();
            builder.Services.AddScoped<IPurchaseOrderDetailsService, PurchaseOrderDetailsService>();
            builder.Services.AddScoped<IPrivilegesService, PrivilegesService>();
            builder.Services.AddScoped<IPurchaseOrdersService, PurchaseOrdersService>();
            builder.Services.AddScoped<IPurchaseOrderStatusService, PurchaseOrderStatusService>();*/

            /*builder.Services.AddScoped<IStatesService, StatesService>();
            builder.Services.AddScoped<IStockTakeService, StockTakeService>();
            builder.Services.AddScoped<ISupervisorEmployeeService, SupervisorEmployeeService>();
            builder.Services.AddScoped<ITaxStatusService, ITaxStatusService>();
            builder.Services.AddScoped<ITitlesService, TitlesService>();*/

        }

        public static void AddRepos(IServiceCollection services)
        {

             services.AddScoped<ICompanyTypesRepository, CompanyTypesRepository>();
             services.AddScoped<IContactsRepository, ContactsRepository>();
             services.AddScoped<IEmployeePrivilegesRepositroy, EmployeePrivilegesRepositroy>();
             services.AddScoped<IEmployeesRepository, EmployeesRepository>();
             services.AddScoped<IMRURepository, MRURepository>();
             services.AddScoped<IOrderDetailsRepository, OrderDetailsRepository>();
             services.AddScoped<IOrderDetailStatusRepository, OrderDetailStatusRepository>();
             services.AddScoped<IOrdersRepository, OrdersRepository>();
             services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();
             services.AddScoped<IProductRepository, ProductRepository>();
             services.AddScoped<IProductCompanyRepository, ProductCompanyRepository>();
             services.AddScoped<IProductVendorRepository, ProductVendorRepository>();
             services.AddScoped<IPurchaseOrderDetailsRepository, PurchaseOrderDetailsRepository>();
             services.AddScoped<IPrivilegesRepository, PrivilegesRepository>();
             services.AddScoped<IPurchaseOrdersRepository, PurchaseOrdersRepository>();
             services.AddScoped<IPurchaseOrderStatusRepository, PurchaseOrderStatusRepository>();
             services.AddScoped<IProductCategoriesRepository, ProductCategoriesRepository>();
             services.AddScoped<IRepositoryManager, RepositoryManager>();
             services.AddScoped<IShippingCompanyRepository, ShippingCompanyRepository>();
             services.AddScoped<IStatesRepository, StatesRepository>();
             services.AddScoped<IStockTakeRepository, StockTakeRepository>();
             services.AddScoped<ISupervisorEmployeeRepository, SupervisorEmployeeRepository>();
             services.AddScoped<ITaxStatusRepository, TaxStatusRepository>();
             services.AddScoped<ITitlesRepositroy, TitlesRepository>();
        }
    }
}
