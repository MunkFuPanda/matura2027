using System;
using System.Diagnostics;
using System.Management;
using System.Collections.Generic;

namespace SystemMonitor {
    /// <summary>
    /// Summary description for SystemData.
    /// </summary>
    public class SystemData {
        #region "Constructor"
        public SystemData() {
            PerformanceCounterCategory cat = new PerformanceCounterCategory("Network Interface");
            _instanceNames = cat.GetInstanceNames();

            _netRecvCounters = new PerformanceCounter[_instanceNames.Length];
            for (int i = 0; i < _instanceNames.Length; i++)
                _netRecvCounters[i] = new PerformanceCounter();

            _netSentCounters = new PerformanceCounter[_instanceNames.Length];
            for (int i = 0; i < _instanceNames.Length; i++)
                _netSentCounters[i] = new PerformanceCounter();

            _compactFormat = false;
        }
        #endregion

        #region "Properties"
        public bool CompactFormat {
            get { return _compactFormat; }
            set { _compactFormat = value; }
        }
        #endregion


        #region "Public Methods"
        public double GetProcessorPercent() {
            return GetCounterValue(_cpuCounter, "Processor", "% Processor Time", "_Total");
        }

        public double GetVirtualMemoryPercent() {
            return GetCounterValue(_memoryCounter, "Memory", "% Committed Bytes In Use", null);
        }

        public double GetVirtualMemoryCurrent() {
            return GetCounterValue(_memoryCounter, "Memory", "Committed Bytes", null);
        }

        public double GetVirtualMemoryMaximum() {
            return GetCounterValue(_memoryCounter, "Memory", "Commit Limit", null);
        }

        public double GetPhysicalMemoryPercent() {
            return GetPhysicalMemoryCurrent() * 100 / GetPhysicalMemoryMaximum();
        }

        public double GetPhysicalMemoryCurrent() {
            double d = GetCounterValue(_memoryCounter, "Memory", "Available Bytes", null);
            return GetPhysicalMemoryMaximum() - d;
        }

        public double GetPhysicalMemoryMaximum() {
            string s = QueryComputerSystem("totalphysicalmemory");
            return Convert.ToDouble(s);
        }


        public enum DiskData { ReadAndWrite, Read, Write };

        public double GetDiskData(DiskData dd) {
            return dd == DiskData.Read ?
                        GetCounterValue(_diskReadCounter, "PhysicalDisk", "Disk Read Bytes/sec", "_Total") :
                    dd == DiskData.Write ?
                        GetCounterValue(_diskWriteCounter, "PhysicalDisk", "Disk Write Bytes/sec", "_Total") :
                    dd == DiskData.ReadAndWrite ?
                        GetCounterValue(_diskReadCounter, "PhysicalDisk", "Disk Read Bytes/sec", "_Total") +
                        GetCounterValue(_diskWriteCounter, "PhysicalDisk", "Disk Write Bytes/sec", "_Total") :
                    0;
        }

        public enum NetData { ReceivedAndSent, Received, Sent };

        public double GetNetData(NetData nd) {
            if (_instanceNames.Length == 0)
                return 0;

            double d = 0;
            for (int i = 0; i < _instanceNames.Length; i++) {
                d += nd == NetData.Received ?
                        GetCounterValue(_netRecvCounters[i], "Network Interface", "Bytes Received/sec", _instanceNames[i]) :
                    nd == NetData.Sent ?
                        GetCounterValue(_netSentCounters[i], "Network Interface", "Bytes Sent/sec", _instanceNames[i]) :
                    nd == NetData.ReceivedAndSent ?
                        GetCounterValue(_netRecvCounters[i], "Network Interface", "Bytes Received/sec", _instanceNames[i]) +
                        GetCounterValue(_netSentCounters[i], "Network Interface", "Bytes Sent/sec", _instanceNames[i]) :
                    0;
            }

            return d;
        }

        enum Unit { B, KB, MB, GB, ER }
        public string FormatBytes(double bytes) {
            int unit = 0;
            while (bytes > 1024) {
                bytes /= 1024;
                ++unit;
            }

            string s = _compactFormat ? ((int)bytes).ToString() : bytes.ToString("F") + " ";
            return s + ((Unit)unit).ToString();
        }

        public string QueryComputerSystem(string type) {
            string str = null;
            ManagementObjectSearcher objCS = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            foreach (ManagementObject objMgmt in objCS.Get()) {
                str = objMgmt[type].ToString();
            }
            return str;
        }

        public string QueryEnvironment(string type) {
            return Environment.ExpandEnvironmentVariables(type);
        }

        public Dictionary<String, double> LogicalDiskFree() {
            Dictionary<String, double> dict = new Dictionary<String, double>();
            object device, space;
            ManagementObjectSearcher objCS = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");
            foreach (ManagementObject objMgmt in objCS.Get()) {
                device = objMgmt["DeviceID"];		// C:
                if (null != device) {
                    space = objMgmt["FreeSpace"];	// C:10.32 GB, D:5.87GB
                    if (null != space)
                        dict.Add(device.ToString(), double.Parse(space.ToString()));
                }
            }

            return dict;
        }

        public Dictionary<String, double> LogicalDiskSize() {
            Dictionary<String, double> dict = new Dictionary<String, double>();
            object device, space;
            ManagementObjectSearcher objCS = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");
            foreach (ManagementObject objMgmt in objCS.Get()) {
                device = objMgmt["DeviceID"];		// C:
                if (null != device) {
                    space = objMgmt["Size"];	// C:10.32 GB, D:5.87GB
                    if (null != space)
                        dict.Add(device.ToString(), double.Parse(space.ToString()));
                }
            }

            return dict;
        }

        #endregion

        #region "Private Helpers"
        double GetCounterValue(PerformanceCounter pc, string categoryName, string counterName, string instanceName) {
            pc.CategoryName = categoryName;
            pc.CounterName = counterName;
            pc.InstanceName = instanceName;
            return pc.NextValue();
        }

        #endregion

        #region "Members"
        bool _compactFormat;

        PerformanceCounter _memoryCounter = new PerformanceCounter();
        PerformanceCounter _cpuCounter = new PerformanceCounter();
        PerformanceCounter _diskReadCounter = new PerformanceCounter();
        PerformanceCounter _diskWriteCounter = new PerformanceCounter();

        string[] _instanceNames;
        PerformanceCounter[] _netRecvCounters;
        PerformanceCounter[] _netSentCounters;

        #endregion
    }

    public delegate void OnLogicalDiskProc(string s);
}
