using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SMC.Proxies;

namespace SMC.Pages
{
    public partial class SensorControlPage : BaseSensorClientControl
    {
        public SensorControlPage()
        {
            InitializeComponent();
        }


        List<Sensor> _sensors = new List<Sensor>();

        void InitSensors()
        {
            SafeInvoke(()=>
                           {
                               tvSensors.Nodes.Clear();

                               //DoorInfo[] doorInfos = DoorControlClient.GetDoorInfo(null);
                               List<DoorInfo> doorInfos = GetList<DoorInfo>(DoorControlClient.GetDoorInfoList);
                                  
                               foreach (DoorInfo info in doorInfos)
                               {
                                   TreeNode doorNode = new TreeNode("Door: " + info.token);
                                   tvSensors.Nodes.Add(doorNode);

                                   // OK
                                   Sensor doorAlarm = new Sensor(info.token, "Alarm", SensorDeviceType.Door);
                                   doorAlarm.Values.AddRange(new string[] { "Normal", "DoorForcedOpen", "DoorOpenTooLong" });
                                   
                                   AddSensorNode(doorNode, doorAlarm);

                                   // OK
                                   Sensor doorMonitor = new Sensor(info.token, "DoorPhysicalState", SensorDeviceType.Door);
                                   doorMonitor.Values.AddRange(new string[] { "Unknown", "Open", "Closed", "Fault" });

                                   AddSensorNode(doorNode, doorMonitor);

                                   // OK
                                   Sensor doorLockMonitor = new Sensor(info.token, "LockPhysicalState", SensorDeviceType.Door);
                                   doorLockMonitor.Values.AddRange(new string[] {"Unknown", "Locked", "Unlocked", "Fault" });
                                   
                                   AddSensorNode(doorNode, doorLockMonitor);

                                   // OK
                                   Sensor doorDoubleLockMonitor = new Sensor(info.token, "DoubleLockPhysicalState", SensorDeviceType.Door);
                                   doorDoubleLockMonitor.Values.AddRange(new string[] { "Unknown", "Locked", "Unlocked", "Fault" });

                                   AddSensorNode(doorNode, doorDoubleLockMonitor);


                                   // OK
                                   Sensor doorTamper = new Sensor(info.token, "Tamper", SensorDeviceType.Door);
                                   doorTamper.Values.AddRange(new string[] { "Unknown", "NotInTamper", "TamperDetected" });

                                   AddSensorNode(doorNode, doorTamper);

                                   // OK
                                   Sensor doorFault = new Sensor(info.token, "Fault", SensorDeviceType.Door);
                                   doorFault.Values.AddRange(new string[] { "Unknown", "NotInFault", "FaultDetected" });

                                   AddSensorNode(doorNode, doorFault);

                               }
                               
                               //CredentialInfo[] credentialInfos = CredentialClient.GetCredentialInfo(null);
                               List<Credential> credentialInfos = GetList<Credential>(CredentialClient.GetCredentialList);

                               foreach (CredentialInfo info in credentialInfos)
                               {
                                   TreeNode credentialNode = new TreeNode("Credential: " + info.token);
                                   tvSensors.Nodes.Add(credentialNode);

                                   // OK
                                   Sensor antipassbackViolated = new Sensor(info.token, "AntipassbackViolated", SensorDeviceType.Credential);
                                   antipassbackViolated.Values.AddRange(new string[] { "True", "False" });

                                   AddSensorNode(credentialNode, antipassbackViolated);

                                  

                               }
                           });


        }


        void AddSensorNode(TreeNode entityNode, Sensor sensor)
        {
            TreeNode sensorNode = entityNode.Nodes.Add(sensor.Name, sensor.Name);
            sensorNode.Tag = sensor;
 
            foreach (string value in sensor.Values)
            {
                sensorNode.Nodes.Add(value);
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            InitSensors();
        }

        private void tvSensors_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = tvSensors.SelectedNode;
            bool enableSet = false;

            if (node != null)
            {
                if (node.Level == 2)
                {
                    enableSet = true;
                }
            }


            btnSet.Enabled = enableSet;
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            TreeNode node = tvSensors.SelectedNode;

            if (node != null)
            {
                if (node.Level == 2)
                {
                    TreeNode sensorNode = node.Parent;
                    Sensor sensor = sensorNode.Tag as Sensor;
                    string deviceType = "";

                    if (sensor.SensorDeviceType == SensorDeviceType.Door)
                    {
                        deviceType = "Door";
                    }

                    if (sensor.SensorDeviceType == SensorDeviceType.Credential)
                    {
                        deviceType = "Credential";
                    }

                    SafeInvoke( () =>
                                    {
                                        SensorServiceClient.SignalReceived(sensor.DeviceToken, deviceType, sensor.Name, node.Text);
                                    });
                }
            }
        }


    }
}
