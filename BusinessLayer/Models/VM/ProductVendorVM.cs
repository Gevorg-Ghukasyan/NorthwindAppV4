using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models.VM
{
    public class ProductVendorVM
    {
        public int Id { get; set; }       
        public ProductVM Product { get; set; }       
        public ProductCompanyVM Company { get; set; }

       
    }
}
