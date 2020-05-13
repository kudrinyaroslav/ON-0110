using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace testdoc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

    class ApplicationInfo
    {
        string CoreSpecification;
        string TestSpecification;
        string ToolVersion;
    }

    class TesterInfo
    {
        string Operator;
        string Organization;
        string Address;
    }

    class DeviceDiscoveryData
    {
        string EndPointAddress;
        string Type;
        string ServiceAddress;
        string Scopes;
        int MetadataVersion;
    }

    class DeviceInfo
    {
        string Manufacturer;
        string Model;
        string SerialNumber;
        string FirmwareVersion;
        string HardwareID;
    }

    class DeviceInfoFull
    {
        DeviceDiscoveryData ByDiscovery;
        DeviceInfo ByDeviceInfo;
        //bool DiscoveryValid;
        //bool InfoValid;
    }

    class DiscoveredDevices
    {
        DeviceInfoFull[] Discovered;
        DeviceInfoFull Current;
        int Active;
        string[] LocalAreas;
    }


    class DeviceCredientials
    {
        string UserName;
        string Password;
    }

    class DeviceTimeouts
    {
        int Reboot;
        int Message;
        int InterTests;
    }

    class ProductFeatures
    {
        bool IPv6;
        bool MPEG4;
        bool Audio;
        bool h264;
    }

    class DeviceEnvironment
    {
        DeviceCredientials Credientials;
        DeviceTimeouts Timeouts;
        ProductFeatures Features;
    }

    class TestOptions
    {
        string[] TestIds;
        bool ManualFirst;
    }

    class TestProfile
    {
        DeviceEnvironment Environment;
        TestOptions Options;
    }

    class StoredProfile
    {
        string Name;
        TestProfile Profile;
    }

    class TestLogRecord
    {
        string Name;
        string Purpose;
        string XMLOut;
        string XMLIn;
        Exception FaultDescription;
        string Notes;
        bool OK;
    }

    class TestLog
    {
        string TestId;
        TestLogRecord[] Records;
        bool Passed;
    }

    /*
     * general context is the main holder of all application data. 
     * 
     */
    class GeneralContext
    {
        ApplicationInfo AppInfo;
        DeviceInfo DevInfo;
        TesterInfo Tester;
        DeviceEnvironment Environment;
        TestLog[] LogRecent;
    }

    class GUI_Discovery
    {
        DiscoveredDevices Devices;
        string DeviceIP;
        Thread WorkingThread;
    }

    class GUI_Setup
    {
        ApplicationInfo AppInfo;
        DeviceInfo DevInfo;
        TesterInfo Tester;
        GUI_Discovery InfoGetter;
    }

    class GUI_Management
    {
        StoredProfile[] Profiles;
        int CurrentProfile;
        string CurrentName;
        DeviceEnvironment Environment;
    }

    class GUI_Test
    {
        DeviceEnvironment Environment;
        TestOptions Options;
        TestLog LocalLog;
        Thread WorkingThread;
    }

    class GUI_Report
    {
        TestLog[] LogAll;
        string FileName;
        bool Saved;
    }

    class GUI_DeviceManagemment
    {
        DeviceInfo DevInfo;
        Thread WorkingThread;
    }

    class GUI_DeviceMedia
    {
        Thread WorkingThread;
    }

    class GUI_DevicePTZ
    {
        Thread WorkingThread;
    }

    class GUI_Requests
    {

    }

}
