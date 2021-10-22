using System;
using System.Drawing;
using System.Windows.Forms;

using DellFanManagement.DellSmbiozBzhLib;
using DellHardwareMonitor.Properties;

namespace DellHardwareMonitor
{
    public partial class Form1 : Form
    {
        private Timer pollingTimer;
        private Boolean isDriverLoaded;
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        private HardwareState state;

        private const int padding = 10;
        private Rectangle LeftSide { get { return new Rectangle(0, 0, padding, this.ClientSize.Height); } }

        public Form1()
        {
            InitializeComponent();

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

            isDriverLoaded = false; //LoadDriver();

            /*if(!isDriverLoaded)
            {
                MessageBox.Show("Failed to load DellSmbiosBzhLib driver.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                System.Environment.Exit(1);
            }*/

            state = new HardwareState();

            pollingTimer = new Timer();
            pollingTimer.Tick += new EventHandler(polling_Tick);
            pollingTimer.Interval = 1000;
            pollingTimer.Enabled = true;
            pollingTimer.Start();
        }

        #region Form functions

        private void Form1_Load(object sender, EventArgs e)
        {
            //Settings.Default.WindowOpacity = 0;
            if (Settings.Default.WindowOpacity == 0)
            {
                Rectangle screenBounds = Screen.FromControl(this).Bounds;
                this.Size = new Size(300, (screenBounds.Height - (padding * 3)));
                this.Location = new Point(screenBounds.Width - this.Size.Width + padding, 0);
                Settings.Default.WindowOpacity = 1;
            }
            else
            {
                this.Location = Settings.Default.WindowLocation;
                this.Size = Settings.Default.WindowSize;
                this.Opacity = Settings.Default.WindowOpacity;
            }

            //this.WindowState = FormWindowState.Minimized;
            //Visible = false;
            ShowInTaskbar = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.WindowLocation = this.Location;
            Settings.Default.WindowSize = this.Size;
            Settings.Default.WindowOpacity = this.Opacity;
            Settings.Default.Save();
        }

        private void trayIcon_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (this.WindowState == FormWindowState.Normal)
            {
                Visible = false;
                ShowInTaskbar = false;
                this.WindowState = FormWindowState.Minimized;
            }
            else
            {
                Visible = true;
                ShowInTaskbar = false;
                this.WindowState = FormWindowState.Normal;
                Activate();
            }
        }

        private void OnInfo(object sender, EventArgs e)
        {
            MessageBox.Show("Info line 1 \nInfo line 2\nInfo line 3");
        }

