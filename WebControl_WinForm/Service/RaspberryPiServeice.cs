using AppUtil.Common;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CPEI_MFG.Services
{
    public class RaspberryPiServeice
    {
        public RaspberryPiServeice(string host, string username, string password)
        {
            Host = host;
            Username = username;
            Password = password;
            LogDir = @"D:\Check_UART";
        }

        public string Host { get; }
        public string Username { get; }
        public string Password { get; }
        public string LogDir { get; set; }

        public void DeleteArpIp(string ip)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ip))
                {
                    return;
                }
                ip = ip.Trim();
                using (var ssh = new SshClient(Host, Username, Password))
                {
                    ssh.Connect();
                    if (ssh.IsConnected)
                    {
                        var cmd = ssh.RunCommand($"echo \"ubnt\" | sudo -S arp -d {ip}");
                        ssh.Disconnect();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DeleteArpIP:({ip}) {ex.Message}");
            }


        }

        public bool ExecuteSSHCommands(int port, string keyWork)
        {
            bool rs = false;
            StringBuilder logBuilder = new StringBuilder();
            logBuilder.AppendLine($"===== Serial Device Test Log - {DateTime.Now} =====");
            AppendLog(logBuilder, $"Device IP: {Host}");
            AppendLog(logBuilder, $"Username: {Username}");
            AppendLog(logBuilder, $"Port: ttyUSB{port}");
            try
            {
                AppendLog(logBuilder, $"Ping to Host: {Host}");
                if (!Util.Ping(Host, 5000))
                {
                    AppendLog(logBuilder, $"Ping to Host: {Host} failed!");
                    return false;
                }
                using (var ssh = new SshClient(Host, Username, Password))
                {
                    ssh.Connect();
                    AppendLog(logBuilder, $"SSH connect to {Username}@{Host}");
                    if (!ssh.IsConnected)
                    {
                        AppendLog(logBuilder, $"SSH connect to {Username}@{Host} failed!");
                        return false;
                    }
                    using (var shell = ssh.CreateShellStream("xterm", 80, 24, 800, 600, 1024))
                    {
                        SendCommand(shell, $"stty -F /dev/ttyUSB{port} 115200 raw -echo", logBuilder);
                        SendCommand(shell, $"cat /dev/ttyUSB{port} &", logBuilder);
                        SendCommand(shell, $"echo > /dev/ttyUSB{port}", logBuilder);
                        SendCommand(shell, $"echo > /dev/ttyUSB{port}", logBuilder);
                        AppendLog(logBuilder, "Final Response:");
                        rs = TryReadUntil(shell, keyWork, logBuilder, 5000);
                        AppendLog(logBuilder, $"Result:  {(rs ? "PASS" : "FAIL")}");
                        SendCommand(shell, $"pkill -f 'cat /dev/ttyUSB{port}'", logBuilder);
                    }
                    ssh.Disconnect();
                    AppendLog(logBuilder, "SSH disconnect.");
                }
                return rs;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"RaspberryPiServeice.ExecuteSSHCommands({port}) {ex.Message}");
                return false;
            }
            finally
            {
                logBuilder.AppendLine("================================================");
                string logFilePath = Path.Combine(LogDir,
                    $"{DateTime.Now:ddMMyyyy}",
                    $"USB{port}",
                    $"{(rs ? "PASS" : "FAIL")}_USB{port}_SerialTestLog_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
                SaveLog(logFilePath, logBuilder.ToString());
            }
        }

        public void DeleteOldlogOf(string filepath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filepath))
                {
                    return;
                }
                filepath = filepath.Trim();
                // ssh vao teraterm
                using (var ssh = new SshClient(Host, Username, Password))
                {
                    ssh.Connect();
                    if (ssh.IsConnected)
                    {
                        // chay lenh  remove file log theo duong dan
                        var cmd = ssh.RunCommand($"echo \"ubnt\" | sudo -S rm -r {filepath}");
                        ssh.Disconnect();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"DeleteOldlogOf:({filepath}) {ex.Message}");
            }


        }
        public void DeleteOldTestLog(string mac, string remoteDir)
        {
            try
            {
                using (var sftp = new SftpClient(Host, Username, Password))
                {
                    sftp.Connect();
                    var files = ListDirectory(sftp, remoteDir);
                    foreach (string file in files)
                    {
                        string name = Path.GetFileName(file);
                        if (name.StartsWith(mac))
                        {
                            DeleteOldlogOf(file);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public string SearchAndDownLoadTestLog(string mac, string remoteDir, string localDir)
        {
            string log_result = "";
            try
            {
                using (var sftp = new SftpClient(Host, Username, Password))
                {
                    sftp.Connect();
                    if (!Directory.Exists(localDir))
                    {
                        Directory.CreateDirectory(localDir);
                    }
                    var files = ListDirectory(sftp, remoteDir);
                    foreach (string file in files)
                    {
                        string name = Path.GetFileName(file);
                        if (name.StartsWith(mac))
                        {
                            string localPath = Path.Combine(localDir, Path.GetFileName(file));
                            var stream = File.OpenWrite(localPath);
                            sftp.DownloadFile(file, stream);
                            stream.Close();
                            log_result = localPath;
                        }
                    }
                }
                return log_result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static void SendCommand(ShellStream shell, string command, StringBuilder stringBuilder)
        {
            shell.WriteLine(command);
            AppendLog(stringBuilder, command);
            Thread.Sleep(100);
        }

        private bool TryReadUntil(ShellStream shell, string expected, StringBuilder stringBuilder, int timeOut = 3000)
        {
            var buffer = new byte[1024];
            DateTime start = DateTime.Now;
            while (DateTime.Now - start < TimeSpan.FromMilliseconds(timeOut))
            {
                if (shell.DataAvailable)
                {
                    string chunk = shell.ReadLine(TimeSpan.FromMilliseconds(1000));
                    AppendLog(stringBuilder, chunk);
                    if (stringBuilder.ToString().Contains(expected))
                    {
                        return true;
                    }
                }
                Thread.Sleep(100);
            }
            return false;
        }
        public static List<string> ListDirectory(SftpClient client, string dirName)
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
        private static void SaveLog(string filePath, string logText)
        {
            try
            {
                string dir = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                File.AppendAllText(filePath, logText, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"RaspberryPiServeice.SaveLog({filePath}) {ex.Message}");
            }
        }

        private static void AppendLog(StringBuilder builder, string logtext)
        {
            builder?.AppendLine($"[{DateTime.Now:dd/MM/yyyy HH:mm:ss}] {logtext}");
        }
    }

}
