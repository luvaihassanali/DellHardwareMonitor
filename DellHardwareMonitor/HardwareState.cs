using System;
using System.Diagnostics;

using LibreHardwareMonitor.Hardware;

namespace DellHardwareMonitor
{
    public class HardwareState
    {
        private Computer computer;
        private IHardware cpu;
        private IHardware gpu;
        private IHardware ram;
        private IHardware hdd;
        private IHardware ssd;
        private IHardware ethernet;
        private IHardware wifi;

        private DriveState[] driveStates;
        private NetworkState[] networkStates;

        private string localhost;
        private string publicIpAddress;

        public HardwareState(string cpuName, string gpuName, string ssdName, string hddName)
        {
            computer = new Computer
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

            if(cpuName.Equals(String.Empty))
            {
                cpuName = "Intel Core i7-9750H";
                gpuName = "NVIDIA GeForce RTX 2060";
                ssdName = "KBG40ZNS512G NVMe KIOXIA 512GB";
                hddName = "WDC WD10SPCX-21KHST0";
            }

            
            foreach (IHardware hardware in computer.Hardware)
            {
                if(hardware.Name.Equals(cpuName))
                {
                    cpu = hardware;
                }

                if (hardware.Name.Equals(gpuName))
                {
                    gpu = hardware;
                }

                if (hardware.Name.Equals(ssdName))
                {
                    ssd = hardware;
                }

                if (hardware.Name.Equals(hddName))
                {
                    hdd = hardware;
                }

                if (hardware.Name.Equals("Generic Memory"))
                {
                    ram = hardware;
                }

                if (hardware.Name.Equals("Ethernet"))
                {
                    ethernet = hardware;
                }

                if (hardware.Name.Equals("Wi-Fi"))
                {
                    wifi = hardware;
                }

                #region switch 
                /*
                switch (hardware.Name)
                {
                    case cpuName: //Constant value is expected
                        cpu = hardware;
                        break;
                    case : 
                        gpu = hardware;
                        break;
                    case "Generic Memory": 
                        ram = hardware;
                        break;
                    case : 
                        hdd = hardware;
                        break;
                    case : 
                        ssd = hardware;
                        break;
                    case "Ethernet":
                        ethernet = hardware;
                        break;
                    case "Wi-Fi":
                        wifi = hardware;
                        break;
                    default:
                        //Local Area Connection, 1, and 10 network sensors available
                        break;
                }*/
                #endregion
            }

            string[] allDriveInstances = new PerformanceCounterCategory("LogicalDisk").GetInstanceNames();
            string[] driveInstances = new string[2];
            int index = 0; 
            foreach(string instance in allDriveInstances)
            {
                if(instance.Equals("C:") || instance.Equals("D:"))
                {
                    driveInstances[index] = instance;
                    index++;
                }
            }

            driveStates = new DriveState[driveInstances.Length];

            for (int i = 0; i < driveStates.Length; i++)
            {
                driveStates[i] = new DriveState(driveInstances[i]);
                driveStates[i].Counters[0] = new PerformanceCounter("LogicalDisk", "Free Megabytes", driveInstances[i]);
                driveStates[i].Counters[1] = new PerformanceCounter("LogicalDisk", "% Free Space", driveInstances[i]);
                driveStates[i].Counters[2] = new PerformanceCounter("LogicalDisk", "Disk Read Bytes/sec", driveInstances[i]);
                driveStates[i].Counters[3] = new PerformanceCounter("LogicalDisk", "Disk Write Bytes/sec", driveInstances[i]);
            }

            string[] networkInstances = new PerformanceCounterCategory("Network Interface").GetInstanceNames();
            networkStates = new NetworkState[networkInstances.Length];

            for (int i = 0; i < networkInstances.Length; i++)
            {
                networkStates[i] = new NetworkState(networkInstances[i]);
                networkStates[i].Counters[0] = new PerformanceCounter("Network Interface", "Bytes Received/sec", networkInstances[i]);
                networkStates[i].Counters[1] = new PerformanceCounter("Network Interface", "Bytes Sent/sec", networkInstances[i]);
            }
        }

        public Computer Computer
        {
            get => computer;
            set => computer = value;
        }

        public IHardware CPU
        {
            get => cpu;
            set => cpu = value;
        }

        public IHardware GPU
        {
            get => gpu;
            set => gpu = value;
        }

        public IHardware RAM
        {
            get => ram;
            set => ram = value;
        }

        public IHardware SSD
        {
            get => ssd;
            set => ssd = value;
        }

        public IHardware HDD
        {
            get => hdd;
            set => ssd = value;
        }

        public IHardware Ethernet
        {
            get => ethernet;
            set => ethernet = value;
        }

        public IHardware WiFi
        {
            get => wifi;
            set => wifi = value;
        }

        public DriveState[] DriveStates
        {
            get => driveStates;
            set => driveStates = value;
        }

        public NetworkState[] NetworkStates
        {
            get => networkStates;
            set => networkStates = value;
        }

        public string LocalHost
        {
            get => localhost;
            set => localhost = value;
        }

        public string PublicIpAddress
        {
            get => publicIpAddress;
            set => publicIpAddress = value;
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

    public class DriveState
    {
        private string interfaceName;
        private PerformanceCounter[] counters;

        public DriveState(string name)
        {
            interfaceName = name;
            counters = new PerformanceCounter[4];
        }

        public string Name
        {
            get => interfaceName;
            set => interfaceName = value;
        }

        public PerformanceCounter[] Counters
        {
            get => counters;
            set => counters = value;
        }
    }

    public class NetworkState
    {
        private string interfaceName;
        private PerformanceCounter[] counters;

        public NetworkState(string name)
        {
            interfaceName = name;
            counters = new PerformanceCounter[2];
        }        

        public string Name
        {
            get => interfaceName;
            set => interfaceName = value;
        }

        public PerformanceCounter[] Counters
        {
            get => counters;
            set => counters = value;
        }
    }
}
