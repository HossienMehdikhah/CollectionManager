using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionManager.Core.Models
{
    public class DownloadURIDTO
    {
        public required string PartNumber { get; set; }
        public required Uri Uri { get; set; }
    }
}
