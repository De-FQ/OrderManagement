

using Data.Base;
using Data.Model.SiteCategory;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utility.Enum;
using Utility.Helpers;

namespace Data.Model.InventoryManagement
{
    public class InventoryItem : BaseCommon
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [ForeignKey("SupplierId")]
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public string Unit { get; set; }

        [Column(TypeName = Utility.Helpers.Constants.DataSize.Decimal2)]
        public decimal UnitCost { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = Utility.Helpers.Constants.DataSize.Decimal2)]
        public decimal CostPrice { get; set; }

        // Navigation properties
       
        public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
