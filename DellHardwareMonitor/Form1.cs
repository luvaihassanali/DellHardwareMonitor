using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Management.Automation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using DellFanManagement.DellSmbiozBzhLib;

namespace DellHardwareMonitor
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime(ref SYSTEMTIME st);
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        private double opacity;
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

        private string iconSet;
        private string dateString;
        private string cpuName = ConfigurationManager.AppSettings["cpuName"];
        private string gpuName = ConfigurationManager.AppSettings["gpuName"];
        private string ssdName = ConfigurationManager.AppSettings["ssdName"];
        private string hddName = ConfigurationManager.AppSettings["hddName"];

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

            /*
            MenuItem ideaMenuItem = new MenuItem();
            ideaMenuItem.Text = "  Idea";
            ideaMenuItem.Click += new EventHandler(OnIdea);
            ideaMenuItem.OwnerDraw = true;
            ideaMenuItem.DrawItem += new DrawItemEventHandler(DrawIdeaMenuItem);
            ideaMenuItem.MeasureItem += new MeasureItemEventHandler(MeasureIdeaMenuItem);

            MenuItem saveMenuItem = new MenuItem();
            saveMenuItem.Text = "  Save";
            saveMenuItem.Click += new EventHandler(OnSave);
            saveMenuItem.OwnerDraw = true;
            saveMenuItem.DrawItem += new DrawItemEventHandler(DrawSaveMenuItem);
            saveMenuItem.MeasureItem += new MeasureItemEventHandler(MeasureSaveMenuItem);

            MenuItem exitNoSaveMenuItem = new MenuItem();
            exitNoSaveMenuItem.Text = "  Kill";
            exitNoSaveMenuItem.Click += new EventHandler(OnExitNoSave);
            exitNoSaveMenuItem.OwnerDraw = true;
            exitNoSaveMenuItem.DrawItem += new DrawItemEventHandler(DrawExitNoSaveMenuItem);
            exitNoSaveMenuItem.MeasureItem += new MeasureItemEventHandler(MeasureExitNoSaveMenuItem);

            MenuItem exitMenuItem = new MenuItem();
            exitMenuItem.Text = "  Exit";
            exitMenuItem.Click += new EventHandler(OnExit);
            exitMenuItem.OwnerDraw = true;
            exitMenuItem.DrawItem += new DrawItemEventHandler(DrawExitMenuItem);
            exitMenuItem.MeasureItem += new MeasureItemEventHandler(MeasureExitMenuItem);

            MenuItem shutdownMenuItem = new MenuItem();
            shutdownMenuItem.Text = "  Shutdown";
            shutdownMenuItem.Click += new EventHandler(OnShutdown);
            shutdownMenuItem.OwnerDraw = true;
            shutdownMenuItem.DrawItem += new DrawItemEventHandler(DrawShutdownMenuItem);
            shutdownMenuItem.MeasureItem += new MeasureItemEventHandler(MeasureShutdownMenuItem);

            trayMenu.MenuItems.AddRange(new MenuItem[]
            {
                ideaMenuItem, exitNoSaveMenuItem, saveMenuItem,  exitMenuItem, shutdownMenuItem
            });
            */

            //trayMenu.MenuItems.Add("Icon set");
            //trayMenu.MenuItems[0].MenuItems.Add("Default", OnIconSet);
            //trayMenu.MenuItems.Add("-");
            trayMenu.MenuItems.Add("Fan control high", FanControl);
            trayMenu.MenuItems.Add("Fan control low", FanControlLow);
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
            singleClickTimer = new System.Windows.Forms.Timer();
            //singleClickTimer.Interval = (int)(SystemInformation.DoubleClickTime / 2); // is 100 ms
            singleClickTimer.Tick += SingleClickTimer_Tick;

            form2 = new Form();
            form2.FormBorderStyle = FormBorderStyle.None;
            form2.StartPosition = FormStartPosition.Manual;
            form2.BackColor = Color.Black;
            form2.ShowInTaskbar = false;
            typeof(Form).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, form2, new object[] { true });
            form2.MouseClick += Form2_MouseClick;
        }

        #region General form functions

        private void Form1_Load(object sender, EventArgs e)
        {
            Properties.Settings.Default.Opacity = 0;
            if (Properties.Settings.Default.Opacity == 0)
            {
                Rectangle screenBounds = Screen.FromControl(this).Bounds;
                this.Size = new Size(309, screenBounds.Height - 45);
                this.Location = new Point(screenBounds.Width - this.Size.Width - 10, 8);
                opacity = 0.8;
            }
            else
            {
                this.Location = Properties.Settings.Default.WindowLocation;
                this.Size = Properties.Settings.Default.WindowSize;
                opacity = Properties.Settings.Default.Opacity;
            }

            if(Properties.Settings.Default.IconSet.Equals("Default"))
            {
                iconSet = "Default";
            }
            OnIconSet(null, null);

            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 20, 20));
            form2.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 20, 20)); //20
            form2.Location = new Point(this.Location.X, this.Location.Y);
            form2.Size = this.Size;
            loadingPictureBox.Location = new System.Drawing.Point(this.Width / 2 - loadingPictureBox.Width / 2, this.Height / 2 - loadingPictureBox.Height / 2);

            isDriverLoaded = LoadDriver();
            if (!isDriverLoaded)
            {
                if (System.IO.File.Exists("bzh_dell_smm_io_x64.sys"))
                {
                    MessageBox.Show("Failed to load DellSmbiosBzhLib driver. Verify administrator priveleges.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Failed to load DellSmbiosBzhLib driver. Verify that bzh_dell_smm_io_x64.sys is in application directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Application.Exit();
                System.Environment.Exit(1);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Fader.FadeInCustom(form2, Fader.FadeSpeed.FourSlow, opacity);
            Fader.FadeIn(this, Fader.FadeSpeed.FourSlow);
            form2.Activate();
            this.Activate();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (systemShutdown)
                CleanUp();

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
            if (e.Button == MouseButtons.Left)
                singleClickTimer.Start();
            
            //To-do: save location for minimize
            if (e != null && e.Button == MouseButtons.Right)
                return;
        }

        #region Tray icon functions

        private void TrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                singleClickTimer.Stop();
                form2.Activate();
                this.Activate();
            }
        }

        private void SingleClickTimer_Tick(object sender, EventArgs e)
        {
            singleClickTimer.Stop();
            Point initPos = this.Location;
            if (this.Location.Y == 10)
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
                if (backgroundWorkerCompleted)
                    pollingTimer.Start();

                ShowInTaskbar = false;
                Visible = true;
                form2.ShowInTaskbar = false;
                form2.Visible = true;

                form2.Activate();
                this.Activate();

                for (int i = 1250; i >= 10; i -= 2)
                {
                    this.Location = new Point(initPos.X, i);
                    form2.Location = new Point(initPos.X, i);
                }
            }
        }

        #endregion 

        private void CleanUp()
        {
            if (backgroundWorker1.IsBusy)
                backgroundWorker1.CancelAsync();
            //To-do: save window loc/size on minimize
            if (this.Location.Y == 10)
            {
                Properties.Settings.Default.WindowLocation = this.Location;
                Properties.Settings.Default.WindowSize = this.Size;
            }
            Properties.Settings.Default.IconSet = iconSet;
            Properties.Settings.Default.Opacity = form2.Opacity;
            Properties.Settings.Default.Save();

            pollingTimer.Stop();
            pollingTimer.Dispose();

            if (isDriverLoaded)
                UnloadDriver();

            if (state != null && state.Computer != null)
                state.Computer.Close();
        }

        protected override void WndProc(ref Message message)
        {
            if (message.Msg == 0x11) //WM_QUERYENDSESSION
                systemShutdown = true;

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

        #region Context menu

        private void OnIconSet(object sender, EventArgs e)
        {
            if (sender != null)
            {
                MenuItem menuItem = sender as MenuItem;
                iconSet = menuItem.Text;
            } else
            {
                iconSet = Properties.Settings.Default.IconSet;
            }

            if (iconSet.Equals("Default"))
            {
                cpuPictureBox.Image = Properties.Resources.default_processor;
                gpuPictureBox.Image = Properties.Resources.default_graphics;
                ramPictureBox.Image = Properties.Resources.default_ram;
                fanPictureBox.Image = Properties.Resources.default_fan;
                ssdPictureBox.Image = Properties.Resources.default_ssd;
                hddPictureBox.Image = Properties.Resources.default_hdd;
                wifiPictureBox.Image = Properties.Resources.default_router;
                iconSet = "Default";
            } else if (iconSet.Equals("Black"))
            {

            }
            this.Refresh();
        }

        private void OnShow(object sender, EventArgs e)
        {
            form2.Activate();
            this.Activate();

            if (this.Location.Y != 10)
            {
                pollingTimer.Start();

                form2.Activate();
                this.Activate();

                Point initPos = this.Location;

                for (int i = 1250; i >= 10; i -= 2)
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
            System.Environment.Exit(0);
        }

        private void ResetOrientation(object sender, EventArgs e)
        {
            Rectangle screenBounds = Screen.FromControl(this).Bounds;
            this.Size = new Size(309, screenBounds.Height - 45);
            this.Location = new Point(screenBounds.Width - this.Size.Width - 10, 8);
            form2.Size = new Size(309, screenBounds.Height - 45);
            form2.Location = new Point(screenBounds.Width - this.Size.Width - 10, 8);
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

        #region Fan control 

        private void FanControlLow(object sender, EventArgs e)
        {
            bool fanOneResult = true;
            bool fanTwoResult = true;

            if (fanControl)
            {
                trayIcon.Icon = Properties.Resources.wrench_yellow;
                //trayMenu.MenuItems[3].Checked = true;
                //trayMenu.MenuItems[2].Checked = false;
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
                trayIcon.Icon = Properties.Resources.wrench;
                fanControlLow = false;
                //trayMenu.MenuItems[3].Checked = false;

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
                //trayMenu.MenuItems[3].Checked = true;

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
            bool fanOneResult = true;
            bool fanTwoResult = true;

            if (fanControlLow)
            {
                trayIcon.Icon = Properties.Resources.wrench_red;
                //trayMenu.MenuItems[3].Checked = false;
                //trayMenu.MenuItems[2].Checked = true;
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
                //trayMenu.MenuItems[2].Checked = false;

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
                //trayMenu.MenuItems[2].Checked = true;

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

        #endregion

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
                        string ipString = ipAddr.ToString();
                        if (ipString.Contains("192.168"))
                        {
                            localhost.Text = ipString;
                            break;
                        }
                    }
                }

                wifiHeaderLbl.Text = GetSSID();

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
                gpuNameLbl.Text = state.GPU.Name.Replace("NVIDIA", "NVIDIA ");
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
            //ramAvailableLbl.Text = memoryAvailable.ToString("0.00");
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
            foreach (Control c in this.Controls)
            {
                c.Visible = true;
            }

            //Fader.FadeOutCustom(form2, Fader.FadeSpeed.ThreeSlow, null, opacity);
            loadingPictureBox.Dispose();
            uploadPictureBox.Visible = false;
            downloadPictureBox.Visible = false;
            backgroundWorkerCompleted = true;
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
            System.Diagnostics.Process.Start("regedit.exe");
            label1.Focus();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("compmgmt.msc");
            label1.Focus();
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
        const int inset = 2; // A single inset value to control teh sizing of the inner rect.

        using (Image offscreenImage = new Bitmap(this.Width, this.Height))
        {
            using (Graphics offscreen = Graphics.FromImage(offscreenImage))
            {
                Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

                if (ProgressBarRenderer.IsSupported)
                    ProgressBarRenderer.DrawHorizontalBar(offscreen, rect);

                rect.Inflate(new Size(-inset, -inset)); // Deflate inner rect.
                rect.Width = (int)(rect.Width * ((double)this.Value / this.Maximum));
                if (rect.Width == 0) rect.Width = 1; // Can't draw rec with width of 0.

                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(rect, this.BackColor, this.ForeColor, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                offscreen.FillRectangle(brush, inset, inset, rect.Width, rect.Height);

                e.Graphics.DrawImage(offscreenImage, 0, 0);
            }
        }
    }
}

public class RoundButton : Button
{
    //https://stackoverflow.com/questions/3708113/round-shaped-buttons
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        System.Drawing.Drawing2D.GraphicsPath grPath = new System.Drawing.Drawing2D.GraphicsPath();
        grPath.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
        this.Region = new System.Drawing.Region(grPath);
        base.OnPaint(e);
    }
}