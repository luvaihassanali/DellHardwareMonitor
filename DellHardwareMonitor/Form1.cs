﻿using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

using DellFanManagement.DellSmbiozBzhLib;

namespace DellHardwareMonitor
{
    public partial class Form1 : Form
    {
        static public int opacity = Properties.Settings.Default.Opacity;
        private const int yValue = 8;
        private bool isDriverLoaded;
        private bool fanControl;
        private bool fanControlLow;
        private bool backgroundWorkerCompleted;
        private bool systemShutdown = false;

        private NotifyIcon trayIcon;
        private Timer pollingTimer;
        private Timer singleClickTimer;
        private ContextMenu trayMenu;
        private HardwareState state;
        private Form form2;

        private string dateString;
        private string cpuName = ConfigurationManager.AppSettings["cpuName"];
        private string gpuName = ConfigurationManager.AppSettings["gpuName"];
        private string ssdName = ConfigurationManager.AppSettings["ssdName"];
        private string hddName = ConfigurationManager.AppSettings["hddName"];
        private bool fourCoreHost = !String.Equals(String.Empty, ConfigurationManager.AppSettings["numCores"]);

        public Form1()
        {
            InitializeComponent();
            backgroundWorkerCompleted = false;
            backgroundWorker1.WorkerReportsProgress = false;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.RunWorkerAsync();

            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;

            #region Tray menu

            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Fan control high", FanControl);
            trayMenu.MenuItems.Add("Fan control low", FanControlLow);
            trayMenu.MenuItems.Add("Fan control off", FanControlOff);
            trayMenu.MenuItems[2].Checked = true;
            trayMenu.MenuItems.Add("-");
            trayMenu.MenuItems.Add("Reset orientation", ResetOrientation);
            trayMenu.MenuItems.Add("Reset network", ResetNetwork);
            trayMenu.MenuItems.Add("-");
            trayMenu.MenuItems.Add("Show", OnShow);
            trayMenu.MenuItems.Add("-");
            trayMenu.MenuItems.Add("Exit", OnExit);

            trayIcon = new NotifyIcon();
            trayIcon.Text = "Dell Hardware Monitor";
            trayIcon.Icon = Properties.Resources.wrench;
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
            trayIcon.MouseClick += new MouseEventHandler(trayIcon_Click);
            trayIcon.MouseDoubleClick += new MouseEventHandler(TrayIcon_MouseDoubleClick);

            #endregion

            pollingTimer = new Timer();
            pollingTimer.Tick += new EventHandler(polling_Tick);
            pollingTimer.Interval = Int32.Parse(ConfigurationManager.AppSettings["pollingInterval"]);
            singleClickTimer = new Timer();
            singleClickTimer.Tick += SingleClickTimer_Tick;

            form2 = new Form2();
            form2.MouseClick += Form2_MouseClick;
            form2.Show();
        }

        private void DrawForm(object sender, EventArgs e)
        {
            roundButton1.Invalidate();
            roundButton2.Invalidate();
            roundButton3.Invalidate();
            roundButton4.Invalidate();
        }

        #region General form functions

