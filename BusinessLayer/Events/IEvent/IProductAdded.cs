using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Events.IEvent
{
    public interface IProductAdded
    {
        public event EventHandler<Product> ProductAdded;
        public void Subscribe(EventHandler<Product> handler);
        public void UnSubscribe(EventHandler<Product> handler);
    }
}
