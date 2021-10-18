using System;
using System.Diagnostics;
using System.Windows.Forms;
using LibreHardwareMonitor.Hardware;
using DellFanManagement.DellSmbiozBzhLib;
using System.Drawing;

namespace DellHardwareMonitor
{
    public partial class DellHardwareMonitorForm : Form
    {
        private Timer timer = new Timer();
        private Boolean isDriverLoaded;
        private Boolean isVisible;
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        
        private uint? leftFanRpm;
        private uint? rightFanRpm;

        public DellHardwareMonitorForm()
        {
            InitializeComponent();
            
            //Visible in Task Manager when you expand process under Apps
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

            isVisible = true;
            isDriverLoaded = LoadDriver();

            if(!isDriverLoaded)
            {
                MessageBox.Show("Failed to load DellSmbiosBzhLib driver.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                System.Environment.Exit(1);
            }

            timer.Tick += new EventHandler(timer_Tick); 
            timer.Interval = 2000;              
            timer.Enabled = true;                       
            timer.Start();

            //TestLibreHardwareMonitor();
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;
            base.OnLoad(e);
        }

        private void trayIcon_Click(object sender, EventArgs e)
        {
            if (isVisible)
            {
                Visible = false;
                ShowInTaskbar = false;
                isVisible = false;
                return;
            }
            else
            {
                Visible = true;
                ShowInTaskbar = false;
                isVisible = true;
                this.WindowState = FormWindowState.Minimized;
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void OnExit(object sender, EventArgs e)
        {
            if (isDriverLoaded)
            {
                UnloadDriver();
            }

            trayIcon.Visible = false;
            trayIcon.Dispose();

            Application.Exit();
            System.Environment.Exit(1);
        }

        private void OnInfo(object sender, EventArgs e)
        {
            MessageBox.Show("Info line 1 \nInfo line 2\nInfo line 3");
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            leftFanRpm = DellSmbiosBzh.GetFanRpm(BzhFanIndex.Fan1);
            rightFanRpm = DellSmbiosBzh.GetFanRpm(BzhFanIndex.Fan2);
            Console.WriteLine("left: " + leftFanRpm + " right: " + rightFanRpm);
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
        private void TestLibreHardwareMonitor()
        {
            Computer computer = new Computer
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true,
                IsMemoryEnabled = true,
                IsMotherboardEnabled = true,
                IsControllerEnabled = true,
                IsNetworkEnabled = true,
                IsStorageEnabled = true
            };

            computer.Open();
            computer.Accept(new UpdateVisitor());

            foreach (IHardware hardware in computer.Hardware)
            {
                Debug.WriteLine("Hardware: {0}", hardware.Name);

                foreach (IHardware subhardware in hardware.SubHardware)
                {
                    Debug.WriteLine("\tSubhardware: {0}", subhardware.Name);

                    foreach (ISensor sensor in subhardware.Sensors)
                    {
                        Debug.WriteLine("\t\tSensor: {0}, value: {1}", sensor.Name, sensor.Value);
                    }
                }

                foreach (ISensor sensor in hardware.Sensors)
                {
                    Debug.WriteLine("\tSensor: {0}, value: {1}", sensor.Name, sensor.Value);
                }
            }

            computer.Close();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if(isDriverLoaded)
            {
                UnloadDriver();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isDriverLoaded)
            {
                UnloadDriver();
            }
        }
    }

    public class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }
        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
        }
        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }
    }
}
