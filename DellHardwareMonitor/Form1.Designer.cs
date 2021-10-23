
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.cpuPictureBox = new System.Windows.Forms.PictureBox();
            this.gpuPictureBox = new System.Windows.Forms.PictureBox();
            this.ramPictureBox = new System.Windows.Forms.PictureBox();
            this.ssdPictureBox = new System.Windows.Forms.PictureBox();
            this.hddPictureBox = new System.Windows.Forms.PictureBox();
            this.wifiPictureBox = new System.Windows.Forms.PictureBox();
            this.cpuName = new System.Windows.Forms.Label();
            this.cpu1Load = new System.Windows.Forms.Label();
            this.cpu2Load = new System.Windows.Forms.Label();
            this.cpu3Load = new System.Windows.Forms.Label();
            this.cpu4Load = new System.Windows.Forms.Label();
            this.cpu5Load = new System.Windows.Forms.Label();
            this.cpu6Load = new System.Windows.Forms.Label();
            this.cpu6Heat = new System.Windows.Forms.Label();
            this.cpu5Heat = new System.Windows.Forms.Label();
            this.cpu4Heat = new System.Windows.Forms.Label();
            this.cpu3Heat = new System.Windows.Forms.Label();
            this.cpu2Heat = new System.Windows.Forms.Label();
            this.cpu1Heat = new System.Windows.Forms.Label();
            this.cpuHeat = new System.Windows.Forms.Label();
            this.cpuLoad = new System.Windows.Forms.Label();
            this.cpuPackageTemp = new System.Windows.Forms.Label();
            this.cpuTotalLoad = new System.Windows.Forms.Label();
            this.cpuPackagePwr = new System.Windows.Forms.Label();
            this.cpu6Clock = new System.Windows.Forms.Label();
            this.cpu5Clock = new System.Windows.Forms.Label();
            this.cpu4Clock = new System.Windows.Forms.Label();
            this.cpu3Clock = new System.Windows.Forms.Label();
            this.cpu2Clock = new System.Windows.Forms.Label();
            this.cpu1Clock = new System.Windows.Forms.Label();
            this.gpuName = new System.Windows.Forms.Label();
            this.gpuTemp = new System.Windows.Forms.Label();
            this.gpuMemUsed = new System.Windows.Forms.Label();
            this.gpuFreeMem = new System.Windows.Forms.Label();
            this.gpuTotalMem = new System.Windows.Forms.Label();
            this.gpuCoreLoad = new System.Windows.Forms.Label();
            this.gpuMemClock = new System.Windows.Forms.Label();
            this.gpuCoreClock = new System.Windows.Forms.Label();
            this.ramLoad = new System.Windows.Forms.Label();
            this.ramUsed = new System.Windows.Forms.Label();
            this.ramLabel = new System.Windows.Forms.Label();
            this.ramAvailable = new System.Windows.Forms.Label();
            this.ramTotal = new System.Windows.Forms.Label();
            this.ssdTemp = new System.Windows.Forms.Label();
            this.ssdUsedPercent = new System.Windows.Forms.Label();
            this.ssdUsedGB = new System.Windows.Forms.Label();
            this.ssdTotalGB = new System.Windows.Forms.Label();
            this.ssdName = new System.Windows.Forms.Label();
            this.ssdFreePercent = new System.Windows.Forms.Label();
            this.ssdFreeGB = new System.Windows.Forms.Label();
            this.cpuFan = new System.Windows.Forms.Label();
            this.gpuFan = new System.Windows.Forms.Label();
            this.hddTemp = new System.Windows.Forms.Label();
            this.hddUsedPercent = new System.Windows.Forms.Label();
            this.hddUsedGB = new System.Windows.Forms.Label();
            this.hddTotalGB = new System.Windows.Forms.Label();
            this.hddName = new System.Windows.Forms.Label();
            this.hddFreePercent = new System.Windows.Forms.Label();
            this.hddFreeGB = new System.Windows.Forms.Label();
            this.wifiBytesSent = new System.Windows.Forms.Label();
            this.wifiBytesRecv = new System.Windows.Forms.Label();
            this.wifiLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.cpuPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gpuPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ramPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssdPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hddPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wifiPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(256, 984);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(16, 16);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
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
            // cpuName
            // 
            this.cpuName.AutoSize = true;
            this.cpuName.Location = new System.Drawing.Point(168, 24);
            this.cpuName.Name = "cpuName";
            this.cpuName.Size = new System.Drawing.Size(53, 13);
            this.cpuName.TabIndex = 7;
            this.cpuName.Text = "cpuName";
            // 
            // cpu1Load
            // 
            this.cpu1Load.AutoSize = true;
            this.cpu1Load.Location = new System.Drawing.Point(128, 72);
            this.cpu1Load.Name = "cpu1Load";
            this.cpu1Load.Size = new System.Drawing.Size(55, 13);
            this.cpu1Load.TabIndex = 8;
            this.cpu1Load.Text = "cpu1Load";
            // 
            // cpu2Load
            // 
            this.cpu2Load.AutoSize = true;
            this.cpu2Load.Location = new System.Drawing.Point(128, 96);
            this.cpu2Load.Name = "cpu2Load";
            this.cpu2Load.Size = new System.Drawing.Size(55, 13);
            this.cpu2Load.TabIndex = 9;
            this.cpu2Load.Text = "cpu2Load";
            // 
            // cpu3Load
            // 
            this.cpu3Load.AutoSize = true;
            this.cpu3Load.Location = new System.Drawing.Point(128, 120);
            this.cpu3Load.Name = "cpu3Load";
            this.cpu3Load.Size = new System.Drawing.Size(55, 13);
            this.cpu3Load.TabIndex = 10;
            this.cpu3Load.Text = "cpu3Load";
            // 
            // cpu4Load
            // 
            this.cpu4Load.AutoSize = true;
            this.cpu4Load.Location = new System.Drawing.Point(128, 144);
            this.cpu4Load.Name = "cpu4Load";
            this.cpu4Load.Size = new System.Drawing.Size(55, 13);
            this.cpu4Load.TabIndex = 11;
            this.cpu4Load.Text = "cpu4Load";
            // 
            // cpu5Load
            // 
            this.cpu5Load.AutoSize = true;
            this.cpu5Load.Location = new System.Drawing.Point(128, 168);
            this.cpu5Load.Name = "cpu5Load";
            this.cpu5Load.Size = new System.Drawing.Size(55, 13);
            this.cpu5Load.TabIndex = 12;
            this.cpu5Load.Text = "cpu5Load";
            // 
            // cpu6Load
            // 
            this.cpu6Load.AutoSize = true;
            this.cpu6Load.Location = new System.Drawing.Point(128, 192);
            this.cpu6Load.Name = "cpu6Load";
            this.cpu6Load.Size = new System.Drawing.Size(55, 13);
            this.cpu6Load.TabIndex = 13;
            this.cpu6Load.Text = "cpu6Load";
            // 
            // cpu6Heat
            // 
            this.cpu6Heat.AutoSize = true;
            this.cpu6Heat.Location = new System.Drawing.Point(200, 192);
            this.cpu6Heat.Name = "cpu6Heat";
            this.cpu6Heat.Size = new System.Drawing.Size(54, 13);
            this.cpu6Heat.TabIndex = 19;
            this.cpu6Heat.Text = "cpu6Heat";
            // 
            // cpu5Heat
            // 
            this.cpu5Heat.AutoSize = true;
            this.cpu5Heat.Location = new System.Drawing.Point(200, 168);
            this.cpu5Heat.Name = "cpu5Heat";
            this.cpu5Heat.Size = new System.Drawing.Size(54, 13);
            this.cpu5Heat.TabIndex = 18;
            this.cpu5Heat.Text = "cpu5Heat";
            // 
            // cpu4Heat
            // 
            this.cpu4Heat.AutoSize = true;
            this.cpu4Heat.Location = new System.Drawing.Point(200, 144);
            this.cpu4Heat.Name = "cpu4Heat";
            this.cpu4Heat.Size = new System.Drawing.Size(54, 13);
            this.cpu4Heat.TabIndex = 17;
            this.cpu4Heat.Text = "cpu4Heat";
            // 
            // cpu3Heat
            // 
            this.cpu3Heat.AutoSize = true;
            this.cpu3Heat.Location = new System.Drawing.Point(200, 120);
            this.cpu3Heat.Name = "cpu3Heat";
            this.cpu3Heat.Size = new System.Drawing.Size(54, 13);
            this.cpu3Heat.TabIndex = 16;
            this.cpu3Heat.Text = "cpu3Heat";
            // 
            // cpu2Heat
            // 
            this.cpu2Heat.AutoSize = true;
            this.cpu2Heat.Location = new System.Drawing.Point(200, 96);
            this.cpu2Heat.Name = "cpu2Heat";
            this.cpu2Heat.Size = new System.Drawing.Size(54, 13);
            this.cpu2Heat.TabIndex = 15;
            this.cpu2Heat.Text = "cpu2Heat";
            // 
            // cpu1Heat
            // 
            this.cpu1Heat.AutoSize = true;
            this.cpu1Heat.Location = new System.Drawing.Point(200, 72);
            this.cpu1Heat.Name = "cpu1Heat";
            this.cpu1Heat.Size = new System.Drawing.Size(54, 13);
            this.cpu1Heat.TabIndex = 14;
            this.cpu1Heat.Text = "cpu1Heat";
            // 
            // cpuHeat
            // 
            this.cpuHeat.AutoSize = true;
            this.cpuHeat.Location = new System.Drawing.Point(208, 48);
            this.cpuHeat.Name = "cpuHeat";
            this.cpuHeat.Size = new System.Drawing.Size(48, 13);
            this.cpuHeat.TabIndex = 21;
            this.cpuHeat.Text = "cpuHeat";
            // 
            // cpuLoad
            // 
            this.cpuLoad.AutoSize = true;
            this.cpuLoad.Location = new System.Drawing.Point(136, 48);
            this.cpuLoad.Name = "cpuLoad";
            this.cpuLoad.Size = new System.Drawing.Size(49, 13);
            this.cpuLoad.TabIndex = 20;
            this.cpuLoad.Text = "cpuLoad";
            // 
            // cpuPackageTemp
            // 
            this.cpuPackageTemp.AutoSize = true;
            this.cpuPackageTemp.Location = new System.Drawing.Point(16, 144);
            this.cpuPackageTemp.Name = "cpuPackageTemp";
            this.cpuPackageTemp.Size = new System.Drawing.Size(95, 13);
            this.cpuPackageTemp.TabIndex = 23;
            this.cpuPackageTemp.Text = "cpuPackageTemp";
            // 
            // cpuTotalLoad
            // 
            this.cpuTotalLoad.AutoSize = true;
            this.cpuTotalLoad.Location = new System.Drawing.Point(16, 120);
            this.cpuTotalLoad.Name = "cpuTotalLoad";
            this.cpuTotalLoad.Size = new System.Drawing.Size(73, 13);
            this.cpuTotalLoad.TabIndex = 22;
            this.cpuTotalLoad.Text = "cpuTotalLoad";
            // 
            // cpuPackagePwr
            // 
            this.cpuPackagePwr.AutoSize = true;
            this.cpuPackagePwr.Location = new System.Drawing.Point(16, 168);
            this.cpuPackagePwr.Name = "cpuPackagePwr";
            this.cpuPackagePwr.Size = new System.Drawing.Size(86, 13);
            this.cpuPackagePwr.TabIndex = 24;
            this.cpuPackagePwr.Text = "cpuPackagePwr";
            // 
            // cpu6Clock
            // 
            this.cpu6Clock.AutoSize = true;
            this.cpu6Clock.Location = new System.Drawing.Point(176, 248);
            this.cpu6Clock.Name = "cpu6Clock";
            this.cpu6Clock.Size = new System.Drawing.Size(58, 13);
            this.cpu6Clock.TabIndex = 30;
            this.cpu6Clock.Text = "cpu6Clock";
            // 
            // cpu5Clock
            // 
            this.cpu5Clock.AutoSize = true;
            this.cpu5Clock.Location = new System.Drawing.Point(104, 248);
            this.cpu5Clock.Name = "cpu5Clock";
            this.cpu5Clock.Size = new System.Drawing.Size(58, 13);
            this.cpu5Clock.TabIndex = 29;
            this.cpu5Clock.Text = "cpu5Clock";
            // 
            // cpu4Clock
            // 
            this.cpu4Clock.AutoSize = true;
            this.cpu4Clock.Location = new System.Drawing.Point(32, 248);
            this.cpu4Clock.Name = "cpu4Clock";
            this.cpu4Clock.Size = new System.Drawing.Size(58, 13);
            this.cpu4Clock.TabIndex = 28;
            this.cpu4Clock.Text = "cpu4Clock";
            // 
            // cpu3Clock
            // 
            this.cpu3Clock.AutoSize = true;
            this.cpu3Clock.Location = new System.Drawing.Point(176, 224);
            this.cpu3Clock.Name = "cpu3Clock";
            this.cpu3Clock.Size = new System.Drawing.Size(58, 13);
            this.cpu3Clock.TabIndex = 27;
            this.cpu3Clock.Text = "cpu3Clock";
            // 
            // cpu2Clock
            // 
            this.cpu2Clock.AutoSize = true;
            this.cpu2Clock.Location = new System.Drawing.Point(104, 224);
            this.cpu2Clock.Name = "cpu2Clock";
            this.cpu2Clock.Size = new System.Drawing.Size(58, 13);
            this.cpu2Clock.TabIndex = 26;
            this.cpu2Clock.Text = "cpu2Clock";
            // 
            // cpu1Clock
            // 
            this.cpu1Clock.AutoSize = true;
            this.cpu1Clock.Location = new System.Drawing.Point(32, 224);
            this.cpu1Clock.Name = "cpu1Clock";
            this.cpu1Clock.Size = new System.Drawing.Size(58, 13);
            this.cpu1Clock.TabIndex = 25;
            this.cpu1Clock.Text = "cpu1Clock";
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
            // gpuTemp
            // 
            this.gpuTemp.AutoSize = true;
            this.gpuTemp.Location = new System.Drawing.Point(120, 328);
            this.gpuTemp.Name = "gpuTemp";
            this.gpuTemp.Size = new System.Drawing.Size(52, 13);
            this.gpuTemp.TabIndex = 38;
            this.gpuTemp.Text = "gpuTemp";
            // 
            // gpuMemUsed
            // 
            this.gpuMemUsed.AutoSize = true;
            this.gpuMemUsed.Location = new System.Drawing.Point(200, 392);
            this.gpuMemUsed.Name = "gpuMemUsed";
            this.gpuMemUsed.Size = new System.Drawing.Size(73, 13);
            this.gpuMemUsed.TabIndex = 37;
            this.gpuMemUsed.Text = "gpuMemUsed";
            // 
            // gpuFreeMem
            // 
            this.gpuFreeMem.AutoSize = true;
            this.gpuFreeMem.Location = new System.Drawing.Point(200, 368);
            this.gpuFreeMem.Name = "gpuFreeMem";
            this.gpuFreeMem.Size = new System.Drawing.Size(69, 13);
            this.gpuFreeMem.TabIndex = 36;
            this.gpuFreeMem.Text = "gpuFreeMem";
            // 
            // gpuTotalMem
            // 
            this.gpuTotalMem.AutoSize = true;
            this.gpuTotalMem.Location = new System.Drawing.Point(200, 344);
            this.gpuTotalMem.Name = "gpuTotalMem";
            this.gpuTotalMem.Size = new System.Drawing.Size(72, 13);
            this.gpuTotalMem.TabIndex = 35;
            this.gpuTotalMem.Text = "gpuTotalMem";
            // 
            // gpuCoreLoad
            // 
            this.gpuCoreLoad.AutoSize = true;
            this.gpuCoreLoad.Location = new System.Drawing.Point(200, 320);
            this.gpuCoreLoad.Name = "gpuCoreLoad";
            this.gpuCoreLoad.Size = new System.Drawing.Size(71, 13);
            this.gpuCoreLoad.TabIndex = 34;
            this.gpuCoreLoad.Text = "gpuCoreLoad";
            // 
            // gpuMemClock
            // 
            this.gpuMemClock.AutoSize = true;
            this.gpuMemClock.Location = new System.Drawing.Point(112, 376);
            this.gpuMemClock.Name = "gpuMemClock";
            this.gpuMemClock.Size = new System.Drawing.Size(75, 13);
            this.gpuMemClock.TabIndex = 33;
            this.gpuMemClock.Text = "gpuMemClock";
            // 
            // gpuCoreClock
            // 
            this.gpuCoreClock.AutoSize = true;
            this.gpuCoreClock.Location = new System.Drawing.Point(112, 352);
            this.gpuCoreClock.Name = "gpuCoreClock";
            this.gpuCoreClock.Size = new System.Drawing.Size(74, 13);
            this.gpuCoreClock.TabIndex = 32;
            this.gpuCoreClock.Text = "gpuCoreClock";
            // 
            // ramLoad
            // 
            this.ramLoad.AutoSize = true;
            this.ramLoad.Location = new System.Drawing.Point(208, 528);
            this.ramLoad.Name = "ramLoad";
            this.ramLoad.Size = new System.Drawing.Size(48, 13);
            this.ramLoad.TabIndex = 43;
            this.ramLoad.Text = "ramLoad";
            // 
            // ramUsed
            // 
            this.ramUsed.AutoSize = true;
            this.ramUsed.Location = new System.Drawing.Point(208, 504);
            this.ramUsed.Name = "ramUsed";
            this.ramUsed.Size = new System.Drawing.Size(49, 13);
            this.ramUsed.TabIndex = 42;
            this.ramUsed.Text = "ramUsed";
            // 
            // ramLabel
            // 
            this.ramLabel.AutoSize = true;
            this.ramLabel.Location = new System.Drawing.Point(168, 480);
            this.ramLabel.Name = "ramLabel";
            this.ramLabel.Size = new System.Drawing.Size(50, 13);
            this.ramLabel.TabIndex = 41;
            this.ramLabel.Text = "ramLabel";
            // 
            // ramAvailable
            // 
            this.ramAvailable.AutoSize = true;
            this.ramAvailable.Location = new System.Drawing.Point(120, 528);
            this.ramAvailable.Name = "ramAvailable";
            this.ramAvailable.Size = new System.Drawing.Size(67, 13);
            this.ramAvailable.TabIndex = 40;
            this.ramAvailable.Text = "ramAvailable";
            // 
            // ramTotal
            // 
            this.ramTotal.AutoSize = true;
            this.ramTotal.Location = new System.Drawing.Point(120, 504);
            this.ramTotal.Name = "ramTotal";
            this.ramTotal.Size = new System.Drawing.Size(48, 13);
            this.ramTotal.TabIndex = 39;
            this.ramTotal.Text = "ramTotal";
            // 
            // ssdTemp
            // 
            this.ssdTemp.AutoSize = true;
            this.ssdTemp.Location = new System.Drawing.Point(96, 640);
            this.ssdTemp.Name = "ssdTemp";
            this.ssdTemp.Size = new System.Drawing.Size(50, 13);
            this.ssdTemp.TabIndex = 50;
            this.ssdTemp.Text = "ssdTemp";
            // 
            // ssdUsedPercent
            // 
            this.ssdUsedPercent.AutoSize = true;
            this.ssdUsedPercent.Location = new System.Drawing.Point(184, 688);
            this.ssdUsedPercent.Name = "ssdUsedPercent";
            this.ssdUsedPercent.Size = new System.Drawing.Size(85, 13);
            this.ssdUsedPercent.TabIndex = 49;
            this.ssdUsedPercent.Text = "ssdUsedPercent";
            // 
            // ssdUsedGB
            // 
            this.ssdUsedGB.AutoSize = true;
            this.ssdUsedGB.Location = new System.Drawing.Point(184, 664);
            this.ssdUsedGB.Name = "ssdUsedGB";
            this.ssdUsedGB.Size = new System.Drawing.Size(63, 13);
            this.ssdUsedGB.TabIndex = 48;
            this.ssdUsedGB.Text = "ssdUsedGB";
            // 
            // ssdTotalGB
            // 
            this.ssdTotalGB.AutoSize = true;
            this.ssdTotalGB.Location = new System.Drawing.Point(184, 640);
            this.ssdTotalGB.Name = "ssdTotalGB";
            this.ssdTotalGB.Size = new System.Drawing.Size(62, 13);
            this.ssdTotalGB.TabIndex = 47;
            this.ssdTotalGB.Text = "ssdTotalGB";
            // 
            // ssdName
            // 
            this.ssdName.AutoSize = true;
            this.ssdName.Location = new System.Drawing.Point(96, 616);
            this.ssdName.Name = "ssdName";
            this.ssdName.Size = new System.Drawing.Size(51, 13);
            this.ssdName.TabIndex = 46;
            this.ssdName.Text = "ssdName";
            // 
            // ssdFreePercent
            // 
            this.ssdFreePercent.AutoSize = true;
            this.ssdFreePercent.Location = new System.Drawing.Point(96, 688);
            this.ssdFreePercent.Name = "ssdFreePercent";
            this.ssdFreePercent.Size = new System.Drawing.Size(81, 13);
            this.ssdFreePercent.TabIndex = 45;
            this.ssdFreePercent.Text = "ssdFreePercent";
            // 
            // ssdFreeGB
            // 
            this.ssdFreeGB.AutoSize = true;
            this.ssdFreeGB.Location = new System.Drawing.Point(96, 664);
            this.ssdFreeGB.Name = "ssdFreeGB";
            this.ssdFreeGB.Size = new System.Drawing.Size(59, 13);
            this.ssdFreeGB.TabIndex = 44;
            this.ssdFreeGB.Text = "ssdFreeGB";
            // 
            // cpuFan
            // 
            this.cpuFan.AutoSize = true;
            this.cpuFan.Location = new System.Drawing.Point(16, 192);
            this.cpuFan.Name = "cpuFan";
            this.cpuFan.Size = new System.Drawing.Size(43, 13);
            this.cpuFan.TabIndex = 51;
            this.cpuFan.Text = "cpuFan";
            // 
            // gpuFan
            // 
            this.gpuFan.AutoSize = true;
            this.gpuFan.Location = new System.Drawing.Point(112, 400);
            this.gpuFan.Name = "gpuFan";
            this.gpuFan.Size = new System.Drawing.Size(43, 13);
            this.gpuFan.TabIndex = 52;
            this.gpuFan.Text = "gpuFan";
            // 
            // hddTemp
            // 
            this.hddTemp.AutoSize = true;
            this.hddTemp.Location = new System.Drawing.Point(104, 784);
            this.hddTemp.Name = "hddTemp";
            this.hddTemp.Size = new System.Drawing.Size(52, 13);
            this.hddTemp.TabIndex = 59;
            this.hddTemp.Text = "hddTemp";
            // 
            // hddUsedPercent
            // 
            this.hddUsedPercent.AutoSize = true;
            this.hddUsedPercent.Location = new System.Drawing.Point(192, 832);
            this.hddUsedPercent.Name = "hddUsedPercent";
            this.hddUsedPercent.Size = new System.Drawing.Size(87, 13);
            this.hddUsedPercent.TabIndex = 58;
            this.hddUsedPercent.Text = "hddUsedPercent";
            // 
            // hddUsedGB
            // 
            this.hddUsedGB.AutoSize = true;
            this.hddUsedGB.Location = new System.Drawing.Point(192, 808);
            this.hddUsedGB.Name = "hddUsedGB";
            this.hddUsedGB.Size = new System.Drawing.Size(65, 13);
            this.hddUsedGB.TabIndex = 57;
            this.hddUsedGB.Text = "hddUsedGB";
            // 
            // hddTotalGB
            // 
            this.hddTotalGB.AutoSize = true;
            this.hddTotalGB.Location = new System.Drawing.Point(192, 784);
            this.hddTotalGB.Name = "hddTotalGB";
            this.hddTotalGB.Size = new System.Drawing.Size(64, 13);
            this.hddTotalGB.TabIndex = 56;
            this.hddTotalGB.Text = "hddTotalGB";
            // 
            // hddName
            // 
            this.hddName.AutoSize = true;
            this.hddName.Location = new System.Drawing.Point(104, 760);
            this.hddName.Name = "hddName";
            this.hddName.Size = new System.Drawing.Size(53, 13);
            this.hddName.TabIndex = 55;
            this.hddName.Text = "hddName";
            // 
            // hddFreePercent
            // 
            this.hddFreePercent.AutoSize = true;
            this.hddFreePercent.Location = new System.Drawing.Point(104, 832);
            this.hddFreePercent.Name = "hddFreePercent";
            this.hddFreePercent.Size = new System.Drawing.Size(83, 13);
            this.hddFreePercent.TabIndex = 54;
            this.hddFreePercent.Text = "hddFreePercent";
            // 
            // hddFreeGB
            // 
            this.hddFreeGB.AutoSize = true;
            this.hddFreeGB.Location = new System.Drawing.Point(104, 808);
            this.hddFreeGB.Name = "hddFreeGB";
            this.hddFreeGB.Size = new System.Drawing.Size(61, 13);
            this.hddFreeGB.TabIndex = 53;
            this.hddFreeGB.Text = "hddFreeGB";
            // 
            // wifiBytesSent
            // 
            this.wifiBytesSent.AutoSize = true;
            this.wifiBytesSent.Location = new System.Drawing.Point(104, 976);
            this.wifiBytesSent.Name = "wifiBytesSent";
            this.wifiBytesSent.Size = new System.Drawing.Size(70, 13);
            this.wifiBytesSent.TabIndex = 62;
            this.wifiBytesSent.Text = "wifiBytesSent";
            // 
            // wifiBytesRecv
            // 
            this.wifiBytesRecv.AutoSize = true;
            this.wifiBytesRecv.Location = new System.Drawing.Point(104, 952);
            this.wifiBytesRecv.Name = "wifiBytesRecv";
            this.wifiBytesRecv.Size = new System.Drawing.Size(74, 13);
            this.wifiBytesRecv.TabIndex = 61;
            this.wifiBytesRecv.Text = "wifiBytesRecv";
            // 
            // wifiLabel
            // 
            this.wifiLabel.AutoSize = true;
            this.wifiLabel.Location = new System.Drawing.Point(104, 928);
            this.wifiLabel.Name = "wifiLabel";
            this.wifiLabel.Size = new System.Drawing.Size(48, 13);
            this.wifiLabel.TabIndex = 60;
            this.wifiLabel.Text = "wifiLabel";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(284, 1011);
            this.Controls.Add(this.wifiBytesSent);
            this.Controls.Add(this.wifiBytesRecv);
            this.Controls.Add(this.wifiLabel);
            this.Controls.Add(this.hddTemp);
            this.Controls.Add(this.hddUsedPercent);
            this.Controls.Add(this.hddUsedGB);
            this.Controls.Add(this.hddTotalGB);
            this.Controls.Add(this.hddName);
            this.Controls.Add(this.hddFreePercent);
            this.Controls.Add(this.hddFreeGB);
            this.Controls.Add(this.gpuFan);
            this.Controls.Add(this.cpuFan);
            this.Controls.Add(this.ssdTemp);
            this.Controls.Add(this.ssdUsedPercent);
            this.Controls.Add(this.ssdUsedGB);
            this.Controls.Add(this.ssdTotalGB);
            this.Controls.Add(this.ssdName);
            this.Controls.Add(this.ssdFreePercent);
            this.Controls.Add(this.ssdFreeGB);
            this.Controls.Add(this.ramLoad);
            this.Controls.Add(this.ramUsed);
            this.Controls.Add(this.ramLabel);
            this.Controls.Add(this.ramAvailable);
            this.Controls.Add(this.ramTotal);
            this.Controls.Add(this.gpuTemp);
            this.Controls.Add(this.gpuMemUsed);
            this.Controls.Add(this.gpuFreeMem);
            this.Controls.Add(this.gpuTotalMem);
            this.Controls.Add(this.gpuCoreLoad);
            this.Controls.Add(this.gpuMemClock);
            this.Controls.Add(this.gpuCoreClock);
            this.Controls.Add(this.gpuName);
            this.Controls.Add(this.cpu6Clock);
            this.Controls.Add(this.cpu5Clock);
            this.Controls.Add(this.cpu4Clock);
            this.Controls.Add(this.cpu3Clock);
            this.Controls.Add(this.cpu2Clock);
            this.Controls.Add(this.cpu1Clock);
            this.Controls.Add(this.cpuPackagePwr);
            this.Controls.Add(this.cpuPackageTemp);
            this.Controls.Add(this.cpuTotalLoad);
            this.Controls.Add(this.cpuHeat);
            this.Controls.Add(this.cpuLoad);
            this.Controls.Add(this.cpu6Heat);
            this.Controls.Add(this.cpu5Heat);
            this.Controls.Add(this.cpu4Heat);
            this.Controls.Add(this.cpu3Heat);
            this.Controls.Add(this.cpu2Heat);
            this.Controls.Add(this.cpu1Heat);
            this.Controls.Add(this.cpu6Load);
            this.Controls.Add(this.cpu5Load);
            this.Controls.Add(this.cpu4Load);
            this.Controls.Add(this.cpu3Load);
            this.Controls.Add(this.cpu2Load);
            this.Controls.Add(this.cpu1Load);
            this.Controls.Add(this.cpuName);
            this.Controls.Add(this.wifiPictureBox);
            this.Controls.Add(this.hddPictureBox);
            this.Controls.Add(this.ssdPictureBox);
            this.Controls.Add(this.ramPictureBox);
            this.Controls.Add(this.gpuPictureBox);
            this.Controls.Add(this.cpuPictureBox);
            this.Controls.Add(this.richTextBox1);
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

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.PictureBox cpuPictureBox;
        private System.Windows.Forms.PictureBox gpuPictureBox;
        private System.Windows.Forms.PictureBox ramPictureBox;
        private System.Windows.Forms.PictureBox ssdPictureBox;
        private System.Windows.Forms.PictureBox hddPictureBox;
        private System.Windows.Forms.PictureBox wifiPictureBox;
        private System.Windows.Forms.Label cpuName;
        private System.Windows.Forms.Label cpu1Load;
        private System.Windows.Forms.Label cpu2Load;
        private System.Windows.Forms.Label cpu3Load;
        private System.Windows.Forms.Label cpu4Load;
        private System.Windows.Forms.Label cpu5Load;
        private System.Windows.Forms.Label cpu6Load;
        private System.Windows.Forms.Label cpu6Heat;
        private System.Windows.Forms.Label cpu5Heat;
        private System.Windows.Forms.Label cpu4Heat;
        private System.Windows.Forms.Label cpu3Heat;
        private System.Windows.Forms.Label cpu2Heat;
        private System.Windows.Forms.Label cpu1Heat;
        private System.Windows.Forms.Label cpuHeat;
        private System.Windows.Forms.Label cpuLoad;
        private System.Windows.Forms.Label cpuPackageTemp;
        private System.Windows.Forms.Label cpuTotalLoad;
        private System.Windows.Forms.Label cpuPackagePwr;
        private System.Windows.Forms.Label cpu6Clock;
        private System.Windows.Forms.Label cpu5Clock;
        private System.Windows.Forms.Label cpu4Clock;
        private System.Windows.Forms.Label cpu3Clock;
        private System.Windows.Forms.Label cpu2Clock;
        private System.Windows.Forms.Label cpu1Clock;
        private System.Windows.Forms.Label gpuName;
        private System.Windows.Forms.Label gpuTemp;
        private System.Windows.Forms.Label gpuMemUsed;
        private System.Windows.Forms.Label gpuFreeMem;
        private System.Windows.Forms.Label gpuTotalMem;
        private System.Windows.Forms.Label gpuCoreLoad;
        private System.Windows.Forms.Label gpuMemClock;
        private System.Windows.Forms.Label gpuCoreClock;
        private System.Windows.Forms.Label ramLoad;
        private System.Windows.Forms.Label ramUsed;
        private System.Windows.Forms.Label ramLabel;
        private System.Windows.Forms.Label ramAvailable;
        private System.Windows.Forms.Label ramTotal;
        private System.Windows.Forms.Label ssdTemp;
        private System.Windows.Forms.Label ssdUsedPercent;
        private System.Windows.Forms.Label ssdUsedGB;
        private System.Windows.Forms.Label ssdTotalGB;
        private System.Windows.Forms.Label ssdName;
        private System.Windows.Forms.Label ssdFreePercent;
        private System.Windows.Forms.Label ssdFreeGB;
        private System.Windows.Forms.Label cpuFan;
        private System.Windows.Forms.Label gpuFan;
        private System.Windows.Forms.Label hddTemp;
        private System.Windows.Forms.Label hddUsedPercent;
        private System.Windows.Forms.Label hddUsedGB;
        private System.Windows.Forms.Label hddTotalGB;
        private System.Windows.Forms.Label hddName;
        private System.Windows.Forms.Label hddFreePercent;
        private System.Windows.Forms.Label hddFreeGB;
        private System.Windows.Forms.Label wifiBytesSent;
        private System.Windows.Forms.Label wifiBytesRecv;
        private System.Windows.Forms.Label wifiLabel;
    }
}

