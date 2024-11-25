using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Models.Base
{
    public class BaseRowOrder
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public long DisplayOrder { get; set; }
    }
}
