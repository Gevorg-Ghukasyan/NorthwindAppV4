using Core.Interfaces.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly AppDataBaseContext _appDataBaseContext;
        private readonly IServiceProvider _provider;

        public RepositoryManager(AppDataBaseContext appDataBaseContext, IServiceProvider provider)
        {
            _appDataBaseContext = appDataBaseContext;
            _provider = provider;

        }

        public ICompanyTypesRepository CompanyTypes { get; set; }
        public IContactsRepository Contacts { get; set; }
        public IEmployeePrivilegesRepositroy EmployeePrivileges { get; set; }
        public IEmployeesRepository Employees { get; set; }
        public IMRURepository MRU { get; set; }
        public IOrderDetailsRepository OrderDetails { get; set; }
        public IOrderDetailStatusRepository OrderDetailStatus { get; set; }
        public IOrdersRepository Orders { get; set; }
        public IOrderStatusRepository OrdersStatus { get; set; }
        public IPrivilegesRepository Privileges { get; set; }
        public IProductCategoriesRepository ProductCategories {
            get
            { 
                return (IProductCategoriesRepository)_provider.GetService(typeof(IProductCategoriesRepository)); 
            }
            set
            {

            }
        }
        public IProductCompanyRepository ProductCompany { get; set; }

        public IProductRepository Products
        {
            get
            { 
                return (IProductRepository)_provider.GetService(typeof(IProductRepository)); 
            }
            set {
                
            }
        }
        public IProductVendorRepository ProductVendor { get; set; }
        public IPurchaseOrderDetailsRepository PurchaseOrderDetails { get; set; }
        public IPurchaseOrdersRepository PurchaseOrders { get; set; }
        public IPurchaseOrderStatusRepository PurchaseOrderStatus { get; set; }
        public IShippingCompanyRepository ShippingCompany { get; set; }
        public IStatesRepository States { get; set; }
        public IStockTakeRepository StockTake { get; set; }
        public ISupervisorEmployeeRepository SupervisorEmployee { get; set; }
        public ITaxStatusRepository TaxStatus { get; set; }
        public ITitlesRepositroy Titles { get; set; }


    }
}
