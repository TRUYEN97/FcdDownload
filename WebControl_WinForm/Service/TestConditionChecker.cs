using AppUtil.Service;
using CPEI_MFG.Config;
using CPEI_MFG.Model;
using System;
using System.Windows.Forms;

namespace CPEI_MFG.Service
{
    public class TestConditionChecker
    {
        private readonly TestConditionConfig config;
        private readonly TestModel model;
        private readonly UnitRegistry unitRegistry;
        public TestConditionChecker(TestModel model)
        {
            config = ConfigLoader.ProgramConfig.TestConditionConfig;
            this.model = model;
            unitRegistry = new UnitRegistry(model.Index);
            FailedCondition = CheckConditionFactory.GetCheckTestFailedConditionInstanceOf(model.Index);
            FailedCondition.EnableFailCheck = config.IsCheckFailCountEnable;
            FailedCondition.EnableOldMac = config.IsCheckMacOldEnable;
            FailedCondition.Spec = config.MaxFailCount;
            model.RJ45Num = unitRegistry.RJ45Count;
            model.ProbeNum = unitRegistry.ProbeCount;
            model.OnCountNumChanged += () =>
            {
                unitRegistry.RJ45Count = model.RJ45Num;
                unitRegistry.ProbeCount = model.ProbeNum;
            };
        }
        public CheckTestFailedCondition FailedCondition { get; }
        public bool CheckTestCondition()
        {
            if (model.RJ45Num >= config.MaxRJ45Count)
            {
                MessageBox.Show($"Number of times RJ45 is used exceeds the regulation ({config.MaxRJ45Count})," +
                    $"\r\nSố lần sử dụng RJ45 vượt quá quy định ({config.MaxRJ45Count})", $"Slot{model.Index}");
                return false;
            }
            if (model.ProbeNum >= config.MaxProbeCount)
            {
                MessageBox.Show($"Number of times Probe is used exceeds the regulation ({config.MaxProbeCount})," +
                    $"\r\nSố lần sử dụng cổng Probe vượt quá quy định ({config.MaxProbeCount})", $"Slot{model.Index}");
                return false;
            }
            return true;
        }
        public bool IsNeedToCheck()
        {
            if (!config.IsEnable) return false;
            return (DateTime.Now - unitRegistry.LastTimeCheck).TotalHours >= config.CheckUartTime || FailedCondition.IsFailedTimeOutOfSpec;
        }
        public void SetUartPassed()
        {
            unitRegistry.LastTimeCheck = DateTime.Now;
            FailedCondition.SetPass();
        }
        public void SetUartFailed()
        {
            unitRegistry.LastTimeCheck = DateTime.MinValue;
        }
    }
}
