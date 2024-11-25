using Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Base
{
    public class BaseService
    {
        protected readonly ApplicationDbContext _dbcontext;
        protected string ErrorMessage = string.Empty;
        public BaseService(ApplicationDbContext context)
        {
            _dbcontext = context;
        }
    }
}
