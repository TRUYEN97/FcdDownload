namespace CPEI_MFG.Config
{
    public class TestConditionConfig
    {
        public bool IsEnable { get; set; }
        public bool IsCheckMacOldEnable { get; set; } = true;
        public bool IsCheckFailCountEnable { get; set; } = true;
        public double CheckUartTime { get; set; } = 10;
        public int MaxFailCount { get; set; } = 3;
        public int MaxRJ45Count { get; set; } = 4000;
        public int MaxProbeCount { get; set; } = 4000;
    }
}
