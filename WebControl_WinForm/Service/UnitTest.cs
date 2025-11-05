using AppUtil.Common;
using AppUtil.Communicate;
using AppUtil.Service;
using CPEI_MFG.Config;
using CPEI_MFG.Model;
using CPEI_MFG.Service;
using CPEI_MFG.Services;
using CPEI_MFG.View;
using OpenQA.Selenium;
using System;
using System.IO;
using System.Windows.Forms;

namespace CPEI_MFG
{
    public class UnitTest
    {
        private readonly TestModel model;
        private readonly ProgramConfig appConfig;
        private readonly Sfis sfis;
        private readonly RaspberryPiServeice pi4;
        private readonly MyTimer checkResultTimer;
        private readonly TestConditionChecker testConditionChecker;
        public UnitTest(int index, ProgramConfig appConfig, Sfis sfis, UnitConfig unit)
        {
            model = new TestModel(index);
            testConditionChecker = new TestConditionChecker(model);
            UnitView = new UnitView(model);
            UnitView.OnInputEnter += UnitView_OnInputEnter;
            UnitView.OnVerifyClick += () =>
            {
                UnitView.LockAll(true);
                CheckUart();
                UnitView.LockAll(false);
            };
            this.appConfig = appConfig;
            this.sfis = sfis;
            UnitConfig = unit;
            UnitView.IsEnable = unit.IsEnable;
            UnitView.IsVisible = unit.IsVisible;
            pi4 = new RaspberryPiServeice("192.168.1.19", "ubnt", "ubnt");
            FailedCondition = testConditionChecker.FailedCondition;
            checkResultTimer = new MyTimer(_ =>
            {
                if (checkResultTimer.IsRunning)
                {
                    WatchingTest();
                }
            });
        }

        public bool IsTesting => checkResultTimer.IsRunning;
        public UnitView UnitView { get; }
        public UnitConfig UnitConfig { get; }
        public IWebDriver Driver { get; set; }
        public CheckTestFailedCondition FailedCondition { get; }
        public event Action<Control> FocusNextControl { add { UnitView.FocusNextControl += value; } remove { UnitView.FocusNextControl -= value; } }
        private bool CheckUart()
        {
            try
            {
                MessageBox.Show("Pls plug the network cable\r\nXin hãy cắm cổng mạng vào sản phẩm.", $"SLOT {model.Index}");
                UnitView.ShowMessage($"Wait ping {UnitConfig.BaseIP}");
                if (Util.Ping(UnitConfig.BaseIP, 5000))
                {
                    UnitView.ShowMessage("Checking UART...");
                    if (pi4.ExecuteSSHCommands(model.Index, UnitConfig.CheckUartKeyWork))
                    {
                        UnitView.ShowMessage("UART checked ok");
                        MessageBox.Show("Pls Unplug the network cable and retest\r\nKiểm tra thành công, xin hãy rút cổng mạng ra khỏi sản phẩm.", $"SLOT {model.Index}");
                        testConditionChecker.SetUartPassed();
                        return true;
                    }
                    else
                    {
                        UnitView.ShowMessage("Check UART fail, pls call TE check Usb serial");
                        MessageBox.Show("Xin hãy gọi TE online để kiểm tra lại cổng Serial.", $"SLOT {model.Index}");
                    }
                }
                else
                {
                    UnitView.ShowMessage($"Ping DUT({UnitConfig.BaseIP}) fail, pls retry");
                }
                testConditionChecker.SetUartFailed();
                return false;
            }
            catch (Exception ex)
            {
                UnitView.ShowErrorMess(ex.ToString());
                MessageBox.Show($"CheckUart:\r\n{ex}", $"Slot{model.Index}");
                return false;
            }
        }

        private static bool CheckCapslock()
        {
            if (Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock))
            {
                MessageBox.Show("Please turn off caps lock!\r\nXin hãy tắt capslock!");
                return false;
            }
            return true;
        }

        private void UnitView_OnInputEnter(string input)
        {
            try
            {
                UnitView.LockAll(true);
                if (Driver == null)
                {
                    UnitView.ShowErrorMess("Web driver == null!");
                    return;
                }
                if (IsTesting || input.Length != appConfig.InputLength)
                {
                    UnitView.ShowErrorMess($"This SN's length != {appConfig.InputLength}");
                    return;
                }
                if (!CheckCapslock())
                {
                    return;
                }
                if (!testConditionChecker.CheckTestCondition())
                {
                    return;
                }
                if (testConditionChecker.IsNeedToCheck() && !CheckUart())
                {
                    return;
                }
                if (CheckInput(input))
                {
                    UnitView.StartTest();
                    checkResultTimer.Start(3000, 1000);
                }
            }
            catch (Exception ex)
            {
                UnitView.ShowErrorMess(ex.ToString());
                MessageBox.Show($"UnitView_OnInputEnter:\r\n{ex}", $"Slot{model.Index}");
            }
            finally
            {
                if (!IsTesting)
                {
                    UnitView.LockAll(false);
                }
            }
        }

