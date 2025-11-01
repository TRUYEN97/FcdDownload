using AppUtil.Common;
using CPEI_MFG;
using CPEI_MFG.Config;
using CPEI_MFG.Service;
using CPEI_MFG.View;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using UiTest.Service.Communicate.Implement.Cmd;

namespace WebControl_WinForm
{


    public partial class UIDL : Form
    {
        private readonly ProgramConfig config;
        private readonly Sfis sfis;
        private readonly List<UnitTest> unitTests;
        private IWebDriver driver;
        public UIDL()
        {
            InitializeComponent();
            Text = $"UI Download - ver: 2025.11.1";
            config = ConfigLoader.ProgramConfig;
            sfis = new Sfis(config);
            unitTests = new List<UnitTest>();
            pnMain.ColumnCount = 1;
            pnMain.RowCount = 0;
            pnMain.Dock = DockStyle.Top;
            pnMain.AutoSize = true;
            pnMain.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnMain.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            pnMain.ColumnStyles.Clear();
            pnMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

        }

        public IWebDriver GetChromeDriver()
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            ChromeOptions options = new ChromeOptions();
            service.HideCommandPromptWindow = true;
            options.AddArgument("log-level=3");
            return new ChromeDriver(service, options);
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

        private bool PingToPi4()
        {
            const string pi4Ip = "192.168.1.19";
            using (CmdProcess cmd = new CmdProcess(false, "arp -d"))
            {
                cmd.WaitForExit();
            }
            Ping devicesPing = new Ping();
            AppUtil.Timer.Stopwatch stopwatch = new AppUtil.Timer.Stopwatch(5000);
            while (stopwatch.IsOntime)
            {
                if (devicesPing.Send(pi4Ip).Status == IPStatus.Success)
                {
                    return true;
                }
            }
            MessageBox.Show($"Ping to Pi4({pi4Ip}) Failed!");
            return false;
        }
        private void Form_Load(object sender, EventArgs e)
        {
            pnMain.Controls.Clear();
            var unitConfig = config.UnitConfigs;
            if (unitConfig == null)
            {
                MessageBox.Show("UnitConfig undefined!");
                Application.Exit();
            }
            foreach (var unit in unitConfig)
            {
                int index = pnMain.Controls.Count;
                var unitTest = new UnitTest(index, config, sfis, unit);
                unitTest.FocusNextControl += ctl =>
                {
                    if (!SelectNextControl(ctl, true, true, true, true))
                    {
                        MessageBox.Show("SelectNextControl");
                    }
                };
                pnMain.Controls.Add(unitTest.UnitView, 0, index);
                unitTests.Add(unitTest);
            }
            CheckCapslock();
            if (!InitialMainInfo()
                || !InitialVersionnInfo()
                || !InitialFcdInfo()
                || !InitialPoe()
                || !PingToPi4()
                || !sfis.Init()
                || !CheckPcName())
            {
                Application.Exit();
                return;
            }
            InitView();
            KillAllSubApp();
            try
            {
                driver = GetChromeDriver();
                if (driver == null)
                {
                    MessageBox.Show("Web driver == null!");
                    Application.Exit();
                    return;
                }
                foreach (var unit in unitTests)
                {
                    unit.Driver = driver;
                }
                driver.Navigate().GoToUrl(config.FcdUrl);
                IWebElement fcdNameElem = driver.FindElement(By.CssSelector("form#userinput h2"));
                if (fcdNameElem == null || string.IsNullOrEmpty(fcdNameElem.Text))
                {
                    MessageBox.Show("FCD undefined!");
                    KillAllSubApp();
                    Application.Exit();
                    return;
                }
                string wedFcdName = fcdNameElem.Text;
                if (wedFcdName != config.FcdName)
                {
                    MessageBox.Show($"Invalid FCD! Config[{config.FcdName}] != [{wedFcdName}]");
                    KillAllSubApp();
                    Application.Exit();
                    return;
                }
                var verConfig = config.VersionConfig;
                driver.FindElement(By.Id("passphrase")).SendKeys("m@8vm*Xr7e");
                driver.FindElement(By.Id("product")).SendKeys(config.UiProduct);
                driver.FindElement(By.Id("bomrev")).SendKeys(verConfig.BOM);
                if (!string.IsNullOrEmpty(verConfig.Me_BOM))
                {
                    driver.FindElement(By.Id("me_bomrev")).SendKeys(verConfig.Me_BOM);
                }
                if (!string.IsNullOrEmpty(verConfig.TlbBOM))
                {
                    driver.FindElement(By.Id("top_level_bomrev")).SendKeys(verConfig.TlbBOM);
                }
                driver.FindElement(By.Id("region")).SendKeys(verConfig.RegionConfig.Region);
                var poeConfig = config.PoeSwitchConfig;
                if (poeConfig?.IsPoeEnable == true && poeConfig.PoePorts.Count > 0)
                {
                    driver.FindElement(By.Id("power_supply_en")).Click();
                    Thread.Sleep(1000);
                    driver.FindElement(By.Id("ps_ipaddr")).Clear();
                    driver.FindElement(By.Id("ps_ipaddr")).SendKeys(poeConfig.PoeIp);
                    driver.FindElement(By.Id("ps_portmap")).Clear();
                    StringBuilder sb = new StringBuilder();
                    foreach (var port in poeConfig.PoePorts)
                    {
                        sb.Append(port);
                        sb.Append(',');
                    }
                    sb.Length--;
                    driver.FindElement(By.Id("ps_portmap")).SendKeys(sb.ToString());
                }
                Thread.Sleep(1000);
                driver.FindElement(By.Id("btnsubmit")).Click();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                KillAllSubApp();
                Application.Exit();
            }
        }
        private void InitView()
        {
            label_Model.Text = config.Model;
            label_Station.Text = config.Station;
            label_StationNO.Text = config.PcName;
        }

