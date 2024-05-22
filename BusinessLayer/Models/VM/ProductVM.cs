using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models.VM
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal StandartUnitCost { get; set; }
        public decimal UnitPrice { get; set; }
        public short? ReorderLevel { get; set; }
        public short? TargetLevel { get; set; }
        public short QuantityPerUnit { get; set; }
        public bool Discontinued { get; set; }
        public short MinimumReorderQuantitiy { get; set; }
        public int ProductCategoriesId { get; set; }
        public string AddedBy {  get; set; }
        public DateTime AddedOn { get; set; }

        public ProductVM()
        {
            AddedBy = "CurrentUser";
            AddedOn = DateTime.UtcNow;
            Discontinued = true;

        }

    }
    
}