        private void OnExit(object sender, EventArgs e)
        {
            Settings.Default.WindowLocation = this.Location;
            Settings.Default.WindowSize = this.Size;
            Settings.Default.WindowOpacity = this.Opacity;
            Settings.Default.Save();

            Console.WriteLine(this.Location);
            Console.WriteLine(this.Size);

            if (isDriverLoaded)
            {
                UnloadDriver();
            }

            if (state != null && state.Computer != null)
            {
                state.Computer.Close();
            }

            trayIcon.Visible = false;
            trayIcon.Dispose();

            Application.Exit();
            System.Environment.Exit(1);
        }

        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == 0x84) // WM_NCHITTEST
            {
                var cursor = this.PointToClient(Cursor.Position);

                if (LeftSide.Contains(cursor)) message.Result = (IntPtr)10;
            }
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
        #endregion

        #region Monitor functions

        private void polling_Tick(object sender, EventArgs e)
        {
            //CPU sensors
            state.CPU.Update();
            string cpuName = state.CPU.Name;
            float? cpuOneLoad = state.CPU.Sensors[0].Value;
            float? cpuTwoLoad = state.CPU.Sensors[1].Value;
            float? cpuThreeLoad = state.CPU.Sensors[2].Value;
            float? cpuFourLoad = state.CPU.Sensors[3].Value;
            float? cpuFiveLoad = state.CPU.Sensors[4].Value;
            float? cpuSixLoad = state.CPU.Sensors[5].Value;
            float? totalLoad = state.CPU.Sensors[6].Value;
            float? cpuOneTemp = state.CPU.Sensors[7].Value;
            float? cpuTwoTemp = state.CPU.Sensors[8].Value;
            float? cpuThreeTemp = state.CPU.Sensors[9].Value;
            float? cpuFourTemp = state.CPU.Sensors[10].Value;
            float? cpuFiveTemp = state.CPU.Sensors[11].Value;
            float? cpuSixTemp = state.CPU.Sensors[12].Value;
            float? cpuPackageTemp = state.CPU.Sensors[13].Value;
            //14 - 19 core distance to tj max
            //float? coreMax = state.CPU.Sensors[20].Value;
            //float? coreAverage = state.CPU.Sensors[21].Value;
            double? cpuOneClock = state.CPU.Sensors[22].Value / 1000d;
            double? cpuTwoClock = state.CPU.Sensors[23].Value / 1000d;
            double? cpuThreeClock = state.CPU.Sensors[24].Value / 1000d;
            double? cpuFourClock = state.CPU.Sensors[25].Value / 1000d;
            double? cpuFiveClock = state.CPU.Sensors[26].Value / 1000d;
            double? cpuSixClock = state.CPU.Sensors[27].Value / 1000d;
            float? cpuPackagePower = state.CPU.Sensors[28].Value;
            //float? cpuCoresPower = state.CPU.Sensors[29].Value;
            //float? cpuGraphicsPower = state.CPU.Sensors[30].Value;
            //float? cpuMemoryPower = state.CPU.Sensors[31].Value;
            //float? cpuBusSpeed = state.CPU.Sensors[32].Value;

            //GPU
            state.GPU.Update();
            string gpuName = state.GPU.Name;
            float? gpuTemp = state.GPU.Sensors[0].Value;
            double? gpuCoreClock = state.GPU.Sensors[1].Value / 1000d;
            double? gpuMemoryClock = state.GPU.Sensors[2].Value / 1000d;
            float? gpuCoreLoad = state.GPU.Sensors[3].Value;
            //float? gpuMemoryController = state.GPU.Sensors[4].Value;
            //Latest NVIDIA drivers apparently leave this setting out
            //float? gpuVideoEngineLoad = state.GPU.Sensors[5].Value;
            //float? gpuBusLoad = state.GPU.Sensors[6].Value;
            double? gpuTotalMemory = state.GPU.Sensors[7].Value / 1000d;
            double? gpuFreeMemory = state.GPU.Sensors[8].Value / 1000d;
            double? gpuMemoryUsed = state.GPU.Sensors[9].Value / 1000d;
            float? gpuPackagePower = state.GPU.Sensors[10].Value;
            //11, 12 GPU PCIe Rx/Tx

            //RAM
            state.RAM.Update();
            float? memoryUsed = state.RAM.Sensors[0].Value;
            float? memoryAvailable = state.RAM.Sensors[1].Value;
            float? memoryLoad = state.RAM.Sensors[2].Value;
            //float? virtualMemoryUsed = state.RAM.Sensors[3].Value;
            //float? virtualMemoryAvailable = state.RAM.Sensors[4].Value;
            //float? virtualMemoryLoad = state.RAM.Sensors[5].Value;

            //SSD
            state.SSD.Update();
            string ssdName = state.SSD.Name;
            float? ssdTemp = state.SSD.Sensors[0].Value;
            //float? ssdAvailableSpare = state.SSD.Sensors[1].Value;
            //float? ssdAvailableSpareThreshold = state.SSD.Sensors[2].Value;
            //float? ssdPercentUsed = state.SSD.Sensors[3].Value;
            //04, 05 data read/written
            //float? ssdTemp1 = state.SSD.Sensors[6].Value;
            //float? ssdUsedSpace = state.SSD.Sensors[7].Value;
            //float? ssdWriteActivity = state.SSD.Sensors[8].Value;
            //float? ssdTotalActivity = state.SSD.Sensors[9].Value;
            //float? ssdReadRate = state.SSD.Sensors[10].Value;
            //float? ssdWriteRate = state.SSD.Sensors[11].Value;

            double ssdFreeGB = state.DriveStates[0].Counters[0].NextValue() / 1024d;
            double ssdFreePercent = state.DriveStates[0].Counters[1].NextValue();
            double ssdUsedPercent = 100d - ssdFreePercent;
            double ssdTotalGB = ssdFreeGB / (ssdFreePercent / 100d);
            double ssdUsedGB = ssdTotalGB - ssdFreeGB;
            
            //HDD
            state.HDD.Update();
            string hddName = state.HDD.Name;
            float? hddTemp = state.HDD.Sensors[0].Value;
            //float? hddUsedSpace = state.HDD.Sensors[1].Value;
            //float? hddWriteActivity = state.HDD.Sensors[2].Value;
            //float? hddTotalActivity = state.HDD.Sensors[3].Value;
            //float? hddReadRate = state.HDD.Sensors[4].Value;
            //float? hddWriteRate = state.HDD.Sensors[5].Value;

            double hddFreeGB = state.DriveStates[1].Counters[0].NextValue() / 1024d;
            double hddFreePercent = state.DriveStates[1].Counters[1].NextValue();
            double hddUsedPercent = 100d - hddFreePercent;
            double hddTotalGB = hddFreeGB / (hddFreePercent / 100d);
            double hddUsedGB = hddTotalGB - hddFreeGB;

            /*//Ethernet
            state.Ethernet.Update();
            float? ethDataUploaded = state.WiFi.Sensors[0].Value;
            float? ethDataDownloaded = state.WiFi.Sensors[1].Value;
            float? ethDownloadSpeed = state.WiFi.Sensors[2].Value;
            float? ethUploadSpeed = state.WiFi.Sensors[3].Value;
            float? ethNetworkUtilisation = state.WiFi.Sensors[4].Value;
            float ethBytesRecv = state.NetworkStates[0].Counters[0].NextValue();
            float ethBytesSent = state.NetworkStates[0].Counters[1].NextValue();*/

            //Wi-Fi
            state.WiFi.Update();
            //float? wifiDataUploaded = state.WiFi.Sensors[0].Value;
            //float? wifiDataDownloaded = state.WiFi.Sensors[1].Value;
            //float? wifiDownloadSpeed = state.WiFi.Sensors[2].Value;
            //float? wifiUploadSpeed = state.WiFi.Sensors[3].Value;
            //float? wifiNetworkUtilisation = state.WiFi.Sensors[4].Value;
            double wifiBytesRecv = state.NetworkStates[1].Counters[0].NextValue() / 1048576d;
            double wifiBytesSent = state.NetworkStates[1].Counters[1].NextValue() / 1048576d;

            richTextBox1.Clear();

            richTextBox1.Text +=
                "cpuName: " + cpuName + "\n" +
                "cpuOneLoad: " + cpuOneLoad + " %\n" +
                "cpuTwoLoad: " + cpuTwoLoad + " %\n" +
                "cpuThreeLoad: " + cpuThreeLoad + " %\n" +
                "cpuFourLoad: " + cpuFourLoad + " %\n" +
                "cpuFiveLoad: " + cpuFiveLoad + " %\n" +
                "cpuSixLoad: " + cpuSixLoad + " %\n" +
                "totalLoad: " + totalLoad + " %\n" +
                "cpuOneTemp: " + cpuOneTemp + " °C\n" +
                "cpuTwoTemp: " + cpuTwoTemp + " °C\n" +
                "cpuThreeTemp: " + cpuThreeTemp + " °C\n" +
                "cpuFourTemp: " + cpuFourTemp + " °C\n" +
                "cpuFiveTemp: " + cpuFiveTemp + " °C\n" +
                "cpuSixTemp: " + cpuSixTemp + " °C\n" +
                "cpuPackageTemp: " + cpuPackageTemp + " °C\n" +
                //"coreMax: " + coreMax + "\n" +
                //"coreAverage: " + coreAverage + "\n" +
                "cpuOneClock: " + cpuOneClock + " GHz\n" +
                "cpuTwoClock: " + cpuTwoClock + " GHz\n" +
                "cpuThreeClock: " + cpuThreeClock + " GHz\n" +
                "cpuFourClock: " + cpuFourClock + " GHz\n" +
                "cpuFiveClock: " + cpuFiveClock + " GHz\n" +
                "cpuSixClock: " + cpuSixClock + " GHz\n" +
                "cpuPackagePower: " + cpuPackagePower + " W\n\n";
                //"cpuCoresPower: " + cpuCoresPower + "\n" +
                //"cpuGraphicsPower: " + cpuGraphicsPower + "\n" +
                //"cpuMemoryPower: " + cpuMemoryPower + "\n" +
                //"cpuBusSpeed: " + cpuBusSpeed + "\n\n";

            richTextBox1.Text +=
                "gpuName: " + gpuName + "\n" +
                "gpuTemp: " + gpuTemp + " °C\n" +
                "gpuCoreClock: " + gpuCoreClock + " GHz\n" +
                "gpuMemoryClock: " + gpuMemoryClock + " GHz\n" +
                "gpuCoreLoad: " + gpuCoreLoad + " %\n" +
                //"gpuMemoryController: " + gpuMemoryController + "\n" +
                //"gpuVideoEngineLoad: " + gpuVideoEngineLoad + " %\n" +
                //"gpuBusLoad: " + gpuBusLoad + "\n" +
                "gpuTotalMemory: " + gpuTotalMemory + " GB\n" +
                "gpuFreeMemory: " + gpuFreeMemory + " GB\n" +
                "gpuMemoryUsed: " + gpuMemoryUsed + " GB\n" +
                "gpuPackagePower: " + gpuPackagePower + " W\n\n";

            richTextBox1.Text +=
                "RAM \n" +
                "memoryTotal: " + (float)(memoryUsed + memoryAvailable) + " GB\n" +
                "memoryUsed: " + memoryUsed + " GB\n" +
                "memoryAvailable: " + memoryAvailable + " GB\n" +
                "memoryLoad: " + memoryLoad + " %\n\n";
                //"virtualMemoryUsed: " + virtualMemoryUsed + "\n" +
                //"virtualMemoryAvailable: " + virtualMemoryAvailable + "\n" +
                //"virtualMemory: " + virtualMemoryLoad + "\n\n";

            richTextBox1.Text +=
                "ssdName: " + ssdName + "\n" +
                "ssdTemp: " + ssdTemp + "\n" +
                //"ssdAvailableSpare: " + ssdAvailableSpare + "\n" +
                //"ssdAvailableSpareThreshold: " + ssdAvailableSpareThreshold + "\n" +
                //"ssdPercentUsed: " + ssdPercentUsed + "\n" +
                //"ssdTemp1: " + ssdTemp1 + "\n" +
                //"ssdUsedSpace: " + ssdUsedSpace + "\n" +
                //"ssdWriteActivity: " + ssdWriteActivity + "\n" +
                //"ssdTotalActivity: " + ssdTotalActivity + "\n" +
                //"ssdReadRate: " + ssdReadRate + "\n" +
                //"ssdWriteRate: " + ssdWriteRate + "\n" +
                "ssdTotalGB: " + ssdTotalGB + " GB\n" +
                "ssdFreeGB: " + ssdFreeGB + " GB\n" +
                "ssdUsedGB: " + ssdUsedGB + " GB\n" +
                "ssdFreePercent: " + ssdFreePercent + " %\n" +
                "ssdUsedPercent: " + ssdUsedPercent + " %\n\n";

            richTextBox1.Text +=
                "hddName: " + hddName + "\n" +
                "hddTemp: " + hddTemp + "\n" +
                //"hddUsedSpace: " + hddUsedSpace + "\n" +
                //"hddWriteActivity: " + hddWriteActivity + "\n" +
                //"hddTotalActivity: " + hddTotalActivity + "\n" +
                //"hddReadRate: " + hddReadRate + "\n" +
                //"hddWriteRate: " + hddWriteRate + "\n" +
                "hddTotalGB: " + hddTotalGB + " GB\n" +
                "hddUsedGB: " + hddUsedGB + " GB\n" +
                "hddFreeGB: " + hddFreeGB + " GB\n" +
                "hddFreePercent: " + hddFreePercent + " %\n" +
                "hddUsedPercent: " + hddUsedPercent + " %\n\n";

            richTextBox1.Text +=
                "Wi-Fi: \n" +
                //"wifiDataUploaded: " + wifiDataUploaded + "\n" +
                //"wifiDataDownloaded: " + wifiDataDownloaded + "\n" +
                //"wifiDownloadSpeed: " + wifiDownloadSpeed + "\n" +
                //"wifiUploadSpeed: " + wifiUploadSpeed + "\n" +
                //"wifiNetworkUtil: " + wifiNetworkUtilisation + "\n" +
                "Download: " + wifiBytesRecv + " MB/s\n" +
                "Upload: " + wifiBytesSent + " MB/s";

            //uint? leftFanRpm = DellSmbiosBzh.GetFanRpm(BzhFanIndex.Fan1);
            //uint? rightFanRpm = DellSmbiosBzh.GetFanRpm(BzhFanIndex.Fan2);
            //Console.WriteLine("left: " + leftFanRpm + " right: " + rightFanRpm);
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
    }

    #endregion

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

