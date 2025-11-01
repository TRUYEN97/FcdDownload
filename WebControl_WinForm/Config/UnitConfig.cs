using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPEI_MFG.Config
{
    public class UnitConfig
    {
        public bool IsEnable { get; set; } = true;
        public string BaseIP { get; set; } = "192.168.1.1";
        public string CheckUartKeyWork { get; set; } = string.Empty;
    }
}
