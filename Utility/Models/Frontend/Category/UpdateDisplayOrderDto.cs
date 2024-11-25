using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Models.Frontend.Category
{
    public class UpdateDisplayOrderDto
    {
        public Guid Guid { get; set; }
        public int DisplayOrder { get; set; }
    }
}