#region Polling func backup
/*
        private void polling_Tick(object sender, EventArgs e)
        {
            //CPU sensors
            state.CPU.Update();
            string cpuName = state.CPU.Name;
            float? cpuOneLoad = state.CPU.Sensors[0].Value;
            float? cpuTwoLoad = state.CPU.Sensors[1].Value;
            float? cpuThreeLoad = state.CPU.Sensors[2].Value;
            float? cpuFourLoad = state.CPU.Sensors[3].Value;
            float? cpuFiveLoad = state.CPU.Sensors[4].Value;
            float? cpuSixLoad = state.CPU.Sensors[5].Value;
            float? totalLoad = state.CPU.Sensors[6].Value;
            float? cpuOneTemp = state.CPU.Sensors[7].Value;
            float? cpuTwoTemp = state.CPU.Sensors[8].Value;
            float? cpuThreeTemp = state.CPU.Sensors[9].Value;
            float? cpuFourTemp = state.CPU.Sensors[10].Value;
            float? cpuFiveTemp = state.CPU.Sensors[11].Value;
            float? cpuSixTemp = state.CPU.Sensors[12].Value;
            float? cpuPackageTemp = state.CPU.Sensors[13].Value;
            //14 - 19 core distance to tj max
            //float? coreMax = state.CPU.Sensors[20].Value;
            //float? coreAverage = state.CPU.Sensors[21].Value;
            double? cpuOneClock = state.CPU.Sensors[22].Value / 1000d;
            double? cpuTwoClock = state.CPU.Sensors[23].Value / 1000d;
            double? cpuThreeClock = state.CPU.Sensors[24].Value / 1000d;
            double? cpuFourClock = state.CPU.Sensors[25].Value / 1000d;
            double? cpuFiveClock = state.CPU.Sensors[26].Value / 1000d;
            double? cpuSixClock = state.CPU.Sensors[27].Value / 1000d;
            float? cpuPackagePower = state.CPU.Sensors[28].Value;
            //float? cpuCoresPower = state.CPU.Sensors[29].Value;
            //float? cpuGraphicsPower = state.CPU.Sensors[30].Value;
            //float? cpuMemoryPower = state.CPU.Sensors[31].Value;
            //float? cpuBusSpeed = state.CPU.Sensors[32].Value;

            //GPU
            state.GPU.Update();
            string gpuName = state.GPU.Name;
            float? gpuTemp = state.GPU.Sensors[0].Value;
            double? gpuCoreClock = state.GPU.Sensors[1].Value / 1000d;
            double? gpuMemoryClock = state.GPU.Sensors[2].Value / 1000d;
            float? gpuCoreLoad = state.GPU.Sensors[3].Value;
            //float? gpuMemoryController = state.GPU.Sensors[4].Value;
            //Latest NVIDIA drivers apparently leave this setting out
            //float? gpuVideoEngineLoad = state.GPU.Sensors[5].Value;
            //float? gpuBusLoad = state.GPU.Sensors[6].Value;
            double? gpuTotalMemory = state.GPU.Sensors[7].Value / 1000d;
            double? gpuFreeMemory = state.GPU.Sensors[8].Value / 1000d;
            double? gpuMemoryUsed = state.GPU.Sensors[9].Value / 1000d;
            float? gpuPackagePower = state.GPU.Sensors[10].Value;
            //11, 12 GPU PCIe Rx/Tx

            //RAM
            state.RAM.Update();
            float? memoryUsed = state.RAM.Sensors[0].Value;
            float? memoryAvailable = state.RAM.Sensors[1].Value;
            float? memoryLoad = state.RAM.Sensors[2].Value;
            //float? virtualMemoryUsed = state.RAM.Sensors[3].Value;
            //float? virtualMemoryAvailable = state.RAM.Sensors[4].Value;
            //float? virtualMemoryLoad = state.RAM.Sensors[5].Value;

            //SSD
            state.SSD.Update();
            string ssdName = state.SSD.Name;
            float? ssdTemp = state.SSD.Sensors[0].Value;
            //float? ssdAvailableSpare = state.SSD.Sensors[1].Value;
            //float? ssdAvailableSpareThreshold = state.SSD.Sensors[2].Value;
            //float? ssdPercentUsed = state.SSD.Sensors[3].Value;
            //04, 05 data read/written
            //float? ssdTemp1 = state.SSD.Sensors[6].Value;
            //float? ssdUsedSpace = state.SSD.Sensors[7].Value;
            //float? ssdWriteActivity = state.SSD.Sensors[8].Value;
            //float? ssdTotalActivity = state.SSD.Sensors[9].Value;
            //float? ssdReadRate = state.SSD.Sensors[10].Value;
            //float? ssdWriteRate = state.SSD.Sensors[11].Value;

            double ssdFreeGB = state.DriveStates[0].Counters[0].NextValue() / 1024d;
            double ssdFreePercent = state.DriveStates[0].Counters[1].NextValue();
            double ssdUsedPercent = 100d - ssdFreePercent;
            double ssdTotalGB = ssdFreeGB / (ssdFreePercent / 100d);
            double ssdUsedGB = ssdTotalGB - ssdFreeGB;
            
            //HDD
            state.HDD.Update();
            string hddName = state.HDD.Name;
            float? hddTemp = state.HDD.Sensors[0].Value;
            //float? hddUsedSpace = state.HDD.Sensors[1].Value;
            //float? hddWriteActivity = state.HDD.Sensors[2].Value;
            //float? hddTotalActivity = state.HDD.Sensors[3].Value;
            //float? hddReadRate = state.HDD.Sensors[4].Value;
            //float? hddWriteRate = state.HDD.Sensors[5].Value;

            double hddFreeGB = state.DriveStates[1].Counters[0].NextValue() / 1024d;
            double hddFreePercent = state.DriveStates[1].Counters[1].NextValue();
            double hddUsedPercent = 100d - hddFreePercent;
            double hddTotalGB = hddFreeGB / (hddFreePercent / 100d);
            double hddUsedGB = hddTotalGB - hddFreeGB;

            //Ethernet
            state.Ethernet.Update();
            float? ethDataUploaded = state.WiFi.Sensors[0].Value;
            float? ethDataDownloaded = state.WiFi.Sensors[1].Value;
            float? ethDownloadSpeed = state.WiFi.Sensors[2].Value;
            float? ethUploadSpeed = state.WiFi.Sensors[3].Value;
            float? ethNetworkUtilisation = state.WiFi.Sensors[4].Value;
            float ethBytesRecv = state.NetworkStates[0].Counters[0].NextValue();
            float ethBytesSent = state.NetworkStates[0].Counters[1].NextValue();

            //Wi-Fi
            state.WiFi.Update();
            //float? wifiDataUploaded = state.WiFi.Sensors[0].Value;
            //float? wifiDataDownloaded = state.WiFi.Sensors[1].Value;
            //float? wifiDownloadSpeed = state.WiFi.Sensors[2].Value;
            //float? wifiUploadSpeed = state.WiFi.Sensors[3].Value;
            //float? wifiNetworkUtilisation = state.WiFi.Sensors[4].Value;
            double wifiBytesRecv = state.NetworkStates[1].Counters[0].NextValue() / 1048576d;
            double wifiBytesSent = state.NetworkStates[1].Counters[1].NextValue() / 1048576d;

            richTextBox1.Clear();

            richTextBox1.Text +=
                "cpuName: " + cpuName + "\n" +
                "cpuOneLoad: " + cpuOneLoad + " %\n" +
                "cpuTwoLoad: " + cpuTwoLoad + " %\n" +
                "cpuThreeLoad: " + cpuThreeLoad + " %\n" +
                "cpuFourLoad: " + cpuFourLoad + " %\n" +
                "cpuFiveLoad: " + cpuFiveLoad + " %\n" +
                "cpuSixLoad: " + cpuSixLoad + " %\n" +
                "totalLoad: " + totalLoad + " %\n" +
                "cpuOneTemp: " + cpuOneTemp + " °C\n" +
                "cpuTwoTemp: " + cpuTwoTemp + " °C\n" +
                "cpuThreeTemp: " + cpuThreeTemp + " °C\n" +
                "cpuFourTemp: " + cpuFourTemp + " °C\n" +
                "cpuFiveTemp: " + cpuFiveTemp + " °C\n" +
                "cpuSixTemp: " + cpuSixTemp + " °C\n" +
                "cpuPackageTemp: " + cpuPackageTemp + " °C\n" +
                //"coreMax: " + coreMax + "\n" +
                //"coreAverage: " + coreAverage + "\n" +
                "cpuOneClock: " + cpuOneClock + " GHz\n" +
                "cpuTwoClock: " + cpuTwoClock + " GHz\n" +
                "cpuThreeClock: " + cpuThreeClock + " GHz\n" +
                "cpuFourClock: " + cpuFourClock + " GHz\n" +
                "cpuFiveClock: " + cpuFiveClock + " GHz\n" +
                "cpuSixClock: " + cpuSixClock + " GHz\n" +
                "cpuPackagePower: " + cpuPackagePower + " W\n\n";
                //"cpuCoresPower: " + cpuCoresPower + "\n" +
                //"cpuGraphicsPower: " + cpuGraphicsPower + "\n" +
                //"cpuMemoryPower: " + cpuMemoryPower + "\n" +
                //"cpuBusSpeed: " + cpuBusSpeed + "\n\n";

            richTextBox1.Text +=
                "gpuName: " + gpuName + "\n" +
                "gpuTemp: " + gpuTemp + " °C\n" +
                "gpuCoreClock: " + gpuCoreClock + " GHz\n" +
                "gpuMemoryClock: " + gpuMemoryClock + " GHz\n" +
                "gpuCoreLoad: " + gpuCoreLoad + " %\n" +
                //"gpuMemoryController: " + gpuMemoryController + "\n" +
                //"gpuVideoEngineLoad: " + gpuVideoEngineLoad + " %\n" +
                //"gpuBusLoad: " + gpuBusLoad + "\n" +
                "gpuTotalMemory: " + gpuTotalMemory + " GB\n" +
                "gpuFreeMemory: " + gpuFreeMemory + " GB\n" +
                "gpuMemoryUsed: " + gpuMemoryUsed + " GB\n" +
                "gpuPackagePower: " + gpuPackagePower + " W\n\n";

            richTextBox1.Text +=
                "RAM \n" +
                "memoryTotal: " + (float)(memoryUsed + memoryAvailable) + " GB\n" +
                "memoryUsed: " + memoryUsed + " GB\n" +
                "memoryAvailable: " + memoryAvailable + " GB\n" +
                "memoryLoad: " + memoryLoad + " %\n\n";
                //"virtualMemoryUsed: " + virtualMemoryUsed + "\n" +
                //"virtualMemoryAvailable: " + virtualMemoryAvailable + "\n" +
                //"virtualMemory: " + virtualMemoryLoad + "\n\n";

            richTextBox1.Text +=
                "ssdName: " + ssdName + "\n" +
                "ssdTemp: " + ssdTemp + "\n" +
                //"ssdAvailableSpare: " + ssdAvailableSpare + "\n" +
                //"ssdAvailableSpareThreshold: " + ssdAvailableSpareThreshold + "\n" +
                //"ssdPercentUsed: " + ssdPercentUsed + "\n" +
                //"ssdTemp1: " + ssdTemp1 + "\n" +
                //"ssdUsedSpace: " + ssdUsedSpace + "\n" +
                //"ssdWriteActivity: " + ssdWriteActivity + "\n" +
                //"ssdTotalActivity: " + ssdTotalActivity + "\n" +
                //"ssdReadRate: " + ssdReadRate + "\n" +
                //"ssdWriteRate: " + ssdWriteRate + "\n" +
                "ssdTotalGB: " + ssdTotalGB + " GB\n" +
                "ssdFreeGB: " + ssdFreeGB + " GB\n" +
                "ssdUsedGB: " + ssdUsedGB + " GB\n" +
                "ssdFreePercent: " + ssdFreePercent + " %\n" +
                "ssdUsedPercent: " + ssdUsedPercent + " %\n\n";

            richTextBox1.Text +=
                "hddName: " + hddName + "\n" +
                "hddTemp: " + hddTemp + "\n" +
                //"hddUsedSpace: " + hddUsedSpace + "\n" +
                //"hddWriteActivity: " + hddWriteActivity + "\n" +
                //"hddTotalActivity: " + hddTotalActivity + "\n" +
                //"hddReadRate: " + hddReadRate + "\n" +
                //"hddWriteRate: " + hddWriteRate + "\n" +
                "hddTotalGB: " + hddTotalGB + " GB\n" +
                "hddUsedGB: " + hddUsedGB + " GB\n" +
                "hddFreeGB: " + hddFreeGB + " GB\n" +
                "hddFreePercent: " + hddFreePercent + " %\n" +
                "hddUsedPercent: " + hddUsedPercent + " %\n\n";

            richTextBox1.Text +=
                "Wi-Fi: \n" +
                //"wifiDataUploaded: " + wifiDataUploaded + "\n" +
                //"wifiDataDownloaded: " + wifiDataDownloaded + "\n" +
                //"wifiDownloadSpeed: " + wifiDownloadSpeed + "\n" +
                //"wifiUploadSpeed: " + wifiUploadSpeed + "\n" +
                //"wifiNetworkUtil: " + wifiNetworkUtilisation + "\n" +
                "Download: " + wifiBytesRecv + " MB/s\n" +
                "Upload: " + wifiBytesSent + " MB/s";

            //uint? leftFanRpm = DellSmbiosBzh.GetFanRpm(BzhFanIndex.Fan1);
            //uint? rightFanRpm = DellSmbiosBzh.GetFanRpm(BzhFanIndex.Fan2);
            //Console.WriteLine("left: " + leftFanRpm + " right: " + rightFanRpm);
        }
*/
#endregion