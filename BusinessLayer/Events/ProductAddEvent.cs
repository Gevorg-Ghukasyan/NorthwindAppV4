using BL.Events.IEvent;
using BL.Models.VM;
using Core.Entities;
using Rpd.EventQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Events
{
    public class ProductAddEvent : Event
    {
        public event EventHandler<ProductVM> ProductAdded;

        public virtual void OnProductAdded(ProductVM product)
        {
            ProductAdded?.Invoke(this, product);
        }

        public void Subscribe(EventHandler<ProductVM> handler)
        {
            ProductAdded += handler;
        }

        public void Unsubscribe(EventHandler<ProductVM> handler)
        {
            ProductAdded -= handler;
        }
    }
}
