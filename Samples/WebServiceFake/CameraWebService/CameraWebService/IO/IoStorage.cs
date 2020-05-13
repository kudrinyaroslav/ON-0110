using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeviceIo;
namespace CameraWebService
{
    public class IoStorage
    {
        private static IoStorage _instance;
        public static IoStorage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new IoStorage();
                }
                return _instance;
            }
        }

        private VideoSource[] _sources;
        public VideoSource[] Sources
        {
            get
            {
                if (_sources == null)
                {
                    List<VideoSource> lst = new List<VideoSource>();

                    for (int i = 1; i < 5; i++)
                    {

                        VideoSource source = new VideoSource();

                        source.token = "source" + i;
                        source.Resolution = new VideoResolution();
                        source.Resolution.Height = 480;
                        source.Resolution.Width = 640;

                        source.Imaging = new ImagingSettings();
                        source.Imaging.BacklightCompensation = new BacklightCompensation();
                        source.Imaging.BacklightCompensation.Mode = BacklightCompensationMode.OFF;

                        lst.Add(source);

                    }
                    _sources = lst.ToArray();
                }

                return _sources;
            }
        }

        private List<DeviceIo.RelayOutput> _relayOutputs;
        public List<DeviceIo.RelayOutput> RelayOutputs
        {
            get
            {
                if (_relayOutputs == null)
                {
                    _relayOutputs = new List<DeviceIo.RelayOutput>();

                    DeviceIo.RelayOutput ro1 = new DeviceIo.RelayOutput();
                    ro1.token = "output1";
                    ro1.Properties = new DeviceIo.RelayOutputSettings();
                    ro1.Properties.DelayTime = "PT10S";
                    ro1.Properties.IdleState = DeviceIo.RelayIdleState.open;
                    ro1.Properties.Mode = DeviceIo.RelayMode.Bistable;
                    _relayOutputs.Add(ro1);


                    DeviceIo.RelayOutput ro2 = new DeviceIo.RelayOutput();
                    ro2.token = "output2";
                    ro2.Properties = new DeviceIo.RelayOutputSettings();
                    ro2.Properties.DelayTime = "PT40S";
                    ro2.Properties.IdleState = DeviceIo.RelayIdleState.open;
                    ro2.Properties.Mode = DeviceIo.RelayMode.Monostable;
                    _relayOutputs.Add(ro2);


                    DeviceIo.RelayOutput ro3 = new DeviceIo.RelayOutput();
                    ro3.token = "output3";
                    ro3.Properties = new DeviceIo.RelayOutputSettings();
                    ro3.Properties.DelayTime = "PT25S";
                    ro3.Properties.IdleState = DeviceIo.RelayIdleState.closed;
                    ro3.Properties.Mode = DeviceIo.RelayMode.Monostable;
                    _relayOutputs.Add(ro3);
                }

                return _relayOutputs;
            }

        }

    }
}
