using AppUtil.Service;
using System;

namespace CPEI_MFG.Service
{
    public class UnitRegistry
    {
        
        private readonly RegistryUtil uartRegistry;
        private readonly RegistryUtil testCountRegistry;
        public UnitRegistry(int index)
        {
            uartRegistry = new RegistryUtil($"Software\\FCD_Download\\UART\\USB{index}");
            testCountRegistry = new RegistryUtil($"Software\\FCD_Download\\Counter\\Unit{index}");
        }

        public DateTime LastTimeCheck
        {
            get
            {
                string timeString = uartRegistry.GetValue("LastTime", "");
                if (DateTime.TryParse(timeString, out var parsedTime))
                {
                    return parsedTime;
                }
                return default;
            }
            set
            {
                uartRegistry.SaveStringValue("LastTime", value.ToString("o"));
            }
        }

        public int RJ45Count
        {
            get
            {
                return testCountRegistry.GetValue("RJ45", 0);
            }
            set
            {
                testCountRegistry.SaveIntValue("RJ45", value);
            }
        }

        public int ProbeCount
        {
            get
            {
                return testCountRegistry.GetValue("PROBE", 0);
            }
            set
            {
                testCountRegistry.SaveIntValue("PROBE", value);
            }
        }
    }
}
