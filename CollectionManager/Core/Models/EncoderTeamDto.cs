using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionManager.Core.Models;

public class EncoderTeamDto
{
    public string EncoderName { get; set; } = string.Empty;
    public IDictionary<string, IEnumerable<Uri>> DownloadLinks { get; set; } = new Dictionary<string, IEnumerable<Uri>>();
    public string TotalValue { get; set; } = string.Empty;
}