        private void Form1_Load(object sender, EventArgs e)
        {
            //Properties.Settings.Default.Opacity = 220;
            if (Properties.Settings.Default.Opacity == 220)
            {
                Rectangle screenBounds = Screen.FromControl(this).Bounds;
                this.Size = new Size(309, screenBounds.Height - 45);
                this.Location = new Point(screenBounds.Width - this.Size.Width - 10, yValue);
            }
            else
            {
                this.Location = Properties.Settings.Default.WindowLocation;
                this.Size = Properties.Settings.Default.WindowSize;
            }

            form2.Location = new Point(this.Location.X, this.Location.Y);
            form2.Size = new Size(this.Size.Width, this.Size.Height);
            loadingPictureBox.Location = new Point(this.Width / 2 - loadingPictureBox.Width / 2, this.Height / 2 - loadingPictureBox.Height / 2);

            isDriverLoaded = LoadDriver();
            if (!isDriverLoaded)
            {
                if (File.Exists("bzh_dell_smm_io_x64.sys"))
                {
                    MessageBox.Show("Failed to load DellSmbiosBzhLib driver. Verify administrator priveleges.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Failed to load DellSmbiosBzhLib driver. Verify that bzh_dell_smm_io_x64.sys is in application directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Application.Exit();
                Environment.Exit(1);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Fader.FadeIn(this, Fader.FadeSpeed.FourSlow);
            form2.Activate();
            this.Activate();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (systemShutdown) CleanUp();
            CleanUp();

            trayIcon.Visible = false;
            trayIcon.Dispose();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            label1.Focus();
        }

        private void Form2_MouseClick(object sender, MouseEventArgs e)
        {
            form2.Activate();
            this.Activate();
        }

        #region Tray icon functions

        private void trayIcon_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) singleClickTimer.Start();
            if (e != null && e.Button == MouseButtons.Right) return;
        }

        private void TrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            label1.Focus();
            if (e.Button == MouseButtons.Left)
            {
                singleClickTimer.Stop();
                Point initPos = this.Location;
                if (this.Location.Y == yValue)
                {
                    Properties.Settings.Default.WindowLocation = this.Location;
                    Properties.Settings.Default.WindowSize = this.Size;

                    for (int i = 0; i < 1251; i += 2)
                    {
                        this.Location = new Point(initPos.X, i);
                        form2.Location = new Point(initPos.X, i);
                    }

                    pollingTimer.Stop();
                    ShowInTaskbar = false;
                    Visible = false;
                    form2.ShowInTaskbar = false;
                    form2.Visible = false;
                }
                else
                {
                    if (backgroundWorkerCompleted) pollingTimer.Start();

                    ShowInTaskbar = false;
                    Visible = true;
                    form2.ShowInTaskbar = false;
                    form2.Visible = true;
                    form2.Activate();
                    this.Activate();

                    for (int i = 1250; i >= yValue; i -= 2)
                    {
                        this.Location = new Point(initPos.X, i);
                        form2.Location = new Point(initPos.X, i);
                    }
                }
            }
        }

        private void SingleClickTimer_Tick(object sender, EventArgs e)
        {
            label1.Focus();
            singleClickTimer.Stop();

            ShowInTaskbar = false;
            Visible = true;
            form2.ShowInTaskbar = false;
            form2.Visible = true;
            form2.Activate();
            this.Activate();

            if (this.Location.Y != yValue)
            {

                if (backgroundWorkerCompleted && !pollingTimer.Enabled)
                    pollingTimer.Start();

                Point initPos = this.Location;
                for (int i = 1250; i >= yValue; i -= 2)
                {
                    this.Location = new Point(initPos.X, i);
                    form2.Location = new Point(initPos.X, i);
                }
            }
        }

        #endregion 

        private void CleanUp()
        {
            if (backgroundWorker1.IsBusy) backgroundWorker1.CancelAsync();

            if (this.Location.Y == yValue)
            {
                Properties.Settings.Default.WindowLocation = this.Location;
                Properties.Settings.Default.WindowSize = this.Size;
            }
            Properties.Settings.Default.Opacity = opacity;
            Properties.Settings.Default.Save();

            pollingTimer.Stop();
            pollingTimer.Dispose();

            if (isDriverLoaded) UnloadDriver();

            if (state != null && state.Computer != null)
                state.Computer.Close();
        }

        protected override void WndProc(ref Message message)
        {   //WM_QUERYENDSESSION 0x11
            if (message.Msg == 0x11) systemShutdown = true;
            base.WndProc(ref message);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Oemplus)
            {
                opacity += 5;
                if (opacity > 220) opacity = 220;
                return true;
            }
            if (keyData == Keys.OemMinus)
            {
                opacity -= 5;
                if (opacity < 35) opacity = 35;
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        #region Context menu

        private void OnShow(object sender, EventArgs e)
        {
            form2.Activate();
            this.Activate();

            if (this.Location.Y != yValue)
            {
                if (backgroundWorkerCompleted)
                    pollingTimer.Start();

                ShowInTaskbar = false;
                Visible = true;
                form2.ShowInTaskbar = false;
                form2.Visible = true;

                form2.Activate();
                this.Activate();

                Point initPos = this.Location;

                for (int i = 1250; i >= yValue; i -= 2)
                {
                    this.Location = new Point(initPos.X, i);
                    form2.Location = new Point(initPos.X, i);
                }
            }
            else
            {

            }
        }

        private void OnExit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            trayIcon.Dispose();

            Application.Exit();
            System.Environment.Exit(0);
        }

