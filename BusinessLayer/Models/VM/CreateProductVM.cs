using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models.VM
{
    public class CreateProductVM
    {
        public ProductVM Product { get; set; }
        public IEnumerable<ProductCategoriesVM> ProductCategories { get; set; }
    }

}
