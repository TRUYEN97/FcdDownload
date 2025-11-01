using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPEI_MFG.Config
{
    public class VersionConfig
    {
        public string FW { get; set; } = string.Empty;
        public string FCD_Version { get; set; } = string.Empty;
        public string BOM { get; set; } = string.Empty;
        public string Me_BOM { get; set; } = string.Empty;
        public string TlbBOM { get; set; } = string.Empty;
        internal RegionConfig RegionConfig { get; set; } = new RegionConfig();
    }
}