        private void ResetOrientation(object sender, EventArgs e)
        {
            Rectangle screenBounds = Screen.FromControl(this).Bounds;
            this.Size = new Size(309, screenBounds.Height - 45);
            this.Location = new Point(screenBounds.Width - this.Size.Width - 10, yValue);
            form2.Size = new Size(309, screenBounds.Height - 45);
            form2.Location = new Point(screenBounds.Width - this.Size.Width - 10, yValue);
        }

        private void ResetNetwork(object sender, EventArgs e)
        {
            wifiHeaderLbl.Text = "";
            publicIP.Text = "";
            localhost.Text = "";
            this.Refresh();

            string publicIpAddr = "N/A";
            string localAddr = "N/A";
            try
            {
                publicIpAddr = new System.Net.WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
            }
            catch
            {
                publicIpAddr = "N/A";
            }

            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (var ipAddr in host.AddressList)
            {
                if (ipAddr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    string ipString = ipAddr.ToString();
                    if (ipString.Contains("192.168"))
                    {
                        localAddr = ipString;
                        break;
                    }
                }
            }

            wifiHeaderLbl.Text = GetSSID();
            publicIP.Text = publicIpAddr;
            localhost.Text = localAddr;
        }

        #region Fan control 

        private void FanControlLow(object sender, EventArgs e)
        {
            bool fanOneResult;
            bool fanTwoResult;

            if (fanControl)
            {
                trayIcon.Icon = Properties.Resources.wrench_yellow;
                trayMenu.MenuItems[1].Checked = true;
                trayMenu.MenuItems[0].Checked = false;
                fanControl = false;
                fanControlLow = true;

                fanOneResult = DellSmbiosBzh.SetFanLevel(BzhFanIndex.Fan1, BzhFanLevel.Level1);
                fanTwoResult = DellSmbiosBzh.SetFanLevel(BzhFanIndex.Fan2, BzhFanLevel.Level1);

                if (!fanOneResult || !fanTwoResult)
                {
                    MessageBox.Show("Unable to change fan speed level.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    System.Environment.Exit(1);
                }

                return;
            }

            if (fanControlLow)
            {
                trayMenu.MenuItems[2].Checked = true;
                trayIcon.Icon = Properties.Resources.wrench;
                fanControlLow = false;
                trayMenu.MenuItems[1].Checked = false;

                bool enableEc = DellSmbiosBzh.EnableAutomaticFanControl(false);
                if (!enableEc)
                {
                    MessageBox.Show("Unable to enable automatic fan control.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    System.Environment.Exit(1);
                }

                fanControlLbl.Text = "Disabled";

            }

            else
            {
                trayIcon.Icon = Properties.Resources.wrench_yellow;
                fanControlLow = true;
                trayMenu.MenuItems[2].Checked = false;
                trayMenu.MenuItems[1].Checked = true;

                bool disableEc = DellSmbiosBzh.DisableAutomaticFanControl(false);

                if (!disableEc)
                {
                    MessageBox.Show("Unable to disable automatic fan control.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    System.Environment.Exit(1);
                }

                fanControlLbl.Text = "Enabled";

                fanOneResult = DellSmbiosBzh.SetFanLevel(BzhFanIndex.Fan1, BzhFanLevel.Level1);
                fanTwoResult = DellSmbiosBzh.SetFanLevel(BzhFanIndex.Fan2, BzhFanLevel.Level1);

                if (!fanOneResult || !fanTwoResult)
                {
                    MessageBox.Show("Unable to change fan speed level.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    System.Environment.Exit(1);
                }
            }
        }

        private void FanControl(object sender, EventArgs e)
        {
            bool fanOneResult;
            bool fanTwoResult;

            if (fanControlLow)
            {
                trayIcon.Icon = Properties.Resources.wrench_red;
                trayMenu.MenuItems[1].Checked = false;
                trayMenu.MenuItems[0].Checked = true;
                fanControlLow = false;
                fanControl = true;

                fanOneResult = DellSmbiosBzh.SetFanLevel(BzhFanIndex.Fan1, BzhFanLevel.Level2);
                fanTwoResult = DellSmbiosBzh.SetFanLevel(BzhFanIndex.Fan2, BzhFanLevel.Level2);

                if (!fanOneResult || !fanTwoResult)
                {
                    MessageBox.Show("Unable to change fan speed level.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    System.Environment.Exit(1);
                }

                return;
            }

            if (fanControl)
            {
                trayIcon.Icon = Properties.Resources.wrench;
                fanControl = false;
                trayMenu.MenuItems[2].Checked = true;
                trayMenu.MenuItems[0].Checked = false;

                bool enableEc = DellSmbiosBzh.EnableAutomaticFanControl(false);
                if (!enableEc)
                {
                    MessageBox.Show("Unable to enable automatic fan control.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    System.Environment.Exit(1);
                }

                fanControlLbl.Text = "Disabled";

            }
            else
            {
                trayIcon.Icon = Properties.Resources.wrench_red;
                fanControl = true;
                trayMenu.MenuItems[2].Checked = false;
                trayMenu.MenuItems[0].Checked = true;

                bool disableEc = DellSmbiosBzh.DisableAutomaticFanControl(false);

                if (!disableEc)
                {
                    MessageBox.Show("Unable to disable automatic fan control.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    System.Environment.Exit(1);
                }

                fanControlLbl.Text = "Enabled";

                fanOneResult = DellSmbiosBzh.SetFanLevel(BzhFanIndex.Fan1, BzhFanLevel.Level2);
                fanTwoResult = DellSmbiosBzh.SetFanLevel(BzhFanIndex.Fan2, BzhFanLevel.Level2);

                if (!fanOneResult || !fanTwoResult)
                {
                    MessageBox.Show("Unable to change fan speed level.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    System.Environment.Exit(1);
                }
            }
        }

        private void FanControlOff(object sender, EventArgs e)
        {
            trayIcon.Icon = Properties.Resources.wrench;
            fanControl = false;
            trayMenu.MenuItems[2].Checked = true;
            trayMenu.MenuItems[1].Checked = false;
            trayMenu.MenuItems[0].Checked = false;

            bool enableEc = DellSmbiosBzh.EnableAutomaticFanControl(false);
            if (!enableEc)
            {
                MessageBox.Show("Unable to enable automatic fan control.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                System.Environment.Exit(1);
            }

            fanControlLbl.Text = "Disabled";
        }

        #endregion

        #endregion

        #region Monitor functions

        private void polling_Tick(object sender, EventArgs e)
        {
            // Refresh LibreHardwareMonitor sensor array
            state.CPU.Update();
            state.GPU.Update();
            state.RAM.Update();
            state.SSD.Update();
            state.HDD.Update();

            // First tick manually called with null parameter
            // Values within block are not expected to change (network)
            // Reset network tray icon context menu option available
            if (sender == null)
            {
                // Get local ip address
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ipAddr in host.AddressList)
                {
                    if (ipAddr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        string ipString = ipAddr.ToString();
                        if (ipString.Contains("192.168.0."))
                        {
                            localhost.Text = ipString;
                            break;
                        }
                    }
                }

                wifiHeaderLbl.Text = GetSSID();

                // Get public IP and current time (OpenCore dual boot workaround to inaccurate Windows time)
                try
                {
                    publicIP.Text = new System.Net.WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
                    dateString = new System.Net.WebClient().DownloadString("http://worldtimeapi.org/api/timezone/America/Toronto.txt");
                    dateString = dateString.Split('\n')[2].Replace("datetime: ", "");
                    dateString = dateString.Substring(0, dateString.Length - 6);
                    DateTime dateValue = DateTime.Parse(dateString).ToUniversalTime();

                    SYSTEMTIME st = new SYSTEMTIME();
                    st.wYear = (short)dateValue.Year; // must be short
                    st.wMonth = (short)dateValue.Month;
                    st.wDay = (short)dateValue.Day;
                    st.wHour = (short)dateValue.Hour;
                    st.wMinute = (short)dateValue.Minute;
                    st.wSecond = (short)dateValue.Second;

                    Form2.Win32.SetSystemTime(ref st);
                }
                catch
                {
                    publicIP.Text = "N/A";
                }

                cpuNameLbl.Text = state.CPU.Name;
                gpuNameLbl.Text = state.GPU.Name.Replace("NVIDIA", "NVIDIA ");
                ssdNameLbl.Text = state.SSD.Name;
                hddNameLbl.Text = state.HDD.Name;
            }

            try
            {
                // CPU Load
                float cpuOneLoad = (float)state.CPU.Sensors[0].Value;
                cpu1LoadLbl.Text = cpuOneLoad.ToString("0");
                float cpuTwoLoad = (float)state.CPU.Sensors[1].Value;
                cpu2LoadLbl.Text = cpuTwoLoad.ToString("0");
                float cpuThreeLoad = (float)state.CPU.Sensors[2].Value;
                cpu3LoadLbl.Text = cpuThreeLoad.ToString("0");
                float cpuFourLoad = (float)state.CPU.Sensors[3].Value;
                cpu4LoadLbl.Text = cpuFourLoad.ToString("0");
                int cpuTotalLoadIdx = fourCoreHost ? 4 : 6;
                float cpuTotalLoadF = (float)state.CPU.Sensors[cpuTotalLoadIdx].Value;
                cpuTotalLoadLbl.Text = cpuTotalLoadF.ToString("0");

                // CPU Temp
                int cpu1TempIdx = fourCoreHost ? 5 : 7;
                cpu1TempLbl.Text = state.CPU.Sensors[cpu1TempIdx].Value.ToString();
                cpu2TempLbl.Text = state.CPU.Sensors[cpu1TempIdx++].Value.ToString();
                cpu3TempLbl.Text = state.CPU.Sensors[cpu1TempIdx++].Value.ToString();
                cpu4TempLbl.Text = state.CPU.Sensors[cpu1TempIdx++].Value.ToString();
                int cpuPkgTempIdx = fourCoreHost ? 9 : 13;
                cpuPackageTempLbl.Text = state.CPU.Sensors[cpuPkgTempIdx].Value.ToString();

                // CPU Speed
                int cpuOneClockIdx = fourCoreHost ? 16 : 22;
                double cpuOneClock = (double)state.CPU.Sensors[cpuOneClockIdx].Value;
                cpu1ClockLbl.Text = cpuOneClock.ToString("0");
                double cpuTwoClock = (double)state.CPU.Sensors[cpuOneClockIdx++].Value;
                cpu2ClockLbl.Text = cpuTwoClock.ToString("0");
                double cpuThreeClock = (double)state.CPU.Sensors[cpuOneClockIdx++].Value;
                cpu3ClockLbl.Text = cpuThreeClock.ToString("0");
                double cpuFourClock = (double)state.CPU.Sensors[cpuOneClockIdx++].Value;
                cpu4ClockLbl.Text = cpuFourClock.ToString("0");
                int cpuPkgPwrIdx = fourCoreHost ? 20 : 28;
                float cpuPackagePower = (float)state.CPU.Sensors[cpuPkgPwrIdx].Value;
                cpuPackagePwrLbl.Text = cpuPackagePower.ToString("0.00");

                if (!fourCoreHost)
                {
                    float cpuFiveLoad = (float)state.CPU.Sensors[4].Value;
                    cpu5LoadLbl.Text = cpuFiveLoad.ToString("0");
                    float cpuSixLoad = (float)state.CPU.Sensors[5].Value;
                    cpu6LoadLbl.Text = cpuSixLoad.ToString("0");
                    cpu5TempLbl.Text = state.CPU.Sensors[11].Value.ToString();
                    cpu6TempLbl.Text = state.CPU.Sensors[12].Value.ToString();
                    double cpuFiveClock = (double)state.CPU.Sensors[26].Value;
                    cpu5ClockLbl.Text = cpuFiveClock.ToString("0");
                    double cpuSixClock = (double)state.CPU.Sensors[27].Value;
                    cpu6ClockLbl.Text = cpuSixClock.ToString("0");
                }

                // GPU
                gpuTempLbl.Text = state.GPU.Sensors[0].Value.ToString();
                gpuCoreClockLbl.Text = (state.GPU.Sensors[1].Value).Value.ToString("0");
                gpuMemClockLbl.Text = (state.GPU.Sensors[2].Value).Value.ToString("0");
                gpuCoreLoadLbl.Text = (state.GPU.Sensors[3].Value).Value.ToString("0");
                gpuTotalMemLbl.Text = (state.GPU.Sensors[7].Value / 1000d).Value.ToString("0.00");
                gpuFreeMemLbl.Text = (state.GPU.Sensors[8].Value / 1000d).Value.ToString("0.00");
                gpuMemUsedLbl.Text = (state.GPU.Sensors[9].Value / 1000d).Value.ToString("0.00");

                // Fans
                uint? leftFanRpm = DellSmbiosBzh.GetFanRpm(BzhFanIndex.Fan1);
                uint? rightFanRpm = DellSmbiosBzh.GetFanRpm(BzhFanIndex.Fan2);
                cpuFanLbl.Text = leftFanRpm.ToString();
                gpuFanLbl.Text = rightFanRpm.ToString();

                if (!fanControl && !fanControlLow)
                {
                    if (leftFanRpm > 0 && rightFanRpm > 0)
                    {
                        trayIcon.Icon = Properties.Resources.wrench_blue;
                    }
                    else
                    {
                        trayIcon.Icon = Properties.Resources.wrench;
                    }
                }

                // RAM
                float memoryUsed = (float)state.RAM.Sensors[0].Value;
                ramUsedLbl.Text = memoryUsed.ToString("0.00");
                float memoryAvailable = (float)state.RAM.Sensors[1].Value;
                ramAvailableLbl.Text = memoryAvailable.ToString("0.00");
                ramTotalLbl.Text = (memoryUsed + memoryAvailable).ToString("0.00");
                float memoryLoad = (float)state.RAM.Sensors[2].Value;
                ramLoadLbl.Text = memoryLoad.ToString("0");

                // SSD
                ssdTempLbl.Text = state.SSD.Sensors[0].Value.ToString();
                double ssdFreeGB = state.DriveStates[0].Counters[0].NextValue() / 1024d;
                ssdFreeGBLbl.Text = ssdFreeGB.ToString("0");
                double ssdFreePercent = state.DriveStates[0].Counters[1].NextValue();
                ssdProgressBar1.Value = (int)ssdFreePercent;
                double ssdUsedPercent = 100d - ssdFreePercent;
                ssdProgressBar1.Value = (int)ssdUsedPercent;
                double ssdTotalGB = ssdFreeGB / (ssdFreePercent / 100d);
                ssdTotalGBLbl.Text = ssdTotalGB.ToString("0");
                double ssdUsedGB = ssdTotalGB - ssdFreeGB;
                ssdUsedGBLbl.Text = ssdUsedGB.ToString("0");

                // HDD
                hddTempLbl.Text = state.HDD.Sensors[0].Value.ToString();
                double hddFreeGB = state.DriveStates[1].Counters[0].NextValue() / 1024d;
                hddFreeGBLbl.Text = hddFreeGB.ToString("0");
                double hddFreePercent = state.DriveStates[1].Counters[1].NextValue();
                double hddUsedPercent = 100d - hddFreePercent;
                hddProgressBar1.Value = (int)hddUsedPercent;
                double hddTotalGB = hddFreeGB / (hddFreePercent / 100d);
                hddTotalGBLbl.Text = hddTotalGB.ToString("0");
                double hddUsedGB = hddTotalGB - hddFreeGB;
                hddUsedGBLbl.Text = hddUsedGB.ToString("0");

                // Wi-Fi
                double wifiBytesRecv = state.NetworkStates[1].Counters[0].NextValue() / 1048576d;
                double wifiBytesSent = state.NetworkStates[1].Counters[1].NextValue() / 1048576d;
                wifiBytesRecvLbl.Text = wifiBytesRecv.ToString("0.00");
                wifiBytesSentLbl.Text = wifiBytesSent.ToString("0.00");
                downloadPictureBox.Visible = wifiBytesRecv > 0.001 ? true : false;
                uploadPictureBox.Visible = wifiBytesSent > 0.001 ? true : false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool LoadDriver()
        {
            bool result = DellSmbiosBzh.Initialize();
            if (!result)
            {
                return false;
            }
            return true;
        }

        private void UnloadDriver()
        {
            DellSmbiosBzh.Shutdown();
            isDriverLoaded = false;
        }

        private string GetSSID()
        {
            var process = new System.Diagnostics.Process
            {
                StartInfo = { FileName = "netsh.exe", Arguments = "wlan show interfaces", UseShellExecute = false, RedirectStandardOutput = true, CreateNoWindow = true }
            };
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string[] lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string line = null;
            foreach (string tempLine in lines)
            {
                if (tempLine.Contains("SSID") && !tempLine.Contains("BSSID"))
                {
                    line = tempLine;
                    break;
                }
            }

            if (line == null)
            {
                return "N/A";
            }

            string ssid = line.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[1].TrimStart();
            return ssid;
        }
        #endregion

        #region Background worker

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            state = new HardwareState(cpuName, gpuName, ssdName, hddName);
            BeginInvoke((MethodInvoker)delegate
            {
                polling_Tick(null, null);
                pollingTimer.Enabled = true;
                pollingTimer.Start();
            });
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            loadingPictureBox.Dispose();
            this.Visible = false;
            foreach (Control c in this.Controls)
            {
                c.Visible = true;
            }
            if (fourCoreHost)
            {
                label2.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                label13.Visible = false;
                label14.Visible = false;
                cpu5TempLbl.Visible = false;
                cpu6TempLbl.Visible = false;
                cpu5LoadLbl.Visible = false;
                cpu6LoadLbl.Visible = false;
                cpu5ClockLbl.Visible = false;
                cpu6ClockLbl.Visible = false;
            }
            uploadPictureBox.Visible = false;
            downloadPictureBox.Visible = false;
            backgroundWorkerCompleted = true;
            Fader.FadeIn(this, Fader.FadeSpeed.Slowest);
        }

        #endregion

        #region Bottom row buttons

        private void button1_Click(object sender, EventArgs e)
        {
            roundButton1.BackgroundImage = Properties.Resources.pia_invert;
            Application.DoEvents();
            string path = @"C:\Program Files\Private Internet Access\pia-client.exe";
            if (!File.Exists(path))
            {
                MessageBox.Show("PIA not installed");
            }
            var script = "Enable-ScheduledTask -TaskName \"LaunchPia\";Start-ScheduledTask -TaskName \"LaunchPia\";Disable-ScheduledTask -TaskName \"LaunchPia\"";
            var powerShell = PowerShell.Create().AddScript(script);
            powerShell.Invoke();
            label1.Focus();
            Task.Delay(500).Wait();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            roundButton2.BackgroundImage = Properties.Resources.windir_invert;
            string path = @"C:\Program Files (x86)\WinDirStat\windirstat.exe";
            if (!File.Exists(path))
            {
                MessageBox.Show("WinDirStat not installed");
            }
            System.Diagnostics.Process.Start(path);
            label1.Focus();
            Task.Delay(500).Wait();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            roundButton3.BackgroundImage = Properties.Resources.regedit_invert;
            System.Diagnostics.Process.Start("regedit.exe");
            label1.Focus();
            Task.Delay(500).Wait();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            roundButton4.BackgroundImage = Properties.Resources.computer_invert;
            System.Diagnostics.Process.Start("compmgmt.msc");
            label1.Focus();
            Task.Delay(500).Wait();
        }

        private void roundButton1_MouseEnter(object sender, EventArgs e)
        {
            roundButton1.BackgroundImage = Properties.Resources.pia;
        }

        private void roundButton1_MouseLeave(object sender, EventArgs e)
        {
            roundButton1.BackgroundImage = Properties.Resources.pia_b;
        }

        private void roundButton2_MouseEnter(object sender, EventArgs e)
        {
            roundButton2.BackgroundImage = Properties.Resources.windir;
        }

        private void roundButton2_MouseLeave(object sender, EventArgs e)
        {
            roundButton2.BackgroundImage = Properties.Resources.windir_b;
        }

        private void roundButton3_MouseEnter(object sender, EventArgs e)
        {
            roundButton3.BackgroundImage = Properties.Resources.regedit;
        }

        private void roundButton3_MouseLeave(object sender, EventArgs e)
        {
            roundButton3.BackgroundImage = Properties.Resources.regedit_b;
        }

        private void roundButton4_MouseEnter(object sender, EventArgs e)
        {
            roundButton4.BackgroundImage = Properties.Resources.computer;
        }

        private void roundButton4_MouseLeave(object sender, EventArgs e)
        {
            roundButton4.BackgroundImage = Properties.Resources.computer_b;
        }

        #endregion

    }
}

[StructLayout(LayoutKind.Sequential)]
public struct SYSTEMTIME
{
    public short wYear;
    public short wMonth;
    public short wDayOfWeek;
    public short wDay;
    public short wHour;
    public short wMinute;
    public short wSecond;
    public short wMilliseconds;
}

//https://stackoverflow.com/questions/778678/how-to-change-the-color-of-progressbar-in-c-sharp-net-3-5 William Daniel answer
public class ColorProgressBar : ProgressBar
{
    public ColorProgressBar()
    {
        this.SetStyle(ControlStyles.UserPaint, true);
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        // None... Helps control the flicker.
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        const int inset = 2; // A single inset value to control teh sizing of the inner rect
        using (Image offscreenImage = new Bitmap(this.Width, this.Height))
        {
            using (Graphics offscreen = Graphics.FromImage(offscreenImage))
            {
                Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
                if (ProgressBarRenderer.IsSupported) ProgressBarRenderer.DrawHorizontalBar(offscreen, rect);

                rect.Inflate(new Size(-inset, -inset)); // Deflate inner rect.
                rect.Width = (int)(rect.Width * ((double)this.Value / this.Maximum));
                if (rect.Width == 0) rect.Width = 1; // Can't draw rec with width of 0.

                LinearGradientBrush brush = new LinearGradientBrush(rect, this.BackColor, this.ForeColor, LinearGradientMode.Vertical);
                offscreen.FillRectangle(brush, inset, inset, rect.Width, rect.Height);
                e.Graphics.DrawImage(offscreenImage, 0, 0);
            }
        }
    }
}
class RoundButton : Button
{
    GraphicsPath GetRoundPath(RectangleF Rect, int radius)
    {
        float m = 2.75F;
        float r2 = radius / 2f;
        GraphicsPath GraphPath = new GraphicsPath();

        GraphPath.AddArc(Rect.X + m, Rect.Y + m, radius, radius, 180, 90);
        GraphPath.AddLine(Rect.X + r2 + m, Rect.Y + m, Rect.Width - r2 - m, Rect.Y + m);
        GraphPath.AddArc(Rect.X + Rect.Width - radius - m, Rect.Y + m, radius, radius, 270, 90);
        GraphPath.AddLine(Rect.Width - m, Rect.Y + r2, Rect.Width - m, Rect.Height - r2 - m);
        GraphPath.AddArc(Rect.X + Rect.Width - radius - m,
                       Rect.Y + Rect.Height - radius - m, radius, radius, 0, 90);
        GraphPath.AddLine(Rect.Width - r2 - m, Rect.Height - m, Rect.X + r2 - m, Rect.Height - m);
        GraphPath.AddArc(Rect.X + m, Rect.Y + Rect.Height - radius - m, radius, radius, 90, 90);
        GraphPath.AddLine(Rect.X + m, Rect.Height - r2 - m, Rect.X + m, Rect.Y + r2 + m);

        GraphPath.CloseFigure();
        return GraphPath;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        int borderRadius = 5;
        base.OnPaint(e);
        RectangleF Rect = new RectangleF(0, 0, this.Width, this.Height);
        GraphicsPath GraphPath = GetRoundPath(Rect, borderRadius);

        this.Region = new Region(GraphPath);
    }
}