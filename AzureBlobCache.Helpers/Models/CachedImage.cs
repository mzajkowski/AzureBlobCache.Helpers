using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Persistence.Migrations.Upgrades.TargetVersionSevenThreeZero;

namespace AzureBlobCache.Helpers.Models
{
    public class CachedImage
    {
        public string WebUrl { get; set; }
        public string CacheUrl { get; set; }
    }
}
