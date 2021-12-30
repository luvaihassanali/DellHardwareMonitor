using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using DellFanManagement.DellSmbiozBzhLib;
using DellHardwareMonitor.Properties;

namespace DellHardwareMonitor
{

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

    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime(ref SYSTEMTIME st);

        private double opacity;
        private bool isDriverLoaded;
        private bool fanControl;
        private bool fanControlLow;
        private NotifyIcon trayIcon;
        private Timer pollingTimer;
        private ContextMenu trayMenu;
        private HardwareState state;
        private Form form2;
        private string dateString;
        private string cpuName = ConfigurationManager.AppSettings["cpuName"];
        private string gpuName = ConfigurationManager.AppSettings["gpuName"];
        private string ssdName = ConfigurationManager.AppSettings["ssdName"];
        private string hddName = ConfigurationManager.AppSettings["hddName"];
        public Form1()
        {
            InitializeComponent();

            backgroundWorker1.WorkerReportsProgress = false;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.RunWorkerAsync();

            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Fan control high", FanControl);
            trayMenu.MenuItems.Add("Fan control low", FanControlLow);
            trayMenu.MenuItems.Add("Reset orientation", ResetOrientation);
            trayMenu.MenuItems.Add("Reset network", ResetNetwork);
            trayMenu.MenuItems.Add("Show", OnShow);
            trayMenu.MenuItems.Add("Exit", OnExit);

            trayIcon = new NotifyIcon();
            trayIcon.Text = "Dell Hardware Monitor";
            trayIcon.Icon = Resources.wrench;
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
            trayIcon.MouseClick += new MouseEventHandler(trayIcon_Click);

            pollingTimer = new Timer();
            pollingTimer.Tick += new EventHandler(polling_Tick);

            form2 = new Form();
            form2.FormBorderStyle = FormBorderStyle.None;
            form2.StartPosition = FormStartPosition.Manual;
            form2.BackColor = Color.Black;
            form2.ShowInTaskbar = false;
            form2.MouseClick += Form2_MouseClick;
        }

        #region Form functions

