using Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.InventoryManagement
{
    public class RecipeIngredient : BaseCommon
    {
        [ForeignKey("RecipeId")]
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        [ForeignKey("IngredientId")]
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public int Quantity { get; set; }
    }
}
