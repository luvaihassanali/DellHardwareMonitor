using System;
using System.Drawing;
using System.Windows.Forms;

using DellFanManagement.DellSmbiozBzhLib;

namespace DellHardwareMonitor
{
    public partial class DellHardwareMonitorForm : Form
    {
        //Wiki: Polling is the process where the computer or controlling device waits for an external device to check for its readiness or state
        private Timer pollingTimer = new Timer();
        private Boolean isDriverLoaded;
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        private HardwareState state;

        public DellHardwareMonitorForm()
        {
            InitializeComponent();

            //Visible in Task Manager when you expand process under Apps if not set shows default Form icon
            this.Icon = Properties.Resources.wrench;

            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Info", OnInfo);
            trayMenu.MenuItems.Add("Exit", OnExit);

            trayIcon = new NotifyIcon();
            trayIcon.Click += new EventHandler(trayIcon_Click);
            trayIcon.Text = "DellHardwareMonitor";
            trayIcon.Icon = new Icon("wrench.ico");
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;

            isDriverLoaded = false; //LoadDriver();

            /*if(!isDriverLoaded)
            {
                MessageBox.Show("Failed to load DellSmbiosBzhLib driver.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                System.Environment.Exit(1);
            }*/

            //state = new HardwareState();

            pollingTimer.Tick += new EventHandler(polling_Tick);
            pollingTimer.Interval = 2000;
            pollingTimer.Enabled = true;
            pollingTimer.Start();
        }

        #region Form functions

        protected override void OnLoad(EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            Visible = false;
            ShowInTaskbar = false;
            base.OnLoad(e);
        }

        private void trayIcon_Click(object sender, EventArgs e)
        {
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
            if (isDriverLoaded)
            {
                UnloadDriver();
            }

            if (state.Computer != null)
            {
                state.Computer.Close();
            }

            trayIcon.Visible = false;
            trayIcon.Dispose();

            Application.Exit();
            System.Environment.Exit(1);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isDriverLoaded)
            {
                UnloadDriver();
            }

            if (state.Computer != null)
            {
                state.Computer.Close();
            }

            trayIcon.Visible = false;
            trayIcon.Dispose();
        }

        #endregion

        #region Monitor functions