        private bool CheckInput(string input)
        {
            try
            {
                model.Reset();
                model.ScanMAC = input.Substring(0, 12);
                if (FailedCondition.IsOldMac(model.ScanMAC))
                {
                    UnitView.ShowErrorMess("Bản này đã test fail 1 lần, vui lòng đổi bản khác!");
                    return false;
                }
                model.ScanHH = Microsoft.VisualBasic.Interaction.InputBox("Vui lòng nhập mã HH (9 kí tự)", "Input", "", 100, 100);
                if (model.ScanHH.Length != 9)
                {
                    MessageBox.Show("Vui lòng kiểm tra lại mã HH", "Warning");
                    return false;
                }
                switch (sfis.CheckMacSfis(model))
                {
                    case Sfis.SfisResult.PASS:
                        FailedCondition.OldMac = model.ScanMAC;
                        string ip = $"192.168.1.3{model.Index + 1}";
                        UnitView.ShowMessage($"Send arp -d {ip}");
                        pi4.DeleteArpIp(ip); 
                        UnitView.ShowMessage("Delete old log");
                        pi4.DeleteOldTestLog(model.ScanMAC, appConfig.LogPath);
                        UnitView.ShowMessage($"Send input({input}) to FCD");
                        string htmlQR = $"macqr{model.Index}";
                        Driver.FindElement(By.Id(htmlQR)).SendKeys(input);
                        Driver.FindElement(By.Id(htmlQR)).SendKeys(OpenQA.Selenium.Keys.Enter);
                        return true;
                    case Sfis.SfisResult.FAIL:
                        UnitView.ShowErrorMess($"This board not {appConfig.Station} station!");
                        break;
                    case Sfis.SfisResult.TIME_OUT:
                        UnitView.ShowErrorMess("Terminal timeout!");
                        break;
                }
                return false;
            }
            catch (Exception ex)
            {
                UnitView.ShowErrorMess(ex.ToString());
                MessageBox.Show($"CheckInput:\r\n{ex}", $"Slot{model.Index}");
                return false;
            }
        }
        private void WatchingTest()
        {
            string res1 = GetWebElement(By.Id($"pgstext{model.Index}"))?.Text;
            if (res1 == null || (!res1.Contains("Successful") && !res1.Contains("Failed")))
            {
                return;
            }
            try
            {
                checkResultTimer.Stop();
                string mac = model.ScanMAC;
                string localDir = Path.Combine("D:\\UBNT_Test_Logs", appConfig.Model, appConfig.PcName, DateTime.Now.ToString("yyyy-MM-dd"));
                string log = pi4.SearchAndDownLoadTestLog(mac, appConfig.LogPath, localDir);
                if (log == null || !File.Exists(log))
                {
                    UnitView.ShowErrorMess("Log test not found!");
                    return;
                }
                Tuple<bool, string> rs = LogAnalyse(log);
                model.TestResult = rs.Item1;
                model.Errorcode = rs.Item2;
                if (model.TestResult)
                {
                    FailedCondition.SetPass();
                }
                else
                {
                    FailedCondition.SetFailed(model.Errorcode);
                }
                SaveLogToServer(log, model.TestResult, mac, model.Errorcode);
                pi4.DeleteOldTestLog(mac, appConfig.LogPath);
                switch (sfis.SendTestResultToSFC(model.TestResult, mac, model.Errorcode))
                {
                    case Sfis.SfisResult.FAIL:
                        {
                            if (model.TestResult)
                            {
                                model.Message = "Send result to SFIS failed!";
                            }
                            break;
                        }
                    case Sfis.SfisResult.TIME_OUT:
                        model.Message = $"Send result to SFIS failed, Terminal time out!";
                        break;
                }
            }
            catch (Exception ex)
            {
                UnitView.ShowErrorMess(ex.ToString());
                MessageBox.Show($"WatchingTest:\r\n{ex}", $"Slot{model.Index}");
            }
            finally
            {
                checkResultTimer.Stop();
                UnitView.StoptTest();
            }
        }
        private IWebElement GetWebElement(By by)
        {
            try
            {
                return Driver?.FindElement(by);
            }
            catch
            {
                return null;
            }
        }