        private static void KillAllSubApp()
        {
            Process[] pProcess = Process.GetProcesses();
            try
            {
                foreach (Process p in pProcess)
                {
                    string pr = p.ProcessName;
                    if (pr == "chromedriver" || pr == "chrome" || pr == "conhost")
                    {
                        try
                        {
                            p.Kill();
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch { }
        }

        public List<string> ListDirectory(SftpClient client, string dirName)
        {
            var files = new List<string>();
            if (!client.Exists(dirName))
            {
                return files;
            }
            foreach (var entry in client.ListDirectory(dirName))
            {
                if (entry.Name == "." || entry.Name == "..")
                {
                    continue;
                }
                if (entry.IsDirectory)
                {
                    files.AddRange(ListDirectory(client, entry.FullName));
                }
                else
                {
                    files.Add(entry.FullName);
                }
            }
            return files;
        }
        private string GetFcdVersionFromName(string fullName)
        {
            fullName = fullName.Replace(".", "").Trim();
            string best = Util.FindGroup(fullName, @"_(\d{2,}_\d{2,})_?");
            return Regex.Replace(best, @"\D", "");
        }

        private bool InitialMainInfo()
        {
            config.PcName = Environment.MachineName.Trim();
            if (string.IsNullOrWhiteSpace(config.Model))
            {
                MessageBox.Show("model == null!");
                return false;
            }
            if (config.InputLength < 12)
            {
                MessageBox.Show($"input Length < 12! ({config.InputLength})");
                return false;
            }
            if (config.Model.Length != 11)
            {
                MessageBox.Show($"model.length != 11! ({config.Model})");
                return false;
            }
            var verConfig = this.config.VersionConfig;
            var regionConfig = verConfig.RegionConfig;
            if (config.Model.EndsWith("TS1") || config.Model.EndsWith("T01"))
            {
                regionConfig.SFC_region = "WORLD";
                regionConfig.LogRegion = "0000";
                regionConfig.Region = "World";
                regionConfig.PathRegion = "World";
            }
            else if (config.Model.EndsWith("TS2") || config.Model.EndsWith("T02"))
            {
                regionConfig.SFC_region = "US";
                regionConfig.LogRegion = "002a";
                regionConfig.Region = "USA/Canada";
                regionConfig.PathRegion = "USA_Canada_UI";
            }
            else
            {
                MessageBox.Show($"region of '{config.Model}' not config!");
                return false;
            }
            if (string.IsNullOrWhiteSpace(regionConfig.SFC_region))
            {
                MessageBox.Show("Region_Version == null!");
                return false;
            }
            if (string.IsNullOrWhiteSpace(config.Station))
            {
                MessageBox.Show("station == null!");
                return false;
            }
            return true;
        }
        private bool InitialPoe()
        {
            var poeConfig = this.config.PoeSwitchConfig;
            if (poeConfig.IsPoeEnable)
            {
                if (string.IsNullOrWhiteSpace(poeConfig.PoeIp))
                {
                    MessageBox.Show("POE Ip == null!");
                    return false;
                }
                if (poeConfig.PoePorts.Count == 0)
                {
                    MessageBox.Show("POE ports == null!");
                    return false;
                }
            }
            return true;
        }

        private bool InitialVersionnInfo()
        {
            var verConfig = this.config.VersionConfig;
            if (string.IsNullOrWhiteSpace(verConfig.FW))
            {
                MessageBox.Show("FW == null!");
                return false;
            }
            if (string.IsNullOrWhiteSpace(verConfig.FCD_Version))
            {
                MessageBox.Show("FCD_Version == null!");
                return false;
            }
            if (string.IsNullOrWhiteSpace(verConfig.BOM))
            {
                MessageBox.Show("BOM_Version == null!");
                return false;
            }
            return true;
        }

        private bool InitialFcdInfo()
        {
            if (string.IsNullOrWhiteSpace(config.UiProduct))
            {
                MessageBox.Show("Invalid UI_product!");
                return false;
            }
            string[] uiProductElems = config.UiProduct.Split(' ');
            if (uiProductElems.Length != 3)
            {
                MessageBox.Show($"Invalid UI_product! ({config.UiProduct})");
                return false;
            }
            config.UiProduct_name = uiProductElems[2];
            var verConfig = this.config.VersionConfig;
            config.LogPath = $"/home/ubnt/usbdisk/reg_logs/{config.UiProduct_name}/rev-{verConfig.BOM}/{verConfig.RegionConfig.PathRegion}";
            if (string.IsNullOrWhiteSpace(config.FcdUrl))
            {
                MessageBox.Show("fcd_Url == null!");
                return false;
            }
            if (string.IsNullOrWhiteSpace(config.FcdName) || verConfig.FCD_Version != GetFcdVersionFromName(config.FcdName))
            {
                MessageBox.Show($"FCD Version does not match FCD name\r\nFCD Version: [{verConfig.FCD_Version}] \r\nFTU name: [{config.FcdName}]");
                return false;
            }
            return true;
        }

        private bool CheckPcName()
        {
            string subModelName = config.Model.Length > 7 ? config.Model.Substring(0, 7) : config.Model;
            if (!config.PcName.Contains("DL"))
            {
                MessageBox.Show($"Vui long kiem tra lai ten may tinh la {config.PcName} khong chinh xac voi chuong trinh hien tai la DL\r\nVui long tim TE online kiem tra ten may tinh theo dinh dang: {subModelName}DL-XX!!!!", "CANH BAO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!config.PcName.Contains(subModelName))
            {
                MessageBox.Show($"Vui long kiem tra lai ten may tinh la {config.PcName} khong chinh xac voi chuong trinh hien tai la DL\r\nVui long tim TE online kiem tra ten may tinh theo dinh dang: {subModelName}DL-XX!!!!", "CANH BAO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (config.PcName.Length != 12)
            {
                MessageBox.Show("Station Name length != 12");
                return false;
            }
            return true;
        }

        private void UIDL_FormClosed(object sender, FormClosedEventArgs e)
        {
            KillAllSubApp();
        }
    }
}