        private void polling_Tick(object sender, EventArgs e)
        {
            //CPU sensors
            state.CPU.Update();
            string cpuName = state.CPU.Name;
            float? cpuOneLoad = state.CPU.Sensors[7].Value;
            float? cpuTwoLoad = state.CPU.Sensors[8].Value;
            float? cpuThreeLoad = state.CPU.Sensors[9].Value;
            float? cpuFourLoad = state.CPU.Sensors[10].Value;
            float? cpuFiveLoad = state.CPU.Sensors[11].Value;
            float? cpuSixLoad = state.CPU.Sensors[12].Value;
            float? totalLoad = state.CPU.Sensors[6].Value;
            float? cpuOneTemp = state.CPU.Sensors[7].Value;
            float? cpuTwoTemp = state.CPU.Sensors[8].Value;
            float? cpuThreeTemp = state.CPU.Sensors[9].Value;
            float? cpuFourTemp = state.CPU.Sensors[10].Value;
            float? cpuFiveTemp = state.CPU.Sensors[11].Value;
            float? cpuSixTemp = state.CPU.Sensors[12].Value;
            float? cpuPackageTemp = state.CPU.Sensors[13].Value;
            //14 - 19 core distance to tj max
            float? coreMax = state.CPU.Sensors[20].Value;
            float? coreAverage = state.CPU.Sensors[21].Value;
            //22 - 27 cpu core clocks: first clock? or all clock...
            float? cpuPackagePower = state.CPU.Sensors[28].Value;
            float? cpuCoresPower = state.CPU.Sensors[29].Value;
            float? cpuGraphicsPower = state.CPU.Sensors[30].Value;
            float? cpuMemoryPower = state.CPU.Sensors[31].Value;
            float? cpuBusSpeed = state.CPU.Sensors[32].Value;

            //GPU
            state.GPU.Update();
            string gpuName = state.GPU.Name;
            float? gpuTemp = state.GPU.Sensors[0].Value;
            float? gpuCoreClock = state.GPU.Sensors[1].Value;
            float? gpuMemory = state.GPU.Sensors[2].Value;
            float? gpuCoreLoad = state.GPU.Sensors[3].Value;
            float? gpuMemoryController = state.GPU.Sensors[4].Value;
            float? gpuVideoEngineLoad = state.GPU.Sensors[5].Value;
            float? gpuBusLoad = state.GPU.Sensors[6].Value;
            float? gpuTotalMemory = state.GPU.Sensors[7].Value;
            float? gpuFreeMemory = state.GPU.Sensors[8].Value;
            float? gpuMemoryUsed = state.GPU.Sensors[9].Value;
            float? gpuPackagePower = state.GPU.Sensors[10].Value;
            //11, 12 GPU PCIe Rx/Tx

            //RAM
            state.RAM.Update();
            float? memoryUsed = state.RAM.Sensors[0].Value;
            float? memoryAvailable = state.RAM.Sensors[1].Value;
            float? genericMemory = state.RAM.Sensors[2].Value;
            float? virtualMemoryUsed = state.RAM.Sensors[3].Value;
            float? virtualMemoryAvailable = state.RAM.Sensors[4].Value;
            float? virtualMemory = state.RAM.Sensors[5].Value;
            
            //SSD
            state.SSD.Update();
            string ssdName = state.SSD.Name;
            float? ssdTemp = state.SSD.Sensors[0].Value;
            float? ssdAvailableSpare = state.SSD.Sensors[1].Value;
            float? ssdAvailableSpareThreshold = state.SSD.Sensors[2].Value;
            float? ssdPercentUsed = state.SSD.Sensors[3].Value;
            //04, 05 data read/written
            float? ssdTemp1 = state.SSD.Sensors[6].Value;
            float? ssdUsedSpace = state.SSD.Sensors[7].Value;
            float? ssdWriteActivity = state.SSD.Sensors[8].Value;
            float? ssdTotalActivity = state.SSD.Sensors[9].Value;
            float? ssdReadRate = state.SSD.Sensors[10].Value;
            float? ssdWriteRate = state.SSD.Sensors[11].Value;

            double ssdFreeGB = state.DriveStates[0].Counters[0].NextValue() / 1024d;
            double ssdFreePercent = state.DriveStates[0].Counters[1].NextValue();
            double ssdUsedPercent = 100d - ssdFreePercent;
            double ssdTotalGB = ssdFreeGB / (ssdFreePercent / 100d);
            double ssdUsedGB = ssdTotalGB - ssdFreeGB;

            //HDD
            state.HDD.Update();
            string hddName = state.HDD.Name;
            float? hddTemp = state.HDD.Sensors[0].Value;
            float? hddUsedSpace = state.HDD.Sensors[1].Value;
            float? hddWriteActivity = state.HDD.Sensors[2].Value;
            float? hddTotalActivity = state.HDD.Sensors[3].Value;
            float? hddReadRate = state.HDD.Sensors[4].Value;
            float? hddWriteRate = state.HDD.Sensors[5].Value;

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
            float? wifiDataUploaded = state.WiFi.Sensors[0].Value;
            float? wifiDataDownloaded = state.WiFi.Sensors[1].Value;
            float? wifiDownloadSpeed = state.WiFi.Sensors[2].Value;
            float? wifiUploadSpeed = state.WiFi.Sensors[3].Value;
            float? wifiNetworkUtilisation = state.WiFi.Sensors[4].Value;
            float wifiBytesRecv = state.NetworkStates[1].Counters[0].NextValue();
            float wifiBytesSent = state.NetworkStates[1].Counters[1].NextValue();

            uint? leftFanRpm = DellSmbiosBzh.GetFanRpm(BzhFanIndex.Fan1);
            uint? rightFanRpm = DellSmbiosBzh.GetFanRpm(BzhFanIndex.Fan2);
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

//copy of polling function
/*
private void polling_Tick(object sender, EventArgs e)
{
    //CPU sensors
    state.CPU.Update();
    string cpuName = state.CPU.Name;
    float? cpuOneLoad = state.CPU.Sensors[7].Value;
    float? cpuTwoLoad = state.CPU.Sensors[8].Value;
    float? cpuThreeLoad = state.CPU.Sensors[9].Value;
    float? cpuFourLoad = state.CPU.Sensors[10].Value;
    float? cpuFiveLoad = state.CPU.Sensors[11].Value;
    float? cpuSixLoad = state.CPU.Sensors[12].Value;
    float? totalLoad = state.CPU.Sensors[6].Value;
    float? cpuOneTemp = state.CPU.Sensors[7].Value;
    float? cpuTwoTemp = state.CPU.Sensors[8].Value;
    float? cpuThreeTemp = state.CPU.Sensors[9].Value;
    float? cpuFourTemp = state.CPU.Sensors[10].Value;
    float? cpuFiveTemp = state.CPU.Sensors[11].Value;
    float? cpuSixTemp = state.CPU.Sensors[12].Value;
    float? cpuPackageTemp = state.CPU.Sensors[13].Value;
    //14 - 19 core distance to tj max
    float? coreMax = state.CPU.Sensors[20].Value;
    float? coreAverage = state.CPU.Sensors[21].Value;
    //22 - 27 cpu core clocks: first clock? or all clock...
    float? cpuPackagePower = state.CPU.Sensors[28].Value;
    float? cpuCoresPower = state.CPU.Sensors[29].Value;
    float? cpuGraphicsPower = state.CPU.Sensors[30].Value;
    float? cpuMemoryPower = state.CPU.Sensors[31].Value;
    float? cpuBusSpeed = state.CPU.Sensors[32].Value;

    //GPU
    state.GPU.Update();
    string gpuName = state.GPU.Name;
    float? gpuTemp = state.GPU.Sensors[0].Value;
    float? gpuCoreClock = state.GPU.Sensors[1].Value;
    float? gpuMemory = state.GPU.Sensors[2].Value;
    float? gpuCoreLoad = state.GPU.Sensors[3].Value;
    float? gpuMemoryController = state.GPU.Sensors[4].Value;
    float? gpuVideoEngineLoad = state.GPU.Sensors[5].Value;
    float? gpuBusLoad = state.GPU.Sensors[6].Value;
    float? gpuTotalMemory = state.GPU.Sensors[7].Value;
    float? gpuFreeMemory = state.GPU.Sensors[8].Value;
    float? gpuMemoryUsed = state.GPU.Sensors[9].Value;
    float? gpuPackagePower = state.GPU.Sensors[10].Value;
    //11, 12 GPU PCIe Rx/Tx

    //RAM
    state.RAM.Update();
    float? memoryUsed = state.RAM.Sensors[0].Value;
    float? memoryAvailable = state.RAM.Sensors[1].Value;
    float? genericMemory = state.RAM.Sensors[2].Value;
    float? virtualMemoryUsed = state.RAM.Sensors[3].Value;
    float? virtualMemoryAvailable = state.RAM.Sensors[4].Value;
    float? virtualMemory = state.RAM.Sensors[5].Value;

    //SSD
    state.SSD.Update();
    string ssdName = state.SSD.Name;
    float? ssdTemp = state.SSD.Sensors[0].Value;
    float? ssdAvailableSpare = state.SSD.Sensors[1].Value;
    float? ssdAvailableSpareThreshold = state.SSD.Sensors[2].Value;
    float? ssdPercentUsed = state.SSD.Sensors[3].Value;
    //04, 05 data read/written
    float? ssdTemp1 = state.SSD.Sensors[6].Value;
    float? ssdUsedSpace = state.SSD.Sensors[7].Value;
    float? ssdWriteActivity = state.SSD.Sensors[8].Value;
    float? ssdTotalActivity = state.SSD.Sensors[9].Value;
    float? ssdReadRate = state.SSD.Sensors[10].Value;
    float? ssdWriteRate = state.SSD.Sensors[11].Value;

    double ssdFreeGB = state.DriveStates[0].Counters[0].NextValue() / 1024d;
    double ssdFreePercent = state.DriveStates[0].Counters[1].NextValue();
    double ssdUsedPercent = 100d - ssdFreePercent;
    double ssdTotalGB = ssdFreeGB / (ssdFreePercent / 100d);
    double ssdUsedGB = ssdTotalGB - ssdFreeGB;

    //HDD
    state.HDD.Update();
    string hddName = state.HDD.Name;
    float? hddTemp = state.HDD.Sensors[0].Value;
    float? hddUsedSpace = state.HDD.Sensors[1].Value;
    float? hddWriteActivity = state.HDD.Sensors[2].Value;
    float? hddTotalActivity = state.HDD.Sensors[3].Value;
    float? hddReadRate = state.HDD.Sensors[4].Value;
    float? hddWriteRate = state.HDD.Sensors[5].Value;

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
    float? wifiDataUploaded = state.WiFi.Sensors[0].Value;
    float? wifiDataDownloaded = state.WiFi.Sensors[1].Value;
    float? wifiDownloadSpeed = state.WiFi.Sensors[2].Value;
    float? wifiUploadSpeed = state.WiFi.Sensors[3].Value;
    float? wifiNetworkUtilisation = state.WiFi.Sensors[4].Value;
    float wifiBytesRecv = state.NetworkStates[1].Counters[0].NextValue();
    float wifiBytesSent = state.NetworkStates[1].Counters[1].NextValue();

    uint? leftFanRpm = DellSmbiosBzh.GetFanRpm(BzhFanIndex.Fan1);
    uint? rightFanRpm = DellSmbiosBzh.GetFanRpm(BzhFanIndex.Fan2);
    //Console.WriteLine("left: " + leftFanRpm + " right: " + rightFanRpm);
}
*/ 