        private void Form1_Load(object sender, EventArgs e)
        {
            //Settings.Default.Opacity = 0;
            if (Settings.Default.Opacity == 0)
            {
                Rectangle screenBounds = Screen.FromControl(this).Bounds;
                this.Size = new Size(315, (screenBounds.Height - (10 * 3)));
                this.Location = new Point(screenBounds.Width - this.Size.Width + 10, 0);
                opacity = 0.8;
            }
            else
            {
                this.Location = Settings.Default.WindowLocation;
                this.Size = Settings.Default.WindowSize;
                opacity = Settings.Default.Opacity;
            }

            pollingTimer.Interval = Int32.Parse(ConfigurationManager.AppSettings["pollingInterval"]);

            form2.Location = new Point(this.Location.X, this.Location.Y);
            form2.Size = this.Size;

            isDriverLoaded = LoadDriver();

            if (!isDriverLoaded)
            {
                if (System.IO.File.Exists("bzh_dell_smm_io_x64.sys"))
                {
                    MessageBox.Show("Failed to load DellSmbiosBzhLib driver. Check administrator priveleges.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Failed to load DellSmbiosBzhLib driver. Check that bzh_dell_smm_io_x64.sys is in application directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Application.Exit();
                System.Environment.Exit(1);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Fader.FadeInCustom(form2, Fader.FadeSpeed.Slowest, opacity);
            Fader.FadeIn(this, Fader.FadeSpeed.Slowest);

            form2.Activate();
            this.Activate();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (systemShutdown)
            {
                CleanUp();
            }

            CleanUp();

            trayIcon.Visible = false;
            trayIcon.Dispose();
        }

        private void Form2_MouseClick(object sender, MouseEventArgs e)
        {
            form2.Activate();
            this.Activate();
        }

        private void trayIcon_Click(object sender, MouseEventArgs e)
        {
            if (e != null && e.Button == MouseButtons.Right)
            {
                return;
            }

            Point initPos = this.Location;

            
            if (this.Location.Y == 0)
            {
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
                pollingTimer.Start();
                ShowInTaskbar = false;
                Visible = true;
                form2.ShowInTaskbar = false;
                form2.Visible = true;

                form2.Activate();
                this.Activate();

                for (int i = 1250; i >= 0; i -= 2)
                {
                    this.Location = new Point(initPos.X, i);
                    form2.Location = new Point(initPos.X, i);
                }
            }
        }

        private void OnShow(object sender, EventArgs e)
        {

            form2.Activate();
            this.Activate();

            if (this.Location.Y != 0)
            {
                pollingTimer.Start();

                form2.Activate();
                this.Activate();

                Point initPos = this.Location;

                for (int i = 1250; i >= 0; i -= 2)
                {
                    this.Location = new Point(initPos.X, i);
                    form2.Location = new Point(initPos.X, i);
                }
            }
        }

        private void OnExit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            trayIcon.Dispose();

            Application.Exit();
            System.Environment.Exit(1);
        }

        private void ResetOrientation(object sender, EventArgs e)
        {
            Rectangle screenBounds = Screen.FromControl(this).Bounds;
            this.Size = new Size(315, (screenBounds.Height - (10 * 3)));
            this.Location = new Point(screenBounds.Width - this.Size.Width + 10, 0);
            form2.Size = new Size(315, (screenBounds.Height - (10 * 3)));
            form2.Location = new Point(screenBounds.Width - this.Size.Width + 10, 0);
        }

        private void ResetNetwork(object sender, EventArgs e)
        {
            try
            {
                publicIP.Text = new System.Net.WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
            }
            catch
            {
                publicIP.Text = "N/A";
            }
        }

        private void FanControlLow(object sender, EventArgs e)
        {
            bool fanOneResult = true;
            bool fanTwoResult = true;

            if (fanControl)
            {
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
                trayIcon.Icon = Resources.wrench;
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
                trayIcon.Icon = Resources.wrenchRed;
                fanControlLow = true;
                trayMenu.MenuItems[1].Checked = true;

                bool disableEc = DellSmbiosBzh.DisableAutomaticFanControl(false);

                if (!disableEc)
                {
                    MessageBox.Show("Unable to disable automatic fan control.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    System.Environment.Exit(1);
                }

                fanControlLbl.Text = "Enable";

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
            bool fanOneResult = true;
            bool fanTwoResult = true;

            trayIcon.Icon = Resources.wrenchRed;

            if (fanControlLow)
            {
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
                trayIcon.Icon = Resources.wrench;
                fanControl = false;
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
                trayIcon.Icon = Resources.wrenchRed;
                fanControl = true;
                trayMenu.MenuItems[0].Checked = true;

                bool disableEc = DellSmbiosBzh.DisableAutomaticFanControl(false);

                if (!disableEc)
                {
                    MessageBox.Show("Unable to disable automatic fan control.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    System.Environment.Exit(1);
                }

                fanControlLbl.Text = "Enable";

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

        private void CleanUp()
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }

            if (this.Location.Y == 0)
            {
                Settings.Default.WindowLocation = this.Location;
                Settings.Default.WindowSize = this.Size;
            }

            Settings.Default.Opacity = form2.Opacity;
            Settings.Default.Save();

            pollingTimer.Stop();
            pollingTimer.Dispose();

            if (isDriverLoaded)
            {
                UnloadDriver();
            }

            if (state != null && state.Computer != null)
            {
                state.Computer.Close();
            }
        }

        private static int WM_QUERYENDSESSION = 0x11;
        private static bool systemShutdown = false;
        protected override void WndProc(ref Message message)
        {
            if (message.Msg == WM_QUERYENDSESSION)
            {
                systemShutdown = true;
            }

            base.WndProc(ref message);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Oemplus)
            {
                form2.Opacity += 0.05;
                return true;
            }
            if (keyData == Keys.OemMinus)
            {
                form2.Opacity -= 0.05;
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        #region Monitor functions

        private void polling_Tick(object sender, EventArgs e)
        {
            //update libre hardware monitor hardware items
            state.CPU.Update();
            state.GPU.Update();
            state.RAM.Update();
            state.SSD.Update();
            state.HDD.Update();
            //state.WiFi.Update();

            //first tick manually called with null parameter
            //do not expect these values to change
            if (sender == null)
            {
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ipAddr in host.AddressList)
                {
                    if (ipAddr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        localhost.Text = ipAddr.ToString();
                        break;
                    }
                }

                //sometimes there's no internet
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

                    SetSystemTime(ref st); // invoke this method.
                }
                catch
                {
                    publicIP.Text = "N/A";
                }

                cpuNameLbl.Text = state.CPU.Name;
                gpuNameLbl.Text = state.GPU.Name;
                ssdNameLbl.Text = state.SSD.Name;
                hddNameLbl.Text = state.HDD.Name;
            }

            float cpuOneLoad = (float)state.CPU.Sensors[0].Value;
            cpu1LoadLbl.Text = cpuOneLoad.ToString("0");
            float cpuTwoLoad = (float)state.CPU.Sensors[1].Value;
            cpu2LoadLbl.Text = cpuTwoLoad.ToString("0");
            float cpuThreeLoad = (float)state.CPU.Sensors[2].Value;
            cpu3LoadLbl.Text = cpuThreeLoad.ToString("0");
            float cpuFourLoad = (float)state.CPU.Sensors[3].Value;
            cpu4LoadLbl.Text = cpuFourLoad.ToString("0");
            float cpuFiveLoad = (float)state.CPU.Sensors[4].Value;
            cpu5LoadLbl.Text = cpuFiveLoad.ToString("0");
            float cpuSixLoad = (float)state.CPU.Sensors[5].Value;
            cpu6LoadLbl.Text = cpuSixLoad.ToString("0");
            float cpuTotalLoadF = (float)state.CPU.Sensors[6].Value;
            cpuTotalLoadLbl.Text = cpuTotalLoadF.ToString("0");
            cpu1TempLbl.Text = state.CPU.Sensors[7].Value.ToString();
            cpu2TempLbl.Text = state.CPU.Sensors[8].Value.ToString();
            cpu3TempLbl.Text = state.CPU.Sensors[9].Value.ToString();
            cpu4TempLbl.Text = state.CPU.Sensors[10].Value.ToString();
            cpu5TempLbl.Text = state.CPU.Sensors[11].Value.ToString();
            cpu6TempLbl.Text = state.CPU.Sensors[12].Value.ToString();
            cpuPackageTempLbl.Text = state.CPU.Sensors[13].Value.ToString();
            double cpuOneClock = (double)state.CPU.Sensors[22].Value;
            cpu1ClockLbl.Text = cpuOneClock.ToString("0");
            double cpuTwoClock = (double)state.CPU.Sensors[23].Value;
            cpu2ClockLbl.Text = cpuTwoClock.ToString("0");
            double cpuThreeClock = (double)state.CPU.Sensors[24].Value;
            cpu3ClockLbl.Text = cpuThreeClock.ToString("0");
            double cpuFourClock = (double)state.CPU.Sensors[25].Value;
            cpu4ClockLbl.Text = cpuFourClock.ToString("0");
            double cpuFiveClock = (double)state.CPU.Sensors[26].Value;
            cpu5ClockLbl.Text = cpuFiveClock.ToString("0");
            double cpuSixClock = (double)state.CPU.Sensors[27].Value;
            cpu6ClockLbl.Text = cpuSixClock.ToString("0");
            float cpuPackagePower = (float)state.CPU.Sensors[28].Value;
            cpuPackagePwrLbl.Text = cpuPackagePower.ToString("0.00");

            gpuTempLbl.Text = state.GPU.Sensors[0].Value.ToString();
            gpuCoreClockLbl.Text = (state.GPU.Sensors[1].Value).Value.ToString("0");
            gpuMemClockLbl.Text = (state.GPU.Sensors[2].Value).Value.ToString("0");
            gpuCoreLoadLbl.Text = (state.GPU.Sensors[3].Value).Value.ToString("0");
            gpuTotalMemLbl.Text = (state.GPU.Sensors[7].Value / 1000d).Value.ToString("0.00");
            gpuFreeMemLbl.Text = (state.GPU.Sensors[8].Value / 1000d).Value.ToString("0.00");
            gpuMemUsedLbl.Text = (state.GPU.Sensors[9].Value / 1000d).Value.ToString("0.00");

            uint? leftFanRpm = DellSmbiosBzh.GetFanRpm(BzhFanIndex.Fan1);
            uint? rightFanRpm = DellSmbiosBzh.GetFanRpm(BzhFanIndex.Fan2);
            cpuFanLbl.Text = leftFanRpm.ToString();
            gpuFanLbl.Text = rightFanRpm.ToString();

            float memoryUsed = (float)state.RAM.Sensors[0].Value;
            ramUsedLbl.Text = memoryUsed.ToString("0.00");
            float memoryAvailable = (float)state.RAM.Sensors[1].Value;
            ramAvailableLbl.Text = memoryAvailable.ToString("0.00");
            ramTotalLbl.Text = (memoryUsed + memoryAvailable).ToString("0.00");
            float memoryLoad = (float)state.RAM.Sensors[2].Value;
            ramLoadLbl.Text = memoryLoad.ToString("0");

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

            double wifiBytesRecv = state.NetworkStates[1].Counters[0].NextValue() / 1048576d;
            wifiBytesRecvLbl.Text = wifiBytesRecv.ToString("0.00");
            if (wifiBytesRecv > 0.01)
            {
                downloadPictureBox.Visible = true;
            }
            else
            {
                downloadPictureBox.Visible = false;
            }

            double wifiBytesSent = state.NetworkStates[1].Counters[1].NextValue() / 1048576d;
            wifiBytesSentLbl.Text = wifiBytesSent.ToString("0.00");
            if (wifiBytesSent > 0.01)
            {
                uploadPictureBox.Visible = true;
            }
            else
            {
                uploadPictureBox.Visible = false;
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
            foreach (Control c in this.Controls)
            {
                c.Visible = true;
            }

            loadingPictureBox.Visible = false;
            uploadPictureBox.Visible = false;
            downloadPictureBox.Visible = false;
        }

        #endregion

        #region Bottom row buttons

        private void button1_Click(object sender, EventArgs e)
        {
            var script = "Enable-ScheduledTask -TaskName \"LaunchPia\";Start-ScheduledTask -TaskName \"LaunchPia\";Disable-ScheduledTask -TaskName \"LaunchPia\"";
            var powerShell = PowerShell.Create().AddScript(script);
            powerShell.Invoke();
            label1.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\Program Files (x86)\WinDirStat\windirstat.exe");
            label1.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("cleanmgr.exe");
            label1.Focus();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("regedit.exe");
            label1.Focus();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("compmgmt.msc");
            label1.Focus();
        }

        #endregion

    }
}