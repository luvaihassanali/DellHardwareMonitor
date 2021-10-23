
namespace DellHardwareMonitor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.cpuPictureBox = new System.Windows.Forms.PictureBox();
            this.gpuPictureBox = new System.Windows.Forms.PictureBox();
            this.ramPictureBox = new System.Windows.Forms.PictureBox();
            this.ssdPictureBox = new System.Windows.Forms.PictureBox();
            this.hddPictureBox = new System.Windows.Forms.PictureBox();
            this.wifiPictureBox = new System.Windows.Forms.PictureBox();
            this.cpuNameLbl = new System.Windows.Forms.Label();
            this.cpu1LoadLbl = new System.Windows.Forms.Label();
            this.cpu2LoadLbl = new System.Windows.Forms.Label();
            this.cpu3LoadLbl = new System.Windows.Forms.Label();
            this.cpu4LoadLbl = new System.Windows.Forms.Label();
            this.cpu5LoadLbl = new System.Windows.Forms.Label();
            this.cpu6LoadLbl = new System.Windows.Forms.Label();
            this.cpu6TempLbl = new System.Windows.Forms.Label();
            this.cpu5TempLbl = new System.Windows.Forms.Label();
            this.cpu4TempLbl = new System.Windows.Forms.Label();
            this.cpu3TempLbl = new System.Windows.Forms.Label();
            this.cpu2TempLbl = new System.Windows.Forms.Label();
            this.cpu1TempLbl = new System.Windows.Forms.Label();
            this.cpuTempHeaderLbl = new System.Windows.Forms.Label();
            this.cpuLoadHeaderLbl = new System.Windows.Forms.Label();
            this.cpuPackageTempLbl = new System.Windows.Forms.Label();
            this.cpuTotalLoadLbl = new System.Windows.Forms.Label();
            this.cpuPackagePwrLbl = new System.Windows.Forms.Label();
            this.cpu6ClockLbl = new System.Windows.Forms.Label();
            this.cpu5ClockLbl = new System.Windows.Forms.Label();
            this.cpu4ClockLbl = new System.Windows.Forms.Label();
            this.cpu3ClockLbl = new System.Windows.Forms.Label();
            this.cpu2ClockLbl = new System.Windows.Forms.Label();
            this.cpu1ClockLbl = new System.Windows.Forms.Label();
            this.gpuName = new System.Windows.Forms.Label();
            this.gpuTempLbl = new System.Windows.Forms.Label();
            this.gpuMemUsedLbl = new System.Windows.Forms.Label();
            this.gpuFreeMemLbl = new System.Windows.Forms.Label();
            this.gpuTotalMemLbl = new System.Windows.Forms.Label();
            this.gpuCoreLoadLbl = new System.Windows.Forms.Label();
            this.gpuMemClockLbl = new System.Windows.Forms.Label();
            this.gpuCoreClockLbl = new System.Windows.Forms.Label();
            this.ramLoadLbl = new System.Windows.Forms.Label();
            this.ramUsedLbl = new System.Windows.Forms.Label();
            this.ramHeaderLbl = new System.Windows.Forms.Label();
            this.ramAvailableLbl = new System.Windows.Forms.Label();
            this.ramTotalLbl = new System.Windows.Forms.Label();
            this.ssdTempLbl = new System.Windows.Forms.Label();
            this.ssdUsedPercentLbl = new System.Windows.Forms.Label();
            this.ssdUsedGBLbl = new System.Windows.Forms.Label();
            this.ssdTotalGBLbl = new System.Windows.Forms.Label();
            this.ssdNameLbl = new System.Windows.Forms.Label();
            this.ssdFreePercentLbl = new System.Windows.Forms.Label();
            this.ssdFreeGBLbl = new System.Windows.Forms.Label();
            this.cpuFanLbl = new System.Windows.Forms.Label();
            this.gpuFanLbl = new System.Windows.Forms.Label();
            this.hddTempLbl = new System.Windows.Forms.Label();
            this.hddUsedPercentLbl = new System.Windows.Forms.Label();
            this.hddUsedGBLbl = new System.Windows.Forms.Label();
            this.hddTotalGBLbl = new System.Windows.Forms.Label();
            this.hddNameLbl = new System.Windows.Forms.Label();
            this.hddFreePercentLbl = new System.Windows.Forms.Label();
            this.hddFreeGBLbl = new System.Windows.Forms.Label();
            this.wifiBytesSentLbl = new System.Windows.Forms.Label();
            this.wifiBytesRecvLbl = new System.Windows.Forms.Label();
            this.wifiHeaderLbl = new System.Windows.Forms.Label();
            this.cpuClockHeaderLbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.cpuPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gpuPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ramPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssdPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hddPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wifiPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // cpuPictureBox
            // 
            this.cpuPictureBox.Image = global::DellHardwareMonitor.Properties.Resources.cpu;
            this.cpuPictureBox.Location = new System.Drawing.Point(16, 32);
            this.cpuPictureBox.Name = "cpuPictureBox";
            this.cpuPictureBox.Size = new System.Drawing.Size(80, 80);
            this.cpuPictureBox.TabIndex = 1;
            this.cpuPictureBox.TabStop = false;
            // 
            // gpuPictureBox
            // 
            this.gpuPictureBox.Image = global::DellHardwareMonitor.Properties.Resources.gpu;
            this.gpuPictureBox.Location = new System.Drawing.Point(16, 320);
            this.gpuPictureBox.Name = "gpuPictureBox";
            this.gpuPictureBox.Size = new System.Drawing.Size(80, 80);
            this.gpuPictureBox.TabIndex = 2;
            this.gpuPictureBox.TabStop = false;
            // 
            // ramPictureBox
            // 
            this.ramPictureBox.Image = global::DellHardwareMonitor.Properties.Resources.ram;
            this.ramPictureBox.Location = new System.Drawing.Point(16, 472);
            this.ramPictureBox.Name = "ramPictureBox";
            this.ramPictureBox.Size = new System.Drawing.Size(80, 80);
            this.ramPictureBox.TabIndex = 3;
            this.ramPictureBox.TabStop = false;
            // 
            // ssdPictureBox
            // 
            this.ssdPictureBox.Image = global::DellHardwareMonitor.Properties.Resources.ssd;
            this.ssdPictureBox.Location = new System.Drawing.Point(0, 616);
            this.ssdPictureBox.Name = "ssdPictureBox";
            this.ssdPictureBox.Size = new System.Drawing.Size(80, 80);
            this.ssdPictureBox.TabIndex = 4;
            this.ssdPictureBox.TabStop = false;
            // 
            // hddPictureBox
            // 
            this.hddPictureBox.Image = global::DellHardwareMonitor.Properties.Resources.hdd;
            this.hddPictureBox.Location = new System.Drawing.Point(8, 760);
            this.hddPictureBox.Name = "hddPictureBox";
            this.hddPictureBox.Size = new System.Drawing.Size(80, 80);
            this.hddPictureBox.TabIndex = 5;
            this.hddPictureBox.TabStop = false;
            // 
            // wifiPictureBox
            // 
            this.wifiPictureBox.Image = global::DellHardwareMonitor.Properties.Resources.wifi;
            this.wifiPictureBox.Location = new System.Drawing.Point(8, 920);
            this.wifiPictureBox.Name = "wifiPictureBox";
            this.wifiPictureBox.Size = new System.Drawing.Size(80, 80);
            this.wifiPictureBox.TabIndex = 6;
            this.wifiPictureBox.TabStop = false;
            // 
            // cpuNameLbl
            // 
            this.cpuNameLbl.AutoSize = true;
            this.cpuNameLbl.Location = new System.Drawing.Point(168, 24);
            this.cpuNameLbl.Name = "cpuNameLbl";
            this.cpuNameLbl.Size = new System.Drawing.Size(53, 13);
            this.cpuNameLbl.TabIndex = 7;
            this.cpuNameLbl.Text = "cpuName";
            // 
            // cpu1LoadLbl
            // 
            this.cpu1LoadLbl.AutoSize = true;
            this.cpu1LoadLbl.Location = new System.Drawing.Point(128, 72);
            this.cpu1LoadLbl.Name = "cpu1LoadLbl";
            this.cpu1LoadLbl.Size = new System.Drawing.Size(55, 13);
            this.cpu1LoadLbl.TabIndex = 8;
            this.cpu1LoadLbl.Text = "cpu1Load";
            // 
            // cpu2LoadLbl
            // 
            this.cpu2LoadLbl.AutoSize = true;
            this.cpu2LoadLbl.Location = new System.Drawing.Point(128, 96);
            this.cpu2LoadLbl.Name = "cpu2LoadLbl";
            this.cpu2LoadLbl.Size = new System.Drawing.Size(55, 13);
            this.cpu2LoadLbl.TabIndex = 9;
            this.cpu2LoadLbl.Text = "cpu2Load";
            // 
            // cpu3LoadLbl
            // 
            this.cpu3LoadLbl.AutoSize = true;
            this.cpu3LoadLbl.Location = new System.Drawing.Point(128, 120);
            this.cpu3LoadLbl.Name = "cpu3LoadLbl";
            this.cpu3LoadLbl.Size = new System.Drawing.Size(55, 13);
            this.cpu3LoadLbl.TabIndex = 10;
            this.cpu3LoadLbl.Text = "cpu3Load";
            // 
            // cpu4LoadLbl
            // 
            this.cpu4LoadLbl.AutoSize = true;
            this.cpu4LoadLbl.Location = new System.Drawing.Point(128, 144);
            this.cpu4LoadLbl.Name = "cpu4LoadLbl";
            this.cpu4LoadLbl.Size = new System.Drawing.Size(55, 13);
            this.cpu4LoadLbl.TabIndex = 11;
            this.cpu4LoadLbl.Text = "cpu4Load";
            // 
            // cpu5LoadLbl
            // 
            this.cpu5LoadLbl.AutoSize = true;
            this.cpu5LoadLbl.Location = new System.Drawing.Point(128, 168);
            this.cpu5LoadLbl.Name = "cpu5LoadLbl";
            this.cpu5LoadLbl.Size = new System.Drawing.Size(55, 13);
            this.cpu5LoadLbl.TabIndex = 12;
            this.cpu5LoadLbl.Text = "cpu5Load";
            // 
            // cpu6LoadLbl
            // 
            this.cpu6LoadLbl.AutoSize = true;
            this.cpu6LoadLbl.Location = new System.Drawing.Point(128, 192);
            this.cpu6LoadLbl.Name = "cpu6LoadLbl";
            this.cpu6LoadLbl.Size = new System.Drawing.Size(55, 13);
            this.cpu6LoadLbl.TabIndex = 13;
            this.cpu6LoadLbl.Text = "cpu6Load";
            // 
            // cpu6TempLbl
            // 
            this.cpu6TempLbl.AutoSize = true;
            this.cpu6TempLbl.Location = new System.Drawing.Point(200, 192);
            this.cpu6TempLbl.Name = "cpu6TempLbl";
            this.cpu6TempLbl.Size = new System.Drawing.Size(58, 13);
            this.cpu6TempLbl.TabIndex = 19;
            this.cpu6TempLbl.Text = "cpu6Temp";
            // 
            // cpu5TempLbl
            // 
            this.cpu5TempLbl.AutoSize = true;
            this.cpu5TempLbl.Location = new System.Drawing.Point(200, 168);
            this.cpu5TempLbl.Name = "cpu5TempLbl";
            this.cpu5TempLbl.Size = new System.Drawing.Size(58, 13);
            this.cpu5TempLbl.TabIndex = 18;
            this.cpu5TempLbl.Text = "cpu5Temp";
            // 
            // cpu4TempLbl
            // 
            this.cpu4TempLbl.AutoSize = true;
            this.cpu4TempLbl.Location = new System.Drawing.Point(200, 144);
            this.cpu4TempLbl.Name = "cpu4TempLbl";
            this.cpu4TempLbl.Size = new System.Drawing.Size(58, 13);
            this.cpu4TempLbl.TabIndex = 17;
            this.cpu4TempLbl.Text = "cpu4Temp";
            // 
            // cpu3TempLbl
            // 
            this.cpu3TempLbl.AutoSize = true;
            this.cpu3TempLbl.Location = new System.Drawing.Point(200, 120);
            this.cpu3TempLbl.Name = "cpu3TempLbl";
            this.cpu3TempLbl.Size = new System.Drawing.Size(58, 13);
            this.cpu3TempLbl.TabIndex = 16;
            this.cpu3TempLbl.Text = "cpu3Temp";
            // 
            // cpu2TempLbl
            // 
            this.cpu2TempLbl.AutoSize = true;
            this.cpu2TempLbl.Location = new System.Drawing.Point(200, 96);
            this.cpu2TempLbl.Name = "cpu2TempLbl";
            this.cpu2TempLbl.Size = new System.Drawing.Size(58, 13);
            this.cpu2TempLbl.TabIndex = 15;
            this.cpu2TempLbl.Text = "cpu2Temp";
            // 
            // cpu1TempLbl
            // 
            this.cpu1TempLbl.AutoSize = true;
            this.cpu1TempLbl.Location = new System.Drawing.Point(200, 72);
            this.cpu1TempLbl.Name = "cpu1TempLbl";
            this.cpu1TempLbl.Size = new System.Drawing.Size(58, 13);
            this.cpu1TempLbl.TabIndex = 14;
            this.cpu1TempLbl.Text = "cpu1Temp";
            // 
            // cpuTempHeaderLbl
            // 
            this.cpuTempHeaderLbl.AutoSize = true;
            this.cpuTempHeaderLbl.Location = new System.Drawing.Point(208, 48);
            this.cpuTempHeaderLbl.Name = "cpuTempHeaderLbl";
            this.cpuTempHeaderLbl.Size = new System.Drawing.Size(52, 13);
            this.cpuTempHeaderLbl.TabIndex = 21;
            this.cpuTempHeaderLbl.Text = "cpuTemp";
            // 
            // cpuLoadHeaderLbl
            // 
            this.cpuLoadHeaderLbl.AutoSize = true;
            this.cpuLoadHeaderLbl.Location = new System.Drawing.Point(136, 48);
            this.cpuLoadHeaderLbl.Name = "cpuLoadHeaderLbl";
            this.cpuLoadHeaderLbl.Size = new System.Drawing.Size(49, 13);
            this.cpuLoadHeaderLbl.TabIndex = 20;
            this.cpuLoadHeaderLbl.Text = "cpuLoad";
            // 
            // cpuPackageTempLbl
            // 
            this.cpuPackageTempLbl.AutoSize = true;
            this.cpuPackageTempLbl.Location = new System.Drawing.Point(16, 144);
            this.cpuPackageTempLbl.Name = "cpuPackageTempLbl";
            this.cpuPackageTempLbl.Size = new System.Drawing.Size(95, 13);
            this.cpuPackageTempLbl.TabIndex = 23;
            this.cpuPackageTempLbl.Text = "cpuPackageTemp";
            // 
            // cpuTotalLoadLbl
            // 
            this.cpuTotalLoadLbl.AutoSize = true;
            this.cpuTotalLoadLbl.Location = new System.Drawing.Point(16, 120);
            this.cpuTotalLoadLbl.Name = "cpuTotalLoadLbl";
            this.cpuTotalLoadLbl.Size = new System.Drawing.Size(73, 13);
            this.cpuTotalLoadLbl.TabIndex = 22;
            this.cpuTotalLoadLbl.Text = "cpuTotalLoad";
            // 
            // cpuPackagePwrLbl
            // 
            this.cpuPackagePwrLbl.AutoSize = true;
            this.cpuPackagePwrLbl.Location = new System.Drawing.Point(16, 168);
            this.cpuPackagePwrLbl.Name = "cpuPackagePwrLbl";
            this.cpuPackagePwrLbl.Size = new System.Drawing.Size(86, 13);
            this.cpuPackagePwrLbl.TabIndex = 24;
            this.cpuPackagePwrLbl.Text = "cpuPackagePwr";
            // 
            // cpu6ClockLbl
            // 
            this.cpu6ClockLbl.AutoSize = true;
            this.cpu6ClockLbl.Location = new System.Drawing.Point(176, 264);
            this.cpu6ClockLbl.Name = "cpu6ClockLbl";
            this.cpu6ClockLbl.Size = new System.Drawing.Size(58, 13);
            this.cpu6ClockLbl.TabIndex = 30;
            this.cpu6ClockLbl.Text = "cpu6Clock";
            // 
            // cpu5ClockLbl
            // 
            this.cpu5ClockLbl.AutoSize = true;
            this.cpu5ClockLbl.Location = new System.Drawing.Point(104, 264);
            this.cpu5ClockLbl.Name = "cpu5ClockLbl";
            this.cpu5ClockLbl.Size = new System.Drawing.Size(58, 13);
            this.cpu5ClockLbl.TabIndex = 29;
            this.cpu5ClockLbl.Text = "cpu5Clock";
            // 
            // cpu4ClockLbl
            // 
            this.cpu4ClockLbl.AutoSize = true;
            this.cpu4ClockLbl.Location = new System.Drawing.Point(32, 264);
            this.cpu4ClockLbl.Name = "cpu4ClockLbl";
            this.cpu4ClockLbl.Size = new System.Drawing.Size(58, 13);
            this.cpu4ClockLbl.TabIndex = 28;
            this.cpu4ClockLbl.Text = "cpu4Clock";
            // 
            // cpu3ClockLbl
            // 
            this.cpu3ClockLbl.AutoSize = true;
            this.cpu3ClockLbl.Location = new System.Drawing.Point(176, 240);
            this.cpu3ClockLbl.Name = "cpu3ClockLbl";
            this.cpu3ClockLbl.Size = new System.Drawing.Size(58, 13);
            this.cpu3ClockLbl.TabIndex = 27;
            this.cpu3ClockLbl.Text = "cpu3Clock";
            // 
            // cpu2ClockLbl
            // 
            this.cpu2ClockLbl.AutoSize = true;
            this.cpu2ClockLbl.Location = new System.Drawing.Point(104, 240);
            this.cpu2ClockLbl.Name = "cpu2ClockLbl";
            this.cpu2ClockLbl.Size = new System.Drawing.Size(58, 13);
            this.cpu2ClockLbl.TabIndex = 26;
            this.cpu2ClockLbl.Text = "cpu2Clock";
            // 
            // cpu1ClockLbl
            // 
            this.cpu1ClockLbl.AutoSize = true;
            this.cpu1ClockLbl.Location = new System.Drawing.Point(32, 240);
            this.cpu1ClockLbl.Name = "cpu1ClockLbl";
            this.cpu1ClockLbl.Size = new System.Drawing.Size(58, 13);
            this.cpu1ClockLbl.TabIndex = 25;
            this.cpu1ClockLbl.Text = "cpu1Clock";
            // 
            // gpuName
            // 
            this.gpuName.AutoSize = true;
            this.gpuName.Location = new System.Drawing.Point(160, 296);
            this.gpuName.Name = "gpuName";
            this.gpuName.Size = new System.Drawing.Size(53, 13);
            this.gpuName.TabIndex = 31;
            this.gpuName.Text = "gpuName";
            // 
            // gpuTempLbl
            // 
            this.gpuTempLbl.AutoSize = true;
            this.gpuTempLbl.Location = new System.Drawing.Point(120, 328);
            this.gpuTempLbl.Name = "gpuTempLbl";
            this.gpuTempLbl.Size = new System.Drawing.Size(52, 13);
            this.gpuTempLbl.TabIndex = 38;
            this.gpuTempLbl.Text = "gpuTemp";
            // 
            // gpuMemUsedLbl
            // 
            this.gpuMemUsedLbl.AutoSize = true;
            this.gpuMemUsedLbl.Location = new System.Drawing.Point(200, 392);
            this.gpuMemUsedLbl.Name = "gpuMemUsedLbl";
            this.gpuMemUsedLbl.Size = new System.Drawing.Size(73, 13);
            this.gpuMemUsedLbl.TabIndex = 37;
            this.gpuMemUsedLbl.Text = "gpuMemUsed";
            // 
            // gpuFreeMemLbl
            // 
            this.gpuFreeMemLbl.AutoSize = true;
            this.gpuFreeMemLbl.Location = new System.Drawing.Point(200, 368);
            this.gpuFreeMemLbl.Name = "gpuFreeMemLbl";
            this.gpuFreeMemLbl.Size = new System.Drawing.Size(69, 13);
            this.gpuFreeMemLbl.TabIndex = 36;
            this.gpuFreeMemLbl.Text = "gpuFreeMem";
            // 
            // gpuTotalMemLbl
            // 
            this.gpuTotalMemLbl.AutoSize = true;
            this.gpuTotalMemLbl.Location = new System.Drawing.Point(200, 344);
            this.gpuTotalMemLbl.Name = "gpuTotalMemLbl";
            this.gpuTotalMemLbl.Size = new System.Drawing.Size(72, 13);
            this.gpuTotalMemLbl.TabIndex = 35;
            this.gpuTotalMemLbl.Text = "gpuTotalMem";
            // 
            // gpuCoreLoadLbl
            // 
            this.gpuCoreLoadLbl.AutoSize = true;
            this.gpuCoreLoadLbl.Location = new System.Drawing.Point(200, 320);
            this.gpuCoreLoadLbl.Name = "gpuCoreLoadLbl";
            this.gpuCoreLoadLbl.Size = new System.Drawing.Size(71, 13);
            this.gpuCoreLoadLbl.TabIndex = 34;
            this.gpuCoreLoadLbl.Text = "gpuCoreLoad";
            // 
            // gpuMemClockLbl
            // 
            this.gpuMemClockLbl.AutoSize = true;
            this.gpuMemClockLbl.Location = new System.Drawing.Point(112, 376);
            this.gpuMemClockLbl.Name = "gpuMemClockLbl";
            this.gpuMemClockLbl.Size = new System.Drawing.Size(75, 13);
            this.gpuMemClockLbl.TabIndex = 33;
            this.gpuMemClockLbl.Text = "gpuMemClock";
            // 
            // gpuCoreClockLbl
            // 
            this.gpuCoreClockLbl.AutoSize = true;
            this.gpuCoreClockLbl.Location = new System.Drawing.Point(112, 352);
            this.gpuCoreClockLbl.Name = "gpuCoreClockLbl";
            this.gpuCoreClockLbl.Size = new System.Drawing.Size(74, 13);
            this.gpuCoreClockLbl.TabIndex = 32;
            this.gpuCoreClockLbl.Text = "gpuCoreClock";
            // 
            // ramLoadLbl
            // 
            this.ramLoadLbl.AutoSize = true;
            this.ramLoadLbl.Location = new System.Drawing.Point(208, 528);
            this.ramLoadLbl.Name = "ramLoadLbl";
            this.ramLoadLbl.Size = new System.Drawing.Size(48, 13);
            this.ramLoadLbl.TabIndex = 43;
            this.ramLoadLbl.Text = "ramLoad";
            // 
            // ramUsedLbl
            // 
            this.ramUsedLbl.AutoSize = true;
            this.ramUsedLbl.Location = new System.Drawing.Point(208, 504);
            this.ramUsedLbl.Name = "ramUsedLbl";
            this.ramUsedLbl.Size = new System.Drawing.Size(49, 13);
            this.ramUsedLbl.TabIndex = 42;
            this.ramUsedLbl.Text = "ramUsed";
            // 
            // ramHeaderLbl
            // 
            this.ramHeaderLbl.AutoSize = true;
            this.ramHeaderLbl.Location = new System.Drawing.Point(168, 480);
            this.ramHeaderLbl.Name = "ramHeaderLbl";
            this.ramHeaderLbl.Size = new System.Drawing.Size(50, 13);
            this.ramHeaderLbl.TabIndex = 41;
            this.ramHeaderLbl.Text = "ramLabel";
            // 
            // ramAvailableLbl
            // 
            this.ramAvailableLbl.AutoSize = true;
            this.ramAvailableLbl.Location = new System.Drawing.Point(120, 528);
            this.ramAvailableLbl.Name = "ramAvailableLbl";
            this.ramAvailableLbl.Size = new System.Drawing.Size(67, 13);
            this.ramAvailableLbl.TabIndex = 40;
            this.ramAvailableLbl.Text = "ramAvailable";
            // 
            // ramTotalLbl
            // 
            this.ramTotalLbl.AutoSize = true;
            this.ramTotalLbl.Location = new System.Drawing.Point(120, 504);
            this.ramTotalLbl.Name = "ramTotalLbl";
            this.ramTotalLbl.Size = new System.Drawing.Size(48, 13);
            this.ramTotalLbl.TabIndex = 39;
            this.ramTotalLbl.Text = "ramTotal";
            // 
            // ssdTempLbl
            // 
            this.ssdTempLbl.AutoSize = true;
            this.ssdTempLbl.Location = new System.Drawing.Point(96, 640);
            this.ssdTempLbl.Name = "ssdTempLbl";
            this.ssdTempLbl.Size = new System.Drawing.Size(50, 13);
            this.ssdTempLbl.TabIndex = 50;
            this.ssdTempLbl.Text = "ssdTemp";
            // 
            // ssdUsedPercentLbl
            // 
            this.ssdUsedPercentLbl.AutoSize = true;
            this.ssdUsedPercentLbl.Location = new System.Drawing.Point(184, 688);
            this.ssdUsedPercentLbl.Name = "ssdUsedPercentLbl";
            this.ssdUsedPercentLbl.Size = new System.Drawing.Size(85, 13);
            this.ssdUsedPercentLbl.TabIndex = 49;
            this.ssdUsedPercentLbl.Text = "ssdUsedPercent";
            // 
            // ssdUsedGBLbl
            // 
            this.ssdUsedGBLbl.AutoSize = true;
            this.ssdUsedGBLbl.Location = new System.Drawing.Point(184, 664);
            this.ssdUsedGBLbl.Name = "ssdUsedGBLbl";
            this.ssdUsedGBLbl.Size = new System.Drawing.Size(63, 13);
            this.ssdUsedGBLbl.TabIndex = 48;
            this.ssdUsedGBLbl.Text = "ssdUsedGB";
            // 
            // ssdTotalGBLbl
            // 
            this.ssdTotalGBLbl.AutoSize = true;
            this.ssdTotalGBLbl.Location = new System.Drawing.Point(184, 640);
            this.ssdTotalGBLbl.Name = "ssdTotalGBLbl";
            this.ssdTotalGBLbl.Size = new System.Drawing.Size(62, 13);
            this.ssdTotalGBLbl.TabIndex = 47;
            this.ssdTotalGBLbl.Text = "ssdTotalGB";
            // 
            // ssdNameLbl
            // 
            this.ssdNameLbl.AutoSize = true;
            this.ssdNameLbl.Location = new System.Drawing.Point(96, 616);
            this.ssdNameLbl.Name = "ssdNameLbl";
            this.ssdNameLbl.Size = new System.Drawing.Size(51, 13);
            this.ssdNameLbl.TabIndex = 46;
            this.ssdNameLbl.Text = "ssdName";
            // 
            // ssdFreePercentLbl
            // 
            this.ssdFreePercentLbl.AutoSize = true;
            this.ssdFreePercentLbl.Location = new System.Drawing.Point(96, 688);
            this.ssdFreePercentLbl.Name = "ssdFreePercentLbl";
            this.ssdFreePercentLbl.Size = new System.Drawing.Size(81, 13);
            this.ssdFreePercentLbl.TabIndex = 45;
            this.ssdFreePercentLbl.Text = "ssdFreePercent";
            // 
            // ssdFreeGBLbl
            // 
            this.ssdFreeGBLbl.AutoSize = true;
            this.ssdFreeGBLbl.Location = new System.Drawing.Point(96, 664);
            this.ssdFreeGBLbl.Name = "ssdFreeGBLbl";
            this.ssdFreeGBLbl.Size = new System.Drawing.Size(59, 13);
            this.ssdFreeGBLbl.TabIndex = 44;
            this.ssdFreeGBLbl.Text = "ssdFreeGB";
            // 
            // cpuFanLbl
            // 
            this.cpuFanLbl.AutoSize = true;
            this.cpuFanLbl.Location = new System.Drawing.Point(16, 192);
            this.cpuFanLbl.Name = "cpuFanLbl";
            this.cpuFanLbl.Size = new System.Drawing.Size(43, 13);
            this.cpuFanLbl.TabIndex = 51;
            this.cpuFanLbl.Text = "cpuFan";
            // 
            // gpuFanLbl
            // 
            this.gpuFanLbl.AutoSize = true;
            this.gpuFanLbl.Location = new System.Drawing.Point(112, 400);
            this.gpuFanLbl.Name = "gpuFanLbl";
            this.gpuFanLbl.Size = new System.Drawing.Size(43, 13);
            this.gpuFanLbl.TabIndex = 52;
            this.gpuFanLbl.Text = "gpuFan";
            // 
            // hddTempLbl
            // 
            this.hddTempLbl.AutoSize = true;
            this.hddTempLbl.Location = new System.Drawing.Point(104, 784);
            this.hddTempLbl.Name = "hddTempLbl";
            this.hddTempLbl.Size = new System.Drawing.Size(52, 13);
            this.hddTempLbl.TabIndex = 59;
            this.hddTempLbl.Text = "hddTemp";
            // 
            // hddUsedPercentLbl
            // 
            this.hddUsedPercentLbl.AutoSize = true;
            this.hddUsedPercentLbl.Location = new System.Drawing.Point(192, 832);
            this.hddUsedPercentLbl.Name = "hddUsedPercentLbl";
            this.hddUsedPercentLbl.Size = new System.Drawing.Size(87, 13);
            this.hddUsedPercentLbl.TabIndex = 58;
            this.hddUsedPercentLbl.Text = "hddUsedPercent";
            // 
            // hddUsedGBLbl
            // 
            this.hddUsedGBLbl.AutoSize = true;
            this.hddUsedGBLbl.Location = new System.Drawing.Point(192, 808);
            this.hddUsedGBLbl.Name = "hddUsedGBLbl";
            this.hddUsedGBLbl.Size = new System.Drawing.Size(65, 13);
            this.hddUsedGBLbl.TabIndex = 57;
            this.hddUsedGBLbl.Text = "hddUsedGB";
            // 
            // hddTotalGBLbl
            // 
            this.hddTotalGBLbl.AutoSize = true;
            this.hddTotalGBLbl.Location = new System.Drawing.Point(192, 784);
            this.hddTotalGBLbl.Name = "hddTotalGBLbl";
            this.hddTotalGBLbl.Size = new System.Drawing.Size(64, 13);
            this.hddTotalGBLbl.TabIndex = 56;
            this.hddTotalGBLbl.Text = "hddTotalGB";
            // 
            // hddNameLbl
            // 
            this.hddNameLbl.AutoSize = true;
            this.hddNameLbl.Location = new System.Drawing.Point(104, 760);
            this.hddNameLbl.Name = "hddNameLbl";
            this.hddNameLbl.Size = new System.Drawing.Size(53, 13);
            this.hddNameLbl.TabIndex = 55;
            this.hddNameLbl.Text = "hddName";
            // 
            // hddFreePercentLbl
            // 
            this.hddFreePercentLbl.AutoSize = true;
            this.hddFreePercentLbl.Location = new System.Drawing.Point(104, 832);
            this.hddFreePercentLbl.Name = "hddFreePercentLbl";
            this.hddFreePercentLbl.Size = new System.Drawing.Size(83, 13);
            this.hddFreePercentLbl.TabIndex = 54;
            this.hddFreePercentLbl.Text = "hddFreePercent";
            // 
            // hddFreeGBLbl
            // 
            this.hddFreeGBLbl.AutoSize = true;
            this.hddFreeGBLbl.Location = new System.Drawing.Point(104, 808);
            this.hddFreeGBLbl.Name = "hddFreeGBLbl";
            this.hddFreeGBLbl.Size = new System.Drawing.Size(61, 13);
            this.hddFreeGBLbl.TabIndex = 53;
            this.hddFreeGBLbl.Text = "hddFreeGB";
            // 
            // wifiBytesSentLbl
            // 
            this.wifiBytesSentLbl.AutoSize = true;
            this.wifiBytesSentLbl.Location = new System.Drawing.Point(104, 976);
            this.wifiBytesSentLbl.Name = "wifiBytesSentLbl";
            this.wifiBytesSentLbl.Size = new System.Drawing.Size(70, 13);
            this.wifiBytesSentLbl.TabIndex = 62;
            this.wifiBytesSentLbl.Text = "wifiBytesSent";
            // 
            // wifiBytesRecvLbl
            // 
            this.wifiBytesRecvLbl.AutoSize = true;
            this.wifiBytesRecvLbl.Location = new System.Drawing.Point(104, 952);
            this.wifiBytesRecvLbl.Name = "wifiBytesRecvLbl";
            this.wifiBytesRecvLbl.Size = new System.Drawing.Size(74, 13);
            this.wifiBytesRecvLbl.TabIndex = 61;
            this.wifiBytesRecvLbl.Text = "wifiBytesRecv";
            // 
            // wifiHeaderLbl
            // 
            this.wifiHeaderLbl.AutoSize = true;
            this.wifiHeaderLbl.Location = new System.Drawing.Point(104, 928);
            this.wifiHeaderLbl.Name = "wifiHeaderLbl";
            this.wifiHeaderLbl.Size = new System.Drawing.Size(48, 13);
            this.wifiHeaderLbl.TabIndex = 60;
            this.wifiHeaderLbl.Text = "wifiLabel";
            // 
            // cpuClockHeaderLbl
            // 
            this.cpuClockHeaderLbl.AutoSize = true;
            this.cpuClockHeaderLbl.Location = new System.Drawing.Point(112, 216);
            this.cpuClockHeaderLbl.Name = "cpuClockHeaderLbl";
            this.cpuClockHeaderLbl.Size = new System.Drawing.Size(52, 13);
            this.cpuClockHeaderLbl.TabIndex = 63;
            this.cpuClockHeaderLbl.Text = "cpuClock";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(284, 1011);
            this.Controls.Add(this.cpuClockHeaderLbl);
            this.Controls.Add(this.wifiBytesSentLbl);
            this.Controls.Add(this.wifiBytesRecvLbl);
            this.Controls.Add(this.wifiHeaderLbl);
            this.Controls.Add(this.hddTempLbl);
            this.Controls.Add(this.hddUsedPercentLbl);
            this.Controls.Add(this.hddUsedGBLbl);
            this.Controls.Add(this.hddTotalGBLbl);
            this.Controls.Add(this.hddNameLbl);
            this.Controls.Add(this.hddFreePercentLbl);
            this.Controls.Add(this.hddFreeGBLbl);
            this.Controls.Add(this.gpuFanLbl);
            this.Controls.Add(this.cpuFanLbl);
            this.Controls.Add(this.ssdTempLbl);
            this.Controls.Add(this.ssdUsedPercentLbl);
            this.Controls.Add(this.ssdUsedGBLbl);
            this.Controls.Add(this.ssdTotalGBLbl);
            this.Controls.Add(this.ssdNameLbl);
            this.Controls.Add(this.ssdFreePercentLbl);
            this.Controls.Add(this.ssdFreeGBLbl);
            this.Controls.Add(this.ramLoadLbl);
            this.Controls.Add(this.ramUsedLbl);
            this.Controls.Add(this.ramHeaderLbl);
            this.Controls.Add(this.ramAvailableLbl);
            this.Controls.Add(this.ramTotalLbl);
            this.Controls.Add(this.gpuTempLbl);
            this.Controls.Add(this.gpuMemUsedLbl);
            this.Controls.Add(this.gpuFreeMemLbl);
            this.Controls.Add(this.gpuTotalMemLbl);
            this.Controls.Add(this.gpuCoreLoadLbl);
            this.Controls.Add(this.gpuMemClockLbl);
            this.Controls.Add(this.gpuCoreClockLbl);
            this.Controls.Add(this.gpuName);
            this.Controls.Add(this.cpu6ClockLbl);
            this.Controls.Add(this.cpu5ClockLbl);
            this.Controls.Add(this.cpu4ClockLbl);
            this.Controls.Add(this.cpu3ClockLbl);
            this.Controls.Add(this.cpu2ClockLbl);
            this.Controls.Add(this.cpu1ClockLbl);
            this.Controls.Add(this.cpuPackagePwrLbl);
            this.Controls.Add(this.cpuPackageTempLbl);
            this.Controls.Add(this.cpuTotalLoadLbl);
            this.Controls.Add(this.cpuTempHeaderLbl);
            this.Controls.Add(this.cpuLoadHeaderLbl);
            this.Controls.Add(this.cpu6TempLbl);
            this.Controls.Add(this.cpu5TempLbl);
            this.Controls.Add(this.cpu4TempLbl);
            this.Controls.Add(this.cpu3TempLbl);
            this.Controls.Add(this.cpu2TempLbl);
            this.Controls.Add(this.cpu1TempLbl);
            this.Controls.Add(this.cpu6LoadLbl);
            this.Controls.Add(this.cpu5LoadLbl);
            this.Controls.Add(this.cpu4LoadLbl);
            this.Controls.Add(this.cpu3LoadLbl);
            this.Controls.Add(this.cpu2LoadLbl);
            this.Controls.Add(this.cpu1LoadLbl);
            this.Controls.Add(this.cpuNameLbl);
            this.Controls.Add(this.wifiPictureBox);
            this.Controls.Add(this.hddPictureBox);
            this.Controls.Add(this.ssdPictureBox);
            this.Controls.Add(this.ramPictureBox);
            this.Controls.Add(this.gpuPictureBox);
            this.Controls.Add(this.cpuPictureBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.Text = "Form1";
            this.TransparencyKey = System.Drawing.Color.Silver;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.cpuPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gpuPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ramPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssdPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hddPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wifiPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox cpuPictureBox;
        private System.Windows.Forms.PictureBox gpuPictureBox;
        private System.Windows.Forms.PictureBox ramPictureBox;
        private System.Windows.Forms.PictureBox ssdPictureBox;
        private System.Windows.Forms.PictureBox hddPictureBox;
        private System.Windows.Forms.PictureBox wifiPictureBox;
        private System.Windows.Forms.Label cpuNameLbl;
        private System.Windows.Forms.Label cpu1LoadLbl;
        private System.Windows.Forms.Label cpu2LoadLbl;
        private System.Windows.Forms.Label cpu3LoadLbl;
        private System.Windows.Forms.Label cpu4LoadLbl;
        private System.Windows.Forms.Label cpu5LoadLbl;
        private System.Windows.Forms.Label cpu6LoadLbl;
        private System.Windows.Forms.Label cpu6TempLbl;
        private System.Windows.Forms.Label cpu5TempLbl;
        private System.Windows.Forms.Label cpu4TempLbl;
        private System.Windows.Forms.Label cpu3TempLbl;
        private System.Windows.Forms.Label cpu2TempLbl;
        private System.Windows.Forms.Label cpu1TempLbl;
        private System.Windows.Forms.Label cpuTempHeaderLbl;
        private System.Windows.Forms.Label cpuLoadHeaderLbl;
        private System.Windows.Forms.Label cpuPackageTempLbl;
        private System.Windows.Forms.Label cpuTotalLoadLbl;
        private System.Windows.Forms.Label cpuPackagePwrLbl;
        private System.Windows.Forms.Label cpu6ClockLbl;
        private System.Windows.Forms.Label cpu5ClockLbl;
        private System.Windows.Forms.Label cpu4ClockLbl;
        private System.Windows.Forms.Label cpu3ClockLbl;
        private System.Windows.Forms.Label cpu2ClockLbl;
        private System.Windows.Forms.Label cpu1ClockLbl;
        private System.Windows.Forms.Label gpuName;
        private System.Windows.Forms.Label gpuTempLbl;
        private System.Windows.Forms.Label gpuMemUsedLbl;
        private System.Windows.Forms.Label gpuFreeMemLbl;
        private System.Windows.Forms.Label gpuTotalMemLbl;
        private System.Windows.Forms.Label gpuCoreLoadLbl;
        private System.Windows.Forms.Label gpuMemClockLbl;
        private System.Windows.Forms.Label gpuCoreClockLbl;
        private System.Windows.Forms.Label ramLoadLbl;
        private System.Windows.Forms.Label ramUsedLbl;
        private System.Windows.Forms.Label ramHeaderLbl;
        private System.Windows.Forms.Label ramAvailableLbl;
        private System.Windows.Forms.Label ramTotalLbl;
        private System.Windows.Forms.Label ssdTempLbl;
        private System.Windows.Forms.Label ssdUsedPercentLbl;
        private System.Windows.Forms.Label ssdUsedGBLbl;
        private System.Windows.Forms.Label ssdTotalGBLbl;
        private System.Windows.Forms.Label ssdNameLbl;
        private System.Windows.Forms.Label ssdFreePercentLbl;
        private System.Windows.Forms.Label ssdFreeGBLbl;
        private System.Windows.Forms.Label cpuFanLbl;
        private System.Windows.Forms.Label gpuFanLbl;
        private System.Windows.Forms.Label hddTempLbl;
        private System.Windows.Forms.Label hddUsedPercentLbl;
        private System.Windows.Forms.Label hddUsedGBLbl;
        private System.Windows.Forms.Label hddTotalGBLbl;
        private System.Windows.Forms.Label hddNameLbl;
        private System.Windows.Forms.Label hddFreePercentLbl;
        private System.Windows.Forms.Label hddFreeGBLbl;
        private System.Windows.Forms.Label wifiBytesSentLbl;
        private System.Windows.Forms.Label wifiBytesRecvLbl;
        private System.Windows.Forms.Label wifiHeaderLbl;
        private System.Windows.Forms.Label cpuClockHeaderLbl;
    }
}

