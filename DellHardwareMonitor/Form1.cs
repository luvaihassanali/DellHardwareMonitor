using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using DellFanManagement.DellSmbiozBzhLib;
using DellHardwareMonitor.Properties;

namespace DellHardwareMonitor
{
    public partial class Form1 : Form
    {
        private Timer pollingTimer;
        private bool isDriverLoaded;
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        private HardwareState state;
        private Form form2;
        private const int padding = 10;
        private Rectangle LeftSide { get { return new Rectangle(0, 0, padding, this.ClientSize.Height); } }

        public Form1()
        {
            InitializeComponent();

            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.RunWorkerAsync();

            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Info", OnInfo);
            trayMenu.MenuItems.Add("Exit", OnExit);

            trayIcon = new NotifyIcon();
            trayIcon.Text = "DellHardwareMonitor";
            trayIcon.Icon = Resources.wrench;
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
            trayIcon.MouseClick += new MouseEventHandler(trayIcon_Click);

            pollingTimer = new Timer();
            pollingTimer.Tick += new EventHandler(polling_Tick);
            pollingTimer.Interval = 1000;

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
            //Settings.Default.WindowOpacity = 0;
            if (Settings.Default.WindowOpacity == 0)
            {
                Rectangle screenBounds = Screen.FromControl(this).Bounds;
                this.Size = new Size(315, (screenBounds.Height - (padding * 3)));
                this.Location = new Point(screenBounds.Width - this.Size.Width + padding, 0);
                Settings.Default.WindowOpacity = 1;
            }
            else
            {

                this.Location = Settings.Default.WindowLocation;
                this.Size = Settings.Default.WindowSize;
                this.Opacity = Settings.Default.WindowOpacity;
            }

            form2.Location = new Point(this.Location.X, this.Location.Y);
            form2.Size = this.Size;

            isDriverLoaded = LoadDriver();

            if(!isDriverLoaded)
            {
                MessageBox.Show("Failed to load DellSmbiosBzhLib driver.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                System.Environment.Exit(1);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Fader.FadeInCustom(form2, Fader.FadeSpeed.Slowest, 0.8);
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

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Oemplus)
            {
                this.Opacity += 0.05;
            }
            if (e.Control && e.KeyCode == Keys.OemMinus)
            {
                this.Opacity -= 0.05;
            }
        }

        private void Form2_MouseClick(object sender, MouseEventArgs e)
        {
            form2.Activate();
            this.Activate();
        }

        private void trayIcon_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            Point initPos = this.Location;

            if (this.Location.Y == 0) 
            {
                for(int i = 0; i < 1251; i += 2)
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

        private void OnInfo(object sender, EventArgs e)
        {
            MessageBox.Show("Info line 1 \nInfo line 2\nInfo line 3");
        }

        private void OnExit(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }

            trayIcon.Visible = false;
            trayIcon.Dispose();

            Application.Exit();
            System.Environment.Exit(1);
        }

        private void CleanUp()
        {
            pollingTimer.Stop();
            pollingTimer.Dispose();

            if(this.Location.Y == 0)
            {
                Settings.Default.WindowLocation = this.Location;
                Settings.Default.WindowSize = this.Size;
                Settings.Default.WindowOpacity = this.Opacity;
                Settings.Default.Save();
            }

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

        #endregion

        #region Monitor functions

        private void polling_Tick(object sender, EventArgs e)
        {
            state.CPU.Update(); 
            state.GPU.Update(); 
            state.RAM.Update(); 
            state.SSD.Update(); 
            state.HDD.Update(); 
            state.WiFi.Update();

            cpuNameLbl.Text = state.CPU.Name;
            float cpuOneLoad = (float) state.CPU.Sensors[0].Value;
            cpu1LoadLbl.Text = cpuOneLoad.ToString("0.00");
            float cpuTwoLoad = (float)state.CPU.Sensors[1].Value;
            cpu2LoadLbl.Text = cpuTwoLoad.ToString("0.00");
            float cpuThreeLoad = (float)state.CPU.Sensors[1].Value;
            cpu3LoadLbl.Text = cpuThreeLoad.ToString("0.00");
            float cpuFourLoad = (float)state.CPU.Sensors[1].Value;
            cpu4LoadLbl.Text = cpuFourLoad.ToString("0.00");
            float cpuFiveLoad = (float)state.CPU.Sensors[1].Value;
            cpu5LoadLbl.Text = cpuFiveLoad.ToString("0.00");
            float cpuSixLoad = (float)state.CPU.Sensors[1].Value;
            cpu6LoadLbl.Text = cpuSixLoad.ToString("0.00");
            float cpuTotalLoadF = (float)state.CPU.Sensors[6].Value;
            cpuTotalLoadLbl.Text = cpuTotalLoadF.ToString("0.00");
            cpu1TempLbl.Text = state.CPU.Sensors[7].Value.ToString();
            cpu2TempLbl.Text = state.CPU.Sensors[8].Value.ToString();
            cpu3TempLbl.Text = state.CPU.Sensors[9].Value.ToString();
            cpu4TempLbl.Text = state.CPU.Sensors[10].Value.ToString();
            cpu5TempLbl.Text = state.CPU.Sensors[11].Value.ToString();
            cpu6TempLbl.Text = state.CPU.Sensors[12].Value.ToString();
            cpuPackageTempLbl.Text = state.CPU.Sensors[13].Value.ToString();
            double cpuOneClock = (double)state.CPU.Sensors[22].Value / 1000d;
            cpu1ClockLbl.Text = cpuOneClock.ToString("0.00");
            double cpuTwoClock = (double)state.CPU.Sensors[23].Value / 1000d;
            cpu2ClockLbl.Text = cpuTwoClock.ToString("0.00");
            double cpuThreeClock = (double)state.CPU.Sensors[24].Value / 1000d;
            cpu3ClockLbl.Text = cpuThreeClock.ToString("0.00");
            double cpuFourClock = (double)state.CPU.Sensors[25].Value / 1000d;
            cpu4ClockLbl.Text = cpuFourClock.ToString("0.00");
            double cpuFiveClock = (double)state.CPU.Sensors[26].Value / 1000d;
            cpu5ClockLbl.Text = cpuFiveClock.ToString("0.00");
            double cpuSixClock = (double)state.CPU.Sensors[27].Value / 1000d;
            cpu6ClockLbl.Text = cpuSixClock.ToString("0.00");
            float cpuPackagePower = (float)state.CPU.Sensors[28].Value;
            cpuPackagePwrLbl.Text = cpuPackagePower.ToString("0.00");
            
            gpuName.Text = state.GPU.Name;
            gpuTempLbl.Text = state.GPU.Sensors[0].Value.ToString();
            gpuCoreClockLbl.Text = (state.GPU.Sensors[1].Value / 1000d).Value.ToString("0.00");
            gpuMemClockLbl.Text = (state.GPU.Sensors[2].Value / 1000d).Value.ToString("0.00");
            gpuCoreLoadLbl.Text = (state.GPU.Sensors[3].Value).Value.ToString("0.00");
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
            ramLoadLbl.Text = memoryLoad.ToString("0.00");

            ssdNameLbl.Text = state.SSD.Name;
            ssdTempLbl.Text = state.SSD.Sensors[0].Value.ToString();
            double ssdFreeGB = state.DriveStates[0].Counters[0].NextValue() / 1024d;
            ssdFreeGBLbl.Text = ssdFreeGB.ToString("0.00");
            double ssdFreePercent = state.DriveStates[0].Counters[1].NextValue();
            ssdProgressBar1.Value = (int)ssdFreePercent;
            double ssdUsedPercent = 100d - ssdFreePercent;
            ssdProgressBar1.Value = (int)ssdUsedPercent;
            double ssdTotalGB = ssdFreeGB / (ssdFreePercent / 100d);
            ssdTotalGBLbl.Text = ssdTotalGB.ToString("0.00");
            double ssdUsedGB = ssdTotalGB - ssdFreeGB;
            ssdUsedGBLbl.Text = ssdUsedGB.ToString("0.00");

            hddNameLbl.Text = state.HDD.Name;
            hddTempLbl.Text = state.HDD.Sensors[0].Value.ToString();
            double hddFreeGB = state.DriveStates[1].Counters[0].NextValue() / 1024d;
            hddFreeGBLbl.Text = hddFreeGB.ToString("0.00");
            double hddFreePercent = state.DriveStates[1].Counters[1].NextValue();
            double hddUsedPercent = 100d - hddFreePercent;
            hddProgressBar1.Value = (int)hddUsedPercent; 
            double hddTotalGB = hddFreeGB / (hddFreePercent / 100d);
            hddTotalGBLbl.Text = hddTotalGB.ToString("0.00");
            double hddUsedGB = hddTotalGB - hddFreeGB;
            hddUsedGBLbl.Text = hddUsedGB.ToString("0.00");

            double wifiBytesRecv = state.NetworkStates[1].Counters[0].NextValue() / 1048576d;
            wifiBytesRecvLbl.Text = wifiBytesRecv.ToString("0.00");
            double wifiBytesSent = state.NetworkStates[1].Counters[1].NextValue() / 1048576d;
            wifiBytesSentLbl.Text = wifiBytesSent.ToString("0.00");
            publicIP.Text = state.PublicIpAddress;
            localhost.Text = state.LocalHost;
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

            this.Invoke(new MethodInvoker(delegate {
                polling_Tick(null, null);
                pollingTimer.Enabled = true;
                pollingTimer.Start();
            }));
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                c.Visible = true;
            }

            loadingPictureBox.Visible = false;
        }

        #endregion

        #region Bottom row buttons

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start(@"C:\Users\luv\Documents\MouseJiggler.exe");
            label1.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start(@"C:\Program Files (x86)\WinDirStat\windirstat.exe");
            label1.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start("cleanmgr.exe");
            label1.Focus();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process.Start("regedit.exe");
            label1.Focus();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Process.Start("compmgmt.msc");
            label1.Focus();
        }

        #endregion

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