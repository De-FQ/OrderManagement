
using Data.Base;
using Data.Model.SiteCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Enum;

namespace Data.Model.InventoryManagement
{
    public class Ingredient : BaseCommon
    {
        public string Name { get; set; }
        public string Unit { get; set; }

        [ForeignKey("InventoryItemId")]
        public int InventoryItemId { get; set; }
        public InventoryItem InventoryItem { get; set; }

        [ForeignKey("ChildCategoryId")]
        public int ChildCategoryId { get; set; }
        public ChildCategory ChildCategory { get; set; }

        //Navigation
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
    }
}

