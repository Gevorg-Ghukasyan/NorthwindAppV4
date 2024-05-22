using Core.Abstraction;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IObservice
{
    public interface IProductObserver : IObserver<Product>
    {
        void ProductChanged(Product product);
    }
}
