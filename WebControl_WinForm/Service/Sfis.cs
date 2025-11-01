using System;
using System.Threading;
using AppUtil.Communicate.Implement.Serial;
using AppUtil.Timer;
using System.Windows.Forms;
using CPEI_MFG.Model;
using CPEI_MFG.Config;

namespace CPEI_MFG.Service
{
    public class Sfis : IDisposable
    {
        private readonly object _lock;
        public ProgramConfig Config { get; private set; }
        public event Action<string, int> WriteDebugMessage;
        public readonly string SYANTE5 = $"{"SYANTE5",-20}END";
        private MySerialPort serialPort;

        public Sfis(ProgramConfig config)
        {
            Config = config;
            _lock = new object();
        }

        public bool Init()
        {
            serialPort?.Dispose();
            var sfisConfig = Config.SfisConfig;
            serialPort = new MySerialPort(sfisConfig.Com, sfisConfig.Baudrate);
            if (!serialPort.Connect())
            {
                MessageBox.Show($"Check SFIS({sfisConfig.Com}) failed!");
                return false;
            }
            SendSYANTE5();
            Thread.Sleep(1000);
            SendSYANTE5();
            return true;
        }

        private enum SfisResponceStatus
        {
            SUCCESS, FAIL, SKIP
        }

        public enum SfisResult
        {
            PASS, FAIL, TIME_OUT
        }
        public string ComName => serialPort?.Name;
        public SfisResult CheckMacSfis(TestModel testModel)
        {
            lock (_lock)
            {
                string mac = testModel?.ScanMAC;
                string snHH = testModel?.ScanHH;
                if (string.IsNullOrEmpty(mac) || mac.Length != 12 || string.IsNullOrEmpty(snHH) || snHH.Length != 9)
                {
                    return SfisResult.FAIL;
                }
                string command = $"{mac,-25}{snHH,-12}{Config.Station,-25}{Config.PcName,-25}END";
                return SendSfis(command, (s, line) =>
                {
                    line = line.Trim();
                    if (line.Contains("ERRO"))
                    {
                        return SfisResponceStatus.FAIL;
                    }
                    else
                    if (line.Length == 25 + 12 + 25 + 10 + 4 && line.Contains("PASS"))
                    {
                        string szModel = line.Substring(37, 25).Trim();
                        if (szModel != Config.Model)
                        {
                            string errorMsg = $"SFC Model :  {szModel} , Setting Model : {Config.Model}";
                            WriteDebugMessage?.Invoke(errorMsg, 2);
                            return SfisResponceStatus.FAIL;
                        }
                        string szMac = line.Substring(0, 25).Trim();
                        if (szMac != testModel.ScanMAC)
                        {
                            string errorMsg = $"SFC Mac : {szMac}, Scan Mac : {testModel.ScanMAC}";
                            testModel.SfisMAC = szMac;
                            WriteDebugMessage?.Invoke(errorMsg, 2);
                            return SfisResponceStatus.FAIL;
                        }
                        testModel.DutMO = line.Substring(25 + 12 + 25, 10).Trim().ToUpper();
                        return SfisResponceStatus.SUCCESS;
                    }
                    return SfisResponceStatus.SKIP;
                }, 5);
            }
        }

        private SfisResult SendSfis(string commnad, Func<MySerialPort, string, SfisResponceStatus> receivedAction, int timeOut = 1)
        {

            SfisResult rs = SfisResult.TIME_OUT;
            bool exit = false;
            serialPort.DataReceivedAction = (s, l) =>
            {
                if (receivedAction == null)
                {
                    return;
                }
                WriteDebugMessage?.Invoke($"SFC-->TE: {l}", 1);
                if (string.IsNullOrWhiteSpace(l) || l.Contains("SYANTE5"))
                {
                    return;
                }
                switch (receivedAction.Invoke(s, l))
                {
                    case SfisResponceStatus.SUCCESS:
                        rs = SfisResult.PASS;
                        exit = true;
                        break;
                    case SfisResponceStatus.FAIL:
                        rs = SfisResult.FAIL;
                        exit = true;
                        break;
                    case SfisResponceStatus.SKIP:
                        break;
                }
            };
            Stopwatch stopwatch = new Stopwatch((timeOut < 1 ? 1 : timeOut) * 1000);
            if (!SendSYANTE5())
            {
                return SfisResult.FAIL;
            }
            Thread.Sleep(1000);
            if (serialPort.WriteLine(commnad ?? string.Empty))
            {
                WriteDebugMessage?.Invoke($"TE-->SFC: {commnad}", 1);
                while (!exit && stopwatch.IsOntime && serialPort.IsConnect)
                {
                    Thread.Sleep(100);
                }
            }
            return rs;
        }

        public SfisResult SendTestResultToSFC(bool result, string mac, string errorCode)//PC-->SFIS For Test Finish
        {
            lock (_lock)
            {
                var verConfig = Config.VersionConfig;
                string bom = verConfig.BOM;
                string meBom = verConfig.Me_BOM;
                string tlvBom = verConfig.TlbBOM;
                string fw = verConfig.FW;
                string fcd = verConfig.FCD_Version;
                string region = verConfig.RegionConfig.SFC_region;
                string pcName = Config.PcName;
                pcName = pcName.Length > 12 ? pcName.Substring(0, 12) : pcName.PadRight(12, '-');
                errorCode = errorCode.Length > 6 ? errorCode.Substring(0, 6) : errorCode.PadRight(6, '-');
                if (string.IsNullOrWhiteSpace(errorCode))
                {
                    errorCode = "UNKNFF";
                }
                string commnad = string.IsNullOrWhiteSpace(meBom) || string.IsNullOrWhiteSpace(tlvBom) ?
                    $"{mac,-25}{mac,-12}{bom,-15}{fw,-25}{fcd,-15}{region,-26}{pcName,-12}" :
                    $"{mac,-25}{mac,-12}{bom,-15}{fw,-25}{fcd,-15}{region,-26}{meBom,-20}{tlvBom,-20}{pcName,-12}";
                if (!result)
                {
                    commnad = $"{commnad}{errorCode}";
                }
                return SendSfis(commnad, (s, line) =>
                {
                    line = line.Trim();
                    if (!line.StartsWith(mac))
                    {
                        return SfisResponceStatus.SKIP;
                    }
                    if (line.Contains("ERRO"))
                    {
                        return SfisResponceStatus.FAIL;
                    }
                    else
                    if (line.EndsWith("PASS"))
                    {
                        return SfisResponceStatus.SUCCESS;
                    }
                    return SfisResponceStatus.SKIP;
                }, 30);
            }
        }

        public bool SendSYANTE5()
        {
            if (serialPort == null)
            {
                return false;
            }
            if (!serialPort.WriteLine(SYANTE5))
            {
                return false;
            }
            return true;
        }

        public void Dispose()
        {
            serialPort?.Dispose();
        }
    }
}
