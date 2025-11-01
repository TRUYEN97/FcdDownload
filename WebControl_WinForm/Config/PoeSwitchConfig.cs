using System;
using System.Collections.Generic;

namespace CPEI_MFG.Config
{
    public class PoeSwitchConfig
    {
        public bool IsPoeEnable { get; set; } = false;
        public string PoeIp { get; set; } = "192.168.1.20";
        public List<int> PoePorts { get; set; } = new List<int> { 12, 13, 14, 15 };
    }
}