        private Tuple<bool, string> LogAnalyse(string logPath)
        {
            if (!File.Exists(logPath))
            {
                return new Tuple<bool, string>(false, "NOLOG");
            }
            string fileText = File.ReadAllText(logPath);
            if (fileText.IndexOf("=== 100 ===") < 0)
            {
                string errorCode = "DL00FF";
                if (fileText.Contains("=== 1 ==="))
                {
                    errorCode = "DL01FF";
                }
                if (fileText.Contains("=== 5 ==="))
                {
                    errorCode = "DL05FF";
                }
                if (fileText.Contains("=== 10 ==="))
                {
                    errorCode = "DL10FF";
                }
                if (fileText.Contains("=== 13 ==="))
                {
                    errorCode = "DL13FF";
                }
                if (fileText.Contains("=== 15 ==="))
                {
                    errorCode = "DL15FF";
                }
                if (fileText.Contains("=== 20 ==="))
                {
                    errorCode = "DL20FF";
                }
                if (fileText.Contains("=== 30 ==="))
                {
                    errorCode = "DL30FF";
                }
                if (fileText.Contains("=== 40 ==="))
                {
                    errorCode = "DL40FF";
                }
                if (fileText.Contains("=== 50 ==="))
                {
                    errorCode = "DL50FF";
                }
                if (fileText.Contains("=== 63 ==="))
                {
                    errorCode = "DL63FF";
                }
                if (fileText.Contains("=== 65 ==="))
                {
                    errorCode = "DL65FF";
                }
                if (fileText.Contains("=== 70 ==="))
                {
                    errorCode = "DL70FF";
                }
                if (fileText.Contains("=== 75 ==="))
                {
                    errorCode = "DL75FF";
                }
                if (fileText.Contains("=== 80 ==="))
                {
                    errorCode = "DL80FF";
                }
                if (fileText.Contains("=== 90 ==="))
                {
                    errorCode = "DL90FF";
                }
                return new Tuple<bool, string>(false, errorCode);
            }
            else
            {
                var verConfig = appConfig.VersionConfig;
                string toplevebom = Util.GetStringBetween(fileText, "toplevelbom='", "', upload=True");
                string bom = $"113-{Util.GetStringBetween(fileText, "bom_rev='", "', dev=")}";
                string mebom = Util.GetStringBetween(fileText, "mebom='", "', pass_phrase");
                string region = Util.GetStringBetween(fileText, "region='", "', row_id");
                if (fileText.IndexOf($" FCD version: {appConfig.FcdName}") < 0)
                {
                    return new Tuple<bool, string>(false, "FCDVER");
                }
                else
                if (!bom.Equals(verConfig.BOM))
                {
                    return new Tuple<bool, string>(false, "BOMVER");
                }
                else
                if (!string.IsNullOrWhiteSpace(verConfig.Me_BOM) && !mebom.Equals(verConfig.Me_BOM))
                {
                    return new Tuple<bool, string>(false, "MEBVER");
                }
                else
                if (!string.IsNullOrWhiteSpace(verConfig.TlbBOM) && !toplevebom.Equals(verConfig.TlbBOM))
                {
                    return new Tuple<bool, string>(false, "TLBVER");
                }
                else
                if (!region.Equals(verConfig.RegionConfig.LogRegion))
                {
                    return new Tuple<bool, string>(false, "REGION");
                }
                return new Tuple<bool, string>(true, "");
            }
        }
        private void SaveLogToServer(string logPath, bool result, string mac, string errorCode)
        {
            string strTime_log = DateTime.Now.ToString("yyyyMMddhhmmss");
            string newName = result ?
                $"PASS_{mac}_{appConfig.Model}_{appConfig.Station}_{appConfig.PcName}_{strTime_log}.txt" :
                $"FAIL_{mac}_{appConfig.Model}_{appConfig.Station}_{appConfig.PcName}_{strTime_log}_{errorCode}.txt";
            try
            {
                string localDir = Path.GetDirectoryName(logPath);
                string newFile = Path.Combine(localDir, newName);
                File.Copy(logPath, newFile);
                File.Delete(logPath);
                string remotePath = Path.Combine("\\UBNT_Test_Logs_Download", appConfig.Model, appConfig.PcName, DateTime.Now.ToString("yyyy-MM-dd"), newName);
                var loogerSetting = appConfig.LoggerConfig;
                using (var sftp = new MySftp(loogerSetting.Host, loogerSetting.Port, loogerSetting.User, loogerSetting.Password))
                {
                    while (!sftp.Connect())
                    {
                        MessageBox.Show($"Connect to sever({loogerSetting.Host}) FAIL, pls check connection to SFTP", $"Slot{model.Index}");
                    }
                    while (!sftp.UploadFile(remotePath, newFile))
                    {
                        MessageBox.Show($"Save log to sever({loogerSetting.Host}) FAIL, pls check connection to SFTP", $"Slot{model.Index}");
                    }
                }
            }
            catch (Exception ex)
            {
                while (true)
                {
                    MessageBox.Show($"SaveLogToServer: {ex}");
                }
            }
        }
    }
}
