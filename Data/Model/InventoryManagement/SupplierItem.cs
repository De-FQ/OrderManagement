using Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.InventoryManagement
{
    public class SupplierItem : BaseCommon
    {
        [ForeignKey("SupplierId")]
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public string Name {  get; set; }
    }
}
