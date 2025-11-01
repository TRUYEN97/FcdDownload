using System;
using System.Collections.Generic;

namespace CPEI_MFG.Config
{
    public class ProgramConfig
    {

        public string Model { get; set; } = string.Empty;
        public string Station { get; set; } = string.Empty;
        internal string PcName { get; set; } = string.Empty;
        public string UiProduct { get; set; } = string.Empty;
        internal string UiProduct_name { get; set; } = string.Empty;
        public string FcdName { get; set; } = string.Empty;
        public string FcdUrl { get; set; } = string.Empty;
        internal string LogPath { get; set; } = string.Empty;
        public int InputLength { get; set; } = 19;
        public List<UnitConfig> UnitConfigs { get; set; } = new List<UnitConfig>()
        { new UnitConfig(), new UnitConfig(), new UnitConfig(), new UnitConfig () };
        public VersionConfig VersionConfig { get; set; } = new VersionConfig();
        public SfisConfig SfisConfig { get; set; } = new SfisConfig();
        public TestConditionConfig TestConditionConfig { get; set; } = new TestConditionConfig();
        public LoggerConfig LoggerConfig { get; set; } = new LoggerConfig();
        public PoeSwitchConfig PoeSwitchConfig { get; set; } = new PoeSwitchConfig();
    }
}
