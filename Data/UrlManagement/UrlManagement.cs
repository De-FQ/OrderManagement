using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.UrlManagement
{
    public class UrlManagement

    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string ShortUrl { get; set; } = string.Empty;
    }
}
