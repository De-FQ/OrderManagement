using Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.InventoryManagement
{
    public class Recipe : BaseCommon
    {
        public string Name { get; set; }

        [ForeignKey("InventoryItemId")]
        public int InventoryItemId { get; set; }
        public InventoryItem InventoryItem { get; set; }

        //Navigation
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set;}

    }
}
