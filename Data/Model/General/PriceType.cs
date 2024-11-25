using Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models.Base;

namespace Data.Model.General
{
    public class PriceType : BaseCommon
    {
        public string Name { get; set; }

        [ForeignKey("PriceTypeCategoryId")]
        public int PriceTypeCategoryId { get; set; }
        public virtual PriceTypeCategory PriceTypeCategory { get; set; }

       
    }
}
