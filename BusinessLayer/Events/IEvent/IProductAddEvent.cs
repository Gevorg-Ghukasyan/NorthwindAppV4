using BL.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Events.IEvent
{
    public interface IProductAddedEvent
    {
        event EventHandler<ProductVM> ProductAdded;

        void NotifyProductAdded(ProductVM product);
    }
}
