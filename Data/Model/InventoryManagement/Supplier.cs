using Data.Base;
using System.Collections.Generic;

namespace Data.Model.InventoryManagement
{
    public class Supplier : BaseCommon
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }

        // Navigation property
        public virtual ICollection<InventoryItem> InventoryItems { get; set; }
        public virtual ICollection<SupplierItem> SupplierItems { get; set; }
    }
}
