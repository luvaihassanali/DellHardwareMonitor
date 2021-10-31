using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

using DellFanManagement.DellSmbiozBzhLib;
using DellHardwareMonitor.Properties;
using log4net;

namespace DellHardwareMonitor
{
    public partial class Form1 : Form
    {
        private double opacity;
        private bool isDriverLoaded;
        private bool fanControl;
        private NotifyIcon trayIcon;
        private Timer pollingTimer;
        private Timer timeTimer;
        private ContextMenu trayMenu;
        private HardwareState state;
        private Form form2;
        //private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            trayMenu.MenuItems.Add("Fan control", FanControl);
            trayMenu.MenuItems.Add("Reset network", ResetNetwork);
            trayMenu.MenuItems.Add("Show", OnShow);
            trayMenu.MenuItems.Add("Exit", OnExit);

            trayIcon = new NotifyIcon();
            trayIcon.Text = "DellHardwareMonitor";
            trayIcon.Icon = Resources.wrench;
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
            trayIcon.MouseClick += new MouseEventHandler(trayIcon_Click);

            pollingTimer = new Timer();
            pollingTimer.Tick += new EventHandler(polling_Tick);
            timeTimer = new Timer();
            timeTimer.Tick += new EventHandler(time_Tick);

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

            pollingTimer.Interval = Int32.Parse(ConfigurationManager.AppSettings["PollingInterval"]);
            timeTimer.Interval = Int32.Parse(ConfigurationManager.AppSettings["TimeInterval"]);

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
            }
            else
            {
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
        }

        private void OnExit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            trayIcon.Dispose();

            Application.Exit();
            System.Environment.Exit(1);
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

        private void FanControl(object sender, EventArgs e)
        {
            bool fanOneResult = true;
            bool fanTwoResult = true;

            if (fanControl)
            {
                fanControl = false;
                trayMenu.MenuItems[0].Checked = false;

                bool enableEc = DellSmbiosBzh.EnableAutomaticFanControl(false);
                if (!enableEc)
                {
                    MessageBox.Show("Unable to enable automatic fan control.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                fanControlLbl.Text = "Disabled";

            } 
            else
            {
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

        private void time_Tick(object sender, EventArgs e)
        {
            var timeUtc = new DateTime(DateTime.Now.Ticks, DateTimeKind.Unspecified); //DateTime.Now;
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime estDate = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
            timeBtn.Text = estDate.ToString("T", System.Globalization.CultureInfo.CreateSpecificCulture("en-us"));
        }

        private void polling_Tick(object sender, EventArgs e)
        {
            //update libre hardware monitor hardware items
            state.CPU.Update();
            state.GPU.Update();
            state.RAM.Update();
            state.SSD.Update();
            state.HDD.Update();
            state.WiFi.Update();

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
                }
                catch
                {
                    publicIP.Text = "N/A";
                }

                cpuNameLbl.Text = state.CPU.Name;
                gpuName.Text = state.GPU.Name;
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

            state = new HardwareState();

            BeginInvoke((MethodInvoker)delegate
            {
                polling_Tick(null, null);
                pollingTimer.Enabled = true;
                pollingTimer.Start();

                time_Tick(null, null);
                timeTimer.Enabled = true;
                timeTimer.Start();
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
            monthCalendar1.Visible = false;
        }

        #endregion

        #region Bottom row buttons

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\Users\luv\Documents\MouseJiggler.exe");
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

        private void timeBtn_Click(object sender, EventArgs e)
        {
            if(monthCalendar1.Visible)
            {
                monthCalendar1.Visible = false;
            } 
            else
            {
                monthCalendar1.Visible = true;
            }
            label1.Focus();
        }

        #endregion

    }

    public class NoEmptyRollingFileAppender : log4net.Appender.RollingFileAppender
    {
        private bool firstRun = true;

        protected override void OpenFile(string fileName, bool append)
        {
            if (firstRun)
            {
                firstRun = false;
                return;
            }
            base.OpenFile(fileName, append);
        }
    }
}

#region Fan control 

/*
// Attempt to load initialize driver
bool success = LoadDriver();

if (!success)
{
    Console.WriteLine("Failed to load driver properly. Press any key to exit.");
    Console.ReadKey();
    System.Environment.Exit(-1);
}
// Disable EC fan control.
Console.WriteLine("Attempting to disable EC control of the fan...");

success = DellSmbiosBzh.DisableAutomaticFanControl(false);

if (!success)
{
    Console.Error.WriteLine("Failed.");
    UnloadDriver();
    Console.WriteLine("Press any key to exit.");
    Console.ReadKey();
    System.Environment.Exit(-1);
}

Console.WriteLine(" ...Success.");

// Crank the fans up, for safety.
Console.WriteLine("Setting fan 1 speed to maximum...");
success = DellSmbiosBzh.SetFanLevel(BzhFanIndex.Fan1, BzhFanLevel.Level2);
if (!success)
{
    Console.Error.WriteLine("Failed.");
    UnloadDriver();
    Console.WriteLine("Press any key to exit.");
    Console.ReadKey();
    System.Environment.Exit(-1);
}

Console.WriteLine("Setting fan 2 speed to maximum...");
success = DellSmbiosBzh.SetFanLevel(BzhFanIndex.Fan2, BzhFanLevel.Level2);
if (!success)
{
    Console.Error.WriteLine("Failed.");
}

Console.WriteLine("Press any key to continue");
Console.ReadKey();

// Enable EC fan control.
// Console.WriteLine("Attempting to enable EC control of the fan...");

success = DellSmbiosBzh.EnableAutomaticFanControl(false);

if (!success)
{
    Console.Error.WriteLine("Failed.");
    UnloadDriver();
    System.Environment.Exit(-1);
}


UnloadDriver();
Console.WriteLine("Press any key to exit.");
Console.ReadKey();
*/

#endregion