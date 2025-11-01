using System;

namespace CPEI_MFG.Model
{
    public class TestModel
    {
        private int rJ45Num;
        private int probeNum;

        public TestModel(int index)
        {
            Reset();
            Index = index;
        }

        public int Index { get; set; }
        public string Input { get; set; }
        public string Errorcode { get; set; }
        public bool TestResult { get; set; }
        public string ScanHH { get; set; }
        public string ScanMAC { get; set; }
        public string SfisMAC { get; set; }
        public string DutMO { get; set; }
        public bool IsDebugModel { get; set; }
        public int PassNum { get; set; }
        public int FailNum { get; set; }
        public int RJ45Num
        {
            get { return rJ45Num; }
            set
            {
                rJ45Num = value;
                OnCountNumChanged?.Invoke();
            }
        }
        public int ProbeNum
        {
            get { return probeNum; }
            set
            {
                probeNum = value;
                OnCountNumChanged?.Invoke();
            }
        }
        public string Message { get; set; }
        internal event Action OnCountNumChanged;
        internal void Reset()
        {
            Input = string.Empty;
            ScanHH = string.Empty;
            ScanMAC = string.Empty;
            SfisMAC = string.Empty;
            DutMO = string.Empty;
            TestResult = false;
            Errorcode = string.Empty;
            Message = string.Empty;
        }
    }
}
